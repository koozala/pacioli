using System.Text.Json;
using System.Text.Json.Serialization;

namespace Pacioli.CLI.Test
{
    /*
     * Holds the configuration of the sample rendering test.
     *
     * The values come from three sources. The order of precedence is:
     *   1. NUnit test parameters, for example:
     *      dotnet test -- TestRunParameters.Parameter(n="SamplesDirectory",value="D:\samples")
     *   2. Environment variables, for example PACIOLI_SAMPLES_DIR
     *   3. The file testsettings.json in the output directory
     */
    public sealed class TestSettings
    {
        public const string SettingsFileName = "testsettings.json";

        public string SamplesDirectory { get; }
        public string CliExecutable { get; }
        public IReadOnlyList<string> IncludePatterns { get; }
        public IReadOnlyList<string> ExcludeDirectories { get; }
        public int TimeoutSeconds { get; }

        /* The names of the elements of the content check. An empty list checks all elements. */
        public IReadOnlyList<string> CheckedElements { get; }

        /* The file for the report of the XML value coverage. */
        public string ReportFile { get; }
        public string ReportFileSource { get; }

        /* Tells where the samples directory value comes from. The test report shows this text. */
        public string SamplesDirectorySource { get; }
        public string CliExecutableSource { get; }

        private TestSettings(SettingsFile file)
        {
            (SamplesDirectory, SamplesDirectorySource) = ReadValue(
                "SamplesDirectory", "PACIOLI_SAMPLES_DIR", file.SamplesDirectory);

            (string cli, string cliSource) = ReadValue(
                "CliExecutable", "PACIOLI_CLI_EXE", file.CliExecutable);

            if (string.IsNullOrWhiteSpace(cli))
            {
                CliExecutable = CliLocator.Find() ?? string.Empty;
                CliExecutableSource = "automatic search";
            }
            else
            {
                CliExecutable = Path.GetFullPath(cli);
                CliExecutableSource = cliSource;
            }

            IncludePatterns = ReadList("IncludePatterns", "PACIOLI_SAMPLE_PATTERNS", file.IncludePatterns,
                new[] { "*.xml", "*.pdf" });

            ExcludeDirectories = ReadList("ExcludeDirectories", "PACIOLI_SAMPLE_EXCLUDE_DIRS", file.ExcludeDirectories,
                new[] { "attachments" });

            CheckedElements = ReadList("CheckedElements", "PACIOLI_CHECKED_ELEMENTS", file.CheckedElements,
                Array.Empty<string>());

            (string report, string reportSource) = ReadValue("ReportFile", "PACIOLI_REPORT_FILE", file.ReportFile);

            if (string.IsNullOrWhiteSpace(report))
            {
                ReportFile = DefaultReportFile();
                ReportFileSource = "default value";
            }
            else
            {
                ReportFile = Path.GetFullPath(report);
                ReportFileSource = reportSource;
            }

            (string timeout, _) = ReadValue("TimeoutSeconds", "PACIOLI_CLI_TIMEOUT", file.TimeoutSeconds?.ToString());
            TimeoutSeconds = int.TryParse(timeout, out int seconds) && seconds > 0 ? seconds : 120;
        }

        /* Reads testsettings.json from the output directory and applies the overrides. */
        public static TestSettings Load()
        {
            string path = Path.Combine(AppContext.BaseDirectory, SettingsFileName);
            SettingsFile file = new SettingsFile();

            if (File.Exists(path))
            {
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                file = JsonSerializer.Deserialize<SettingsFile>(File.ReadAllText(path), options) ?? new SettingsFile();
            }

            return new TestSettings(file);
        }

        /*
         * The report goes to the folder TestResults beside the solution file. There a
         * reader finds it without a search. If the solution is not there, the report goes
         * to the output folder of the test.
         */
        private static string DefaultReportFile()
        {
            string? solution = CliLocator.FindSolutionDirectory(AppContext.BaseDirectory);
            string folder = solution == null
                ? AppContext.BaseDirectory
                : Path.Combine(solution, "TestResults");

            return Path.Combine(folder, "xml-value-report.md");
        }

        private static (string value, string source) ReadValue(string parameterName, string environmentName, string? fileValue)
        {
            string? fromParameter = TestContext.Parameters.Get(parameterName);
            if (!string.IsNullOrWhiteSpace(fromParameter))
            {
                return (fromParameter!.Trim(), $"test parameter {parameterName}");
            }

            string? fromEnvironment = Environment.GetEnvironmentVariable(environmentName);
            if (!string.IsNullOrWhiteSpace(fromEnvironment))
            {
                return (fromEnvironment!.Trim(), $"environment variable {environmentName}");
            }

            if (!string.IsNullOrWhiteSpace(fileValue))
            {
                return (fileValue!.Trim(), SettingsFileName);
            }

            return (string.Empty, "default value");
        }

        /* Reads a list value. In a test parameter or in an environment variable, a semicolon separates the entries. */
        private static IReadOnlyList<string> ReadList(
            string parameterName, string environmentName, string[]? fileValue, string[] fallback)
        {
            (string value, string source) = ReadValue(parameterName, environmentName, null);

            if (!string.IsNullOrWhiteSpace(value))
            {
                var entries = value
                    .Split(';', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                    .ToArray();
                if (entries.Length > 0)
                {
                    return entries;
                }
            }

            if (fileValue != null && fileValue.Length > 0)
            {
                return fileValue;
            }

            return fallback;
        }

        private sealed class SettingsFile
        {
            [JsonPropertyName("samplesDirectory")]
            public string? SamplesDirectory { get; set; }

            [JsonPropertyName("cliExecutable")]
            public string? CliExecutable { get; set; }

            [JsonPropertyName("includePatterns")]
            public string[]? IncludePatterns { get; set; }

            [JsonPropertyName("excludeDirectories")]
            public string[]? ExcludeDirectories { get; set; }

            [JsonPropertyName("timeoutSeconds")]
            public int? TimeoutSeconds { get; set; }

            [JsonPropertyName("checkedElements")]
            public string[]? CheckedElements { get; set; }

            [JsonPropertyName("reportFile")]
            public string? ReportFile { get; set; }
        }
    }
}
