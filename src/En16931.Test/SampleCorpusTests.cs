using System.Text;
using System.Xml.Linq;
using En16931.Model;
using En16931.Parsing;

namespace En16931.Test;

/*
 * Reads each XML sample of the configured directories.
 *
 * The test does not compare values. It answers three questions:
 *
 *   1. Does the reader find the syntax of each document?
 *   2. Does the reader give a model for each document, without an exception?
 *   3. How many documents of each syntax does the corpus hold?
 *
 * The third question is the reason for this test in phase 0 of the migration: it tells
 * you the size of the UBL work.
 *
 * A document of an other standard, for example the Italian FatturaPA, must give the
 * syntax Unknown. The list NotAnEn16931Invoice names the directories of such documents.
 * A document that gives Unknown and that no entry explains makes the test fail.
 *
 * Configuration: see SampleDirectories and testsettings.json. If no directory of the
 * settings is on this computer, each test case here gives the result "ignored".
 */
[TestFixture]
public class SampleCorpusTests
{
    /* The directory of the attachments of an invoice. These files are not invoices. */
    private static readonly string[] ExcludeDirectories = ["attachments"];

    /*
     * Documents of an other standard. The path of the sample must hold one of these
     * parts. Keep a reason with each entry.
     */
    private static readonly (string Part, string Reason)[] NotAnEn16931Invoice =
    [
        ("fatturaPA", "The Italian FatturaPA. It is not one of the two syntaxes of EN 16931.")
    ];

    private static IReadOnlyList<string> Samples => SampleDirectories.XmlFiles(ExcludeDirectories);

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

    [Test]
    public void SamplesDirectories_AreOnThisComputer()
    {
        var configured = SampleDirectories.Configured();

        if (configured.Count == 0)
        {
            Assert.Ignore(
                "No samples directory is configured. Put the paths in testsettings.json, " +
                "or set the environment variable PACIOLI_SAMPLES_DIRS.");
        }

        var missing = configured.Where(entry => !Directory.Exists(entry)).ToList();

        if (missing.Count == configured.Count)
        {
            Assert.Ignore($"No configured directory is on this computer: {string.Join("; ", missing)}");
        }

        Assert.That(missing, Is.Empty,
            $"These directories of testsettings.json are not on this computer: {string.Join("; ", missing)}");
    }

    [TestCaseSource(nameof(SampleCases))]
    public void Sample_ReadsIntoTheModel(string? sample)
    {
        if (sample == null)
        {
            Assert.Ignore("No sample directory is on this computer.");
            return;
        }

        /*
         * A document of an other standard is out of scope. Look at it before the XML
         * step, because such a document can also be incomplete: the corpus holds
         * documents with an artificial error.
         */
        string? outOfScope = ReasonOutOfScope(sample);
        if (outOfScope != null)
        {
            Assert.Ignore(outOfScope);
            return;
        }

        XDocument document;
        try
        {
            document = XDocument.Load(sample, LoadOptions.None);
        }
        catch (System.Xml.XmlException error)
        {
            Assert.Fail($"The file is not well formed XML: {error.Message}");
            return;
        }

        InvoiceSyntax syntax = InvoiceReader.DetectSyntax(document);

        if (syntax == InvoiceSyntax.Unknown)
        {
            Assert.Fail(
                $"The reader does not know the root element \"{document.Root?.Name}\". " +
                "If the document is not an EN 16931 invoice, add an entry to NotAnEn16931Invoice.");
            return;
        }

        Invoice invoice;
        try
        {
            invoice = InvoiceReader.Read(document);
        }
        catch (Exception error)
        {
            Assert.Fail($"The reader gives {error.GetType().Name}: {error.Message}");
            return;
        }

        Assert.That(invoice.Syntax, Is.EqualTo(syntax),
            "The syntax of the model must agree with the syntax that DetectSyntax gives.");
    }

    /*
     * Counts the samples of each syntax, and writes the counts to the console.
     *
     * This test case does not fail on a count. It is a report. It fails only if the
     * corpus holds no invoice at all, because then the configuration is wrong.
     */
    [Test]
    public void Corpus_Survey()
    {
        var samples = Samples;

        if (samples.Count == 0)
        {
            Assert.Ignore("No sample directory is on this computer.");
            return;
        }

        var counts = new Dictionary<InvoiceSyntax, int>
        {
            [InvoiceSyntax.Cii] = 0,
            [InvoiceSyntax.Ubl] = 0,
            [InvoiceSyntax.Unknown] = 0
        };

        var byDirectory = new SortedDictionary<string, (int Cii, int Ubl, int Unknown)>(
            StringComparer.OrdinalIgnoreCase);

        var notWellFormed = new List<string>();
        int outOfScope = 0;

        foreach (string sample in samples)
        {
            if (ReasonOutOfScope(sample) != null)
            {
                outOfScope++;
                continue;
            }

            InvoiceSyntax syntax;
            try
            {
                syntax = InvoiceReader.DetectSyntax(XDocument.Load(sample, LoadOptions.None));
            }
            catch (System.Xml.XmlException)
            {
                notWellFormed.Add(SampleDirectories.ShortName(sample));
                continue;
            }

            counts[syntax]++;

            string group = GroupOf(sample);
            var entry = byDirectory.TryGetValue(group, out var found) ? found : (0, 0, 0);
            byDirectory[group] = syntax switch
            {
                InvoiceSyntax.Cii => (entry.Item1 + 1, entry.Item2, entry.Item3),
                InvoiceSyntax.Ubl => (entry.Item1, entry.Item2 + 1, entry.Item3),
                _ => (entry.Item1, entry.Item2, entry.Item3 + 1)
            };
        }

        var report = new StringBuilder();
        report.AppendLine($"XML samples : {samples.Count}");
        report.AppendLine($"CII         : {counts[InvoiceSyntax.Cii]}");
        report.AppendLine($"UBL         : {counts[InvoiceSyntax.Ubl]}");
        report.AppendLine($"Unknown     : {counts[InvoiceSyntax.Unknown]}");
        report.AppendLine($"Out of scope: {outOfScope} (an other standard)");
        report.AppendLine($"Not XML     : {notWellFormed.Count}");
        report.AppendLine();
        report.AppendLine("Group                                     CII    UBL  Unknown");

        foreach (var (group, entry) in byDirectory)
        {
            report.AppendLine($"{group,-40} {entry.Cii,5} {entry.Ubl,6} {entry.Unknown,8}");
        }

        if (notWellFormed.Count > 0)
        {
            report.AppendLine();
            report.AppendLine("Files that are not well formed XML:");
            notWellFormed.ForEach(file => report.AppendLine($"  {file}"));
        }

        TestContext.Out.WriteLine(report.ToString());

        Assert.That(counts[InvoiceSyntax.Cii] + counts[InvoiceSyntax.Ubl], Is.GreaterThan(0),
            "The corpus holds no EN 16931 invoice. Look at the paths in testsettings.json.");
    }

    /* The reason why a sample is not an EN 16931 invoice, or null. */
    private static string? ReasonOutOfScope(string sample) => NotAnEn16931Invoice
        .Where(entry => sample.Contains(entry.Part, StringComparison.OrdinalIgnoreCase))
        .Select(entry => entry.Reason)
        .FirstOrDefault();

    /* The first two parts of the path below the samples directory. */
    private static string GroupOf(string sample)
    {
        string[] parts = SampleDirectories.ShortName(sample)
            .Split(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);

        return parts.Length <= 1 ? "." : string.Join('/', parts.Take(Math.Min(2, parts.Length - 1)));
    }
}
