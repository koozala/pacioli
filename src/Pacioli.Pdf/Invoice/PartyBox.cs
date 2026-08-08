using iText.IO.Font.Constants;
using iText.Kernel.Font;
using iText.Layout.Borders;
using iText.Layout.Element;
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

            string? contactName = party.DefinedTradeContact
                .Select(contact => contact.PersonName)
                .FirstOrDefault(name => !string.IsNullOrWhiteSpace(name));

            if (!string.IsNullOrWhiteSpace(contactName))
            {
                AddText($"{contactName}\n");
            }

            TradeAddress? address = party.PostalTradeAddress;

            /*
             * The Cross Industry Invoice has three address lines. The page shows each
             * line that has a value, in the order of the document.
             */
            foreach (string? line in new[] { address?.LineOne, address?.LineTwo, address?.LineThree })
            {
                if (!string.IsNullOrWhiteSpace(line))
                {
                    AddText($"{line}\n");
                }
            }

            StringBuilder sb = new StringBuilder();
            string blank = string.Empty;
            if (!string.IsNullOrWhiteSpace(address?.CountryId))
            {
                sb.Append(address!.CountryId);
                blank = " ";
            }
            if (!string.IsNullOrWhiteSpace(address?.PostcodeCode?.Value))
            {
                sb.Append($"{blank}{address!.PostcodeCode!.Value}");
                blank = " ";
            }
            if (!string.IsNullOrWhiteSpace(address?.CityName))
            {
                sb.Append($"{blank}{address!.CityName}");
                blank = " ";
            }
            if (sb.Length > 0)
            {
                AddText(sb.ToString());
                sb.Append("\n");
            }

            Identifier? id = party.Id.FirstOrDefault(entry => !string.IsNullOrWhiteSpace(entry?.Value));
            if (id != null)
            {
                AddText($"{id.Value} ({id.SchemeId})\n");
            }

            // Steuer-ID und USt-Idnr
            foreach (var reg in party.SpecifiedTaxRegistration)
            {
                if (!string.IsNullOrWhiteSpace(reg?.Id?.Value))
                {
                    AddText($"{reg!.Id!.SchemeId}: {reg!.Id!.Value}\n");
                } 
            }

            foreach (var gid in party.GlobalId)
            {
                if (!string.IsNullOrWhiteSpace(gid?.Value))
                {
                    AddText($"{gid!.SchemeId}: {gid!.Value}\n");
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
    }
}
