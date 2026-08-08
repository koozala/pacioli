using s2industries.ZUGFeRD;

namespace Pacioli.CLI.NET8.Test.Content
{
    /*
     * The set of elements that must be visible on the rendered PDF.
     *
     * HOW TO ADD AN ELEMENT
     * Add one entry to the list All. Use one of these factory methods:
     *   InvoiceElementRule.Text   for a text value
     *   InvoiceElementRule.Date   for a date. It accepts the forms of more than one culture.
     *   InvoiceElementRule.Amount for an amount. It uses the format of the renderer.
     *   InvoiceElementRule.Custom for a special written form.
     *
     * A rule can give more than one value. The rule "Line item name" is an example.
     * The test then looks for each value.
     *
     * If a rule gives no value, the invoice has no data for this element. The test
     * does not check the element, and it does not fail. Thus a rule can also apply to
     * a profile with few data fields, for example MINIMUM.
     *
     * A rule must only expect data that the renderer writes to the page.
     * See InvoiceWriter.Write in the project Pacioli.Pdf.
     */
    public static class InvoiceElementRules
    {
        public static IReadOnlyList<InvoiceElementRule> All { get; } = new List<InvoiceElementRule>
        {
            InvoiceElementRule.Text("Invoice number",
                descriptor => new[] { descriptor.InvoiceNo }),

            InvoiceElementRule.Date("Invoice date",
                descriptor => new[] { descriptor.InvoiceDate }),

            InvoiceElementRule.Text("Seller name",
                descriptor => new[] { descriptor.Seller?.Name }),

            InvoiceElementRule.Text("Buyer name",
                descriptor => new[] { descriptor.Buyer?.Name }),

            InvoiceElementRule.Text("Currency",
                descriptor => new[] { CurrencyText(descriptor.Currency) }),

            InvoiceElementRule.Amount("Tax basis amount",
                descriptor => new[] { descriptor.TaxBasisAmount }),

            InvoiceElementRule.Amount("Tax total amount",
                descriptor => new[] { descriptor.TaxTotalAmount }),

            InvoiceElementRule.Amount("Grand total amount",
                descriptor => new[] { descriptor.GrandTotalAmount }),

            InvoiceElementRule.Amount("Payable amount",
                descriptor => new[] { descriptor.DuePayableAmount }),

            InvoiceElementRule.Text("Line item name",
                descriptor => descriptor.GetTradeLineItems().Select(item => item.Name)),
        };

        /* Gives the rules with the given names. An empty selection gives all rules. */
        public static IReadOnlyList<InvoiceElementRule> Select(IReadOnlyList<string> names)
        {
            if (names.Count == 0)
            {
                return All;
            }

            return All
                .Where(rule => names.Any(name => string.Equals(name, rule.Name, StringComparison.OrdinalIgnoreCase)))
                .ToList();
        }

        /* Gives the name of the currency, or null if the code is not known. */
        private static string? CurrencyText(CurrencyCodes currency)
        {
            return Enum.IsDefined(typeof(CurrencyCodes), currency) ? currency.ToString() : null;
        }
    }
}
