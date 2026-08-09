using iText.Kernel.Colors;
using iText.Layout;
using iText.Layout.Borders;
using iText.Layout.Element;
using iText.Layout.Properties;
using Pacioli.Language.Resources;
using System;
using Vit.FeRD.ExtendedDocument.Model;

namespace Pacioli.Pdf.Invoice.Sections
{
    /*
     * The head of the page: the name of the source file, the title of the document,
     * the date, the invoice number, and the order number. Then comes a group with the
     * references of the document, in a light frame.
     */
    internal sealed class HeaderSection : IInvoiceSection
    {
        public string Name => "Header";

        public void Write(Document doc, InvoiceContext context)
        {
            Paragraph infoText1 = new Paragraph().SetTextAlignment(TextAlignment.CENTER).SetFontSize(10);
            infoText1.SetBorder(new SolidBorder(0.5f));
            infoText1.Add(string.Format(Resources.infoText1, context.SourceFile));
            doc.Add(infoText1);

            // Überschrift

            if (!string.IsNullOrWhiteSpace(context.Document?.Name))
            {
                doc.Add(new Paragraph($"{context.Document!.Name}")
                    .SetTextAlignment(TextAlignment.CENTER).SetFontSize(14).SetFont(context.Bold).SetMarginTop(10.0f));
            }
            doc.Add(new Paragraph($"{TypeCodeMapping.Map(context.Document?.TypeCode)}")
                .SetTextAlignment(TextAlignment.CENTER).SetFontSize(12).SetFont(context.Bold).SetMarginTop(10.0f));

            // Datum usw.

            DateOnly? invoiceDate = InvoiceText.DateOf(context.Document?.IssueDateTime);
            if (invoiceDate != null)
            {
                string f1 = invoiceDate.Value.ToString("d");
                Paragraph dateLine = new Paragraph($"{Resources.docDate}: {f1}")
                    .SetTextAlignment(TextAlignment.RIGHT).SetFontSize(12);
                doc.Add(dateLine);
            }

            string? invoiceNo = InvoiceText.Text(context.Document?.Id);
            if (!string.IsNullOrWhiteSpace(invoiceNo))
            {
                Paragraph invoiceNoLine = new Paragraph($"{Resources.invoiceNo}: {invoiceNo}")
                    .SetTextAlignment(TextAlignment.RIGHT).SetFontSize(12);
                doc.Add(invoiceNoLine);
            }

            string? orderNo = InvoiceText.Text(context.Agreement?.BuyerOrderReferencedDocument?.IssuerAssignedId);
            if (!string.IsNullOrWhiteSpace(orderNo))
            {
                doc.Add(new Paragraph($"{Resources.orderNo}: {orderNo}")
                    .SetTextAlignment(TextAlignment.RIGHT).SetFontSize(12));
            }

            
        }

        
    }

}