using iText.Layout;
using Pacioli.Language.Resources;
using System.Collections.Generic;
using System.Text;
using Vit.FeRD.ExtendedDocument.Model;

namespace Pacioli.Pdf.Invoice.Sections
{
    /*
     * The bank accounts.
     *
     * First come the accounts of the seller, then the accounts of the buyer. The buyer
     * gives an account for a direct debit.
     *
     * The BIC is not in the account. It sits in the financial institution beside it.
     */
    internal sealed class BankAccountsSection : IInvoiceSection
    {
        public string Name => "Bank accounts";

        public void Write(Document doc, InvoiceContext context)
        {
            var paymentMeans = context.Settlement?.SpecifiedTradeSettlementPaymentMeans
                               ?? new List<TradeSettlementPaymentMeans>();

            foreach (var means in paymentMeans)
            {
                CreditorFinancialAccount? account = means.PayeePartyCreditorFinancialAccount;
                if (account == null)
                {
                    continue;
                }

                StringBuilder sb = new StringBuilder();
                sb.Append($"{Resources.accountHolder}: {account.AccountName}");
                sb.Append($"\nIBAN: {account.IbanId?.Value}");
                sb.Append($"\nBIC: {means.PayeeSpecifiedCreditorFinancialInstitution?.BicId?.Value}");
                if (!string.IsNullOrWhiteSpace(account.ProprietaryId?.Value))
                {
                    sb.Append($"\n{account.ProprietaryId!.Value}");
                }
                doc.Add(new PartyBox(Resources.bankverbindung, sb.ToString()));
            }

            foreach (var means in paymentMeans)
            {
                DebtorFinancialAccount? account = means.PayerPartyDebtorFinancialAccount;
                if (account == null)
                {
                    continue;
                }

                doc.Add(new PartyBox(Resources.bankverbindung,
                    $"{account.AccountName}\n{account.IbanId?.Value}\n{means.PayerSpecifiedDebtorFinancialInstitution?.BicId?.Value}"));
            }
        }
    }
}
