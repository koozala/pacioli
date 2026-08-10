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

        /*
         * The embedded files of the source PDF file, without the invoice. An enclosure of
         * the invoice can stand there and not in the XML; see GetAttachments.
         */
        IReadOnlyList<EInvoiceExtractor.EmbeddedFile> embeddedFiles =
            new List<EInvoiceExtractor.EmbeddedFile>();

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
            embeddedFiles = extractor.EmbeddedFiles;

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
         *
         * An attachment can stand in two places. The XML can hold the data of the file in
         * base64 form; then AttachmentsSection has it already. Or the PDF file holds the
         * file beside the invoice, and the XML names it with a URI of the form
         * "#ef=Aufmass.png". This method joins the two: it gives each attachment of the
         * XML its data from the PDF file, and it adds each embedded file that the XML
         * does not name. Thus the application can open each file that the document
         * brings.
         */
        public IReadOnlyList<AttachmentDescriptor> GetAttachments()
        {
            var attachments = new List<AttachmentDescriptor>(
                context?.Attachments ?? (IReadOnlyList<AttachmentDescriptor>)new List<AttachmentDescriptor>());

            if (embeddedFiles.Count == 0)
            {
                return attachments;
            }

            var open = new List<EInvoiceExtractor.EmbeddedFile>(embeddedFiles);

            /* First each attachment of the XML that has no data of its own. */
            foreach (AttachmentDescriptor attachment in attachments)
            {
                if (attachment.Data != null && attachment.Data.Length > 0)
                {
                    continue;
                }

                EInvoiceExtractor.EmbeddedFile? file = TakeFileFor(attachment, open);
                if (file == null)
                {
                    continue;
                }

                attachment.Data = file.Data;
                if (string.IsNullOrWhiteSpace(attachment.FileName))
                {
                    attachment.FileName = file.Name ?? string.Empty;
                }
            }

            /*
             * Then each embedded file that no attachment of the XML took. The user can
             * open it, although the invoice does not name it, or names it with a name
             * that the PDF file does not use.
             */
            foreach (EInvoiceExtractor.EmbeddedFile file in open)
            {
                attachments.Add(new AttachmentDescriptor
                {
                    FileName = file.Name ?? string.Empty,
                    Name = file.Name,
                    Data = file.Data
                });
            }

            return attachments;
        }

        /*
         * The embedded file that belongs to an attachment of the XML, or null.
         *
         * The XML names the file in the form "#ef=Aufmass.png". The name in the PDF file
         * is mostly the same, but a program that makes the file can give it a prefix, or
         * it can change the name. Thus the method takes three steps:
         *
         *   1. A name that is equal.
         *   2. A name that ends with the name of the reference, for a prefix.
         *   3. The one file that is left with the same kind, for example ".pdf".
         *
         * Step 3 applies only if exactly one file of that kind is left. With two files of
         * one kind the method gives nothing back, because a wrong file is worse than no
         * file: the two files then stand for themselves in the list of the attachments,
         * and the user can open each one.
         *
         * A file that the method gives back goes out of the list, thus two attachments
         * never take the same file.
         */
        private static EInvoiceExtractor.EmbeddedFile? TakeFileFor(
            AttachmentDescriptor attachment,
            List<EInvoiceExtractor.EmbeddedFile> open)
        {
            string wanted = EmbeddedFileNameOf(attachment);

            if (wanted.Length == 0)
            {
                return null;
            }

            EInvoiceExtractor.EmbeddedFile? found =
                open.Find(file => Equal(file.Name, wanted))
                ?? open.Find(file => EndsWith(file.Name, wanted))
                ?? OnlyFileOfTheKind(open, System.IO.Path.GetExtension(wanted));

            if (found != null)
            {
                open.Remove(found);
            }

            return found;
        }

        /* The one file with this extension, or null if there is none or more than one. */
        private static EInvoiceExtractor.EmbeddedFile? OnlyFileOfTheKind(
            List<EInvoiceExtractor.EmbeddedFile> open, string extension)
        {
            if (string.IsNullOrWhiteSpace(extension))
            {
                return null;
            }

            EInvoiceExtractor.EmbeddedFile? only = null;

            foreach (EInvoiceExtractor.EmbeddedFile file in open)
            {
                if (!EndsWith(file.Name, extension))
                {
                    continue;
                }

                if (only != null)
                {
                    return null;
                }

                only = file;
            }

            return only;
        }

        /*
         * The name of the embedded file that an attachment names, without the mark
         * "#ef=". An attachment that names no embedded file gives its own file name, thus
         * a document that writes the plain name also works.
         */
        private static string EmbeddedFileNameOf(AttachmentDescriptor attachment)
        {
            const string mark = "#ef=";

            string uri = attachment.Uri ?? string.Empty;
            int position = uri.IndexOf(mark, System.StringComparison.OrdinalIgnoreCase);

            if (position >= 0)
            {
                return uri.Substring(position + mark.Length).Trim();
            }

            return attachment.FileName.Trim();
        }

        private static bool Equal(string? name, string wanted) =>
            name != null && string.Equals(name, wanted, System.StringComparison.OrdinalIgnoreCase);

        private static bool EndsWith(string? name, string wanted) =>
            name != null && name.EndsWith(wanted, System.StringComparison.OrdinalIgnoreCase);

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
