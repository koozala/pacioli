using iText.Layout.Element;
using iText.Layout.Properties;
using System;
using System.Collections.Generic;
using System.Text;
using Vit.FeRD.ExtendedDocument.Model;

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
                tab.AddCell(new ItemCellRight($"{Fmt.ToString(charge.ActualAmount?.Value, "#,0.00")}"));
                tab.AddCell(new ItemCellRight($"{charge.CalculationPercent}%"));
                tab.AddCell(new ItemCellRight($"{Fmt.ToString(charge.BasisAmount?.Value, "#,0.00")}"));

                StringBuilder taxText = new StringBuilder();
                if (charge.CategoryTradeTax != null)
                {
                    taxText.Append($"{charge.CategoryTradeTax.RateApplicablePercent?.ToString("0.00")}% " +
                                   $"{charge.CategoryTradeTax.CategoryCode ?? string.Empty}");
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
