using En16931.Model;

namespace En16931.Test;

/*
 * The discounts (Skonto) of XRechnung. XRechnung gives each discount as one line in the
 * text of the payment terms (BT-20), for example "#SKONTO#TAGE=7#PROZENT=3.00#". The
 * parsers take these lines out of the text and give them to the term as discounts.
 *
 * The tests change the text of the payment terms in the two fixtures.
 */
[TestFixture]
public class XRechnungDiscountTests
{
    private const string FixtureText = "Zahlbar bis zum 14.04.2026.";

    private static Invoice ReadCii(string text) =>
        InvoiceReader.ReadXml(Fixtures.Cii.Replace(FixtureText, text));

    private static Invoice ReadUbl(string text) =>
        InvoiceReader.ReadXml(Fixtures.Ubl.Replace(FixtureText, text));

    private static IEnumerable<TestCaseData> Readers()
    {
        yield return new TestCaseData((Func<string, Invoice>)ReadCii).SetName("{m}(CII)");
        yield return new TestCaseData((Func<string, Invoice>)ReadUbl).SetName("{m}(UBL)");
    }

    [TestCaseSource(nameof(Readers))]
    public void ReadsOneDiscount(Func<string, Invoice> read)
    {
        var term = read("#SKONTO#TAGE=10#PROZENT=3.00#\n").Extensions!.PaymentTerms.Single();

        var discount = term.Discounts.Single();
        Assert.That(discount.BasisPeriodDays, Is.EqualTo(10m));
        Assert.That(discount.Percentage, Is.EqualTo(3.00m));
        Assert.That(discount.BasisAmount, Is.Null);
        Assert.That(discount.BasisDate, Is.Null, "XRechnung gives no day. The days count from BT-2.");

        /* The text holds only the discount, thus no text stays. */
        Assert.That(term.Description, Is.Null);
    }

    [TestCaseSource(nameof(Readers))]
    public void ReadsEachDiscountAndKeepsTheOtherText(Func<string, Invoice> read)
    {
        var invoice = read(
            "#SKONTO#TAGE=7#PROZENT=3.00#\r\n" +
            "#SKONTO#TAGE=14#PROZENT=2.00#BASISBETRAG=150.00#\r\n" +
            "Zahlbar innerhalb von 30 Tagen netto.");
        var term = invoice.Extensions!.PaymentTerms.Single();

        Assert.That(term.Discounts, Has.Count.EqualTo(2));
        Assert.That(term.Discounts[0].BasisPeriodDays, Is.EqualTo(7m));
        Assert.That(term.Discounts[0].Percentage, Is.EqualTo(3.00m));
        Assert.That(term.Discounts[1].BasisPeriodDays, Is.EqualTo(14m));
        Assert.That(term.Discounts[1].Percentage, Is.EqualTo(2.00m));
        Assert.That(term.Discounts[1].BasisAmount, Is.EqualTo(new Amount(150.00m, "EUR")));

        Assert.That(term.Description, Is.EqualTo("Zahlbar innerhalb von 30 Tagen netto."));

        /* The core keeps the full value of BT-20. */
        Assert.That(invoice.PaymentTerms, Does.Contain("#SKONTO#TAGE=7#PROZENT=3.00#"));
    }

    [TestCaseSource(nameof(Readers))]
    public void KeepsALineThatDoesNotAgreeWithTheForm(Func<string, Invoice> read)
    {
        const string text = "#SKONTO#TAGE=sieben#PROZENT=3,00#";

        var term = read(text).Extensions!.PaymentTerms.Single();

        Assert.That(term.Discounts, Is.Empty);
        Assert.That(term.Description, Is.EqualTo(text));
    }

    [TestCaseSource(nameof(Readers))]
    public void DoesNotChangeATextWithoutADiscount(Func<string, Invoice> read)
    {
        var term = read(FixtureText).Extensions!.PaymentTerms.Single();

        Assert.That(term.Discounts, Is.Empty);
        Assert.That(term.Description, Is.EqualTo(FixtureText));
    }
}
