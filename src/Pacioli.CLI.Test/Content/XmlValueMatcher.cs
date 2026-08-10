using System.Globalization;

namespace Pacioli.CLI.Test.Content
{
    /*
     * Decides if a value of the XML input is visible on the rendered PDF.
     *
     * The renderer does not always write a value in the same form as the XML input:
     *
     *   - A date comes as "20200305" in a CII document, and as "2020-03-05" in a UBL
     *     document. The renderer writes "05.03.2020".
     *   - An amount comes as "198.00" in the XML. The renderer writes "198,00".
     *   - A quantity comes as "20.0000" in the XML. The renderer writes "20".
     *
     * Thus the matcher makes the forms that the renderer can use, and it looks for each
     * of them.
     */
    public static class XmlValueMatcher
    {
        /*
         * The forms of a date. CII gives 8 digits with the format code 102; UBL gives the
         * form of ISO 8601. The renderer writes the date in the form of the culture.
         */
        public const string DateFormat = "yyyyMMdd";

        public static readonly string[] DateFormats = { DateFormat, "yyyy-MM-dd" };

        public static bool IsVisible(string value, PdfTextContent page)
        {
            return BuildCandidates(value).Any(page.Contains);
        }

        /* Makes all forms of a value that the renderer can write. */
        public static IReadOnlyList<string> BuildCandidates(string value)
        {
            string text = value.Trim();
            var candidates = new List<string> { text };

            if (TryParseDate(text, out DateTime date))
            {
                candidates.AddRange(ExpectedValue.Cultures.Select(culture => date.ToString("d", culture)));
            }

            if (TryParseNumber(text, out decimal number))
            {
                ExpectedValue? amount = ExpectedValue.FromAmount(number);
                if (amount != null)
                {
                    candidates.AddRange(amount.Candidates);
                }

                candidates.AddRange(ExpectedValue.FromQuantity(number).Candidates);
            }

            return candidates.Distinct(StringComparer.Ordinal).ToList();
        }

        public static bool TryParseDate(string text, out DateTime date)
        {
            return DateTime.TryParseExact(text, DateFormats, CultureInfo.InvariantCulture,
                DateTimeStyles.None, out date);
        }

        public static bool TryParseNumber(string text, out decimal number)
        {
            return decimal.TryParse(text, NumberStyles.Number, CultureInfo.InvariantCulture, out number);
        }
    }
}
