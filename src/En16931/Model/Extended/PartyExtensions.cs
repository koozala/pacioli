namespace En16931.Model.Extended;

/// <summary>
/// Data of a party that goes over the core of EN 16931. See <see cref="InvoiceExtensions"/>
/// for the reason of this group of types.
/// </summary>
public sealed class PartyExtensions
{
    /// <summary>
    /// Each contact person of the party. The core holds the first one (BG-6 and BG-9),
    /// because EN 16931 gives one contact per party.
    /// </summary>
    public List<Contact> Contacts { get; } = [];

    /// <summary>
    /// The identifiers of the party that stand under a scheme, apart from the plain
    /// identifiers. The core puts both in one list (BT-29), thus a reader cannot see
    /// which is which.
    /// </summary>
    public List<Identifier> GlobalIdentifiers { get; } = [];

    /// <summary>
    /// Each tax registration of the party, with its scheme, for example <c>VA</c> for the
    /// VAT number and <c>FC</c> for the tax number. The core holds one of each (BT-31 and
    /// BT-32) and it loses the scheme.
    /// </summary>
    public List<Identifier> TaxRegistrations { get; } = [];

    /// <summary>The address of the legal organisation, if it is not the postal address.</summary>
    public PostalAddress? LegalOrganizationAddress { get; set; }

    /// <summary>The number of the electronic address, beside its identifier (BT-34).</summary>
    public string? ElectronicAddressNumber { get; set; }
}

/// <summary>
/// Data of an invoice note (BG-1) that goes over the core.
/// </summary>
public sealed class NoteExtensions
{
    /// <summary>
    /// The code of the content of the note (UNTDID 4451). The core holds the code of the
    /// subject (BT-21).
    /// </summary>
    public string? ContentCode { get; set; }
}

/// <summary>
/// Data of a contact person that goes over the core of EN 16931. EN 16931 gives a contact
/// a name, a telephone number and an email address; a document can give more.
/// </summary>
public sealed class ContactExtensions
{
    /// <summary>The department of the person, for example "Buchhaltung".</summary>
    public string? DepartmentName { get; set; }

    /// <summary>The fax number.</summary>
    public string? Fax { get; set; }

    /// <summary>The code of the role of the contact.</summary>
    public string? TypeCode { get; set; }
}

/// <summary>
/// Data of a supporting document (BG-24) that goes over the core.
/// </summary>
public sealed class SupportingDocumentExtensions
{
    /// <summary>The day of the document.</summary>
    public DateOnly? IssueDate { get; set; }

    /// <summary>The position of the invoice that the document belongs to.</summary>
    public string? LineId { get; set; }

    /// <summary>The type of the document (UNTDID 1001).</summary>
    public string? TypeCode { get; set; }

    /// <summary>The type of the reference (UNTDID 1153).</summary>
    public string? ReferenceTypeCode { get; set; }
}

/// <summary>
/// Data of a preceding invoice (BG-3) that goes over the core.
/// </summary>
public sealed class PrecedingInvoiceExtensions
{
    /// <summary>The name of the document.</summary>
    public string? Name { get; set; }

    /// <summary>The address of the document.</summary>
    public string? Uri { get; set; }

    /// <summary>The type of the reference (UNTDID 1153).</summary>
    public string? ReferenceTypeCode { get; set; }

    /// <summary>The position of the invoice that the document belongs to.</summary>
    public string? LineId { get; set; }
}

/// <summary>
/// Data of the payment instructions (BG-16) that goes over the core.
/// </summary>
public sealed class PaymentExtensions
{
    /// <summary>The name of the account of the payer, for a direct debit.</summary>
    public string? DebitedAccountName { get; set; }

    /// <summary>The bank of the payer (BIC), for a direct debit.</summary>
    public string? DebitedServiceProviderIdentifier { get; set; }

    /// <summary>
    /// The account number of the payee that is not an IBAN. The core puts the IBAN and
    /// this number in one property (BT-84).
    /// </summary>
    public List<string> ProprietaryAccountIdentifiers { get; } = [];
}
