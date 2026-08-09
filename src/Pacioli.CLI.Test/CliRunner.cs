using System.Diagnostics;
using System.Text;

namespace Pacioli.CLI.Test
{
    /* Holds the result of one call of the CLI tool. */
    public sealed class CliResult
    {
        public required string CommandLine { get; init; }
        public required int ExitCode { get; init; }
        public required string StandardOutput { get; init; }
        public required string StandardError { get; init; }
        public required bool TimedOut { get; init; }
        public required TimeSpan Duration { get; init; }
    }

    /* Starts the CLI tool and collects its output. */
    public sealed class CliRunner
    {
        private readonly string _executable;
        private readonly int _timeoutSeconds;

        public CliRunner(string executable, int timeoutSeconds)
        {
            _executable = executable;
            _timeoutSeconds = timeoutSeconds;
        }

        /*
         * Renders one input document to a PDF file.
         *
         * inputFile     The sample document. It is an XML file or a ZUGFeRD PDF file.
         * outputFolder  The directory for the PDF file. The caller must make this directory.
         * pdfName       The name of the PDF file.
         */
        public CliResult Render(string inputFile, string outputFolder, string pdfName)
        {
            var arguments = new[]
            {
                "-in", inputFile,
                "-genpdf",
                "-output-folder", outputFolder,
                "-pdfname", pdfName
            };

            return Run(arguments);
        }

        public CliResult Run(IReadOnlyList<string> arguments)
        {
            var startInfo = new ProcessStartInfo
            {
                FileName = _executable,
                WorkingDirectory = Path.GetDirectoryName(_executable) ?? AppContext.BaseDirectory,
                UseShellExecute = false,
                CreateNoWindow = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                StandardOutputEncoding = Encoding.UTF8,
                StandardErrorEncoding = Encoding.UTF8
            };

            foreach (string argument in arguments)
            {
                startInfo.ArgumentList.Add(argument);
            }

            var standardOutput = new StringBuilder();
            var standardError = new StringBuilder();
            var stopwatch = Stopwatch.StartNew();

            using var process = new Process { StartInfo = startInfo };

            /* Read both streams while the process runs. If you do not do this, a full pipe stops the process. */
            process.OutputDataReceived += (_, e) => { if (e.Data != null) standardOutput.AppendLine(e.Data); };
            process.ErrorDataReceived += (_, e) => { if (e.Data != null) standardError.AppendLine(e.Data); };

            process.Start();
            process.BeginOutputReadLine();
            process.BeginErrorReadLine();

            bool completed = process.WaitForExit(_timeoutSeconds * 1000);
            int exitCode;

            if (completed)
            {
                /* This overload waits for the end of the two output streams. */
                process.WaitForExit();
                exitCode = process.ExitCode;
            }
            else
            {
                try
                {
                    process.Kill(entireProcessTree: true);
                }
                catch (InvalidOperationException)
                {
                    /* The process stopped between the timeout and the kill. */
                }
                exitCode = -1;
            }

            stopwatch.Stop();

            return new CliResult
            {
                CommandLine = BuildCommandLine(_executable, arguments),
                ExitCode = exitCode,
                StandardOutput = standardOutput.ToString().TrimEnd(),
                StandardError = standardError.ToString().TrimEnd(),
                TimedOut = !completed,
                Duration = stopwatch.Elapsed
            };
        }

        /* Makes a readable command line. The test report shows it, so that you can repeat the call. */
        public static string BuildCommandLine(string executable, IReadOnlyList<string> arguments)
        {
            var parts = new List<string> { Quote(executable) };
            parts.AddRange(arguments.Select(Quote));
            return string.Join(" ", parts);
        }

        private static string Quote(string value)
        {
            return value.Contains(' ') ? $"\"{value}\"" : value;
        }
    }
}
