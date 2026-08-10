namespace En16931.Model;

/// <summary>
/// An EN 16931 identifier: a value plus the optional scheme it is issued under.
/// Both syntaxes carry the scheme as an XML attribute — <c>schemeID</c> in UBL,
/// <c>schemeID</c> or <c>schemeAgencyID</c> in CII.
/// </summary>
public sealed record Identifier(string Value, string? SchemeId = null, string? SchemeVersionId = null)
{
    /// <inheritdoc/>
    public override string ToString() =>
        SchemeId is null ? Value : $"{Value} ({SchemeId})";
}

/// <summary>
/// A monetary amount together with the currency it is expressed in. The currency
/// is only carried where a term may deviate from the invoice currency (BT-5),
/// notably BT-111 in the VAT accounting currency (BT-6).
/// </summary>
public sealed record Amount(decimal Value, string? CurrencyCode = null);

/// <summary>
/// A quantity with its unit of measure code (UN/ECE Recommendation 20).
/// </summary>
public sealed record Quantity(decimal Value, string? UnitCode = null);

/// <summary>
/// BT-125 attached document, with the metadata both syntaxes carry as attributes.
/// </summary>
public sealed class Attachment
{
    /// <summary>BT-125 The attached document as binary content.</summary>
    public byte[]? Content { get; set; }

    /// <summary>BT-125-1 Mime code of the attached document.</summary>
    public string? MimeCode { get; set; }

    /// <summary>BT-125-2 File name of the attached document.</summary>
    public string? FileName { get; set; }
}
