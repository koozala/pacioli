using En16931.Model;
using iText.IO.Font.Constants;
using iText.Layout;
using iText.Layout.Element;
using Pacioli.Language.Resources;
using CodeLists = En16931.Codes.CodeLists;

namespace Pacioli.Pdf.Invoice.Sections
{
    /*
     * The notes of the document (BG-1).
     *
     * A note can have a subject code (BT-21) and a content code. If the document holds a
     * note, the page shows the head of the document above the table of the notes.
     */
    internal sealed class NotesSection : IInvoiceSection
    {
        public string Name => "Notes";

        public void Write(Document doc, InvoiceContext context)
        {
            var invoice = context.Core;

            Table tab = new Table(3);
            int count = 0;

            foreach (InvoiceNote note in invoice.Notes)
            {
                tab.AddCell(new ItemCell(note.SubjectCode ?? string.Empty));
                tab.AddCell(new ItemCell(note.Extensions?.ContentCode ?? string.Empty));
                tab.AddCell(new ItemCell(note.Text ?? string.Empty));
                count++;
            }

            if (count > 0)
            {
                doc.Add(new Paragraph(Resources.WeitereUnterlagen).SetFont(context.Bold).SetMarginTop(25.0f));
                doc.Add(tab);
            }
        }
    }
}
