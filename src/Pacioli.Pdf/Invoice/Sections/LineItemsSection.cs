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
    internal sealed class  LineItemsSection : IInvoiceSection
    {
        public string Name => "Line items";

        public void Write(Document doc, InvoiceContext context)
        {
            Table tab = new Table(8).SetWidth(context.FullWidth);
            tab.AddHeaderCell(new ItemCell($"{Resources.pos}"));
            tab.AddHeaderCell(new ItemCell($"{Resources.description}"));
            tab.AddHeaderCell(new ItemCell($"{Resources.tax}"));
            tab.AddHeaderCell(new ItemCell($"{Resources.amount}"));
            tab.AddHeaderCell(new ItemCell($"{Resources.netUnitPrice}"));
            tab.AddHeaderCell(new ItemCell($"{Resources.grossUnitPrice}"));
            tab.AddHeaderCell(new ItemCell($"{Resources.basis}"));
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
                tab.AddCell(new ItemCellRight(BuildPriceBasis(lineAgreement)));
                tab.AddCell(new ItemCellRight($"{Fmt.ToString(lineSettlement?.SpecifiedTradeSettlementLineMonetarySummation?.LineTotalAmount?.Value, "#,0.00")}"));
            }
            doc.Add(tab);
        }

        /*
         * Builds the text of the price basis column.
         *
         * The basis quantity tells for how many units the price applies, for example
         * "100 Anz." for a price for 100 pieces. The net price and the gross price can
         * have a different basis. If the two are equal, the column shows one line.
         */
        private static string BuildPriceBasis(LineTradeAgreement? lineAgreement)
        {
            string net = FormatBasisQuantity(lineAgreement?.NetPriceProductTradePrice?.BasisQuantity);
            string gross = FormatBasisQuantity(lineAgreement?.GrossPriceProductTradePrice?.BasisQuantity);

            if (net.Length == 0)
            {
                return gross;
            }

            if (gross.Length == 0 || string.Equals(net, gross, System.StringComparison.Ordinal))
            {
                return net;
            }

            return $"{net}\n{gross}";
        }

        private static string FormatBasisQuantity(Quantity? quantity)
        {
            if (quantity == null)
            {
                return string.Empty;
            }

            return InvoiceText.FormatGroup(InvoiceText.FormatQuantity(quantity), quantity.UnitCode.ToUnit());
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
                s1.AppendLine($"{CodeText.Name(CodeLists.IdentifierScheme, product!.GlobalId!.SchemeId)} {product.GlobalId.Value}");
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

            /* The classification of the article, for example a tariff number. */
            foreach (var classification in product?.DesignatedProductClassification ?? new List<ProductClassification>())
            {
                string text = InvoiceText.FormatGroup(classification.ClassName, classification.ClassCode?.Value);
                if (text.Length > 0)
                {
                    s1.AppendLine(text);
                }
            }

            /* The serial number of the article. */
            foreach (var instance in product?.IndividualTradeProductInstance ?? new List<TradeProductInstance>())
            {
                string text = InvoiceText.FormatGroup(
                    InvoiceText.Text(instance.SupplierAssignedSerialId), InvoiceText.Text(instance.BatchId));
                if (text.Length > 0)
                {
                    s1.AppendLine($"{Resources.Seriennummer}: {text}");
                }
            }

            /* The articles in a set, for example the parts of a bundle. */
            foreach (var included in product?.IncludedReferencedProduct ?? new List<ReferencedProduct>())
            {
                string text = InvoiceText.FormatGroup(
                    included.Name,
                    included.Description,
                    InvoiceText.Text(included.SellerAssignedId),
                    InvoiceText.Text(included.BuyerAssignedId),
                    InvoiceText.Text(included.IndustryAssignedId),
                    string.Join(" ", included.GlobalId.Select(id => InvoiceText.Text(id))),
                    InvoiceText.FormatGroup(InvoiceText.FormatQuantity(included.UnitQuantity),
                                            included.UnitQuantity?.UnitCode.ToUnit()));
                if (text.Length > 0)
                {
                    s1.AppendLine($"{Resources.EnthaltenesProdukt}: {text}");
                }
            }
            foreach (var note in item.AssociatedDocumentLineDocument?.IncludedNote ?? new List<Note>())
            {
                s1.AppendLine($"{CodeText.Describe(CodeLists.NoteSubject, note.SubjectCode)} {CodeText.Describe(CodeLists.NoteSubject, note.ContentCode)} {note.Content}");
            }
            InvoiceText.AppendReference(s1, Resources.refOrderDocument, lineAgreement?.BuyerOrderReferencedDocument);
            InvoiceText.AppendReference(s1, Resources.refContractdocument, lineAgreement?.ContractReferencedDocument);
            InvoiceText.AppendReference(s1, Resources.refDeliveryNote, lineDelivery?.DeliveryNoteReferencedDocument);
            InvoiceText.AppendReference(s1, Resources.Angebot, lineAgreement?.QuotationReferencedDocument);
            if (lineDelivery?.ShipToTradeParty != null)
            {
                s1.AppendLine($"{Resources.shipTo}: {InvoiceText.FormatTradeParty(lineDelivery.ShipToTradeParty)}");
            }
            lineAgreement?.AdditionalReferencedDocument?.ForEach(doc => InvoiceText.AppendReference(s1, Resources.ZusaetzlichesDokument, doc));

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

            lineSettlement?.AdditionalReferencedDocument?.ForEach(doc => InvoiceText.AppendReference(s1, Resources.ZusaetzlichesDokument, doc));
            lineSettlement?.ApplicableTradeTax.ForEach(tax => s1.Append(InvoiceText.FormatTradeTax(tax)));

            foreach (var charge in lineAgreement?.GrossPriceProductTradePrice?.AppliedTradeAllowanceCharge ?? new List<TradeAllowanceCharge>())
            {
                InvoiceText.AppendAllowanceCharge(s1, charge);
            }

            return s1.ToString();
        }
    }
}
