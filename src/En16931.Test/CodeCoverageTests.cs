using System.Text;
using System.Xml.Linq;
using En16931.Codes;
using En16931.Model;

namespace En16931.Test;

/*
 * Each code of each sample document must give a name.
 *
 * The test reads a sample into the model, then it takes each value that is a code and
 * looks for it in the list of that code. The model is the same for both syntaxes, thus
 * one test covers CII and UBL.
 *
 * A code without a name is a hole in a code list. The renderer then shows the code, and
 * not the text of the code: the page shows "H87" and not "Stück".
 *
 * The test also covers the parser: a code that comes to the wrong property does not agree
 * with its list, and the test names the sample and the term.
 *
 * If no samples directory is on this computer, each test case here gives the result
 * "ignored". See SampleDirectories.
 */
[TestFixture]
public class CodeCoverageTests
{
    private static readonly string[] ExcludeDirectories = ["attachments"];

    /* Documents of an other standard. See SampleCorpusTests. */
    private static readonly string[] OutOfScope = ["fatturaPA"];

    /* One value that must be in a list: the term, the list, and the code. */
    private sealed record CodeUse(string Term, CodeList List, string Code);

    private static IReadOnlyList<string> Samples => SampleDirectories
        .XmlFiles(ExcludeDirectories)
        .Where(sample => !OutOfScope.Any(part => sample.Contains(part, StringComparison.OrdinalIgnoreCase)))
        .ToList();

    private static IEnumerable<TestCaseData> SampleCases()
    {
        var samples = Samples;

        if (samples.Count == 0)
        {
            yield return new TestCaseData((string?)null).SetName("no sample directory");
            yield break;
        }

        foreach (string sample in samples)
        {
            yield return new TestCaseData(sample).SetName(SampleDirectories.ShortName(sample));
        }
    }

    [TestCaseSource(nameof(SampleCases))]
    public void Sample_EachCodeGivesAName(string? sample)
    {
        if (sample == null)
        {
            Assert.Ignore("No sample directory is on this computer.");
            return;
        }

        Invoice invoice;
        try
        {
            invoice = InvoiceReader.Read(XDocument.Load(sample, LoadOptions.None));
        }
        catch (Exception error)
        {
            Assert.Ignore($"The reader does not read this document: {error.GetType().Name}. " +
                          "SampleCorpusTests reports the cause.");
            return;
        }

        var without = CodeUsesOf(invoice)
            .Where(use => use.List.GetName(use.Code) is null)
            .ToList();

        if (without.Count == 0)
        {
            return;
        }

        var message = new StringBuilder();
        message.AppendLine($"{without.Count} code(s) of this sample have no name:");

        foreach (var use in without.DistinctBy(use => (use.Term, use.Code)))
        {
            message.AppendLine($"  {use.Term}: \"{use.Code}\" is not in {use.List.Name} ({use.List.Id})");
        }

        Assert.Fail(message.ToString());
    }

    /* Each code of one invoice, with the list that must hold it. */
    private static IEnumerable<CodeUse> CodeUsesOf(Invoice invoice)
    {
        var uses = new List<CodeUse>();

        void Add(string term, CodeList list, string? code)
        {
            if (!string.IsNullOrWhiteSpace(code))
            {
                uses.Add(new CodeUse(term, list, code!.Trim()));
            }
        }

        void AddAddress(string term, PostalAddress? address) =>
            Add(term, CodeLists.Country, address?.CountryCode);

        void AddParty(string term, Party? party)
        {
            if (party is null) return;
            AddAddress($"{term} country", party.PostalAddress);
            Add($"{term} electronic address scheme", CodeLists.ElectronicAddressScheme,
                party.ElectronicAddress?.SchemeId);
        }

        Add("BT-3 invoice type code", CodeLists.DocumentType, invoice.InvoiceTypeCode);
        Add("BT-5 currency", CodeLists.Currency, invoice.CurrencyCode);
        Add("BT-6 tax currency", CodeLists.Currency, invoice.TaxCurrencyCode);
        Add("BT-8 tax point date code", CodeLists.TimeReference, invoice.TaxPointDateCode);

        foreach (var note in invoice.Notes)
        {
            Add("BT-21 note subject code", CodeLists.NoteSubject, note.SubjectCode);
        }

        AddParty("seller", invoice.Seller);
        AddParty("buyer", invoice.Buyer);
        AddParty("payee", invoice.Payee);
        AddParty("tax representative", invoice.SellerTaxRepresentative);
        AddAddress("delivery country", invoice.Delivery?.Address);

        Add("BT-81 payment means type code", CodeLists.PaymentMeans,
            invoice.PaymentInstructions?.PaymentMeansTypeCode);

        foreach (var vat in invoice.VatBreakdown)
        {
            Add("BT-118 VAT category code", CodeLists.TaxCategory, vat.CategoryCode);
            Add("BT-121 VAT exemption reason code", CodeLists.TaxExemptionReason, vat.ExemptionReasonCode);
        }

        foreach (var entry in invoice.Allowances.Concat(invoice.Charges))
        {
            Add("BT-98 / BT-105 reason code", CodeLists.ReasonFor(entry), entry.ReasonCode);
            Add("BT-95 / BT-102 VAT category code", CodeLists.TaxCategory, entry.VatCategoryCode);
        }

        /*
         * BT-125-1, the media type of an attachment, is not in this test. The
         * specification allows six media types only, and a document of the practice can
         * hold an other one, for example "application/octet-stream". Such a value is not
         * a hole in a code list: the media type is already a readable text, thus the page
         * shows it as it stands. The list CodeLists.MimeCode says which media types the
         * specification allows; that question belongs to a validation and not here.
         */

        foreach (var line in invoice.Lines)
        {
            Add("BT-130 unit of measure", CodeLists.UnitOfMeasure, line.InvoicedQuantity?.UnitCode);
            Add("BT-150 price base unit", CodeLists.UnitOfMeasure, line.Price.BaseQuantity?.UnitCode);
            Add("BT-151 line VAT category code", CodeLists.TaxCategory, line.Vat.CategoryCode);
            Add("BT-159 country of origin", CodeLists.Country, line.Item.CountryOfOrigin);
            Add("BT-157-1 item standard identifier scheme", CodeLists.IdentifierScheme,
                line.Item.StandardIdentifier?.SchemeId);

            foreach (var classification in line.Item.ClassificationIdentifiers)
            {
                Add("BT-158-1 item classification scheme", CodeLists.ItemTypeIdentification,
                    classification.SchemeId);
            }

            foreach (var entry in line.Allowances.Concat(line.Charges))
            {
                Add("BT-140 / BT-145 reason code", CodeLists.ReasonFor(entry), entry.ReasonCode);
            }
        }

        return uses;
    }
}
