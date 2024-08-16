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
                .Add(new Text($"{text}"))
                .SetPadding(5.0f)
                .SetBorder(new SolidBorder(2.0f));
        }

        public PartyBox(string title, Party party)
        {
            var bold = PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD);
            Text t1 = new Text($"{title}\n").SetFont(bold);
            this.Add(t1);

            if (!string.IsNullOrWhiteSpace(party.Name))
            {
                this.Add($"{party.Name}\n");
            }

            if (!string.IsNullOrWhiteSpace(party.ContactName))
            {
                this.Add($"{party.ContactName}\n");
            }

            if (!string.IsNullOrWhiteSpace(party.AddressLine3))
            {
                this.Add($"{party.AddressLine3}");
            }

            if (!string.IsNullOrWhiteSpace(party.Street))
            {
                this.Add($"{party.Street}\n");
            }

            if (!string.IsNullOrWhiteSpace(party.City))
            {
                this.Add($"{party.Postcode} {party.City}\n");
            }

            if (!string.IsNullOrWhiteSpace(party.Country.ToString()))
            {
                this.Add($"{party.Country}");
            }

            this
                .SetPadding(5.0f)
                .SetBorder(new SolidBorder(2.0f));
        }
    }
}
