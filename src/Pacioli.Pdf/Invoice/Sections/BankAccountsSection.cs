using En16931.Model;
using iText.Layout;
using Pacioli.Language.Resources;
using System.Text;

namespace Pacioli.Pdf.Invoice.Sections
{
    /*
     * The bank accounts.
     *
     * First come the accounts of the seller (BG-17), then the account of the buyer. The
     * buyer gives an account for a direct debit (BT-91).
     */
    internal sealed class BankAccountsSection : IInvoiceSection
    {
        public string Name => "Bank accounts";

        public void Write(Document doc, InvoiceContext context)
        {
            PaymentInstructions? payment = context.Core.PaymentInstructions;
            if (payment == null)
            {
                return;
            }

            foreach (CreditTransfer transfer in payment.CreditTransfers)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append($"{Resources.accountHolder}: {transfer.AccountName}");
                sb.Append($"\nIBAN: {transfer.AccountIdentifier}");
                sb.Append($"\nBIC: {transfer.ServiceProviderIdentifier}");
                doc.Add(new PartyBox(Resources.bankverbindung, sb.ToString()));
            }

            /*
             * An account number that is not an IBAN. The core puts it in the same
             * property as the IBAN, thus the extension names it apart.
             */
            foreach (string proprietary in payment.Extensions?.ProprietaryAccountIdentifiers ?? [])
            {
                doc.Add(new PartyBox(Resources.bankverbindung, proprietary));
            }

            DirectDebit? debit = payment.DirectDebit;
            if (debit?.DebitedAccountIdentifier != null)
            {
                doc.Add(new PartyBox(Resources.bankverbindung,
                    $"{payment.Extensions?.DebitedAccountName}\n{debit.DebitedAccountIdentifier}\n" +
                    $"{payment.Extensions?.DebitedServiceProviderIdentifier}"));
            }
        }
    }
}
