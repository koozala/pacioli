using iText.Kernel.Colors;
using iText.Layout;
using iText.Layout.Element;
using Pacioli.Language.Resources;
using System.Collections.Generic;
using System.Text;
using Vit.FeRD.ExtendedDocument.Model;

namespace Pacioli.Pdf.Invoice.Sections
{
    /*
     * The notes of the document.
     *
     * A note can have a subject code and a content code. If a note has a code, the page
     * shows a line with the code above the text of the note.
     */
    internal sealed class NotesSection : IInvoiceSection
    {
        public string Name => "Notes";

        public void Write(Document doc, InvoiceContext context)
        {
            var notes = context.Document?.IncludedNote ?? new List<Note>();

            if (notes.Count == 0)
            {
                return;
            }

            doc.Add(new Paragraph($"{Resources.remarks}")
                .SetFont(context.Bold).SetFontSize(12.0f).SetMarginTop(20.0f));

            foreach (var note in notes)
            {
                StringBuilder sb = new StringBuilder();
                InvoiceText.AddContentCodeLine(sb, note.ContentCode);
                InvoiceText.AddSubjectCodeLine(sb, note.SubjectCode);
                if (sb.Length > 0)
                {
                    Paragraph codeLine = new Paragraph(sb.ToString()).SetFontSize(11f);
                    codeLine.SetBackgroundColor(ColorConstants.CYAN);
                    doc.Add(codeLine);
                }

                Paragraph line = new Paragraph($"{note.Content}").SetFontSize(11f);
                line.SetBackgroundColor(ColorConstants.LIGHT_GRAY);
                doc.Add(line);
            }
        }
    }
}
