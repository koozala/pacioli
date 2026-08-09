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

        public static void AppendReference(StringBuilder sb, string caption, ReferencedDocument? refdoc)
        {
            if (refdoc == null)
            {
                return;
            }

            string pos = refdoc.LineId == null ? string.Empty : $" ({refdoc.LineId})";
            string t = $"{caption}: {refdoc.IssuerAssignedId?.Value} {pos}";
            DateOnly? issued = DateOf(refdoc.FormattedIssueDateTime);
            if (issued != null)
            {
                t += $" {issued.Value.ToString("d")}";
            }
            sb.AppendLine(t);
        }

        public static void AppendAllowanceCharge(StringBuilder sb, TradeAllowanceCharge charge)
        {
            string zuschlag = charge.ChargeIndicator ? Resources.Zuschlag : Resources.Abschlag;
            sb.AppendLine(
                $"{zuschlag}: {Fmt.ToStringWithUnit(charge.CalculationPercent, "0.0", "%")} " +
                $"{Fmt.ToString(charge.ActualAmount?.Value, "#,#0.00")} {charge.ActualAmount?.CurrencyId} " +
                $"{Fmt.ToStringWithName($"{Resources.basis}:", charge.BasisAmount?.Value, "#,#0.00")} {charge.Reason}");
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

        public static string FormatTradeContact(TradeContact contact)
        {
            StringBuilder sb = new StringBuilder();
            AppendContact(sb, contact);
            return sb.ToString();
        }

        public static string FormatAddress(TradeAddress addr)
        {
            StringBuilder sb = new StringBuilder();
            if (!string.IsNullOrWhiteSpace(addr.LineOne)) sb.AppendLine(addr.LineOne);
            if (!string.IsNullOrWhiteSpace(addr.LineTwo)) sb.AppendLine(addr.LineTwo);
            if (!string.IsNullOrWhiteSpace(addr.LineThree)) sb.AppendLine(addr.LineThree);
            if ((addr.PostcodeCode != null)) sb.Append(addr.PostcodeCode.ToString() + " ");
            if (!string.IsNullOrWhiteSpace(addr.CityName)) sb.Append(addr.CityName);
            if (!string.IsNullOrWhiteSpace(addr.CountrySubDivisionName)) sb.Append(" " + addr.CountrySubDivisionName);
            if (!string.IsNullOrWhiteSpace(addr.CountryId)) sb.Append(" " + addr.CountryId);
            return sb.ToString();
        }

        /*
         * The full text of a party: the name, the free text, the numbers, the address,
         * the tax numbers, the legal organisation, the contacts, and the electronic
         * address. Each part gets its own line.
         *
         * A part that the party does not give makes no line. The text has no empty line
         * at its end, thus the caller can put the text after a caption.
         *
         * The role code does not come to the page. It is a code, and the page shows the
         * role in the caption of the caller, for example "Lieferung an".
         */
        public static string FormatTradeParty(TradeParty party)
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
            Add(party.Description);

            foreach (Identifier? id in party.Id)
            {
                Add(Text(id));
            }

            foreach (Identifier? id in party.GlobalId)
            {
                Add(FormatGroup(id?.SchemeId, Text(id)));
            }

            if (party.PostalTradeAddress != null)
            {
                Add(FormatAddress(party.PostalTradeAddress));
            }

            foreach (TaxRegistration? reg in party.SpecifiedTaxRegistration)
            {
                if (!string.IsNullOrWhiteSpace(reg?.Id?.Value))
                {
                    Add($"{reg!.Id!.SchemeId}: {reg.Id.Value}");
                }
            }

            if (party.SpecifiedLegalOrganization != null)
            {
                Add(FormatLegalOrg(party.SpecifiedLegalOrganization));
            }

            foreach (TradeContact? contact in party.DefinedTradeContact)
            {
                if (contact != null)
                {
                    Add(FormatTradeContact(contact));
                }
            }

            if (party.UriUniversalCommunication != null)
            {
                Add(FormatGroup(Text(party.UriUniversalCommunication.UriId),
                                party.UriUniversalCommunication.CompleteNumber));
            }

            return sb.ToString().TrimEnd();
        }

        /* "4.284,00 EUR" for an amount, or an empty text. */
        public static string FormatAmount(Amount? amount)
        {
            if (amount == null)
            {
                return string.Empty;
            }

            return FormatGroup(amount.Value.ToString("#,0.00"), amount.CurrencyId);
        }

        /*
         * A payment that the buyer made before this invoice: the amount, the day of the
         * payment, the tax in the amount, and the invoice of the payment.
         *
         * The caller writes this text as one item without a caption. Thus the text names
         * each part itself. The text has no empty line at its end.
         */
        public static string FormatAdvancePayment(AdvancePayment advancePayment)
        {
            StringBuilder s1 = new StringBuilder();

            string paid = FormatAmount(advancePayment.PaidAmount);
            s1.AppendLine(paid.Length > 0 ? $"{Resources.Anzahlung}: {paid}" : $"{Resources.Anzahlung}:");

            DateOnly? received = DateOf(advancePayment.FormattedReceivedDateTime);
            if (received != null)
            {
                s1.AppendLine($"{Resources.ErhaltenAm}: {received.Value.ToString("d")}");
            }

            foreach (TradeTax? tax in advancePayment.IncludedTradeTax)
            {
                if (tax == null)
                {
                    continue;
                }

                s1.AppendLine(FormatGroup(
                    $"{Resources.EnthalteneSteuer}:",
                    tax.RateApplicablePercent.HasValue ? tax.RateApplicablePercent.Value.ToString("0.00") + "%" : null,
                    tax.TypeCode.ToTaxTypeName(),
                    tax.CategoryCode.ToTaxCategoryName(),
                    FormatAmount(tax.CalculatedAmount),
                    tax.BasisAmount == null ? null : $"{Resources.basis}: " + FormatAmount(tax.BasisAmount)));
            }

            ReferencedDocument? invoice = advancePayment.InvoiceSpecifiedReferencedDocument;
            if (invoice != null)
            {
                s1.AppendLine(FormatGroup(
                    $"{Resources.RechnungDerAnzahlung}:",
                    Text(invoice.IssuerAssignedId),
                    invoice.Name,
                    DateOf(invoice.FormattedIssueDateTime)?.ToString("d")));
            }

            return s1.ToString().TrimEnd();
        }

        public static string FormatLegalOrg(LegalOrganization org)
        {
            StringBuilder sb = new StringBuilder();
            if (!string.IsNullOrWhiteSpace(org.TradingBusinessName)) sb.AppendLine(org.TradingBusinessName);
            if (org.PostalTradeAddress != null) sb.AppendLine(FormatAddress(org.PostalTradeAddress));
            if (org.Id != null) sb.AppendLine(InvoiceText.Text(org.Id));
            return sb.ToString();
        }

        public static string FormatReferencedDocument(ReferencedDocument doc)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine($"{Resources.Referenz}: {InvoiceText.Text(doc.IssuerAssignedId)}");
            sb.AppendLine($"{Resources.attachment}: {(doc.AttachmentBinaryObject == null ? Resources.Nein : Resources.Ja)}");
            if (doc.Name != null) sb.AppendLine($"{doc.Name}");
            if (doc.FormattedIssueDateTime != null) sb.AppendLine($"{Resources.docDate}: {InvoiceText.DateOf(doc.FormattedIssueDateTime)?.ToString("d")}");
            if (doc.UriId != null) sb.AppendLine($"URI: {InvoiceText.Text(doc.UriId)}");
            if (doc.ReferenceTypeCode != null) sb.AppendLine($"{Resources.Typ}: {doc.ReferenceTypeCode}");
            if (doc.LineId != null) sb.AppendLine($"{Resources.Position}: {InvoiceText.Text(doc.LineId)}");

            return sb.ToString();
        }

        public static void AddContentCodeLine(StringBuilder sb, Code? code)
        {
            if (!string.IsNullOrWhiteSpace(code?.Value))
            {
                if (sb.Length > 0) sb.Append(", ");
                sb.Append($"{Resources.Inhaltscodierung}: {code!.Value}");
            }
        }

        public static void AddSubjectCodeLine(StringBuilder sb, Code? code)
        {
            if (!string.IsNullOrWhiteSpace(code?.Value))
            {
                if (sb.Length > 0) sb.Append(", ");
                sb.Append($"{Resources.Betreffcodierung}: {code!.Value}");
            }
        }

        /*
         * One tax of a position or of the document: the rate with its type and its
         * category, the amount, the basis amounts, the reason of an exemption, and the
         * day of the tax.
         *
         * A part that the document does not give makes no text. If the tax gives
         * nothing, the method gives an empty text, thus the caller gets no empty line.
         *
         * The text ends with a line break, because the caller appends it to a text that
         * holds one item per line.
         *
         * The code of the due date type does not come to the page. It is a code, and the
         * page shows the day itself.
         */
        public static string FormatTradeTax(TradeTax tax)
        {
            string rate = FormatGroup(
                tax.RateApplicablePercent.HasValue
                    ? tax.RateApplicablePercent.Value.ToString("0.00") + "%"
                    : null,
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
                FormatGroup(tax.ExemptionReason, tax.ExemptionReasonCode?.Value),
                DateOf(tax.TaxPointDate)?.ToString("d"));

            string all = FormatGroup(rate, details);

            return all.Length == 0 ? string.Empty : $"{Resources.tax}: {all}{Environment.NewLine}";
        }

    }
}
