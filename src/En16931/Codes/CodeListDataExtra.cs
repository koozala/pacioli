namespace En16931.Codes;

/// <summary>
/// Codes that documents of the practice hold, and that the official code list workbook
/// does not hold. The format is the same as <see cref="CodeListData"/>: one line per
/// entry, the code and the name divided by a vertical bar.
/// </summary>
/// <remarks>
/// <para>
/// <see cref="CodeListData"/> comes from the workbook of the specification, and a program
/// makes it. Do not put an entry there by hand, because the next run of the program
/// removes it. This file is the place for an entry that the workbook does not give.
/// </para>
/// <para>
/// An entry must have a reason, and the reason must name a document that holds the code.
/// An entry must not repeat a code of the official list; the test
/// <c>CodeListTests.NoExtraEntryRepeatsAnOfficialCode</c> guards this.
/// </para>
/// </remarks>
internal static class CodeListDataExtra
{
    /// <summary>
    /// Schemes of an electronic address (EAS).
    /// </summary>
    /// <remarks>
    /// 9958 was the scheme of the German Leitweg-ID. The EAS list does not give it any
    /// more, because the Leitweg-ID belongs to BT-10 today. Documents of the practice
    /// still use it: the samples <c>EN16931_ElektronischeAdresse</c> and
    /// <c>EN16931_RechnungsUebertragung</c> of the corpus hold it, in both syntaxes.
    /// Without this entry a page shows "9958" in the place of a name.
    /// </remarks>
    internal const string ElectronicAddressScheme =
        """
        9958|German Leitweg ID
        """;

    /// <summary>The German names of the entries above.</summary>
    internal const string ElectronicAddressSchemeDe =
        """
        9958|Leitweg-ID
        """;
}
