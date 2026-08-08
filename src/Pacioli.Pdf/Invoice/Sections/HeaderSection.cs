using iText.Kernel.Colors;
using iText.Layout;
using iText.Layout.Borders;
using iText.Layout.Element;
using iText.Layout.Properties;
using Pacioli.Language.Resources;
using System;

namespace Pacioli.Pdf.Invoice.Sections
{
    /*
     * The head of the page: the name of the source file, the title of the document,
     * the date, the invoice number, and the order number. Then comes a group with the
     * references of the document, in a light frame.
     */
    internal sealed class HeaderSection : IInvoiceSection
    {
        public string Name => "Header";

        public void Write(Document doc, InvoiceContext context)
        {
            Paragraph infoText1 = new Paragraph().SetTextAlignment(TextAlignment.CENTER).SetFontSize(10);
            infoText1.SetBorder(new SolidBorder(0.5f));
            infoText1.Add(string.Format(Resources.infoText1, context.SourceFile));
            doc.Add(infoText1);

            // Überschrift

            if (!string.IsNullOrWhiteSpace(context.Document?.Name))
            {
                doc.Add(new Paragraph($"{context.Document!.Name}")
                    .SetTextAlignment(TextAlignment.CENTER).SetFontSize(14).SetFont(context.Bold).SetMarginTop(10.0f));
            }
            doc.Add(new Paragraph($"{TypeCodeMapping.Map(context.Document?.TypeCode)}")
                .SetTextAlignment(TextAlignment.CENTER).SetFontSize(12).SetFont(context.Bold).SetMarginTop(10.0f));

            // Datum usw.

            DateOnly? invoiceDate = InvoiceText.DateOf(context.Document?.IssueDateTime);
            if (invoiceDate != null)
            {
                string f1 = invoiceDate.Value.ToString("d");
                Paragraph dateLine = new Paragraph($"{Resources.docDate}: {f1}")
                    .SetTextAlignment(TextAlignment.RIGHT).SetFontSize(12);
                doc.Add(dateLine);
            }

            string? invoiceNo = InvoiceText.Text(context.Document?.Id);
            if (!string.IsNullOrWhiteSpace(invoiceNo))
            {
                Paragraph invoiceNoLine = new Paragraph($"{Resources.invoiceNo}: {invoiceNo}")
                    .SetTextAlignment(TextAlignment.RIGHT).SetFontSize(12);
                doc.Add(invoiceNoLine);
            }

            string? orderNo = InvoiceText.Text(context.Agreement?.BuyerOrderReferencedDocument?.IssuerAssignedId);
            if (!string.IsNullOrWhiteSpace(orderNo))
            {
                doc.Add(new Paragraph($"{Resources.orderNo}: {orderNo}")
                    .SetTextAlignment(TextAlignment.RIGHT).SetFontSize(12));
            }


            /*
             * The references of the document.
             *
             * The items come from ApplicableHeaderTradeAgreement and from
             * ApplicableHeaderTradeSettlement. A light frame holds them together, thus
             * the reader sees them as one group.
             *
             * The group goes to the page only if it has an item. Without this test, an
             * invoice without these data would get an empty frame.
             */

            Div referenceGroup = new Div()
                .SetBorder(new SolidBorder(ColorConstants.LIGHT_GRAY, 0.5f))
                .SetPadding(5.0f)
                .SetMarginTop(10.0f);

            int referenceCount = 0;

            void AddReference(string text)
            {
                referenceGroup.Add(new Paragraph(text)
                    .SetTextAlignment(TextAlignment.RIGHT).SetFontSize(12));
                referenceCount++;
            }


            // ApplicableHeaderTradeAgreement

            var agreement = context.Invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement;

            if (agreement.ContractReferencedDocument != null)
            {
                var contract = agreement.ContractReferencedDocument;
                if (contract.IssuerAssignedId != null)
                {
                    AddReference($"Vertragsnummer: {contract.IssuerAssignedId}");
                }
            }

            if (agreement.BuyerReference != null)
            {
                AddReference($"Bestellreferenz: {agreement.BuyerReference}");
            }

            if (agreement.QuotationReferencedDocument != null)
            {
                AddReference($"Angebotsnummer: {agreement.QuotationReferencedDocument.IssuerAssignedId}");
            }

            if (agreement.SellerOrderReferencedDocument != null)
            {
                AddReference($"Auftragsnummer: {agreement.SellerOrderReferencedDocument.IssuerAssignedId}");
            }

            if (agreement.SpecifiedProcuringProject != null)
            {
                var project = agreement.SpecifiedProcuringProject;
                if (!string.IsNullOrWhiteSpace(project.Name) || !string.IsNullOrWhiteSpace(project.Id?.Value))
                {
                    AddReference($"Projekt: {project.Name} {project.Id?.Value}");
                }
            }


            // ApplicableHeaderTradeSettlement

            var settlement = context.Invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement;

            if (settlement.BillingSpecifiedPeriod != null)
            {
                var period = settlement.BillingSpecifiedPeriod;
                if (period.StartDateTime != null && period.EndDateTime != null)
                {
                    string startDate = InvoiceText.DateOf(period.StartDateTime)?.ToString("d") ?? "";
                    string endDate = InvoiceText.DateOf(period.EndDateTime)?.ToString("d") ?? "";
                    AddReference($"Abrechnungszeitraum: {Resources.billingPeriod}: {startDate} - {endDate}");
                }
                else
                {
                    string completeDate = InvoiceText.DateOf(period.CompleteDateTime)?.ToString("d") ?? "";
                    AddReference($"Abrechnungszeitraum: {Resources.billingPeriod}: {completeDate}");
                }
            }

            if (settlement.PaymentReference != null)
            {
                AddReference($"Zahlungsreferenz: {settlement.PaymentReference}");
            }

            if (referenceCount > 0)
            {
                doc.Add(referenceGroup);
            }
        }
    }
}
