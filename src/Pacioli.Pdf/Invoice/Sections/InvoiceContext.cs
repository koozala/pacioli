using iText.IO.Font.Constants;
using iText.Kernel.Font;
using iText.Layout.Properties;
using Pacioli.Pdf.ERechnung;
using System;
using System.Collections.Generic;
using En16931.Model;

namespace Pacioli.Pdf.Invoice.Sections
{
    /*
     * The data that each section needs: the invoice, the name of the source file, and the
     * few things of the page that more than one section uses.
     *
     * The model of En16931 is flat, thus this class needs no short way into a tree any
     * more. A section reads Core.Seller.Name, and data over the core of EN 16931 stands
     * under Extensions.
     */
    public sealed class InvoiceContext
    {
        private readonly List<AttachmentDescriptor> _attachments = new List<AttachmentDescriptor>();

        public InvoiceContext(En16931.Model.Invoice invoice, string sourceFile)
        {
            Core = invoice;
            SourceFile = sourceFile;
        }

        /*
         * The invoice, in the model of En16931.
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

        /*
         * The attachments that the sections found. AttachmentsSection fills the list.
         * InvoiceWriter.GetAttachments gives it to the application.
         */
        public IReadOnlyList<AttachmentDescriptor> Attachments => _attachments;

        public void AddAttachment(AttachmentDescriptor attachment) => _attachments.Add(attachment);
    }
}
