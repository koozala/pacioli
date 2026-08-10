using iText.IO.Font.Constants;
using iText.Kernel.Font;
using iText.Layout.Properties;
using Pacioli.Pdf.ERechnung;
using System;
using System.Collections.Generic;
using Vit.FeRD.ExtendedDocument.Model;

namespace Pacioli.Pdf.Invoice.Sections
{
    /*
     * The data that each section needs.
     *
     * The model of the Cross Industry Invoice puts a value deep in the tree. Thus this
     * class gives the four groups of the transaction directly: Agreement, Delivery,
     * Settlement, and Summation. A section then reads one step only.
     *
     * Each property can be null, because a profile with few fields, for example MINIMUM,
     * does not fill all groups.
     */
    public sealed class InvoiceContext
    {
        private readonly List<AttachmentDescriptor> _attachments = new List<AttachmentDescriptor>();

        public InvoiceContext(CrossIndustryInvoice invoice, En16931.Model.Invoice core, string sourceFile)
        {
            Invoice = invoice;
            Core = core;
            SourceFile = sourceFile;
        }

        /*
         * The invoice in the model of the old library. A section that is not yet moved
         * reads it. It goes away with the last section; see doc/En16931-Migration.md.
         */
        public CrossIndustryInvoice Invoice { get; }

        /*
         * The invoice in the model of En16931. A section that is moved reads this one.
         *
         * The model is flat: a section reads Core.Seller.Name and not a path of six
         * steps. Each property carries its BT or BG number. Data that goes over the core
         * of EN 16931 stands under Extensions.
         */
        public En16931.Model.Invoice Core { get; }

        /* The name of the file that the user gave to the tool. */
        public string SourceFile { get; }

        public PdfFont Bold { get; } = PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD);

        public UnitValue FullWidth { get; } = UnitValue.CreatePercentValue(100.0f);

        /* The groups of the invoice. */

        public ExchangedDocument? Document => Invoice.ExchangedDocument;

        public SupplyChainTradeTransaction? Transaction => Invoice.SupplyChainTradeTransaction;

        public HeaderTradeAgreement? Agreement => Transaction?.ApplicableHeaderTradeAgreement;

        public HeaderTradeDelivery? Delivery => Transaction?.ApplicableHeaderTradeDelivery;

        public HeaderTradeSettlement? Settlement => Transaction?.ApplicableHeaderTradeSettlement;

        public TradeSettlementHeaderMonetarySummation? Summation =>
            Settlement?.SpecifiedTradeSettlementHeaderMonetarySummation;

        public IList<SupplyChainTradeLineItem> LineItems =>
            Transaction?.IncludedSupplyChainTradeLineItem
            ?? (IList<SupplyChainTradeLineItem>)Array.Empty<SupplyChainTradeLineItem>();

        /*
         * The attachments that the sections found. AttachmentsSection fills the list.
         * InvoiceWriter.GetAttachments gives it to the application.
         */
        public IReadOnlyList<AttachmentDescriptor> Attachments => _attachments;

        public void AddAttachment(AttachmentDescriptor attachment) => _attachments.Add(attachment);
    }
}
