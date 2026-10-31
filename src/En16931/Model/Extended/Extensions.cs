namespace En16931.Model.Extended;

/// <summary>
/// Data that goes over the core of EN 16931.
/// </summary>
/// <remarks>
/// <para>
/// The core model holds the business terms of EN 16931-1, and nothing else. A document of
/// the ZUGFeRD EXTENDED profile holds more: an advance payment, a discount term, a serial
/// number of an article, and so on. A reader of the invoice needs this data, thus the
/// model must carry it — but it does not belong to the core, and a property of the core
/// must keep its BT or BG number.
/// </para>
/// <para>
/// Therefore the core types carry an <c>Extensions</c> property. The property is
/// <see langword="null"/> if the document gives no such data. A UBL document leaves most
/// of them null today; see the parser.
/// </para>
/// <para>
/// <b>What goes here, and what does not.</b> A term with a BT or a BG number belongs to
/// the core. A term without one belongs here. The renderer of Pacioli names each group
/// that it shows; do not add a group that no reader needs.
/// </para>
/// </remarks>
public sealed class InvoiceExtensions
{
    /// <summary>The title of the document, for example "RECHNUNG". CII only.</summary>
    public string? Title { get; set; }

    /// <summary>The party that gets the invoice, if it is not the buyer. CII only.</summary>
    public Party? Invoicee { get; set; }

    /// <summary>The party that sends the goods. CII only.</summary>
    public Party? ShipFrom { get; set; }

    /// <summary>
    /// The party that gets the goods, with its full data. The core holds the name, the
    /// location and the address of this party (BG-13 and BG-15), but not its identifiers,
    /// its tax registrations and its contacts.
    /// </summary>
    public Party? ShipTo { get; set; }

    /// <summary>
    /// Each invoiced object identifier. BT-18 gives one, and the core holds the first one,
    /// but a document of the EXTENDED profile can name more than one object.
    /// </summary>
    public List<Identifier> InvoicedObjectIdentifiers { get; } = [];

    /// <summary>The name of the project. The core holds its identifier (BT-11).</summary>
    public string? ProjectName { get; set; }

    /// <summary>The date of the purchase order. The core holds its number (BT-13).</summary>
    public DateOnly? PurchaseOrderDate { get; set; }

    /// <summary>The name of the sales order document. The core holds its number (BT-14).</summary>
    public string? SalesOrderName { get; set; }

    /// <summary>The quotation that the invoice names.</summary>
    public ReferencedDocument? Quotation { get; set; }

    /// <summary>The delivery note that the invoice names.</summary>
    public ReferencedDocument? DeliveryNote { get; set; }

    /// <summary>The despatch advice. The core holds its number (BT-16).</summary>
    public ReferencedDocument? DespatchAdvice { get; set; }

    /// <summary>The modes of transport (UN/ECE Recommendation 19).</summary>
    public List<string> TransportModeCodes { get; } = [];

    /// <summary>
    /// Each document that the head of the invoice names, as the document gives it.
    /// </summary>
    /// <remarks>
    /// CII puts three business terms in the same group and separates them by a type code:
    /// 130 is the invoiced object (BT-18), 50 is the tender or lot (BT-17), and each other
    /// code is a supporting document (BG-24). The core follows that split, thus a reader
    /// of the core cannot show the group as one list any more. This list keeps the group
    /// together, and a document appears here beside its place in the core.
    /// </remarks>
    public List<ReferencedDocument> AdditionalDocuments { get; } = [];

    /// <summary>The type of the accounting account. The core holds its identifier (BT-19).</summary>
    public string? AccountingAccountTypeCode { get; set; }

    /// <summary>The payment terms. The core holds the text of the first one (BT-20).</summary>
    public List<PaymentTerm> PaymentTerms { get; } = [];

    /// <summary>The payments that the buyer made before this invoice.</summary>
    public List<AdvancePayment> AdvancePayments { get; } = [];

    /// <summary>The costs of the transport.</summary>
    public List<LogisticsServiceCharge> LogisticsServiceCharges { get; } = [];

    /// <summary>The rate between the currency of the invoice and the currency of the tax.</summary>
    public CurrencyExchange? TaxCurrencyExchange { get; set; }
}

/// <summary>A document that the invoice names.</summary>
public sealed class ReferencedDocument
{
    /// <summary>The number that the issuer gave to the document.</summary>
    public string? Reference { get; set; }

    /// <summary>The name of the document.</summary>
    public string? Name { get; set; }

    /// <summary>The day of the document.</summary>
    public DateOnly? IssueDate { get; set; }

    /// <summary>The address of the document, if it is not in the invoice.</summary>
    public string? Uri { get; set; }

    /// <summary>The position of the invoice that the document belongs to.</summary>
    public string? LineId { get; set; }

    /// <summary>The type of the reference (UNTDID 1153).</summary>
    public string? ReferenceTypeCode { get; set; }
}

/// <summary>One payment term of the invoice.</summary>
public sealed class PaymentTerm
{
    /// <summary>The text of the term.</summary>
    public string? Description { get; set; }

    /// <summary>The day of the payment.</summary>
    public DateOnly? DueDate { get; set; }

    /// <summary>The reference of a SEPA mandate.</summary>
    public string? DirectDebitMandateReference { get; set; }

    /// <summary>
    /// What the buyer takes off for an early payment (Skonto). A term can give more than
    /// one discount, for example 3 % in 7 days and 2 % in 14 days. CII EXTENDED and UBL
    /// give one discount for each term. XRechnung gives each discount as one line in the
    /// text of the term (see <c>XRechnungDiscounts</c>).
    /// </summary>
    public List<PaymentTermCondition> Discounts { get; } = [];

    /// <summary>What the buyer pays for a late payment (Verzugszinsen).</summary>
    public PaymentTermCondition? Penalty { get; set; }
}

/// <summary>
/// A discount or a penalty of a payment term. Both have the same form: a percentage of a
/// basis amount, from a day, after a number of days.
/// </summary>
public sealed class PaymentTermCondition
{
    /// <summary>The percentage.</summary>
    public decimal? Percentage { get; set; }

    /// <summary>The amount itself.</summary>
    public Amount? Amount { get; set; }

    /// <summary>The amount that the percentage applies to.</summary>
    public Amount? BasisAmount { get; set; }

    /// <summary>The number of days.</summary>
    public decimal? BasisPeriodDays { get; set; }

    /// <summary>
    /// The day from which the days count. If a discount has no day, the days count from
    /// the invoice date (BT-2). XRechnung gives no day for a discount.
    /// </summary>
    public DateOnly? BasisDate { get; set; }
}

/// <summary>A payment that the buyer made before this invoice.</summary>
public sealed class AdvancePayment
{
    /// <summary>The amount of the payment.</summary>
    public Amount? PaidAmount { get; set; }

    /// <summary>The day of the payment.</summary>
    public DateOnly? ReceivedDate { get; set; }

    /// <summary>The tax in the amount.</summary>
    public List<TaxDetail> IncludedTax { get; } = [];

    /// <summary>The invoice of the payment.</summary>
    public ReferencedDocument? Invoice { get; set; }
}

/// <summary>The cost of a transport service.</summary>
public sealed class LogisticsServiceCharge
{
    /// <summary>What the charge is for.</summary>
    public string? Description { get; set; }

    /// <summary>The amount.</summary>
    public Amount? Amount { get; set; }

    /// <summary>The tax of the charge.</summary>
    public List<TaxDetail> Tax { get; } = [];
}

/// <summary>The rate between two currencies.</summary>
public sealed class CurrencyExchange
{
    /// <summary>The currency of the invoice.</summary>
    public string? SourceCurrencyCode { get; set; }

    /// <summary>The currency of the tax.</summary>
    public string? TargetCurrencyCode { get; set; }

    /// <summary>The rate.</summary>
    public decimal? Rate { get; set; }

    /// <summary>The day of the rate.</summary>
    public DateOnly? RateDate { get; set; }
}

/// <summary>
/// The full data of one tax. The core holds the VAT breakdown (BG-23) with the terms of
/// EN 16931; this type holds the values that a document of the EXTENDED profile adds.
/// </summary>
public sealed class TaxDetail
{
    /// <summary>The type of the tax (UNTDID 5153), for example <c>VAT</c>.</summary>
    public string? TypeCode { get; set; }

    /// <summary>The category of the tax (UNTDID 5305).</summary>
    public string? CategoryCode { get; set; }

    /// <summary>The rate.</summary>
    public decimal? RatePercent { get; set; }

    /// <summary>The amount of the tax.</summary>
    public Amount? CalculatedAmount { get; set; }

    /// <summary>The amount that the tax applies to.</summary>
    public Amount? BasisAmount { get; set; }

    /// <summary>The part of the basis that comes from the positions.</summary>
    public Amount? LineTotalBasisAmount { get; set; }

    /// <summary>The part of the basis that comes from the allowances and the charges.</summary>
    public Amount? AllowanceChargeBasisAmount { get; set; }

    /// <summary>The text of an exemption.</summary>
    public string? ExemptionReason { get; set; }

    /// <summary>The code of an exemption (VATEX).</summary>
    public string? ExemptionReasonCode { get; set; }

    /// <summary>The day of the tax.</summary>
    public DateOnly? TaxPointDate { get; set; }
}
