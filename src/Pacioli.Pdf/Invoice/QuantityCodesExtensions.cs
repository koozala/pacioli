using Pacioli.Language.Resources;

namespace Pacioli.Pdf.Invoice
{
    /*
     * Gives the name of a unit code, for example "H87" becomes "Anz." in German and
     * "pcs" in English.
     *
     * The names sit in the resources. The key is "unit" with the code, for example
     * "unitH87". Thus a new unit needs a resource only, and no change of this class,
     * and the name follows the language of the user.
     */
    internal static class QuantityCodesExtensions
    {
        public const string ResourcePrefix = "unit";

        public static string ToUnit(this string? code)
        {
            string key = (code ?? string.Empty).Trim();

            if (key.Length == 0)
            {
                return string.Empty;
            }

            /* The packaging unit had a resource of its own before this class used the prefix. */
            if (key == "XPK")
            {
                return Resources.packaging;
            }

            return Resources.ResourceManager.GetString(ResourcePrefix + key, Resources.Culture) ?? string.Empty;
        }
    }
}
