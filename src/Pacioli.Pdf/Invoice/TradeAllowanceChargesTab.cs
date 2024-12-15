using iText.Layout.Element;
using iText.Layout.Properties;
using s2industries.ZUGFeRD;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pacioli.Pdf.Invoice
{
    public class TradeAllowanceChargesTab
    {
        Table tab;
        UnitValue full = UnitValue.CreatePercentValue(100.0f);

        public TradeAllowanceChargesTab()
        {
            tab = new Table(6).SetWidth(full);
            tab.AddHeaderCell("");
            tab.AddHeaderCell("Grund");
            tab.AddHeaderCell("Act. Amount");
            tab.AddHeaderCell("Anteil");
            tab.AddHeaderCell("Betrag");
            tab.AddHeaderCell("Steuer");
        }

        public void Add(IEnumerable<TradeAllowanceCharge> charges)
        {
            foreach (var charge in charges)
            {
                var text = charge.ChargeIndicator ? "Zuschlag" : "Abschlag";
                tab.AddCell(new ItemCell(text));
                tab.AddCell(new ItemCell($"{charge.Reason}"));
                tab.AddCell(new ItemCellRight($"{charge.ActualAmount.ToString("#,0.00")}"));
                tab.AddCell(new ItemCellRight($"{charge.ChargePercentage}%"));
                tab.AddCell(new ItemCellRight($"{charge.Amount.ToString("#,0.00")}"));

                StringBuilder taxText = new StringBuilder();
                if (charge.Tax != null)
                {
                    taxText.Append($"{charge.Tax.Percent.ToString("0.00")}% {charge.Tax.CategoryCode?.ToString() ?? string.Empty}");
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
