using s2industries.ZUGFeRD;

namespace Pacioli.Pdf.Invoice
{
    internal static class TaxCategoryCodesExtensions
    {
        public static string ToUnit(this TaxCategoryCodes category)
        {
            switch (category)
            {
                case TaxCategoryCodes.S:
                    return string.Empty;
                default:
                    return category.ToString();
            }
        }
    }
}
