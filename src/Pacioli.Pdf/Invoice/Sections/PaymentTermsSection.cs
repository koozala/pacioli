using En16931.Model.Extended;
using iText.Layout;
using Pacioli.Language.Resources;
using System.Text;

namespace Pacioli.Pdf.Invoice.Sections
{
    /*
     * The payment terms: the text, the due date, and the discount.
     * A document can hold more than one term. Each term gets its own box.
     */
    internal sealed class PaymentTermsSection : IInvoiceSection
    {
        public string Name => "Payment terms";

        public void Write(Document doc, InvoiceContext context)
        {
            foreach (PaymentTerm term in context.Core.Extensions?.PaymentTerms ?? [])
            {
                StringBuilder sb = new StringBuilder();

                if (!string.IsNullOrWhiteSpace(term.Description)) sb.Append(term.Description);

                if (term.DueDate != null)
                {
                    sb.Append($"\n{Resources.dueDate}: {term.DueDate.Value.ToString("d")}");
                }

                /*
                 * The discount terms (Skonto): the percentage, the amount of the
                 * discount, the amount that it applies to, the number of days, and the
                 * day from which the days count.
                 */
                string terms = FormatCondition(term.Discount);
                if (terms.Length > 0)
                {
                    sb.Append($"\n{Resources.Skonto}: {terms}");
                }

                if (sb.Length > 0)
                {
                    doc.Add(new PartyBox($"{Resources.dueDate}", sb.ToString()));
                }
            }
        }

        /* A discount or a penalty of a payment term. A part that the document does not give makes no text. */
        internal static string FormatCondition(PaymentTermCondition? condition)
        {
            if (condition == null)
            {
                return string.Empty;
            }

            return CoreText.FormatGroup(
                condition.Percentage.HasValue ? condition.Percentage.Value.ToString("0.00") + "%" : null,
                CoreText.FormatAmount(condition.Amount),
                condition.BasisAmount == null
                    ? null
                    : $"{Resources.basis}: {CoreText.FormatAmount(condition.BasisAmount)}",
                condition.BasisPeriodDays == null
                    ? null
                    : $"{condition.BasisPeriodDays.Value} {Resources.days}",
                condition.BasisDate?.ToString("d"));
        }
    }
}
