using En16931.Model;
using iText.Layout;
using Pacioli.Language.Resources;
using System.Text;

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
            WriteContact(doc, context.Core.Seller, Resources.contactSeller);
            WriteContact(doc, context.Core.Buyer, Resources.contactBuyer);
        }

        private static void WriteContact(Document doc, Party? party, string caption)
        {
            /* BG-6 and BG-9 give one contact. The first contact of the document. */
            Contact? contact = party?.Contact;
            if (contact == null)
            {
                return;
            }

            StringBuilder sb = new StringBuilder();
            CoreText.AppendContact(sb, contact);

            if (sb.Length > 0)
            {
                doc.Add(new PartyBox($"{caption}", sb.ToString()));
            }
        }
    }
}
