using iText.Kernel.Colors;
using iText.Layout;
using iText.Layout.Borders;
using iText.Layout.Element;
using iText.Layout.Properties;
using Pacioli.Language.Resources;
using System;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using Vit.FeRD.ExtendedDocument.Model;

namespace Pacioli.Pdf.Invoice.Sections
{
    internal sealed class TradeAgreementSection : IInvoiceSection
    {
        public string Name => "Trade Agreement";

        public void Write(Document doc, InvoiceContext context)
        {
            WriteApplicableHeaderTradeAgreement(doc, context.Invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement);
            WriteApplicableHeaderTradeSettlement(doc, context.Invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement);
            WriteApplicableHeaderTradeDelivery(doc, context.Invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeDelivery);
        }

        void WriteApplicableHeaderTradeDelivery(Document doc, HeaderTradeDelivery delivery)
        {
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

            if (delivery.DeliveryNoteReferencedDocument != null)
            {
                AddItem($"{Resources.Lieferschein}: {InvoiceText.FormatGroup(InvoiceText.Text(delivery.DeliveryNoteReferencedDocument.IssuerAssignedId),
                    delivery.DeliveryNoteReferencedDocument.Name,
                    InvoiceText.Text(delivery.DeliveryNoteReferencedDocument.UriId),
                    InvoiceText.DateOf(delivery.DeliveryNoteReferencedDocument.FormattedIssueDateTime).ToString())}");
            }

            if (delivery.ActualDeliverySupplyChainEvent != null)
            {
                AddItem($"{Resources.Lieferdatum}: {InvoiceText.DateOf(delivery.ActualDeliverySupplyChainEvent.OccurrenceDateTime)}");
            }

            if (delivery.RelatedSupplyChainConsignment != null)
            {
                delivery.RelatedSupplyChainConsignment.SpecifiedLogisticsTransportMovement.ForEach(movement =>
                {
                    AddItem($"{Resources.Transportart}: {CodeText.Name(CodeLists.TransportMode, movement.ModeCode)}");
                });
            }

            if (delivery.ShipFromTradeParty != null)
            {
                AddItem($"{Resources.LieferungAb}: {delivery.ShipFromTradeParty.Name}");
            }

            if (delivery.DespatchAdviceReferencedDocument != null)
            {
                AddItem($"{Resources.Versandhinweis}: {InvoiceText.FormatGroup(InvoiceText.Text(delivery.DespatchAdviceReferencedDocument.IssuerAssignedId),
                    delivery.DespatchAdviceReferencedDocument.Name,
                    InvoiceText.Text(delivery.DespatchAdviceReferencedDocument.UriId),
                    InvoiceText.DateOf(delivery.DespatchAdviceReferencedDocument.FormattedIssueDateTime).ToString())}");
            }

            if (count > 0)
            {
                doc.Add(group);
            }
        }

        void WriteApplicableHeaderTradeAgreement(Document doc, HeaderTradeAgreement agreement)
        {
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

            if (agreement.AdditionalReferencedDocument.Count > 0)
            {
                AddReference($"{Resources.Dokumente}:");
                foreach (var addDoc in agreement.AdditionalReferencedDocument)
                {
                    Table tab = new Table(6);
                    tab.SetBorder(Border.NO_BORDER);
                    tab.AddCell(new Cell().Add(new Paragraph("*")).SetBorder(Border.NO_BORDER));
                    tab.AddCell(new Cell().Add(new Paragraph($"{InvoiceText.Text(addDoc.IssuerAssignedId)}")).SetBorder(Border.NO_BORDER));
                    tab.AddCell(new Cell().Add(new Paragraph($"{addDoc.Name}")).SetBorder(Border.NO_BORDER));
                    tab.AddCell(new Cell().Add(new Paragraph($"{InvoiceText.DateOf(addDoc.FormattedIssueDateTime)}")).SetBorder(Border.NO_BORDER));
                    tab.AddCell(new Cell().Add(new Paragraph($"{InvoiceText.Text(addDoc.UriId)}")).SetBorder(Border.NO_BORDER));
                    tab.AddCell(new Cell().Add(new Paragraph($"{Resources.pos} {InvoiceText.Text(addDoc.LineId)}")).SetBorder(Border.NO_BORDER));
                    agreementGroup.Add(tab);
                    count++;
                }
            }

            if (agreement.ContractReferencedDocument != null)
            {
                var contract = agreement.ContractReferencedDocument;
                if (contract.IssuerAssignedId != null)
                {
                    AddReference($"{Resources.Vertragsnummer}: {contract.IssuerAssignedId}");
                }
            }

            if (agreement.BuyerReference != null)
            {
                AddReference($"{Resources.Bestellnummer}: {agreement.BuyerReference}");
            }

            if (agreement.QuotationReferencedDocument != null)
            {
                AddReference($"{Resources.Angebotsnummer}: {agreement.QuotationReferencedDocument.IssuerAssignedId}");
            }

            if (agreement.SellerOrderReferencedDocument != null)
            {
                AddReference($"{Resources.Auftragsnummer}: {agreement.SellerOrderReferencedDocument.IssuerAssignedId}");
            }

            if (agreement.SpecifiedProcuringProject != null)
            {
                var project = agreement.SpecifiedProcuringProject;
                if (!string.IsNullOrWhiteSpace(project.Name) || !string.IsNullOrWhiteSpace(project.Id?.Value))
                {
                    AddReference($"{Resources.Projekt}: {project.Name} {project.Id?.Value}");
                }
            }

            if (agreement.SellerTaxRepresentativeTradeParty != null)
            {
                var taxRep = agreement.SellerTaxRepresentativeTradeParty;
                var p = new Paragraph($"{Resources.Steuervertreter}:\n");
                if (taxRep.Description != null)
                {
                    p.Add(taxRep.Description+ "\n");
                }
                if (!string.IsNullOrWhiteSpace(taxRep.Name))
                {
                    p.Add(taxRep.Name+ "\n");
                }
                if (taxRep.PostalTradeAddress != null)
                {
                    p.Add(InvoiceText.FormatAddress(taxRep.PostalTradeAddress)+ "\n");
                }
                foreach (var contact in taxRep.DefinedTradeContact)
                {
                    p.Add(InvoiceText.FormatTradeContact(contact)+ "\n");
                }
                foreach (var reg in taxRep.SpecifiedTaxRegistration)
                {
                    p.Add($"{Resources.SteuerId}: {InvoiceText.Text(reg.Id)}\n");
                }
                if (taxRep.SpecifiedLegalOrganization != null)
                {
                    p.Add(InvoiceText.FormatLegalOrg(taxRep.SpecifiedLegalOrganization)+ "\n");
                }
                if (taxRep.UriUniversalCommunication != null)
                {
                    p.Add(InvoiceText.Text(taxRep.UriUniversalCommunication.UriId)+ "\n");
                    if (taxRep.UriUniversalCommunication.CompleteNumber != null)
                    {
                        p.Add(taxRep.UriUniversalCommunication.CompleteNumber+ "\n");
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

        void WriteApplicableHeaderTradeSettlement(Document doc, HeaderTradeSettlement settlement)
        {
            Div settlementGroup = new Div()
                .SetBorder(new SolidBorder(ColorConstants.GREEN, 0.5f))
                .SetPadding(5.0f)
                .SetMarginTop(10.0f);

            int count = 0;

            void AddItem(string text)
            {
                settlementGroup.Add(new Paragraph(text).SetFontSize(12));
                count++;
            }

            if (settlement.BillingSpecifiedPeriod != null)
            {
                var period = settlement.BillingSpecifiedPeriod;
                if (period.StartDateTime != null && period.EndDateTime != null)
                {
                    string startDate = InvoiceText.DateOf(period.StartDateTime)?.ToString("d") ?? "";
                    string endDate = InvoiceText.DateOf(period.EndDateTime)?.ToString("d") ?? "";
                    AddItem($"{Resources.billingPeriod}: {startDate} - {endDate}");
                }
                else
                {
                    string completeDate = InvoiceText.DateOf(period.CompleteDateTime)?.ToString("d") ?? "";
                    AddItem($"{Resources.billingPeriod}: {completeDate}");
                }
            }


            if (settlement.PaymentReference != null)
            {
                AddItem($"{Resources.Zahlungsreferenz}: {settlement.PaymentReference}");
                count++;
            }

            foreach (var account in settlement.ReceivableSpecifiedTradeAccountingAccount)
            {
                AddItem($"{Resources.Buchungsreferenz}: {InvoiceText.Text(account.Id)} {account.TypeCode ?? string.Empty}");
                count++;
            }

            if (settlement.InvoiceReferencedDocument.Count > 0)
            {
                AddItem($"{Resources.Rechnungsreferenzen}:");
                foreach (var invDoc in settlement.InvoiceReferencedDocument)
                {
                    AddItem(InvoiceText.FormatReferencedDocument(invDoc));
                    count++;
                }
            }

            settlement.SpecifiedAdvancePayment.ForEach(payment =>
            {
                AddItem(InvoiceText.FormatAdvancePayment(payment));
                count++;
            });

            /*
             * The penalty terms (Verzugszinsen): what the buyer pays if the payment is
             * late. A payment term holds them beside the discount terms.
             */
            foreach (var terms in settlement.SpecifiedTradePaymentTerms)
            {
                string penalty = FormatPenaltyTerms(terms?.ApplicableTradePaymentPenaltyTerms);
                if (penalty.Length > 0)
                {
                    AddItem($"{Resources.Verzugszinsen}: {penalty}");
                }
            }

            if (count > 0)
            {
                doc.Add(settlementGroup);
            }
        }

        /*
         * The percentage, the amount of the penalty, the amount that it applies to, the
         * number of days, and the day from which the days count. A part that the
         * document does not give makes no text.
         */
        static string FormatPenaltyTerms(TradePaymentPenaltyTerms? penalty)
        {
            if (penalty == null)
            {
                return string.Empty;
            }

            return InvoiceText.FormatGroup(
                penalty.CalculationPercent.HasValue
                    ? penalty.CalculationPercent.Value.ToString("0.00") + "%"
                    : null,
                InvoiceText.FormatAmount(penalty.ActualPenaltyAmount),
                penalty.BasisAmount == null
                    ? null
                    : $"{Resources.basis}: {InvoiceText.FormatAmount(penalty.BasisAmount)}",
                penalty.BasisPeriodMeasure == null
                    ? null
                    : $"{penalty.BasisPeriodMeasure.Value} {Resources.days}",
                InvoiceText.DateOf(penalty.BasisDateTime)?.ToString("d"));
        }
    }
}
