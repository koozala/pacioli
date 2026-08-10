using iText.Layout;
using iText.Layout.Element;
using Pacioli.Language.Resources;
using System.Collections.Generic;
using Vit.FeRD.ExtendedDocument.Model;
using CodeLists = En16931.Codes.CodeLists;

namespace Pacioli.Pdf.Invoice.Sections
{
    /* The table with one row for each tax rate of the document. */
    internal sealed class TaxSummarySection : IInvoiceSection
    {
        public string Name => "Tax summary";

        public void Write(Document doc, InvoiceContext context)
        {
            doc.Add(new Paragraph($"{Resources.taxSums}:").SetMarginTop(20.0f));

            Table taxTab = new Table(6).SetFontSize(12).SetWidth(context.FullWidth);
            taxTab.AddHeaderCell($"{Resources.tax}");
            taxTab.AddHeaderCell($"{Resources.taxCategory}");
            taxTab.AddHeaderCell($"{Resources.taxBasis}");
            taxTab.AddHeaderCell($"{Resources.taxAmount}");
            taxTab.AddHeaderCell($"{Resources.exception}");
            taxTab.AddHeaderCell($"{Resources.allowanceChargeBasis}");

            foreach (var tax in context.Settlement?.ApplicableTradeTax ?? new List<TradeTax>())
            {
                taxTab.AddCell(new ItemCellRight($"{tax.RateApplicablePercent?.ToString("0.00")}"));
                taxTab.AddCell(new ItemCellRight($"{tax.TypeCode.ToTaxTypeName()} {tax.CategoryCode.ToTaxCategoryName()}"));
                /*
                 * The basis of the tax, and below it the part that comes from the
                 * positions. The second value exists only in some documents.
                 */
                taxTab.AddCell(new ItemCellRight(InvoiceText.FormatGroup(
                    $"{tax.BasisAmount?.Value}",
                    tax.LineTotalBasisAmount == null ? null : $"({Resources.Positionen}: {tax.LineTotalBasisAmount.Value})")));
                taxTab.AddCell(new ItemCellRight($"{tax.CalculatedAmount?.Value}"));
                taxTab.AddCell(new ItemCellRight($"{tax.ExemptionReason} {CodeText.Name(CodeLists.TaxExemptionReason, tax.ExemptionReasonCode)}"));
                taxTab.AddCell(new ItemCellRight($"{tax.AllowanceChargeBasisAmount?.Value}"));
            }

            doc.Add(taxTab);

            /*
             * The exchange rate for the tax. A document with a foreign currency gives
             * the rate between the currency of the invoice and the currency of the tax.
             */
            TradeCurrencyExchange? exchange = context.Settlement?.TaxApplicableTradeCurrencyExchange;
            if (exchange != null)
            {
                doc.Add(new Paragraph(InvoiceText.FormatGroup(
                    $"{Resources.Umrechnungskurs}:",
                    exchange.ConversionRate.ToString("0.#####"),
                    InvoiceText.FormatGroup(exchange.SourceCurrencyCode, "->", exchange.TargetCurrencyCode),
                    InvoiceText.DateOf(exchange.ConversionRateDateTime)?.ToString("d"))).SetFontSize(12));
            }
        }
    }
}
