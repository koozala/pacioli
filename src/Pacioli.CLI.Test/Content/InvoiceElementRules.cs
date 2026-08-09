using Vit.FeRD.ExtendedDocument.Model;

namespace Pacioli.CLI.Test.Content
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
     * For a value of each position, use LineItemText, LineItemAmount, LineItemQuantity,
     * LineItemPercent, or LineItem. These rules add the position number to the label.
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
     *
     * The data model comes from Vit.FeRD.ExtendedDocument. It follows the structure of
     * the Cross Industry Invoice, thus a value sits deeper than in a flat model.
     */
    public static class InvoiceElementRules
    {
        public static IReadOnlyList<InvoiceElementRule> All { get; } = new List<InvoiceElementRule>
        {
            InvoiceElementRule.Text("Invoice number",
                invoice => new[] { invoice.ExchangedDocument?.Id?.Value }),

            InvoiceElementRule.Date("Invoice date",
                invoice => new[] { ToDateTime(invoice.ExchangedDocument?.IssueDateTime?.Date) }),

            InvoiceElementRule.Text("Seller name",
                invoice => new[] { Agreement(invoice)?.SellerTradeParty?.Name }),

            InvoiceElementRule.Text("Buyer name",
                invoice => new[] { Agreement(invoice)?.BuyerTradeParty?.Name }),

            InvoiceElementRule.Text("Currency",
                invoice => new[] { Settlement(invoice)?.InvoiceCurrencyCode }),

            InvoiceElementRule.Amount("Tax basis amount",
                invoice => new[] { Summation(invoice)?.TaxBasisTotalAmount?.Value }),

            InvoiceElementRule.Amount("Tax total amount",
                invoice => new[] { Summation(invoice)?.TaxTotalAmount.FirstOrDefault()?.Value }),

            InvoiceElementRule.Amount("Grand total amount",
                invoice => new[] { Summation(invoice)?.GrandTotalAmount?.Value }),

            InvoiceElementRule.Amount("Payable amount",
                invoice => new[] { Summation(invoice)?.DuePayableAmount?.Value }),

            /*
             * The data of each position of the invoice. The renderer writes these values
             * to the item table. A rule adds the position number to each value, thus the
             * report tells you which position has the problem.
             */

            InvoiceElementRule.LineItemText("Line item name",
                item => item.SpecifiedTradeProduct?.Name),

            InvoiceElementRule.LineItemText("Line item seller assigned ID",
                item => item.SpecifiedTradeProduct?.SellerAssignedId?.Value),

            InvoiceElementRule.LineItemQuantity("Line item quantity",
                item => item.SpecifiedLineTradeDelivery?.BilledQuantity?.Value),

            InvoiceElementRule.LineItemPercent("Line item tax rate",
                item => item.SpecifiedLineTradeSettlement?.ApplicableTradeTax.FirstOrDefault()?.RateApplicablePercent),

            InvoiceElementRule.LineItemAmount("Line item net unit price",
                item => item.SpecifiedLineTradeAgreement?.NetPriceProductTradePrice?.ChargeAmount?.Value),

            InvoiceElementRule.LineItemAmount("Line item gross unit price",
                item => item.SpecifiedLineTradeAgreement?.GrossPriceProductTradePrice?.ChargeAmount?.Value),

            InvoiceElementRule.LineItemAmount("Line item total amount",
                item => item.SpecifiedLineTradeSettlement?.SpecifiedTradeSettlementLineMonetarySummation?.LineTotalAmount?.Value),
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

        /*
         * Gives the names that do not agree with a rule.
         *
         * A name with a spelling error would remove its rule without a message. Then the
         * content check could pass because it checks nothing. Thus the test reports these
         * names as a failure.
         */
        public static IReadOnlyList<string> FindUnknownNames(IReadOnlyList<string> names)
        {
            return names
                .Where(name => !All.Any(rule => string.Equals(name, rule.Name, StringComparison.OrdinalIgnoreCase)))
                .ToList();
        }

        private static HeaderTradeAgreement? Agreement(CrossIndustryInvoice invoice) =>
            invoice.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement;

        private static HeaderTradeSettlement? Settlement(CrossIndustryInvoice invoice) =>
            invoice.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement;

        private static TradeSettlementHeaderMonetarySummation? Summation(CrossIndustryInvoice invoice) =>
            Settlement(invoice)?.SpecifiedTradeSettlementHeaderMonetarySummation;

        /* The model gives a DateOnly. The rule takes a DateTime, thus the test converts it. */
        private static DateTime? ToDateTime(DateOnly? date) =>
            date?.ToDateTime(TimeOnly.MinValue);
    }
}
