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
        public static InvoiceWriter Open(string fileName, string attachmentLocation, out bool isZugferd)
        {
            ZugferdReader zr = new ZugferdReader(fileName);
            Stream? data;

            isZugferd = zr.Read(out data);
            if (isZugferd)
            {
                var writer = new InvoiceWriter(data!, attachmentLocation);
                data!.Close();
                return writer;
            }
            else
            {
                return new InvoiceWriter(fileName, attachmentLocation);
            }

        }
    }
}
