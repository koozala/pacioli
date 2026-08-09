using iText.Layout;
using Pacioli.Language.Resources;
using System;
using System.Collections.Generic;
using System.Text;
using Vit.FeRD.ExtendedDocument.Model;

namespace Pacioli.Pdf.Invoice.Sections
{
    /*
     * The payment terms: the text, the due date, and the number of days.
     * A document can hold more than one term. Each term gets its own box.
     */
    internal sealed class PaymentTermsSection : IInvoiceSection
    {
        public string Name => "Payment terms";

        public void Write(Document doc, InvoiceContext context)
        {
            foreach (var pt in context.Settlement?.SpecifiedTradePaymentTerms ?? new List<TradePaymentTerms>())
            {
                StringBuilder sb = new StringBuilder();
                if (!string.IsNullOrWhiteSpace(pt.Description)) sb.Append(pt.Description);

                DateOnly? dueDate = InvoiceText.DateOf(pt.DueDateDateTime);
                if (dueDate != null) sb.Append($"\n{Resources.dueDate}: {dueDate.Value.ToString("d")}");

                /*
                 * The discount terms (Skonto): the percentage, the amount of the
                 * discount, the amount that it applies to, the number of days, and the
                 * day from which the days count.
                 */
                TradePaymentDiscountTerms? discount = pt.ApplicableTradePaymentDiscountTerms;
                if (discount != null)
                {
                    string terms = InvoiceText.FormatGroup(
                        discount.CalculationPercent.HasValue
                            ? discount.CalculationPercent.Value.ToString("0.00") + "%"
                            : null,
                        InvoiceText.FormatAmount(discount.ActualDiscountAmount),
                        discount.BasisAmount == null
                            ? null
                            : $"{Resources.basis}: {InvoiceText.FormatAmount(discount.BasisAmount)}",
                        discount.BasisPeriodMeasure == null
                            ? null
                            : $"{discount.BasisPeriodMeasure.Value} {Resources.days}",
                        InvoiceText.DateOf(discount.BasisDateTime)?.ToString("d"));

                    if (terms.Length > 0)
                    {
                        sb.Append($"\n{Resources.Skonto}: {terms}");
                    }
                }

                if (sb.Length > 0)
                {
                    doc.Add(new PartyBox($"{Resources.dueDate}", sb.ToString()));
                }
            }
        }
    }
}
