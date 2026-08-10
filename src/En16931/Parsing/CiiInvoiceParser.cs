using System.Xml.Linq;
using En16931.Model;
using static En16931.Parsing.Namespaces;

namespace En16931.Parsing;

/// <summary>
/// Reads a UN/CEFACT Cross Industry Invoice — the syntax ZUGFeRD and Factur-X
/// embed into a PDF/A-3 — into the EN 16931 model.
/// </summary>
public sealed partial class CiiInvoiceParser : IInvoiceParser
{
    /// <inheritdoc/>
    public InvoiceSyntax Syntax => InvoiceSyntax.Cii;

    /// <inheritdoc/>
    public bool CanParse(XDocument document) =>
        document.Root?.Name == Rsm + "CrossIndustryInvoice";

    /// <inheritdoc/>
    public Invoice Parse(XDocument document)
    {
        ArgumentNullException.ThrowIfNull(document);
        var root = document.Root
            ?? throw new InvoiceFormatException("The document has no root element.");
        if (root.Name != Rsm + "CrossIndustryInvoice")
            throw new InvoiceFormatException(
                $"Expected rsm:CrossIndustryInvoice as root element but found '{root.Name}'.");

        var invoice = new Invoice { Syntax = InvoiceSyntax.Cii };

        ReadContext(root.El(Rsm + "ExchangedDocumentContext"), invoice);
        ReadDocument(root.El(Rsm + "ExchangedDocument"), invoice);

        var transaction = root.El(Rsm + "SupplyChainTradeTransaction");
        ReadAgreement(transaction.El(Ram + "ApplicableHeaderTradeAgreement"), invoice);
        ReadDelivery(transaction.El(Ram + "ApplicableHeaderTradeDelivery"), invoice);
        ReadSettlement(transaction.El(Ram + "ApplicableHeaderTradeSettlement"), invoice);

        foreach (var line in transaction.Els(Ram + "IncludedSupplyChainTradeLineItem"))
            invoice.Lines.Add(ReadLine(line));

        return invoice;
    }

    private static void ReadContext(XElement? context, Invoice invoice)
    {
        invoice.BusinessProcessType = context
            .Path(Ram + "BusinessProcessSpecifiedDocumentContextParameter", Ram + "ID").Text();
        invoice.SpecificationIdentifier = context
            .Path(Ram + "GuidelineSpecifiedDocumentContextParameter", Ram + "ID").Text();
    }

    private static void ReadDocument(XElement? document, Invoice invoice)
    {
        invoice.InvoiceNumber = document.El(Ram + "ID").Text();
        invoice.InvoiceTypeCode = document.El(Ram + "TypeCode").Text();
        invoice.IssueDate = document.Path(Ram + "IssueDateTime", Udt + "DateTimeString").CiiDate();

        ReadDocumentExtensions(document, invoice);

        foreach (var note in document.Els(Ram + "IncludedNote"))
        {
            invoice.Notes.Add(new InvoiceNote
            {
                Text = note.El(Ram + "Content").Text(),
                SubjectCode = note.El(Ram + "SubjectCode").Text(),
                Extensions = ReadNoteExtensions(note)
            });
        }
    }

    private static void ReadAgreement(XElement? agreement, Invoice invoice)
    {
        invoice.BuyerReference = agreement.El(Ram + "BuyerReference").Text();
        invoice.Seller = ReadParty(agreement.El(Ram + "SellerTradeParty"));
        invoice.Buyer = ReadParty(agreement.El(Ram + "BuyerTradeParty"));
        invoice.SellerTaxRepresentative = ReadParty(agreement.El(Ram + "SellerTaxRepresentativeTradeParty"));

        invoice.PurchaseOrderReference =
            agreement.Path(Ram + "BuyerOrderReferencedDocument", Ram + "IssuerAssignedID").Text();
        invoice.SalesOrderReference =
            agreement.Path(Ram + "SellerOrderReferencedDocument", Ram + "IssuerAssignedID").Text();
        invoice.ContractReference =
            agreement.Path(Ram + "ContractReferencedDocument", Ram + "IssuerAssignedID").Text();
        invoice.ProjectReference =
            agreement.Path(Ram + "SpecifiedProcuringProject", Ram + "ID").Text();

        ReadAgreementExtensions(agreement, invoice);

        // CII puts three business terms into the same element and separates them
        // by TypeCode: 130 invoiced object, 50 tender or lot, 916 supporting document.
        foreach (var reference in agreement.Els(Ram + "AdditionalReferencedDocument"))
        {
            /* The group as the document gives it, before the split by the type code. */
            var asGiven = ReadReferencedDocument(reference);
            if (asGiven is not null) Ext(invoice).AdditionalDocuments.Add(asGiven);

            var id = reference.El(Ram + "IssuerAssignedID");
            switch (reference.El(Ram + "TypeCode").Text())
            {
                case "130":
                    /*
                     * BT-18 gives one object identifier, but a document of the EXTENDED
                     * profile can name more than one. The core keeps the first, and the
                     * extension keeps each one.
                     */
                    var objectIdentifier = ReadIdentifier(id);
                    if (objectIdentifier is not null)
                    {
                        invoice.InvoicedObjectIdentifier ??= objectIdentifier;
                        Ext(invoice).InvoicedObjectIdentifiers.Add(objectIdentifier);
                    }
                    break;
                case "50":
                    invoice.TenderOrLotReference = id.Text();
                    break;
                default:
                    invoice.SupportingDocuments.Add(ReadSupportingDocument(reference));
                    break;
            }
        }
    }

    private static SupportingDocument ReadSupportingDocument(XElement reference)
    {
        var binary = reference.El(Ram + "AttachmentBinaryObject");
        return new SupportingDocument
        {
            Reference = reference.El(Ram + "IssuerAssignedID").Text(),
            Description = reference.El(Ram + "Name").Text(),
            ExternalLocation = reference.El(Ram + "URIID").Text(),
            Extensions = ReadSupportingDocumentExtensions(reference),
            Attachment = binary is null ? null : new Attachment
            {
                Content = TryDecodeBase64(binary.Text()),
                MimeCode = binary.Attr("mimeCode"),
                FileName = binary.Attr("filename")
            }
        };
    }

    private static void ReadDelivery(XElement? delivery, Invoice invoice)
    {
        invoice.DespatchAdviceReference =
            delivery.Path(Ram + "DespatchAdviceReferencedDocument", Ram + "IssuerAssignedID").Text();
        invoice.ReceivingAdviceReference =
            delivery.Path(Ram + "ReceivingAdviceReferencedDocument", Ram + "IssuerAssignedID").Text();

        ReadDeliveryExtensions(delivery, invoice);

        var shipTo = delivery.El(Ram + "ShipToTradeParty");
        var date = delivery
            .Path(Ram + "ActualDeliverySupplyChainEvent", Ram + "OccurrenceDateTime", Udt + "DateTimeString")
            .CiiDate();

        if (shipTo is null && date is null) return;

        invoice.Delivery = new DeliveryInformation
        {
            PartyName = shipTo.El(Ram + "Name").Text(),
            LocationIdentifier = ReadIdentifier(shipTo.El(Ram + "ID")),
            ActualDeliveryDate = date,
            Address = ReadAddress(shipTo.El(Ram + "PostalTradeAddress"))
        };
    }

    private static void ReadSettlement(XElement? settlement, Invoice invoice)
    {
        invoice.CurrencyCode = settlement.El(Ram + "InvoiceCurrencyCode").Text();
        invoice.TaxCurrencyCode = settlement.El(Ram + "TaxCurrencyCode").Text();
        invoice.BuyerAccountingReference =
            settlement.Path(Ram + "ReceivableSpecifiedTradeAccountingAccount", Ram + "ID").Text();
        invoice.Payee = ReadParty(settlement.El(Ram + "PayeeTradeParty"));

        // BT-90 sits on the settlement in CII but belongs to the seller semantically.
        var creditorReference = settlement.El(Ram + "CreditorReferenceID").Text();
        if (creditorReference is not null && invoice.Seller is not null)
            invoice.Seller.BankAssignedCreditorIdentifier = creditorReference;

        ReadSettlementExtensions(settlement, invoice);

        var terms = settlement.El(Ram + "SpecifiedTradePaymentTerms");
        invoice.PaymentTerms = terms.El(Ram + "Description").Text();
        invoice.PaymentDueDate = terms.Path(Ram + "DueDateDateTime", Udt + "DateTimeString").CiiDate();

        ReadPaymentInstructions(settlement, terms, invoice);

        var period = settlement.El(Ram + "BillingSpecifiedPeriod");
        if (period is not null)
        {
            invoice.InvoicingPeriod = new Period
            {
                StartDate = period.Path(Ram + "StartDateTime", Udt + "DateTimeString").CiiDate(),
                EndDate = period.Path(Ram + "EndDateTime", Udt + "DateTimeString").CiiDate()
            };
        }

        foreach (var tax in settlement.Els(Ram + "ApplicableTradeTax"))
        {
            invoice.VatBreakdown.Add(new VatBreakdown
            {
                TaxAmount = tax.El(Ram + "CalculatedAmount").Dec(),
                TaxableAmount = tax.El(Ram + "BasisAmount").Dec(),
                CategoryCode = tax.El(Ram + "CategoryCode").Text(),
                CategoryRate = tax.El(Ram + "RateApplicablePercent").Dec(),
                ExemptionReasonText = tax.El(Ram + "ExemptionReason").Text(),
                ExemptionReasonCode = tax.El(Ram + "ExemptionReasonCode").Text(),
                Detail = ReadTaxDetail(tax)
            });

            // BT-7 and BT-8 are document level terms that CII carries on the tax entry.
            invoice.TaxPointDate ??= tax.Path(Ram + "TaxPointDate", Udt + "DateString").CiiDate();
            invoice.TaxPointDateCode ??= tax.El(Ram + "DueDateTypeCode").Text();
        }

        foreach (var allowanceCharge in settlement.Els(Ram + "SpecifiedTradeAllowanceCharge"))
        {
            var parsed = ReadAllowanceCharge(allowanceCharge, documentLevel: true);
            (parsed.IsCharge ? invoice.Charges : invoice.Allowances).Add(parsed);
        }

        foreach (var reference in settlement.Els(Ram + "InvoiceReferencedDocument"))
        {
            invoice.PrecedingInvoiceReferences.Add(new PrecedingInvoiceReference
            {
                InvoiceNumber = reference.El(Ram + "IssuerAssignedID").Text(),
                IssueDate = reference
                    .Path(Ram + "FormattedIssueDateTime", Qdt + "DateTimeString").CiiDate()
                    ?? reference.Path(Ram + "FormattedIssueDateTime", Udt + "DateTimeString").CiiDate(),
                Extensions = ReadPrecedingInvoiceExtensions(reference)
            });
        }

        ReadTotals(settlement.El(Ram + "SpecifiedTradeSettlementHeaderMonetarySummation"), invoice);
    }

    private static void ReadPaymentInstructions(XElement? settlement, XElement? terms, Invoice invoice)
    {
        var means = settlement.Els(Ram + "SpecifiedTradeSettlementPaymentMeans").ToList();
        var remittance = settlement.El(Ram + "PaymentReference").Text();
        var mandate = terms.El(Ram + "DirectDebitMandateID").Text();
        if (means.Count == 0 && remittance is null && mandate is null) return;

        var instructions = new PaymentInstructions
        {
            PaymentMeansTypeCode = means.FirstOrDefault().El(Ram + "TypeCode").Text(),
            PaymentMeansText = means.FirstOrDefault().El(Ram + "Information").Text(),
            RemittanceInformation = remittance
        };

        foreach (var mean in means)
        {
            var account = mean.El(Ram + "PayeePartyCreditorFinancialAccount");
            var institution = mean.El(Ram + "PayeeSpecifiedCreditorFinancialInstitution");
            if (account is not null || institution is not null)
            {
                instructions.CreditTransfers.Add(new CreditTransfer
                {
                    AccountIdentifier = account.El(Ram + "IBANID").Text()
                                        ?? account.El(Ram + "ProprietaryID").Text(),
                    AccountName = account.El(Ram + "AccountName").Text(),
                    ServiceProviderIdentifier = institution.El(Ram + "BICID").Text()
                });
            }

            var card = mean.El(Ram + "ApplicableTradeSettlementFinancialCard");
            if (card is not null)
            {
                instructions.PaymentCard ??= new PaymentCard
                {
                    PrimaryAccountNumber = card.El(Ram + "ID").Text(),
                    HolderName = card.El(Ram + "CardholderName").Text()
                };
            }

            var debited = mean.Path(Ram + "PayerPartyDebtorFinancialAccount", Ram + "IBANID").Text();
            if (debited is not null || mandate is not null)
            {
                instructions.DirectDebit ??= new DirectDebit
                {
                    DebitedAccountIdentifier = debited,
                    MandateReference = mandate
                };
            }
        }

        if (instructions.DirectDebit is null && mandate is not null)
            instructions.DirectDebit = new DirectDebit { MandateReference = mandate };

        instructions.Extensions = ReadPaymentExtensions(means);

        invoice.PaymentInstructions = instructions;
    }

    private static void ReadTotals(XElement? summation, Invoice invoice)
    {
        if (summation is null) return;

        var totals = invoice.Totals;
        totals.LineTotalAmount = summation.El(Ram + "LineTotalAmount").Dec();
        totals.AllowanceTotalAmount = summation.El(Ram + "AllowanceTotalAmount").Dec();
        totals.ChargeTotalAmount = summation.El(Ram + "ChargeTotalAmount").Dec();
        totals.TaxExclusiveAmount = summation.El(Ram + "TaxBasisTotalAmount").Dec();
        totals.TaxInclusiveAmount = summation.El(Ram + "GrandTotalAmount").Dec();
        totals.PaidAmount = summation.El(Ram + "TotalPrepaidAmount").Dec();
        totals.RoundingAmount = summation.El(Ram + "RoundingAmount").Dec();
        totals.DuePayableAmount = summation.El(Ram + "DuePayableAmount").Dec();

        // BT-110 and BT-111 share one element and differ by currency.
        foreach (var tax in summation.Els(Ram + "TaxTotalAmount"))
        {
            var value = tax.Dec();
            if (value is null) continue;
            var currency = tax.Attr("currencyID");
            var amount = new Amount(value.Value, currency);

            if (currency is not null && invoice.TaxCurrencyCode is not null
                                     && currency == invoice.TaxCurrencyCode
                                     && currency != invoice.CurrencyCode)
                totals.TaxAmountInAccountingCurrency ??= amount;
            else
                totals.TaxAmount ??= amount;
        }
    }

    private static InvoiceLine ReadLine(XElement line)
    {
        var document = line.El(Ram + "AssociatedDocumentLineDocument");
        var agreement = line.El(Ram + "SpecifiedLineTradeAgreement");
        var delivery = line.El(Ram + "SpecifiedLineTradeDelivery");
        var settlement = line.El(Ram + "SpecifiedLineTradeSettlement");

        var result = new InvoiceLine
        {
            Id = document.El(Ram + "LineID").Text(),
            Note = document.Path(Ram + "IncludedNote", Ram + "Content").Text(),
            OrderLineReference = agreement.Path(Ram + "BuyerOrderReferencedDocument", Ram + "LineID").Text(),
            InvoicedQuantity = ReadQuantity(delivery.El(Ram + "BilledQuantity")),
            NetAmount = settlement
                .Path(Ram + "SpecifiedTradeSettlementLineMonetarySummation", Ram + "LineTotalAmount").Dec(),
            ObjectIdentifier = ReadIdentifier(
                settlement.Path(Ram + "AdditionalReferencedDocument", Ram + "IssuerAssignedID")),
            BuyerAccountingReference = settlement
                .Path(Ram + "ReceivableSpecifiedTradeAccountingAccount", Ram + "ID").Text(),
            Item = ReadItem(line.El(Ram + "SpecifiedTradeProduct")),
            Price = ReadPrice(agreement),
            Extensions = ReadLineExtensions(line),
            Vat = new LineVatInformation
            {
                CategoryCode = settlement.Path(Ram + "ApplicableTradeTax", Ram + "CategoryCode").Text(),
                Rate = settlement.Path(Ram + "ApplicableTradeTax", Ram + "RateApplicablePercent").Dec()
            }
        };

        var period = settlement.El(Ram + "BillingSpecifiedPeriod");
        if (period is not null)
        {
            result.Period = new Period
            {
                StartDate = period.Path(Ram + "StartDateTime", Udt + "DateTimeString").CiiDate(),
                EndDate = period.Path(Ram + "EndDateTime", Udt + "DateTimeString").CiiDate()
            };
        }

        foreach (var allowanceCharge in settlement.Els(Ram + "SpecifiedTradeAllowanceCharge"))
        {
            var parsed = ReadAllowanceCharge(allowanceCharge, documentLevel: false);
            (parsed.IsCharge ? result.Charges : result.Allowances).Add(parsed);
        }

        return result;
    }

    private static PriceDetails ReadPrice(XElement? agreement)
    {
        var gross = agreement.El(Ram + "GrossPriceProductTradePrice");
        var net = agreement.El(Ram + "NetPriceProductTradePrice");
        return new PriceDetails
        {
            GrossPrice = gross.El(Ram + "ChargeAmount").Dec(),
            Discount = gross.Path(Ram + "AppliedTradeAllowanceCharge", Ram + "ActualAmount").Dec(),
            NetPrice = net.El(Ram + "ChargeAmount").Dec(),
            BaseQuantity = ReadQuantity(net.El(Ram + "BasisQuantity")
                                        ?? gross.El(Ram + "BasisQuantity")),
            Extensions = ReadPriceExtensions(agreement)
        };
    }

    private static LineItem ReadItem(XElement? product)
    {
        var item = new LineItem
        {
            Name = product.El(Ram + "Name").Text(),
            Description = product.El(Ram + "Description").Text(),
            SellerIdentifier = product.El(Ram + "SellerAssignedID").Text(),
            BuyerIdentifier = product.El(Ram + "BuyerAssignedID").Text(),
            StandardIdentifier = ReadIdentifier(product.El(Ram + "GlobalID")),
            CountryOfOrigin = product.Path(Ram + "OriginTradeCountry", Ram + "ID").Text(),
            Extensions = product is null ? null : ReadItemExtensions(product)
        };

        foreach (var classification in product.Els(Ram + "DesignatedProductClassification"))
        {
            var code = ReadIdentifier(classification.El(Ram + "ClassCode"));
            if (code is not null) item.ClassificationIdentifiers.Add(code);
        }

        foreach (var characteristic in product.Els(Ram + "ApplicableProductCharacteristic"))
        {
            item.Attributes.Add(new ItemAttribute
            {
                Name = characteristic.El(Ram + "Description").Text(),
                Value = characteristic.El(Ram + "Value").Text()
            });
        }

        return item;
    }

    private static AllowanceCharge ReadAllowanceCharge(XElement element, bool documentLevel)
    {
        var result = new AllowanceCharge
        {
            IsCharge = element.Path(Ram + "ChargeIndicator", Udt + "Indicator").Bool() ?? false,
            Amount = element.El(Ram + "ActualAmount").Dec(),
            BaseAmount = element.El(Ram + "BasisAmount").Dec(),
            Percentage = element.El(Ram + "CalculationPercent").Dec(),
            Reason = element.El(Ram + "Reason").Text(),
            ReasonCode = element.El(Ram + "ReasonCode").Text()
        };

        if (documentLevel)
        {
            var tax = element.El(Ram + "CategoryTradeTax");
            result.VatCategoryCode = tax.El(Ram + "CategoryCode").Text();
            result.VatRate = tax.El(Ram + "RateApplicablePercent").Dec();
        }

        return result;
    }

    private static Party? ReadParty(XElement? party)
    {
        if (party is null) return null;

        var result = new Party
        {
            Name = party.El(Ram + "Name").Text(),
            AdditionalLegalInformation = party.El(Ram + "Description").Text(),
            TradingName = party.Path(Ram + "SpecifiedLegalOrganization", Ram + "TradingBusinessName").Text(),
            LegalRegistrationIdentifier = ReadIdentifier(
                party.Path(Ram + "SpecifiedLegalOrganization", Ram + "ID")),
            ElectronicAddress = ReadIdentifier(
                party.Path(Ram + "URIUniversalCommunication", Ram + "URIID")),
            PostalAddress = ReadAddress(party.El(Ram + "PostalTradeAddress")),
            Contact = ReadContact(party.El(Ram + "DefinedTradeContact")),
            Extensions = ReadPartyExtensions(party)
        };

        foreach (var id in party.Els(Ram + "ID").Concat(party.Els(Ram + "GlobalID")))
        {
            var identifier = ReadIdentifier(id);
            if (identifier is not null) result.Identifiers.Add(identifier);
        }

        // schemeID separates the VAT identifier (VA) from any other registration (FC).
        foreach (var registration in party.Els(Ram + "SpecifiedTaxRegistration"))
        {
            var id = registration.El(Ram + "ID");
            var value = id.Text();
            if (value is null) continue;
            if (string.Equals(id.Attr("schemeID"), "VA", StringComparison.OrdinalIgnoreCase))
                result.VatIdentifier ??= value;
            else
                result.TaxRegistrationIdentifier ??= value;
        }

        return result;
    }

    private static PostalAddress? ReadAddress(XElement? address) => address is null ? null : new PostalAddress
    {
        AddressLine1 = address.El(Ram + "LineOne").Text(),
        AddressLine2 = address.El(Ram + "LineTwo").Text(),
        AddressLine3 = address.El(Ram + "LineThree").Text(),
        City = address.El(Ram + "CityName").Text(),
        PostCode = address.El(Ram + "PostcodeCode").Text(),
        CountryCode = address.El(Ram + "CountryID").Text(),
        CountrySubdivision = address.El(Ram + "CountrySubDivisionName").Text()
    };

    private static Contact? ReadContact(XElement? contact) => contact is null ? null : new Contact
    {
        Name = contact.El(Ram + "PersonName").Text(),
        Telephone = contact.Path(Ram + "TelephoneUniversalCommunication", Ram + "CompleteNumber").Text(),
        Email = contact.Path(Ram + "EmailURIUniversalCommunication", Ram + "URIID").Text(),
        Extensions = ReadContactExtensions(contact)
    };

    private static Quantity? ReadQuantity(XElement? element)
    {
        var value = element.Dec();
        return value is null ? null : new Quantity(value.Value, element.Attr("unitCode"));
    }

    private static Identifier? ReadIdentifier(XElement? element)
    {
        var value = element.Text();
        return value is null ? null
            : new Identifier(value, element.Attr("schemeID"), element.Attr("schemeVersionID"));
    }

    private static byte[]? TryDecodeBase64(string? value)
    {
        if (value is null) return null;
        try { return Convert.FromBase64String(value); }
        catch (FormatException) { return null; }
    }
}
