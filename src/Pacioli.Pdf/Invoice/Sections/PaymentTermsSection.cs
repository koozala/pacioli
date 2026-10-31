using En16931.Model.Extended;
using iText.Layout;
using Pacioli.Language.Resources;
using System;
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

                void AddLine(string line)
                {
                    if (sb.Length > 0) sb.Append('\n');
                    sb.Append(line);
                }

                if (!string.IsNullOrWhiteSpace(term.Description)) AddLine(term.Description);

                if (term.DueDate != null)
                {
                    AddLine($"{Resources.dueDate}: {term.DueDate.Value.ToString("d")}");
                }

                /*
                 * The discount terms (Skonto): the percentage, the amount of the
                 * discount, the amount that it applies to, the number of days, and the
                 * day from which the days count. A term can give more than one discount,
                 * each discount gets its own line.
                 */
                foreach (PaymentTermCondition discount in term.Discounts)
                {
                    string terms = FormatDiscount(discount, context.Core.IssueDate);
                    if (terms.Length > 0)
                    {
                        AddLine($"{Resources.Skonto}: {terms}");
                    }
                }

                if (sb.Length > 0)
                {
                    doc.Add(new PartyBox($"{Resources.dueDate}", sb.ToString()));
                }
            }
        }

        /*
         * A discount, and the last day on which the buyer can take it off. The days count
         * from the day of the discount. If the discount has no day, they count from the
         * invoice date (BT-2), as XRechnung specifies.
         */
        internal static string FormatDiscount(PaymentTermCondition discount, DateOnly? issueDate)
        {
            string text = FormatCondition(discount);
            DateOnly? start = discount.BasisDate ?? issueDate;

            if (text.Length == 0 || start == null || discount.BasisPeriodDays is not decimal days ||
                days != decimal.Truncate(days) || days < 0 || days > 3650)
            {
                return text;
            }

            DateOnly lastDay = start.Value.AddDays((int)days);
            return CoreText.FormatGroup(text, $"({Resources.until} {lastDay.ToString("d")})");
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
