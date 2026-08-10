using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Layout;
using Pacioli.Pdf.ERechnung;
using Pacioli.Pdf.Invoice.Sections;
using System.Collections.Generic;
using System.IO;

namespace Pacioli.Pdf.Invoice
{
    /*
     * Writes an invoice to a PDF page.
     *
     * The parser is the project En16931. The class reads the invoice, then it gives
     * the data to each section of the list Sections. A section writes one part of the
     * page. Thus this class holds no content of its own: to change the content of the
     * page, change a section, or change the list.
     *
     * See InvoiceSections.Default for the list, and IInvoiceSection for the interface.
     */
    public class InvoiceWriter
    {
        En16931.Model.Invoice invoice;
        string sourceFile;

        Stream XmlData = new MemoryStream();
        InvoiceContext? context;

        public bool IsZugferd { get; private set; }

        /*
         * The parts of the page, in the order of the page.
         * You can add a part, remove a part, or move a part, before you call Write.
         */
        public List<IInvoiceSection> Sections { get; } = InvoiceSections.Default();

        public InvoiceWriter(string fileName)
        {
            sourceFile = fileName;

            var extractor = new EInvoiceExtractor(fileName);
            IsZugferd = extractor.HasInvoice;

            if (IsZugferd)
            {
                using Stream data = extractor.XmlDataStream!;
                data.CopyTo(XmlData);
            }
            else
            {
                using var fstream = new FileStream(fileName, FileMode.Open, FileAccess.Read);
                fstream.CopyTo(XmlData);
            }
            XmlData.Seek(0, SeekOrigin.Begin);

            invoice = En16931.InvoiceReader.Read(XmlData);
            XmlData.Seek(0, SeekOrigin.Begin);
        }

        public Stream GetXml() => XmlData;

        /*
         * The attachments of the invoice. The list is empty before the first call of
         * Write, because AttachmentsSection fills it.
         */
        public IReadOnlyList<AttachmentDescriptor> GetAttachments() =>
            context?.Attachments ?? (IReadOnlyList<AttachmentDescriptor>)new List<AttachmentDescriptor>();

        public void Write(string outputFileName)
        {
            PdfWriter pdfWriter = new PdfWriter(outputFileName);
            PdfDocument document = new PdfDocument(pdfWriter);
            Document doc = new Document(document);

            document.AddNewPage();

            context = new InvoiceContext(invoice, sourceFile);

            foreach (IInvoiceSection section in Sections)
            {
                section.Write(doc, context);
            }

            doc.Close();
        }
    }
}
