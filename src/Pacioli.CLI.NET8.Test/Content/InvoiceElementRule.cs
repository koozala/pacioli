using s2industries.ZUGFeRD;

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
        private readonly Func<InvoiceDescriptor, IEnumerable<ExpectedValue?>> _selector;

        private InvoiceElementRule(string name, Func<InvoiceDescriptor, IEnumerable<ExpectedValue?>> selector)
        {
            Name = name;
            _selector = selector;
        }

        public string Name { get; }

        /* Makes a rule for one or more text values. */
        public static InvoiceElementRule Text(string name, Func<InvoiceDescriptor, IEnumerable<string?>> select)
        {
            return new InvoiceElementRule(name, descriptor => select(descriptor).Select(ExpectedValue.FromText));
        }

        /* Makes a rule for one or more dates. */
        public static InvoiceElementRule Date(string name, Func<InvoiceDescriptor, IEnumerable<DateTime?>> select)
        {
            return new InvoiceElementRule(name, descriptor => select(descriptor).Select(ExpectedValue.FromDate));
        }

        /* Makes a rule for one or more amounts. */
        public static InvoiceElementRule Amount(string name, Func<InvoiceDescriptor, IEnumerable<decimal?>> select)
        {
            return new InvoiceElementRule(name, descriptor => select(descriptor).Select(value => ExpectedValue.FromAmount(value)));
        }

        /* Makes a rule that builds its values itself. Use it for a special written form. */
        public static InvoiceElementRule Custom(string name, Func<InvoiceDescriptor, IEnumerable<ExpectedValue?>> select)
        {
            return new InvoiceElementRule(name, select);
        }

        public IReadOnlyList<ElementCheckResult> Check(InvoiceDescriptor descriptor, PdfTextContent page)
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
