using En16931.Model;
using En16931.Model.Extended;
using iText.Layout;
using iText.Layout.Element;
using Pacioli.Language.Resources;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CodeLists = En16931.Codes.CodeLists;

namespace Pacioli.Pdf.Invoice.Sections
{
    /*
     * The table of the positions (BG-25).
     *
     * The table has eight columns: the position number, the description, the tax, the
     * quantity, the net unit price, the gross unit price, the price basis, and the total
     * of the position. The column with the description holds all other data of the
     * position.
     */
    internal sealed class LineItemsSection : IInvoiceSection
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
            foreach (InvoiceLine line in context.Core.Lines)
            {
                TaxDetail? lineTax = line.Extensions?.Tax.FirstOrDefault();

                lineCount++;
                tab.AddCell(new ItemCell($"{line.Id ?? $"({lineCount})"}"));
                tab.AddCell(new ItemCell(BuildDescription(line)));
                tab.AddCell(new ItemCell(
                    $"{line.Vat.Rate?.ToString("0.00")}% {lineTax?.TypeCode.ToTaxTypeName()} {line.Vat.CategoryCode.ToTaxCategoryName()}"));
                tab.AddCell(new ItemCellRight(
                    $"{CoreText.FormatQuantity(line.InvoicedQuantity)} {line.InvoicedQuantity?.UnitCode.ToUnit()}"));
                tab.AddCell(new ItemCellRight($"{Fmt.ToString(line.Price.NetPrice, "#,0.00")}"));
                tab.AddCell(new ItemCellRight($"{Fmt.ToString(line.Price.GrossPrice, "#,0.00")}"));
                tab.AddCell(new ItemCellRight(BuildPriceBasis(line.Price)));
                tab.AddCell(new ItemCellRight($"{Fmt.ToString(line.NetAmount, "#,0.00")}"));
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
        private static string BuildPriceBasis(PriceDetails price)
        {
            string net = CoreText.FormatQuantityWithUnit(price.Extensions?.NetBaseQuantity);
            string gross = CoreText.FormatQuantityWithUnit(price.Extensions?.GrossBaseQuantity);

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

        /* Builds the text of the description column of one position. */
        private static string BuildDescription(InvoiceLine line)
        {
            LineItem item = line.Item;
            InvoiceLineExtensions? extensions = line.Extensions;
            LineItemExtensions? itemExtensions = item.Extensions;

            StringBuilder s1 = new StringBuilder();
            s1.AppendLine(item.Name);

            if (!string.IsNullOrEmpty(item.Description))
            {
                s1.AppendLine(item.Description);
            }

            if (extensions?.ActualDeliveryDate != null)
            {
                s1.AppendLine($"{Resources.deliveryDate}: {extensions.ActualDeliveryDate.Value.ToString("d")}");
            }

            if (!string.IsNullOrWhiteSpace(item.SellerIdentifier))
            {
                s1.AppendLine($"{Resources.sellerAssignedId}: {item.SellerIdentifier}");
            }

            if (item.StandardIdentifier != null)
            {
                s1.AppendLine(
                    $"{CodeText.Name(CodeLists.IdentifierScheme, item.StandardIdentifier.SchemeId)} {item.StandardIdentifier.Value}");
            }

            if (line.Period?.StartDate != null && line.Period?.EndDate != null)
            {
                s1.AppendLine($"{Resources.billingPeriod}: " +
                              $"{line.Period.StartDate.Value.ToString("d")} - {line.Period.EndDate.Value.ToString("d")}");
            }

            if (!string.IsNullOrWhiteSpace(line.InvoicedQuantity?.UnitCode))
            {
                s1.AppendLine($"{Resources.unitQuantity}: {line.InvoicedQuantity!.UnitCode.ToUnit()}");
            }

            foreach (ItemAttribute attribute in item.Attributes)
            {
                s1.AppendLine($"{attribute.Name}: {attribute.Value}");
            }

            /* The classification of the article, for example a tariff number. */
            for (int index = 0; index < item.ClassificationIdentifiers.Count; index++)
            {
                string? className = itemExtensions != null && index < itemExtensions.ClassificationNames.Count
                    ? itemExtensions.ClassificationNames[index]
                    : null;

                string text = CoreText.FormatGroup(className, item.ClassificationIdentifiers[index].Value);
                if (text.Length > 0)
                {
                    s1.AppendLine(text);
                }
            }

            /* The serial number of the article. */
            foreach (ProductInstance instance in itemExtensions?.Instances ?? [])
            {
                string text = CoreText.FormatGroup(instance.SerialId, instance.BatchId);
                if (text.Length > 0)
                {
                    s1.AppendLine($"{Resources.Seriennummer}: {text}");
                }
            }

            /* The articles in a set, for example the parts of a bundle. */
            foreach (IncludedProduct included in itemExtensions?.IncludedProducts ?? [])
            {
                string text = CoreText.FormatGroup(
                    included.Name,
                    included.Description,
                    included.SellerIdentifier,
                    included.BuyerIdentifier,
                    included.IndustryIdentifier,
                    string.Join(" ", included.GlobalIdentifiers.Select(id => id.Value)),
                    CoreText.FormatQuantityWithUnit(included.UnitQuantity));

                if (text.Length > 0)
                {
                    s1.AppendLine($"{Resources.EnthaltenesProdukt}: {text}");
                }
            }

            foreach (InvoiceNote note in NotesOf(line))
            {
                s1.AppendLine($"{CodeText.Describe(CodeLists.NoteSubject, note.SubjectCode)} " +
                              $"{CodeText.Describe(CodeLists.NoteSubject, note.Extensions?.ContentCode)} {note.Text}");
            }

            CoreText.AppendReference(s1, Resources.refOrderDocument, extensions?.PurchaseOrder);
            CoreText.AppendReference(s1, Resources.refContractdocument, extensions?.Contract);
            CoreText.AppendReference(s1, Resources.refDeliveryNote, extensions?.DeliveryNote);
            CoreText.AppendReference(s1, Resources.Angebot, extensions?.Quotation);

            if (extensions?.ShipTo != null)
            {
                s1.AppendLine($"{Resources.shipTo}: {CoreText.FormatParty(extensions.ShipTo)}");
            }

            foreach (SupportingDocument document in extensions?.Documents ?? [])
            {
                CoreText.AppendReference(s1, Resources.ZusaetzlichesDokument, AsReference(document));
            }

            if (!string.IsNullOrWhiteSpace(item.BuyerIdentifier))
            {
                s1.AppendLine($"{Resources.buyerAssignedId}: {item.BuyerIdentifier}");
            }

            if (line.BuyerAccountingReference != null || extensions?.AccountingAccountTypeCode != null)
            {
                s1.AppendLine($"{Resources.account}: {line.BuyerAccountingReference} " +
                              $"{extensions?.AccountingAccountTypeCode}");
            }

            foreach (AllowanceCharge charge in line.Allowances.Concat(line.Charges))
            {
                CoreText.AppendAllowanceCharge(s1, charge);
            }

            foreach (TaxDetail tax in extensions?.Tax ?? [])
            {
                s1.Append(CoreText.FormatTax(tax));
            }

            foreach (AllowanceCharge charge in line.Price.Extensions?.GrossPriceAllowances ?? [])
            {
                CoreText.AppendAllowanceCharge(s1, charge);
            }

            return s1.ToString();
        }

        /*
         * The notes of a position. The extension holds each one; if it holds none, the
         * core gives the one note of EN 16931 (BT-127).
         */
        private static IEnumerable<InvoiceNote> NotesOf(InvoiceLine line)
        {
            if (line.Extensions is { Notes.Count: > 0 })
            {
                return line.Extensions.Notes;
            }

            return line.Note == null ? [] : [new InvoiceNote { Text = line.Note }];
        }

        /* A supporting document as a reference, for the one line form. */
        private static ReferencedDocument AsReference(SupportingDocument document) => new()
        {
            Reference = document.Reference,
            Name = document.Description,
            Uri = document.ExternalLocation,
            IssueDate = document.Extensions?.IssueDate,
            LineId = document.Extensions?.LineId,
            ReferenceTypeCode = document.Extensions?.ReferenceTypeCode
        };
    }
}
