using Pacioli.Language.Resources;
using CodeLists = En16931.Codes.CodeLists;

namespace Pacioli.Pdf.Invoice
{
    /*
     * Gives the name of a tax type code. The model keeps the code as a string.
     *
     * "VAT" keeps the short form from the resources, because it stands in a column of
     * the item table: "MwSt" in German and "VAT" in English. The code list of the
     * specification gives "Umsatzsteuer", and that text is too wide for the column.
     *
     * Each other code takes its name from the code list, thus the page shows a text and
     * not a code.
     */
    internal static class TaxTypesExtensions
    {
        public static string ToTaxTypeName(this string? taxType)
        {
            string code = (taxType ?? string.Empty).Trim();

            if (code.Length == 0)
            {
                return string.Empty;
            }

            if (code == "VAT")
            {
                return Resources.MwSt;
            }

            return CodeText.Name(CodeLists.TaxType, code);
        }
    }
}
