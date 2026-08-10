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
            var invoice = context.Core;
            var extensions = invoice.Extensions;

            if (invoice.Seller != null)
            {
                doc.Add(new PartyBox($"{Resources.seller}", invoice.Seller));
            }

            if (extensions?.Invoicee != null)
            {
                doc.Add(new PartyBox(Resources.invoicee, extensions.Invoicee));
            }

            if (invoice.Payee != null)
            {
                doc.Add(new PartyBox(Resources.payee, invoice.Payee));
            }

            if (extensions?.ShipFrom != null)
            {
                doc.Add(new PartyBox(Resources.shipFrom, extensions.ShipFrom));
            }

            if (extensions?.ShipTo != null)
            {
                doc.Add(new PartyBox(Resources.shipTo, extensions.ShipTo));
            }

            if (invoice.Buyer != null)
            {
                doc.Add(new PartyBox(Resources.buyer, invoice.Buyer));
            }
        }
    }
}
