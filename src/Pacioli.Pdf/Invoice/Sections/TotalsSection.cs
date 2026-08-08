using iText.Layout;
using iText.Layout.Borders;
using iText.Layout.Element;
using Pacioli.Language.Resources;
using System.Collections.Generic;
using Vit.FeRD.ExtendedDocument.Model;

namespace Pacioli.Pdf.Invoice.Sections
{
    /*
     * The summary of the amounts.
     *
     * The first rows show each payment that the buyer made before this invoice. A row
     * with the mark "*****" is such a payment. The other rows come from the summation of
     * the document.
     */
    internal sealed class TotalsSection : IInvoiceSection
    {
        public string Name => "Totals";

        public void Write(Document doc, InvoiceContext context)
        {
            doc.Add(new Paragraph($"{Resources.sumOverview}:").SetMarginTop(20.0f));

            Table sumTab = new Table(2).SetFontSize(12).SetWidth(context.FullWidth).SetBorder(Border.NO_BORDER);

            foreach (var advance in context.Settlement?.SpecifiedAdvancePayment ?? new List<AdvancePayment>())
            {
                sumTab.AddCell(new ItemCell($"***** {Resources.prepaid}"));
                sumTab.AddCell(new ItemCellRight($"{Fmt.ToString(advance.PaidAmount?.Value, "#,0.00")}"));
            }

            var summation = context.Summation;

            AddRow(sumTab, Resources.prepaid, summation?.TotalPrepaidAmount);
            AddRow(sumTab, Resources.allowance, summation?.AllowanceTotalAmount);
            AddRow(sumTab, Resources.charge, summation?.ChargeTotalAmount);
            AddRow(sumTab, Resources.taxBasis, summation?.TaxBasisTotalAmount);
            AddRow(sumTab, Resources.taxAmount, FirstOrNull(summation?.TaxTotalAmount));
            AddRow(sumTab, Resources.totalSum, summation?.GrandTotalAmount);
            AddRow(sumTab, Resources.roundingAmount, summation?.RoundingAmount);
            AddRow(sumTab, Resources.payableAmount, summation?.DuePayableAmount);

            doc.Add(sumTab);
        }

        private static void AddRow(Table table, string caption, Amount? amount)
        {
            table.AddCell(new ItemCell(caption));
            table.AddCell(new ItemCellRight($"{Fmt.ToString(amount?.Value, "#,0.00")}"));
        }

        /*
         * The summation can hold more than one tax amount. A document with a foreign
         * currency holds one amount for each currency. The page shows the first amount.
         */
        private static Amount? FirstOrNull(IList<Amount>? amounts)
        {
            return amounts != null && amounts.Count > 0 ? amounts[0] : null;
        }
    }
}
