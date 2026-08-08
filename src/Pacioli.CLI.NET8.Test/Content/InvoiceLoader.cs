using Pacioli.Pdf.ERechnung;
using s2industries.ZUGFeRD;

namespace Pacioli.CLI.NET8.Test.Content
{
    /*
     * Reads the invoice data of a sample document.
     *
     * The loader uses the same steps as the CLI tool. A ZUGFeRD PDF file holds the XML
     * data as an attachment. An XML file holds the data directly.
     *
     * Note: the test reads the data with the same library as the renderer. Thus the
     * content check tests the render step. It does not test the parse step.
     */
    public static class InvoiceLoader
    {
        public static InvoiceDescriptor Load(string samplePath)
        {
            var extractor = new ZugferdExtractor(samplePath);

            if (extractor.IsZugferdFile && extractor.XmlDataStream != null)
            {
                return InvoiceDescriptor.Load(extractor.XmlDataStream);
            }

            return InvoiceDescriptor.Load(samplePath);
        }
    }
}
