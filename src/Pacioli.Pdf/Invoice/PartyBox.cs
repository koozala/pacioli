using iText.IO.Font.Constants;
using iText.Kernel.Font;
using iText.Layout.Borders;
using iText.Layout.Element;
using iText.StyledXmlParser.Css.Resolve.Shorthand.Impl;
using Pacioli.Language.Resources;
using Pacioli.Pdf.Invoice.Sections;
using System.Linq;
using System.Text;
using Vit.FeRD.ExtendedDocument.Model;

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

        public PartyBox(string title, TradeParty party)
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
             * address, the telephone, and the fax. Before, the box showed the name of
             * the first person only, thus the other data of a contact was not on the
             * page. FormatTradeContact ends each line with a line break.
             */
            foreach (TradeContact? contact in party.DefinedTradeContact)
            {
                if (contact != null)
                {
                    AddText(InvoiceText.FormatTradeContact(contact));
                }
            }

            if (!string.IsNullOrWhiteSpace(party.Description))
            {
                AddText($"{party.Description}\n");
            }

            if (party.PostalTradeAddress != null)
            {
                AddText(InvoiceText.FormatAddress(party.PostalTradeAddress) + "\n");
            }

            Identifier? id = party.Id.FirstOrDefault(entry => !string.IsNullOrWhiteSpace(entry?.Value));
            if (id != null)
            {
                string s = id.SchemeId != null ? $"({id.SchemeId})" : string.Empty;
                AddText($"ID: {id.Value} {s}\n");
            }

            // Steuer-ID und USt-Idnr
            foreach (var reg in party.SpecifiedTaxRegistration)
            {
                if (!string.IsNullOrWhiteSpace(reg?.Id?.Value))
                {
                    AddText($"{Resources.SteuerId}: {reg!.Id!.SchemeId}: {reg!.Id!.Value}\n");
                }
            }

            foreach (var gid in party.GlobalId)
            {
                if (gid != null)
                {
                    AddText($"{Resources.GlobalId}: ({gid.SchemeId}) {gid.Value}\n");
                }
            }

            if (party.UriUniversalCommunication?.UriId != null)
            {
                AddLine(party.UriUniversalCommunication?.UriId?.Value ?? string.Empty);
            }

            if (party.SpecifiedLegalOrganization != null)
            {
                if (party.SpecifiedLegalOrganization.TradingBusinessName != null)
                {
                    AddText($"{Resources.NameDerOrganisation}: {party.SpecifiedLegalOrganization.TradingBusinessName}\n");
                }
                AddText($"ID: {InvoiceText.Text(party.SpecifiedLegalOrganization.Id)}\n");
                if (party.SpecifiedLegalOrganization.PostalTradeAddress != null)
                {
                    AddText(InvoiceText.FormatAddress(party.SpecifiedLegalOrganization.PostalTradeAddress) + "\n");
                }
            }

            this
                .SetPadding(5.0f)
                .SetBorder(new SolidBorder(2.0f));
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
