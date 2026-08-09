using iText.Layout;
using iText.Layout.Element;
using Pacioli.Language.Resources;
using System;
using System.Linq;
using System.Text;
using Vit.FeRD.ExtendedDocument.Model;

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
            if (!string.IsNullOrWhiteSpace(context.Agreement?.BuyerReference))
            {
                doc.Add(new Paragraph($"{Resources.refOrder}: {context.Agreement!.BuyerReference}"));
            }

            // Kundennummer

            StringBuilder reStr = new StringBuilder();
            Identifier? buyerId = context.Agreement?.BuyerTradeParty?.Id
                .FirstOrDefault(entry => !string.IsNullOrWhiteSpace(entry?.Value));
            if (buyerId != null)
            {
                reStr.Append($"{Resources.customerNo}: ");
                reStr.Append($" {buyerId.Value}");
                reStr.Append($" ({buyerId.SchemeId})");
            }
            if (reStr.Length > 0)
            {
                doc.Add(new Paragraph(reStr.ToString()));
            }

            DateOnly? orderDate = InvoiceText.DateOf(context.Agreement?.BuyerOrderReferencedDocument?.FormattedIssueDateTime);
            if (orderDate != null)
            {
                doc.Add(new Paragraph($"{Resources.orderDate}: {orderDate.Value.ToString("d")}"));
            }

            ProcuringProject? project = context.Agreement?.SpecifiedProcuringProject;
            if (project != null &&
                (!string.IsNullOrWhiteSpace(project.Name) || !string.IsNullOrWhiteSpace(project.Id?.Value)))
            {
                doc.Add(new Paragraph($"{Resources.projectNo}: {project.Name} {project.Id?.Value}"));
            }

            DateOnly? deliveryDate = InvoiceText.DateOf(context.Delivery?.ActualDeliverySupplyChainEvent?.OccurrenceDateTime);
            if (deliveryDate != null)
            {
                doc.Add(new Paragraph($"{Resources.deliveryDate}: {deliveryDate.Value.ToString("d")}"));
            }

            doc.Add(new Paragraph($"{Resources.currencyNote} {context.Settlement?.InvoiceCurrencyCode}")
                .SetFont(context.Bold));

            if (context.Agreement?.SellerOrderReferencedDocument != null)
            {
                doc.Add(new Paragraph($"{Resources.UnsereBestellnummer}: {context.Agreement.SellerOrderReferencedDocument.Name ?? "--"}"));
            }
        }
    }
}
