namespace En16931;

/// <summary>The XML syntaxes EN 16931 permits, per CEN/TS 16931-2.</summary>
public enum InvoiceSyntax
{
    /// <summary>The syntax could not be determined.</summary>
    Unknown = 0,

    /// <summary>UN/CEFACT Cross Industry Invoice — the syntax ZUGFeRD and Factur-X use.</summary>
    Cii,

    /// <summary>OASIS UBL 2.1 Invoice or CreditNote.</summary>
    Ubl
}
