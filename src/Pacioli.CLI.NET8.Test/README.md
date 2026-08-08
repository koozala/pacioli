# Pacioli.CLI.NET8.Test

This project renders each sample document with the CLI tool `pacioli-cmd`. It reports
each document that the CLI tool cannot render, and each document that misses a key
element on the rendered page.

## What the test does

1. It finds the CLI tool `pacioli-cmd`.
2. It finds the sample documents in the samples directory.
3. It makes two test cases for each sample document, and a third test case for each XML
   sample.
4. For each sample, it starts the CLI tool with these arguments:

   ```
   pacioli-cmd -in <sample> -genpdf -output-folder <temporary folder> -pdfname <name>.pdf
   ```

   The CLI tool runs one time for each sample. The two test cases use the same PDF file.

5. It writes the result of the XML value check to a report file.
6. It deletes the rendered PDF files at the end of the run.

## The test cases

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

These elements are the default set.

Data of the document:

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

Data of each position. The renderer writes these values to the item table:

| Element                      | Source                        |
|------------------------------|-------------------------------|
| Line item name               | `Name`                        |
| Line item seller assigned ID | `SellerAssignedID`            |
| Line item quantity           | `BilledQuantity`              |
| Line item tax rate           | `TaxPercent`                  |
| Line item net unit price     | `NetUnitPrice`                |
| Line item gross unit price   | `GrossUnitPrice`              |
| Line item total amount       | `LineTotalAmount`             |

A position rule gives one value for each position of the invoice. It adds the position
number to the label of each value. Thus the report tells you which position has the
problem, for example:

```
  - Line item total amount: "position 2: 1.150,00" (accepted forms: "1.150,00" | "1,150.00")
```

If the invoice has no data for an element, the test does not check that element, and the
test case does not fail. Thus the same rules apply to a profile with few data fields, for
example MINIMUM, and to a profile with many data fields, for example EXTENDED. The number
of checked elements goes from 9, for a MINIMUM sample, to 171, for a large EXTENDED sample.

If the CLI tool cannot render the sample, this test case gives the result "skipped". The
test case `Sample_RendersToPdf` reports the cause.

The failure message shows each element that is not on the page, with the value and with
the accepted written forms.

### XmlSample_AllValuesAreVisible

This test case uses the **XML samples only**. A ZUGFeRD PDF sample holds the same XML data
as an attachment, thus it would give the same result a second time.

The test reads each value of the XML input: the text of each element that has no child
element, and the value of each attribute. Then it looks for each value on the rendered PDF.
The two other test cases check a fixed set of elements; this test case checks everything.

The renderer does not always write a value in the form of the XML. Thus the test makes the
forms that the renderer can use:

| In the XML | On the page  | Rule                                          |
|------------|--------------|-----------------------------------------------|
| `20200305` | `05.03.2020` | 8 digits are a date. It accepts more cultures. |
| `198.00`   | `198,00`     | A number gets the amount format `#,0.00`.     |
| `20.0000`  | `20`         | A number also gets the quantity forms.        |

The class `Content\XmlValueExclusions.cs` holds the values that the test does **not** look
for. Each entry has a pattern, a kind, and a reason. Keep the two kinds apart, because they
have a different meaning:

| Kind          | Meaning                                                                     |
|---------------|-----------------------------------------------------------------------------|
| `NotRendered` | The renderer must not write this value in this form. A code is an example: the XML holds `H87`, and the page shows `Anz.`. These entries stay. |
| `KnownGap`    | The renderer does not show this value, but a reader of the invoice needs it. Each entry is a task for the renderer. |

A pattern has four forms:

| Pattern  | Meaning                                        |
|----------|------------------------------------------------|
| `@name`  | The attribute with this name.                  |
| `/a/b/`  | Each value whose path holds this part.         |
| `/a/b`   | Each value whose path ends with this part.     |
| `name`   | The element with this name.                    |

A test case fails if a value is not on the page and no entry applies to it. The message
shows the line number in the XML, the path, and the value.

**A known gap also gets the test.** The renderer can show a value in one sample and not
show it in an other sample. Thus the count of the gaps stays correct. If the renderer
starts to show a value, the gap goes away without a change of this test, and the summary
reports the entry as one that you can remove.

To make the check stronger, remove an entry from `XmlValueExclusions.All` and start the
test. The report then tells you which samples hold the value.

## The report file

At the end of the run, the test writes the result of `XmlSample_AllValuesAreVisible` to a
file, for a review. The default position is beside the solution file:

```
src\TestResults\xml-value-report.md
```

The file is in Markdown form. It has these parts:

| Part | Content |
|---|---|
| Summary | How many values are on the page, how many the renderer does not write by design, how many are a known gap, and how many make the test fail. |
| Values that are missing, and that have no entry | The failures, with the line number in the XML, the path, and the value. |
| Known gaps of the renderer | Each gap, with the number of values, the number of samples, and the reason. Then examples with the sample and the line number. |
| Entries of kind KnownGap without a hit | Entries that you can remove. |
| Each sample | One row for each sample, with its counts. |

The test also writes a short form of the gaps to the console.

**A run with a filter writes no file.** If no XML sample runs, the test keeps the file as
it is. Thus a run of one test case does not replace a complete report with an empty one.

The folder `TestResults` is in `.gitignore`. Thus the report does not go into the
repository.

To put the report in an other place, use the setting `reportFile`, the test parameter
`ReportFile`, or the environment variable `PACIOLI_REPORT_FILE`:

```
$env:PACIOLI_REPORT_FILE = "D:\reports\pacioli.md"
dotnet test src\Pacioli.CLI.NET8.Test\Pacioli.CLI.NET8.Test.csproj
```

## How to check more elements

Add one entry to the list `All` in `Content\InvoiceElementRules.cs`. Use one of these
factory methods:

For a value of the document:

| Method                      | Function                                                     |
|-----------------------------|--------------------------------------------------------------|
| `InvoiceElementRule.Text`   | A text value.                                                |
| `InvoiceElementRule.Date`   | A date. It accepts the forms of more than one culture.       |
| `InvoiceElementRule.Amount` | An amount. It uses the format of the renderer.               |
| `InvoiceElementRule.Custom` | A value with a special written form.                         |

For a value of each position:

| Method                                | Function                                           |
|---------------------------------------|----------------------------------------------------|
| `InvoiceElementRule.LineItemText`     | A text value.                                      |
| `InvoiceElementRule.LineItemAmount`   | An amount, in the format `#,0.00`.                 |
| `InvoiceElementRule.LineItemQuantity` | A quantity. The renderer selects the decimal places. |
| `InvoiceElementRule.LineItemPercent`  | A tax rate, with a percent sign.                   |
| `InvoiceElementRule.LineItem`         | A value with a special written form.               |

Example: to check the order number, add this entry:

```csharp
InvoiceElementRule.Text("Order number",
    descriptor => new[] { descriptor.OrderNo }),
```

Example: to check the description of each position, add this entry:

```csharp
InvoiceElementRule.LineItemText("Line item description",
    item => item.Description),
```

A rule can give more than one value. The rule then looks for each value. Each position
rule is an example. A rule that gives no value is not applied.

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
| `reportFile`        | `TestResults\xml-value-report.md` beside the solution   | The file for the report of the XML value coverage.        |

A name in `checkedElements` must agree with a rule. If it does not, the test case
`CheckedElements_AreKnown` fails and shows the names that exist. Without this test case,
a name with a spelling error would remove its rule without a message, and the content
check could pass because it checks nothing.

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
| `reportFile`         | `ReportFile`          | `PACIOLI_REPORT_FILE`           |

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
dotnet test src\Pacioli.CLI.NET8.Test\Pacioli.CLI.NET8.Test.csproj --filter "Name~AllValuesAreVisible"
```

Show the summary of the gaps of the renderer:

```
dotnet test src\Pacioli.CLI.NET8.Test\Pacioli.CLI.NET8.Test.csproj -l "console;verbosity=detailed"
```
