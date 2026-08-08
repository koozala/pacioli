namespace Pacioli.Pdf.Invoice
{
    /*
     * Gives the name of a tax category code. The model keeps the code as a string.
     * The category "S" is the usual rate, thus the page does not show it.
     */
    internal static class TaxCategoryCodesExtensions
    {
        public static string ToTaxCategoryName(this string? category)
        {
            switch (category?.Trim())
            {
                case "S":
                    return string.Empty;
                case null:
                    return string.Empty;
                default:
                    return category!.Trim();
            }
        }
    }
}
