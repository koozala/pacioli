using En16931.Model;
using iText.Layout.Element;
using iText.Layout.Properties;
using Pacioli.Language.Resources;
using System.Collections.Generic;
using System.Text;

namespace Pacioli.Pdf.Invoice
{
    /* The table of the allowances and the charges of the document (BG-20 and BG-21). */
    public class TradeAllowanceChargesTab
    {
        Table tab;
        UnitValue full = UnitValue.CreatePercentValue(100.0f);

        public TradeAllowanceChargesTab()
        {
            tab = new Table(6).SetWidth(full);
            tab.AddHeaderCell("");
            tab.AddHeaderCell(Resources.Grund);
            tab.AddHeaderCell(Resources.IstBetrag);
            tab.AddHeaderCell(Resources.Anteil);
            tab.AddHeaderCell(Resources.money);
            tab.AddHeaderCell(Resources.tax);
        }

        public void Add(IEnumerable<AllowanceCharge> charges)
        {
            foreach (AllowanceCharge charge in charges)
            {
                var text = charge.IsCharge ? Resources.Zuschlag : Resources.Abschlag;
                tab.AddCell(new ItemCell(text));
                tab.AddCell(new ItemCell($"{charge.Reason}"));
                tab.AddCell(new ItemCellRight($"{Fmt.ToString(charge.Amount, "#,0.00")}"));
                tab.AddCell(new ItemCellRight($"{charge.Percentage}%"));
                tab.AddCell(new ItemCellRight($"{Fmt.ToString(charge.BaseAmount, "#,0.00")}"));

                StringBuilder taxText = new StringBuilder();
                if (charge.VatCategoryCode != null || charge.VatRate != null)
                {
                    taxText.Append($"{charge.VatRate?.ToString("0.00")}% {charge.VatCategoryCode ?? string.Empty}");
                }
                tab.AddCell(taxText.ToString());
            }
        }

        public Table GetTab()
        {
            return tab;
        }
    }
}
