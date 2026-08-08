using iText.Layout;
using Pacioli.Language.Resources;
using System.Collections.Generic;
using System.Text;
using Vit.FeRD.ExtendedDocument.Model;

namespace Pacioli.Pdf.Invoice.Sections
{
    /*
     * How the buyer pays: the free text of the payment means, a card, the SEPA creditor
     * identifier, and the SEPA mandate.
     *
     * The section writes one box for all of this data. It writes nothing if the document
     * gives none of it.
     */
    internal sealed class PaymentMeansSection : IInvoiceSection
    {
        public string Name => "Payment means";

        public void Write(Document doc, InvoiceContext context)
        {
            StringBuilder pmStr = new StringBuilder();

            foreach (var means in context.Settlement?.SpecifiedTradeSettlementPaymentMeans
                                  ?? new List<TradeSettlementPaymentMeans>())
            {
                if (!string.IsNullOrWhiteSpace(means.Information)) pmStr.AppendLine(means.Information);
                if (means.ApplicableTradeSettlementFinancialCard != null)
                {
                    pmStr.AppendLine($"{Resources.card}: {means.ApplicableTradeSettlementFinancialCard.Id?.Value} " +
                                     $"{means.ApplicableTradeSettlementFinancialCard.CardholderName}");
                }
            }

            if (!string.IsNullOrWhiteSpace(context.Settlement?.CreditorReferenceId?.Value))
            {
                pmStr.AppendLine(context.Settlement!.CreditorReferenceId!.Value);
            }

            foreach (var terms in context.Settlement?.SpecifiedTradePaymentTerms ?? new List<TradePaymentTerms>())
            {
                if (!string.IsNullOrWhiteSpace(terms.DirectDebitMandateId?.Value))
                {
                    pmStr.AppendLine($"{Resources.sepaMandate}: {terms.DirectDebitMandateId!.Value}");
                }
            }

            if (pmStr.Length > 0)
            {
                doc.Add(new PartyBox($"{Resources.paymentTerms}", pmStr.ToString()));
            }
        }
    }
}
