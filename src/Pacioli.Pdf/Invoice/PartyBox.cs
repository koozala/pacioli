using iText.IO.Font.Constants;
using iText.Kernel.Font;
using iText.Layout.Borders;
using iText.Layout.Element;
using s2industries.ZUGFeRD;

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

            if (!string.IsNullOrWhiteSpace(party.ContactName))
            {
                AddText($"{party.ContactName}\n");
            }

            if (!string.IsNullOrWhiteSpace(party.AddressLine3))
            {
                AddText($"{party.AddressLine3}");
            }

            if (!string.IsNullOrWhiteSpace(party.Street))
            {
                AddText($"{party.Street}\n");
            }

            if (!string.IsNullOrWhiteSpace(party.City))
            {
                AddText($"{party.Postcode} {party.City}\n");
            }

            if (!string.IsNullOrWhiteSpace(party.Country.ToString()))
            {
                AddText($"{party.Country}\n");
            }

            AddText($"{party.ID.SchemeID.ToString()} {party.ID.ID}\n");

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
