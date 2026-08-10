using Vit.FeRD.ExtendedDocument.Model;
using CodeLists = En16931.Codes.CodeLists;

namespace Pacioli.Pdf.Invoice
{
    /*
     * Gives the name of a tax category code. The model keeps the code as a string.
     *
     * The category "S" is the usual rate, thus the page does not show it. Each other
     * category takes its name from the code list of the specification: the page showed
     * "K" before, and it now shows "Steuerfreie innergemeinschaftliche Lieferung".
     */
    internal static class TaxCategoryCodesExtensions
    {
        public static string ToTaxCategoryName(this string? category)
        {
            string code = (category ?? string.Empty).Trim();

            if (code.Length == 0 || code == "S")
            {
                return string.Empty;
            }

            return CodeText.Name(CodeLists.TaxCategory, code);
        }
    }
}
