using Pacioli.Language.Resources;

namespace Pacioli.Pdf.Invoice
{
    /*
     * Gives the name of a unit code, for example "H87" becomes "Anz.".
     *
     * The model of Vit.FeRD.ExtendedDocument keeps the unit code as a string. Thus this
     * class takes a string, and not an enumeration.
     */
    internal static class QuantityCodesExtensions
    {
        public static string ToUnit(this string? code)
        {
            switch (code?.Trim())
            {
                case "SEC":
                    return "Sekunde(n)";
                case "MMK":
                    return "mm^2";
                case "XTU":
                    return "Tube(n)";
                case "XTN":
                    return "Dose(n)";
                case "XBE":
                    return "Bund";
                case "XRO":
                    return "Rolle";
                case "XST":
                    return "Papierbogen";
                case "ANN":
                    return "Jahr(e)";
                case "NAR":
                    return "Anz.";
                case "C62":
                    return "Stk.";
                case "CMT":
                    return "cm";
                case "DAY":
                    return "Tag(e)";
                case "FF":
                    return "a";
                case "GRM":
                    return "Gramm";
                case "H87":
                    return "Anz.";
                case "HUR":
                    return "Std.";
                case "KGM":
                    return "kg";
                case "KMT":
                    return "km";
                case "KT":
                    return "Kit";
                case "KWH":
                    return "kWh";
                case "KWT":
                    return "kW";
                case "LS":
                    return "pausch.";
                case "LTR":
                    return "l";
                case "MIN":
                    return "min";
                case "MMT":
                    return "mm";
                case "MON":
                    return "Monat(e)";
                case "MTK":
                    return "m^2";
                case "MTQ":
                    return "m^3";
                case "MTR":
                    return "m";
                case "P1":
                    return "%";
                case "PR":
                    return "Paar";
                case "SET":
                    return "Set(s)";
                case "T3":
                    return "Tsd.";
                case "TNE":
                    return "t";
                case "WEE":
                    return "Woche(n)";
                case "XBA":
                    return "Fass";
                case "XBD":
                    return "Tf";
                case "XBG":
                    return "Beutel";
                case "XBJ":
                    return "Eimer";
                case "XBO":
                    return "Fl";
                case "XCI":
                    return "Kanister";
                case "XCT":
                    return "Kt";
                case "XPK":
                    return Resources.packaging;
                case "XPP":
                    return "lose";
                case "XPX":
                    return "Pal";
                case "XRD":
                    return "Stg";
                case "XSA":
                    return "Sack";
                default:
                    return string.Empty;
            }
        }
    }
}
