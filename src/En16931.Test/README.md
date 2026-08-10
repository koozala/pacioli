# En16931.Test

The test of the parser project `En16931`. `En16931` reads an EN 16931 invoice in both
permitted XML syntaxes — CII and UBL — into one model.

This project belongs to the migration to `En16931`. See
[doc/En16931-Migration.md](../../doc/En16931-Migration.md).

## The test cases

| Group | File | Function |
|---|---|---|
| The entry point | `InvoiceReaderTests.cs` | The reader finds the syntax, and it gives a clear error for a document that is not an invoice. |
| CII | `CiiInvoiceParserTests.cs` | The CII parser reads each part of the fixture into the model. |
| UBL | `UblInvoiceParserTests.cs` | The UBL parser does the same, also for a credit note. |
| Both | `SyntaxAgreementTests.cs` | Each business term gives the same value from both syntaxes. |
| The code lists | `CodeListTests.cs` | The form of the 24 lists, and the names of the codes that a page shows most. |
| The code lists | `CodeCoverageTests.cs` | Each code of each sample document gives a name. |
| The corpus | `SampleCorpusTests.cs` | The parser reads each sample document of the configured directories. |

## The fixtures

`Fixtures.cs` holds two small invoices: one in CII, one in UBL. **The two documents hold
the same business content.** `SyntaxAgreementTests` reads both and compares 45 business
terms, one test case per term. If a term reads differently from the two syntaxes, that
test case names the term and the two values.

This is the promise of the library, and the renderer depends on it: the renderer reads the
model only, thus the page of a UBL invoice must be the page of the same invoice in CII.

The two documents are written by hand. Thus the repository holds no file of a
specification, and no condition of use applies to it.

## The code lists

`CodeCoverageTests` reads each sample into the model, takes each value that is a code, and
looks for it in the list of that code. It works from the model and not from the XML, thus
one test covers both syntaxes.

A code without a name is a hole in a code list: the page then shows `H87` and not `Stück`.
The test also covers the parser, because a code that comes to the wrong property does not
agree with its list.

A code that documents of the practice use, and that the workbook of the specification does
not give, goes into `En16931\Codes\CodeListDataExtra.cs`. Do not put it in
`CodeListData.g.cs`: a program makes that file.

## The corpus

`SampleCorpusTests` reads each XML file of the directories in `testsettings.json`. It
answers three questions:

1. Does the reader find the syntax of each document?
2. Does the reader give a model for each document, without an exception?
3. How many documents of each syntax does the corpus hold?

The test case `Corpus_Survey` writes the counts to the console. It does not fail on a
count; it is a report.

A document of an other standard, for example the Italian FatturaPA, must give the syntax
`Unknown`. The list `NotAnEn16931Invoice` names such documents, with a reason. A document
that the reader does not know, and that no entry explains, makes the test fail.

**The corpus does not belong to the repository.** If no directory of the settings is on
this computer, each test case of `SampleCorpusTests` gives the result "ignored". The other
test cases use the fixtures only, thus they always run.

### Configuration

| Source | Form |
|---|---|
| NUnit test parameter | `SamplesDirectories`, with a semicolon between the entries |
| Environment variable | `PACIOLI_SAMPLES_DIRS`, with a semicolon between the entries |
| `testsettings.json` | the list `samplesDirectories` |

The order above is the order of precedence.

## How to run the test

```
dotnet test src\En16931.Test\En16931.Test.csproj
```

Show the survey of the corpus:

```
dotnet test src\En16931.Test\En16931.Test.csproj -l "console;verbosity=detailed"
```

Run the fixture tests only, without the corpus:

```
dotnet test src\En16931.Test\En16931.Test.csproj --filter "FullyQualifiedName!~SampleCorpusTests"
```
