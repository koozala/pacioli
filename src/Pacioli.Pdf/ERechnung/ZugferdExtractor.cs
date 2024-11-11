using Pacioli.Pdf.Invoice;
using Pacioli.Pdf.zugferd;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Pacioli.Pdf.ERechnung
{
    public class ZugferdExtractor
    {
        public bool IsZugferdFile { get; set; } = false;
        private Stream? _xmlDataStream = null;
        public Stream? XmlDataStream { get { return _xmlDataStream; } }

        public ZugferdExtractor(string fileName)
        {
            ZugferdReader reader = new ZugferdReader(fileName);
            IsZugferdFile = reader.Read(out _xmlDataStream);
        }

    }
}
