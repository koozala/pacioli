using System.Globalization;

namespace Pacioli.CLI.NET8.Test.Content
{
    /*
     * One value that must be visible on the rendered PDF.
     *
     * A value can have more than one written form. A date is an example: the renderer
     * uses the culture of the machine. Thus the test accepts each form in Candidates.
     */
    public sealed class ExpectedValue
    {
        public ExpectedValue(string label, IReadOnlyList<string> candidates)
        {
            Label = label;
            Candidates = candidates;
        }

        /* The value in its raw form. The report shows this text. */
        public string Label { get; }

        /* The accepted written forms. The test looks for each of them. */
        public IReadOnlyList<string> Candidates { get; }

        /* The cultures that the test accepts for a date or for an amount. */
        public static IReadOnlyList<CultureInfo> Cultures { get; } = new[]
        {
            CultureInfo.CurrentCulture,
            CultureInfo.InvariantCulture,
            CultureInfo.GetCultureInfo("de-DE"),
            CultureInfo.GetCultureInfo("en-US")
        };

        public static ExpectedValue? FromText(string? value)
        {
            string text = (value ?? string.Empty).Trim();
            return text.Length == 0 ? null : new ExpectedValue(text, new[] { text });
        }

        public static ExpectedValue? FromDate(DateTime? value)
        {
            if (!value.HasValue)
            {
                return null;
            }

            var candidates = Cultures
                .Select(culture => value.Value.ToString("d", culture))
                .Distinct(StringComparer.Ordinal)
                .ToList();

            return new ExpectedValue(value.Value.ToString("d", CultureInfo.CurrentCulture), candidates);
        }

        /*
         * Builds the forms of an amount.
         * The renderer uses the format "#,0.00". See the class Fmt in Pacioli.Pdf.
         */
        public static ExpectedValue? FromAmount(decimal? value, string format = "#,0.00")
        {
            if (!value.HasValue)
            {
                return null;
            }

            var candidates = Cultures
                .Select(culture => value.Value.ToString(format, culture))
                .Distinct(StringComparer.Ordinal)
                .ToList();

            return new ExpectedValue(value.Value.ToString(format, CultureInfo.CurrentCulture), candidates);
        }
    }
}
