using Pacioli.Language.Resources;
using s2industries.ZUGFeRD;

namespace Pacioli.Pdf.Invoice
{
    internal static class QuantityCodesExtensions
    {
        public static string ToUnit(this QuantityCodes c)
        {
            switch (c)
            {
                case QuantityCodes.SEC:
                    return "Sekunde(n)";
                case QuantityCodes.MMK:
                    return "mm^2";
                case QuantityCodes.XTU:
                    return "Tube(n)";
                case QuantityCodes.XTN:
                    return "Dose(n)";
                case QuantityCodes.XBE:
                    return "Bund";
                case QuantityCodes.XRO:
                    return "Rolle";
                case QuantityCodes.XST:
                    return "Papierbogen";
                case QuantityCodes.ANN:
                    return "Jahr(e)";
                case QuantityCodes.NAR:
                    return "Anz.";
                case QuantityCodes.C62:
                    return "Stk.";
                case QuantityCodes.CMT:
                    return "cm";
                case QuantityCodes.DAY:
                    return "Tag(e)";
                case QuantityCodes.FF:
                    return "a";
                case QuantityCodes.GRM:
                    return "Gramm";
                case QuantityCodes.H87:
                    return "Anz.";
                case QuantityCodes.HAR:
                    return "ha";
                case QuantityCodes.HUR:
                    return "Std.";
                case QuantityCodes.KGM:
                    return "kg";
                case QuantityCodes.KMT:
                    return "km";
                case QuantityCodes.KT:
                    return "Kit";
                case QuantityCodes.KTM:
                    return "km";
                case QuantityCodes.KWH:
                    return "kWh";
                case QuantityCodes.KWT:
                    return "kW";
                case QuantityCodes.LS:
                    return "pausch.";
                case QuantityCodes.LTR:
                    return "l";
                case QuantityCodes.MIN:
                    return "min";
                case QuantityCodes.MMT:
                    return "mm";
                case QuantityCodes.MON:
                    return "Monat(e)";
                case QuantityCodes.MTK:
                    return "m^2";
                case QuantityCodes.MTQ:
                    return "m^3";
                case QuantityCodes.MTR:
                    return "m";
                case QuantityCodes.P1:
                    return "%";
                case QuantityCodes.PR:
                    return "Paar";
                case QuantityCodes.SET:
                    return "Set(s)";
                case QuantityCodes.T3:
                    return "Tsd.";
                case QuantityCodes.TNE:
                    return "t";
                case QuantityCodes.WEE:
                    return "Woche(n)";
                case QuantityCodes.XBA:
                    return "Fass";
                case QuantityCodes.XBD:
                    return "Tf";
                case QuantityCodes.XBG:
                    return "Beutel";
                case QuantityCodes.XBJ:
                    return "Eimer";
                case QuantityCodes.XBO:
                    return "Fl";
                case QuantityCodes.XCI:
                    return "Kanister";
                case QuantityCodes.XCT:
                    return "Kt";
                case QuantityCodes.XPK:
                    return Resources.packaging;
                case QuantityCodes.XPP:
                    return "lose";
                case QuantityCodes.XPX:
                    return "Pal";
                case QuantityCodes.XRD:
                    return "Stg";
                case QuantityCodes.XSA:
                    return "Sack";
                default:
                    return "-";
            }
        }
    }
}
