using Pacioli.Language.Resources;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pacioli.Pdf.Invoice
{
    /*
     * Gives the text of a document type code, for example "380" or "387".
     *
     * The model of Vit.FeRD.ExtendedDocument keeps the code as a string, thus this class
     * reads the text directly from the resources. Each text has the key "type" with the
     * code, for example "type380". To add a text for a new code, add a resource with this
     * name. A change of this class is not necessary.
     */
    internal class TypeCodeMapping
    {
        public const string ResourcePrefix = "type";

        public static string Map(string? typeCode)
        {
            if (string.IsNullOrWhiteSpace(typeCode))
            {
                return string.Empty;
            }

            string? text = Resources.ResourceManager.GetString(
                ResourcePrefix + typeCode!.Trim(), Resources.Culture);

            return text ?? string.Empty;
        }
    }
}
