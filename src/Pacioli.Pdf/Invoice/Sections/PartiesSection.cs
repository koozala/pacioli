using iText.Layout;
using Pacioli.Language.Resources;

namespace Pacioli.Pdf.Invoice.Sections
{
    /*
     * The parties of the invoice, each one in a box.
     *
     * The order is: seller, invoicee, payee, goods sender, goods receiver, buyer.
     * A party that the invoice does not give gets no box.
     */
    internal sealed class PartiesSection : IInvoiceSection
    {
        public string Name => "Parties";

        public void Write(Document doc, InvoiceContext context)
        {
            if (context.Agreement?.SellerTradeParty != null)
            {
                doc.Add(new PartyBox($"{Resources.seller}", context.Agreement.SellerTradeParty));
            }

            if (context.Settlement?.InvoiceeTradeParty != null)
            {
                doc.Add(new PartyBox(Resources.invoicee, context.Settlement.InvoiceeTradeParty));
            }

            if (context.Settlement?.PayeeTradeParty != null)
            {
                doc.Add(new PartyBox(Resources.payee, context.Settlement.PayeeTradeParty));
            }

            if (context.Delivery?.ShipFromTradeParty != null)
            {
                doc.Add(new PartyBox(Resources.shipFrom, context.Delivery.ShipFromTradeParty));
            }

            if (context.Delivery?.ShipToTradeParty != null)
            {
                doc.Add(new PartyBox(Resources.shipTo, context.Delivery.ShipToTradeParty));
            }

            if (context.Agreement?.BuyerTradeParty != null)
            {
                doc.Add(new PartyBox(Resources.buyer, context.Agreement.BuyerTradeParty));
            }
        }
    }
}
