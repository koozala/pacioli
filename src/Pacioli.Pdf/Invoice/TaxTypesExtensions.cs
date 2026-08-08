namespace Pacioli.Pdf.Invoice
{
    /* Gives the name of a tax type code. The model keeps the code as a string. */
    internal static class TaxTypesExtensions
    {
        public static string ToTaxTypeName(this string? taxType)
        {
            switch (taxType?.Trim())
            {
                case "VAT":
                    return "MwSt";
                case null:
                    return string.Empty;
                default:
                    return taxType!.Trim();
            }
        }
    }
}
