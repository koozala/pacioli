using En16931.Model;
using En16931.Model.Extended;
using iText.Kernel.Colors;
using iText.Layout;
using iText.Layout.Borders;
using iText.Layout.Element;
using iText.Layout.Properties;
using Pacioli.Language.Resources;
using System.Text;
using CodeLists = En16931.Codes.CodeLists;

/* Invoice is also the name of the namespace Pacioli.Pdf.Invoice, thus it needs an alias. */
using CoreInvoice = En16931.Model.Invoice;

namespace Pacioli.Pdf.Invoice.Sections
{
    /*
     * The three groups of the head of the transaction, each one in a frame:
     * the agreement, the settlement, and the delivery.
     */
    internal sealed class TradeAgreementSection : IInvoiceSection
    {
        public string Name => "Trade Agreement";

        public void Write(Document doc, InvoiceContext context)
        {
            WriteAgreement(doc, context.Core);
            WriteSettlement(doc, context.Core);
            WriteDelivery(doc, context.Core);
        }

        void WriteDelivery(Document doc, CoreInvoice invoice)
        {
            InvoiceExtensions? extensions = invoice.Extensions;

            Div group = new Div()
                .SetBorder(new SolidBorder(ColorConstants.BLUE, 0.5f))
                .SetPadding(5.0f)
                .SetMarginTop(10.0f);
            int count = 0;

            void AddItem(string text)
            {
                group.Add(new Paragraph(text)
                    .SetTextAlignment(TextAlignment.RIGHT).SetFontSize(12));
                count++;
            }

            if (extensions?.DeliveryNote != null)
            {
                ReferencedDocument note = extensions.DeliveryNote;
                AddItem($"{Resources.Lieferschein}: {CoreText.FormatGroup(
                    note.Reference, note.Name, note.Uri, note.IssueDate.ToString())}");
            }

            if (invoice.Delivery?.ActualDeliveryDate != null)
            {
                AddItem($"{Resources.Lieferdatum}: {invoice.Delivery.ActualDeliveryDate}");
            }

            foreach (string mode in extensions?.TransportModeCodes ?? [])
            {
                AddItem($"{Resources.Transportart}: {CodeText.Name(CodeLists.TransportMode, mode)}");
            }

            if (extensions?.ShipFrom != null)
            {
                AddItem($"{Resources.LieferungAb}: {extensions.ShipFrom.Name}");
            }

            if (extensions?.DespatchAdvice != null)
            {
                ReferencedDocument advice = extensions.DespatchAdvice;
                AddItem($"{Resources.Versandhinweis}: {CoreText.FormatGroup(
                    advice.Reference, advice.Name, advice.Uri, advice.IssueDate.ToString())}");
            }

            if (count > 0)
            {
                doc.Add(group);
            }
        }

        void WriteAgreement(Document doc, CoreInvoice invoice)
        {
            InvoiceExtensions? extensions = invoice.Extensions;

            Div agreementGroup = new Div()
                .SetBorder(new SolidBorder(ColorConstants.LIGHT_GRAY, 0.5f))
                .SetPadding(5.0f)
                .SetMarginTop(10.0f);
            int count = 0;

            void AddReference(string text)
            {
                agreementGroup.Add(new Paragraph(text).SetFontSize(12));
                count++;
            }

            /*
             * Each document that the head of the invoice names. The list holds the group
             * as the document gives it: the invoiced object (BT-18) and the tender or lot
             * (BT-17) stand in the same group as a supporting document (BG-24).
             */
            var documents = extensions?.AdditionalDocuments ?? [];

            if (documents.Count > 0)
            {
                AddReference($"{Resources.Dokumente}:");

                foreach (ReferencedDocument document in documents)
                {
                    Table tab = new Table(6);
                    tab.SetBorder(Border.NO_BORDER);
                    tab.AddCell(new Cell().Add(new Paragraph("*")).SetBorder(Border.NO_BORDER));
                    tab.AddCell(new Cell().Add(new Paragraph($"{document.Reference}")).SetBorder(Border.NO_BORDER));
                    tab.AddCell(new Cell().Add(new Paragraph($"{document.Name}")).SetBorder(Border.NO_BORDER));
                    tab.AddCell(new Cell().Add(new Paragraph($"{document.IssueDate}")).SetBorder(Border.NO_BORDER));
                    tab.AddCell(new Cell().Add(new Paragraph($"{document.Uri}")).SetBorder(Border.NO_BORDER));
                    tab.AddCell(new Cell().Add(new Paragraph($"{Resources.pos} {document.LineId}")).SetBorder(Border.NO_BORDER));
                    agreementGroup.Add(tab);
                    count++;
                }
            }

            if (invoice.ContractReference != null)
            {
                AddReference($"{Resources.Vertragsnummer}: {invoice.ContractReference}");
            }

            if (invoice.BuyerReference != null)
            {
                AddReference($"{Resources.Bestellnummer}: {invoice.BuyerReference}");
            }

            if (extensions?.Quotation != null)
            {
                AddReference($"{Resources.Angebotsnummer}: {extensions.Quotation.Reference}");
            }

            if (invoice.SalesOrderReference != null)
            {
                AddReference($"{Resources.Auftragsnummer}: {invoice.SalesOrderReference}");
            }

            if (!string.IsNullOrWhiteSpace(extensions?.ProjectName)
                || !string.IsNullOrWhiteSpace(invoice.ProjectReference))
            {
                AddReference($"{Resources.Projekt}: {extensions?.ProjectName} {invoice.ProjectReference}");
            }

            Party? taxRep = invoice.SellerTaxRepresentative;
            if (taxRep != null)
            {
                var p = new Paragraph($"{Resources.Steuervertreter}:\n");

                if (taxRep.AdditionalLegalInformation != null)
                {
                    p.Add(taxRep.AdditionalLegalInformation + "\n");
                }
                if (!string.IsNullOrWhiteSpace(taxRep.Name))
                {
                    p.Add(taxRep.Name + "\n");
                }
                if (taxRep.PostalAddress != null)
                {
                    p.Add(CoreText.FormatAddress(taxRep.PostalAddress) + "\n");
                }
                foreach (Contact contact in taxRep.Extensions?.Contacts ?? [])
                {
                    p.Add(CoreText.FormatContact(contact) + "\n");
                }
                if (taxRep.Extensions == null && taxRep.Contact != null)
                {
                    p.Add(CoreText.FormatContact(taxRep.Contact) + "\n");
                }
                foreach (Identifier registration in taxRep.Extensions?.TaxRegistrations ?? [])
                {
                    p.Add($"{Resources.SteuerId}: {registration.Value}\n");
                }

                string legal = CoreText.FormatLegalOrganization(taxRep);
                if (legal.Length > 0)
                {
                    p.Add(legal + "\n");
                }

                if (taxRep.ElectronicAddress != null)
                {
                    p.Add(taxRep.ElectronicAddress.Value + "\n");
                    if (taxRep.Extensions?.ElectronicAddressNumber != null)
                    {
                        p.Add(taxRep.Extensions.ElectronicAddressNumber + "\n");
                    }
                }

                agreementGroup.Add(p);
                count++;
            }

            if (count > 0)
            {
                doc.Add(agreementGroup);
            }
        }

        void WriteSettlement(Document doc, CoreInvoice invoice)
        {
            InvoiceExtensions? extensions = invoice.Extensions;

            Div settlementGroup = new Div()
                .SetBorder(new SolidBorder(ColorConstants.GREEN, 0.5f))
                .SetPadding(5.0f)
                .SetMarginTop(10.0f);

            StringBuilder sb = new StringBuilder();

            int count = 0;

            void AddItem(string text)
            {
                //settlementGroup.Add(new Paragraph(text).SetFontSize(12));
                sb.AppendLine(text);
                count++;
            }

            Period? period = invoice.InvoicingPeriod;
            if (period != null)
            {
                string startDate = period.StartDate?.ToString("d") ?? "";
                string endDate = period.EndDate?.ToString("d") ?? "";
                AddItem($"{Resources.billingPeriod}: {startDate} - {endDate}");
            }

            if (invoice.PaymentInstructions?.RemittanceInformation != null)
            {
                AddItem($"{Resources.Zahlungsreferenz}: {invoice.PaymentInstructions.RemittanceInformation}");
                count++;
            }

            if (invoice.BuyerAccountingReference != null || extensions?.AccountingAccountTypeCode != null)
            {
                AddItem($"{Resources.Buchungsreferenz}: {invoice.BuyerAccountingReference} " +
                        $"{extensions?.AccountingAccountTypeCode ?? string.Empty}");
                count++;
            }

            if (invoice.PrecedingInvoiceReferences.Count > 0)
            {
                AddItem($"{Resources.Rechnungsreferenzen}:");

                foreach (PrecedingInvoiceReference reference in invoice.PrecedingInvoiceReferences)
                {
                    AddItem(CoreText.FormatReferencedDocument(new ReferencedDocument
                    {
                        Reference = reference.InvoiceNumber,
                        Name = reference.Extensions?.Name,
                        IssueDate = reference.IssueDate,
                        Uri = reference.Extensions?.Uri,
                        ReferenceTypeCode = reference.Extensions?.ReferenceTypeCode,
                        LineId = reference.Extensions?.LineId
                    }, hasAttachment: false));
                    count++;
                }
            }

            foreach (AdvancePayment payment in extensions?.AdvancePayments ?? [])
            {
                AddItem(CoreText.FormatAdvancePayment(payment));
                count++;
            }

            /*
             * The penalty terms (Verzugszinsen): what the buyer pays if the payment is
             * late. A payment term holds them beside the discount terms.
             */
            foreach (PaymentTerm term in extensions?.PaymentTerms ?? [])
            {
                string penalty = PaymentTermsSection.FormatCondition(term.Penalty);
                if (penalty.Length > 0)
                {
                    AddItem($"{Resources.Verzugszinsen}: {penalty}");
                }
            }

            if (count > 0)
            {
                settlementGroup.Add(new Paragraph(sb.ToString()).SetFontSize(12));
                doc.Add(settlementGroup);
            }
        }
    }
}
