namespace En16931.Model;

/// <summary>BG-16 Payment instructions.</summary>
public sealed class PaymentInstructions
{
    /// <summary>BT-81 Payment means type code (UNTDID 4461).</summary>
    public string? PaymentMeansTypeCode { get; set; }

    /// <summary>BT-82 Payment means text.</summary>
    public string? PaymentMeansText { get; set; }

    /// <summary>BT-83 Remittance information.</summary>
    public string? RemittanceInformation { get; set; }

    /// <summary>BG-17 Credit transfers.</summary>
    public List<CreditTransfer> CreditTransfers { get; } = [];

    /// <summary>BG-18 Payment card information.</summary>
    public PaymentCard? PaymentCard { get; set; }

    /// <summary>BG-19 Direct debit.</summary>
    public DirectDebit? DirectDebit { get; set; }
}

/// <summary>BG-17 Credit transfer.</summary>
public sealed class CreditTransfer
{
    /// <summary>BT-84 Payment account identifier (IBAN or a proprietary account number).</summary>
    public string? AccountIdentifier { get; set; }

    /// <summary>BT-85 Payment account name.</summary>
    public string? AccountName { get; set; }

    /// <summary>BT-86 Payment service provider identifier (BIC).</summary>
    public string? ServiceProviderIdentifier { get; set; }
}

/// <summary>BG-18 Payment card information.</summary>
public sealed class PaymentCard
{
    /// <summary>BT-87 Payment card primary account number. Never a full PAN — see BR-51.</summary>
    public string? PrimaryAccountNumber { get; set; }

    /// <summary>BT-88 Payment card holder name.</summary>
    public string? HolderName { get; set; }
}

/// <summary>BG-19 Direct debit.</summary>
public sealed class DirectDebit
{
    /// <summary>BT-89 Mandate reference identifier.</summary>
    public string? MandateReference { get; set; }

    /// <summary>BT-91 Debited account identifier.</summary>
    public string? DebitedAccountIdentifier { get; set; }
}

/// <summary>
/// BG-20 / BG-21 document level and BG-27 / BG-28 line level allowances and charges.
/// Both syntaxes model an allowance and a charge with the same structure and
/// separate them by an indicator, so one type serves all four groups.
/// </summary>
public sealed class AllowanceCharge
{
    /// <summary><c>true</c> for a charge (BG-21 / BG-28), <c>false</c> for an allowance (BG-20 / BG-27).</summary>
    public bool IsCharge { get; set; }

    /// <summary>BT-92 / BT-99 / BT-136 / BT-141 Amount.</summary>
    public decimal? Amount { get; set; }

    /// <summary>BT-93 / BT-100 / BT-137 / BT-142 Base amount.</summary>
    public decimal? BaseAmount { get; set; }

    /// <summary>BT-94 / BT-101 / BT-138 / BT-143 Percentage.</summary>
    public decimal? Percentage { get; set; }

    /// <summary>BT-97 / BT-104 / BT-139 / BT-144 Reason.</summary>
    public string? Reason { get; set; }

    /// <summary>BT-98 / BT-105 / BT-140 / BT-145 Reason code (UNTDID 5189 / 7161).</summary>
    public string? ReasonCode { get; set; }

    /// <summary>BT-95 / BT-102 VAT category code. Document level only.</summary>
    public string? VatCategoryCode { get; set; }

    /// <summary>BT-96 / BT-103 VAT rate. Document level only.</summary>
    public decimal? VatRate { get; set; }
}

/// <summary>BG-22 Document totals.</summary>
public sealed class DocumentTotals
{
    /// <summary>BT-106 Sum of invoice line net amounts.</summary>
    public decimal? LineTotalAmount { get; set; }

    /// <summary>BT-107 Sum of allowances on document level.</summary>
    public decimal? AllowanceTotalAmount { get; set; }

    /// <summary>BT-108 Sum of charges on document level.</summary>
    public decimal? ChargeTotalAmount { get; set; }

    /// <summary>BT-109 Invoice total amount without VAT.</summary>
    public decimal? TaxExclusiveAmount { get; set; }

    /// <summary>BT-110 Invoice total VAT amount, in the invoice currency (BT-5).</summary>
    public Amount? TaxAmount { get; set; }

    /// <summary>BT-111 Invoice total VAT amount, in the VAT accounting currency (BT-6).</summary>
    public Amount? TaxAmountInAccountingCurrency { get; set; }

    /// <summary>BT-112 Invoice total amount with VAT.</summary>
    public decimal? TaxInclusiveAmount { get; set; }

    /// <summary>BT-113 Paid amount.</summary>
    public decimal? PaidAmount { get; set; }

    /// <summary>BT-114 Rounding amount.</summary>
    public decimal? RoundingAmount { get; set; }

    /// <summary>BT-115 Amount due for payment.</summary>
    public decimal? DuePayableAmount { get; set; }
}

/// <summary>BG-23 VAT breakdown — one entry per VAT category and rate.</summary>
public sealed class VatBreakdown
{
    /// <summary>BT-116 VAT category taxable amount.</summary>
    public decimal? TaxableAmount { get; set; }

    /// <summary>BT-117 VAT category tax amount.</summary>
    public decimal? TaxAmount { get; set; }

    /// <summary>BT-118 VAT category code (UNTDID 5305), for example S, Z, E, AE, K, G, O.</summary>
    public string? CategoryCode { get; set; }

    /// <summary>BT-119 VAT category rate.</summary>
    public decimal? CategoryRate { get; set; }

    /// <summary>BT-120 VAT exemption reason text.</summary>
    public string? ExemptionReasonText { get; set; }

    /// <summary>BT-121 VAT exemption reason code (VATEX).</summary>
    public string? ExemptionReasonCode { get; set; }
}
