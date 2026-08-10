namespace En16931.Model.Extended;

/// <summary>
/// Data of one position that goes over the core of EN 16931.
/// See <see cref="InvoiceExtensions"/> for the reason of this group of types.
/// </summary>
public sealed class InvoiceLineExtensions
{
    /// <summary>The day of the delivery of this position.</summary>
    public DateOnly? ActualDeliveryDate { get; set; }

    /// <summary>
    /// Each note of the position. The core holds the text of the first one (BT-127).
    /// </summary>
    public List<InvoiceNote> Notes { get; } = [];

    /// <summary>The party that gets the goods of this position.</summary>
    public Party? ShipTo { get; set; }

    /// <summary>The type of the accounting account. The core holds its identifier (BT-133).</summary>
    public string? AccountingAccountTypeCode { get; set; }

    /// <summary>How many packages the position holds. EN 16931 has no term for it.</summary>
    public Quantity? PackageQuantity { get; set; }

    /// <summary>How many units the position charges, if it is not the invoiced quantity.</summary>
    public Quantity? ChargeFreeQuantity { get; set; }

    /// <summary>
    /// The full tax of the position. The core holds the category and the rate (BG-30).
    /// </summary>
    public List<TaxDetail> Tax { get; } = [];

    /// <summary>The purchase order that the position names.</summary>
    public ReferencedDocument? PurchaseOrder { get; set; }

    /// <summary>The contract that the position names.</summary>
    public ReferencedDocument? Contract { get; set; }

    /// <summary>The quotation that the position names.</summary>
    public ReferencedDocument? Quotation { get; set; }

    /// <summary>The delivery note that the position names.</summary>
    public ReferencedDocument? DeliveryNote { get; set; }

    /// <summary>
    /// The documents of the position, with their data. A document of a position can hold
    /// an attachment, for example a bill of quantities.
    /// </summary>
    public List<SupportingDocument> Documents { get; } = [];
}

/// <summary>
/// Data of an article that goes over the core of EN 16931.
/// </summary>
public sealed class LineItemExtensions
{
    /// <summary>
    /// The single articles of the position, with their serial number and their batch.
    /// </summary>
    public List<ProductInstance> Instances { get; } = [];

    /// <summary>The articles inside a set, for example the parts of a bundle.</summary>
    public List<IncludedProduct> IncludedProducts { get; } = [];

    /// <summary>
    /// The name of each classification, in the order of
    /// <see cref="LineItem.ClassificationIdentifiers"/>. The core holds the code (BT-158)
    /// and not the name.
    /// </summary>
    public List<string?> ClassificationNames { get; } = [];

    /// <summary>The identifier that the industry gave to the article.</summary>
    public string? IndustryIdentifier { get; set; }
}

/// <summary>One single article of a position.</summary>
public sealed class ProductInstance
{
    /// <summary>The serial number that the seller gave.</summary>
    public string? SerialId { get; set; }

    /// <summary>The batch of the article.</summary>
    public string? BatchId { get; set; }
}

/// <summary>An article inside a set.</summary>
public sealed class IncludedProduct
{
    /// <summary>The name of the article.</summary>
    public string? Name { get; set; }

    /// <summary>The description of the article.</summary>
    public string? Description { get; set; }

    /// <summary>The identifier that the seller gave.</summary>
    public string? SellerIdentifier { get; set; }

    /// <summary>The identifier that the buyer gave.</summary>
    public string? BuyerIdentifier { get; set; }

    /// <summary>The identifier that the industry gave.</summary>
    public string? IndustryIdentifier { get; set; }

    /// <summary>The identifiers under a scheme, for example a GTIN.</summary>
    public List<Identifier> GlobalIdentifiers { get; } = [];

    /// <summary>How many of the article are in the set.</summary>
    public Quantity? UnitQuantity { get; set; }
}

/// <summary>
/// Data of the price of a position that goes over the core of EN 16931.
/// </summary>
public sealed class PriceExtensions
{
    /// <summary>
    /// The base quantity of the net price. The core holds one base quantity (BT-149),
    /// but the net price and the gross price can have a different one.
    /// </summary>
    public Quantity? NetBaseQuantity { get; set; }

    /// <summary>The base quantity of the gross price.</summary>
    public Quantity? GrossBaseQuantity { get; set; }

    /// <summary>
    /// The discount on the gross price, with its percentage, its basis and its reason.
    /// The core holds the amount alone (BT-147).
    /// </summary>
    public List<AllowanceCharge> GrossPriceAllowances { get; } = [];
}
