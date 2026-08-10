using En16931.Model;

namespace En16931.Test;

/* The UBL parser reads the fixture into the model. */
[TestFixture]
public class UblInvoiceParserTests
{
    private static Invoice Invoice => InvoiceReader.ReadXml(Fixtures.Ubl);

    [Test]
    public void ReadsTheSyntaxAndTheProfile()
    {
        var invoice = Invoice;

        Assert.That(invoice.Syntax, Is.EqualTo(InvoiceSyntax.Ubl));
        Assert.That(invoice.SpecificationIdentifier, Is.EqualTo("urn:cen.eu:en16931:2017"));
    }

    [Test]
    public void ReadsTheDocument()
    {
        var invoice = Invoice;

        Assert.That(invoice.InvoiceNumber, Is.EqualTo("R-2026-0815"));
        Assert.That(invoice.InvoiceTypeCode, Is.EqualTo("380"));
        Assert.That(invoice.IssueDate, Is.EqualTo(new DateOnly(2026, 3, 15)));
        Assert.That(invoice.PaymentDueDate, Is.EqualTo(new DateOnly(2026, 4, 14)));
        Assert.That(invoice.CurrencyCode, Is.EqualTo("EUR"));
        Assert.That(invoice.BuyerReference, Is.EqualTo("04011000-12345-34"));
        Assert.That(invoice.PurchaseOrderReference, Is.EqualTo("B-2026-0042"));
    }

    /*
     * UBL has no element for the subject code of a note (BT-21). The converters put
     * the code before the text, between two number signs.
     */
    [Test]
    public void ReadsTheSubjectCodeOfANoteFromTheText()
    {
        var note = Invoice.Notes.Single();

        Assert.That(note.SubjectCode, Is.EqualTo("AAI"));
        Assert.That(note.Text, Is.EqualTo("Zahlbar ohne Abschlag."));
    }

    [Test]
    public void ReadsANoteWithoutASubjectCode()
    {
        string xml = Fixtures.Ubl.Replace(
            "<cbc:Note>#AAI#Zahlbar ohne Abschlag.</cbc:Note>",
            "<cbc:Note>Zahlbar ohne Abschlag.</cbc:Note>");

        var note = InvoiceReader.ReadXml(xml).Notes.Single();

        Assert.That(note.SubjectCode, Is.Null);
        Assert.That(note.Text, Is.EqualTo("Zahlbar ohne Abschlag."));
    }

    /* The name of the seller comes from the legal entity, not from the party name. */
    [Test]
    public void ReadsTheSeller()
    {
        var seller = Invoice.Seller;

        Assert.That(seller, Is.Not.Null);
        Assert.That(seller!.Name, Is.EqualTo("Muster GmbH"));
        Assert.That(seller.VatIdentifier, Is.EqualTo("DE123456788"));
        Assert.That(seller.PostalAddress?.AddressLine1, Is.EqualTo("Marktstr. 153"));
        Assert.That(seller.PostalAddress?.City, Is.EqualTo("Salzgitter"));
        Assert.That(seller.PostalAddress?.PostCode, Is.EqualTo("38226"));
        Assert.That(seller.PostalAddress?.CountryCode, Is.EqualTo("DE"));
        Assert.That(seller.Contact?.Name, Is.EqualTo("Erika Musterfrau"));
        Assert.That(seller.Contact?.Email, Is.EqualTo("erika@muster.example"));
    }

    [Test]
    public void ReadsTheBuyer()
    {
        Assert.That(Invoice.Buyer?.Name, Is.EqualTo("Kunde AG"));
        Assert.That(Invoice.Buyer?.PostalAddress?.City, Is.EqualTo("Berlin"));
    }

    [Test]
    public void ReadsTheDeliveryDate()
    {
        Assert.That(Invoice.Delivery?.ActualDeliveryDate, Is.EqualTo(new DateOnly(2026, 3, 12)));
    }

    [Test]
    public void ReadsThePaymentInstructions()
    {
        var payment = Invoice.PaymentInstructions;

        Assert.That(payment, Is.Not.Null);
        Assert.That(payment!.PaymentMeansTypeCode, Is.EqualTo("58"));
        Assert.That(payment.RemittanceInformation, Is.EqualTo("R-2026-0815"));

        var transfer = payment.CreditTransfers.Single();
        Assert.That(transfer.AccountIdentifier, Is.EqualTo("DE02120300000000202051"));
        Assert.That(transfer.AccountName, Is.EqualTo("Muster GmbH"));
        Assert.That(transfer.ServiceProviderIdentifier, Is.EqualTo("BYLADEM1001"));
    }

    [Test]
    public void ReadsTheVatBreakdown()
    {
        var vat = Invoice.VatBreakdown.Single();

        Assert.That(vat.CategoryCode, Is.EqualTo("S"));
        Assert.That(vat.CategoryRate, Is.EqualTo(19.00m));
        Assert.That(vat.TaxableAmount, Is.EqualTo(190.00m));
        Assert.That(vat.TaxAmount, Is.EqualTo(36.10m));
    }

    [Test]
    public void ReadsTheTotals()
    {
        var totals = Invoice.Totals;

        Assert.That(totals.LineTotalAmount, Is.EqualTo(190.00m));
        Assert.That(totals.TaxExclusiveAmount, Is.EqualTo(190.00m));
        Assert.That(totals.TaxAmount?.Value, Is.EqualTo(36.10m));
        Assert.That(totals.TaxInclusiveAmount, Is.EqualTo(226.10m));
        Assert.That(totals.DuePayableAmount, Is.EqualTo(226.10m));
    }

    [Test]
    public void ReadsThePosition()
    {
        var line = Invoice.Lines.Single();

        Assert.That(line.Id, Is.EqualTo("1"));
        Assert.That(line.Item.Name, Is.EqualTo("Trennscheibe"));
        Assert.That(line.Item.SellerIdentifier, Is.EqualTo("ART-4711"));
        Assert.That(line.InvoicedQuantity?.Value, Is.EqualTo(20.0m));
        Assert.That(line.InvoicedQuantity?.UnitCode, Is.EqualTo("H87"));
        Assert.That(line.Price.NetPrice, Is.EqualTo(9.50m));
        Assert.That(line.NetAmount, Is.EqualTo(190.00m));
        Assert.That(line.Vat.CategoryCode, Is.EqualTo("S"));
        Assert.That(line.Vat.Rate, Is.EqualTo(19.00m));
    }

    /*
     * A credit note uses other names: CreditNoteTypeCode, CreditNoteLine and
     * CreditedQuantity. The model must not show the difference.
     */
    [Test]
    public void ReadsACreditNote()
    {
        var invoice = InvoiceReader.ReadXml(Fixtures.UblCreditNote);

        Assert.That(invoice.Syntax, Is.EqualTo(InvoiceSyntax.Ubl));
        Assert.That(invoice.InvoiceNumber, Is.EqualTo("G-2026-0001"));
        Assert.That(invoice.InvoiceTypeCode, Is.EqualTo("381"));

        var line = invoice.Lines.Single();
        Assert.That(line.Item.Name, Is.EqualTo("Trennscheibe"));
        Assert.That(line.InvoicedQuantity?.Value, Is.EqualTo(2.0m));
        Assert.That(line.InvoicedQuantity?.UnitCode, Is.EqualTo("H87"));
    }
}
