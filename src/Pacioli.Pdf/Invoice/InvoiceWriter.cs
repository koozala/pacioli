using iText.Commons.Actions;
using iText.IO.Font.Constants;
using iText.Kernel.Colors;
using iText.Kernel.Font;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas;
using iText.Kernel.Pdf.Event;
using iText.Layout;
using iText.Layout.Borders;
using iText.Layout.Element;
using iText.Layout.Properties;
using Pacioli.Language.Resources;
using Pacioli.Pdf.ERechnung;
using Pacioli.Pdf.zugferd;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Vit.FeRD.ExtendedDocument;
using Vit.FeRD.ExtendedDocument.Model;

namespace Pacioli.Pdf.Invoice
{
    class FooterEventHandler : IEventHandler
    {
        float _leftMargin, _bottomMargin, _rightMargin, _pageWidth;
        Paragraph _para;

        public FooterEventHandler(float leftMargin, float bottomMargin, float rightMargin, float pageWidth, Paragraph para)
        {
            _leftMargin = leftMargin;
            _bottomMargin = bottomMargin;
            _rightMargin = rightMargin;
            _pageWidth = pageWidth;
            _para = para;
        }

        public void HandleEvent(IEvent ev)
        {
            PdfDocumentEvent docEvent = (PdfDocumentEvent)ev;
            PdfDocument doc = docEvent.GetDocument();
            PdfPage page = docEvent.GetPage();
            PdfCanvas cv = new PdfCanvas(page);

            var c = new Canvas(cv, new iText.Kernel.Geom.Rectangle(_leftMargin, _bottomMargin, _pageWidth - _leftMargin - _rightMargin, 50.0f));
            c.Add(_para);
        }

        public void OnEvent(IEvent @event)
        {
            PdfDocumentEvent docEvent = (PdfDocumentEvent)@event;
            PdfDocument doc = docEvent.GetDocument();
            PdfPage page = docEvent.GetPage();
            PdfCanvas cv = new PdfCanvas(page);

            var c = new Canvas(cv, new iText.Kernel.Geom.Rectangle(_leftMargin, _bottomMargin, _pageWidth - _leftMargin - _rightMargin, 50.0f));
            c.Add(_para);
        }
    }

    /*
     * Writes an invoice to a PDF page.
     *
     * The parser is Vit.FeRD.ExtendedDocument. Its model follows the structure of the
     * Cross Industry Invoice, thus the data sits deeper than in a flat model. The
     * properties Agreement, Delivery, Settlement and Summation give the four groups that
     * this class uses most.
     */
    public class InvoiceWriter
    {
        CrossIndustryInvoice invoice;
        UnitValue full = UnitValue.CreatePercentValue(100.0f);
        PdfFont bold = PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD);
        string sourceFile;

        List<AttachmentDescriptor> attachments = new List<AttachmentDescriptor>();
        Stream XmlData = new MemoryStream();

        public bool IsZugferd { get; private set; }

        public InvoiceWriter(string fileName)
        {
            sourceFile = fileName;

            var zr = new ZugferdReader(fileName);
            IsZugferd = zr.Read(out Stream? data);

            if (IsZugferd)
            {
                invoice = FeRDInvoice.Load(data!);
                data!.Seek(0, SeekOrigin.Begin);
                data!.CopyTo(XmlData);
                data!.Dispose();
            }
            else
            {
                invoice = FeRDInvoice.Load(fileName);
                using var fstream = new FileStream(fileName, FileMode.Open, FileAccess.Read);
                fstream.CopyTo(XmlData);
            }
            XmlData.Seek(0, SeekOrigin.Begin);
        }

        public Stream GetXml() => XmlData;

        public IReadOnlyList<AttachmentDescriptor> GetAttachments() => attachments;

        /* The four groups of the Cross Industry Invoice. */

        SupplyChainTradeTransaction? Transaction => invoice.SupplyChainTradeTransaction;
        HeaderTradeAgreement? Agreement => Transaction?.ApplicableHeaderTradeAgreement;
        HeaderTradeDelivery? Delivery => Transaction?.ApplicableHeaderTradeDelivery;
        HeaderTradeSettlement? Settlement => Transaction?.ApplicableHeaderTradeSettlement;
        TradeSettlementHeaderMonetarySummation? Summation => Settlement?.SpecifiedTradeSettlementHeaderMonetarySummation;
        ExchangedDocument? Document => invoice.ExchangedDocument;

        IList<SupplyChainTradeLineItem> LineItems =>
            Transaction?.IncludedSupplyChainTradeLineItem ?? (IList<SupplyChainTradeLineItem>)Array.Empty<SupplyChainTradeLineItem>();

        public void Write(string outputFileName)
        {
            PdfWriter pdfWriter = new PdfWriter(outputFileName);
            PdfDocument document = new PdfDocument(pdfWriter);
            Document doc = new Document(document);

            var leftMargin = doc.GetLeftMargin();
            var bottomMargin = doc.GetBottomMargin();
            var rightMargin = doc.GetRightMargin();
            PageSize psize = document.GetDefaultPageSize();
            var pageWidth = psize.GetWidth();
            var pageHeight = psize.GetHeight();

            PdfPage page1 = document.AddNewPage();

            Paragraph infoText1 = new Paragraph().SetTextAlignment(TextAlignment.CENTER).SetFontSize(10);
            infoText1.SetBorder(new SolidBorder(0.5f));
            infoText1.Add(string.Format(Resources.infoText1, sourceFile));
            doc.Add(infoText1);


            // Überschrift

            if (!string.IsNullOrWhiteSpace(Document?.Name))
            {
                doc.Add(new Paragraph($"{Document!.Name}").SetTextAlignment(TextAlignment.CENTER).SetFontSize(14).SetFont(bold).SetMarginTop(10.0f));
            }
            doc.Add(new Paragraph($"{TypeCodeMapping.Map(Document?.TypeCode)}").SetTextAlignment(TextAlignment.CENTER).SetFontSize(12).SetFont(bold).SetMarginTop(10.0f));


            // Datum usw.

            DateOnly? invoiceDate = DateOf(Document?.IssueDateTime);
            if (invoiceDate != null)
            {
                string f1 = invoiceDate.Value.ToString("d");
                Paragraph dateLine = new Paragraph($"{Resources.docDate}: {f1}").SetTextAlignment(iText.Layout.Properties.TextAlignment.RIGHT).SetFontSize(12);
                doc.Add(dateLine);
            }

            string? invoiceNo = Text(Document?.Id);
            if (!string.IsNullOrWhiteSpace(invoiceNo))
            {
                Paragraph invoiceNoLine = new Paragraph($"{Resources.invoiceNo}: {invoiceNo}").SetTextAlignment(TextAlignment.RIGHT).SetFontSize(12);
                doc.Add(invoiceNoLine);
            }

            string? orderNo = Text(Agreement?.BuyerOrderReferencedDocument?.IssuerAssignedId);
            if (!string.IsNullOrWhiteSpace(orderNo))
            {
                doc.Add(new Paragraph($"{Resources.orderNo}: {orderNo}").SetTextAlignment(TextAlignment.RIGHT).SetFontSize(12));
            }

            if (Agreement?.SellerTradeParty != null)
            {
                var p1 = new PartyBox($"{Resources.seller}", Agreement.SellerTradeParty);
                doc.Add(p1);
            }

            if (Settlement?.InvoiceeTradeParty != null)
            {
                var p2 = new PartyBox(Resources.invoicee, Settlement.InvoiceeTradeParty);
                doc.Add(p2);
            }

            if (Settlement?.PayeeTradeParty != null)
            {
                var p3 = new PartyBox(Resources.payee, Settlement.PayeeTradeParty);
                doc.Add(p3);
            }

            if (Delivery?.ShipFromTradeParty != null)
            {
                doc.Add(new PartyBox(Resources.shipFrom, Delivery.ShipFromTradeParty));
            }

            if (Delivery?.ShipToTradeParty != null)
            {
                doc.Add(new PartyBox(Resources.shipTo, Delivery.ShipToTradeParty));
            }

            if (Agreement?.BuyerTradeParty != null)
            {
                doc.Add(new PartyBox(Resources.buyer, Agreement.BuyerTradeParty));
            }

            if (!string.IsNullOrWhiteSpace(Agreement?.BuyerReference))
            {
                doc.Add(new Paragraph($"{Resources.refOrder}: {Agreement!.BuyerReference}"));
            }

            // Kundennummer

            StringBuilder reStr = new StringBuilder();
            Identifier? buyerId = Agreement?.BuyerTradeParty?.Id
                .FirstOrDefault(entry => !string.IsNullOrWhiteSpace(entry?.Value));
            if (buyerId != null)
            {
                reStr.Append($"{Resources.customerNo}: ");
                reStr.Append($" {buyerId.Value}");
                reStr.Append($" ({buyerId.SchemeId})");
            }
            if (reStr.Length > 0)
            {
                doc.Add(new Paragraph(reStr.ToString()));
            }

            DateOnly? orderDate = DateOf(Agreement?.BuyerOrderReferencedDocument?.FormattedIssueDateTime);
            if (orderDate != null)
            {
                doc.Add(new Paragraph($"{Resources.orderDate}: {orderDate.Value.ToString("d")}"));
            }

            ProcuringProject? project = Agreement?.SpecifiedProcuringProject;
            if (project != null &&
                (!string.IsNullOrWhiteSpace(project.Name) || !string.IsNullOrWhiteSpace(project.Id?.Value)))
            {
                doc.Add(new Paragraph($"{Resources.projectNo}: {project.Name} {project.Id?.Value}"));
            }

            DateOnly? deliveryDate = DateOf(Delivery?.ActualDeliverySupplyChainEvent?.OccurrenceDateTime);
            if (deliveryDate != null)
            {
                doc.Add(new Paragraph($"{Resources.deliveryDate}: {deliveryDate.Value.ToString("d")}"));
            }

            doc.Add(new Paragraph($"{Resources.currencyNote} {Settlement?.InvoiceCurrencyCode}").SetFont(bold));

            /*
             * Positionen
             */

            Table tab = new Table(7).SetWidth(full);
            tab.AddHeaderCell(new ItemCell($"{Resources.pos}"));
            tab.AddHeaderCell(new ItemCell($"{Resources.description}"));
            tab.AddHeaderCell(new ItemCell($"{Resources.tax}"));
            tab.AddHeaderCell(new ItemCell($"{Resources.amount}"));
            tab.AddHeaderCell(new ItemCell($"{Resources.netUnitPrice}"));
            tab.AddHeaderCell(new ItemCell($"{Resources.grossUnitPrice}"));
            tab.AddHeaderCell(new ItemCell($"{Resources.sum}"));

            int lineCount = 0;
            foreach (var item in LineItems)
            {
                TradeProduct? product = item.SpecifiedTradeProduct;
                LineTradeAgreement? lineAgreement = item.SpecifiedLineTradeAgreement;
                LineTradeDelivery? lineDelivery = item.SpecifiedLineTradeDelivery;
                LineTradeSettlement? lineSettlement = item.SpecifiedLineTradeSettlement;
                TradeTax? lineTax = lineSettlement?.ApplicableTradeTax.FirstOrDefault();
                Quantity? billedQuantity = lineDelivery?.BilledQuantity;

                lineCount++;
                tab.AddCell(new ItemCell($"{lineCount}"));
                {
                    StringBuilder s1 = new StringBuilder();
                    s1.AppendLine(product?.Name);
                    if (!string.IsNullOrEmpty(product?.Description))
                    {
                        s1.AppendLine(product!.Description);
                    }

                    DateOnly? itemDeliveryDate = DateOf(lineDelivery?.ActualDeliverySupplyChainEvent?.OccurrenceDateTime);
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
                    DateOnly? periodStart = DateOf(billingPeriod?.StartDateTime);
                    DateOnly? periodEnd = DateOf(billingPeriod?.EndDateTime);
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
                    AppendReference(s1, Resources.refOrderDocument, lineAgreement?.BuyerOrderReferencedDocument);
                    AppendReference(s1, Resources.refContractdocument, lineAgreement?.ContractReferencedDocument);
                    AppendReference(s1, Resources.refDeliveryNote, lineDelivery?.DeliveryNoteReferencedDocument);

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
                        AppendAllowanceCharge(s1, charge);
                    }
                    foreach (var charge in lineAgreement?.GrossPriceProductTradePrice?.AppliedTradeAllowanceCharge ?? new List<TradeAllowanceCharge>())
                    {
                        AppendAllowanceCharge(s1, charge);
                    }
                    tab.AddCell(new ItemCell(s1.ToString()));
                }
                tab.AddCell(new ItemCell($"{lineTax?.RateApplicablePercent?.ToString("0.00")}% {lineTax?.TypeCode.ToTaxTypeName()} {lineTax?.CategoryCode.ToTaxCategoryName()}"));
                tab.AddCell(new ItemCellRight($"{FormatQuantity(billedQuantity)} {billedQuantity?.UnitCode.ToUnit()}"));
                tab.AddCell(new ItemCellRight($"{Fmt.ToString(lineAgreement?.NetPriceProductTradePrice?.ChargeAmount?.Value, "#,0.00")}"));
                tab.AddCell(new ItemCellRight($"{Fmt.ToString(lineAgreement?.GrossPriceProductTradePrice?.ChargeAmount?.Value, "#,0.00")}"));
                tab.AddCell(new ItemCellRight($"{Fmt.ToString(lineSettlement?.SpecifiedTradeSettlementLineMonetarySummation?.LineTotalAmount?.Value, "#,0.00")}"));
            }
            doc.Add(tab);

            foreach (var refdoc in Agreement?.AdditionalReferencedDocument ?? new List<ReferencedDocument>())
            {
                AddAttachment(doc, refdoc);
            }

            foreach (var item in LineItems)
            {
                foreach (var refdoc in item.SpecifiedLineTradeSettlement?.AdditionalReferencedDocument ?? new List<ReferencedDocument>())
                {
                    AddAttachment(doc, refdoc);
                }
                foreach (var refdoc in item.SpecifiedLineTradeAgreement?.AdditionalReferencedDocument ?? new List<ReferencedDocument>())
                {
                    AddAttachment(doc, refdoc);
                }
            }


            doc.Add(new Paragraph($"{Resources.taxSums}:").SetMarginTop(20.0f));
            Table taxTab = new Table(6).SetFontSize(12).SetWidth(full);
            taxTab.AddHeaderCell($"{Resources.tax}");
            taxTab.AddHeaderCell($"{Resources.taxCategory}");
            taxTab.AddHeaderCell($"{Resources.taxBasis}");
            taxTab.AddHeaderCell($"{Resources.taxAmount}");
            taxTab.AddHeaderCell($"{Resources.exception}");
            taxTab.AddHeaderCell($"{Resources.allowanceChargeBasis}");
            foreach (var tax in Settlement?.ApplicableTradeTax ?? new List<TradeTax>())
            {
                taxTab.AddCell(new ItemCellRight($"{tax.RateApplicablePercent?.ToString("0.00")}"));
                taxTab.AddCell(new ItemCellRight($"{tax.TypeCode.ToTaxTypeName()} {tax.CategoryCode.ToTaxCategoryName()}"));
                taxTab.AddCell(new ItemCellRight($"{tax.BasisAmount?.Value}"));
                taxTab.AddCell(new ItemCellRight($"{tax.CalculatedAmount?.Value}"));
                taxTab.AddCell(new ItemCellRight($"{tax.ExemptionReason} {tax.ExemptionReasonCode?.Value}"));
                taxTab.AddCell(new ItemCellRight($"{tax.AllowanceChargeBasisAmount?.Value}"));
            }
            doc.Add(taxTab);

            var headerCharges = Settlement?.SpecifiedTradeAllowanceCharge ?? new List<TradeAllowanceCharge>();
            if (headerCharges.Count > 0)
            {
                doc.Add(new Paragraph($"Rabatte und Zuschläge").SetMarginTop(20.0f));
                TradeAllowanceChargesTab chargeTab = new TradeAllowanceChargesTab();
                chargeTab.Add(headerCharges);
                doc.Add(chargeTab.GetTab());
            }


            doc.Add(new Paragraph($"{Resources.sumOverview}:").SetMarginTop(20.0f));
            Table sumTab = new Table(2).SetFontSize(12).SetWidth(full).SetBorder(Border.NO_BORDER);
            sumTab.AddCell(new ItemCell($"{Resources.prepaid}"));
            sumTab.AddCell(new ItemCellRight($"{Fmt.ToString(Summation?.TotalPrepaidAmount?.Value, "#,0.00")}"));
            sumTab.AddCell(new ItemCell($"{Resources.allowance}"));
            sumTab.AddCell(new ItemCellRight($"{Fmt.ToString(Summation?.AllowanceTotalAmount?.Value, "#,0.00")}"));
            sumTab.AddCell(new ItemCell($"{Resources.charge}"));
            sumTab.AddCell(new ItemCellRight($"{Fmt.ToString(Summation?.ChargeTotalAmount?.Value, "#,0.00")}"));
            sumTab.AddCell(new ItemCell($"{Resources.taxBasis}"));
            sumTab.AddCell(new ItemCellRight($"{Fmt.ToString(Summation?.TaxBasisTotalAmount?.Value, "#,0.00")}"));
            sumTab.AddCell(new ItemCell($"{Resources.taxAmount}"));
            sumTab.AddCell(new ItemCellRight($"{Fmt.ToString(Summation?.TaxTotalAmount.FirstOrDefault()?.Value, "#,0.00")}"));
            sumTab.AddCell(new ItemCell($"{Resources.totalSum}"));
            sumTab.AddCell(new ItemCellRight($"{Fmt.ToString(Summation?.GrandTotalAmount?.Value, "#,0.00")}"));
            sumTab.AddCell(new ItemCell($"{Resources.roundingAmount}"));
            sumTab.AddCell(new ItemCellRight($"{Fmt.ToString(Summation?.RoundingAmount?.Value, "#,0.00")}"));
            sumTab.AddCell(new ItemCell($"{Resources.payableAmount}"));
            sumTab.AddCell(new ItemCellRight($"{Fmt.ToString(Summation?.DuePayableAmount?.Value, "#,0.00")}"));

            doc.Add(sumTab);


            foreach (var means in Settlement?.SpecifiedTradeSettlementPaymentMeans ?? new List<TradeSettlementPaymentMeans>())
            {
                CreditorFinancialAccount? account = means.PayeePartyCreditorFinancialAccount;
                if (account == null)
                {
                    continue;
                }

                StringBuilder sb = new StringBuilder();
                sb.Append($"{Resources.accountHolder}: {account.AccountName}");
                sb.Append($"\nIBAN: {account.IbanId?.Value}");
                sb.Append($"\nBIC: {means.PayeeSpecifiedCreditorFinancialInstitution?.BicId?.Value}");
                if (!string.IsNullOrWhiteSpace(account.ProprietaryId?.Value))
                {
                    sb.Append($"\n{account.ProprietaryId!.Value}");
                }
                var box = new PartyBox(Resources.bankverbindung, sb.ToString());
                doc.Add(box);
            }

            foreach (var means in Settlement?.SpecifiedTradeSettlementPaymentMeans ?? new List<TradeSettlementPaymentMeans>())
            {
                DebtorFinancialAccount? account = means.PayerPartyDebtorFinancialAccount;
                if (account == null)
                {
                    continue;
                }

                var box = new PartyBox(Resources.bankverbindung,
                    $"{account.AccountName}\n{account.IbanId?.Value}\n{means.PayerSpecifiedDebtorFinancialInstitution?.BicId?.Value}");
                doc.Add(box);
            }

            StringBuilder pmStr = new StringBuilder();
            foreach (var means in Settlement?.SpecifiedTradeSettlementPaymentMeans ?? new List<TradeSettlementPaymentMeans>())
            {
                if (!string.IsNullOrWhiteSpace(means.Information)) pmStr.AppendLine(means.Information);
                if (means.ApplicableTradeSettlementFinancialCard != null)
                {
                    pmStr.AppendLine($"{Resources.card}: {means.ApplicableTradeSettlementFinancialCard.Id?.Value} " +
                                     $"{means.ApplicableTradeSettlementFinancialCard.CardholderName}");
                }
            }
            if (!string.IsNullOrWhiteSpace(Settlement?.CreditorReferenceId?.Value))
            {
                pmStr.AppendLine(Settlement!.CreditorReferenceId!.Value);
            }
            foreach (var terms in Settlement?.SpecifiedTradePaymentTerms ?? new List<TradePaymentTerms>())
            {
                if (!string.IsNullOrWhiteSpace(terms.DirectDebitMandateId?.Value))
                {
                    pmStr.AppendLine($"{Resources.sepaMandate}: {terms.DirectDebitMandateId!.Value}");
                }
            }
            if (pmStr.Length > 0)
            {
                var paybox = new PartyBox($"{Resources.paymentTerms}", pmStr.ToString());
                doc.Add(paybox);
            }


            UniversalCommunication? buyerAddress = Agreement?.BuyerTradeParty?.UriUniversalCommunication;
            if (buyerAddress?.UriId != null)
            {
                doc.Add(new Paragraph($"{buyerAddress.UriId.SchemeId} {buyerAddress.UriId.Value}"));
            }

            foreach (var pt in Settlement?.SpecifiedTradePaymentTerms ?? new List<TradePaymentTerms>())
            {
                StringBuilder sb = new StringBuilder();
                if (!string.IsNullOrWhiteSpace(pt.Description)) sb.Append(pt.Description);

                DateOnly? dueDate = DateOf(pt.DueDateDateTime);
                if (dueDate != null) sb.Append($"\n{Resources.dueDate}: {dueDate.Value.ToString("d")}");

                Measure? dueDays = pt.ApplicableTradePaymentDiscountTerms?.BasisPeriodMeasure;
                if (dueDays != null) sb.Append($"\n{Resources.dueDate}: {dueDays.Value} {Resources.days}");

                if (sb.Length > 0)
                {
                    var tbox = new PartyBox($"{Resources.dueDate}", sb.ToString());
                    doc.Add(tbox);
                }
            }

            TradeContact? sellerContact = Agreement?.SellerTradeParty?.DefinedTradeContact.FirstOrDefault();
            if (sellerContact != null)
            {
                StringBuilder scStr = new StringBuilder();
                AppendContact(scStr, sellerContact);
                if (scStr.Length > 0)
                {
                    doc.Add(new PartyBox($"{Resources.contactSeller}", scStr.ToString()));
                }
            }

            TradeContact? buyerContact = Agreement?.BuyerTradeParty?.DefinedTradeContact.FirstOrDefault();
            if (buyerContact != null)
            {
                StringBuilder bcStr = new StringBuilder();
                AppendContact(bcStr, buyerContact);
                if (bcStr.Length > 0)
                {
                    doc.Add(new PartyBox($"{Resources.contactBuyer}", bcStr.ToString()));
                }
            }


            var notes = Document?.IncludedNote ?? new List<Note>();
            if (notes.Count > 0)
            {
                doc.Add(new Paragraph($"{Resources.remarks}").SetFont(bold).SetFontSize(12.0f).SetMarginTop(20.0f));

                foreach (var note in notes)
                {
                    StringBuilder sb = new StringBuilder();
                    AddContentCodeLine(sb, note.ContentCode);
                    AddSubjectCodeLine(sb, note.SubjectCode);
                    if (sb.Length > 0)
                    {
                        Paragraph codeLine = new Paragraph(sb.ToString()).SetFontSize(11f);
                        codeLine.SetBackgroundColor(ColorConstants.CYAN);
                        doc.Add(codeLine);
                    }

                    Paragraph line = new Paragraph($"{note.Content}").SetFontSize(11f);
                    line.SetBackgroundColor(ColorConstants.LIGHT_GRAY);
                    doc.Add(line);
                }
            }

            doc.Close();
        }

        /* Writes one line for an attached document, and keeps the attachment for the caller. */
        void AddAttachment(Document doc, ReferencedDocument refdoc)
        {
            BinaryObject? binary = refdoc.AttachmentBinaryObject;

            doc.Add(new Paragraph($"{Resources.attachment}: {refdoc.Name} {binary?.FileName}"));

            attachments.Add(new AttachmentDescriptor
            {
                FileName = binary?.FileName ?? string.Empty,
                Name = refdoc.Name,
                Data = binary?.Value,
                MimeType = binary?.MimeCode
            });
        }

        void AppendReference(StringBuilder sb, string caption, ReferencedDocument? refdoc)
        {
            if (refdoc == null)
            {
                return;
            }

            string t = $"{caption}: {refdoc.IssuerAssignedId?.Value}";
            DateOnly? issued = DateOf(refdoc.FormattedIssueDateTime);
            if (issued != null)
            {
                t += $" {issued.Value.ToString("d")}";
            }
            sb.AppendLine(t);
        }

        void AppendAllowanceCharge(StringBuilder sb, TradeAllowanceCharge charge)
        {
            string zuschlag = charge.ChargeIndicator ? "Zuschlag" : "Abschlag";
            sb.AppendLine(
                $"{zuschlag}: {Fmt.ToStringWithUnit(charge.CalculationPercent, "0.0", "%")} " +
                $"{Fmt.ToString(charge.ActualAmount?.Value, "#,#0.00")} {charge.ActualAmount?.CurrencyId} " +
                $"{Fmt.ToStringWithName("Basis:", charge.BasisAmount?.Value, "#,#0.00")} {charge.Reason}");
        }

        void AppendContact(StringBuilder sb, TradeContact contact)
        {
            if (!string.IsNullOrWhiteSpace(contact.PersonName)) sb.AppendLine(contact.PersonName);
            if (!string.IsNullOrWhiteSpace(contact.DepartmentName)) sb.AppendLine(contact.DepartmentName);

            string? mail = contact.EmailUriUniversalCommunication?.UriId?.Value;
            if (!string.IsNullOrWhiteSpace(mail)) sb.AppendLine(mail);

            string? phone = contact.TelephoneUniversalCommunication?.CompleteNumber;
            if (!string.IsNullOrWhiteSpace(phone)) sb.AppendLine($"{Resources.phone}: {phone}");

            string? fax = contact.FaxUniversalCommunication?.CompleteNumber;
            if (!string.IsNullOrWhiteSpace(fax)) sb.AppendLine($"{Resources.fax}: {fax}");
        }

        void AddContentCodeLine(StringBuilder sb, Code? code)
        {
            if (!string.IsNullOrWhiteSpace(code?.Value))
            {
                if (sb.Length > 0) sb.Append(", ");
                sb.Append($"Inhaltscodierung: {code!.Value}");
            }
        }

        void AddSubjectCodeLine(StringBuilder sb, Code? code)
        {
            if (!string.IsNullOrWhiteSpace(code?.Value))
            {
                if (sb.Length > 0) sb.Append(", ");
                sb.Append($"Betreffcodierung: {code!.Value}");
            }
        }

        /* The value of an identifier, or null. */
        static string? Text(Identifier? id) => id?.Value;

        /* The date of a CII date, or null if the document gives none. */
        static DateOnly? DateOf(CiiDate? date) => date?.Date;

        /* The renderer selects the number of decimal places from the value. */
        static string FormatQuantity(Quantity? quantity)
        {
            if (quantity == null)
            {
                return string.Empty;
            }

            return quantity.Value.ToString(DecimalExtensions.PrecFmt(quantity.Value));
        }
    }
}
