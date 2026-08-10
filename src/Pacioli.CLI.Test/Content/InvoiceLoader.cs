using Pacioli.Pdf.ERechnung;
using Vit.FeRD.ExtendedDocument;
using Vit.FeRD.ExtendedDocument.Model;

namespace Pacioli.CLI.Test.Content
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
        public static CrossIndustryInvoice Load(string samplePath)
        {
            var extractor = new EInvoiceExtractor(samplePath);

            if (extractor.HasInvoice)
            {
                return FeRDInvoice.Load(extractor.XmlDataStream!);
            }

            return FeRDInvoice.Load(samplePath);
        }
    }
}
