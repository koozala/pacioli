using iText.IO.Font.Constants;
using iText.Kernel.Colors;
using iText.Kernel.Events;
using iText.Kernel.Font;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas;
using iText.Layout;
using iText.Layout.Borders;
using iText.Layout.Element;
using iText.Layout.Properties;
using s2industries.ZUGFeRD;
using System.IO;
using System.Linq;
using System.Text;

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

        public void HandleEvent(Event ev)
        {
            PdfDocumentEvent docEvent = (PdfDocumentEvent)ev;
            PdfDocument doc = docEvent.GetDocument();
            PdfPage page = docEvent.GetPage();
            PdfCanvas cv = new PdfCanvas(page);

            var c = new Canvas(cv, new iText.Kernel.Geom.Rectangle(_leftMargin, _bottomMargin, _pageWidth - _leftMargin - _rightMargin, 50.0f));
            c.Add(_para);
        }
    }

    public class InvoiceWriter
    {
        InvoiceDescriptor descriptor;
        UnitValue full = UnitValue.CreatePercentValue(100.0f);
        PdfFont bold = PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD);

        public InvoiceWriter(InvoiceDescriptor _descriptor)
        {
            descriptor = _descriptor;
        }

        public InvoiceWriter(string fileName)
        {
            descriptor = InvoiceDescriptor.Load(fileName);
        }

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

            //var regNote = descriptor.Notes.SingleOrDefault(x => x.SubjectCode == SubjectCodes.REG);
            //if (regNote != null)
            //{
            //    Paragraph regLine = new Paragraph($"{regNote.Content}").SetFontSize(8).SetTextAlignment(TextAlignment.CENTER).SetFixedPosition(0.0f, 20.0f, full);
            //    document.AddEventHandler(PdfDocumentEvent.END_PAGE, new FooterEventHandler(leftMargin, bottomMargin, rightMargin, pageWidth, regLine));
            //}

            //doc.SetBottomMargin(150.0f);

            PdfPage page1 = document.AddNewPage();

            Paragraph header = new Paragraph().SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER).SetFontSize(16);
            header.Add("Rechnung");
            doc.Add(header);

            if (descriptor.InvoiceDate != null)
            {
                string f1 = descriptor.InvoiceDate.Value.ToString("d");
                Paragraph dateLine = new Paragraph($"Datum: {f1}").SetTextAlignment(iText.Layout.Properties.TextAlignment.RIGHT).SetFontSize(14);
                doc.Add(dateLine);
            }

            var p1 = new PartyBox("Verkäufer", descriptor.Seller);
            doc.Add(p1);

            //var bold = PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD);
            //Text t1 = new Text("Verkäufer\n").SetFont(bold);
            //Paragraph seller = new Paragraph()
            //    .Add(t1)
            //    .Add(new Text($"{descriptor.Seller.Name}\n{descriptor.Seller.Street}\n{descriptor.Seller.Postcode} {descriptor.Seller.City}\n{descriptor.Seller.Country}"))
            //    .SetPadding(5.0f)
            //    .SetBorder(new SolidBorder(2.0f));
            //doc.Add(seller);

            if (descriptor.Invoicee != null)
            {
                var p2 = new PartyBox("Rechnungsempfänger", descriptor.Invoicee);
                doc.Add(p2);
            }

            //if (descriptor.Invoicer != null)
            //{
            //    var p2 = new PartyBox("Rechnung ausgestellt", descriptor.Invoicer);
            //    doc.Add(p2);
            //}

            if (descriptor.Payee != null)
            {
                var p3 = new PartyBox("Zahlung durch", descriptor.Buyer);
                doc.Add(p3);
            }

            if (descriptor.ShipFrom != null)
            {
                doc.Add(new PartyBox("Warenempfänger", descriptor.ShipTo));
            }

            if (descriptor.ShipTo != null)
            {
                doc.Add(new PartyBox("Lieferung durch", descriptor.ShipTo));
            }

            Paragraph numberLine = new Paragraph($"Rechnungsnummer: {descriptor.InvoiceNo}  Kundennr.: {descriptor.Buyer.ID.ID}")
                .SetTextAlignment(iText.Layout.Properties.TextAlignment.LEFT).SetFontSize(14);
            doc.Add(numberLine);

            doc.Add(new Paragraph($"Bestellnr.: {descriptor.OrderNo}"));
            if (descriptor.OrderDate.HasValue) doc.Add(new Paragraph($"Bestelldatum: {descriptor.OrderDate}"));

            if (descriptor.SpecifiedProcuringProject != null)
            {
                doc.Add(new Paragraph($"Projekt: {descriptor.SpecifiedProcuringProject.Name} {descriptor.SpecifiedProcuringProject.ID}"));
            }

            if (descriptor.ActualDeliveryDate.HasValue)
            {
                doc.Add(new Paragraph($"Lieferdatum: {descriptor.ActualDeliveryDate.Value.ToString("d")}"));
            }

            doc.Add(new Paragraph($"Alle Beträge in {descriptor.Currency}"));

            Table tab = new Table(7).SetWidth(full);
            tab.AddHeaderCell(new ItemCell("Pos."));
            tab.AddHeaderCell(new ItemCell("Beschreibung"));
            tab.AddHeaderCell(new ItemCell("Steuersatz"));
            tab.AddHeaderCell(new ItemCell("Menge"));
            tab.AddHeaderCell(new ItemCell("Einzelpreis"));
            tab.AddHeaderCell(new ItemCell("Brutto-Einzelpreis"));
            tab.AddHeaderCell(new ItemCell("Summe"));

            int lineCount = 0;
            foreach (var item in descriptor.TradeLineItems)
            {
                lineCount++;
                tab.AddCell(new ItemCell($"{lineCount}"));
                {
                    StringBuilder s1 = new StringBuilder();
                    s1.AppendLine(item.Name);
                    if (!string.IsNullOrEmpty(item.Description))
                    {
                        s1.AppendLine(item.Description);
                    }
                    if (item.ActualDeliveryDate.HasValue)
                    {
                        s1.AppendLine($"Lieferung: {item.ActualDeliveryDate.Value.ToString("d")}");
                    }
                    if (!string.IsNullOrWhiteSpace(item.SellerAssignedID))
                    {
                        s1.AppendLine($"Artikelnummer: {item.SellerAssignedID}");
                    }

                    if (item.GlobalID != null)
                    {
                        s1.AppendLine($"{item.GlobalID.SchemeID} {item.GlobalID.ID}");
                    }
                    if (item.BillingPeriodStart.HasValue)
                    {
                        s1.AppendLine($"Abrechnungszeitraum: {item.BillingPeriodStart.Value.ToString("d")} - {item.BillingPeriodEnd.Value.ToString("d")}");
                    }
                    if (item.UnitQuantity.HasValue && item.UnitQuantity.Value != 1m)
                    {
                        s1.AppendLine($"Einheit: {item.UnitQuantity.Value.ToString(DecimalExtensions.PrecFmt(item.UnitQuantity.Value))}");
                    }
                    foreach (var attr in item.ApplicableProductCharacteristics)
                    {
                        s1.AppendLine($"{attr.Description}: {attr.Value}");
                    }
                    tab.AddCell(new ItemCell(s1.ToString()));
                }
                tab.AddCell(new ItemCell($"{item.TaxPercent.ToString("0.00")}% {item.TaxType.ToUnit()} {item.TaxCategoryCode.ToUnit()}"));
                tab.AddCell(new ItemCellRight($"{item.BilledQuantity.ToString(DecimalExtensions.PrecFmt(item.BilledQuantity))} {item.UnitCode.ToUnit()}"));
                tab.AddCell(new ItemCellRight($"{Fmt.ToString(item.NetUnitPrice, "#,0.00")}"));
                tab.AddCell(new ItemCellRight($"{Fmt.ToString(item.GrossUnitPrice, "#,0.00")}"));
                tab.AddCell(new ItemCellRight($"{Fmt.ToString(item.LineTotalAmount, "#,0.00")}"));
            }
            doc.Add(tab);

            doc.Add(new Paragraph("Anmerkungen zu den Positionen:"));
            int countNotes = 0;
            foreach (var item in descriptor.TradeLineItems)
            {
                doc.Add(new Paragraph($"{item.AssociatedDocument.LineID}:"));
                if (item.AssociatedDocument != null && item.AssociatedDocument.Notes.Any())
                {
                    countNotes++;
                    foreach (var note in item.AssociatedDocument.Notes)
                    {
                        doc.Add(new Paragraph($"{note.SubjectCode} {note.ContentCode} {note.Content}"));
                    }
                }
                if (item.AdditionalReferencedDocuments.Count > 0)
                {
                    foreach (var refdoc in item.AdditionalReferencedDocuments)
                    {
                        string targetPath = @"c:\tmp";
                        File.WriteAllBytes(System.IO.Path.Combine(targetPath, refdoc.Filename), refdoc.AttachmentBinaryObject);
                        doc.Add(new Paragraph($"Attached document: {refdoc.Name} {refdoc.Filename}"));
                    }
                }
                if (item.BuyerOrderReferencedDocument != null)
                {
                    doc.Add(new Paragraph($"Referenz Bestelldokument: {item.BuyerOrderReferencedDocument.ID} {item.BuyerOrderReferencedDocument.IssueDateTime.Value.ToString("d")}"));
                }
                if (item.ContractReferencedDocument != null)
                {
                    doc.Add(new Paragraph($"Referenz Vertrag: {item.ContractReferencedDocument.ID} {item.ContractReferencedDocument.IssueDateTime.Value.ToString("d")}"));
                }
                if (item.DeliveryNoteReferencedDocument != null)
                {
                    doc.Add(new Paragraph($"Referenz Lieferschein: {item.DeliveryNoteReferencedDocument.ID} {item.DeliveryNoteReferencedDocument.IssueDateTime.Value.ToString("d")}"));
                }
                if (!string.IsNullOrWhiteSpace(item.BuyerAssignedID))
                {
                    doc.Add(new Paragraph($"Artikelnummer Käufer: {item.BuyerAssignedID}"));
                }
                foreach (var acct in item.ReceivableSpecifiedTradeAccountingAccounts)
                {
                    doc.Add(new Paragraph($"Konto: {acct.TradeAccountID} {acct.TradeAccountTypeCode}"));
                }
            }
            doc.Add(new Paragraph($"{countNotes} Anmerkungen"));


            doc.Add(new Paragraph("Steuersummen:").SetMarginTop(20.0f));
            Table taxTab = new Table(6).SetFontSize(12).SetWidth(full);
            taxTab.AddHeaderCell("Steuersatz");
            taxTab.AddHeaderCell("Kategorie");
            taxTab.AddHeaderCell("Basis");
            taxTab.AddHeaderCell("Betrag");
            taxTab.AddHeaderCell("Ausnahme");
            taxTab.AddHeaderCell("AllwChgBasis");
            foreach (var tax in descriptor.Taxes)
            {
                taxTab.AddCell(new ItemCellRight($"{tax.Percent.ToString("0.00")}"));
                taxTab.AddCell(new ItemCellRight($"{tax.TypeCode.ToUnit()} {tax.CategoryCode.Value.ToUnit()}"));
                taxTab.AddCell(new ItemCellRight($"{tax.BasisAmount}"));
                taxTab.AddCell(new ItemCellRight($"{tax.TaxAmount}"));
                taxTab.AddCell(new ItemCellRight($"{tax.ExemptionReason} {tax.ExemptionReasonCode}"));
                taxTab.AddCell(new ItemCellRight($"{tax.AllowanceChargeBasisAmount}"));
                //Paragraph line = new Paragraph($"{tax.BasisAmount} {tax.TaxAmount} {tax.Percent} {tax.CategoryCode} {tax.ExemptionReason} {tax.ExemptionReasonCode} {tax.AllowanceChargeBasisAmount} {tax.CategoryCode}");
                //doc.Add(line);
            }
            doc.Add(taxTab);




            doc.Add(new Paragraph("Summenübersicht:").SetMarginTop(20.0f));
            Table sumTab = new Table(2).SetFontSize(12).SetWidth(full).SetBorder(Border.NO_BORDER);
            sumTab.AddCell(new ItemCell("Vorauszahlung"));
            sumTab.AddCell(new ItemCellRight($"{Fmt.ToString(descriptor.TotalPrepaidAmount, "#,0.00")}"));
            sumTab.AddCell(new ItemCell("Abzüge"));
            sumTab.AddCell(new ItemCellRight($"{Fmt.ToString(descriptor.AllowanceTotalAmount, "#,0.00")}"));
            sumTab.AddCell(new ItemCell("Zuschläge"));
            sumTab.AddCell(new ItemCellRight($"{Fmt.ToString(descriptor.ChargeTotalAmount, "#,0.00")}"));
            sumTab.AddCell(new ItemCell("Steuerbasis"));
            sumTab.AddCell(new ItemCellRight($"{Fmt.ToString(descriptor.TaxBasisAmount, "#,0.00")}"));
            sumTab.AddCell(new ItemCell("Steuerbetrag"));
            sumTab.AddCell(new ItemCellRight($"{Fmt.ToString(descriptor.TaxTotalAmount, "#,0.00")}"));
            sumTab.AddCell(new ItemCell("Gesamtsumme"));
            sumTab.AddCell(new ItemCellRight($"{Fmt.ToString(descriptor.GrandTotalAmount, "#,0.00")}"));
            sumTab.AddCell(new ItemCell("Rundungsbetrag"));
            sumTab.AddCell(new ItemCellRight($"{Fmt.ToString(descriptor.RoundingAmount, "#,0.00")}"));
            sumTab.AddCell(new ItemCell("Zu zahlen"));
            sumTab.AddCell(new ItemCellRight($"{Fmt.ToString(descriptor.DuePayableAmount, "#,0.00")}"));

            doc.Add(sumTab);


            foreach (var bank in descriptor.CreditorBankAccounts)
            {
                var box = new PartyBox(bank.BankName, $"{bank.Name}\n{bank.IBAN}\n{bank.BIC}\n{bank.Bankleitzahl}\n{bank.ID}");
                doc.Add(box);
            }

            foreach (var bank in descriptor.DebitorBankAccounts)
            {
                var box = new PartyBox(bank.BankName, $"{bank.Name}\n{bank.IBAN}\n{bank.BIC}\n{bank.Bankleitzahl}\n{bank.ID}");
                doc.Add(box);
            }

            var pm = descriptor.PaymentMeans;
            var paybox = new PartyBox("Zahlbedingungen", $"{pm.Information}\n{pm.TypeCode}\n{pm.SEPACreditorIdentifier}\n{pm.FinancialCard}\n{pm.SEPAMandateReference}");
            doc.Add(paybox);

            foreach (var acct in descriptor.DebitorBankAccounts)
            {
                Paragraph line = new Paragraph($"{acct.Name} {acct.IBAN} {acct.BankName}");
                doc.Add(line);
            }

            Paragraph termsLine = new Paragraph($"{descriptor.PaymentTerms.Description} {descriptor.PaymentTerms.DueDate}").SetMarginTop(20.0f);
            doc.Add(termsLine);

            Paragraph paymentLine = new Paragraph($"{descriptor.PaymentMeans.Information} {descriptor.PaymentMeans.SEPACreditorIdentifier} {descriptor.PaymentMeans.SEPAMandateReference}");
            doc.Add(paymentLine);


            if (descriptor.BuyerElectronicAddress != null)
            {
                doc.Add(new Paragraph($"{descriptor.BuyerElectronicAddress.ElectronicAddressSchemeID} {descriptor.BuyerElectronicAddress.Address}"));
            }

            var pt = descriptor.PaymentTerms;
            if (pt != null)
            {
                StringBuilder sb = new StringBuilder();
                if (!string.IsNullOrWhiteSpace(pt.Description)) sb.Append(pt.Description);
                if (pt.DueDate.HasValue) sb.Append($" {pt.DueDate.Value.ToString("d")}");
                var tbox = new PartyBox("Fälligkeit", sb.ToString());
                doc.Add(tbox);
            }

            if (descriptor.SellerContact != null)
            {
                var scbox = new PartyBox("Kontaktdaten Verkäufer", $"{descriptor.SellerContact.Name}\n{descriptor.SellerContact.OrgUnit}\n{descriptor.SellerContact.EmailAddress}\n{descriptor.SellerContact.PhoneNo}\n{descriptor.SellerContact.FaxNo}");
                doc.Add(scbox);
            }

            if (descriptor.BuyerContact != null)
            {
                var bcbox = new PartyBox("Kontaktdaten Käufer", $"{descriptor.BuyerContact.Name}\n{descriptor.BuyerContact.OrgUnit}\n{descriptor.BuyerContact.EmailAddress}\n{descriptor.BuyerContact.PhoneNo}\n{descriptor.BuyerContact.FaxNo}");
                doc.Add(bcbox);
            }


            doc.Add(new Paragraph("Anmerkungen").SetFont(bold).SetFontSize(12.0f).SetMarginTop(20.0f));

            foreach (var note in descriptor.Notes.Where(x => x.SubjectCode == SubjectCodes.Unknown))
            {
                Paragraph line = new Paragraph($"{note.Content}");
                line.SetBackgroundColor(ColorConstants.LIGHT_GRAY);
                doc.Add(line);
            }

            foreach (var note in descriptor.Notes.Where(x => x.SubjectCode != SubjectCodes.Unknown))
            {
                string code = note.SubjectCode == SubjectCodes.Unknown ? "[Anmerkung]" : note.SubjectCode.ToString();
                Paragraph line = new Paragraph($"{note.Content}");
                line.SetBackgroundColor(ColorConstants.LIGHT_GRAY);
                doc.Add(line);
            }



            //for (int pageNo = 1; pageNo <= document.GetNumberOfPages(); pageNo++)
            //{
            //    var page = document.GetPage(pageNo);
            //    page.SetPageLabel(PageLabelNumberingStyle.DECIMAL_ARABIC_NUMERALS, "Seite");
            //    PdfCanvas cv = new PdfCanvas(page);
            //    var c = new Canvas(cv, new Rectangle(leftMargin, bottomMargin, 200.0f, 10.0f));
            //    Paragraph line = new Paragraph($"Seite {pageNo}").SetFontSize(9.0f);
            //    c.Add(line);
            //}


            doc.Close();
        }
    }
}
