using s2industries.ZUGFeRD;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pacioli.Pdf.ERechnung
{
    public class AttachmentDescriptor
    {
        public string FileName { get; set; } = "";
        public string FullPath { get; set; } = "";
        public AdditionalReferencedDocument? ReferencedDocument { get; set; }
    }
}
