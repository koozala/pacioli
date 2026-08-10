using System.Globalization;
using System.Xml.Linq;

namespace En16931.Parsing;

/// <summary>
/// Small null-tolerant helpers over LINQ to XML. Every method accepts a null
/// element and returns null, so a parser can walk a path without guarding each
/// step. This is what makes the parsers permissive: a missing or malformed value
/// yields null instead of an exception.
/// </summary>
internal static class XmlHelpers
{
    /// <summary>First child element with the given name, or null.</summary>
    public static XElement? El(this XElement? parent, XName name) =>
        parent?.Element(name);

    /// <summary>All child elements with the given name.</summary>
    public static IEnumerable<XElement> Els(this XElement? parent, XName name) =>
        parent?.Elements(name) ?? [];

    /// <summary>Walks a chain of child element names, stopping at the first missing step.</summary>
    public static XElement? Path(this XElement? parent, params XName[] names)
    {
        var current = parent;
        foreach (var name in names)
        {
            current = current?.Element(name);
            if (current is null) return null;
        }
        return current;
    }

    /// <summary>Trimmed text content, or null when absent or empty.</summary>
    public static string? Text(this XElement? element)
    {
        var value = element?.Value.Trim();
        return string.IsNullOrEmpty(value) ? null : value;
    }

    /// <summary>Trimmed attribute value, or null when absent or empty.</summary>
    public static string? Attr(this XElement? element, XName name)
    {
        var value = element?.Attribute(name)?.Value.Trim();
        return string.IsNullOrEmpty(value) ? null : value;
    }

    /// <summary>Decimal content, parsed invariantly. Null when absent or not a number.</summary>
    public static decimal? Dec(this XElement? element)
    {
        var text = element.Text();
        return text is not null
            && decimal.TryParse(text, NumberStyles.Float, CultureInfo.InvariantCulture, out var value)
            ? value
            : null;
    }

    /// <summary>Boolean content. Accepts <c>true</c>/<c>false</c> and <c>1</c>/<c>0</c>.</summary>
    public static bool? Bool(this XElement? element) => element.Text() switch
    {
        "true" or "TRUE" or "True" or "1" => true,
        "false" or "FALSE" or "False" or "0" => false,
        _ => null
    };

    /// <summary>
    /// A UBL date, which is always ISO 8601 <c>YYYY-MM-DD</c>.
    /// </summary>
    public static DateOnly? IsoDate(this XElement? element)
    {
        var text = element.Text();
        if (text is null) return null;
        // Some senders append a time or a zone offset. Take the date part.
        var datePart = text.Length > 10 ? text[..10] : text;
        return DateOnly.TryParseExact(datePart, "yyyy-MM-dd", CultureInfo.InvariantCulture,
            DateTimeStyles.None, out var value) ? value : null;
    }

    /// <summary>
    /// A CII date string. The <c>format</c> attribute names a UNTDID 2379 code;
    /// 102 (CCYYMMDD) is the only one EN 16931 allows, but 610 and 616 appear in
    /// the wild and are accepted here.
    /// </summary>
    public static DateOnly? CiiDate(this XElement? element)
    {
        var text = element.Text();
        if (text is null) return null;
        var format = element.Attr("format") ?? "102";
        return format switch
        {
            "102" => Exact(text, "yyyyMMdd"),
            "610" => Exact(text + "01", "yyyyMMdd"),
            "616" => null, // week of year — not a calendar date
            _ => Exact(text, "yyyyMMdd") ?? IsoDateFrom(text)
        };

        static DateOnly? Exact(string value, string format) =>
            DateOnly.TryParseExact(value, format, CultureInfo.InvariantCulture,
                DateTimeStyles.None, out var parsed) ? parsed : null;

        static DateOnly? IsoDateFrom(string value) =>
            DateOnly.TryParseExact(value.Length > 10 ? value[..10] : value, "yyyy-MM-dd",
                CultureInfo.InvariantCulture, DateTimeStyles.None, out var parsed) ? parsed : null;
    }
}
