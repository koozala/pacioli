using En16931.Model;
using iText.IO.Font.Constants;
using iText.Kernel.Font;
using iText.Layout.Borders;
using iText.Layout.Element;
using Pacioli.Language.Resources;
using Pacioli.Pdf.Invoice.Sections;
using System.Collections.Generic;
using System.Linq;
using CodeLists = En16931.Codes.CodeLists;

namespace Pacioli.Pdf.Invoice
{
    internal class PartyBox : Paragraph
    {
        public PartyBox(string title, string text)
        {
            var bold = PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD);
            Text t1 = new Text($"{title}\n").SetFont(bold);
            this.Add(t1)
                .SetPadding(5.0f)
                .SetBorder(new SolidBorder(2.0f));
            AddText(text);
        }

        public PartyBox(string title, Party party)
        {
            var bold = PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD);
            Text t1 = new Text($"{title}\n").SetFont(bold).SetFontSize(12.0f);
            this.Add(t1);

            if (!string.IsNullOrWhiteSpace(party.Name))
            {
                AddText($"{party.Name}\n");
            }

            /*
             * The full data of each contact person: the name, the department, the mail
             * address, the telephone, and the fax. EN 16931 gives one contact per party
             * (BG-6 and BG-9); a document can give more, and then they stand in the
             * extension. FormatContact ends each line with a line break.
             */
            foreach (Contact contact in ContactsOf(party))
            {
                AddText(CoreText.FormatContact(contact));
            }

            /* BT-33, the free text about the seller. */
            if (!string.IsNullOrWhiteSpace(party.AdditionalLegalInformation))
            {
                AddText($"{party.AdditionalLegalInformation}\n");
            }

            if (party.PostalAddress != null)
            {
                AddText(CoreText.FormatAddress(party.PostalAddress) + "\n");
            }

            /*
             * The identifier of the party (BT-29). The core holds the plain identifiers
             * and the identifiers with a scheme in one list; the ones with a scheme come
             * below, thus this line takes an identifier without a scheme.
             */
            Identifier? id = party.Identifiers
                .FirstOrDefault(entry => !string.IsNullOrWhiteSpace(entry?.Value));
            if (id != null)
            {
                AddText($"ID: {CodeText.Name(CodeLists.IdentifierScheme, id.SchemeId)} {id.Value}\n");
            }

            // Steuer-ID und USt-Idnr

            foreach (Identifier registration in party.Extensions?.TaxRegistrations ?? [])
            {
                if (!string.IsNullOrWhiteSpace(registration.Value))
                {
                    AddText($"{Resources.SteuerId}: {CodeText.Name(CodeLists.IdentifierScheme, registration.SchemeId)} {registration.Value}\n");
                }
            }

            foreach (Identifier global in party.Extensions?.GlobalIdentifiers ?? [])
            {
                AddText($"{Resources.GlobalId}: {CodeText.Name(CodeLists.IdentifierScheme, global.SchemeId)} {global.Value}\n");
            }

            if (party.ElectronicAddress != null)
            {
                AddLine($"{CodeText.Name(CodeLists.ElectronicAddressScheme, party.ElectronicAddress.SchemeId)}: {party.ElectronicAddress.Value}");
            }

            /*
             * The legal organisation: its name (BT-28), its identifier (BT-30), and its
             * address if the document gives one.
             */
            if (party.TradingName != null || party.LegalRegistrationIdentifier != null
                                          || party.Extensions?.LegalOrganizationAddress != null)
            {
                if (party.TradingName != null)
                {
                    AddText($"{Resources.NameDerOrganisation}: {party.TradingName}\n");
                }

                AddText($"ID: {party.LegalRegistrationIdentifier?.Value}\n");

                if (party.Extensions?.LegalOrganizationAddress != null)
                {
                    AddText(CoreText.FormatAddress(party.Extensions.LegalOrganizationAddress) + "\n");
                }
            }

            this
                .SetPadding(5.0f)
                .SetBorder(new SolidBorder(2.0f));
        }

        /*
         * The contacts of a party. The extension holds each one; if it holds none, the
         * core gives the one contact of EN 16931.
         */
        private static IEnumerable<Contact> ContactsOf(Party party)
        {
            if (party.Extensions is { Contacts.Count: > 0 })
            {
                return party.Extensions.Contacts;
            }

            return party.Contact == null ? [] : [party.Contact];
        }

        void AddText(string text)
        {
            Text t = new Text(text).SetFontSize(10f);
            this.Add(t);
        }

        void AddLine(string text)
        {
            this.Add(new Text(text + "\n").SetFontSize(10f));
        }
    }
}
