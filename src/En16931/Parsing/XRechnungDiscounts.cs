using System.Globalization;
using System.Text.RegularExpressions;
using En16931.Model;
using En16931.Model.Extended;

namespace En16931.Parsing;

/*
 * The discounts (Skonto) of XRechnung.
 *
 * The core of EN 16931 has no element for a discount. Thus XRechnung puts each discount
 * as one line into the text of the payment terms (BT-20). Rule BR-DE-18 gives the form:
 *
 *   #SKONTO#TAGE=7#PROZENT=3.00#
 *   #SKONTO#TAGE=14#PROZENT=2.00#BASISBETRAG=100.00#
 *
 * TAGE is the number of days. The days count from the invoice date (BT-2). PROZENT is the
 * percentage. BASISBETRAG is optional: the amount that the percentage applies to. If it is
 * not there, the percentage applies to the amount due (BT-115). The words stay German in
 * each language of the invoice. The same form applies to CII and to UBL.
 *
 * This class takes these lines out of the text of each payment term and adds them to the
 * discounts of the term. The other lines stay in the text. A line that starts with
 * #SKONTO# but does not agree with the form also stays in the text, thus no data goes lost.
 *
 * The core (Invoice.PaymentTerms) keeps the full text, because it is the value of BT-20.
 */
internal static partial class XRechnungDiscounts
{
    private const string Marker = "#SKONTO#";

    [GeneratedRegex(
        @"^#SKONTO#TAGE=(?<days>\d+)#PROZENT=(?<percent>\d+(?:\.\d+)?)#(?:BASISBETRAG=(?<basis>-?\d+(?:\.\d+)?)#)?$",
        RegexOptions.CultureInvariant)]
    private static partial Regex DiscountLine();

    public static void Apply(Invoice invoice)
    {
        foreach (var term in invoice.Extensions?.PaymentTerms ?? [])
        {
            Apply(term, invoice.CurrencyCode);
        }
    }

    private static void Apply(PaymentTerm term, string? currencyCode)
    {
        var text = term.Description;
        if (text is null || !text.Contains(Marker, StringComparison.Ordinal)) return;

        var discounts = new List<PaymentTermCondition>();
        var otherLines = new List<string>();

        foreach (var line in text.Split('\n'))
        {
            var match = DiscountLine().Match(line.Trim());
            if (match.Success)
            {
                discounts.Add(ReadDiscount(match, currencyCode));
            }
            else
            {
                otherLines.Add(line.TrimEnd('\r'));
            }
        }

        if (discounts.Count == 0) return;

        term.Discounts.AddRange(discounts);

        var rest = string.Join("\n", otherLines).Trim();
        term.Description = rest.Length == 0 ? null : rest;
    }

    private static PaymentTermCondition ReadDiscount(Match match, string? currencyCode)
    {
        var basis = match.Groups["basis"];

        return new PaymentTermCondition
        {
            BasisPeriodDays = decimal.Parse(match.Groups["days"].Value, CultureInfo.InvariantCulture),
            Percentage = decimal.Parse(match.Groups["percent"].Value, CultureInfo.InvariantCulture),
            BasisAmount = basis.Success
                ? new Amount(decimal.Parse(basis.Value, NumberStyles.Number, CultureInfo.InvariantCulture),
                    currencyCode)
                : null
        };
    }
}
