using En16931.Model;
using En16931.Model.Extended;
using iText.Layout;
using iText.Layout.Element;
using Pacioli.Language.Resources;
using CodeLists = En16931.Codes.CodeLists;

namespace Pacioli.Pdf.Invoice.Sections
{
    /* The table with one row for each tax rate of the document (BG-23). */
    internal sealed class TaxSummarySection : IInvoiceSection
    {
        public string Name => "Tax summary";

        public void Write(Document doc, InvoiceContext context)
        {
            doc.Add(new Paragraph($"{Resources.taxSums}:").SetMarginTop(20.0f));

            /*
             * KeepTogether holds the table on one page. Without it, a page break can go
             * through a row, and then a long text of an exemption stands with one part on
             * each page, with the head of the table between the two parts.
             */
            Table taxTab = new Table(6).SetFontSize(12).SetWidth(context.FullWidth).SetKeepTogether(true);
            taxTab.AddHeaderCell($"{Resources.tax}");
            taxTab.AddHeaderCell($"{Resources.taxCategory}");
            taxTab.AddHeaderCell($"{Resources.taxBasis}");
            taxTab.AddHeaderCell($"{Resources.taxAmount}");
            taxTab.AddHeaderCell($"{Resources.exception}");
            taxTab.AddHeaderCell($"{Resources.allowanceChargeBasis}");

            foreach (VatBreakdown vat in context.Core.VatBreakdown)
            {
                /* The full tax of the entry, where the document gives more than the core. */
                TaxDetail? detail = vat.Detail;

                taxTab.AddCell(new ItemCellRight($"{vat.CategoryRate?.ToString("0.00")}"));
                taxTab.AddCell(new ItemCellRight(
                    $"{detail?.TypeCode.ToTaxTypeName()} {vat.CategoryCode.ToTaxCategoryName()}"));

                /*
                 * The basis of the tax, and below it the part that comes from the
                 * positions. The second value exists only in some documents.
                 */
                taxTab.AddCell(new ItemCellRight(CoreText.FormatGroup(
                    $"{vat.TaxableAmount}",
                    detail?.LineTotalBasisAmount == null
                        ? null
                        : $"({Resources.Positionen}: {detail.LineTotalBasisAmount.Value})")));

                taxTab.AddCell(new ItemCellRight($"{vat.TaxAmount}"));
                taxTab.AddCell(new ItemCellRight(
                    $"{vat.ExemptionReasonText} {CodeText.Name(CodeLists.TaxExemptionReason, vat.ExemptionReasonCode)}"));
                taxTab.AddCell(new ItemCellRight($"{detail?.AllowanceChargeBasisAmount?.Value}"));
            }

            doc.Add(taxTab);

            /*
             * The exchange rate for the tax. A document with a foreign currency gives
             * the rate between the currency of the invoice and the currency of the tax.
             */
            CurrencyExchange? exchange = context.Core.Extensions?.TaxCurrencyExchange;
            if (exchange != null)
            {
                doc.Add(new Paragraph(CoreText.FormatGroup(
                    $"{Resources.Umrechnungskurs}:",
                    exchange.Rate?.ToString("0.#####"),
                    CoreText.FormatGroup(exchange.SourceCurrencyCode, "->", exchange.TargetCurrencyCode),
                    exchange.RateDate?.ToString("d"))).SetFontSize(12));
            }
        }
    }
}
