using En16931.Conformance;

namespace En16931.Test;

/*
 * The catalog of the specification identifiers (BT-24), and the comparison of the
 * declarations of a document (BT-24 and the XMP metadata of a PDF file).
 */
[TestFixture]
public class ConformanceTests
{
    [TestCase("urn:ferd:CrossIndustryDocument:invoice:1p0:basic", SpecificationFamily.Zugferd1, InvoiceProfile.Basic, "1.0")]
    [TestCase("urn:ferd:CrossIndustryDocument:invoice:1p0:comfort", SpecificationFamily.Zugferd1, InvoiceProfile.Comfort, "1.0")]
    [TestCase("urn:ferd:CrossIndustryDocument:invoice:1p0:extended", SpecificationFamily.Zugferd1, InvoiceProfile.Extended, "1.0")]
    [TestCase("urn:zugferd.de:2p0:minimum", SpecificationFamily.Zugferd20, InvoiceProfile.Minimum, "2.0")]
    [TestCase("urn:zugferd.de:2p0:basicwl", SpecificationFamily.Zugferd20, InvoiceProfile.BasicWL, "2.0")]
    [TestCase("urn:cen.eu:en16931:2017#compliant#urn:zugferd.de:2p0:basic", SpecificationFamily.Zugferd20, InvoiceProfile.Basic, "2.0")]
    [TestCase("urn:cen.eu:en16931:2017#conformant#urn:zugferd.de:2p0:extended", SpecificationFamily.Zugferd20, InvoiceProfile.Extended, "2.0")]
    [TestCase("urn:factur-x.eu:1p0:minimum", SpecificationFamily.FacturX, InvoiceProfile.Minimum, "1.0")]
    [TestCase("urn:factur-x.eu:1p0:basicwl", SpecificationFamily.FacturX, InvoiceProfile.BasicWL, "1.0")]
    [TestCase("urn:cen.eu:en16931:2017#compliant#urn:factur-x.eu:1p0:basic", SpecificationFamily.FacturX, InvoiceProfile.Basic, "1.0")]
    [TestCase("urn:cen.eu:en16931:2017#conformant#urn:factur-x.eu:1p0:extended", SpecificationFamily.FacturX, InvoiceProfile.Extended, "1.0")]
    [TestCase("urn:cen.eu:en16931:2017", SpecificationFamily.En16931, InvoiceProfile.En16931, null)]
    [TestCase("urn:cen.eu:en16931:2017#compliant#urn:xoev-de:kosit:standard:xrechnung_1.2", SpecificationFamily.XRechnung, InvoiceProfile.XRechnung, "1.2")]
    [TestCase("urn:cen.eu:en16931:2017#compliant#urn:xeinkauf.de:kosit:xrechnung_2.3", SpecificationFamily.XRechnung, InvoiceProfile.XRechnung, "2.3")]
    [TestCase("urn:cen.eu:en16931:2017#compliant#urn:xeinkauf.de:kosit:xrechnung_3.0", SpecificationFamily.XRechnung, InvoiceProfile.XRechnung, "3.0")]
    [TestCase("urn:cen.eu:en16931:2017#compliant#urn:xeinkauf.de:kosit:xrechnung_3.0#conformant#urn:xeinkauf.de:kosit:extension:xrechnung_3.0",
        SpecificationFamily.XRechnung, InvoiceProfile.XRechnungExtension, "3.0")]
    [TestCase("urn:cen.eu:en16931:2017#compliant#urn:fdc:peppol.eu:2017:poacc:billing:3.0", SpecificationFamily.PeppolBis, InvoiceProfile.PeppolBisBilling, "3.0")]
    [TestCase("  URN:CEN.EU:EN16931:2017  ", SpecificationFamily.En16931, InvoiceProfile.En16931, null)]
    public void KnowsTheIdentifier(string identifier, SpecificationFamily family, InvoiceProfile profile, string? version)
    {
        SpecificationInfo info = SpecificationCatalog.Identify(identifier);

        Assert.That(info.IsRecognized, Is.True);
        Assert.That(info.Family, Is.EqualTo(family));
        Assert.That(info.Profile, Is.EqualTo(profile));
        Assert.That(info.Version, Is.EqualTo(version));
        Assert.That(info.StandardName, Is.Not.Null);
        Assert.That(info.ProfileName, Is.Not.Null);
    }

    [TestCase(null)]
    [TestCase("")]
    [TestCase("urn:cen.eu:en16931:2017#compliant#urn:fdc:nen.nl:nlcius:v1.0")]
    [TestCase("urn:factur-x.eu:1p0:comfort")]
    [TestCase("urn:ferd:CrossIndustryDocument:invoice:1p0:minimum")]
    [TestCase("something else")]
    public void DoesNotKnowTheIdentifier(string? identifier)
    {
        SpecificationInfo info = SpecificationCatalog.Identify(identifier);

        Assert.That(info.IsRecognized, Is.False);
        Assert.That(info.Profile, Is.EqualTo(InvoiceProfile.Unknown));
        Assert.That(info.StandardName, Is.Null);
    }

    [TestCase("MINIMUM", InvoiceProfile.Minimum)]
    [TestCase("BASIC WL", InvoiceProfile.BasicWL)]
    [TestCase("basic", InvoiceProfile.Basic)]
    [TestCase("EN 16931", InvoiceProfile.En16931)]
    [TestCase("EN_16931", InvoiceProfile.En16931)]
    [TestCase("COMFORT", InvoiceProfile.Comfort)]
    [TestCase("EXTENDED", InvoiceProfile.Extended)]
    [TestCase("XRECHNUNG", InvoiceProfile.XRechnung)]
    [TestCase("PREMIUM", InvoiceProfile.Unknown)]
    [TestCase(null, InvoiceProfile.Unknown)]
    public void ReadsTheConformanceLevel(string? level, InvoiceProfile profile)
    {
        Assert.That(SpecificationCatalog.ProfileOfConformanceLevel(level), Is.EqualTo(profile));
    }

    /* A correct Factur-X EXTENDED file. */
    private static DeclarationInput GoodPdf() => new()
    {
        SpecificationIdentifier = "urn:cen.eu:en16931:2017#conformant#urn:factur-x.eu:1p0:extended",
        XmlRootNamespace = DeclarationCheck.CiiXmlNamespace,
        IsPdf = true,
        EmbeddedFileName = "factur-x.xml",
        XmpNamespace = DeclarationCheck.FacturXXmpNamespace,
        XmpConformanceLevel = "EXTENDED",
        XmpDocumentFileName = "factur-x.xml",
        PdfAPart = "3"
    };

    [Test]
    public void ACorrectFileHasNoIssue()
    {
        Assert.That(DeclarationCheck.Check(GoodPdf()), Is.Empty);
    }

    [Test]
    public void AnXmlFileHasNoIssue()
    {
        var input = new DeclarationInput
        {
            SpecificationIdentifier = "urn:cen.eu:en16931:2017#compliant#urn:xeinkauf.de:kosit:xrechnung_3.0",
            XmlRootNamespace = "urn:oasis:names:specification:ubl:schema:xsd:Invoice-2",
            IsPdf = false
        };

        Assert.That(DeclarationCheck.Check(input), Is.Empty);
    }

    [Test]
    public void FindsAnOtherConformanceLevel()
    {
        var issue = DeclarationCheck.Check(GoodPdf() with { XmpConformanceLevel = "BASIC" }).Single();

        Assert.That(issue.Rule, Is.EqualTo(DeclarationRule.ConformanceLevel));
        Assert.That(issue.Expected, Is.EqualTo("EXTENDED"));
        Assert.That(issue.Actual, Is.EqualTo("BASIC"));
    }

    [Test]
    public void AnUnknownConformanceLevelIsAnIssue()
    {
        var issue = DeclarationCheck.Check(GoodPdf() with { XmpConformanceLevel = "PREMIUM" }).Single();

        Assert.That(issue.Rule, Is.EqualTo(DeclarationRule.ConformanceLevel));
    }

    [TestCase("COMFORT")]
    [TestCase("EN 16931")]
    public void TheProfileEn16931TakesBothNames(string level)
    {
        var input = GoodPdf() with { SpecificationIdentifier = "urn:cen.eu:en16931:2017", XmpConformanceLevel = level };

        Assert.That(DeclarationCheck.Check(input), Is.Empty);
    }

    [Test]
    public void XRechnungInAPdfFileHasTheLevelXRechnung()
    {
        var input = GoodPdf() with
        {
            SpecificationIdentifier = "urn:cen.eu:en16931:2017#compliant#urn:xeinkauf.de:kosit:xrechnung_3.0",
            XmpConformanceLevel = "XRECHNUNG",
            EmbeddedFileName = "xrechnung.xml",
            XmpDocumentFileName = "xrechnung.xml"
        };

        Assert.That(DeclarationCheck.Check(input), Is.Empty);
    }

    [Test]
    public void AnUnknownIdentifierGivesNoLevelIssue()
    {
        var input = GoodPdf() with { SpecificationIdentifier = "urn:something:else" };

        Assert.That(DeclarationCheck.Check(input), Is.Empty);
    }

    [Test]
    public void FindsAnOtherFileName()
    {
        var issue = DeclarationCheck.Check(GoodPdf() with { XmpDocumentFileName = "ZUGFeRD-invoice.xml" }).Single();

        Assert.That(issue.Rule, Is.EqualTo(DeclarationRule.DocumentFileName));
        Assert.That(issue.Expected, Is.EqualTo("factur-x.xml"));
        Assert.That(issue.Actual, Is.EqualTo("ZUGFeRD-invoice.xml"));
    }

    [Test]
    public void FindsZugferd1MetadataWithCiiD16B()
    {
        var input = GoodPdf() with { XmpNamespace = DeclarationCheck.Zugferd1XmpNamespace };

        var issue = DeclarationCheck.Check(input).Single();

        Assert.That(issue.Rule, Is.EqualTo(DeclarationRule.XmpFamily));
        Assert.That(issue.Actual, Is.EqualTo(DeclarationCheck.CiiXmlNamespace));
    }

    [Test]
    public void FindsFacturXMetadataWithUbl()
    {
        var input = GoodPdf() with { XmlRootNamespace = "urn:oasis:names:specification:ubl:schema:xsd:Invoice-2" };

        Assert.That(DeclarationCheck.Check(input).Single().Rule, Is.EqualTo(DeclarationRule.XmpFamily));
    }

    [TestCase("1", "1")]
    [TestCase("2", "2")]
    [TestCase(null, "-")]
    public void APdfFileMustBePdfA3(string? part, string actual)
    {
        var issue = DeclarationCheck.Check(GoodPdf() with { PdfAPart = part }).Single();

        Assert.That(issue.Rule, Is.EqualTo(DeclarationRule.PdfAPart));
        Assert.That(issue.Actual, Is.EqualTo(actual));
    }

    [Test]
    public void APdfFileWithoutXmpGetsOnlyThePdfARule()
    {
        var input = GoodPdf() with { XmpNamespace = null, PdfAPart = null };

        Assert.That(DeclarationCheck.Check(input).Select(issue => issue.Rule), Is.EqualTo(new[] { DeclarationRule.PdfAPart }));
    }
}
