using iText.Layout;
using iText.Layout.Element;
using Pacioli.Language.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vit.FeRD.ExtendedDocument.Model;

namespace Pacioli.Pdf.Invoice.Sections
{
    /*
     * The table of the positions.
     *
     * The table has seven columns: the position number, the description, the tax, the
     * quantity, the net unit price, the gross unit price, and the total of the position.
     * The column with the description holds all other data of the position.
     */
    internal sealed class LineItemsSection : IInvoiceSection
    {
        public string Name => "Line items";

        public void Write(Document doc, InvoiceContext context)
        {
            Table tab = new Table(7).SetWidth(context.FullWidth);
            tab.AddHeaderCell(new ItemCell($"{Resources.pos}"));
            tab.AddHeaderCell(new ItemCell($"{Resources.description}"));
            tab.AddHeaderCell(new ItemCell($"{Resources.tax}"));
            tab.AddHeaderCell(new ItemCell($"{Resources.amount}"));
            tab.AddHeaderCell(new ItemCell($"{Resources.netUnitPrice}"));
            tab.AddHeaderCell(new ItemCell($"{Resources.grossUnitPrice}"));
            tab.AddHeaderCell(new ItemCell($"{Resources.sum}"));

            int lineCount = 0;
            foreach (var item in context.LineItems)
            {
                LineTradeAgreement? lineAgreement = item.SpecifiedLineTradeAgreement;
                LineTradeDelivery? lineDelivery = item.SpecifiedLineTradeDelivery;
                LineTradeSettlement? lineSettlement = item.SpecifiedLineTradeSettlement;
                TradeTax? lineTax = lineSettlement?.ApplicableTradeTax.FirstOrDefault();
                Quantity? billedQuantity = lineDelivery?.BilledQuantity;

                lineCount++;
                tab.AddCell(new ItemCell($"{item.AssociatedDocumentLineDocument.LineId.Value ?? $"({lineCount})"}"));
                tab.AddCell(new ItemCell(BuildDescription(item)));
                tab.AddCell(new ItemCell($"{lineTax?.RateApplicablePercent?.ToString("0.00")}% {lineTax?.TypeCode.ToTaxTypeName()} {lineTax?.CategoryCode.ToTaxCategoryName()}"));
                tab.AddCell(new ItemCellRight($"{InvoiceText.FormatQuantity(billedQuantity)} {billedQuantity?.UnitCode.ToUnit()}"));
                tab.AddCell(new ItemCellRight($"{Fmt.ToString(lineAgreement?.NetPriceProductTradePrice?.ChargeAmount?.Value, "#,0.00")}"));
                tab.AddCell(new ItemCellRight($"{Fmt.ToString(lineAgreement?.GrossPriceProductTradePrice?.ChargeAmount?.Value, "#,0.00")}"));
                tab.AddCell(new ItemCellRight($"{Fmt.ToString(lineSettlement?.SpecifiedTradeSettlementLineMonetarySummation?.LineTotalAmount?.Value, "#,0.00")}"));
            }
            doc.Add(tab);
        }

        /* Builds the text of the description column of one position. */
        private static string BuildDescription(SupplyChainTradeLineItem item)
        {
            TradeProduct? product = item.SpecifiedTradeProduct;
            LineTradeAgreement? lineAgreement = item.SpecifiedLineTradeAgreement;
            LineTradeDelivery? lineDelivery = item.SpecifiedLineTradeDelivery;
            LineTradeSettlement? lineSettlement = item.SpecifiedLineTradeSettlement;
            Quantity? billedQuantity = lineDelivery?.BilledQuantity;

            StringBuilder s1 = new StringBuilder();
            s1.AppendLine(product?.Name);
            if (!string.IsNullOrEmpty(product?.Description))
            {
                s1.AppendLine(product!.Description);
            }

            DateOnly? itemDeliveryDate = InvoiceText.DateOf(lineDelivery?.ActualDeliverySupplyChainEvent?.OccurrenceDateTime);
            if (itemDeliveryDate != null)
            {
                s1.AppendLine($"{Resources.deliveryDate}: {itemDeliveryDate.Value.ToString("d")}");
            }
            if (!string.IsNullOrWhiteSpace(product?.SellerAssignedId?.Value))
            {
                s1.AppendLine($"{Resources.sellerAssignedId}: {product!.SellerAssignedId!.Value}");
            }

            if (!string.IsNullOrWhiteSpace(product?.GlobalId?.Value))
            {
                s1.AppendLine($"{product!.GlobalId!.SchemeId} {product.GlobalId.Value}");
            }

            SpecifiedPeriod? billingPeriod = lineSettlement?.BillingSpecifiedPeriod;
            DateOnly? periodStart = InvoiceText.DateOf(billingPeriod?.StartDateTime);
            DateOnly? periodEnd = InvoiceText.DateOf(billingPeriod?.EndDateTime);
            if (periodStart != null && periodEnd != null)
            {
                s1.AppendLine($"{Resources.billingPeriod}: {periodStart.Value.ToString("d")} - {periodEnd.Value.ToString("d")}");
            }
            if (!string.IsNullOrWhiteSpace(billedQuantity?.UnitCode))
            {
                s1.AppendLine($"{Resources.unitQuantity}: {billedQuantity!.UnitCode.ToUnit()}");
            }
            foreach (var attr in product?.ApplicableProductCharacteristic ?? new List<ProductCharacteristic>())
            {
                s1.AppendLine($"{attr.Description}: {attr.Value}");
            }
            foreach (var note in item.AssociatedDocumentLineDocument?.IncludedNote ?? new List<Note>())
            {
                s1.AppendLine($"{note.SubjectCode?.Value} {note.ContentCode?.Value} {note.Content}");
            }
            InvoiceText.AppendReference(s1, Resources.refOrderDocument, lineAgreement?.BuyerOrderReferencedDocument);
            InvoiceText.AppendReference(s1, Resources.refContractdocument, lineAgreement?.ContractReferencedDocument);
            InvoiceText.AppendReference(s1, Resources.refDeliveryNote, lineDelivery?.DeliveryNoteReferencedDocument);

            if (!string.IsNullOrWhiteSpace(product?.BuyerAssignedId?.Value))
            {
                s1.AppendLine($"{Resources.buyerAssignedId}: {product!.BuyerAssignedId!.Value}");
            }
            foreach (var acct in lineSettlement?.ReceivableSpecifiedTradeAccountingAccount ?? new List<TradeAccountingAccount>())
            {
                s1.AppendLine($"{Resources.account}: {acct.Id?.Value} {acct.TypeCode}");
            }
            foreach (var charge in lineSettlement?.SpecifiedTradeAllowanceCharge ?? new List<TradeAllowanceCharge>())
            {
                InvoiceText.AppendAllowanceCharge(s1, charge);
            }
            foreach (var charge in lineAgreement?.GrossPriceProductTradePrice?.AppliedTradeAllowanceCharge ?? new List<TradeAllowanceCharge>())
            {
                InvoiceText.AppendAllowanceCharge(s1, charge);
            }

            return s1.ToString();
        }
    }
}
