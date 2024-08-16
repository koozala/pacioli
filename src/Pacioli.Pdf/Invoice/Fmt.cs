
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
    }
}
