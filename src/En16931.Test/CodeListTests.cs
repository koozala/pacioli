using En16931.Codes;
using En16931.Model;

namespace En16931.Test;

/*
 * The code lists give the name of a code.
 *
 * The renderer shows the name and not the code: the document holds "H87", and the page
 * shows "Stück". Thus a list that loses an entry makes an empty place on the page, or a
 * code where a text belongs.
 */
[TestFixture]
public class CodeListTests
{
    /* The lists, for the test cases that apply to each one. */
    private static IEnumerable<CodeList> AllLists => CodeLists.All;

    [Test]
    public void TheClassHolds24Lists()
    {
        Assert.That(CodeLists.All, Has.Count.EqualTo(24));
    }

    [TestCaseSource(nameof(AllLists))]
    public void TheListHasEntries(CodeList list)
    {
        Assert.That(list.Count, Is.GreaterThan(0), $"{list.Name} ({list.Id}) is empty.");
    }

    [TestCaseSource(nameof(AllLists))]
    public void TheListHasAnIdAndAName(CodeList list)
    {
        Assert.That(list.Id, Is.Not.Empty);
        Assert.That(list.Name, Is.Not.Empty);
    }

    /* Each German entry must also be in the English list. */
    [TestCaseSource(nameof(AllLists))]
    public void EachGermanEntryHasAnEnglishEntry(CodeList list)
    {
        var without = list.GermanEntries.Keys
            .Where(code => !list.Entries.ContainsKey(code))
            .ToList();

        Assert.That(without, Is.Empty,
            $"{list.Name}: the German table holds a code that the official list does not hold: " +
            string.Join(", ", without));
    }

    /*
     * An entry of CodeListDataExtra fills a hole of the official list. The parser gives
     * the official name priority, thus an extra entry can never change a name of the
     * specification. This test case controls that each extra entry is in the list.
     */
    [TestCaseSource(nameof(AllLists))]
    public void EachExtraEntryIsInTheList(CodeList list)
    {
        foreach (var code in list.ExtraEntries.Keys)
        {
            Assert.That(list.Entries.ContainsKey(code), Is.True,
                $"{list.Name}: the extra entry \"{code}\" is not in the list.");
        }
    }

    [Test]
    public void ElectronicAddressScheme_KnowsTheLeitwegId()
    {
        Assert.That(CodeLists.ElectronicAddressScheme.GetName("9958"), Is.EqualTo("German Leitweg ID"));
        Assert.That(CodeLists.ElectronicAddressScheme.GetName("9958", CodeLanguage.German),
            Is.EqualTo("Leitweg-ID"));
    }

    /* The names of the specification. */
    [TestCase("380", "Commercial invoice")]
    [TestCase("381", "Credit note")]
    [TestCase("384", "Corrected invoice")]
    public void DocumentType_GivesTheEnglishName(string code, string expected)
    {
        Assert.That(CodeLists.DocumentType.GetName(code), Is.EqualTo(expected));
    }

    [TestCase("380", "Handelsrechnung")]
    [TestCase("381", "Gutschrift")]
    public void DocumentType_GivesTheGermanName(string code, string expected)
    {
        Assert.That(CodeLists.DocumentType.GetName(code, CodeLanguage.German), Is.EqualTo(expected));
    }

    /* The codes that the renderer of Pacioli shows most. */
    [Test]
    public void UnitOfMeasure_KnowsThePiece()
    {
        Assert.That(CodeLists.UnitOfMeasure.GetName("H87"), Is.EqualTo("piece"));
        Assert.That(CodeLists.UnitOfMeasure.GetName("H87", CodeLanguage.German), Is.EqualTo("Stück"));
    }

    [Test]
    public void TaxCategory_KnowsTheReverseCharge()
    {
        Assert.That(CodeLists.TaxCategory.GetName("AE"), Is.Not.Null);
        Assert.That(CodeLists.TaxCategory.GetName("AE", CodeLanguage.German),
            Does.Contain("Reverse Charge").IgnoreCase);
    }

    [Test]
    public void TaxExemptionReason_KnowsAVatexCode()
    {
        Assert.That(CodeLists.TaxExemptionReason.GetName("VATEX-EU-O"), Is.Not.Null);
    }

    /* An unknown code gives null, and Describe still gives a usable text. */
    [Test]
    public void GetName_GivesNull_ForAnUnknownCode()
    {
        Assert.That(CodeLists.DocumentType.GetName("ZZZ"), Is.Null);
    }

    [Test]
    public void Describe_GivesTheCodeAlone_ForAnUnknownCode()
    {
        Assert.That(CodeLists.DocumentType.Describe("ZZZ"), Is.EqualTo("ZZZ"));
    }

    [Test]
    public void Describe_GivesTheCodeAndTheName()
    {
        Assert.That(CodeLists.DocumentType.Describe("380"), Is.EqualTo("380 (Commercial invoice)"));
        Assert.That(CodeLists.DocumentType.Describe("380", CodeLanguage.German),
            Is.EqualTo("380 (Handelsrechnung)"));
    }

    [Test]
    public void GetName_GivesNull_ForNull()
    {
        Assert.That(CodeLists.DocumentType.GetName(null), Is.Null);
    }

    [Test]
    public void Describe_GivesAnEmptyText_ForNullAndForSpace()
    {
        Assert.That(CodeLists.DocumentType.Describe(null), Is.Empty);
        Assert.That(CodeLists.DocumentType.Describe("   "), Is.Empty);
    }

    /* A code with a space around it must give its name. The XML can hold such a value. */
    [Test]
    public void GetName_TakesACodeWithSpaceAround()
    {
        Assert.That(CodeLists.DocumentType.GetName(" 380 "), Is.EqualTo("Commercial invoice"));
    }

    /* The codes are case sensitive, thus a wrong case must not give a name. */
    [Test]
    public void GetName_IsCaseSensitive()
    {
        Assert.That(CodeLists.UnitOfMeasure.GetName("h87"), Is.Null);
    }

    /*
     * A German name falls back to the English name. VATEX has no German table, thus this
     * list shows the fallback.
     */
    [Test]
    public void GetName_FallsBackToTheEnglishName()
    {
        string? english = CodeLists.TaxExemptionReason.GetName("VATEX-EU-O");
        string? german = CodeLists.TaxExemptionReason.GetName("VATEX-EU-O", CodeLanguage.German);

        Assert.That(CodeLists.TaxExemptionReason.GetGermanName("VATEX-EU-O"), Is.Null);
        Assert.That(german, Is.EqualTo(english));
    }

    [Test]
    public void ById_FindsAList()
    {
        Assert.That(CodeLists.ById("1001"), Is.SameAs(CodeLists.DocumentType));
        Assert.That(CodeLists.ById("5305"), Is.SameAs(CodeLists.TaxCategory));
        Assert.That(CodeLists.ById("no such list"), Is.Null);
    }

    /* An allowance and a charge take their reason from two different lists. */
    [Test]
    public void ReasonFor_TakesTheListOfTheAllowanceOrOfTheCharge()
    {
        Assert.That(CodeLists.ReasonFor(new AllowanceCharge { IsCharge = false }),
            Is.SameAs(CodeLists.AllowanceReason));
        Assert.That(CodeLists.ReasonFor(new AllowanceCharge { IsCharge = true }),
            Is.SameAs(CodeLists.ChargeReason));
    }

    [Test]
    public void GermanCoverage_NamesTheListsWithAGermanTable()
    {
        var coverage = CodeLists.GermanCoverage.ToList();

        Assert.That(coverage, Is.Not.Empty);
        Assert.That(coverage.All(entry => entry.German > 0), Is.True);
        Assert.That(coverage.All(entry => entry.German <= entry.Total), Is.True);

        /* These three lists have no German table. See the README of the library. */
        var names = coverage.Select(entry => entry.List.Id).ToList();
        Assert.That(names, Does.Not.Contain("VATEX"));
        Assert.That(names, Does.Not.Contain("6313"));
        Assert.That(names, Does.Not.Contain("ISO 639-2"));
    }
}
