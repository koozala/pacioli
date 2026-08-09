using iText.Layout;
using iText.Layout.Element;
using Pacioli.Language.Resources;
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
            var services = context.Settlement?.SpecifiedLogisticsServiceCharge ?? new List<LogisticsServiceCharge>();

            if (headerCharges.Count == 0 && services.Count == 0)
            {
                return;
            }

            doc.Add(new Paragraph(Resources.RabatteUndZuschlaege).SetMarginTop(20.0f));

            if (headerCharges.Count > 0)
            {
                TradeAllowanceChargesTab chargeTab = new TradeAllowanceChargesTab();
                chargeTab.Add(headerCharges);
                doc.Add(chargeTab.GetTab());
            }

            /*
             * The costs of the transport. The description of the document already names
             * the kind of the cost, thus the line needs no caption of its own.
             */
            foreach (var service in services)
            {
                doc.Add(new Paragraph(InvoiceText.FormatGroup(
                    service.Description,
                    InvoiceText.FormatAmount(service.AppliedAmount),
                    FormatServiceTax(service))).SetFontSize(11));
            }
        }

        /* The tax rates of a service charge, for example "19,00% MwSt". */
        private static string FormatServiceTax(LogisticsServiceCharge service)
        {
            var parts = new List<string>();

            foreach (var tax in service.AppliedTradeTax)
            {
                string text = InvoiceText.FormatGroup(
                    tax.RateApplicablePercent.HasValue ? tax.RateApplicablePercent.Value.ToString("0.00") + "%" : null,
                    tax.TypeCode.ToTaxTypeName(),
                    tax.CategoryCode.ToTaxCategoryName());

                if (text.Length > 0)
                {
                    parts.Add(text);
                }
            }

            return string.Join(" ", parts);
        }
    }
}
