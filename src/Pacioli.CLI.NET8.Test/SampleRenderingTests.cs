using System.Text;

namespace Pacioli.CLI.NET8.Test
{
    /*
     * Renders each sample document with the CLI tool pacioli-cmd.
     *
     * One test case exists for each sample. A test case fails if:
     *   - the CLI tool stops with an error code,
     *   - the CLI tool writes to the error stream,
     *   - the CLI tool does not stop before the timeout,
     *   - the CLI tool writes no PDF file, or an incomplete PDF file.
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
        private static readonly List<string> Problems = new List<string>();

        private string _temporaryDirectory = string.Empty;
        private CliRunner _runner = null!;

        [OneTimeSetUp]
        public void CreateTemporaryDirectory()
        {
            _temporaryDirectory = Path.Combine(
                Path.GetTempPath(),
                "Pacioli.CLI.NET8.Test",
                Guid.NewGuid().ToString("N"));

            Directory.CreateDirectory(_temporaryDirectory);
            _runner = new CliRunner(Settings.CliExecutable, Settings.TimeoutSeconds);

            TestContext.Progress.WriteLine($"CLI tool         : {Settings.CliExecutable} ({Settings.CliExecutableSource})");
            TestContext.Progress.WriteLine($"Samples directory: {Settings.SamplesDirectory} ({Settings.SamplesDirectorySource})");
            TestContext.Progress.WriteLine($"Samples found    : {Samples.Count}");
            TestContext.Progress.WriteLine($"Output directory : {_temporaryDirectory} (deleted at the end)");
        }

        [OneTimeTearDown]
        public void RemoveTemporaryDirectory()
        {
            WriteSummary();

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
                "The test did not find pacioli-cmd. Build the project Pacioli.CLI.NET8, " +
                "or set the path with the environment variable PACIOLI_CLI_EXE.");

            Assert.That(File.Exists(Settings.CliExecutable), Is.True,
                $"The CLI tool does not exist: {Settings.CliExecutable} (source: {Settings.CliExecutableSource})");
        }

        /* Reports a wrong samples directory with a clear message. */
        [Test, Order(1)]
        public void SamplesDirectory_ContainsSamples()
        {
            Assert.That(Directory.Exists(Settings.SamplesDirectory), Is.True,
                $"The samples directory does not exist: \"{Settings.SamplesDirectory}\" " +
                $"(source: {Settings.SamplesDirectorySource}). " +
                "Set the correct path in testsettings.json, or with the environment variable PACIOLI_SAMPLES_DIR.");

            Assert.That(Samples, Is.Not.Empty,
                $"The test found no sample document in \"{Settings.SamplesDirectory}\" " +
                $"with the patterns {string.Join(", ", Settings.IncludePatterns)}.");
        }

        [TestCaseSource(nameof(SampleCases)), Order(2)]
        public void Sample_RendersToPdf(SampleFile sample)
        {
            string outputFolder = Path.Combine(_temporaryDirectory, $"case_{sample.Index:D4}");
            Directory.CreateDirectory(outputFolder);

            string pdfName = Path.GetFileNameWithoutExtension(sample.FullPath) + ".pdf";
            string pdfPath = Path.Combine(outputFolder, pdfName);

            CliResult result = _runner.Render(sample.FullPath, outputFolder, pdfName);

            string? problem = DescribeProblem(result, pdfPath);

            if (problem == null)
            {
                TestContext.Out.WriteLine(
                    $"OK: {sample.RelativePath} -> {new FileInfo(pdfPath).Length} bytes " +
                    $"in {result.Duration.TotalMilliseconds:F0} ms");
                return;
            }

            var message = new StringBuilder();
            message.AppendLine($"The CLI tool could not render the sample: {sample.RelativePath}");
            message.AppendLine($"Problem     : {problem}");
            message.AppendLine($"Command     : {result.CommandLine}");
            message.AppendLine($"Exit code   : {result.ExitCode}");
            message.AppendLine($"Duration    : {result.Duration.TotalMilliseconds:F0} ms");

            if (result.StandardOutput.Length > 0)
            {
                message.AppendLine($"Standard out:{Environment.NewLine}{result.StandardOutput}");
            }
            if (result.StandardError.Length > 0)
            {
                message.AppendLine($"Error out   :{Environment.NewLine}{result.StandardError}");
            }

            lock (Problems)
            {
                Problems.Add($"{sample.RelativePath}: {problem}");
            }

            Assert.Fail(message.ToString());
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

        private static void WriteSummary()
        {
            TestContext.Progress.WriteLine(string.Empty);
            TestContext.Progress.WriteLine("Summary of the sample rendering:");
            TestContext.Progress.WriteLine($"  Samples : {Samples.Count}");
            TestContext.Progress.WriteLine($"  Problems: {Problems.Count}");

            foreach (string problem in Problems)
            {
                TestContext.Progress.WriteLine($"  - {problem}");
            }
        }
    }
}
