namespace Pacioli.CLI.Test.Content
{
    /*
     * The values of the XML input that the test does not look for.
     *
     * The list has two kinds of entry. Keep the two kinds apart, because they have a
     * different meaning:
     *
     *   NotRendered  The renderer must not write this value in this form. A code is an
     *                example: the XML holds "H87", and the page shows "Anz.". These
     *                entries stay.
     *
     *   KnownGap     The renderer does not show this value, but a reader of the invoice
     *                needs it. Each entry is a task for the renderer. If the renderer
     *                starts to show the value, remove the entry. Then the test keeps the
     *                new function.
     *
     * To make the check stronger, remove an entry and start the test. The report then
     * tells you which samples hold the value.
     */
    public static class XmlValueExclusions
    {
        public static IReadOnlyList<XmlValueExclusion> All { get; } = new List<XmlValueExclusion>
        {
            /* ---------------------------------------------------------------
             * Kind 1: the renderer must not write these values in this form.
             * --------------------------------------------------------------- */

            new("@format", ExclusionKind.NotRendered,
                "The format code of a date, for example \"102\". The page shows the date."),

            new("@unitCode", ExclusionKind.NotRendered,
                "The code of a unit, for example \"H87\". The page shows the name of the unit."),

            new("@schemeID", ExclusionKind.NotRendered,
                "The code of an ID scheme, for example \"VA\"."),

            new("@mimeCode", ExclusionKind.NotRendered,
                "The media type of an attachment."),

            new("@listID", ExclusionKind.NotRendered, "The ID of a code list."),
            new("@listVersionID", ExclusionKind.NotRendered, "The version of a code list."),

            new("TypeCode", ExclusionKind.NotRendered,
                "A code, for example \"380\" or \"VAT\". The page shows the text of the code."),

            new("CategoryCode", ExclusionKind.NotRendered,
                "A code of a tax category, for example \"AE\". The page shows the text of the code. " +
                "The category \"S\" is the usual rate, thus the page shows nothing for it."),

            new("/ApplicableHeaderTradeSettlement/ApplicableTradeTax/ExemptionReasonCode",
                ExclusionKind.NotRendered,
                "A code of a VATEX exemption reason, for example \"VATEX-EU-O\". " +
                "The tax table shows the text of the code."),

            new("ReferenceTypeCode", ExclusionKind.NotRendered, "A code of a reference type."),
            new("LineStatusReasonCode", ExclusionKind.NotRendered, "A code of a line status."),
            new("ReasonCode", ExclusionKind.NotRendered,
                "A code of a reason. The page shows the text of the reason."),

            new("Indicator", ExclusionKind.NotRendered,
                "A true or false value. The page shows a text, for example \"Zuschlag\"."),

            new("ClassCode", ExclusionKind.NotRendered, "A code of a product classification."),

            new("AttachmentBinaryObject", ExclusionKind.NotRendered,
                "The data of an attachment, in base64 form."),

            new("/ExchangedDocumentContext/GuidelineSpecifiedDocumentContextParameter/ID",
                ExclusionKind.NotRendered,
                "The ID of the ZUGFeRD profile, for example \"urn:factur-x.eu:1p0:minimum\"."),

            new("/ExchangedDocumentContext/BusinessProcessSpecifiedDocumentContextParameter/ID",
                ExclusionKind.NotRendered,
                "The ID of the business process."),

            /*
             * The names below belong to UBL. The two syntaxes give the same business term
             * a different name, thus a CII entry above does not apply to a UBL document.
             */

            new("/TaxScheme/ID", ExclusionKind.NotRendered,
                "The scheme of a tax, almost always \"VAT\". The page shows the name of the tax."),

            new("InvoiceTypeCode", ExclusionKind.NotRendered,
                "A code, for example \"380\". The page shows the text of the code."),

            new("CreditNoteTypeCode", ExclusionKind.NotRendered,
                "A code, for example \"381\". The page shows the text of the code."),

            new("PaymentMeansCode", ExclusionKind.NotRendered,
                "A code of a payment means. The page shows the text of the code."),

            new("ChargeIndicator", ExclusionKind.NotRendered,
                "A true or false value. The page shows a text, for example \"Zuschlag\"."),

            new("CustomizationID", ExclusionKind.NotRendered,
                "The ID of the profile, for example \"urn:cen.eu:en16931:2017\"."),

            new("ProfileID", ExclusionKind.NotRendered,
                "The ID of the business process."),

            new("/Invoice/Note", ExclusionKind.NotRendered,
                "A note of a UBL document. UBL has no element for the subject code (BT-21), " +
                "thus the code stands before the text between two number signs, for example " +
                "\"#AAI#Ein Hinweis\". The page shows the code and the text apart."),

            new("/CreditNote/Note", ExclusionKind.NotRendered,
                "A note of a UBL credit note. See /Invoice/Note."),

            new("AllowanceChargeReasonCode", ExclusionKind.NotRendered,
                "A code of a reason. The page shows the text of the reason."),

            new("TaxExemptionReasonCode", ExclusionKind.NotRendered,
                "A code of a VATEX exemption reason. The page shows the text of the code."),

            new("EmbeddedDocumentBinaryObject", ExclusionKind.NotRendered,
                "The data of an attachment, in base64 form."),

            new("DocumentTypeCode", ExclusionKind.NotRendered,
                "A code of a document type. The page shows the text of the code."),

            /* ---------------------------------------------------------------
             * Kind 2: the renderer does not show these values yet.
             * Each entry is a task for the renderer.
             * --------------------------------------------------------------- */

            /* Party data */

            //new("/SpecifiedTaxRegistration/ID", ExclusionKind.KnownGap,
            //    "The tax number and the VAT ID of a party. A German invoice must show the VAT ID."),

/*            new("/SpecifiedLegalOrganization/", ExclusionKind.KnownGap,
                "The register data of a party, for example \"HRA 45678\"."),

            new("/SellerTaxRepresentativeTradeParty/", ExclusionKind.KnownGap,
                "The tax representative of the seller. The renderer does not show this party."),

            new("/PayeeTradeParty/", ExclusionKind.KnownGap,
                "The payee. If a payee exists, the renderer shows the buyer in its place."),

            //new("/SellerTradeParty/GlobalID", ExclusionKind.KnownGap, "The global ID of the seller."),
            //new("/BuyerTradeParty/GlobalID", ExclusionKind.KnownGap, "The global ID of the buyer."),
            //new("/ShipToTradeParty/GlobalID", ExclusionKind.KnownGap, "The global ID of the goods receiver."),
            //new("/InvoiceeTradeParty/GlobalID", ExclusionKind.KnownGap, "The global ID of the invoicee."),

            new("/SellerTradeParty/Description", ExclusionKind.KnownGap,
                "Free text about the seller, for example the name of the manager."),

            new("/SellerTradeParty/URIUniversalCommunication/", ExclusionKind.KnownGap,
                "The electronic address of the seller. The renderer shows the address of the buyer only."),

            new("/ShipToTradeParty/URIUniversalCommunication/", ExclusionKind.KnownGap,
                "The electronic address of the goods receiver."),

            new("/DefinedTradeContact/DepartmentName", ExclusionKind.KnownGap,
                "The department of a contact person."),
*/
            /* References */
/*
            new("/SellerOrderReferencedDocument/", ExclusionKind.KnownGap,
                "The order number of the seller."),

            new("/ApplicableHeaderTradeAgreement/ContractReferencedDocument/", ExclusionKind.KnownGap,
                "The contract number of the document."),

            new("/ApplicableHeaderTradeAgreement/AdditionalReferencedDocument/IssuerAssignedID",
                ExclusionKind.KnownGap,
                "The number of an additional document. The page shows the name and the file name only."),

            new("/DespatchAdviceReferencedDocument/", ExclusionKind.KnownGap,
                "The number of the despatch advice."),

            new("/ApplicableHeaderTradeDelivery/DeliveryNoteReferencedDocument/", ExclusionKind.KnownGap,
                "The number of the delivery note of the document."),

            new("/InvoiceReferencedDocument/", ExclusionKind.KnownGap,
                "The number and the date of the invoice that this document corrects."),

            new("/QuotationReferencedDocument/", ExclusionKind.KnownGap,
                "The number of the quotation."),

            new("/BuyerOrderReferencedDocument/LineID", ExclusionKind.KnownGap,
                "The position number in the order of the buyer."),

            new("/DeliveryNoteReferencedDocument/LineID", ExclusionKind.KnownGap,
                "The position number in the delivery note."),
*/
            /* Amounts and sums */
/*

            new("/SpecifiedTradeSettlementHeaderMonetarySummation/LineTotalAmount",
                ExclusionKind.KnownGap,
                "The sum of all positions. The summary table of the page has no row for it."),

            new("/ApplicableTradeTax/LineTotalBasisAmount", ExclusionKind.KnownGap,
                "The basis amount of a tax rate, from the positions."),

            new("/ApplicableTradePaymentDiscountTerms/", ExclusionKind.KnownGap,
                "The discount terms (Skonto): the days, the percentage, and the amount."),

            new("/SpecifiedLogisticsServiceCharge/", ExclusionKind.KnownGap,
                "The transport costs."),

            new("/TaxApplicableTradeCurrencyExchange/", ExclusionKind.KnownGap,
                "The exchange rate for the tax amount."),

            new("/SpecifiedAdvancePayment/", ExclusionKind.KnownGap,
                "A payment that the buyer made before this invoice."),

            new("/ApplicableHeaderTradeSettlement/ReceivableSpecifiedTradeAccountingAccount/ID",
                ExclusionKind.KnownGap,
                "The cost centre of the document. The page shows the cost centre of a position only."),

            new("/ApplicableHeaderTradeSettlement/PaymentReference", ExclusionKind.KnownGap,
                "The payment reference for the bank transfer."),

            new("/ApplicableHeaderTradeSettlement/BillingSpecifiedPeriod/", ExclusionKind.KnownGap,
                "The billing period of the document. The page shows the period of a position only."),
*/
            /* Positions */
/*
            new("/AssociatedDocumentLineDocument/LineID", ExclusionKind.KnownGap,
                "The position number from the XML. The page shows a number that it counts itself."),

            new("/AssociatedDocumentLineDocument/ParentLineID", ExclusionKind.KnownGap,
                "The number of the parent position, for a sub position."),

            new("/IncludedReferencedProduct/", ExclusionKind.KnownGap,
                "A product inside a set, for example the parts of a bundle."),

            new("/IndividualTradeProductInstance/", ExclusionKind.KnownGap,
                "The serial number of an article."),

            new("/DesignatedProductClassification/ClassName", ExclusionKind.KnownGap,
                "The name of a product classification, for example \"Zolltarifnummer\"."),

            new("/NetPriceProductTradePrice/BasisQuantity", ExclusionKind.KnownGap,
                "The price basis quantity, for example a price for 100 pieces."),

            new("/GrossPriceProductTradePrice/BasisQuantity", ExclusionKind.KnownGap,
                "The price basis quantity of the gross price."),

            new("/SpecifiedLineTradeDelivery/ShipToTradeParty/", ExclusionKind.KnownGap,
                "The goods receiver of one position."),

            new("/SpecifiedLineTradeAgreement/AdditionalReferencedDocument/", ExclusionKind.KnownGap,
                "An additional document of one position, for example a bill of quantities."),

            new("/SpecifiedLineTradeSettlement/AdditionalReferencedDocument/IssuerAssignedID",
                ExclusionKind.KnownGap,
                "The number of an additional document of a position, for example an object ID. " +
                "The page shows the name and the file name only."),
*/

            /*
             * The samples of the specification leave no gap. The entries below come from
             * the corpus of ZUGFeRD, which holds documents of the practice. See the note
             * about the corpus in testsettings.json.
             */

            new("/OriginCountry/IdentificationCode", ExclusionKind.KnownGap,
                "BT-159, the country of origin of an article. The page does not show it. " +
                "A CII document gives the same term in OriginTradeCountry/ID, and a country " +
                "code of two letters agrees with almost each page, thus this gap stayed hidden " +
                "until the UBL samples came."),

            new("/ReceivingAdviceReferencedDocument/IssuerAssignedID", ExclusionKind.KnownGap,
                "BT-15, the reference of the receiving advice. The page shows the despatch " +
                "advice (BT-16) only. See TradeAgreementSection.")
        };

        public static XmlValueExclusion? Find(XmlValue value)
        {
            return All.FirstOrDefault(exclusion => exclusion.Matches(value));
        }
    }
}
