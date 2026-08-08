using iText.Layout;
using iText.Layout.Element;
using Pacioli.Language.Resources;
using System.Collections.Generic;
using Vit.FeRD.ExtendedDocument.Model;

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
                taxTab.AddCell(new ItemCellRight($"{tax.BasisAmount?.Value}"));
                taxTab.AddCell(new ItemCellRight($"{tax.CalculatedAmount?.Value}"));
                taxTab.AddCell(new ItemCellRight($"{tax.ExemptionReason} {tax.ExemptionReasonCode?.Value}"));
                taxTab.AddCell(new ItemCellRight($"{tax.AllowanceChargeBasisAmount?.Value}"));
            }

            doc.Add(taxTab);
        }
    }
}
