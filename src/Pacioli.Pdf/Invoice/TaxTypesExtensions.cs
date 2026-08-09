using Pacioli.Language.Resources;

namespace Pacioli.Pdf.Invoice
{
    /*
     * Gives the name of a tax type code. The model keeps the code as a string.
     * "VAT" becomes "MwSt" in German and "VAT" in English.
     */
    internal static class TaxTypesExtensions
    {
        public static string ToTaxTypeName(this string? taxType)
        {
            switch (taxType?.Trim())
            {
                case "VAT":
                    return Resources.MwSt;
                case null:
                    return string.Empty;
                default:
                    return taxType!.Trim();
            }
        }
    }
}
