using Pacioli.Language.Resources;
using System;
using System.Text;
using Vit.FeRD.ExtendedDocument.Model;

namespace Pacioli.Pdf.Invoice.Sections
{
    /* The text steps that more than one section needs. */
    internal static class InvoiceText
    {
        /* The value of an identifier, or null. */
        public static string? Text(Identifier? id) => id?.Value;

        /* The date of a CII date, or null if the document gives none. */
        public static DateOnly? DateOf(CiiDate? date) => date?.Date;

        /* The renderer selects the number of decimal places from the value. */
        public static string FormatQuantity(Quantity? quantity)
        {
            if (quantity == null)
            {
                return string.Empty;
            }

            return quantity.Value.ToString(DecimalExtensions.PrecFmt(quantity.Value));
        }

        public static void AppendReference(StringBuilder sb, string caption, ReferencedDocument? refdoc)
        {
            if (refdoc == null)
            {
                return;
            }

            string t = $"{caption}: {refdoc.IssuerAssignedId?.Value}";
            DateOnly? issued = DateOf(refdoc.FormattedIssueDateTime);
            if (issued != null)
            {
                t += $" {issued.Value.ToString("d")}";
            }
            sb.AppendLine(t);
        }

        public static void AppendAllowanceCharge(StringBuilder sb, TradeAllowanceCharge charge)
        {
            string zuschlag = charge.ChargeIndicator ? "Zuschlag" : "Abschlag";
            sb.AppendLine(
                $"{zuschlag}: {Fmt.ToStringWithUnit(charge.CalculationPercent, "0.0", "%")} " +
                $"{Fmt.ToString(charge.ActualAmount?.Value, "#,#0.00")} {charge.ActualAmount?.CurrencyId} " +
                $"{Fmt.ToStringWithName("Basis:", charge.BasisAmount?.Value, "#,#0.00")} {charge.Reason}");
        }

        public static void AppendContact(StringBuilder sb, TradeContact contact)
        {
            if (!string.IsNullOrWhiteSpace(contact.PersonName)) sb.AppendLine(contact.PersonName);
            if (!string.IsNullOrWhiteSpace(contact.DepartmentName)) sb.AppendLine(contact.DepartmentName);

            string? mail = contact.EmailUriUniversalCommunication?.UriId?.Value;
            if (!string.IsNullOrWhiteSpace(mail)) sb.AppendLine(mail);

            string? phone = contact.TelephoneUniversalCommunication?.CompleteNumber;
            if (!string.IsNullOrWhiteSpace(phone)) sb.AppendLine($"{Resources.phone}: {phone}");

            string? fax = contact.FaxUniversalCommunication?.CompleteNumber;
            if (!string.IsNullOrWhiteSpace(fax)) sb.AppendLine($"{Resources.fax}: {fax}");
        }

        public static void AddContentCodeLine(StringBuilder sb, Code? code)
        {
            if (!string.IsNullOrWhiteSpace(code?.Value))
            {
                if (sb.Length > 0) sb.Append(", ");
                sb.Append($"Inhaltscodierung: {code!.Value}");
            }
        }

        public static void AddSubjectCodeLine(StringBuilder sb, Code? code)
        {
            if (!string.IsNullOrWhiteSpace(code?.Value))
            {
                if (sb.Length > 0) sb.Append(", ");
                sb.Append($"Betreffcodierung: {code!.Value}");
            }
        }
    }
}
