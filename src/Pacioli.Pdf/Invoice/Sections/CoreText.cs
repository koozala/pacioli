using En16931.Model;
using Pacioli.Language.Resources;
using System.Text;

namespace Pacioli.Pdf.Invoice.Sections
{
    /*
     * The text steps that work on the model of En16931.
     *
     * The migration moves a section from the old model to En16931. A text step of that
     * section moves with it, from InvoiceText to this class. When the last section is
     * moved, InvoiceText goes away and this class holds each step.
     *
     * See doc/En16931-Migration.md, phase 5.
     */
    internal static class CoreText
    {
        /* The values that are not empty, with one space between them. */
        public static string FormatGroup(params string?[] values)
        {
            string separator = string.Empty;
            StringBuilder sb = new StringBuilder();

            foreach (string? value in values)
            {
                if (!string.IsNullOrEmpty(value))
                {
                    sb.Append(separator);
                    sb.Append(value);
                    separator = " ";
                }
            }

            return sb.ToString();
        }

        /* "4.284,00 EUR" for an amount, or an empty text. */
        public static string FormatAmount(Amount? amount) =>
            amount == null ? string.Empty : FormatGroup(amount.Value.ToString("#,0.00"), amount.CurrencyCode);

        /* The renderer selects the number of decimal places from the value. */
        public static string FormatQuantity(Quantity? quantity) =>
            quantity == null
                ? string.Empty
                : quantity.Value.ToString(DecimalExtensions.PrecFmt(quantity.Value));

        /* The quantity with the name of its unit, for example "100 Anz.". */
        public static string FormatQuantityWithUnit(Quantity? quantity) =>
            quantity == null
                ? string.Empty
                : FormatGroup(FormatQuantity(quantity), quantity.UnitCode.ToUnit());

        /*
         * The full data of one contact person: the name, the department, the mail
         * address, the telephone, and the fax. Each part gets its own line.
         */
        public static void AppendContact(StringBuilder sb, Contact contact)
        {
            if (!string.IsNullOrWhiteSpace(contact.Name)) sb.AppendLine(contact.Name);
            if (!string.IsNullOrWhiteSpace(contact.Extensions?.DepartmentName))
                sb.AppendLine(contact.Extensions!.DepartmentName);
            if (!string.IsNullOrWhiteSpace(contact.Email)) sb.AppendLine(contact.Email);
            if (!string.IsNullOrWhiteSpace(contact.Telephone))
                sb.AppendLine($"{Resources.phone}: {contact.Telephone}");
            if (!string.IsNullOrWhiteSpace(contact.Extensions?.Fax))
                sb.AppendLine($"{Resources.fax}: {contact.Extensions!.Fax}");
        }

        public static string FormatContact(Contact contact)
        {
            StringBuilder sb = new StringBuilder();
            AppendContact(sb, contact);
            return sb.ToString();
        }

        /* The address in the form of a letter: the lines, then the place, then the country. */
        public static string FormatAddress(PostalAddress address)
        {
            StringBuilder sb = new StringBuilder();

            if (!string.IsNullOrWhiteSpace(address.AddressLine1)) sb.AppendLine(address.AddressLine1);
            if (!string.IsNullOrWhiteSpace(address.AddressLine2)) sb.AppendLine(address.AddressLine2);
            if (!string.IsNullOrWhiteSpace(address.AddressLine3)) sb.AppendLine(address.AddressLine3);
            if (!string.IsNullOrWhiteSpace(address.PostCode)) sb.Append(address.PostCode + " ");
            if (!string.IsNullOrWhiteSpace(address.City)) sb.Append(address.City);
            if (!string.IsNullOrWhiteSpace(address.CountrySubdivision)) sb.Append(" " + address.CountrySubdivision);
            if (!string.IsNullOrWhiteSpace(address.CountryCode)) sb.Append(" " + address.CountryCode);

            return sb.ToString();
        }
    }
}
