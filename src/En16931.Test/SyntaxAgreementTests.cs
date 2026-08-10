using En16931.Model;

namespace En16931.Test;

/*
 * The promise of the library: the model does not show the syntax.
 *
 * The two fixtures hold the same invoice, one time in CII and one time in UBL. Each
 * business term must give the same value from both documents.
 *
 * This test guards the renderer. The renderer reads the model only. If a term reads
 * differently from the two syntaxes, the page of a UBL invoice is not the page of the
 * same invoice in CII, and this test says which term makes the difference.
 */
[TestFixture]
public class SyntaxAgreementTests
{
    private static readonly Invoice Cii = InvoiceReader.ReadXml(Fixtures.Cii);
    private static readonly Invoice Ubl = InvoiceReader.ReadXml(Fixtures.Ubl);

    /* The term, and how to take it from the model. */
    private static readonly (string Term, Func<Invoice, object?> Select)[] Terms =
    [
        ("BT-1 Invoice number", invoice => invoice.InvoiceNumber),
        ("BT-2 Issue date", invoice => invoice.IssueDate),
        ("BT-3 Invoice type code", invoice => invoice.InvoiceTypeCode),
        ("BT-5 Currency", invoice => invoice.CurrencyCode),
        ("BT-9 Payment due date", invoice => invoice.PaymentDueDate),
        ("BT-10 Buyer reference", invoice => invoice.BuyerReference),
        ("BT-13 Purchase order reference", invoice => invoice.PurchaseOrderReference),
        ("BT-20 Payment terms", invoice => invoice.PaymentTerms),
        ("BT-21 Note subject code", invoice => invoice.Notes.Single().SubjectCode),
        ("BT-22 Note", invoice => invoice.Notes.Single().Text),
        ("BT-24 Specification identifier", invoice => invoice.SpecificationIdentifier),

        ("BT-27 Seller name", invoice => invoice.Seller?.Name),
        ("BT-31 Seller VAT identifier", invoice => invoice.Seller?.VatIdentifier),
        ("BT-35 Seller address line 1", invoice => invoice.Seller?.PostalAddress?.AddressLine1),
        ("BT-37 Seller city", invoice => invoice.Seller?.PostalAddress?.City),
        ("BT-38 Seller post code", invoice => invoice.Seller?.PostalAddress?.PostCode),
        ("BT-40 Seller country", invoice => invoice.Seller?.PostalAddress?.CountryCode),
        ("BT-41 Seller contact point", invoice => invoice.Seller?.Contact?.Name),
        ("BT-43 Seller contact email", invoice => invoice.Seller?.Contact?.Email),

        ("BT-44 Buyer name", invoice => invoice.Buyer?.Name),
        ("BT-50 Buyer address line 1", invoice => invoice.Buyer?.PostalAddress?.AddressLine1),
        ("BT-52 Buyer city", invoice => invoice.Buyer?.PostalAddress?.City),
        ("BT-53 Buyer post code", invoice => invoice.Buyer?.PostalAddress?.PostCode),

        ("BT-72 Actual delivery date", invoice => invoice.Delivery?.ActualDeliveryDate),

        ("BT-81 Payment means type code",
            invoice => invoice.PaymentInstructions?.PaymentMeansTypeCode),
        ("BT-83 Remittance information",
            invoice => invoice.PaymentInstructions?.RemittanceInformation),
        ("BT-84 Payment account identifier",
            invoice => invoice.PaymentInstructions?.CreditTransfers.Single().AccountIdentifier),
        ("BT-85 Payment account name",
            invoice => invoice.PaymentInstructions?.CreditTransfers.Single().AccountName),
        ("BT-86 Payment service provider identifier",
            invoice => invoice.PaymentInstructions?.CreditTransfers.Single().ServiceProviderIdentifier),

        ("BT-106 Sum of line net amounts", invoice => invoice.Totals.LineTotalAmount),
        ("BT-109 Total without VAT", invoice => invoice.Totals.TaxExclusiveAmount),
        ("BT-110 Total VAT amount", invoice => invoice.Totals.TaxAmount?.Value),
        ("BT-112 Total with VAT", invoice => invoice.Totals.TaxInclusiveAmount),
        ("BT-115 Amount due for payment", invoice => invoice.Totals.DuePayableAmount),

        ("BT-116 VAT taxable amount", invoice => invoice.VatBreakdown.Single().TaxableAmount),
        ("BT-117 VAT tax amount", invoice => invoice.VatBreakdown.Single().TaxAmount),
        ("BT-118 VAT category code", invoice => invoice.VatBreakdown.Single().CategoryCode),
        ("BT-119 VAT category rate", invoice => invoice.VatBreakdown.Single().CategoryRate),

        ("BT-126 Line identifier", invoice => invoice.Lines.Single().Id),
        ("BT-129 Invoiced quantity", invoice => invoice.Lines.Single().InvoicedQuantity?.Value),
        ("BT-130 Unit of measure", invoice => invoice.Lines.Single().InvoicedQuantity?.UnitCode),
        ("BT-131 Line net amount", invoice => invoice.Lines.Single().NetAmount),
        ("BT-146 Item net price", invoice => invoice.Lines.Single().Price.NetPrice),
        ("BT-151 Line VAT category code", invoice => invoice.Lines.Single().Vat.CategoryCode),
        ("BT-152 Line VAT rate", invoice => invoice.Lines.Single().Vat.Rate),
        ("BT-153 Item name", invoice => invoice.Lines.Single().Item.Name),
        ("BT-155 Item seller identifier", invoice => invoice.Lines.Single().Item.SellerIdentifier)
    ];

    private static IEnumerable<string> TermNames => Terms.Select(entry => entry.Term);

    [TestCaseSource(nameof(TermNames))]
    public void TheTermReadsTheSameFromBothSyntaxes(string term)
    {
        var select = Terms.Single(entry => entry.Term == term).Select;

        object? fromCii = select(Cii);
        object? fromUbl = select(Ubl);

        Assert.That(fromCii, Is.Not.Null, $"{term}: the CII document gives no value.");
        Assert.That(fromUbl, Is.EqualTo(fromCii),
            $"{term}: CII gives \"{fromCii}\", UBL gives \"{fromUbl}\".");
    }

    /* The syntax itself is the one value that must differ. */
    [Test]
    public void TheSyntaxIsTheOnlyDifference()
    {
        Assert.That(Cii.Syntax, Is.EqualTo(InvoiceSyntax.Cii));
        Assert.That(Ubl.Syntax, Is.EqualTo(InvoiceSyntax.Ubl));
    }
}
