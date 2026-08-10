using System.Xml.Linq;
using En16931.Model;

namespace En16931.Parsing;

/// <summary>
/// Reads one XML syntax into the EN 16931 model.
/// </summary>
/// <remarks>
/// Implementations parse permissively: a missing or malformed value yields
/// <c>null</c> on the model rather than an exception, so an invoice that breaks
/// the business rules can still be read and inspected. Rule conformance is a
/// separate concern — validate with the EN 16931 Schematron.
/// </remarks>
public interface IInvoiceParser
{
    /// <summary>The syntax this parser reads.</summary>
    InvoiceSyntax Syntax { get; }

    /// <summary>Whether the document's root element belongs to this syntax.</summary>
    bool CanParse(XDocument document);

    /// <summary>Reads the document into the model.</summary>
    /// <exception cref="InvoiceFormatException">The root element is not of this syntax.</exception>
    Invoice Parse(XDocument document);
}

/// <summary>Thrown when a document is not a recognisable EN 16931 invoice.</summary>
public sealed class InvoiceFormatException : Exception
{
    /// <summary>Creates the exception with a message.</summary>
    public InvoiceFormatException(string message) : base(message) { }

    /// <summary>Creates the exception with a message and an inner exception.</summary>
    public InvoiceFormatException(string message, Exception innerException)
        : base(message, innerException) { }
}
