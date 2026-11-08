namespace En16931.Conformance;

/// <summary>The rule that a disagreement breaks.</summary>
public enum DeclarationRule
{
    /// <summary>The conformance level of the XMP metadata does not agree with the profile of BT-24.</summary>
    ConformanceLevel,

    /// <summary>The file name of the XMP metadata is not the name of the embedded XML file.</summary>
    DocumentFileName,

    /// <summary>The family of the XMP metadata (ZUGFeRD 1 or 2.x) does not agree with the XML syntax.</summary>
    XmpFamily,

    /// <summary>The PDF file is not PDF/A-3.</summary>
    PdfAPart
}

/// <summary>One disagreement. Expected and Actual are the values for a message.</summary>
public sealed record DeclarationIssue(DeclarationRule Rule, string Expected, string Actual);

/// <summary>The values that a document declares. Each value is optional.</summary>
public sealed record DeclarationInput
{
    /// <summary>BT-24.</summary>
    public string? SpecificationIdentifier { get; init; }

    /// <summary>The namespace of the root element of the XML.</summary>
    public string? XmlRootNamespace { get; init; }

    /// <summary>True if the invoice is an embedded file of a PDF file.</summary>
    public bool IsPdf { get; init; }

    /// <summary>The name of the embedded XML file, for example "factur-x.xml".</summary>
    public string? EmbeddedFileName { get; init; }

    /// <summary>The namespace of the ZUGFeRD or Factur-X properties of the XMP metadata.</summary>
    public string? XmpNamespace { get; init; }

    /// <summary>fx:ConformanceLevel.</summary>
    public string? XmpConformanceLevel { get; init; }

    /// <summary>fx:DocumentFileName.</summary>
    public string? XmpDocumentFileName { get; init; }

    /// <summary>pdfaid:part, for example "3".</summary>
    public string? PdfAPart { get; init; }
}

/*
 * Compares the declarations of a document and gives each disagreement.
 *
 * A document declares its profile in two places: in BT-24 of the XML, and for a PDF
 * file in the XMP metadata. The class compares only facts of the data. It does not
 * validate the content: a document without a disagreement can still break the rules
 * of its profile.
 *
 * The rules:
 *   1. The XMP conformance level agrees with the profile of BT-24.
 *   2. fx:DocumentFileName is the name of the embedded XML file.
 *   3. The XMP namespace of ZUGFeRD 1 goes with the XML of ZUGFeRD 1. The XMP namespaces
 *      of ZUGFeRD 2.0 and Factur-X go with CII D16B.
 *   4. A PDF file with an embedded invoice is PDF/A-3.
 *
 * A rule applies only if the values that it compares are in the document. The rules 1
 * to 3 need XMP metadata of ZUGFeRD or Factur-X.
 */
public static class DeclarationCheck
{
    public const string Zugferd1XmpNamespace = "urn:ferd:pdfa:CrossIndustryDocument:invoice:1p0#";
    public const string Zugferd20XmpNamespace = "urn:zugferd:pdfa:CrossIndustryDocument:invoice:2p0#";
    public const string FacturXXmpNamespace = "urn:factur-x:pdfa:CrossIndustryDocument:invoice:1p0#";

    public const string Zugferd1XmlNamespace = "urn:ferd:CrossIndustryDocument:invoice:1p0";
    public const string CiiXmlNamespace = "urn:un:unece:uncefact:data:standard:CrossIndustryInvoice:100";

    /// <summary>The family of an XMP namespace, or Unknown.</summary>
    public static SpecificationFamily FamilyOfXmpNamespace(string? xmpNamespace) => xmpNamespace switch
    {
        Zugferd1XmpNamespace => SpecificationFamily.Zugferd1,
        Zugferd20XmpNamespace => SpecificationFamily.Zugferd20,
        FacturXXmpNamespace => SpecificationFamily.FacturX,
        _ => SpecificationFamily.Unknown
    };

    public static IReadOnlyList<DeclarationIssue> Check(DeclarationInput input)
    {
        var issues = new List<DeclarationIssue>();

        if (!input.IsPdf)
        {
            return issues;
        }

        SpecificationFamily xmpFamily = FamilyOfXmpNamespace(input.XmpNamespace);
        bool hasXmp = xmpFamily != SpecificationFamily.Unknown;

        /* Rule 1 */
        if (hasXmp && input.XmpConformanceLevel != null)
        {
            SpecificationInfo declared = SpecificationCatalog.Identify(input.SpecificationIdentifier);
            if (declared.IsRecognized)
            {
                IReadOnlyList<InvoiceProfile> expected = LevelsFor(declared.Profile);
                InvoiceProfile actual = SpecificationCatalog.ProfileOfConformanceLevel(input.XmpConformanceLevel);

                if (!expected.Contains(actual))
                {
                    string expectedText = expected.Count == 0
                        ? "-"
                        : string.Join(" / ", expected.Select(profile => SpecificationCatalog.NameOf(profile)));
                    issues.Add(new DeclarationIssue(DeclarationRule.ConformanceLevel, expectedText, input.XmpConformanceLevel));
                }
            }
        }

        /* Rule 2 */
        if (hasXmp && input.XmpDocumentFileName != null && input.EmbeddedFileName != null
            && !string.Equals(input.XmpDocumentFileName.Trim(), input.EmbeddedFileName, StringComparison.Ordinal))
        {
            issues.Add(new DeclarationIssue(DeclarationRule.DocumentFileName, input.EmbeddedFileName, input.XmpDocumentFileName));
        }

        /* Rule 3 */
        if (hasXmp && input.XmlRootNamespace != null)
        {
            string expectedXml = xmpFamily == SpecificationFamily.Zugferd1 ? Zugferd1XmlNamespace : CiiXmlNamespace;
            if (!string.Equals(input.XmlRootNamespace, expectedXml, StringComparison.Ordinal))
            {
                issues.Add(new DeclarationIssue(DeclarationRule.XmpFamily, expectedXml, input.XmlRootNamespace));
            }
        }

        /* Rule 4 */
        if (input.PdfAPart?.Trim() != "3")
        {
            issues.Add(new DeclarationIssue(DeclarationRule.PdfAPart, "3", input.PdfAPart?.Trim() ?? "-"));
        }

        return issues;
    }

    /*
     * The conformance levels of the XMP metadata that go with a profile of BT-24.
     *
     * For EN 16931, the method also takes COMFORT: ZUGFeRD 2.x names the profile
     * "EN 16931 (COMFORT)", and some programs write COMFORT. Peppol BIS has no level of
     * ZUGFeRD or Factur-X, thus each level is a disagreement.
     */
    private static IReadOnlyList<InvoiceProfile> LevelsFor(InvoiceProfile profile) => profile switch
    {
        InvoiceProfile.Minimum => [InvoiceProfile.Minimum],
        InvoiceProfile.BasicWL => [InvoiceProfile.BasicWL],
        InvoiceProfile.Basic => [InvoiceProfile.Basic],
        InvoiceProfile.Comfort => [InvoiceProfile.Comfort],
        InvoiceProfile.En16931 => [InvoiceProfile.En16931, InvoiceProfile.Comfort],
        InvoiceProfile.Extended => [InvoiceProfile.Extended],
        InvoiceProfile.XRechnung => [InvoiceProfile.XRechnung],
        InvoiceProfile.XRechnungExtension => [InvoiceProfile.XRechnung, InvoiceProfile.Extended],
        _ => []
    };
}
