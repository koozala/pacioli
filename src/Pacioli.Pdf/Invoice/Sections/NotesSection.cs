using iText.Kernel.Colors;
using iText.Layout;
using iText.Layout.Element;
using Pacioli.Language.Resources;
using System.Collections.Generic;
using System.Text;
using Vit.FeRD.ExtendedDocument.Model;
using CodeLists = En16931.Codes.CodeLists;

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
            var exchangeDoc = context.Invoice.ExchangedDocument;

            Table tab = new Table(3);
            int count = 0;


            foreach (var note in exchangeDoc.IncludedNote)
            {
                if (note != null)
                {
                    tab.AddCell(new ItemCell(note.SubjectCode?.Value ?? string.Empty));
                    tab.AddCell(new ItemCell(note.ContentCode?.Value ?? string.Empty));
                    tab.AddCell(new ItemCell(note.Content ?? string.Empty));
                    count++;
                }
            }

            if (count > 0)
            {
                doc.Add(new Paragraph(Resources.WeitereUnterlagen));
                if (exchangeDoc.Name != null) doc.Add(new Paragraph(exchangeDoc.Name));
                doc.Add(new Paragraph($"{Resources.Dokumentendatum}: {InvoiceText.DateOf(exchangeDoc.IssueDateTime).ToString()}"));
                doc.Add(new Paragraph($"{Resources.Code}: {CodeText.Name(CodeLists.DocumentType, exchangeDoc.TypeCode)}"));
                doc.Add(tab);
            }
        }
    }

}
