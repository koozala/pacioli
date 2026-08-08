# Pacioli.CLI.NET8.Test

This project renders each sample document with the CLI tool `pacioli-cmd`. It reports
each document that the CLI tool cannot render, and each document that misses a key
element on the rendered page.

## What the test does

1. It finds the CLI tool `pacioli-cmd`.
2. It finds the sample documents in the samples directory.
3. It makes two test cases for each sample document.
4. For each sample, it starts the CLI tool with these arguments:

   ```
   pacioli-cmd -in <sample> -genpdf -output-folder <temporary folder> -pdfname <name>.pdf
   ```

   The CLI tool runs one time for each sample. The two test cases use the same PDF file.

5. It deletes the rendered PDF files at the end of the run.

## The two test cases

### Sample_RendersToPdf

The CLI tool must make a good PDF file. The test case fails if one of these conditions
is true:

- The CLI tool stops with an exit code that is not 0.
- The CLI tool writes a message to the error stream.
- The CLI tool does not stop before the timeout.
- The CLI tool writes no PDF file.
- The PDF file is empty, or it does not start with `%PDF-`, or it has no `%%EOF` mark
  at its end.

The failure message shows the sample, the problem, the full command line, the exit code,
and the output of the CLI tool. You can copy the command line to repeat the call by hand.

### Sample_RenderedPdf_ShowsKeyElements

The key elements of the invoice must be visible on the rendered PDF. The test reads the
invoice data from the sample, and it reads the text from the rendered PDF. Then it looks
for each element.

These elements are the default set:

| Element            | Source                                    |
|--------------------|-------------------------------------------|
| Invoice number     | `InvoiceNo`                               |
| Invoice date       | `InvoiceDate`                             |
| Seller name        | `Seller.Name`                             |
| Buyer name         | `Buyer.Name`                              |
| Currency           | `Currency`                                |
| Tax basis amount   | `TaxBasisAmount`                          |
| Tax total amount   | `TaxTotalAmount`                          |
| Grand total amount | `GrandTotalAmount`                        |
| Payable amount     | `DuePayableAmount`                        |
| Line item name     | `Name` of each trade line item            |

If the invoice has no data for an element, the test does not check that element, and the
test case does not fail. Thus the same rules apply to a profile with few data fields, for
example MINIMUM, and to a profile with many data fields, for example EXTENDED.

If the CLI tool cannot render the sample, this test case gives the result "skipped". The
test case `Sample_RendersToPdf` reports the cause.

The failure message shows each element that is not on the page, with the value and with
the accepted written forms.

## How to check more elements

Add one entry to the list `All` in `Content\InvoiceElementRules.cs`. Use one of these
factory methods:

| Method                      | Function                                                     |
|-----------------------------|--------------------------------------------------------------|
| `InvoiceElementRule.Text`   | A text value.                                                |
| `InvoiceElementRule.Date`   | A date. It accepts the forms of more than one culture.       |
| `InvoiceElementRule.Amount` | An amount. It uses the format of the renderer.               |
| `InvoiceElementRule.Custom` | A value with a special written form.                         |

Example: to check the order number, add this entry:

```csharp
InvoiceElementRule.Text("Order number",
    descriptor => new[] { descriptor.OrderNo }),
```

A rule can give more than one value. The rule then looks for each value. The rule
"Line item name" is an example. A rule that gives no value is not applied.

A rule must only expect data that the renderer writes to the page. See `InvoiceWriter.Write`
in the project `Pacioli.Pdf`.

## How the test reads the text of a PDF

The test reads the text two times, because one order is not sufficient:

- **Visual order.** The text comes in the order of the page. This order is correct for a
  paragraph. But in a table, it mixes the columns: if the text of a cell goes to a second
  line, the text of the other columns comes between the two lines of that cell.
- **Content order.** The text comes in the order of the PDF content. The renderer writes
  each table cell as one block. Thus the text of a cell stays together.

A value is visible if one of the two orders holds it.

The test then applies two steps to the text and to the value:

1. It replaces each group of space characters with one space. The renderer breaks a long
   value into more than one line. This step removes the line breaks.
2. If step 1 gives no result, it also removes each space that comes directly after a
   hyphen or a slash. The renderer can break `Brechsand-Splitt-Gemisch` into
   `Brechsand-Splitt-` and `Gemisch`. The text is complete, but it now holds a space that
   the value does not hold.

**Note:** the test reads the invoice data with the same library as the renderer. Thus the
content check tests the render step. It does not test the parse step.

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
| `checkedElements`   | `[]` (all elements)                                     | The elements of the content check, by name.               |

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
| `checkedElements`    | `CheckedElements`     | `PACIOLI_CHECKED_ELEMENTS`      |

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

Check two elements only:

```
$env:PACIOLI_CHECKED_ELEMENTS = "Invoice number;Grand total amount"
dotnet test src\Pacioli.CLI.NET8.Test\Pacioli.CLI.NET8.Test.csproj
```

Run the render test only, or the content test only:

```
dotnet test src\Pacioli.CLI.NET8.Test\Pacioli.CLI.NET8.Test.csproj --filter "Name~Sample_RendersToPdf"
dotnet test src\Pacioli.CLI.NET8.Test\Pacioli.CLI.NET8.Test.csproj --filter "Name~ShowsKeyElements"
```
