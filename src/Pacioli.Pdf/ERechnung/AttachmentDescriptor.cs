using System;
using System.Collections.Generic;
using System.Text;

namespace Pacioli.Pdf.ERechnung
{
    /*
     * One attachment of an invoice.
     *
     * The class holds the data itself. Thus the application does not need the model of
     * the parser to show an attachment or to write it to a file.
     */
    public class AttachmentDescriptor
    {
        /* The name of the file, for example "Aufmass.pdf". */
        public string FileName { get; set; } = "";

        /* The name of the document, if the invoice gives one. */
        public string? Name { get; set; }

        /*
         * The number that the issuer gave to the document, for example "LV 1.1.1.1.1.".
         * An invoice can name a document without the data of its file. Then this number
         * is the only way to find the document.
         */
        public string? ReferenceId { get; set; }

        /* The content of the file, or null if the invoice holds a reference only. */
        public byte[]? Data { get; set; }

        /* The media type, for example "application/pdf". */
        public string? MimeType { get; set; }
    }
}
