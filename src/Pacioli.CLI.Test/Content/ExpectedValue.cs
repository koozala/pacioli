using System.Globalization;

namespace Pacioli.CLI.Test.Content
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

        /*
         * The largest number of decimal places that the renderer uses for a quantity.
         * See the class DecimalExtensions in Pacioli.Pdf.
         */
        public const int MaximumQuantityDecimals = 6;

        public static ExpectedValue? FromText(string? value, string? labelPrefix = null)
        {
            string text = (value ?? string.Empty).Trim();
            return text.Length == 0 ? null : new ExpectedValue(MakeLabel(labelPrefix, text), new[] { text });
        }

        public static ExpectedValue? FromDate(DateTime? value, string? labelPrefix = null)
        {
            if (!value.HasValue)
            {
                return null;
            }

            var candidates = Cultures
                .Select(culture => value.Value.ToString("d", culture))
                .Distinct(StringComparer.Ordinal)
                .ToList();

            string label = value.Value.ToString("d", CultureInfo.CurrentCulture);
            return new ExpectedValue(MakeLabel(labelPrefix, label), candidates);
        }

        /*
         * Builds the forms of an amount.
         * The renderer uses the format "#,0.00". See the class Fmt in Pacioli.Pdf.
         */
        public static ExpectedValue? FromAmount(decimal? value, string? labelPrefix = null, string format = "#,0.00")
        {
            if (!value.HasValue)
            {
                return null;
            }

            var candidates = Cultures
                .Select(culture => value.Value.ToString(format, culture))
                .Distinct(StringComparer.Ordinal)
                .ToList();

            string label = value.Value.ToString(format, CultureInfo.CurrentCulture);
            return new ExpectedValue(MakeLabel(labelPrefix, label), candidates);
        }

        /*
         * Builds the forms of a quantity.
         *
         * The renderer selects the number of decimal places from the value. Thus this
         * method makes one form for each number of decimal places from 0 to 6. It keeps
         * only the forms that show the value exactly. A form that rounds the value is not
         * correct, and it can also agree with another number on the page.
         */
        public static ExpectedValue FromQuantity(decimal value, string? labelPrefix = null)
        {
            var candidates = new List<string>();

            for (int decimals = 0; decimals <= MaximumQuantityDecimals; decimals++)
            {
                if (Math.Round(value, decimals) != value)
                {
                    continue;
                }

                string format = decimals == 0 ? "#,0" : "#,0." + new string('0', decimals);
                candidates.AddRange(Cultures.Select(culture => value.ToString(format, culture)));
            }

            string label = value.ToString("0.############", CultureInfo.CurrentCulture);
            return new ExpectedValue(
                MakeLabel(labelPrefix, label),
                candidates.Distinct(StringComparer.Ordinal).ToList());
        }

        /*
         * Builds the forms of a tax rate.
         * The renderer uses the format "0.00" and adds a percent sign directly after it.
         */
        public static ExpectedValue FromPercent(decimal value, string? labelPrefix = null)
        {
            var candidates = Cultures
                .Select(culture => value.ToString("0.00", culture) + "%")
                .Distinct(StringComparer.Ordinal)
                .ToList();

            string label = value.ToString("0.00", CultureInfo.CurrentCulture) + "%";
            return new ExpectedValue(MakeLabel(labelPrefix, label), candidates);
        }

        private static string MakeLabel(string? prefix, string value)
        {
            return string.IsNullOrEmpty(prefix) ? value : $"{prefix}: {value}";
        }
    }
}
