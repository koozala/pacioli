using iText.Kernel.Pdf;
using Org.BouncyCastle.Crypto.Generators;
using s2industries.ZUGFeRD;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Pacioli.Pdf.zugferd
{
    public class ZugferdReader
    {
        string _fileName;

        public ZugferdReader(string fileName)
        {
            _fileName = fileName;
        }

        /*
         * This function uses an heuristic to find out if a PDF file has an attached ERechnung file.
         */
        public bool Read(out Stream? xmlStream)
        {
            xmlStream = null;
            try
            {
                PdfReader pdfReader = new PdfReader(_fileName);
                PdfDocument pdfDoc = new PdfDocument(pdfReader);

                for (int i = 0; i < pdfDoc.GetNumberOfPdfObjects(); i++)
                {
                    var obj = pdfDoc.GetPdfObject(i);
                    if (obj != null && obj.IsStream())
                    {
                        var s = (PdfStream)obj;
                        var t = s.KeySet();
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
                        xmlStream = mStream;

                        try
                        {
                            /* If this doesn't raise an exception, it is most probably the attachment we are looking for... 
                             * There may be better ways to get access to the attachment. TODO: Find out.
                             */
                            var desc = InvoiceDescriptor.Load(xmlStream);
                            xmlStream.Seek(0, SeekOrigin.Begin);
                            return true;
                        }
                        catch
                        {
                        }
                    }
                }
                pdfDoc.Close();
                xmlStream = null;
                return false;
            }
            catch
            {
                return false;
            }
        }

    }
}

