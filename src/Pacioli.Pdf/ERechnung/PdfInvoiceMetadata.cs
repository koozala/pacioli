using En16931.Conformance;
using iText.Kernel.Pdf;
using System;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;

namespace Pacioli.Pdf.ERechnung
{
    /*
     * The metadata of a PDF file that hold the declaration of an e-invoice.
     *
     * ZUGFeRD and Factur-X write their declaration into the XMP metadata of the PDF file,
     * with properties in one of three namespaces:
     *
     *   ZUGFeRD 1.0         urn:ferd:pdfa:CrossIndustryDocument:invoice:1p0#
     *   ZUGFeRD 2.0         urn:zugferd:pdfa:CrossIndustryDocument:invoice:2p0#
     *   ZUGFeRD 2.1+ and    urn:factur-x:pdfa:CrossIndustryDocument:invoice:1p0#
     *   Factur-X
     *
     * The properties are ConformanceLevel, DocumentType, DocumentFileName and Version.
     * XMP can write a property as an element or as an attribute of rdf:Description, thus
     * the class takes both forms.
     *
     * The class also reads the PDF/A identification (pdfaid:part and pdfaid:conformance),
     * the producer and the date of creation. For the producer and the date, the
     * information dictionary of the PDF file is the second source.
     */
    public sealed class PdfInvoiceMetadata
    {
        static readonly XNamespace PdfAId = "http://www.aiim.org/pdfa/ns/id/";
        static readonly XNamespace AdobePdf = "http://ns.adobe.com/pdf/1.3/";
        static readonly XNamespace Xmp = "http://ns.adobe.com/xap/1.0/";

        static readonly string[] InvoiceNamespaces =
        {
            DeclarationCheck.FacturXXmpNamespace,
            DeclarationCheck.Zugferd20XmpNamespace,
            DeclarationCheck.Zugferd1XmpNamespace
        };

        /* The namespace of the invoice properties, or null if the XMP has none. */
        public string? InvoiceNamespace { get; private set; }

        public string? ConformanceLevel { get; private set; }
        public string? DocumentType { get; private set; }
        public string? DocumentFileName { get; private set; }
        public string? Version { get; private set; }

        /* For example "3". */
        public string? PdfAPart { get; private set; }

        /* For example "B". */
        public string? PdfAConformance { get; private set; }

        public string? Producer { get; private set; }

        /* The date as the file writes it, for example "2024-11-12T10:15:00+01:00". */
        public string? CreationDate { get; private set; }

        /* True if the PDF file has XMP metadata. */
        public bool HasXmp { get; private set; }

        /* Reads the metadata. The method raises no exception. */
        public static PdfInvoiceMetadata Read(PdfDocument document)
        {
            var metadata = new PdfInvoiceMetadata();

            try
            {
                byte[]? xmp = document.GetXmpMetadataBytes();
                if (xmp != null && xmp.Length > 0)
                {
                    metadata.ReadXmp(xmp);
                }
            }
            catch (Exception)
            {
                /* XMP with a defect. The other values stay. */
            }

            try
            {
                PdfDocumentInfo info = document.GetDocumentInfo();
                metadata.Producer ??= Clean(info.GetProducer());
                metadata.CreationDate ??= Clean(info.GetMoreInfo("CreationDate"));
            }
            catch (Exception)
            {
                /* No information dictionary. */
            }

            return metadata;
        }

        void ReadXmp(byte[] xmp)
        {
            var settings = new XmlReaderSettings
            {
                DtdProcessing = DtdProcessing.Prohibit,
                XmlResolver = null
            };

            using var stream = new MemoryStream(xmp, writable: false);
            using var reader = XmlReader.Create(stream, settings);
            XDocument document = XDocument.Load(reader);
            HasXmp = true;

            foreach (string ns in InvoiceNamespaces)
            {
                XNamespace invoice = ns;
                string? level = Property(document, invoice + "ConformanceLevel");
                string? type = Property(document, invoice + "DocumentType");
                string? fileName = Property(document, invoice + "DocumentFileName");
                string? version = Property(document, invoice + "Version");

                if (level != null || type != null || fileName != null || version != null)
                {
                    InvoiceNamespace = ns;
                    ConformanceLevel = level;
                    DocumentType = type;
                    DocumentFileName = fileName;
                    Version = version;
                    break;
                }
            }

            PdfAPart = Property(document, PdfAId + "part");
            PdfAConformance = Property(document, PdfAId + "conformance");
            Producer = Property(document, AdobePdf + "Producer");
            CreationDate = Property(document, Xmp + "CreateDate");
        }

        /*
         * The value of a property: the text of an element, or the value of an attribute.
         * The description of a schema in pdfaExtension holds the namespace as text, not
         * as a property, thus it does not count.
         */
        static string? Property(XDocument document, XName name)
        {
            string? value = document.Descendants(name).Select(element => element.Value).FirstOrDefault()
                            ?? document.Descendants().Attributes(name).Select(attribute => attribute.Value).FirstOrDefault();
            return Clean(value);
        }

        static string? Clean(string? value)
        {
            return string.IsNullOrWhiteSpace(value) ? null : value.Trim();
        }
    }
}
