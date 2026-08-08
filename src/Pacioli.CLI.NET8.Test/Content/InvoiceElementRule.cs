using Vit.FeRD.ExtendedDocument.Model;

namespace Pacioli.CLI.NET8.Test.Content
{
    /*
     * One rule of the content check.
     *
     * A rule takes the invoice data and gives the values that must be visible on the
     * rendered PDF. A rule can give no value. Then the invoice has no data for this
     * element, and the test does not check it.
     *
     * To add a new element, add one entry to InvoiceElementRules.All.
     */
    public sealed class InvoiceElementRule
    {
        private readonly Func<CrossIndustryInvoice, IEnumerable<ExpectedValue?>> _selector;

        private InvoiceElementRule(string name, Func<CrossIndustryInvoice, IEnumerable<ExpectedValue?>> selector)
        {
            Name = name;
            _selector = selector;
        }

        public string Name { get; }

        /* Makes a rule for one or more text values. */
        public static InvoiceElementRule Text(string name, Func<CrossIndustryInvoice, IEnumerable<string?>> select)
        {
            return new InvoiceElementRule(name, descriptor => select(descriptor).Select(value => ExpectedValue.FromText(value)));
        }

        /* Makes a rule for one or more dates. */
        public static InvoiceElementRule Date(string name, Func<CrossIndustryInvoice, IEnumerable<DateTime?>> select)
        {
            return new InvoiceElementRule(name, descriptor => select(descriptor).Select(value => ExpectedValue.FromDate(value)));
        }

        /* Makes a rule for one or more amounts. */
        public static InvoiceElementRule Amount(string name, Func<CrossIndustryInvoice, IEnumerable<decimal?>> select)
        {
            return new InvoiceElementRule(name, descriptor => select(descriptor).Select(value => ExpectedValue.FromAmount(value)));
        }

        /* Makes a rule that builds its values itself. Use it for a special written form. */
        public static InvoiceElementRule Custom(string name, Func<CrossIndustryInvoice, IEnumerable<ExpectedValue?>> select)
        {
            return new InvoiceElementRule(name, select);
        }

        /*
         * Makes a rule for one value of each line item.
         *
         * The rule adds the position number to the label of each value. Thus the report
         * tells you which position of the invoice has the problem.
         *
         * The select function gets the line item and the position number. The position
         * number starts at 1, as on the rendered page.
         */
        public static InvoiceElementRule LineItem(string name, Func<SupplyChainTradeLineItem, string, ExpectedValue?> select)
        {
            return new InvoiceElementRule(name, invoice => LineItemsOf(invoice)
                .Select((item, index) => select(item, $"position {index + 1}")));
        }

        /* Makes a rule for a text value of each line item. */
        public static InvoiceElementRule LineItemText(string name, Func<SupplyChainTradeLineItem, string?> select)
        {
            return LineItem(name, (item, label) => ExpectedValue.FromText(select(item), label));
        }

        /* Makes a rule for an amount of each line item. */
        public static InvoiceElementRule LineItemAmount(string name, Func<SupplyChainTradeLineItem, decimal?> select)
        {
            return LineItem(name, (item, label) => ExpectedValue.FromAmount(select(item), label));
        }

        /* Makes a rule for a quantity of each line item. */
        public static InvoiceElementRule LineItemQuantity(string name, Func<SupplyChainTradeLineItem, decimal?> select)
        {
            return LineItem(name, (item, label) =>
            {
                decimal? value = select(item);
                return value.HasValue ? ExpectedValue.FromQuantity(value.Value, label) : null;
            });
        }

        /* Makes a rule for a tax rate of each line item. */
        public static InvoiceElementRule LineItemPercent(string name, Func<SupplyChainTradeLineItem, decimal?> select)
        {
            return LineItem(name, (item, label) =>
            {
                decimal? value = select(item);
                return value.HasValue ? ExpectedValue.FromPercent(value.Value, label) : null;
            });
        }

        /* The positions of an invoice, or an empty list if the invoice has none. */
        public static IEnumerable<SupplyChainTradeLineItem> LineItemsOf(CrossIndustryInvoice invoice)
        {
            return invoice.SupplyChainTradeTransaction?.IncludedSupplyChainTradeLineItem
                   ?? (IEnumerable<SupplyChainTradeLineItem>)Array.Empty<SupplyChainTradeLineItem>();
        }

        public IReadOnlyList<ElementCheckResult> Check(CrossIndustryInvoice descriptor, PdfTextContent page)
        {
            List<ExpectedValue?> values;

            try
            {
                values = _selector(descriptor).ToList();
            }
            catch (Exception error)
            {
                return new[]
                {
                    new ElementCheckResult
                    {
                        RuleName = Name,
                        Value = string.Empty,
                        Outcome = ElementCheckOutcome.Error,
                        Error = $"{error.GetType().Name}: {error.Message}"
                    }
                };
            }

            if (values.Count == 0 || values.All(value => value == null))
            {
                return new[]
                {
                    new ElementCheckResult
                    {
                        RuleName = Name,
                        Value = string.Empty,
                        Outcome = ElementCheckOutcome.NotApplicable
                    }
                };
            }

            return values
                .Where(value => value != null)
                .Select(value => new ElementCheckResult
                {
                    RuleName = Name,
                    Value = value!.Label,
                    Candidates = value.Candidates,
                    Outcome = page.ContainsAny(value.Candidates)
                        ? ElementCheckOutcome.Found
                        : ElementCheckOutcome.Missing
                })
                .ToList();
        }
    }
}
