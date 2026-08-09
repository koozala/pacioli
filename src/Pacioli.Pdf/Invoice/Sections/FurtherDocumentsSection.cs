using iText.Layout;
using iText.Layout.Element;
using Pacioli.Language.Resources;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pacioli.Pdf.Invoice.Sections
{
    internal sealed class FurtherDocumentsSection : IInvoiceSection
    {
        public string Name => throw new NotImplementedException();

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
                doc.Add(tab);
            }
        }
    }
}
