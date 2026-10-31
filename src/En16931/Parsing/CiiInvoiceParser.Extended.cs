using System.Xml.Linq;
using En16931.Model;
using En16931.Model.Extended;
using static En16931.Parsing.Namespaces;

namespace En16931.Parsing;

/// <summary>
/// The part of the CII parser that reads the data over the core of EN 16931.
/// </summary>
/// <remarks>
/// The methods here take the same element as the method of the core that calls them, thus
/// a value can never come to the wrong object. See <see cref="InvoiceExtensions"/> for
/// what belongs here and what does not.
/// </remarks>
public sealed partial class CiiInvoiceParser
{
    /// <summary>The extensions of the invoice. The method makes them on the first call.</summary>
    private static InvoiceExtensions Ext(Invoice invoice) =>
        invoice.Extensions ??= new InvoiceExtensions();

    // --- document -------------------------------------------------------------

    private static void ReadDocumentExtensions(XElement? document, Invoice invoice)
    {
        var title = document.El(Ram + "Name").Text();
        if (title is not null) Ext(invoice).Title = title;
    }

    private static NoteExtensions? ReadNoteExtensions(XElement note)
    {
        var contentCode = note.El(Ram + "ContentCode").Text();
        return contentCode is null ? null : new NoteExtensions { ContentCode = contentCode };
    }

    // --- agreement ------------------------------------------------------------

    private static void ReadAgreementExtensions(XElement? agreement, Invoice invoice)
    {
        if (agreement is null) return;

        var extensions = Ext(invoice);

        extensions.ProjectName = agreement.Path(Ram + "SpecifiedProcuringProject", Ram + "Name").Text();
        extensions.SalesOrderName = agreement.Path(Ram + "SellerOrderReferencedDocument", Ram + "Name").Text();
        extensions.PurchaseOrderDate = FormattedDate(agreement.El(Ram + "BuyerOrderReferencedDocument"));
        extensions.Quotation = ReadReferencedDocument(agreement.El(Ram + "QuotationReferencedDocument"));
    }

    // --- delivery -------------------------------------------------------------

    private static void ReadDeliveryExtensions(XElement? delivery, Invoice invoice)
    {
        if (delivery is null) return;

        var extensions = Ext(invoice);

        extensions.ShipFrom = ReadParty(delivery.El(Ram + "ShipFromTradeParty"));
        extensions.ShipTo = ReadParty(delivery.El(Ram + "ShipToTradeParty"));
        extensions.DeliveryNote = ReadReferencedDocument(delivery.El(Ram + "DeliveryNoteReferencedDocument"));
        extensions.DespatchAdvice = ReadReferencedDocument(delivery.El(Ram + "DespatchAdviceReferencedDocument"));

        foreach (var movement in delivery
                     .El(Ram + "RelatedSupplyChainConsignment")
                     .Els(Ram + "SpecifiedLogisticsTransportMovement"))
        {
            var mode = movement.El(Ram + "ModeCode").Text();
            if (mode is not null) extensions.TransportModeCodes.Add(mode);
        }
    }

    // --- settlement -----------------------------------------------------------

    private static void ReadSettlementExtensions(XElement? settlement, Invoice invoice)
    {
        if (settlement is null) return;

        var extensions = Ext(invoice);

        extensions.Invoicee = ReadParty(settlement.El(Ram + "InvoiceeTradeParty"));
        extensions.AccountingAccountTypeCode = settlement
            .Path(Ram + "ReceivableSpecifiedTradeAccountingAccount", Ram + "TypeCode").Text();

        foreach (var terms in settlement.Els(Ram + "SpecifiedTradePaymentTerms"))
        {
            extensions.PaymentTerms.Add(ReadPaymentTerm(terms));
        }

        foreach (var payment in settlement.Els(Ram + "SpecifiedAdvancePayment"))
        {
            extensions.AdvancePayments.Add(ReadAdvancePayment(payment));
        }

        foreach (var charge in settlement.Els(Ram + "SpecifiedLogisticsServiceCharge"))
        {
            var service = new LogisticsServiceCharge
            {
                Description = charge.El(Ram + "Description").Text(),
                Amount = ReadAmount(charge.El(Ram + "AppliedAmount"))
            };

            foreach (var tax in charge.Els(Ram + "AppliedTradeTax"))
            {
                service.Tax.Add(ReadTaxDetail(tax));
            }

            extensions.LogisticsServiceCharges.Add(service);
        }

        var exchange = settlement.El(Ram + "TaxApplicableTradeCurrencyExchange");
        if (exchange is not null)
        {
            extensions.TaxCurrencyExchange = new CurrencyExchange
            {
                SourceCurrencyCode = exchange.El(Ram + "SourceCurrencyCode").Text(),
                TargetCurrencyCode = exchange.El(Ram + "TargetCurrencyCode").Text(),
                Rate = exchange.El(Ram + "ConversionRate").Dec(),
                RateDate = exchange.El(Ram + "ConversionRateDateTime").El(Udt + "DateTimeString").CiiDate()
            };
        }
    }

    private static PaymentTerm ReadPaymentTerm(XElement terms)
    {
        var term = new PaymentTerm
        {
            Description = terms.El(Ram + "Description").Text(),
            DueDate = terms.Path(Ram + "DueDateDateTime", Udt + "DateTimeString").CiiDate(),
            DirectDebitMandateReference = terms.El(Ram + "DirectDebitMandateID").Text(),
            Penalty = ReadCondition(terms.El(Ram + "ApplicableTradePaymentPenaltyTerms"),
                Ram + "ActualPenaltyAmount")
        };

        var discount = ReadCondition(terms.El(Ram + "ApplicableTradePaymentDiscountTerms"),
            Ram + "ActualDiscountAmount");
        if (discount is not null) term.Discounts.Add(discount);

        return term;
    }

    /*
     * A discount and a penalty have the same form. Only the name of the amount differs,
     * thus the caller gives that name.
     */
    private static PaymentTermCondition? ReadCondition(XElement? condition, XName amountName)
    {
        if (condition is null) return null;

        return new PaymentTermCondition
        {
            Percentage = condition.El(Ram + "CalculationPercent").Dec(),
            Amount = ReadAmount(condition.El(amountName)),
            BasisAmount = ReadAmount(condition.El(Ram + "BasisAmount")),
            BasisPeriodDays = condition.El(Ram + "BasisPeriodMeasure").Dec(),
            BasisDate = condition.Path(Ram + "BasisDateTime", Udt + "DateTimeString").CiiDate()
        };
    }

    private static AdvancePayment ReadAdvancePayment(XElement payment)
    {
        var result = new AdvancePayment
        {
            PaidAmount = ReadAmount(payment.El(Ram + "PaidAmount")),
            ReceivedDate = FormattedDate(payment),
            Invoice = ReadReferencedDocument(payment.El(Ram + "InvoiceSpecifiedReferencedDocument"))
        };

        foreach (var tax in payment.Els(Ram + "IncludedTradeTax"))
        {
            result.IncludedTax.Add(ReadTaxDetail(tax));
        }

        return result;
    }

    private static PaymentExtensions? ReadPaymentExtensions(List<XElement> means)
    {
        var extensions = new PaymentExtensions();
        var used = false;

        foreach (var mean in means)
        {
            var payer = mean.El(Ram + "PayerPartyDebtorFinancialAccount");
            var name = payer.El(Ram + "AccountName").Text();
            var bic = mean.Path(Ram + "PayerSpecifiedDebtorFinancialInstitution", Ram + "BICID").Text();

            if (name is not null) { extensions.DebitedAccountName ??= name; used = true; }
            if (bic is not null) { extensions.DebitedServiceProviderIdentifier ??= bic; used = true; }

            var proprietary = mean
                .Path(Ram + "PayeePartyCreditorFinancialAccount", Ram + "ProprietaryID").Text();
            if (proprietary is not null)
            {
                extensions.ProprietaryAccountIdentifiers.Add(proprietary);
                used = true;
            }
        }

        return used ? extensions : null;
    }

    // --- party ----------------------------------------------------------------

    private static PartyExtensions? ReadPartyExtensions(XElement party)
    {
        var extensions = new PartyExtensions();

        foreach (var contact in party.Els(Ram + "DefinedTradeContact"))
        {
            var read = ReadContact(contact);
            if (read is not null) extensions.Contacts.Add(read);
        }

        foreach (var id in party.Els(Ram + "GlobalID"))
        {
            var identifier = ReadIdentifier(id);
            if (identifier is not null) extensions.GlobalIdentifiers.Add(identifier);
        }

        foreach (var registration in party.Els(Ram + "SpecifiedTaxRegistration"))
        {
            var identifier = ReadIdentifier(registration.El(Ram + "ID"));
            if (identifier is not null) extensions.TaxRegistrations.Add(identifier);
        }

        extensions.LegalOrganizationAddress = ReadAddress(
            party.Path(Ram + "SpecifiedLegalOrganization", Ram + "PostalTradeAddress"));

        extensions.ElectronicAddressNumber = party
            .Path(Ram + "URIUniversalCommunication", Ram + "CompleteNumber").Text();

        bool used = extensions.Contacts.Count > 1
                    || extensions.GlobalIdentifiers.Count > 0
                    || extensions.TaxRegistrations.Count > 0
                    || extensions.LegalOrganizationAddress is not null
                    || extensions.ElectronicAddressNumber is not null;

        return used ? extensions : null;
    }

    private static ContactExtensions? ReadContactExtensions(XElement contact)
    {
        var extensions = new ContactExtensions
        {
            DepartmentName = contact.El(Ram + "DepartmentName").Text(),
            Fax = contact.Path(Ram + "FaxUniversalCommunication", Ram + "CompleteNumber").Text(),
            TypeCode = contact.El(Ram + "TypeCode").Text()
        };

        return extensions.DepartmentName is null && extensions.Fax is null && extensions.TypeCode is null
            ? null
            : extensions;
    }

    // --- documents ------------------------------------------------------------

    private static ReferencedDocument? ReadReferencedDocument(XElement? document)
    {
        if (document is null) return null;

        return new ReferencedDocument
        {
            Reference = document.El(Ram + "IssuerAssignedID").Text(),
            Name = document.El(Ram + "Name").Text(),
            IssueDate = FormattedDate(document),
            Uri = document.El(Ram + "URIID").Text(),
            LineId = document.El(Ram + "LineID").Text(),
            ReferenceTypeCode = document.El(Ram + "ReferenceTypeCode").Text()
        };
    }

    private static SupportingDocumentExtensions? ReadSupportingDocumentExtensions(XElement document)
    {
        var extensions = new SupportingDocumentExtensions
        {
            IssueDate = FormattedDate(document),
            LineId = document.El(Ram + "LineID").Text(),
            TypeCode = document.El(Ram + "TypeCode").Text(),
            ReferenceTypeCode = document.El(Ram + "ReferenceTypeCode").Text()
        };

        return extensions.IssueDate is null && extensions.LineId is null
                                            && extensions.TypeCode is null
                                            && extensions.ReferenceTypeCode is null
            ? null
            : extensions;
    }

    private static PrecedingInvoiceExtensions? ReadPrecedingInvoiceExtensions(XElement document)
    {
        var extensions = new PrecedingInvoiceExtensions
        {
            Name = document.El(Ram + "Name").Text(),
            Uri = document.El(Ram + "URIID").Text(),
            ReferenceTypeCode = document.El(Ram + "ReferenceTypeCode").Text(),
            LineId = document.El(Ram + "LineID").Text()
        };

        return extensions.Name is null && extensions.Uri is null
                                       && extensions.ReferenceTypeCode is null
                                       && extensions.LineId is null
            ? null
            : extensions;
    }

    // --- tax ------------------------------------------------------------------

    private static TaxDetail ReadTaxDetail(XElement tax) => new()
    {
        TypeCode = tax.El(Ram + "TypeCode").Text(),
        CategoryCode = tax.El(Ram + "CategoryCode").Text(),
        RatePercent = tax.El(Ram + "RateApplicablePercent").Dec(),
        CalculatedAmount = ReadAmount(tax.El(Ram + "CalculatedAmount")),
        BasisAmount = ReadAmount(tax.El(Ram + "BasisAmount")),
        LineTotalBasisAmount = ReadAmount(tax.El(Ram + "LineTotalBasisAmount")),
        AllowanceChargeBasisAmount = ReadAmount(tax.El(Ram + "AllowanceChargeBasisAmount")),
        ExemptionReason = tax.El(Ram + "ExemptionReason").Text(),
        ExemptionReasonCode = tax.El(Ram + "ExemptionReasonCode").Text(),
        TaxPointDate = tax.Path(Ram + "TaxPointDate", Udt + "DateString").CiiDate()
    };

    // --- position -------------------------------------------------------------

    private static InvoiceLineExtensions? ReadLineExtensions(XElement line)
    {
        var document = line.El(Ram + "AssociatedDocumentLineDocument");
        var agreement = line.El(Ram + "SpecifiedLineTradeAgreement");
        var delivery = line.El(Ram + "SpecifiedLineTradeDelivery");
        var settlement = line.El(Ram + "SpecifiedLineTradeSettlement");

        var extensions = new InvoiceLineExtensions
        {
            ActualDeliveryDate = delivery
                .Path(Ram + "ActualDeliverySupplyChainEvent", Ram + "OccurrenceDateTime", Udt + "DateTimeString")
                .CiiDate(),
            ShipTo = ReadParty(delivery.El(Ram + "ShipToTradeParty")),
            AccountingAccountTypeCode = settlement
                .Path(Ram + "ReceivableSpecifiedTradeAccountingAccount", Ram + "TypeCode").Text(),
            PackageQuantity = ReadQuantity(delivery.El(Ram + "PackageQuantity")),
            ChargeFreeQuantity = ReadQuantity(delivery.El(Ram + "ChargeFreeQuantity")),
            PurchaseOrder = ReadReferencedDocument(agreement.El(Ram + "BuyerOrderReferencedDocument")),
            Contract = ReadReferencedDocument(agreement.El(Ram + "ContractReferencedDocument")),
            Quotation = ReadReferencedDocument(agreement.El(Ram + "QuotationReferencedDocument")),
            DeliveryNote = ReadReferencedDocument(delivery.El(Ram + "DeliveryNoteReferencedDocument"))
        };

        foreach (var note in document.Els(Ram + "IncludedNote"))
        {
            extensions.Notes.Add(new InvoiceNote
            {
                Text = note.El(Ram + "Content").Text(),
                SubjectCode = note.El(Ram + "SubjectCode").Text(),
                Extensions = ReadNoteExtensions(note)
            });
        }

        foreach (var tax in settlement.Els(Ram + "ApplicableTradeTax"))
        {
            extensions.Tax.Add(ReadTaxDetail(tax));
        }

        /*
         * A document of a position can hold an attachment, for example a bill of
         * quantities. The renderer gives it to the application.
         */
        foreach (var document2 in agreement.Els(Ram + "AdditionalReferencedDocument")
                     .Concat(settlement.Els(Ram + "AdditionalReferencedDocument")))
        {
            extensions.Documents.Add(ReadSupportingDocument(document2));
        }

        bool used = extensions.ActualDeliveryDate is not null
                    || extensions.PackageQuantity is not null
                    || extensions.ChargeFreeQuantity is not null
                    || extensions.ShipTo is not null
                    || extensions.AccountingAccountTypeCode is not null
                    || extensions.PurchaseOrder is not null
                    || extensions.Contract is not null
                    || extensions.Quotation is not null
                    || extensions.DeliveryNote is not null
                    || extensions.Notes.Count > 0
                    || extensions.Tax.Count > 0
                    || extensions.Documents.Count > 0;

        return used ? extensions : null;
    }

    private static LineItemExtensions? ReadItemExtensions(XElement product)
    {
        var extensions = new LineItemExtensions
        {
            IndustryIdentifier = product.El(Ram + "IndustryAssignedID").Text()
        };

        foreach (var instance in product.Els(Ram + "IndividualTradeProductInstance"))
        {
            extensions.Instances.Add(new ProductInstance
            {
                SerialId = instance.El(Ram + "SupplierAssignedSerialID").Text(),
                BatchId = instance.El(Ram + "BatchID").Text()
            });
        }

        foreach (var included in product.Els(Ram + "IncludedReferencedProduct"))
        {
            var read = new IncludedProduct
            {
                Name = included.El(Ram + "Name").Text(),
                Description = included.El(Ram + "Description").Text(),
                SellerIdentifier = included.El(Ram + "SellerAssignedID").Text(),
                BuyerIdentifier = included.El(Ram + "BuyerAssignedID").Text(),
                IndustryIdentifier = included.El(Ram + "IndustryAssignedID").Text(),
                UnitQuantity = ReadQuantity(included.El(Ram + "UnitQuantity"))
            };

            foreach (var id in included.Els(Ram + "GlobalID"))
            {
                var identifier = ReadIdentifier(id);
                if (identifier is not null) read.GlobalIdentifiers.Add(identifier);
            }

            extensions.IncludedProducts.Add(read);
        }

        /*
         * The name of a classification stands beside its code. The core keeps the code,
         * thus this list follows the order of LineItem.ClassificationIdentifiers.
         */
        foreach (var classification in product.Els(Ram + "DesignatedProductClassification"))
        {
            if (ReadIdentifier(classification.El(Ram + "ClassCode")) is not null)
            {
                extensions.ClassificationNames.Add(classification.El(Ram + "ClassName").Text());
            }
        }

        bool used = extensions.IndustryIdentifier is not null
                    || extensions.Instances.Count > 0
                    || extensions.IncludedProducts.Count > 0
                    || extensions.ClassificationNames.Any(name => name is not null);

        return used ? extensions : null;
    }

    private static PriceExtensions? ReadPriceExtensions(XElement? agreement)
    {
        var gross = agreement.El(Ram + "GrossPriceProductTradePrice");
        var net = agreement.El(Ram + "NetPriceProductTradePrice");

        var extensions = new PriceExtensions
        {
            NetBaseQuantity = ReadQuantity(net.El(Ram + "BasisQuantity")),
            GrossBaseQuantity = ReadQuantity(gross.El(Ram + "BasisQuantity"))
        };

        foreach (var allowance in gross.Els(Ram + "AppliedTradeAllowanceCharge"))
        {
            extensions.GrossPriceAllowances.Add(ReadAllowanceCharge(allowance, documentLevel: false));
        }

        bool used = extensions.NetBaseQuantity is not null
                    || extensions.GrossBaseQuantity is not null
                    || extensions.GrossPriceAllowances.Count > 0;

        return used ? extensions : null;
    }

    // --- helpers --------------------------------------------------------------

    /// <summary>An amount with its currency, or null.</summary>
    private static Amount? ReadAmount(XElement? element)
    {
        var value = element.Dec();
        return value is null ? null : new Amount(value.Value, element.Attr("currencyID"));
    }

    /// <summary>
    /// The date of a referenced document. CII puts it in FormattedIssueDateTime, and the
    /// date string stands under the qualified or under the unqualified namespace.
    /// </summary>
    private static DateOnly? FormattedDate(XElement? document)
    {
        var formatted = document.El(Ram + "FormattedIssueDateTime")
                        ?? document.El(Ram + "FormattedReceivedDateTime");

        return formatted.El(Qdt + "DateTimeString").CiiDate()
               ?? formatted.El(Udt + "DateTimeString").CiiDate();
    }
}
