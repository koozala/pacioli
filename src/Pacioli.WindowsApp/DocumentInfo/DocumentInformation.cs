using En16931;
using En16931.Conformance;
using Pacioli.Pdf.ERechnung;
using Pacioli.Pdf.Invoice;
using System.Xml;

namespace Pacioli.WindowsApp.DocumentInfo
{
    /*
     * The meta information of an open document: the file, the XML, the declared standard
     * and profile, and for a PDF file the XMP metadata.
     *
     * The class only collects values. The catalog and the comparison of the declarations
     * are in the library En16931 (namespace En16931.Conformance), the reader of the XMP
     * metadata is in Pacioli.Pdf.
     *
     * All values are DECLARATIONS of the document. They do not tell that the document
     * keeps the rules: only a validation can tell that.
     */
    public sealed class DocumentInformation
    {
        public string FilePath { get; init; } = string.Empty;

        /* The size of the file in bytes, or null if the file is not there any more. */
        public long? FileSize { get; init; }

        /* True for a PDF file with an embedded invoice, false for an XML file. */
        public bool IsPdf { get; init; }

        /* The name of the embedded XML file, for example "factur-x.xml". */
        public string? EmbeddedXmlName { get; init; }

        public InvoiceSyntax Syntax { get; init; }

        /* The name of the root element as the file writes it, for example "rsm:CrossIndustryInvoice". */
        public string? RootElement { get; init; }

        public string? RootNamespace { get; init; }

        /* The namespace declarations of the root element: prefix (empty for the default) and URI. */
        public IReadOnlyList<KeyValuePair<string, string>> Namespaces { get; init; } = Array.Empty<KeyValuePair<string, string>>();

        /* BT-24 and its meaning. */
        public SpecificationInfo Specification { get; init; } = SpecificationCatalog.Identify(null);

        /* BT-23, as the document writes it. */
        public string? BusinessProcessType { get; init; }

        public PdfInvoiceMetadata? PdfMetadata { get; init; }

        public IReadOnlyList<DeclarationIssue> Issues { get; init; } = Array.Empty<DeclarationIssue>();

        /*
         * Collects the information. The XML stream of the writer goes back to position 0,
         * because the XML view reads it after this method.
         */
        public static DocumentInformation Create(string filePath, InvoiceWriter writer)
        {
            Stream xml = writer.GetXml();
            (string? rootElement, string? rootNamespace, List<KeyValuePair<string, string>> namespaces) = ReadRoot(xml);

            PdfInvoiceMetadata? metadata = writer.PdfMetadata;
            bool isPdf = writer.IsZugferd;

            var input = new DeclarationInput
            {
                SpecificationIdentifier = writer.Invoice.SpecificationIdentifier,
                XmlRootNamespace = rootNamespace,
                IsPdf = isPdf,
                EmbeddedFileName = writer.EmbeddedXmlName,
                XmpNamespace = metadata?.InvoiceNamespace,
                XmpConformanceLevel = metadata?.ConformanceLevel,
                XmpDocumentFileName = metadata?.DocumentFileName,
                PdfAPart = metadata?.PdfAPart
            };

            return new DocumentInformation
            {
                FilePath = filePath,
                FileSize = SizeOf(filePath),
                IsPdf = isPdf,
                EmbeddedXmlName = writer.EmbeddedXmlName,
                Syntax = writer.Invoice.Syntax,
                RootElement = rootElement,
                RootNamespace = rootNamespace,
                Namespaces = namespaces,
                Specification = SpecificationCatalog.Identify(writer.Invoice.SpecificationIdentifier),
                BusinessProcessType = writer.Invoice.BusinessProcessType,
                PdfMetadata = metadata,
                Issues = DeclarationCheck.Check(input)
            };
        }

        /* Reads the root element only. The rest of the XML stays unread. */
        static (string?, string?, List<KeyValuePair<string, string>>) ReadRoot(Stream xml)
        {
            var namespaces = new List<KeyValuePair<string, string>>();
            long position = xml.Position;

            try
            {
                xml.Seek(0, SeekOrigin.Begin);
                var settings = new XmlReaderSettings { DtdProcessing = DtdProcessing.Prohibit, XmlResolver = null, CloseInput = false };
                using var reader = XmlReader.Create(xml, settings);

                if (reader.MoveToContent() != XmlNodeType.Element)
                {
                    return (null, null, namespaces);
                }

                string name = reader.Name;
                string ns = reader.NamespaceURI;

                while (reader.MoveToNextAttribute())
                {
                    if (reader.Prefix == "xmlns")
                    {
                        namespaces.Add(new KeyValuePair<string, string>(reader.LocalName, reader.Value));
                    }
                    else if (reader.Name == "xmlns")
                    {
                        namespaces.Add(new KeyValuePair<string, string>(string.Empty, reader.Value));
                    }
                }

                return (name, ns, namespaces);
            }
            catch (XmlException)
            {
                return (null, null, namespaces);
            }
            finally
            {
                xml.Seek(position, SeekOrigin.Begin);
            }
        }

        static long? SizeOf(string filePath)
        {
            try
            {
                return new FileInfo(filePath).Length;
            }
            catch (Exception ex) when (ex is IOException || ex is UnauthorizedAccessException)
            {
                return null;
            }
        }
    }
}
