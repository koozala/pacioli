using System.Xml.Linq;

namespace En16931.Parsing;

/// <summary>XML namespaces of the two syntaxes.</summary>
public static class Namespaces
{
    // --- UN/CEFACT CII (D16B) ---

    /// <summary>CII document namespace.</summary>
    public static readonly XNamespace Rsm = "urn:un:unece:uncefact:data:standard:CrossIndustryInvoice:100";

    /// <summary>CII reusable aggregate business information entities.</summary>
    public static readonly XNamespace Ram = "urn:un:unece:uncefact:data:standard:ReusableAggregateBusinessInformationEntity:100";

    /// <summary>CII unqualified data types.</summary>
    public static readonly XNamespace Udt = "urn:un:unece:uncefact:data:standard:UnqualifiedDataType:100";

    /// <summary>CII qualified data types.</summary>
    public static readonly XNamespace Qdt = "urn:un:unece:uncefact:data:standard:QualifiedDataType:100";

    // --- OASIS UBL 2.1 ---

    /// <summary>UBL Invoice document namespace.</summary>
    public static readonly XNamespace UblInvoice = "urn:oasis:names:specification:ubl:schema:xsd:Invoice-2";

    /// <summary>UBL CreditNote document namespace.</summary>
    public static readonly XNamespace UblCreditNote = "urn:oasis:names:specification:ubl:schema:xsd:CreditNote-2";

    /// <summary>UBL common aggregate components.</summary>
    public static readonly XNamespace Cac = "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2";

    /// <summary>UBL common basic components.</summary>
    public static readonly XNamespace Cbc = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2";
}
