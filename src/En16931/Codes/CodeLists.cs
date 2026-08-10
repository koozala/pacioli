namespace En16931.Codes;

/// <summary>
/// The code lists of the specification, as lookup tables from a code to a readable name.
/// </summary>
/// <remarks>
/// <para>
/// The model keeps a code as it stands in the document, because a code is the value of the
/// business term. These lists give the name of a code, thus an application can show a text
/// and not a code: <c>H87</c> becomes <c>Stück</c>, and <c>AE</c> becomes
/// <c>Umkehrung der Steuerschuldnerschaft</c>.
/// </para>
/// <para>
/// The English entries come from the official code list workbook of Factur-X 1.09.2
/// (ZUGFeRD 2.5.2). The specification gives no German code list. The German names are
/// therefore kept by hand and they cover the lists and the codes that a German invoice
/// really shows. A code without a German name falls back to the English name, thus
/// <see cref="CodeList.GetName(string?, CodeLanguage)"/> always gives a result for a known
/// code. <see cref="GermanCoverage"/> tells how far the German table goes.
/// </para>
/// <para>
/// A list reads its entries on first use, thus a list that the application never touches
/// costs no time and no memory. Use <see cref="CodeList.Describe(string?, CodeLanguage)"/>
/// for a display or for a log, because it also gives a result for a code that the list
/// does not hold.
/// </para>
/// <para>
/// The lists come from the project <c>Vit.FeRD.ExtendedDocument</c>. Some of them belong to
/// a business term that the core model does not hold; the doc comment names them. These
/// lists stay, because a document of the EXTENDED profile holds such a code.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// var invoice = InvoiceReader.ReadFile("invoice.xml");
///
/// Console.WriteLine(CodeLists.DocumentType.GetName(invoice.InvoiceTypeCode));
///
/// foreach (var vat in invoice.VatBreakdown)
///     Console.WriteLine($"{CodeLists.TaxCategory.Describe(vat.CategoryCode, CodeLanguage.German)} " +
///                       $"{vat.CategoryRate}%");
/// </code>
/// </example>
public static class CodeLists
{
    /// <summary>
    /// Document type codes (UNTDID 1001) for BT-3, the invoice type code.
    /// </summary>
    public static CodeList DocumentType { get; } =
        new("1001", "Document type code", CodeListData.DocumentType, CodeListDataDe.DocumentType);

    /// <summary>
    /// Reference type codes (UNTDID 1153) for the type of a referenced document.
    /// The core model does not hold this term.
    /// </summary>
    public static CodeList ReferenceType { get; } =
        new("1153", "Reference type code", CodeListData.ReferenceType, CodeListDataDe.ReferenceType);

    /// <summary>
    /// Tax category codes (UNTDID 5305) for BT-118 of the VAT breakdown, BT-151 of a
    /// position, and BT-95 and BT-102 of an allowance or of a charge.
    /// </summary>
    public static CodeList TaxCategory { get; } =
        new("5305", "Tax category code", CodeListData.TaxCategory, CodeListDataDe.TaxCategory);

    /// <summary>
    /// Tax type codes (UNTDID 5153). EN 16931 allows <c>VAT</c> only, thus the core model
    /// does not hold this term. The EXTENDED profile also allows the other codes.
    /// </summary>
    public static CodeList TaxType { get; } =
        new("5153", "Tax type code", CodeListData.TaxType, CodeListDataDe.TaxType);

    /// <summary>
    /// Reason codes for a tax exemption (VATEX) for BT-121 of the VAT breakdown.
    /// </summary>
    public static CodeList TaxExemptionReason { get; } =
        new("VATEX", "Tax exemption reason code", CodeListData.TaxExemptionReason);

    /// <summary>
    /// Time reference codes (UNTDID 2475) for BT-8, the code of the value added tax
    /// point date.
    /// </summary>
    public static CodeList TimeReference { get; } =
        new("2475", "Time reference code", CodeListData.TimeReference, CodeListDataDe.TimeReference);

    /// <summary>
    /// Payment means codes (UNTDID 4461) for BT-81, the payment means type code.
    /// </summary>
    public static CodeList PaymentMeans { get; } =
        new("4461", "Payment means code", CodeListData.PaymentMeans, CodeListDataDe.PaymentMeans);

    /// <summary>
    /// Subject codes of a note (UNTDID 4451) for BT-21, the invoice note subject code.
    /// </summary>
    public static CodeList NoteSubject { get; } =
        new("4451", "Note subject code", CodeListData.NoteSubject, CodeListDataDe.NoteSubject);

    /// <summary>
    /// Reason codes of an allowance (UNTDID 5189) for BT-98 and BT-140, if
    /// <see cref="Model.AllowanceCharge.IsCharge"/> is <see langword="false"/>.
    /// Use <see cref="ReasonFor"/> to get the correct list.
    /// </summary>
    public static CodeList AllowanceReason { get; } =
        new("5189", "Allowance reason code", CodeListData.AllowanceReason, CodeListDataDe.AllowanceReason);

    /// <summary>
    /// Reason codes of a charge (UNTDID 7161) for BT-105 and BT-145, if
    /// <see cref="Model.AllowanceCharge.IsCharge"/> is <see langword="true"/>.
    /// Use <see cref="ReasonFor"/> to get the correct list.
    /// </summary>
    public static CodeList ChargeReason { get; } =
        new("7161", "Charge reason code", CodeListData.ChargeReason, CodeListDataDe.ChargeReason);

    /// <summary>
    /// Codes of an item classification (UNTDID 7143) for BT-158-1, the scheme of an entry
    /// of <see cref="Model.LineItem.ClassificationIdentifiers"/>.
    /// </summary>
    public static CodeList ItemTypeIdentification { get; } =
        new("7143", "Item type identification code", CodeListData.ItemTypeIdentification, CodeListDataDe.ItemTypeIdentification);

    /// <summary>
    /// Codes of an item attribute (UNTDID 6313 and Factur-X). The core model does not hold
    /// this term: BG-32 gives the name of an attribute as a text.
    /// </summary>
    public static CodeList ProductCharacteristic { get; } =
        new("6313", "Product characteristic code", CodeListData.ProductCharacteristic);

    /// <summary>
    /// Line status codes (UNTDID 1229). The core model does not hold this term. A document
    /// of the EXTENDED profile uses it for a sub invoice line.
    /// </summary>
    public static CodeList LineStatus { get; } =
        new("1229", "Line status code", CodeListData.LineStatus, CodeListDataDe.LineStatus);

    /// <summary>
    /// Reason codes of the line status (Factur-X). The core model does not hold this term.
    /// </summary>
    public static CodeList LineStatusReason { get; } =
        new("Factur-X", "Line status reason code", CodeListData.LineStatusReason, CodeListDataDe.LineStatusReason);

    /// <summary>
    /// Format codes of a date (UNTDID 2379). The model holds a date as a
    /// <see cref="DateOnly"/>, thus the format code stays in the document. The parser
    /// reads it; see the CII date in the parsing helpers.
    /// </summary>
    public static CodeList DateFormat { get; } =
        new("2379", "Date format code", CodeListData.DateFormat, CodeListDataDe.DateFormat);

    /// <summary>
    /// Units of measure (UN/ECE Recommendation 20 and 21) for BT-130 and BT-150, the unit
    /// of a <see cref="Model.Quantity"/>.
    /// </summary>
    public static CodeList UnitOfMeasure { get; } =
        new("Rec 20/21", "Unit of measure code", CodeListData.UnitOfMeasure, CodeListDataDe.UnitOfMeasure);

    /// <summary>
    /// Country codes (ISO 3166-1 alpha-2) for
    /// <see cref="Model.PostalAddress.CountryCode"/> and for BT-159, the country of origin
    /// of an item.
    /// </summary>
    public static CodeList Country { get; } =
        new("ISO 3166-1", "Country code", CodeListData.Country, CodeListDataDe.Country);

    /// <summary>
    /// Currency codes (ISO 4217) for BT-5 and BT-6, and for the currency of an
    /// <see cref="Model.Amount"/>.
    /// </summary>
    public static CodeList Currency { get; } =
        new("ISO 4217", "Currency code", CodeListData.Currency, CodeListDataDe.Currency);

    /// <summary>
    /// Language codes (ISO 639-2). The core model does not hold this term.
    /// </summary>
    public static CodeList Language { get; } =
        new("ISO 639-2", "Language code", CodeListData.Language);

    /// <summary>
    /// Schemes of an electronic address (EAS) for BT-34-1 and BT-49-1, the scheme of
    /// <see cref="Model.Party.ElectronicAddress"/>.
    /// </summary>
    public static CodeList ElectronicAddressScheme { get; } =
        new("EAS", "Electronic address scheme",
            CodeListData.ElectronicAddressScheme, CodeListDataDe.ElectronicAddressScheme,
            CodeListDataExtra.ElectronicAddressScheme, CodeListDataExtra.ElectronicAddressSchemeDe);

    /// <summary>
    /// Schemes of an identifier (ISO/IEC 6523 ICD) for BT-29-1, the scheme of an entry of
    /// <see cref="Model.Party.Identifiers"/>, and for BT-157-1, the scheme of
    /// <see cref="Model.LineItem.StandardIdentifier"/>.
    /// </summary>
    public static CodeList IdentifierScheme { get; } =
        new("ICD", "Identifier scheme code", CodeListData.IdentifierScheme, CodeListDataDe.IdentifierScheme);

    /// <summary>
    /// MIME types that the specification allows for BT-125-1, the media type of
    /// <see cref="Model.Attachment.MimeCode"/>.
    /// </summary>
    public static CodeList MimeCode { get; } =
        new("MIME", "MIME type", CodeListData.MimeCode, CodeListDataDe.MimeCode);

    /// <summary>
    /// Terms of delivery (Incoterms). The core model does not hold this term.
    /// </summary>
    public static CodeList Incoterms { get; } =
        new("INCOTERMS", "Delivery terms code", CodeListData.Incoterms, CodeListDataDe.Incoterms);

    /// <summary>
    /// Modes of transport (UN/ECE Recommendation 19). The core model does not hold this
    /// term. A document of the EXTENDED profile gives it with the delivery.
    /// </summary>
    public static CodeList TransportMode { get; } =
        new("Rec 19", "Transport mode code", CodeListData.TransportMode, CodeListDataDe.TransportMode);

    /// <summary>All code lists of this class.</summary>
    public static IReadOnlyList<CodeList> All { get; } =
    [
        DocumentType, ReferenceType, TaxCategory, TaxType, TaxExemptionReason, TimeReference,
        PaymentMeans, NoteSubject, AllowanceReason, ChargeReason, ItemTypeIdentification,
        ProductCharacteristic, LineStatus, LineStatusReason, DateFormat, UnitOfMeasure,
        Country, Currency, Language, ElectronicAddressScheme, IdentifierScheme, MimeCode,
        Incoterms, TransportMode
    ];

    /// <summary>
    /// Gives the list with the given <see cref="CodeList.Id"/>, for example <c>1001</c>, or
    /// <see langword="null"/> if no list has this identifier.
    /// </summary>
    public static CodeList? ById(string id) =>
        All.FirstOrDefault(list => string.Equals(list.Id, id, StringComparison.OrdinalIgnoreCase));

    /// <summary>
    /// Gives the reason codes that belong to an allowance or to a charge:
    /// <see cref="ChargeReason"/> for a charge, <see cref="AllowanceReason"/> for an allowance.
    /// </summary>
    public static CodeList ReasonFor(Model.AllowanceCharge allowanceCharge) =>
        allowanceCharge.IsCharge ? ChargeReason : AllowanceReason;

    /// <summary>
    /// The lists that have German names, together with the number of German entries and
    /// the number of all entries. Use it to see where the German table stops.
    /// </summary>
    public static IEnumerable<(CodeList List, int German, int Total)> GermanCoverage =>
        All.Where(list => list.GermanCount > 0)
           .Select(list => (list, list.GermanCount, list.Count));
}
