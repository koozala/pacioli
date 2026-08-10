namespace En16931.Model;

/// <summary>
/// BG-4 Seller, BG-7 Buyer, BG-10 Payee and BG-11 Seller tax representative party.
/// The four groups carry the same kind of information under different BT numbers;
/// the BT numbers per role are listed on each property.
/// </summary>
public sealed class Party
{
    /// <summary>BT-29 / BT-46 / BT-60 Party identifiers.</summary>
    public List<Identifier> Identifiers { get; } = [];

    /// <summary>BT-27 / BT-44 / BT-59 / BT-62 Party name.</summary>
    public string? Name { get; set; }

    /// <summary>BT-28 / BT-45 Trading name.</summary>
    public string? TradingName { get; set; }

    /// <summary>BT-30 / BT-47 / BT-61 Legal registration identifier.</summary>
    public Identifier? LegalRegistrationIdentifier { get; set; }

    /// <summary>BT-33 Seller additional legal information.</summary>
    public string? AdditionalLegalInformation { get; set; }

    /// <summary>BT-31 / BT-48 / BT-63 VAT identifier.</summary>
    public string? VatIdentifier { get; set; }

    /// <summary>BT-32 Tax registration identifier — a registration other than VAT.</summary>
    public string? TaxRegistrationIdentifier { get; set; }

    /// <summary>BT-34 / BT-49 Electronic address.</summary>
    public Identifier? ElectronicAddress { get; set; }

    /// <summary>BT-90 Bank assigned creditor identifier (SEPA).</summary>
    public string? BankAssignedCreditorIdentifier { get; set; }

    /// <summary>BG-5 / BG-8 / BG-12 Postal address.</summary>
    public PostalAddress? PostalAddress { get; set; }

    /// <summary>BG-6 / BG-9 Contact.</summary>
    public Contact? Contact { get; set; }
}

/// <summary>
/// BG-5 Seller, BG-8 Buyer, BG-12 Tax representative and BG-15 Deliver to address.
/// </summary>
public sealed class PostalAddress
{
    /// <summary>BT-35 / BT-50 / BT-64 / BT-75 Address line 1.</summary>
    public string? AddressLine1 { get; set; }

    /// <summary>BT-36 / BT-51 / BT-65 / BT-76 Address line 2.</summary>
    public string? AddressLine2 { get; set; }

    /// <summary>BT-162 / BT-163 / BT-164 / BT-165 Address line 3.</summary>
    public string? AddressLine3 { get; set; }

    /// <summary>BT-37 / BT-52 / BT-66 / BT-77 City.</summary>
    public string? City { get; set; }

    /// <summary>BT-38 / BT-53 / BT-67 / BT-78 Post code.</summary>
    public string? PostCode { get; set; }

    /// <summary>BT-39 / BT-54 / BT-68 / BT-79 Country subdivision.</summary>
    public string? CountrySubdivision { get; set; }

    /// <summary>BT-40 / BT-55 / BT-69 / BT-80 Country code (ISO 3166-1 alpha-2).</summary>
    public string? CountryCode { get; set; }
}

/// <summary>BG-6 Seller contact and BG-9 Buyer contact.</summary>
public sealed class Contact
{
    /// <summary>BT-41 / BT-56 Contact point.</summary>
    public string? Name { get; set; }

    /// <summary>BT-42 / BT-57 Contact telephone number.</summary>
    public string? Telephone { get; set; }

    /// <summary>BT-43 / BT-58 Contact email address.</summary>
    public string? Email { get; set; }
}

/// <summary>BG-13 Delivery information.</summary>
public sealed class DeliveryInformation
{
    /// <summary>BT-70 Deliver to party name.</summary>
    public string? PartyName { get; set; }

    /// <summary>BT-71 Deliver to location identifier.</summary>
    public Identifier? LocationIdentifier { get; set; }

    /// <summary>BT-72 Actual delivery date.</summary>
    public DateOnly? ActualDeliveryDate { get; set; }

    /// <summary>BG-15 Deliver to address.</summary>
    public PostalAddress? Address { get; set; }
}
