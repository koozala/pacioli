using iText.Layout.Element;

namespace Pacioli.Pdf.Invoice
{
    internal class ItemCell : Cell
    {
        const float pad = 5.0f;

        public ItemCell(string text) : base(1, 1)
        {
            this.Add(new Paragraph(text));
            this.SetFontSize(10);
            this.SetPaddings(pad, pad, pad, pad);
        }
    }

    internal class ItemCellRight : Cell
    {
        const float pad = 5.0f;

        public ItemCellRight(string text) : base(1, 1)
        {
            this.Add(new Paragraph(text));
            this.SetFontSize(10);
            this.SetPaddings(pad, pad, pad, pad);
            this.SetTextAlignment(iText.Layout.Properties.TextAlignment.RIGHT);
        }
    }
}
