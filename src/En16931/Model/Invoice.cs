namespace En16931.Model;

/// <summary>
/// BG-0 — an EN 16931 core invoice, independent of the XML syntax it was read from.
/// Property names follow the business term names of EN 16931-1; every property
/// carries its BT or BG number.
/// </summary>
public sealed class Invoice
{
    /// <summary>The syntax the instance was read from.</summary>
    public InvoiceSyntax Syntax { get; set; }

    // --- BG-2 PROCESS CONTROL -------------------------------------------------

    /// <summary>BT-24 Specification identifier. Identifies the rule set the invoice follows.</summary>
    public string? SpecificationIdentifier { get; set; }

    /// <summary>BT-23 Business process type.</summary>
    public string? BusinessProcessType { get; set; }

    // --- document level -------------------------------------------------------

    /// <summary>BT-1 Invoice number.</summary>
    public string? InvoiceNumber { get; set; }

    /// <summary>BT-2 Invoice issue date.</summary>
    public DateOnly? IssueDate { get; set; }

    /// <summary>BT-3 Invoice type code (UNTDID 1001), for example 380 or 381.</summary>
    public string? InvoiceTypeCode { get; set; }

    /// <summary>BT-5 Invoice currency code (ISO 4217).</summary>
    public string? CurrencyCode { get; set; }

    /// <summary>BT-6 VAT accounting currency code.</summary>
    public string? TaxCurrencyCode { get; set; }

    /// <summary>BT-7 Value added tax point date.</summary>
    public DateOnly? TaxPointDate { get; set; }

    /// <summary>BT-8 Value added tax point date code.</summary>
    public string? TaxPointDateCode { get; set; }

    /// <summary>BT-9 Payment due date.</summary>
    public DateOnly? PaymentDueDate { get; set; }

    /// <summary>BT-10 Buyer reference.</summary>
    public string? BuyerReference { get; set; }

    /// <summary>BT-11 Project reference.</summary>
    public string? ProjectReference { get; set; }

    /// <summary>BT-12 Contract reference.</summary>
    public string? ContractReference { get; set; }

    /// <summary>BT-13 Purchase order reference.</summary>
    public string? PurchaseOrderReference { get; set; }

    /// <summary>BT-14 Sales order reference.</summary>
    public string? SalesOrderReference { get; set; }

    /// <summary>BT-15 Receiving advice reference.</summary>
    public string? ReceivingAdviceReference { get; set; }

    /// <summary>BT-16 Despatch advice reference.</summary>
    public string? DespatchAdviceReference { get; set; }

    /// <summary>BT-17 Tender or lot reference.</summary>
    public string? TenderOrLotReference { get; set; }

    /// <summary>BT-18 Invoiced object identifier.</summary>
    public Identifier? InvoicedObjectIdentifier { get; set; }

    /// <summary>BT-19 Buyer accounting reference.</summary>
    public string? BuyerAccountingReference { get; set; }

    /// <summary>BT-20 Payment terms.</summary>
    public string? PaymentTerms { get; set; }

    /// <summary>BG-1 Invoice notes.</summary>
    public List<InvoiceNote> Notes { get; } = [];

    /// <summary>BG-3 Preceding invoice references.</summary>
    public List<PrecedingInvoiceReference> PrecedingInvoiceReferences { get; } = [];

    /// <summary>BG-4 Seller.</summary>
    public Party? Seller { get; set; }

    /// <summary>BG-7 Buyer.</summary>
    public Party? Buyer { get; set; }

    /// <summary>BG-10 Payee.</summary>
    public Party? Payee { get; set; }

    /// <summary>BG-11 Seller tax representative party.</summary>
    public Party? SellerTaxRepresentative { get; set; }

    /// <summary>BG-13 Delivery information.</summary>
    public DeliveryInformation? Delivery { get; set; }

    /// <summary>BG-14 Invoicing period.</summary>
    public Period? InvoicingPeriod { get; set; }

    /// <summary>BG-16 Payment instructions.</summary>
    public PaymentInstructions? PaymentInstructions { get; set; }

    /// <summary>BG-20 Document level allowances.</summary>
    public List<AllowanceCharge> Allowances { get; } = [];

    /// <summary>BG-21 Document level charges.</summary>
    public List<AllowanceCharge> Charges { get; } = [];

    /// <summary>BG-22 Document totals.</summary>
    public DocumentTotals Totals { get; set; } = new();

    /// <summary>BG-23 VAT breakdown.</summary>
    public List<VatBreakdown> VatBreakdown { get; } = [];

    /// <summary>BG-24 Additional supporting documents.</summary>
    public List<SupportingDocument> SupportingDocuments { get; } = [];

    /// <summary>BG-25 Invoice lines.</summary>
    public List<InvoiceLine> Lines { get; } = [];

    /// <summary>Data that goes over the core. Null if the document gives none.</summary>
    public Extended.InvoiceExtensions? Extensions { get; set; }
}

/// <summary>BG-1 — one invoice note.</summary>
public sealed class InvoiceNote
{
    /// <summary>BT-21 Invoice note subject code (UNTDID 4451).</summary>
    public string? SubjectCode { get; set; }

    /// <summary>BT-22 Invoice note.</summary>
    public string? Text { get; set; }

    /// <summary>Data that goes over the core. Null if the document gives none.</summary>
    public Extended.NoteExtensions? Extensions { get; set; }
}

/// <summary>BG-3 — a reference to a preceding invoice.</summary>
public sealed class PrecedingInvoiceReference
{
    /// <summary>BT-25 Preceding invoice reference.</summary>
    public string? InvoiceNumber { get; set; }

    /// <summary>BT-26 Preceding invoice issue date.</summary>
    public DateOnly? IssueDate { get; set; }

    /// <summary>Data that goes over the core. Null if the document gives none.</summary>
    public Extended.PrecedingInvoiceExtensions? Extensions { get; set; }
}

/// <summary>BG-24 — an additional supporting document.</summary>
public sealed class SupportingDocument
{
    /// <summary>BT-122 Supporting document reference.</summary>
    public string? Reference { get; set; }

    /// <summary>BT-123 Supporting document description.</summary>
    public string? Description { get; set; }

    /// <summary>BT-124 External document location.</summary>
    public string? ExternalLocation { get; set; }

    /// <summary>BT-125 Attached document.</summary>
    public Attachment? Attachment { get; set; }

    /// <summary>Data that goes over the core. Null if the document gives none.</summary>
    public Extended.SupportingDocumentExtensions? Extensions { get; set; }
}

/// <summary>BG-14 and BG-26 — a date range.</summary>
public sealed class Period
{
    /// <summary>BT-73 / BT-134 Period start date.</summary>
    public DateOnly? StartDate { get; set; }

    /// <summary>BT-74 / BT-135 Period end date.</summary>
    public DateOnly? EndDate { get; set; }
}
