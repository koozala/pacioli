using En16931.Model;
using iText.Layout;
using iText.Layout.Element;
using Pacioli.Language.Resources;
using System;
using System.Linq;
using System.Text;
using CodeLists = En16931.Codes.CodeLists;

namespace Pacioli.Pdf.Invoice.Sections
{
    /*
     * The references of the document: the reference of the buyer, the customer number,
     * the order date, the project, the delivery date, and the currency.
     */
    internal sealed class ReferencesSection : IInvoiceSection
    {
        public string Name => "References";

        public void Write(Document doc, InvoiceContext context)
        {
            var invoice = context.Core;

            if (!string.IsNullOrWhiteSpace(invoice.BuyerReference))
            {
                doc.Add(new Paragraph($"{Resources.refOrder}: {invoice.BuyerReference}"));
            }

            // Kundennummer

            Identifier? buyerId = invoice.Buyer?.Identifiers
                .FirstOrDefault(entry => !string.IsNullOrWhiteSpace(entry?.Value));
            if (buyerId != null)
            {
                StringBuilder reStr = new StringBuilder();
                reStr.Append($"{Resources.customerNo}: ");
                reStr.Append($" {buyerId.Value}");
                if (!string.IsNullOrWhiteSpace(buyerId.SchemeId))
                {
                    reStr.Append($" ({CodeText.Name(CodeLists.IdentifierScheme, buyerId.SchemeId)})");
                }
                doc.Add(new Paragraph(reStr.ToString()));
            }

            DateOnly? orderDate = invoice.Extensions?.PurchaseOrderDate;
            if (orderDate != null)
            {
                doc.Add(new Paragraph($"{Resources.orderDate}: {orderDate.Value.ToString("d")}"));
            }

            string? projectName = invoice.Extensions?.ProjectName;
            if (!string.IsNullOrWhiteSpace(projectName) || !string.IsNullOrWhiteSpace(invoice.ProjectReference))
            {
                doc.Add(new Paragraph($"{Resources.projectNo}: {projectName} {invoice.ProjectReference}"));
            }

            /*
             * The number of the sales order (BT-14). The page shows the name of the
             * document, and "--" if the document gives none.
             */
            if (invoice.SalesOrderReference != null || invoice.Extensions?.SalesOrderName != null)
            {
                doc.Add(new Paragraph(
                    $"{Resources.UnsereBestellnummer}: {invoice.Extensions?.SalesOrderName ?? "--"}"));
            }

        }
    }
}
