using En16931.Model;
using En16931.Model.Extended;
using iText.Layout;
using iText.Layout.Element;
using Pacioli.Language.Resources;
using System.Collections.Generic;
using System.Linq;

namespace Pacioli.Pdf.Invoice.Sections
{
    /*
     * The allowances and the charges of the document (BG-20 and BG-21), and the costs of
     * the transport. The section writes nothing if the document has none.
     */
    internal sealed class AllowanceChargeSection : IInvoiceSection
    {
        public string Name => "Allowances and charges";

        public void Write(Document doc, InvoiceContext context)
        {
            var invoice = context.Core;

            /* The order of the document: an allowance and a charge stand in one list. */
            List<AllowanceCharge> headerCharges = invoice.Allowances.Concat(invoice.Charges).ToList();
            IReadOnlyList<LogisticsServiceCharge> services =
                invoice.Extensions?.LogisticsServiceCharges ?? [];

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
            foreach (LogisticsServiceCharge service in services)
            {
                doc.Add(new Paragraph(CoreText.FormatGroup(
                    service.Description,
                    CoreText.FormatAmount(service.Amount),
                    FormatServiceTax(service))).SetFontSize(11));
            }
        }

        /* The tax rates of a service charge, for example "19,00% MwSt". */
        private static string FormatServiceTax(LogisticsServiceCharge service)
        {
            var parts = new List<string>();

            foreach (TaxDetail tax in service.Tax)
            {
                string text = CoreText.FormatGroup(
                    tax.RatePercent.HasValue ? tax.RatePercent.Value.ToString("0.00") + "%" : null,
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
