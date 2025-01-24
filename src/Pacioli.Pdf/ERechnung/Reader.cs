using Pacioli.Pdf.Invoice;
using Pacioli.Pdf.zugferd;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Pacioli.Pdf.ERechnung
{
    public class Reader
    {

        public static InvoiceWriter Open(string fileName, string attachmentLocation, out bool isZugferd, out byte[]? xmlData)
        {
            ZugferdReader zr = new ZugferdReader(fileName);
            Stream? data;

            isZugferd = zr.Read(out data);
            if (isZugferd)
            {
                xmlData = new byte[data!.Length];
                data.Read(xmlData, 0, xmlData.Length);
                data.Seek(0, SeekOrigin.Begin);

                var writer = new InvoiceWriter(data!, attachmentLocation, fileName);
                data!.Close();
                return writer;
            }
            else
            {
                xmlData = null;
                return new InvoiceWriter(fileName, attachmentLocation);
            }

        }
    }
}
