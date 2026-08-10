using System.Xml.Linq;
using En16931;
using Pacioli.Pdf.ERechnung;
using Vit.FeRD.ExtendedDocument;
using Vit.FeRD.ExtendedDocument.Model;

namespace Pacioli.CLI.Test.Content
{
    /*
     * Reads the invoice data of a sample document with Vit.FeRD.ExtendedDocument.
     *
     * THE LIBRARY HERE IS NOT THE LIBRARY OF THE RENDERER. The renderer reads with
     * En16931; this project keeps the old library as an independent control. Thus the
     * content check compares the two parsers, and it tests the parse step too.
     * See doc/En16931-Migration.md, phase 1.
     *
     * The old library reads CII only. For a UBL document the loader gives null, and the
     * test case then gives the result "ignored": a control needs two parsers, and only
     * one of them knows UBL.
     */
    public static class InvoiceLoader
    {
        /* The invoice, or null if the old library cannot read this syntax. */
        public static CrossIndustryInvoice? Load(string samplePath)
        {
            var extractor = new EInvoiceExtractor(samplePath);

            if (extractor.HasInvoice)
            {
                if (extractor.Syntax != InvoiceSyntax.Cii)
                {
                    return null;
                }

                return FeRDInvoice.Load(extractor.XmlDataStream!);
            }

            if (SyntaxOf(samplePath) != InvoiceSyntax.Cii)
            {
                return null;
            }

            return FeRDInvoice.Load(samplePath);
        }

        private static InvoiceSyntax SyntaxOf(string path)
        {
            try
            {
                return InvoiceReader.DetectSyntax(XDocument.Load(path, LoadOptions.None));
            }
            catch (System.Xml.XmlException)
            {
                return InvoiceSyntax.Unknown;
            }
        }
    }
}
