using iText.Layout;
using Pacioli.Language.Resources;
using System.Linq;
using System.Text;
using Vit.FeRD.ExtendedDocument.Model;

namespace Pacioli.Pdf.Invoice.Sections
{
    /*
     * The contact person of the seller and the contact person of the buyer.
     * A party can give more than one contact. The page shows the first one.
     */
    internal sealed class ContactsSection : IInvoiceSection
    {
        public string Name => "Contacts";

        public void Write(Document doc, InvoiceContext context)
        {
            WriteContact(doc, context.Agreement?.SellerTradeParty, Resources.contactSeller);
            WriteContact(doc, context.Agreement?.BuyerTradeParty, Resources.contactBuyer);
        }

        private static void WriteContact(Document doc, TradeParty? party, string caption)
        {
            TradeContact? contact = party?.DefinedTradeContact.FirstOrDefault();
            if (contact == null)
            {
                return;
            }

            StringBuilder sb = new StringBuilder();
            InvoiceText.AppendContact(sb, contact);

            if (sb.Length > 0)
            {
                doc.Add(new PartyBox($"{caption}", sb.ToString()));
            }
        }
    }
}
