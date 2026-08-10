using System.Text;
using System.Xml.Linq;
using En16931.Parsing;

namespace En16931.Test;

/*
 * The entry point: it must find the syntax of a document, and it must give a clear
 * error for a document that is not an invoice.
 */
[TestFixture]
public class InvoiceReaderTests
{
    [Test]
    public void DetectSyntax_FindsCii()
    {
        Assert.That(InvoiceReader.DetectSyntax(XDocument.Parse(Fixtures.Cii)),
            Is.EqualTo(InvoiceSyntax.Cii));
    }

    [Test]
    public void DetectSyntax_FindsUblInvoice()
    {
        Assert.That(InvoiceReader.DetectSyntax(XDocument.Parse(Fixtures.Ubl)),
            Is.EqualTo(InvoiceSyntax.Ubl));
    }

    [Test]
    public void DetectSyntax_FindsUblCreditNote()
    {
        Assert.That(InvoiceReader.DetectSyntax(XDocument.Parse(Fixtures.UblCreditNote)),
            Is.EqualTo(InvoiceSyntax.Ubl));
    }

    /* A document of an other standard must not read as an invoice. */
    [Test]
    public void DetectSyntax_GivesUnknown_ForAnOtherDocument()
    {
        var document = XDocument.Parse(
            "<FatturaElettronica xmlns=\"http://ivaservizi.agenziaentrate.gov.it/docs/xsd/fatture/v1.2\" />");

        Assert.That(InvoiceReader.DetectSyntax(document), Is.EqualTo(InvoiceSyntax.Unknown));
    }

    [Test]
    public void Read_Throws_ForAnOtherDocument()
    {
        Assert.Throws<InvoiceFormatException>(() => InvoiceReader.ReadXml("<Order />"));
    }

    [Test]
    public void Read_Throws_ForTextThatIsNotXml()
    {
        Assert.Throws<InvoiceFormatException>(() => InvoiceReader.ReadXml("this is not xml"));
    }

    [Test]
    public void ReadXml_AndReadStream_GiveTheSameResult()
    {
        var fromText = InvoiceReader.ReadXml(Fixtures.Cii);

        using var stream = new MemoryStream(Encoding.UTF8.GetBytes(Fixtures.Cii));
        var fromStream = InvoiceReader.Read(stream);

        Assert.That(fromStream.InvoiceNumber, Is.EqualTo(fromText.InvoiceNumber));
        Assert.That(fromStream.Totals.DuePayableAmount, Is.EqualTo(fromText.Totals.DuePayableAmount));
    }

    [Test]
    public void ReadFile_ReadsFromTheDisk()
    {
        string path = Path.Combine(Path.GetTempPath(), $"en16931-{Guid.NewGuid():N}.xml");
        File.WriteAllText(path, Fixtures.Ubl, Encoding.UTF8);

        try
        {
            Assert.That(InvoiceReader.ReadFile(path).InvoiceNumber, Is.EqualTo("R-2026-0815"));
        }
        finally
        {
            File.Delete(path);
        }
    }

    [Test]
    public void TryRead_GivesFalse_ForAnOtherDocument()
    {
        using var stream = new MemoryStream(Encoding.UTF8.GetBytes("<Order />"));

        Assert.That(InvoiceReader.TryRead(stream, out var invoice), Is.False);
        Assert.That(invoice, Is.Null);
    }

    [Test]
    public void TryRead_GivesTrue_ForAnInvoice()
    {
        using var stream = new MemoryStream(Encoding.UTF8.GetBytes(Fixtures.Cii));

        Assert.That(InvoiceReader.TryRead(stream, out var invoice), Is.True);
        Assert.That(invoice, Is.Not.Null);
    }

    /*
     * TryRead is the test that the PDF reader will use: it gives each stream of a PDF
     * file to the parser. A stream that holds no XML must not stop the search.
     */
    [Test]
    public void TryRead_GivesFalse_ForBytesThatAreNotXml()
    {
        using var stream = new MemoryStream([0x25, 0x50, 0x44, 0x46, 0x2D]);

        Assert.That(InvoiceReader.TryRead(stream, out _), Is.False);
    }
}
