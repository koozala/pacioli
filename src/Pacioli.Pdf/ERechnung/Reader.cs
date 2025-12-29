using Pacioli.Pdf.Invoice;
using Pacioli.Pdf.zugferd;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Pacioli.Pdf.ERechnung
{
    public class Reader
    {
        public InvoiceWriter Open(string fileName, string attachmentLocation, out bool isZugferd, Stream xmlData)
        {
            ZugferdReader zr = new ZugferdReader(fileName);
            Stream? data = null;

            isZugferd = zr.Read(out data);
            if (isZugferd)
            {
                var writer = new InvoiceWriter(data!, attachmentLocation, fileName);

                data!.Seek(0, SeekOrigin.Begin);
                data!.CopyTo(xmlData);
                data!.Close();
                xmlData.Seek(0, SeekOrigin.Begin);
                return writer;
            }
            else
            {
                // It's an XML file
                using (var fstream = new FileStream(fileName, FileMode.Open, FileAccess.Read))
                {
                    fstream.CopyTo(xmlData);
                    xmlData.Seek(0, SeekOrigin.Begin);
                }
                return new InvoiceWriter(fileName, attachmentLocation);
            }
        }

    }
}
