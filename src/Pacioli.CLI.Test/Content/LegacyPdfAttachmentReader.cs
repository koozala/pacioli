using iText.Kernel.Pdf;
using Vit.FeRD.ExtendedDocument;

namespace Pacioli.CLI.Test.Content
{
    /*
     * The way that Pacioli took the invoice out of a PDF file before phase 3 of the
     * migration to En16931.
     *
     * The class walks through each object of the PDF file. It gives each stream to the
     * parser of the old library. If the call does not raise an exception, the stream
     * holds the invoice.
     *
     * THIS IS A COPY. It is not the code of the application any more; the application
     * uses Pacioli.Pdf.ERechnung.EInvoiceExtractor. The copy stays here as a control:
     * PdfAttachmentTests gives each PDF sample to both ways and compares the bytes. Thus
     * the new way must find the same invoice as the old way.
     *
     * The copy goes away in phase 7, together with the package Vit.FeRD.ExtendedDocument.
     * See doc/En16931-Migration.md.
     */
    public static class LegacyPdfAttachmentReader
    {
        /* The bytes of the invoice, or null if the file holds none. */
        public static byte[]? Read(string fileName)
        {
            try
            {
                PdfReader pdfReader = new PdfReader(fileName);
                PdfDocument pdfDoc = new PdfDocument(pdfReader);

                for (int i = 0; i < pdfDoc.GetNumberOfPdfObjects(); i++)
                {
                    var obj = pdfDoc.GetPdfObject(i);
                    if (obj != null && obj.IsStream())
                    {
                        byte[] data;
                        try
                        {
                            data = ((PdfStream)obj).GetBytes();
                        }
                        catch
                        {
                            data = ((PdfStream)obj).GetBytes(true);
                        }

                        MemoryStream mStream = new MemoryStream();
                        mStream.Write(data, 0, data.Length);
                        mStream.Seek(0, SeekOrigin.Begin);

                        try
                        {
                            /* If this does not raise an exception, it is the attachment. */
                            FeRDInvoice.Load(mStream);
                            return data;
                        }
                        catch
                        {
                        }
                    }
                }

                pdfDoc.Close();
                return null;
            }
            catch
            {
                return null;
            }
        }
    }
}
