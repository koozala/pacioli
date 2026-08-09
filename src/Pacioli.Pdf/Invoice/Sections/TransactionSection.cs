using iText.Kernel.Colors;
using iText.Layout;
using iText.Layout.Borders;
using iText.Layout.Element;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vit.FeRD.ExtendedDocument.Model;

namespace Pacioli.Pdf.Invoice.Sections
{
    /*
     * The positions of the invoice, as text.
     *
     * The table of LineItemsSection shows the numbers of a position in columns. This
     * section writes the full content of a position in words. Thus the page also shows
     * the data that has no column: the identification of the article, the price basis,
     * the references, and the notes.
     *
     * Each position gets its own frame.
     */
    internal sealed class TransactionSection : IInvoiceSection
    {
        public string Name => "SupplyChainTradeTransaction";

        public void Write(Document doc, InvoiceContext context)
        {
            if (context.Transaction == null)
            {
                return;
            }

            var lineSection = new TransactionLineItemSection();
            int number = 0;

            foreach (var item in context.Transaction.IncludedSupplyChainTradeLineItem)
            {
                number++;
                lineSection.Write(doc, context, item, number);
            }
        }
    }

    /*
     * One position of the invoice, as text.
     *
     * The class writes a line for each value that the position gives. A value that the
     * position does not give makes no line. Thus a position of the profile BASIC gets a
     * short block, and a position of the profile EXTENDED gets a long block.
     */
    internal sealed class TransactionLineItemSection
    {
        public void Write(Document doc, InvoiceContext context, SupplyChainTradeLineItem item, int number)
        {
            TradeProduct? product = item.SpecifiedTradeProduct;
            LineTradeAgreement? agreement = item.SpecifiedLineTradeAgreement;
            LineTradeDelivery? delivery = item.SpecifiedLineTradeDelivery;
            LineTradeSettlement? settlement = item.SpecifiedLineTradeSettlement;
            DocumentLineDocument? lineDocument = item.AssociatedDocumentLineDocument;

            Div group = new Div()
                .SetBorder(new SolidBorder(ColorConstants.GRAY, 0.5f))
                .SetPadding(5.0f)
                .SetMarginTop(10.0f);

            int count = 0;

            void AddLine(string? text)
            {
                if (string.IsNullOrWhiteSpace(text))
                {
                    return;
                }
                group.Add(new Paragraph(text).SetFontSize(11));
                count++;
            }

            void AddValue(string label, string? value)
            {
                if (!string.IsNullOrWhiteSpace(value))
                {
                    AddLine($"{label}: {value}");
                }
            }

            /* The title of the position. */

            string lineId = InvoiceText.Text(lineDocument?.LineId) ?? number.ToString();
            group.Add(new Paragraph($"Position {lineId}").SetFont(context.Bold).SetFontSize(12));
            count++;

            AddValue("Übergeordnete Position", InvoiceText.Text(lineDocument?.ParentLineId));
            AddValue("Art der Position", InvoiceText.FormatGroup(
                lineDocument?.LineStatusCode, lineDocument?.LineStatusReasonCode?.Value));

            /* The article. */

            AddLine(product?.Name);
            AddLine(product?.Description);

            AddValue("Artikelnummer des Verkäufers", InvoiceText.Text(product?.SellerAssignedId));
            AddValue("Artikelnummer des Käufers", InvoiceText.Text(product?.BuyerAssignedId));
            AddValue("Artikelnummer der Branche", InvoiceText.Text(product?.IndustryAssignedId));
            AddValue("Globale Artikelnummer", InvoiceText.FormatGroup(
                product?.GlobalId?.SchemeId, product?.GlobalId?.Value));
            AddValue("Modell", InvoiceText.FormatGroup(product?.ModelName, InvoiceText.Text(product?.ModelId)));
            AddValue("Marke", product?.BrandName);
            AddValue("Herkunftsland", product?.OriginTradeCountry?.Id);

            foreach (var batch in product?.BatchId ?? new List<Identifier>())
            {
                AddValue("Charge", InvoiceText.Text(batch));
            }

            foreach (var instance in product?.IndividualTradeProductInstance ?? new List<TradeProductInstance>())
            {
                AddValue("Seriennummer", InvoiceText.FormatGroup(
                    InvoiceText.Text(instance.SupplierAssignedSerialId), InvoiceText.Text(instance.BatchId)));
            }

            foreach (var attribute in product?.ApplicableProductCharacteristic ?? new List<ProductCharacteristic>())
            {
                AddValue(string.IsNullOrWhiteSpace(attribute.Description) ? "Merkmal" : attribute.Description,
                    InvoiceText.FormatGroup(attribute.Value, FormatMeasure(attribute.ValueMeasure)));
            }

            foreach (var classification in product?.DesignatedProductClassification ?? new List<ProductClassification>())
            {
                AddValue("Klassifizierung", InvoiceText.FormatGroup(
                    classification.ClassName, classification.ClassCode?.Value));
            }

            foreach (var included in product?.IncludedReferencedProduct ?? new List<ReferencedProduct>())
            {
                AddValue("Enthaltenes Produkt", InvoiceText.FormatGroup(
                    included.Name,
                    included.Description,
                    InvoiceText.Text(included.SellerAssignedId),
                    InvoiceText.Text(included.BuyerAssignedId),
                    InvoiceText.Text(included.IndustryAssignedId),
                    string.Join(" ", included.GlobalId.Select(id => InvoiceText.Text(id))),
                    FormatQuantity(included.UnitQuantity)));
            }

            /* The quantity and the price. */

            AddValue("Menge", FormatQuantity(delivery?.BilledQuantity));
            AddValue("Gratismenge", FormatQuantity(delivery?.ChargeFreeQuantity));
            AddValue("Anzahl der Packstücke", FormatQuantity(delivery?.PackageQuantity));

            AddValue("Nettopreis", FormatPrice(agreement?.NetPriceProductTradePrice));
            AddValue("Bruttopreis", FormatPrice(agreement?.GrossPriceProductTradePrice));

            foreach (var charge in agreement?.GrossPriceProductTradePrice?.AppliedTradeAllowanceCharge
                                  ?? new List<TradeAllowanceCharge>())
            {
                AddLine(FormatAllowanceCharge(charge));
            }

            /* The tax and the sum. */

            foreach (var tax in settlement?.ApplicableTradeTax ?? new List<TradeTax>())
            {
                AddValue("Steuer", InvoiceText.FormatGroup(
                    tax.RateApplicablePercent?.ToString("0.00") + "%",
                    tax.TypeCode.ToTaxTypeName(),
                    tax.CategoryCode.ToTaxCategoryName()));
                AddValue("Steuerbefreiung", InvoiceText.FormatGroup(
                    tax.ExemptionReason, tax.ExemptionReasonCode?.Value));
            }

            foreach (var charge in settlement?.SpecifiedTradeAllowanceCharge ?? new List<TradeAllowanceCharge>())
            {
                AddLine(FormatAllowanceCharge(charge));
            }

            var summation = settlement?.SpecifiedTradeSettlementLineMonetarySummation;
            AddValue("Positionssumme", FormatAmount(summation?.LineTotalAmount));
            AddValue("Summe der Zuschläge", FormatAmount(summation?.ChargeTotalAmount));
            AddValue("Summe der Abschläge", FormatAmount(summation?.AllowanceTotalAmount));

            /* The delivery and the period. */

            AddValue("Lieferdatum", FormatDate(delivery?.ActualDeliverySupplyChainEvent?.OccurrenceDateTime));
            AddValue("Leistungszeitraum", FormatPeriod(settlement?.BillingSpecifiedPeriod));

            AddValue("Lieferanschrift", FormatParty(delivery?.ShipToTradeParty));
            AddValue("Endgültige Lieferanschrift", FormatParty(delivery?.UltimateShipToTradeParty));

            /* The references of the position. */

            AddValue("Bestellung des Käufers", FormatReference(agreement?.BuyerOrderReferencedDocument));
            AddValue("Auftrag des Verkäufers", FormatReference(agreement?.SellerOrderReferencedDocument));
            AddValue("Angebot", FormatReference(agreement?.QuotationReferencedDocument));
            AddValue("Vertrag", FormatReference(agreement?.ContractReferencedDocument));
            AddValue("Lieferschein", FormatReference(delivery?.DeliveryNoteReferencedDocument));
            AddValue("Versandhinweis", FormatReference(delivery?.DespatchAdviceReferencedDocument));
            AddValue("Rechnungsreferenz", FormatReference(settlement?.InvoiceReferencedDocument));

            foreach (var refdoc in agreement?.AdditionalReferencedDocument ?? new List<ReferencedDocument>())
            {
                AddValue("Weiteres Dokument", FormatReference(refdoc));
            }

            foreach (var refdoc in settlement?.AdditionalReferencedDocument ?? new List<ReferencedDocument>())
            {
                AddValue("Weiteres Dokument", FormatReference(refdoc));
            }

            foreach (var account in settlement?.ReceivableSpecifiedTradeAccountingAccount
                                   ?? new List<TradeAccountingAccount>())
            {
                AddValue("Buchungsreferenz", InvoiceText.FormatGroup(
                    InvoiceText.Text(account.Id), account.TypeCode));
            }

            /* The notes of the position. */

            foreach (var note in lineDocument?.IncludedNote ?? new List<Note>())
            {
                AddValue("Hinweis", InvoiceText.FormatGroup(
                    note.SubjectCode?.Value, note.ContentCode?.Value, note.Content));
            }

            if (count > 0)
            {
                doc.Add(group);
            }
        }

        /* "9,90 EUR" for an amount, or an empty text. */
        private static string FormatAmount(Amount? amount)
        {
            if (amount == null)
            {
                return string.Empty;
            }

            return InvoiceText.FormatGroup(amount.Value.ToString("#,0.00"), amount.CurrencyId);
        }

        /* "20 Anz." for a quantity, or an empty text. */
        private static string FormatQuantity(Quantity? quantity)
        {
            if (quantity == null)
            {
                return string.Empty;
            }

            return InvoiceText.FormatGroup(InvoiceText.FormatQuantity(quantity), quantity.UnitCode.ToUnit());
        }

        private static string FormatMeasure(Measure? measure)
        {
            if (measure == null)
            {
                return string.Empty;
            }

            return InvoiceText.FormatGroup(measure.Value.ToString("0.####"), measure.UnitCode.ToUnit());
        }

        /*
         * "9,90 EUR je 100 Anz." for a price.
         * The basis quantity tells for how many units the price applies.
         */
        private static string FormatPrice(TradePrice? price)
        {
            if (price == null)
            {
                return string.Empty;
            }

            string amount = FormatAmount(price.ChargeAmount);
            string basis = FormatQuantity(price.BasisQuantity);

            if (amount.Length == 0)
            {
                return string.Empty;
            }

            return basis.Length == 0 ? amount : $"{amount} je {basis}";
        }

        private static string FormatDate(CiiDate? date) => InvoiceText.DateOf(date)?.ToString("d") ?? string.Empty;

        private static string FormatPeriod(SpecifiedPeriod? period)
        {
            if (period == null)
            {
                return string.Empty;
            }

            string start = FormatDate(period.StartDateTime);
            string end = FormatDate(period.EndDateTime);

            if (start.Length > 0 && end.Length > 0)
            {
                return $"{start} - {end}";
            }

            return InvoiceText.FormatGroup(start, end, FormatDate(period.CompleteDateTime));
        }

        /* The number of a document, with its position, its name, and its date. */
        private static string FormatReference(ReferencedDocument? refdoc)
        {
            if (refdoc == null)
            {
                return string.Empty;
            }

            string? lineId = InvoiceText.Text(refdoc.LineId);

            return InvoiceText.FormatGroup(
                InvoiceText.Text(refdoc.IssuerAssignedId),
                string.IsNullOrWhiteSpace(lineId) ? null : $"Pos. {lineId}",
                refdoc.Name,
                FormatDate(refdoc.FormattedIssueDateTime),
                InvoiceText.Text(refdoc.UriId));
        }

        /* The name and the address of a party, in one line for each part. */
        private static string FormatParty(TradeParty? party)
        {
            if (party == null)
            {
                return string.Empty;
            }

            var parts = new StringBuilder();

            string ids = string.Join(" ", party.Id.Select(id => InvoiceText.Text(id))
                                                 .Where(text => !string.IsNullOrWhiteSpace(text)));

            parts.Append(InvoiceText.FormatGroup(party.Name, ids));

            if (party.PostalTradeAddress != null)
            {
                string address = InvoiceText.FormatAddress(party.PostalTradeAddress).Trim();
                if (address.Length > 0)
                {
                    if (parts.Length > 0)
                    {
                        parts.Append('\n');
                    }
                    parts.Append(address);
                }
            }

            return parts.ToString();
        }

        private static string FormatAllowanceCharge(TradeAllowanceCharge charge)
        {
            string caption = charge.ChargeIndicator ? "Zuschlag" : "Abschlag";

            return $"{caption}: " + InvoiceText.FormatGroup(
                charge.CalculationPercent.HasValue ? charge.CalculationPercent.Value.ToString("0.0") + "%" : null,
                FormatAmount(charge.ActualAmount),
                charge.BasisAmount != null ? "Basis: " + FormatAmount(charge.BasisAmount) : null,
                charge.Reason,
                charge.ReasonCode?.Value);
        }
    }
}
