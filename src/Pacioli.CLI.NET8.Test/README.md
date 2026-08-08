# Pacioli.CLI.NET8.Test

This project renders each sample document with the CLI tool `pacioli-cmd`. It reports
each document that the CLI tool cannot render.

## What the test does

1. It finds the CLI tool `pacioli-cmd`.
2. It finds the sample documents in the samples directory.
3. It makes one test case for each sample document.
4. For each sample, it starts the CLI tool with these arguments:

   ```
   pacioli-cmd -in <sample> -genpdf -output-folder <temporary folder> -pdfname <name>.pdf
   ```

5. It deletes the rendered PDF files at the end of the run.

## When a test case fails

A test case fails if one of these conditions is true:

- The CLI tool stops with an exit code that is not 0.
- The CLI tool writes a message to the error stream.
- The CLI tool does not stop before the timeout.
- The CLI tool writes no PDF file.
- The PDF file is empty, or it does not start with `%PDF-`, or it has no `%%EOF` mark
  at its end.

The failure message shows the sample, the problem, the full command line, the exit code,
and the output of the CLI tool. You can copy the command line to repeat the call by hand.

## Output files

The PDF files go to a new directory below the temporary directory of the system:

```
%TEMP%\Pacioli.CLI.NET8.Test\<identifier>\case_<number>\
```

The test deletes this directory at the end of the run. No rendered file stays on the disk.

## Configuration

The file `testsettings.json` holds the default configuration. The build copies this file
to the output directory.

| Name                | Default                                                 | Function                                                  |
|---------------------|---------------------------------------------------------|-----------------------------------------------------------|
| `samplesDirectory`  | the path in `test/info.md`                              | The directory that holds the sample documents.            |
| `cliExecutable`     | empty (the test searches for the file)                  | The path of `pacioli-cmd.exe`.                            |
| `includePatterns`   | `[ "*.xml", "*.pdf" ]`                                  | The file patterns of the samples.                         |
| `excludeDirectories`| `[ "attachments" ]`                                     | Directory names to ignore.                                |
| `timeoutSeconds`    | `120`                                                   | The maximum time for one call of the CLI tool.            |

The directory `attachments` holds enclosures of an invoice. These files are not invoices.
Thus the default configuration ignores them.

If `cliExecutable` is empty, the test searches for the file. It goes up from its own output
directory to the directory of `Pacioli.sln`. Then it looks in `Pacioli.CLI.NET8\bin`. It
takes the newest executable. The build configuration of the test has priority.

### Overrides

You can override each value. The order of precedence is:

1. NUnit test parameter
2. Environment variable
3. `testsettings.json`

| Setting              | Test parameter        | Environment variable            |
|----------------------|-----------------------|---------------------------------|
| `samplesDirectory`   | `SamplesDirectory`    | `PACIOLI_SAMPLES_DIR`           |
| `cliExecutable`      | `CliExecutable`       | `PACIOLI_CLI_EXE`               |
| `includePatterns`    | `IncludePatterns`     | `PACIOLI_SAMPLE_PATTERNS`       |
| `excludeDirectories` | `ExcludeDirectories`  | `PACIOLI_SAMPLE_EXCLUDE_DIRS`   |
| `timeoutSeconds`     | `TimeoutSeconds`      | `PACIOLI_CLI_TIMEOUT`           |

For a list, a semicolon separates the entries.

## How to run the test

Run all sample renderings:

```
dotnet test src\Pacioli.CLI.NET8.Test\Pacioli.CLI.NET8.Test.csproj
```

Use a different samples directory:

```
dotnet test src\Pacioli.CLI.NET8.Test\Pacioli.CLI.NET8.Test.csproj -- TestRunParameters.Parameter(n="SamplesDirectory",value="D:\my samples")
```

Or with an environment variable (PowerShell):

```
$env:PACIOLI_SAMPLES_DIR = "D:\my samples"
dotnet test src\Pacioli.CLI.NET8.Test\Pacioli.CLI.NET8.Test.csproj
```
