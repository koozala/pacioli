using En16931.Model;
using En16931.Model.Extended;
using iText.Layout;
using iText.Layout.Borders;
using iText.Layout.Element;
using Pacioli.Language.Resources;
using System.Collections.Generic;

namespace Pacioli.Pdf.Invoice.Sections
{
    /*
     * The summary of the amounts.
     *
     * The first rows show each payment that the buyer made before this invoice. The other
     * rows come from the totals of the document (BG-22).
     */
    internal sealed class TotalsSection : IInvoiceSection
    {
        public string Name => "Totals";

        public void Write(Document doc, InvoiceContext context)
        {
            doc.Add(new Paragraph($"{Resources.sumOverview}:").SetMarginTop(20.0f));

            Table sumTab = new Table(2).SetFontSize(12).SetWidth(context.FullWidth).SetBorder(Border.NO_BORDER);

            foreach (AdvancePayment advance in context.Core.Extensions?.AdvancePayments
                                               ?? (IReadOnlyList<AdvancePayment>)new List<AdvancePayment>())
            {
                sumTab.AddCell(new ItemCell(Resources.prepaid));
                sumTab.AddCell(new ItemCellRight($"{Fmt.ToString(advance.PaidAmount?.Value, "#,0.00")}"));
            }

            DocumentTotals totals = context.Core.Totals;

            AddRow(sumTab, Resources.Positionssumme, totals.LineTotalAmount);
            AddRow(sumTab, Resources.prepaid, totals.PaidAmount);
            AddRow(sumTab, Resources.allowance, totals.AllowanceTotalAmount);
            AddRow(sumTab, Resources.charge, totals.ChargeTotalAmount);
            AddRow(sumTab, Resources.taxBasis, totals.TaxExclusiveAmount);
            AddTaxRows(sumTab, totals);
            AddRow(sumTab, Resources.totalSum, totals.TaxInclusiveAmount);
            AddRow(sumTab, Resources.roundingAmount, totals.RoundingAmount);
            AddRow(sumTab, Resources.payableAmount, totals.DuePayableAmount);

            doc.Add(sumTab);
        }

        private static void AddRow(Table table, string caption, decimal? amount)
        {
            table.AddCell(new ItemCell(caption));
            table.AddCell(new ItemCellRight($"{Fmt.ToString(amount, "#,0.00")}"));
        }

        /*
         * The tax amounts. A document with a foreign currency gives one amount for the
         * currency of the invoice (BT-110) and one for the currency of the tax (BT-111).
         * Each amount gets its own row, and the caption names the currency of the amount.
         *
         * If the document gives one amount only, the row keeps the plain caption. Thus a
         * document with one currency looks as before.
         */
        private static void AddTaxRows(Table table, DocumentTotals totals)
        {
            var amounts = new List<Amount>();
            if (totals.TaxAmount != null) amounts.Add(totals.TaxAmount);
            if (totals.TaxAmountInAccountingCurrency != null) amounts.Add(totals.TaxAmountInAccountingCurrency);

            if (amounts.Count == 0)
            {
                AddRow(table, Resources.taxAmount, null);
                return;
            }

            foreach (Amount amount in amounts)
            {
                string caption = amounts.Count > 1 && !string.IsNullOrWhiteSpace(amount.CurrencyCode)
                    ? $"{Resources.taxAmount} ({amount.CurrencyCode})"
                    : Resources.taxAmount;

                AddRow(table, caption, amount.Value);
            }
        }
    }
}
