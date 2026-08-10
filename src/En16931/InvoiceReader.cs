using System.Xml.Linq;
using En16931.Model;
using En16931.Parsing;

namespace En16931;

/// <summary>
/// Entry point. Detects which of the two EN 16931 syntaxes a document uses and
/// reads it into the syntax-neutral model.
/// </summary>
/// <example>
/// <code>
/// var invoice = InvoiceReader.ReadFile(@"C:\in\factur-x.xml");
/// Console.WriteLine($"{invoice.InvoiceNumber}: {invoice.Totals.DuePayableAmount}");
/// </code>
/// </example>
public static class InvoiceReader
{
    private static readonly IInvoiceParser[] Parsers = [new CiiInvoiceParser(), new UblInvoiceParser()];

    /// <summary>
    /// Determines the syntax from the document's root element, without parsing it.
    /// </summary>
    public static InvoiceSyntax DetectSyntax(XDocument document)
    {
        ArgumentNullException.ThrowIfNull(document);
        foreach (var parser in Parsers)
            if (parser.CanParse(document))
                return parser.Syntax;
        return InvoiceSyntax.Unknown;
    }

    /// <summary>Reads an already loaded document.</summary>
    /// <exception cref="InvoiceFormatException">The root element belongs to neither syntax.</exception>
    public static Invoice Read(XDocument document)
    {
        ArgumentNullException.ThrowIfNull(document);
        foreach (var parser in Parsers)
            if (parser.CanParse(document))
                return parser.Parse(document);

        throw new InvoiceFormatException(
            $"'{document.Root?.Name.ToString() ?? "(empty document)"}' is neither a CII " +
            "CrossIndustryInvoice nor a UBL Invoice or CreditNote.");
    }

    /// <summary>Reads an invoice from a stream.</summary>
    /// <exception cref="InvoiceFormatException">The stream is not well formed XML, or belongs to neither syntax.</exception>
    public static Invoice Read(Stream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);
        return Read(Load(() => XDocument.Load(stream, LoadOptions.None)));
    }

    /// <summary>Reads an invoice from an XML string.</summary>
    /// <exception cref="InvoiceFormatException">The text is not well formed XML, or belongs to neither syntax.</exception>
    public static Invoice ReadXml(string xml)
    {
        ArgumentNullException.ThrowIfNull(xml);
        return Read(Load(() => XDocument.Parse(xml, LoadOptions.None)));
    }

    /// <summary>Reads an invoice from a file.</summary>
    /// <exception cref="InvoiceFormatException">The file is not well formed XML, or belongs to neither syntax.</exception>
    public static Invoice ReadFile(string path)
    {
        ArgumentNullException.ThrowIfNull(path);
        return Read(Load(() => XDocument.Load(path, LoadOptions.None)));
    }

    /// <summary>Reads an invoice from a stream, returning false instead of throwing.</summary>
    public static bool TryRead(Stream stream, out Invoice? invoice)
    {
        try
        {
            invoice = Read(stream);
            return true;
        }
        catch (Exception exception) when (exception is InvoiceFormatException or ArgumentNullException)
        {
            invoice = null;
            return false;
        }
    }

    private static XDocument Load(Func<XDocument> load)
    {
        try
        {
            return load();
        }
        catch (System.Xml.XmlException exception)
        {
            throw new InvoiceFormatException("The document is not well formed XML.", exception);
        }
    }
}
