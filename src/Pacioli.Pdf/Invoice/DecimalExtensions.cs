using System;
using System.Text;

namespace Pacioli.Pdf.Invoice
{
    internal class DecimalExtensions
    {
        public static string PrecFmt(decimal value)
        {
            StringBuilder fmt0 = new StringBuilder();
            decimal workVal = value;

            int factor = 1;

            while (factor < 1000000)
            {
                if (workVal == Math.Floor(workVal))
                {
                    if (fmt0.Length == 0)
                    {
                        return "#,0";
                    }
                    else
                    {
                        return "#,0." + fmt0.ToString();
                    }
                }
                fmt0.Append("0");
                factor = factor * 10;
            }
            return "#,0." + fmt0.ToString();
        }
    }
}
