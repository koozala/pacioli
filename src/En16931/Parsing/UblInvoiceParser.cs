using System.Text.RegularExpressions;
using System.Xml.Linq;
using En16931.Model;
using static En16931.Parsing.Namespaces;

namespace En16931.Parsing;

/// <summary>
/// Reads an OASIS UBL 2.1 Invoice or CreditNote into the EN 16931 model.
/// </summary>
public sealed partial class UblInvoiceParser : IInvoiceParser
{
    /// <inheritdoc/>
    public InvoiceSyntax Syntax => InvoiceSyntax.Ubl;

    /// <inheritdoc/>
    public bool CanParse(XDocument document) =>
        document.Root?.Name == UblInvoice + "Invoice"
        || document.Root?.Name == UblCreditNote + "CreditNote";

    /// <inheritdoc/>
    public Invoice Parse(XDocument document)
    {
        ArgumentNullException.ThrowIfNull(document);
        var root = document.Root
            ?? throw new InvoiceFormatException("The document has no root element.");
        if (!CanParse(document))
            throw new InvoiceFormatException(
                $"Expected a UBL Invoice or CreditNote as root element but found '{root.Name}'.");

        var isCreditNote = root.Name == UblCreditNote + "CreditNote";
        var invoice = new Invoice { Syntax = InvoiceSyntax.Ubl };

        ReadHeader(root, invoice, isCreditNote);
        ReadReferences(root, invoice);
        ReadParties(root, invoice);
        ReadDelivery(root.El(Cac + "Delivery"), invoice);
        ReadPaymentInstructions(root, invoice);
        ReadAllowancesAndCharges(root, invoice);
        ReadTaxTotals(root, invoice);
        ReadTotals(root.El(Cac + "LegalMonetaryTotal"), invoice);
        ReadExtensions(root, invoice);

        var lineName = isCreditNote ? "CreditNoteLine" : "InvoiceLine";
        foreach (var line in root.Els(Cac + lineName))
            invoice.Lines.Add(ReadLine(line, isCreditNote));

        return invoice;
    }

    private static void ReadHeader(XElement root, Invoice invoice, bool isCreditNote)
    {
        invoice.SpecificationIdentifier = root.El(Cbc + "CustomizationID").Text();
        invoice.BusinessProcessType = root.El(Cbc + "ProfileID").Text();
        invoice.InvoiceNumber = root.El(Cbc + "ID").Text();
        invoice.IssueDate = root.El(Cbc + "IssueDate").IsoDate();
        invoice.PaymentDueDate = root.El(Cbc + "DueDate").IsoDate();
        invoice.InvoiceTypeCode = root
            .El(Cbc + (isCreditNote ? "CreditNoteTypeCode" : "InvoiceTypeCode")).Text();
        invoice.CurrencyCode = root.El(Cbc + "DocumentCurrencyCode").Text();
        invoice.TaxCurrencyCode = root.El(Cbc + "TaxCurrencyCode").Text();
        invoice.TaxPointDate = root.El(Cbc + "TaxPointDate").IsoDate();
        invoice.BuyerReference = root.El(Cbc + "BuyerReference").Text();
        invoice.BuyerAccountingReference = root.El(Cbc + "AccountingCost").Text();
        invoice.PaymentTerms = root.Path(Cac + "PaymentTerms", Cbc + "Note").Text();

        foreach (var note in root.Els(Cbc + "Note"))
            invoice.Notes.Add(ReadNote(note.Text()));

        var period = root.El(Cac + "InvoicePeriod");
        if (period is not null)
        {
            invoice.TaxPointDateCode = period.El(Cbc + "DescriptionCode").Text();
            var start = period.El(Cbc + "StartDate").IsoDate();
            var end = period.El(Cbc + "EndDate").IsoDate();
            if (start is not null || end is not null)
                invoice.InvoicingPeriod = new Period { StartDate = start, EndDate = end };
        }
    }

    /// <summary>
    /// UBL has no element for BT-21. The established convention, which the CII to
    /// UBL converters follow, is to prefix the note text with the subject code
    /// between two hash characters.
    /// </summary>
    private static InvoiceNote ReadNote(string? text)
    {
        if (text is null) return new InvoiceNote();
        var match = NoteSubjectCode().Match(text);
        return match.Success
            ? new InvoiceNote { SubjectCode = match.Groups[1].Value, Text = match.Groups[2].Value }
            : new InvoiceNote { Text = text };
    }

    [GeneratedRegex(@"^#([A-Z0-9]{3})#(.*)$", RegexOptions.Singleline)]
    private static partial Regex NoteSubjectCode();

    private static void ReadReferences(XElement root, Invoice invoice)
    {
        invoice.PurchaseOrderReference = root.Path(Cac + "OrderReference", Cbc + "ID").Text();
        invoice.SalesOrderReference = root.Path(Cac + "OrderReference", Cbc + "SalesOrderID").Text();
        invoice.ContractReference = root.Path(Cac + "ContractDocumentReference", Cbc + "ID").Text();
        invoice.ProjectReference = root.Path(Cac + "ProjectReference", Cbc + "ID").Text();
        invoice.DespatchAdviceReference = root.Path(Cac + "DespatchDocumentReference", Cbc + "ID").Text();
        invoice.ReceivingAdviceReference = root.Path(Cac + "ReceiptDocumentReference", Cbc + "ID").Text();
        invoice.TenderOrLotReference = root.Path(Cac + "OriginatorDocumentReference", Cbc + "ID").Text();

        foreach (var billing in root.Els(Cac + "BillingReference"))
        {
            var reference = billing.El(Cac + "InvoiceDocumentReference");
            if (reference is null) continue;
            invoice.PrecedingInvoiceReferences.Add(new PrecedingInvoiceReference
            {
                InvoiceNumber = reference.El(Cbc + "ID").Text(),
                IssueDate = reference.El(Cbc + "IssueDate").IsoDate()
            });
        }

        // DocumentTypeCode 130 marks BT-18; everything else is a supporting document.
        foreach (var reference in root.Els(Cac + "AdditionalDocumentReference"))
        {
            var id = reference.El(Cbc + "ID");
            if (reference.El(Cbc + "DocumentTypeCode").Text() == "130")
            {
                invoice.InvoicedObjectIdentifier = ReadIdentifier(id);
                continue;
            }

            var attachment = reference.El(Cac + "Attachment");
            var binary = attachment.El(Cbc + "EmbeddedDocumentBinaryObject");
            invoice.SupportingDocuments.Add(new SupportingDocument
            {
                Reference = id.Text(),
                Description = reference.El(Cbc + "DocumentDescription").Text(),
                ExternalLocation = attachment
                    .Path(Cac + "ExternalReference", Cbc + "URI").Text(),
                Attachment = binary is null ? null : new Attachment
                {
                    Content = TryDecodeBase64(binary.Text()),
                    MimeCode = binary.Attr("mimeCode"),
                    FileName = binary.Attr("filename")
                }
            });
        }
    }

    private static void ReadParties(XElement root, Invoice invoice)
    {
        invoice.Seller = ReadParty(root.Path(Cac + "AccountingSupplierParty", Cac + "Party"),
            nameFromLegalEntity: true);
        invoice.Buyer = ReadParty(root.Path(Cac + "AccountingCustomerParty", Cac + "Party"),
            nameFromLegalEntity: true);
        invoice.Payee = ReadParty(root.El(Cac + "PayeeParty"), nameFromLegalEntity: false);
        invoice.SellerTaxRepresentative = ReadParty(root.El(Cac + "TaxRepresentativeParty"),
            nameFromLegalEntity: false);
    }

    private static void ReadDelivery(XElement? delivery, Invoice invoice)
    {
        if (delivery is null) return;

        var location = delivery.El(Cac + "DeliveryLocation");
        invoice.Delivery = new DeliveryInformation
        {
            ActualDeliveryDate = delivery.El(Cbc + "ActualDeliveryDate").IsoDate(),
            LocationIdentifier = ReadIdentifier(location.El(Cbc + "ID")),
            PartyName = delivery.Path(Cac + "DeliveryParty", Cac + "PartyName", Cbc + "Name").Text(),
            Address = ReadAddress(location.El(Cac + "Address"))
        };
    }

    private static void ReadPaymentInstructions(XElement root, Invoice invoice)
    {
        var allMeans = root.Els(Cac + "PaymentMeans").ToList();
        if (allMeans.Count == 0) return;

        var first = allMeans[0];
        var code = first.El(Cbc + "PaymentMeansCode");
        var instructions = new PaymentInstructions
        {
            PaymentMeansTypeCode = code.Text(),
            PaymentMeansText = code.Attr("name"),
            RemittanceInformation = first.El(Cbc + "PaymentID").Text()
        };

        foreach (var means in allMeans)
        {
            var account = means.El(Cac + "PayeeFinancialAccount");
            if (account is not null)
            {
                instructions.CreditTransfers.Add(new CreditTransfer
                {
                    AccountIdentifier = account.El(Cbc + "ID").Text(),
                    AccountName = account.El(Cbc + "Name").Text(),
                    ServiceProviderIdentifier = account
                        .Path(Cac + "FinancialInstitutionBranch", Cbc + "ID").Text()
                });
            }

            var card = means.El(Cac + "CardAccount");
            if (card is not null)
            {
                instructions.PaymentCard ??= new PaymentCard
                {
                    PrimaryAccountNumber = card.El(Cbc + "PrimaryAccountNumberID").Text(),
                    HolderName = card.El(Cbc + "HolderName").Text()
                };
            }

            var mandate = means.El(Cac + "PaymentMandate");
            if (mandate is not null)
            {
                instructions.DirectDebit ??= new DirectDebit
                {
                    MandateReference = mandate.El(Cbc + "ID").Text(),
                    DebitedAccountIdentifier = mandate
                        .Path(Cac + "PayerFinancialAccount", Cbc + "ID").Text()
                };
            }
        }

        invoice.PaymentInstructions = instructions;
    }

    private static void ReadAllowancesAndCharges(XElement root, Invoice invoice)
    {
        foreach (var element in root.Els(Cac + "AllowanceCharge"))
        {
            var parsed = ReadAllowanceCharge(element, documentLevel: true);
            (parsed.IsCharge ? invoice.Charges : invoice.Allowances).Add(parsed);
        }
    }

    private static void ReadTaxTotals(XElement root, Invoice invoice)
    {
        foreach (var taxTotal in root.Els(Cac + "TaxTotal"))
        {
            var amountElement = taxTotal.El(Cbc + "TaxAmount");
            var value = amountElement.Dec();
            if (value is not null)
            {
                var currency = amountElement.Attr("currencyID");
                var amount = new Amount(value.Value, currency);

                // BT-110 and BT-111 share one element and differ by currency.
                if (currency is not null && invoice.TaxCurrencyCode is not null
                                         && currency == invoice.TaxCurrencyCode
                                         && currency != invoice.CurrencyCode)
                    invoice.Totals.TaxAmountInAccountingCurrency ??= amount;
                else
                    invoice.Totals.TaxAmount ??= amount;
            }

            foreach (var subtotal in taxTotal.Els(Cac + "TaxSubtotal"))
            {
                var category = subtotal.El(Cac + "TaxCategory");
                invoice.VatBreakdown.Add(new VatBreakdown
                {
                    TaxableAmount = subtotal.El(Cbc + "TaxableAmount").Dec(),
                    TaxAmount = subtotal.El(Cbc + "TaxAmount").Dec(),
                    CategoryCode = category.El(Cbc + "ID").Text(),
                    CategoryRate = category.El(Cbc + "Percent").Dec(),
                    ExemptionReasonText = category.El(Cbc + "TaxExemptionReason").Text(),
                    ExemptionReasonCode = category.El(Cbc + "TaxExemptionReasonCode").Text(),
                    Detail = ReadTaxDetail(subtotal)
                });
            }
        }
    }

    private static void ReadTotals(XElement? monetaryTotal, Invoice invoice)
    {
        if (monetaryTotal is null) return;

        var totals = invoice.Totals;
        totals.LineTotalAmount = monetaryTotal.El(Cbc + "LineExtensionAmount").Dec();
        totals.AllowanceTotalAmount = monetaryTotal.El(Cbc + "AllowanceTotalAmount").Dec();
        totals.ChargeTotalAmount = monetaryTotal.El(Cbc + "ChargeTotalAmount").Dec();
        totals.TaxExclusiveAmount = monetaryTotal.El(Cbc + "TaxExclusiveAmount").Dec();
        totals.TaxInclusiveAmount = monetaryTotal.El(Cbc + "TaxInclusiveAmount").Dec();
        totals.PaidAmount = monetaryTotal.El(Cbc + "PrepaidAmount").Dec();
        totals.RoundingAmount = monetaryTotal.El(Cbc + "PayableRoundingAmount").Dec();
        totals.DuePayableAmount = monetaryTotal.El(Cbc + "PayableAmount").Dec();
    }

    private static InvoiceLine ReadLine(XElement line, bool isCreditNote)
    {
        var quantityName = isCreditNote ? "CreditedQuantity" : "InvoicedQuantity";
        var item = line.El(Cac + "Item");

        var result = new InvoiceLine
        {
            Id = line.El(Cbc + "ID").Text(),
            Note = line.El(Cbc + "Note").Text(),
            InvoicedQuantity = ReadQuantity(line.El(Cbc + quantityName)),
            NetAmount = line.El(Cbc + "LineExtensionAmount").Dec(),
            BuyerAccountingReference = line.El(Cbc + "AccountingCost").Text(),
            OrderLineReference = line.Path(Cac + "OrderLineReference", Cbc + "LineID").Text(),
            ObjectIdentifier = ReadIdentifier(line.Path(Cac + "DocumentReference", Cbc + "ID")),
            Item = ReadItem(item),
            Price = ReadPrice(line.El(Cac + "Price")),
            Extensions = ReadLineExtensions(line, isCreditNote),
            Vat = new LineVatInformation
            {
                CategoryCode = item.Path(Cac + "ClassifiedTaxCategory", Cbc + "ID").Text(),
                Rate = item.Path(Cac + "ClassifiedTaxCategory", Cbc + "Percent").Dec()
            }
        };

        var period = line.El(Cac + "InvoicePeriod");
        if (period is not null)
        {
            var start = period.El(Cbc + "StartDate").IsoDate();
            var end = period.El(Cbc + "EndDate").IsoDate();
            if (start is not null || end is not null)
                result.Period = new Period { StartDate = start, EndDate = end };
        }

        foreach (var element in line.Els(Cac + "AllowanceCharge"))
        {
            var parsed = ReadAllowanceCharge(element, documentLevel: false);
            (parsed.IsCharge ? result.Charges : result.Allowances).Add(parsed);
        }

        return result;
    }

    private static PriceDetails ReadPrice(XElement? price)
    {
        var allowance = price.El(Cac + "AllowanceCharge");
        return new PriceDetails
        {
            NetPrice = price.El(Cbc + "PriceAmount").Dec(),
            Discount = allowance.El(Cbc + "Amount").Dec(),
            GrossPrice = allowance.El(Cbc + "BaseAmount").Dec(),
            BaseQuantity = ReadQuantity(price.El(Cbc + "BaseQuantity")),
            Extensions = ReadPriceExtensions(price)
        };
    }

    private static LineItem ReadItem(XElement? item)
    {
        var result = new LineItem
        {
            Name = item.El(Cbc + "Name").Text(),
            Description = item.El(Cbc + "Description").Text(),
            SellerIdentifier = item.Path(Cac + "SellersItemIdentification", Cbc + "ID").Text(),
            BuyerIdentifier = item.Path(Cac + "BuyersItemIdentification", Cbc + "ID").Text(),
            StandardIdentifier = ReadIdentifier(
                item.Path(Cac + "StandardItemIdentification", Cbc + "ID")),
            CountryOfOrigin = item.Path(Cac + "OriginCountry", Cbc + "IdentificationCode").Text()
        };

        foreach (var classification in item.Els(Cac + "CommodityClassification"))
        {
            var code = ReadIdentifier(classification.El(Cbc + "ItemClassificationCode"));
            if (code is not null) result.ClassificationIdentifiers.Add(code);
        }

        foreach (var property in item.Els(Cac + "AdditionalItemProperty"))
        {
            result.Attributes.Add(new ItemAttribute
            {
                Name = property.El(Cbc + "Name").Text(),
                Value = property.El(Cbc + "Value").Text()
            });
        }

        return result;
    }

    private static AllowanceCharge ReadAllowanceCharge(XElement element, bool documentLevel)
    {
        var result = new AllowanceCharge
        {
            IsCharge = element.El(Cbc + "ChargeIndicator").Bool() ?? false,
            Amount = element.El(Cbc + "Amount").Dec(),
            BaseAmount = element.El(Cbc + "BaseAmount").Dec(),
            Percentage = element.El(Cbc + "MultiplierFactorNumeric").Dec(),
            Reason = element.El(Cbc + "AllowanceChargeReason").Text(),
            ReasonCode = element.El(Cbc + "AllowanceChargeReasonCode").Text()
        };

        if (documentLevel)
        {
            var category = element.El(Cac + "TaxCategory");
            result.VatCategoryCode = category.El(Cbc + "ID").Text();
            result.VatRate = category.El(Cbc + "Percent").Dec();
        }

        return result;
    }

    /// <param name="party">The <c>cac:Party</c> element, or null.</param>
    /// <param name="nameFromLegalEntity">
    /// Seller and buyer take their name (BT-27 / BT-44) from the legal entity and
    /// their trading name (BT-28 / BT-45) from the party name. Payee (BT-59) and
    /// tax representative (BT-62) take their name from the party name instead.
    /// </param>
    private static Party? ReadParty(XElement? party, bool nameFromLegalEntity)
    {
        if (party is null) return null;

        var legalEntity = party.El(Cac + "PartyLegalEntity");
        var partyName = party.Path(Cac + "PartyName", Cbc + "Name").Text();

        var result = new Party
        {
            Name = nameFromLegalEntity
                ? legalEntity.El(Cbc + "RegistrationName").Text() ?? partyName
                : partyName ?? legalEntity.El(Cbc + "RegistrationName").Text(),
            TradingName = nameFromLegalEntity ? partyName : null,
            LegalRegistrationIdentifier = ReadIdentifier(legalEntity.El(Cbc + "CompanyID")),
            AdditionalLegalInformation = legalEntity.El(Cbc + "CompanyLegalForm").Text(),
            ElectronicAddress = ReadIdentifier(party.El(Cbc + "EndpointID")),
            PostalAddress = ReadAddress(party.El(Cac + "PostalAddress")),
            Contact = ReadContact(party.El(Cac + "Contact")),
            Extensions = ReadPartyExtensions(party)
        };

        foreach (var identification in party.Els(Cac + "PartyIdentification"))
        {
            var id = identification.El(Cbc + "ID");
            var identifier = ReadIdentifier(id);
            if (identifier is null) continue;

            // A SEPA scheme identifier marks BT-90 rather than a party identifier.
            if (string.Equals(identifier.SchemeId, "SEPA", StringComparison.OrdinalIgnoreCase))
                result.BankAssignedCreditorIdentifier ??= identifier.Value;
            else
                result.Identifiers.Add(identifier);
        }

        foreach (var scheme in party.Els(Cac + "PartyTaxScheme"))
        {
            var value = scheme.El(Cbc + "CompanyID").Text();
            if (value is null) continue;
            var schemeId = scheme.Path(Cac + "TaxScheme", Cbc + "ID").Text();
            if (string.Equals(schemeId, "VAT", StringComparison.OrdinalIgnoreCase))
                result.VatIdentifier ??= value;
            else
                result.TaxRegistrationIdentifier ??= value;
        }

        return result;
    }

    private static PostalAddress? ReadAddress(XElement? address) => address is null ? null : new PostalAddress
    {
        AddressLine1 = address.El(Cbc + "StreetName").Text(),
        AddressLine2 = address.El(Cbc + "AdditionalStreetName").Text(),
        AddressLine3 = address.Path(Cac + "AddressLine", Cbc + "Line").Text(),
        City = address.El(Cbc + "CityName").Text(),
        PostCode = address.El(Cbc + "PostalZone").Text(),
        CountrySubdivision = address.El(Cbc + "CountrySubentity").Text(),
        CountryCode = address.Path(Cac + "Country", Cbc + "IdentificationCode").Text()
    };

    private static Contact? ReadContact(XElement? contact) => contact is null ? null : new Contact
    {
        Name = contact.El(Cbc + "Name").Text(),
        Telephone = contact.El(Cbc + "Telephone").Text(),
        Email = contact.El(Cbc + "ElectronicMail").Text()
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
