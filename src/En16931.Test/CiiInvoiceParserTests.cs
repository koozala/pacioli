using En16931.Model;

namespace En16931.Test;

/* The CII parser reads the fixture into the model. */
[TestFixture]
public class CiiInvoiceParserTests
{
    private static Invoice Invoice => InvoiceReader.ReadXml(Fixtures.Cii);

    [Test]
    public void ReadsTheSyntaxAndTheProfile()
    {
        var invoice = Invoice;

        Assert.That(invoice.Syntax, Is.EqualTo(InvoiceSyntax.Cii));
        Assert.That(invoice.SpecificationIdentifier, Is.EqualTo("urn:cen.eu:en16931:2017"));
    }

    [Test]
    public void ReadsTheDocument()
    {
        var invoice = Invoice;

        Assert.That(invoice.InvoiceNumber, Is.EqualTo("R-2026-0815"));
        Assert.That(invoice.InvoiceTypeCode, Is.EqualTo("380"));
        Assert.That(invoice.IssueDate, Is.EqualTo(new DateOnly(2026, 3, 15)));
        Assert.That(invoice.CurrencyCode, Is.EqualTo("EUR"));
        Assert.That(invoice.BuyerReference, Is.EqualTo("04011000-12345-34"));
        Assert.That(invoice.PurchaseOrderReference, Is.EqualTo("B-2026-0042"));
    }

    /* The date of CII has the form CCYYMMDD, with the format code 102. */
    [Test]
    public void ReadsADateWithTheFormatCode102()
    {
        Assert.That(Invoice.PaymentDueDate, Is.EqualTo(new DateOnly(2026, 4, 14)));
    }

    [Test]
    public void ReadsTheNote()
    {
        var note = Invoice.Notes.Single();

        Assert.That(note.SubjectCode, Is.EqualTo("AAI"));
        Assert.That(note.Text, Is.EqualTo("Zahlbar ohne Abschlag."));
    }

    [Test]
    public void ReadsTheSeller()
    {
        var seller = Invoice.Seller;

        Assert.That(seller, Is.Not.Null);
        Assert.That(seller!.Name, Is.EqualTo("Muster GmbH"));
        Assert.That(seller.VatIdentifier, Is.EqualTo("DE123456788"));
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
        Assert.That(totals.TaxAmount?.CurrencyCode, Is.EqualTo("EUR"));
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

    /* The parser must not raise an exception for a document with few elements. */
    [Test]
    public void ReadsADocumentWithoutATransaction()
    {
        const string xml = """
            <rsm:CrossIndustryInvoice
                xmlns:rsm="urn:un:unece:uncefact:data:standard:CrossIndustryInvoice:100"
                xmlns:ram="urn:un:unece:uncefact:data:standard:ReusableAggregateBusinessInformationEntity:100">
              <rsm:ExchangedDocument>
                <ram:ID>R-1</ram:ID>
              </rsm:ExchangedDocument>
            </rsm:CrossIndustryInvoice>
            """;

        var invoice = InvoiceReader.ReadXml(xml);

        Assert.That(invoice.InvoiceNumber, Is.EqualTo("R-1"));
        Assert.That(invoice.Lines, Is.Empty);
        Assert.That(invoice.Seller, Is.Null);
        Assert.That(invoice.Totals.DuePayableAmount, Is.Null);
    }

    /* A value that is not a number must give null, and it must not stop the parser. */
    [Test]
    public void GivesNull_ForAnAmountThatIsNotANumber()
    {
        string xml = Fixtures.Cii.Replace(
            "<ram:DuePayableAmount>226.10</ram:DuePayableAmount>",
            "<ram:DuePayableAmount>zweihundert</ram:DuePayableAmount>");

        var invoice = InvoiceReader.ReadXml(xml);

        Assert.That(invoice.Totals.DuePayableAmount, Is.Null);

        /* The other amounts of the same group stay. */
        Assert.That(invoice.Totals.TaxInclusiveAmount, Is.EqualTo(226.10m));
    }
}
