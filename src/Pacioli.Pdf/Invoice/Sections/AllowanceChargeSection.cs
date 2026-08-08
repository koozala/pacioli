using iText.Layout;
using iText.Layout.Element;
using System.Collections.Generic;
using Vit.FeRD.ExtendedDocument.Model;

namespace Pacioli.Pdf.Invoice.Sections
{
    /*
     * The allowances and the charges of the document.
     * The section writes nothing if the document has none.
     */
    internal sealed class AllowanceChargeSection : IInvoiceSection
    {
        public string Name => "Allowances and charges";

        public void Write(Document doc, InvoiceContext context)
        {
            var headerCharges = context.Settlement?.SpecifiedTradeAllowanceCharge ?? new List<TradeAllowanceCharge>();

            if (headerCharges.Count == 0)
            {
                return;
            }

            doc.Add(new Paragraph($"Rabatte und Zuschläge").SetMarginTop(20.0f));
            TradeAllowanceChargesTab chargeTab = new TradeAllowanceChargesTab();
            chargeTab.Add(headerCharges);
            doc.Add(chargeTab.GetTab());
        }
    }
}
