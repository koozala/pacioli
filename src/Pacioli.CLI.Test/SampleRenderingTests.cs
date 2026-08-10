using System.Text;
using Pacioli.CLI.Test.Content;
using Vit.FeRD.ExtendedDocument.Model;

namespace Pacioli.CLI.Test
{
    /*
     * Renders each sample document with the CLI tool pacioli-cmd, and checks the result.
     *
     * Each sample has two test cases:
     *
     *   Sample_RendersToPdf
     *     The CLI tool must make a good PDF file. The case fails if:
     *       - the CLI tool stops with an error code,
     *       - the CLI tool writes to the error stream,
     *       - the CLI tool does not stop before the timeout,
     *       - the CLI tool writes no PDF file, or an incomplete PDF file.
     *
     *   Sample_RenderedPdf_ShowsKeyElements
     *     The key elements of the invoice must be visible on the rendered PDF.
     *     The class InvoiceElementRules holds the set of elements. To check more
     *     elements, add a rule to that class.
     *
     * The CLI tool renders each sample one time only. The two test cases use the same
     * PDF file.
     *
     * The PDF files go to a temporary directory. The test deletes this directory
     * at the end of the run.
     *
     * Configuration: see testsettings.json and the class TestSettings.
     */
    [TestFixture]
    public class SampleRenderingTests
    {
        private static readonly TestSettings Settings = TestSettings.Load();
        private static readonly IReadOnlyList<SampleFile> Samples = SampleLocator.Find(Settings);

        private readonly Dictionary<int, RenderedSample> _renders = new Dictionary<int, RenderedSample>();
        private readonly List<string> _renderProblems = new List<string>();
        private readonly List<string> _contentProblems = new List<string>();
        private readonly Dictionary<string, int> _knownGapHits = new Dictionary<string, int>();
        private readonly XmlCoverageReport _report = new XmlCoverageReport();
        private readonly object _lock = new object();

        private IReadOnlyList<InvoiceElementRule> _rules = Array.Empty<InvoiceElementRule>();
        private string _temporaryDirectory = string.Empty;
        private CliRunner _runner = null!;

        [OneTimeSetUp]
        public void CreateTemporaryDirectory()
        {
            _temporaryDirectory = Path.Combine(
                Path.GetTempPath(),
                "Pacioli.CLI.Test",
                Guid.NewGuid().ToString("N"));

            Directory.CreateDirectory(_temporaryDirectory);
            _runner = new CliRunner(Settings.CliExecutable, Settings.TimeoutSeconds);
            _rules = InvoiceElementRules.Select(Settings.CheckedElements);

            TestContext.Progress.WriteLine($"CLI tool         : {Settings.CliExecutable} ({Settings.CliExecutableSource})");
            TestContext.Progress.WriteLine(
                $"Samples directory: {string.Join(" | ", Settings.SamplesDirectories)} ({Settings.SamplesDirectorySource})");
            TestContext.Progress.WriteLine($"Samples found    : {Samples.Count}");
            TestContext.Progress.WriteLine($"Checked elements : {string.Join(", ", _rules.Select(rule => rule.Name))}");
            TestContext.Progress.WriteLine($"Output directory : {_temporaryDirectory} (deleted at the end)");
        }

        [OneTimeTearDown]
        public void RemoveTemporaryDirectory()
        {
            WriteSummary();
            WriteReportFile();

            try
            {
                if (Directory.Exists(_temporaryDirectory))
                {
                    Directory.Delete(_temporaryDirectory, recursive: true);
                }

                /* Remove the parent directory too, but only if no other test run uses it. */
                string? parent = Path.GetDirectoryName(_temporaryDirectory);
                if (parent != null && Directory.Exists(parent) && !Directory.EnumerateFileSystemEntries(parent).Any())
                {
                    Directory.Delete(parent);
                }
            }
            catch (IOException error)
            {
                /* A locked file must not make the test run fail. */
                TestContext.Progress.WriteLine(
                    $"Warning: the temporary directory stays on the disk: {_temporaryDirectory} ({error.Message})");
            }
        }

        /* Reports a missing CLI tool with a clear message. */
        [Test, Order(1)]
        public void CliTool_IsAvailable()
        {
            Assert.That(Settings.CliExecutable, Is.Not.Empty,
                "The test did not find pacioli-cmd. Build the project Pacioli.CLI, " +
                "or set the path with the environment variable PACIOLI_CLI_EXE.");

            Assert.That(File.Exists(Settings.CliExecutable), Is.True,
                $"The CLI tool does not exist: {Settings.CliExecutable} (source: {Settings.CliExecutableSource})");
        }

        /* Reports a wrong samples directory with a clear message. */
        [Test, Order(1)]
        public void SamplesDirectory_ContainsSamples()
        {
            Assert.That(Settings.SamplesDirectories, Is.Not.Empty,
                "No samples directory is configured. Set samplesDirectories in testsettings.json, " +
                "or use the environment variable PACIOLI_SAMPLES_DIR.");

            var missing = Settings.SamplesDirectories.Where(entry => !Directory.Exists(entry)).ToList();

            Assert.That(missing, Is.Empty,
                $"A samples directory does not exist: \"{string.Join("\", \"", missing)}\" " +
                $"(source: {Settings.SamplesDirectorySource}). " +
                "Set the correct path in testsettings.json, or with the environment variable PACIOLI_SAMPLES_DIR.");

            Assert.That(Samples, Is.Not.Empty,
                $"The test found no sample document in \"{string.Join("\", \"", Settings.SamplesDirectories)}\" " +
                $"with the patterns {string.Join(", ", Settings.IncludePatterns)}.");
        }

        /*
         * Reports a name of the setting checkedElements that does not agree with a rule.
         * Without this test case, a name with a spelling error would remove its rule
         * without a message, and the content check could pass because it checks nothing.
         */
        [Test, Order(1)]
        public void CheckedElements_AreKnown()
        {
            var unknown = InvoiceElementRules.FindUnknownNames(Settings.CheckedElements);

            Assert.That(unknown, Is.Empty,
                $"The setting checkedElements holds a name without a rule: {string.Join(", ", unknown)}. " +
                $"These names exist: {string.Join(", ", InvoiceElementRules.All.Select(rule => rule.Name))}.");

            Assert.That(_rules, Is.Not.Empty, "The content check has no rule.");
        }

        [TestCaseSource(nameof(SampleCases)), Order(2)]
        public void Sample_RendersToPdf(SampleFile sample)
        {
            RenderedSample rendered = EnsureRendered(sample);

            if (rendered.Problem == null)
            {
                TestContext.Out.WriteLine(
                    $"OK: {sample.RelativePath} -> {new FileInfo(rendered.PdfPath).Length} bytes " +
                    $"in {rendered.Result.Duration.TotalMilliseconds:F0} ms");
                return;
            }

            var message = new StringBuilder();
            message.AppendLine($"The CLI tool could not render the sample: {sample.RelativePath}");
            message.AppendLine($"Problem     : {rendered.Problem}");
            message.AppendLine($"Command     : {rendered.Result.CommandLine}");
            message.AppendLine($"Exit code   : {rendered.Result.ExitCode}");
            message.AppendLine($"Duration    : {rendered.Result.Duration.TotalMilliseconds:F0} ms");

            if (rendered.Result.StandardOutput.Length > 0)
            {
                message.AppendLine($"Standard out:{Environment.NewLine}{rendered.Result.StandardOutput}");
            }
            if (rendered.Result.StandardError.Length > 0)
            {
                message.AppendLine($"Error out   :{Environment.NewLine}{rendered.Result.StandardError}");
            }

            lock (_lock)
            {
                _renderProblems.Add($"{sample.RelativePath}: {rendered.Problem}");
            }

            Assert.Fail(message.ToString());
        }

        [TestCaseSource(nameof(SampleCases)), Order(3)]
        public void Sample_RenderedPdf_ShowsKeyElements(SampleFile sample)
        {
            RenderedSample rendered = EnsureRendered(sample);

            if (rendered.Problem != null)
            {
                /* Without a good PDF file, no content check is possible.
                 * The test case Sample_RendersToPdf reports the cause. */
                Assert.Ignore($"The CLI tool could not render this sample: {rendered.Problem}");
            }

            /*
             * The control reads with the old library, and that library knows CII only.
             * A UBL document therefore has no second parser, and the case gives the
             * result "ignored". XmlSample_AllValuesAreVisible still checks each value of
             * such a document, because it works from the XML and not from a model.
             */
            CrossIndustryInvoice? descriptor = InvoiceLoader.Load(sample.FullPath);

            if (descriptor == null)
            {
                Assert.Ignore(
                    "The control library Vit.FeRD.ExtendedDocument reads CII only, thus it " +
                    "cannot give the expected values of this document.");
            }

            PdfTextContent page = PdfTextContent.Read(rendered.PdfPath);

            var results = _rules.SelectMany(rule => rule.Check(descriptor!, page)).ToList();
            var failures = results.Where(result => result.IsFailure).ToList();

            int checkedCount = results.Count(result => result.Outcome == ElementCheckOutcome.Found);
            int skippedCount = results.Count(result => result.Outcome == ElementCheckOutcome.NotApplicable);

            if (failures.Count == 0)
            {
                TestContext.Out.WriteLine(
                    $"OK: {sample.RelativePath} -> {checkedCount} elements found, " +
                    $"{skippedCount} elements without data, {page.PageCount} pages");
                return;
            }

            var message = new StringBuilder();
            message.AppendLine($"The rendered PDF does not show all key elements: {sample.RelativePath}");
            message.AppendLine($"Missing  : {failures.Count} of {failures.Count + checkedCount} elements");
            message.AppendLine($"Pages    : {page.PageCount}");
            message.AppendLine("Elements :");

            foreach (var failure in failures.Take(MaximumReportedFailures))
            {
                message.AppendLine($"  - {failure.Describe()}");
            }

            if (failures.Count > MaximumReportedFailures)
            {
                message.AppendLine($"  - ... and {failures.Count - MaximumReportedFailures} more");
            }

            lock (_lock)
            {
                _contentProblems.Add(
                    $"{sample.RelativePath}: {failures.Count} element(s) missing " +
                    $"({string.Join(", ", failures.Take(MaximumReportedFailures).Select(f => f.RuleName).Distinct())})");
            }

            Assert.Fail(message.ToString());
        }

        /*
         * Looks for each value of the XML input on the rendered PDF.
         *
         * This test case uses the XML samples only. A ZUGFeRD PDF sample holds the same
         * XML data as an attachment, thus it would give the same result a second time.
         *
         * The class XmlValueExclusions holds the values that the test does not look for.
         * It has two kinds of entry: a value that the renderer must not write in this
         * form, for example a code; and a value that the renderer does not show yet.
         */
        [TestCaseSource(nameof(XmlSampleCases)), Order(4)]
        public void XmlSample_AllValuesAreVisible(SampleFile sample)
        {
            RenderedSample rendered = EnsureRendered(sample);

            if (rendered.Problem != null)
            {
                Assert.Ignore($"The CLI tool could not render this sample: {rendered.Problem}");
            }

            PdfTextContent page = PdfTextContent.Read(rendered.PdfPath);
            var values = XmlValueExtractor.Extract(sample.FullPath);

            var missing = new List<XmlValue>();
            var notOnPage = new List<NotOnPageValue>();
            int found = 0;
            int notRendered = 0;
            int knownGap = 0;

            foreach (XmlValue value in values)
            {
                XmlValueExclusion? exclusion = XmlValueExclusions.Find(value);

                if (exclusion?.Kind == ExclusionKind.NotRendered)
                {
                    notRendered++;
                    notOnPage.Add(new NotOnPageValue
                    {
                        Value = value,
                        Kind = NotOnPageKind.NotRendered,
                        Pattern = exclusion.Pattern,
                        Reason = exclusion.Reason
                    });
                    continue;
                }

                /*
                 * A known gap also gets the test. The renderer can show a value in one
                 * sample and not show it in an other sample. Thus the count of the gaps
                 * stays correct, and a gap that the renderer closes goes away without
                 * a change of this test.
                 */
                if (XmlValueMatcher.IsVisible(value.Value, page))
                {
                    found++;
                    continue;
                }

                if (exclusion != null)
                {
                    knownGap++;
                    lock (_lock)
                    {
                        _knownGapHits[exclusion.Pattern] = _knownGapHits.GetValueOrDefault(exclusion.Pattern) + 1;
                    }
                    _report.AddKnownGap(sample.RelativePath, exclusion, value);
                    notOnPage.Add(new NotOnPageValue
                    {
                        Value = value,
                        Kind = NotOnPageKind.KnownGap,
                        Pattern = exclusion.Pattern,
                        Reason = exclusion.Reason
                    });
                    continue;
                }

                missing.Add(value);
                notOnPage.Add(new NotOnPageValue { Value = value, Kind = NotOnPageKind.Missing });
            }

            _report.AddSample(sample.RelativePath, found, notOnPage);

            if (missing.Count == 0)
            {
                TestContext.Out.WriteLine(
                    $"OK: {sample.RelativePath} -> {found} of {found + notRendered + knownGap} XML values found, " +
                    $"{notRendered} not rendered by design, {knownGap} known gaps");
                return;
            }

            var message = new StringBuilder();
            message.AppendLine($"The rendered PDF does not show all values of the XML input: {sample.RelativePath}");
            message.AppendLine($"Missing : {missing.Count} of {found + missing.Count} checked values");
            message.AppendLine("Values  :");

            foreach (XmlValue value in missing.Take(MaximumReportedFailures))
            {
                message.AppendLine($"  - line {value.LineNumber} {value.Path}");
                message.AppendLine($"    value: \"{Shorten(value.Value)}\"");
            }

            if (missing.Count > MaximumReportedFailures)
            {
                message.AppendLine($"  - ... and {missing.Count - MaximumReportedFailures} more");
            }

            message.AppendLine(
                "If the renderer must not show a value, add an entry to XmlValueExclusions.All.");

            lock (_lock)
            {
                _contentProblems.Add($"{sample.RelativePath}: {missing.Count} XML value(s) missing");
            }

            Assert.Fail(message.ToString());
        }

        private static string Shorten(string value)
        {
            string text = PdfTextContent.Normalize(value);
            return text.Length <= 80 ? text : text[..80] + "...";
        }

        private const int MaximumReportedFailures = 10;

        /* Renders one sample. The result stays in the memory, so that the CLI runs one time only. */
        private RenderedSample EnsureRendered(SampleFile sample)
        {
            lock (_lock)
            {
                if (_renders.TryGetValue(sample.Index, out RenderedSample? existing))
                {
                    return existing;
                }

                string outputFolder = Path.Combine(_temporaryDirectory, $"case_{sample.Index:D4}");
                Directory.CreateDirectory(outputFolder);

                string pdfName = Path.GetFileNameWithoutExtension(sample.FullPath) + ".pdf";
                string pdfPath = Path.Combine(outputFolder, pdfName);

                CliResult result = _runner.Render(sample.FullPath, outputFolder, pdfName);

                var rendered = new RenderedSample
                {
                    PdfPath = pdfPath,
                    Result = result,
                    Problem = DescribeProblem(result, pdfPath)
                };

                _renders[sample.Index] = rendered;
                return rendered;
            }
        }

        /* Gives the reason of the failure, or null if the render is good. */
        private static string? DescribeProblem(CliResult result, string pdfPath)
        {
            if (result.TimedOut)
            {
                return $"The CLI tool did not stop in {Settings.TimeoutSeconds} seconds. The test stopped it.";
            }

            if (result.ExitCode != 0)
            {
                return $"The CLI tool stopped with the exit code {result.ExitCode}.";
            }

            if (result.StandardError.Length > 0)
            {
                return "The CLI tool wrote a message to the error stream.";
            }

            return PdfCheck.FindProblem(pdfPath);
        }

        /* Makes one test case for each sample. The name of the test case is the relative path. */
        public static IEnumerable<TestCaseData> SampleCases()
        {
            foreach (SampleFile sample in Samples)
            {
                yield return new TestCaseData(sample).SetArgDisplayNames(sample.RelativePath);
            }
        }

        /* Makes one test case for each XML sample. */
        public static IEnumerable<TestCaseData> XmlSampleCases()
        {
            foreach (SampleFile sample in Samples.Where(IsXml))
            {
                yield return new TestCaseData(sample).SetArgDisplayNames(sample.RelativePath);
            }
        }

        private static bool IsXml(SampleFile sample)
        {
            return Path.GetExtension(sample.FullPath).Equals(".xml", StringComparison.OrdinalIgnoreCase);
        }

        private void WriteSummary()
        {
            TestContext.Progress.WriteLine(string.Empty);
            TestContext.Progress.WriteLine("Summary:");
            TestContext.Progress.WriteLine($"  Samples         : {Samples.Count}");
            TestContext.Progress.WriteLine($"  Render problems : {_renderProblems.Count}");

            foreach (string problem in _renderProblems)
            {
                TestContext.Progress.WriteLine($"    - {problem}");
            }

            TestContext.Progress.WriteLine($"  Content problems: {_contentProblems.Count}");

            foreach (string problem in _contentProblems)
            {
                TestContext.Progress.WriteLine($"    - {problem}");
            }

            WriteKnownGaps();
        }

        /*
         * Writes the result of XmlSample_AllValuesAreVisible to a file for a review.
         *
         * If no XML sample ran, the method writes no file. A run with a filter must not
         * replace a complete report with an empty one.
         */
        private void WriteReportFile()
        {
            if (_report.SampleCount == 0)
            {
                TestContext.Progress.WriteLine(
                    "  No XML sample ran. The test keeps the report file as it is.");
                return;
            }

            _report.SamplesDirectory = string.Join(" | ", Settings.SamplesDirectories);
            _report.CliExecutable = Settings.CliExecutable;

            try
            {
                _report.WriteTo(Settings.ReportFile);
                TestContext.Progress.WriteLine($"  Report: {Settings.ReportFile}");
            }
            catch (Exception error) when (error is IOException or UnauthorizedAccessException)
            {
                /* A report that the test cannot write must not make the test run fail. */
                TestContext.Progress.WriteLine(
                    $"  Warning: the test could not write the report to {Settings.ReportFile} ({error.Message})");
            }
        }

        /*
         * Shows how many values of the XML input the renderer does not show.
         * Each line is a task for the renderer. If the renderer starts to show a value,
         * remove the entry from XmlValueExclusions.All.
         */
        private void WriteKnownGaps()
        {
            if (_knownGapHits.Count == 0)
            {
                return;
            }

            TestContext.Progress.WriteLine(
                $"  Known gaps of the renderer: {_knownGapHits.Values.Sum()} value(s) " +
                $"in {_knownGapHits.Count} element(s)");

            foreach (var entry in _knownGapHits.OrderByDescending(item => item.Value))
            {
                string reason = XmlValueExclusions.All
                    .First(exclusion => exclusion.Pattern == entry.Key).Reason;
                TestContext.Progress.WriteLine($"    {entry.Value,5}  {entry.Key}  ({reason})");
            }

            /* An entry without a hit is not necessary. You can remove it. */
            var unused = XmlValueExclusions.All
                .Where(exclusion => exclusion.Kind == ExclusionKind.KnownGap)
                .Select(exclusion => exclusion.Pattern)
                .Where(pattern => !_knownGapHits.ContainsKey(pattern))
                .ToList();

            if (unused.Count > 0)
            {
                TestContext.Progress.WriteLine(
                    $"  Entries of kind KnownGap without a hit ({unused.Count}). You can remove them:");

                foreach (string pattern in unused)
                {
                    TestContext.Progress.WriteLine($"           {pattern}");
                }
            }
        }

        /* Holds the rendered PDF of one sample. */
        private sealed class RenderedSample
        {
            public required string PdfPath { get; init; }
            public required CliResult Result { get; init; }

            /* The reason of the failure, or null if the render is good. */
            public required string? Problem { get; init; }
        }
    }
}
