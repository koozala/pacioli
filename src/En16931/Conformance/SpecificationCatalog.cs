using System.Text.RegularExpressions;

namespace En16931.Conformance;

/// <summary>The family of a specification that an invoice declares.</summary>
public enum SpecificationFamily
{
    /// <summary>The identifier is not known.</summary>
    Unknown = 0,

    /// <summary>EN 16931 without a CIUS or an extension.</summary>
    En16931,

    /// <summary>ZUGFeRD 1.0 (urn:ferd:...).</summary>
    Zugferd1,

    /// <summary>ZUGFeRD 2.0 (urn:zugferd.de:2p0:...).</summary>
    Zugferd20,

    /// <summary>ZUGFeRD 2.1 or later, and Factur-X 1.0 (urn:factur-x.eu:1p0:...).</summary>
    FacturX,

    /// <summary>XRechnung, the German CIUS of EN 16931.</summary>
    XRechnung,

    /// <summary>Peppol BIS Billing 3.0.</summary>
    PeppolBis
}

/// <summary>The profile of a specification.</summary>
public enum InvoiceProfile
{
    Unknown = 0,
    Minimum,
    BasicWL,
    Basic,

    /// <summary>The COMFORT profile of ZUGFeRD 1.0.</summary>
    Comfort,

    /// <summary>EN 16931 (in ZUGFeRD 2.x and Factur-X also named COMFORT).</summary>
    En16931,
    Extended,
    XRechnung,
    XRechnungExtension,
    PeppolBisBilling
}

/// <summary>What the catalog knows about a specification identifier (BT-24).</summary>
public sealed record SpecificationInfo(
    string? Identifier,
    SpecificationFamily Family,
    InvoiceProfile Profile,
    string? Version)
{
    /// <summary>True if the catalog knows the identifier.</summary>
    public bool IsRecognized => Family != SpecificationFamily.Unknown;

    /// <summary>The name of the standard, for example "ZUGFeRD 2.1+ / Factur-X 1.0".</summary>
    public string? StandardName => SpecificationCatalog.NameOf(Family);

    /// <summary>The name of the profile, for example "EXTENDED".</summary>
    public string? ProfileName => SpecificationCatalog.NameOf(Profile);
}

/*
 * Gives the standard and the profile that a specification identifier (BT-24) declares.
 *
 * The identifier is a DECLARATION of the document. It does not tell that the document
 * keeps the rules of that profile: only a validation can tell that.
 *
 * The catalog knows these identifiers:
 *
 *   ZUGFeRD 1.0        urn:ferd:CrossIndustryDocument:invoice:1p0:basic|comfort|extended
 *   ZUGFeRD 2.0        urn:zugferd.de:2p0:minimum, ...:basicwl,
 *                      urn:cen.eu:en16931:2017#compliant#urn:zugferd.de:2p0:basic,
 *                      urn:cen.eu:en16931:2017#conformant#urn:zugferd.de:2p0:extended
 *   ZUGFeRD 2.1+ and   urn:factur-x.eu:1p0:minimum, ...:basicwl,
 *   Factur-X 1.0       urn:cen.eu:en16931:2017#compliant#urn:factur-x.eu:1p0:basic,
 *                      urn:cen.eu:en16931:2017#conformant#urn:factur-x.eu:1p0:extended
 *   EN 16931           urn:cen.eu:en16931:2017 (also the profile EN 16931 of ZUGFeRD 2.x
 *                      and Factur-X)
 *   XRechnung          ...#compliant#urn:xoev-de:kosit:standard:xrechnung_1.2 (1.x),
 *                      ...#compliant#urn:xeinkauf.de:kosit:xrechnung_3.0 (2.x and 3.x),
 *                      ...#conformant#urn:xeinkauf.de:kosit:extension:xrechnung_3.0
 *   Peppol BIS 3.0     ...#compliant#urn:fdc:peppol.eu:2017:poacc:billing:3.0
 */
public static class SpecificationCatalog
{
    private const string En16931Core = "urn:cen.eu:en16931:2017";

    private static readonly Regex XRechnungVersion =
        new(@"xrechnung_(?<version>\d+(\.\d+)*)", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);

    private static readonly Regex ProfileToken =
        new(@"(?<prefix>urn:ferd:crossindustrydocument:invoice:1p0|urn:zugferd\.de:2p0|urn:factur-x\.eu:1p0):(?<profile>[a-z]+)$",
            RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);

    /// <summary>Gives the standard and the profile of a specification identifier (BT-24).</summary>
    public static SpecificationInfo Identify(string? identifier)
    {
        string? id = identifier?.Trim();
        if (string.IsNullOrEmpty(id))
        {
            return new SpecificationInfo(identifier, SpecificationFamily.Unknown, InvoiceProfile.Unknown, null);
        }

        string lower = id.ToLowerInvariant();

        /* XRechnung first: its identifier starts with the identifier of EN 16931. */
        if (lower.Contains("kosit:extension:xrechnung"))
        {
            return new SpecificationInfo(id, SpecificationFamily.XRechnung, InvoiceProfile.XRechnungExtension, XRechnungVersionOf(id));
        }

        if (lower.Contains("kosit:standard:xrechnung") || lower.Contains("urn:xeinkauf.de:kosit:xrechnung"))
        {
            return new SpecificationInfo(id, SpecificationFamily.XRechnung, InvoiceProfile.XRechnung, XRechnungVersionOf(id));
        }

        if (lower.EndsWith("urn:fdc:peppol.eu:2017:poacc:billing:3.0", StringComparison.Ordinal))
        {
            return new SpecificationInfo(id, SpecificationFamily.PeppolBis, InvoiceProfile.PeppolBisBilling, "3.0");
        }

        Match token = ProfileToken.Match(lower);
        if (token.Success)
        {
            string prefix = token.Groups["prefix"].Value;
            InvoiceProfile profile = token.Groups["profile"].Value switch
            {
                "minimum" => InvoiceProfile.Minimum,
                "basicwl" => InvoiceProfile.BasicWL,
                "basic" => InvoiceProfile.Basic,
                "comfort" => InvoiceProfile.Comfort,
                "extended" => InvoiceProfile.Extended,
                _ => InvoiceProfile.Unknown
            };

            (SpecificationFamily family, string version) = prefix switch
            {
                "urn:ferd:crossindustrydocument:invoice:1p0" => (SpecificationFamily.Zugferd1, "1.0"),
                "urn:zugferd.de:2p0" => (SpecificationFamily.Zugferd20, "2.0"),
                _ => (SpecificationFamily.FacturX, "1.0")
            };

            /* COMFORT is a profile of ZUGFeRD 1.0 only, MINIMUM and BASIC WL of 2.x only. */
            bool valid = profile switch
            {
                InvoiceProfile.Unknown => false,
                InvoiceProfile.Comfort => family == SpecificationFamily.Zugferd1,
                InvoiceProfile.Minimum or InvoiceProfile.BasicWL => family != SpecificationFamily.Zugferd1,
                _ => true
            };

            if (valid)
            {
                return new SpecificationInfo(id, family, profile, version);
            }
        }

        if (lower == En16931Core)
        {
            return new SpecificationInfo(id, SpecificationFamily.En16931, InvoiceProfile.En16931, null);
        }

        return new SpecificationInfo(id, SpecificationFamily.Unknown, InvoiceProfile.Unknown, null);
    }

    /*
     * The profile of a conformance level of the XMP metadata of a PDF file, for example
     * "BASIC WL" or "EN 16931". The method ignores case, spaces, "_" and "-".
     */
    public static InvoiceProfile ProfileOfConformanceLevel(string? level)
    {
        return Normalize(level) switch
        {
            "MINIMUM" => InvoiceProfile.Minimum,
            "BASICWL" => InvoiceProfile.BasicWL,
            "BASIC" => InvoiceProfile.Basic,
            "COMFORT" => InvoiceProfile.Comfort,
            "EN16931" => InvoiceProfile.En16931,
            "EXTENDED" => InvoiceProfile.Extended,
            "XRECHNUNG" => InvoiceProfile.XRechnung,
            _ => InvoiceProfile.Unknown
        };
    }

    /// <summary>The name of a family, or null for <see cref="SpecificationFamily.Unknown"/>.</summary>
    public static string? NameOf(SpecificationFamily family) => family switch
    {
        SpecificationFamily.En16931 => "EN 16931",
        SpecificationFamily.Zugferd1 => "ZUGFeRD 1.0",
        SpecificationFamily.Zugferd20 => "ZUGFeRD 2.0",
        SpecificationFamily.FacturX => "ZUGFeRD 2.1+ / Factur-X 1.0",
        SpecificationFamily.XRechnung => "XRechnung",
        SpecificationFamily.PeppolBis => "Peppol BIS Billing",
        _ => null
    };

    /// <summary>The name of a profile, or null for <see cref="InvoiceProfile.Unknown"/>.</summary>
    public static string? NameOf(InvoiceProfile profile) => profile switch
    {
        InvoiceProfile.Minimum => "MINIMUM",
        InvoiceProfile.BasicWL => "BASIC WL",
        InvoiceProfile.Basic => "BASIC",
        InvoiceProfile.Comfort => "COMFORT",
        InvoiceProfile.En16931 => "EN 16931",
        InvoiceProfile.Extended => "EXTENDED",
        InvoiceProfile.XRechnung => "XRECHNUNG",
        InvoiceProfile.XRechnungExtension => "XRECHNUNG Extension",
        InvoiceProfile.PeppolBisBilling => "Billing",
        _ => null
    };

    internal static string Normalize(string? level)
    {
        if (level == null)
        {
            return string.Empty;
        }

        return new string(level.Where(c => c != ' ' && c != '_' && c != '-').ToArray()).ToUpperInvariant();
    }

    private static string? XRechnungVersionOf(string identifier)
    {
        Match match = XRechnungVersion.Match(identifier);
        return match.Success ? match.Groups["version"].Value : null;
    }
}
