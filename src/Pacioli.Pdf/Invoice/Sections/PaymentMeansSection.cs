using En16931.Model;
using En16931.Model.Extended;
using iText.Layout;
using Pacioli.Language.Resources;
using System.Text;

namespace Pacioli.Pdf.Invoice.Sections
{
    /*
     * How the buyer pays: the free text of the payment means (BT-82), a card (BG-18), the
     * SEPA creditor identifier (BT-90), and the SEPA mandate (BT-89).
     *
     * The section writes one box for all of this data. It writes nothing if the document
     * gives none of it.
     */
    internal sealed class PaymentMeansSection : IInvoiceSection
    {
        public string Name => "Payment means";

        public void Write(Document doc, InvoiceContext context)
        {
            var invoice = context.Core;
            PaymentInstructions? payment = invoice.PaymentInstructions;

            StringBuilder pmStr = new StringBuilder();

            if (!string.IsNullOrWhiteSpace(payment?.PaymentMeansText))
            {
                pmStr.AppendLine(payment!.PaymentMeansText);
            }

            PaymentCard? card = payment?.PaymentCard;
            if (card != null)
            {
                pmStr.AppendLine($"{Resources.card}: {card.PrimaryAccountNumber} {card.HolderName}");
            }

            /* BT-90 stands on the settlement in the document, and on the seller in the model. */
            if (!string.IsNullOrWhiteSpace(invoice.Seller?.BankAssignedCreditorIdentifier))
            {
                pmStr.AppendLine(invoice.Seller!.BankAssignedCreditorIdentifier);
            }

            foreach (PaymentTerm term in invoice.Extensions?.PaymentTerms ?? [])
            {
                if (!string.IsNullOrWhiteSpace(term.DirectDebitMandateReference))
                {
                    pmStr.AppendLine($"{Resources.sepaMandate}: {term.DirectDebitMandateReference}");
                }
            }

            if (pmStr.Length > 0)
            {
                doc.Add(new PartyBox($"{Resources.paymentTerms}", pmStr.ToString()));
            }
        }
    }
}
