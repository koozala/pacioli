using En16931.Model;
using En16931.Model.Extended;
using Pacioli.Language.Resources;
using System;
using System.Text;
using CodeLists = En16931.Codes.CodeLists;

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

        /* One line for a document that the invoice names: the number, the position, the day. */
        public static void AppendReference(StringBuilder sb, string caption, ReferencedDocument? document)
        {
            if (document == null)
            {
                return;
            }

            string pos = document.LineId == null ? string.Empty : $" ({document.LineId})";
            string t = $"{caption}: {document.Reference} {pos}";

            if (document.IssueDate != null)
            {
                t += $" {document.IssueDate.Value.ToString("d")}";
            }

            sb.AppendLine(t);
        }

        /* One line for an allowance or a charge. */
        public static void AppendAllowanceCharge(StringBuilder sb, AllowanceCharge charge)
        {
            string zuschlag = charge.IsCharge ? Resources.Zuschlag : Resources.Abschlag;
            sb.AppendLine(
                $"{zuschlag}: {Fmt.ToStringWithUnit(charge.Percentage, "0.0", "%")} " +
                $"{Fmt.ToString(charge.Amount, "#,#0.00")} " +
                $"{Fmt.ToStringWithName($"{Resources.basis}:", charge.BaseAmount, "#,#0.00")} {charge.Reason}");
        }

        /*
         * One tax of a position or of the document: the rate with its type and its
         * category, the amount, the basis amounts, the reason of an exemption, and the
         * day of the tax.
         *
         * A part that the document does not give makes no text. The text ends with a line
         * break, because the caller appends it to a text that holds one item per line.
         */
        public static string FormatTax(TaxDetail tax)
        {
            string rate = FormatGroup(
                tax.RatePercent.HasValue ? tax.RatePercent.Value.ToString("0.00") + "%" : null,
                tax.TypeCode.ToTaxTypeName(),
                tax.CategoryCode.ToTaxCategoryName());

            string details = FormatGroup(
                tax.CalculatedAmount == null ? null : $"{Resources.money}: {FormatAmount(tax.CalculatedAmount)}",
                tax.BasisAmount == null ? null : $"{Resources.basis}: {FormatAmount(tax.BasisAmount)}",
                tax.LineTotalBasisAmount == null
                    ? null
                    : $"{Resources.BasisDerPositionen}: {FormatAmount(tax.LineTotalBasisAmount)}",
                tax.AllowanceChargeBasisAmount == null
                    ? null
                    : $"{Resources.BasisDerZuschlaegeUndAbschlaege}: {FormatAmount(tax.AllowanceChargeBasisAmount)}",
                FormatGroup(tax.ExemptionReason, tax.ExemptionReasonCode),
                tax.TaxPointDate?.ToString("d"));

            string all = FormatGroup(rate, details);

            return all.Length == 0 ? string.Empty : $"{Resources.tax}: {all}{Environment.NewLine}";
        }

        /*
         * The full text of a party: the name, the free text, the numbers, the address,
         * the tax numbers, the legal organisation, the contacts, and the electronic
         * address. Each part gets its own line.
         *
         * A part that the party does not give makes no line. The text has no empty line
         * at its end, thus the caller can put the text after a caption.
         */
        public static string FormatParty(Party party)
        {
            StringBuilder sb = new StringBuilder();

            void Add(string? text)
            {
                if (!string.IsNullOrWhiteSpace(text))
                {
                    sb.AppendLine(text!.TrimEnd());
                }
            }

            Add(party.Name);
            Add(party.AdditionalLegalInformation);

            foreach (Identifier id in party.Identifiers)
            {
                Add(id.Value);
            }

            foreach (Identifier id in party.Extensions?.GlobalIdentifiers ?? [])
            {
                Add(FormatGroup(CodeText.Name(CodeLists.IdentifierScheme, id.SchemeId), id.Value));
            }

            if (party.PostalAddress != null)
            {
                Add(FormatAddress(party.PostalAddress));
            }

            foreach (Identifier registration in party.Extensions?.TaxRegistrations ?? [])
            {
                Add($"{registration.SchemeId}: {registration.Value}");
            }

            Add(FormatLegalOrganization(party));

            foreach (Contact contact in party.Extensions?.Contacts ?? [])
            {
                Add(FormatContact(contact));
            }

            if (party.Extensions == null && party.Contact != null)
            {
                Add(FormatContact(party.Contact));
            }

            if (party.ElectronicAddress != null)
            {
                Add(FormatGroup(party.ElectronicAddress.Value, party.Extensions?.ElectronicAddressNumber));
            }

            return sb.ToString().TrimEnd();
        }

        /* The legal organisation of a party: its name, its address, and its identifier. */
        public static string FormatLegalOrganization(Party party)
        {
            StringBuilder sb = new StringBuilder();

            if (!string.IsNullOrWhiteSpace(party.TradingName)) sb.AppendLine(party.TradingName);
            if (party.Extensions?.LegalOrganizationAddress != null)
                sb.AppendLine(FormatAddress(party.Extensions.LegalOrganizationAddress));
            if (party.LegalRegistrationIdentifier != null)
                sb.AppendLine(party.LegalRegistrationIdentifier.Value);

            return sb.ToString();
        }

        /* The full text of a document that the invoice names. */
        public static string FormatReferencedDocument(ReferencedDocument document, bool hasAttachment)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine($"{Resources.Referenz}: {document.Reference}");
            sb.AppendLine($"{Resources.attachment}: {(hasAttachment ? Resources.Ja : Resources.Nein)}");
            if (document.Name != null) sb.AppendLine($"{document.Name}");
            if (document.IssueDate != null)
                sb.AppendLine($"{Resources.docDate}: {document.IssueDate.Value.ToString("d")}");
            if (document.Uri != null) sb.AppendLine($"URI: {document.Uri}");
            if (document.ReferenceTypeCode != null)
                sb.AppendLine($"{Resources.Typ}: {CodeText.Name(CodeLists.ReferenceType, document.ReferenceTypeCode)}");
            if (document.LineId != null) sb.AppendLine($"{Resources.Position}: {document.LineId}");

            return sb.ToString();
        }

        /*
         * A payment that the buyer made before this invoice: the amount, the day of the
         * payment, the tax in the amount, and the invoice of the payment.
         */
        public static string FormatAdvancePayment(AdvancePayment payment)
        {
            StringBuilder s1 = new StringBuilder();

            string paid = FormatAmount(payment.PaidAmount);
            s1.AppendLine(paid.Length > 0 ? $"{Resources.Anzahlung}: {paid}" : $"{Resources.Anzahlung}:");

            if (payment.ReceivedDate != null)
            {
                s1.AppendLine($"{Resources.ErhaltenAm}: {payment.ReceivedDate.Value.ToString("d")}");
            }

            foreach (TaxDetail tax in payment.IncludedTax)
            {
                s1.AppendLine(FormatGroup(
                    $"{Resources.EnthalteneSteuer}:",
                    tax.RatePercent.HasValue ? tax.RatePercent.Value.ToString("0.00") + "%" : null,
                    tax.TypeCode.ToTaxTypeName(),
                    tax.CategoryCode.ToTaxCategoryName(),
                    FormatAmount(tax.CalculatedAmount),
                    tax.BasisAmount == null ? null : $"{Resources.basis}: " + FormatAmount(tax.BasisAmount)));
            }

            if (payment.Invoice != null)
            {
                s1.AppendLine(FormatGroup(
                    $"{Resources.RechnungDerAnzahlung}:",
                    payment.Invoice.Reference,
                    payment.Invoice.Name,
                    payment.Invoice.IssueDate?.ToString("d")));
            }

            return s1.ToString().TrimEnd();
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
