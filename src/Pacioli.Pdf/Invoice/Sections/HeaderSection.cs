using iText.Layout;
using iText.Layout.Borders;
using iText.Layout.Element;
using iText.Layout.Properties;
using Pacioli.Language.Resources;
using System;

namespace Pacioli.Pdf.Invoice.Sections
{
    /*
     * The head of the page: the name of the source file, the title of the document,
     * the date, the invoice number, and the order number.
     */
    internal sealed class HeaderSection : IInvoiceSection
    {
        public string Name => "Header";

        public void Write(Document doc, InvoiceContext context)
        {
            var invoice = context.Core;

            Paragraph infoText1 = new Paragraph().SetTextAlignment(TextAlignment.CENTER).SetFontSize(10);
            infoText1.SetBorder(new SolidBorder(0.5f));
            infoText1.Add(string.Format(Resources.infoText1, context.SourceFile));
            doc.Add(infoText1);

            // Überschrift

            string? title = invoice.Extensions?.Title;
            if (!string.IsNullOrWhiteSpace(title))
            {
                doc.Add(new Paragraph($"{title}")
                    .SetTextAlignment(TextAlignment.CENTER).SetFontSize(14).SetFont(context.Bold).SetMarginTop(10.0f));
            }

            doc.Add(new Paragraph($"{TypeCodeMapping.Map(invoice.InvoiceTypeCode)}")
                .SetTextAlignment(TextAlignment.CENTER).SetFontSize(12).SetFont(context.Bold).SetMarginTop(10.0f));

            // Datum usw.

            DateOnly? invoiceDate = invoice.IssueDate;
            if (invoiceDate != null)
            {
                string f1 = invoiceDate.Value.ToString("d");
                Paragraph dateLine = new Paragraph($"{Resources.docDate}: {f1}")
                    .SetTextAlignment(TextAlignment.RIGHT).SetFontSize(12);
                doc.Add(dateLine);
            }

            if (!string.IsNullOrWhiteSpace(invoice.InvoiceNumber))
            {
                Paragraph invoiceNoLine = new Paragraph($"{Resources.invoiceNo}: {invoice.InvoiceNumber}")
                    .SetTextAlignment(TextAlignment.RIGHT).SetFontSize(12);
                doc.Add(invoiceNoLine);
            }

            if (!string.IsNullOrWhiteSpace(invoice.PurchaseOrderReference))
            {
                doc.Add(new Paragraph($"{Resources.orderNo}: {invoice.PurchaseOrderReference}")
                    .SetTextAlignment(TextAlignment.RIGHT).SetFontSize(12));
            }
        }
    }
}
