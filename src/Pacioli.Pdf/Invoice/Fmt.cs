
namespace Pacioli.Pdf.Invoice
{
    internal class Fmt
    {
        public static string ToString(decimal? x, string fmtString)
        {
            if (x.HasValue)
            {
                return x.Value.ToString(fmtString);
            }
            else
            {
                return "--";
            }
        }

        public static string ToStringWithUnit(decimal? x, string fmtString, string unit)
        {
            if (x.HasValue)
            {
                return $"{x.Value.ToString(fmtString)}{unit}";
            }
            else
            {
                return string.Empty;
            }
        }

        public static string ToStringWithName(string name, decimal? x, string fmtString)
        {
            if (x.HasValue)
            {
                return $"{name} {x.Value.ToString(fmtString)}";
            }
            else
            {
                return string.Empty;
            }
        }
    }
}
