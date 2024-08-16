using s2industries.ZUGFeRD;

namespace Pacioli.Pdf.Invoice
{
    internal static class TaxTypesExtensions
    {
        public static string ToUnit(this TaxTypes tax)
        {
            switch (tax)
            {
                case TaxTypes.VAT:
                    return "MwSt";
                default:
                    return tax.ToString();
            }
        }
    }
}
