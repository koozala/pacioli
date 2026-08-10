using Pacioli.Language.Resources;
using System.Globalization;
using Vit.FeRD.ExtendedDocument.Model;

/*
 * The two libraries both have a class CodeList and a class CodeLanguage. Thus each name
 * needs its own alias. The code lists come from En16931; the model still comes from
 * Vit.FeRD.ExtendedDocument.
 */
using CodeLanguage = En16931.Codes.CodeLanguage;
using CodeList = En16931.Codes.CodeList;

namespace Pacioli.Pdf.Invoice
{
    /*
     * Gives the clear text of a code.
     *
     * The project En16931 holds the code lists of the specification in the class
     * CodeLists. Each list gives the name of a code in German and in English.
     * If a list has no German name for a code, it gives the English name.
     *
     * A list does not hold a name for each code: the list of the units holds 2162 codes
     * and 48 German names, and the list of the note subjects knows "AAI" but not "ST3".
     * Thus each method here gives the code itself back if the list holds no name. A page
     * with a code is better than a page with an empty place.
     */
    internal static class CodeText
    {
        /* The language of the code lists follows the language of the resources. */
        public static CodeLanguage Language =>
            (Resources.Culture ?? CultureInfo.CurrentUICulture).TwoLetterISOLanguageName == "de"
                ? CodeLanguage.German
                : CodeLanguage.English;

        /* The name of the code, or the code itself. */
        public static string Name(CodeList list, string? code)
        {
            string key = (code ?? string.Empty).Trim();

            if (key.Length == 0)
            {
                return string.Empty;
            }

            string? name = list.GetName(key, Language);

            return string.IsNullOrWhiteSpace(name) ? key : name!;
        }

        public static string Name(CodeList list, Code? code) => Name(list, code?.Value);

        public static string Name(CodeList list, Identifier? id) => Name(list, id?.Value);

        /*
         * The code and its name, for example "AAI (Allgemeine Information)". If the list
         * holds no name, the method gives the code alone.
         *
         * Use this form where the code itself has a meaning for the reader, for example
         * for the codes of a note.
         */
        public static string Describe(CodeList list, string? code)
        {
            string key = (code ?? string.Empty).Trim();

            if (key.Length == 0)
            {
                return string.Empty;
            }

            string? name = list.GetName(key, Language);

            return string.IsNullOrWhiteSpace(name) ? key : $"{key} ({name})";
        }

        public static string Describe(CodeList list, Code? code) => Describe(list, code?.Value);
    }
}
