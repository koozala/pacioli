namespace En16931.Model;

/// <summary>BG-25 Invoice line.</summary>
public sealed class InvoiceLine
{
    /// <summary>BT-126 Invoice line identifier.</summary>
    public string? Id { get; set; }

    /// <summary>BT-127 Invoice line note.</summary>
    public string? Note { get; set; }

    /// <summary>BT-128 Invoice line object identifier.</summary>
    public Identifier? ObjectIdentifier { get; set; }

    /// <summary>BT-129 Invoiced quantity, with BT-130 as its unit of measure.</summary>
    public Quantity? InvoicedQuantity { get; set; }

    /// <summary>BT-131 Invoice line net amount.</summary>
    public decimal? NetAmount { get; set; }

    /// <summary>BT-132 Referenced purchase order line reference.</summary>
    public string? OrderLineReference { get; set; }

    /// <summary>BT-133 Invoice line buyer accounting reference.</summary>
    public string? BuyerAccountingReference { get; set; }

    /// <summary>BG-26 Invoice line period.</summary>
    public Period? Period { get; set; }

    /// <summary>BG-27 Invoice line allowances.</summary>
    public List<AllowanceCharge> Allowances { get; } = [];

    /// <summary>BG-28 Invoice line charges.</summary>
    public List<AllowanceCharge> Charges { get; } = [];

    /// <summary>BG-29 Price details.</summary>
    public PriceDetails Price { get; set; } = new();

    /// <summary>BG-30 Line VAT information.</summary>
    public LineVatInformation Vat { get; set; } = new();

    /// <summary>BG-31 Item information.</summary>
    public LineItem Item { get; set; } = new();

    /// <summary>Data that goes over the core. Null if the document gives none.</summary>
    public Extended.InvoiceLineExtensions? Extensions { get; set; }
}

/// <summary>BG-29 Price details.</summary>
public sealed class PriceDetails
{
    /// <summary>BT-146 Item net price.</summary>
    public decimal? NetPrice { get; set; }

    /// <summary>BT-147 Item price discount.</summary>
    public decimal? Discount { get; set; }

    /// <summary>BT-148 Item gross price.</summary>
    public decimal? GrossPrice { get; set; }

    /// <summary>BT-149 Item price base quantity, with BT-150 as its unit of measure.</summary>
    public Quantity? BaseQuantity { get; set; }

    /// <summary>Data that goes over the core. Null if the document gives none.</summary>
    public Extended.PriceExtensions? Extensions { get; set; }
}

/// <summary>BG-30 Line VAT information.</summary>
public sealed class LineVatInformation
{
    /// <summary>BT-151 Invoiced item VAT category code.</summary>
    public string? CategoryCode { get; set; }

    /// <summary>BT-152 Invoiced item VAT rate.</summary>
    public decimal? Rate { get; set; }
}

/// <summary>BG-31 Item information.</summary>
public sealed class LineItem
{
    /// <summary>BT-153 Item name.</summary>
    public string? Name { get; set; }

    /// <summary>BT-154 Item description.</summary>
    public string? Description { get; set; }

    /// <summary>BT-155 Item seller's identifier.</summary>
    public string? SellerIdentifier { get; set; }

    /// <summary>BT-156 Item buyer's identifier.</summary>
    public string? BuyerIdentifier { get; set; }

    /// <summary>BT-157 Item standard identifier, for example a GTIN.</summary>
    public Identifier? StandardIdentifier { get; set; }

    /// <summary>BT-158 Item classification identifiers.</summary>
    public List<Identifier> ClassificationIdentifiers { get; } = [];

    /// <summary>BT-159 Item country of origin.</summary>
    public string? CountryOfOrigin { get; set; }

    /// <summary>BG-32 Item attributes.</summary>
    public List<ItemAttribute> Attributes { get; } = [];

    /// <summary>Data that goes over the core. Null if the document gives none.</summary>
    public Extended.LineItemExtensions? Extensions { get; set; }
}

/// <summary>BG-32 Item attribute.</summary>
public sealed class ItemAttribute
{
    /// <summary>BT-160 Item attribute name.</summary>
    public string? Name { get; set; }

    /// <summary>BT-161 Item attribute value.</summary>
    public string? Value { get; set; }
}
