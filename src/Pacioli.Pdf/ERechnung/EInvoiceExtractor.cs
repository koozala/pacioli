using En16931;
using iText.Kernel.Pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Pacioli.Pdf.ERechnung
{
    /*
     * Takes the invoice out of a PDF file.
     *
     * A Factur-X, ZUGFeRD or XRechnung PDF file is a PDF/A-3 file with the invoice as an
     * embedded file. The name tree of the embedded files holds it, thus this class reads
     * that tree and does not walk through each object of the file.
     *
     * The class finds a CII invoice and a UBL invoice, thus its name does not say
     * "Zugferd" any more.
     *
     * HOW THE CLASS SELECTS THE ATTACHMENT
     *
     * A PDF file can hold more than one attachment: the invoice, and enclosures, for
     * example a bill of quantities. The class puts the attachments in an order and takes
     * the first one that is an invoice:
     *
     *   1. The attachments with a name of the specification, for example "factur-x.xml".
     *   2. The other attachments with the ending ".xml".
     *   3. Each other attachment.
     *
     * Inside a group, the order of the document stays. The test is
     * InvoiceReader.DetectSyntax: it looks at the root element only, thus it is fast and
     * it does not raise an exception.
     */
    public sealed class EInvoiceExtractor
    {
        /*
         * The names that the specifications give to the attachment. The order is the
         * order of the test.
         */
        private static readonly string[] KnownNames =
        {
            "factur-x.xml",
            "zugferd-invoice.xml",
            "xrechnung.xml",
            "cii.xml"
        };

        public EInvoiceExtractor(string fileName)
        {
            FileName = fileName;

            try
            {
                Read(fileName);
            }
            catch (Exception)
            {
                /*
                 * A file that is not a PDF file, a file that a program holds, or a PDF
                 * file with a defect: the caller then reads the file as an XML file.
                 */
                XmlDataStream?.Dispose();
                XmlDataStream = null;
                AttachmentName = null;
                Syntax = InvoiceSyntax.Unknown;
            }
        }

        /* The file that the caller gave. */
        public string FileName { get; }

        /* True if the PDF file holds an invoice. */
        public bool HasInvoice => XmlDataStream != null;

        /* The XML data of the invoice, at position 0, or null. */
        public Stream? XmlDataStream { get; private set; }

        /* The name of the attachment, for example "factur-x.xml", or null. */
        public string? AttachmentName { get; private set; }

        /* The syntax of the invoice: Cii, Ubl, or Unknown if the file holds none. */
        public InvoiceSyntax Syntax { get; private set; } = InvoiceSyntax.Unknown;

        private void Read(string fileName)
        {
            using var reader = new PdfReader(fileName);
            using var document = new PdfDocument(reader);

            foreach (Attachment attachment in AttachmentsOf(document))
            {
                using var candidate = new MemoryStream(attachment.Data, writable: false);

                InvoiceSyntax syntax;
                try
                {
                    syntax = InvoiceReader.DetectSyntax(System.Xml.Linq.XDocument.Load(candidate));
                }
                catch (System.Xml.XmlException)
                {
                    /* The attachment is not XML. The next one gets the test. */
                    continue;
                }

                if (syntax == InvoiceSyntax.Unknown)
                {
                    continue;
                }

                XmlDataStream = new MemoryStream(attachment.Data, writable: false);
                AttachmentName = attachment.Name;
                Syntax = syntax;
                return;
            }
        }

        /* One attachment of the PDF file. */
        private sealed class Attachment
        {
            public Attachment(string? name, byte[] data)
            {
                Name = name;
                Data = data;
            }

            public string? Name { get; }
            public byte[] Data { get; }
        }

        /*
         * The attachments of the file, in the order of the test. The method reads the
         * data of an attachment only when the caller asks for it, thus a file with a big
         * enclosure does not go into the memory before the invoice is found.
         */
        private static IEnumerable<Attachment> AttachmentsOf(PdfDocument document)
        {
            var files = new List<(string? Name, PdfDictionary Specification)>();

            PdfNameTree tree = document.GetCatalog().GetNameTree(PdfName.EmbeddedFiles);

            foreach (var entry in tree.GetNames())
            {
                if (entry.Value is not PdfDictionary specification)
                {
                    continue;
                }

                files.Add((NameOf(specification) ?? entry.Key?.ToString(), specification));
            }

            /*
             * A name of the specification comes first, then an other XML file, then the
             * rest. OrderBy keeps the order of the document inside a group.
             */
            foreach (var file in files.OrderBy(entry => GroupOf(entry.Name)))
            {
                byte[]? data = DataOf(file.Specification);

                if (data != null && data.Length > 0)
                {
                    yield return new Attachment(file.Name, data);
                }
            }
        }

        /* 0 for a name of the specification, 1 for an other XML file, 2 for the rest. */
        private static int GroupOf(string? name)
        {
            if (name == null)
            {
                return 2;
            }

            if (KnownNames.Any(known => string.Equals(name, known, StringComparison.OrdinalIgnoreCase)))
            {
                return 0;
            }

            return name.EndsWith(".xml", StringComparison.OrdinalIgnoreCase) ? 1 : 2;
        }

        /*
         * The name of the file. UF holds the name in Unicode form and F holds the older
         * form; the specification asks for both, and a file of the practice can give one
         * of the two only.
         */
        private static string? NameOf(PdfDictionary specification) =>
            specification.GetAsString(PdfName.UF)?.ToUnicodeString()
            ?? specification.GetAsString(PdfName.F)?.ToUnicodeString();

        /* The data of the embedded file, or null if the entry holds none. */
        private static byte[]? DataOf(PdfDictionary specification)
        {
            PdfDictionary? embedded = specification.GetAsDictionary(PdfName.EF);

            PdfStream? stream = embedded?.GetAsStream(PdfName.F)
                                ?? embedded?.GetAsStream(PdfName.UF);

            if (stream == null)
            {
                return null;
            }

            try
            {
                return stream.GetBytes();
            }
            catch (Exception)
            {
                /* A stream with a filter that iText does not know. */
                try
                {
                    return stream.GetBytes(false);
                }
                catch (Exception)
                {
                    return null;
                }
            }
        }
    }
}
