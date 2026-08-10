# Migration to the project En16931

This document is a plan. It tells you how Pacioli can replace the external package
`Vit.FeRD.ExtendedDocument` with the new project `src\En16931`.

- Made on: 2026-08-10
- Branch: `EN16931`
- State: phase 0 to 6 are done. **Pacioli reads a UBL invoice**, and `Pacioli.Pdf` does not
  use `Vit.FeRD.ExtendedDocument` any more. The package is in the test project only, where
  it is the independent control of the parser for a CII document. Phase 7 is left.

## Contents

- [The two libraries](#the-two-libraries)
- [What the migration gives, and what it costs](#what-the-migration-gives-and-what-it-costs)
- [The three strategies](#the-three-strategies)
- [The gap between the two models](#the-gap-between-the-two-models)
- [The plan](#the-plan)
- [Risks](#risks)
- [The files that change](#the-files-that-change)

## The two libraries

| | `Vit.FeRD.ExtendedDocument` 0.0.1-7 | `En16931` (new) |
|---|---|---|
| Form | NuGet package, external | Project in this solution |
| Syntax | CII only | CII **and** UBL 2.1 |
| Model | The tree of the CII schema | Flat business terms (BT and BG) |
| Profile | Factur-X 1.09.2 EXTENDED | EN 16931 core only |
| Code lists | 24 lists, German and English | the same 24 lists, after the port of phase 2 |
| Writer | yes | no (Pacioli does not write) |
| Size | ~1 assembly | 15 files |

Pacioli reads invoices only. It does not write them. Thus the writer of the package has
no value for Pacioli.

Note: the project did the same step one time before. Pacioli used `ZUGFeRD-csharp` of
s2industries, and it moved to `Vit.FeRD.ExtendedDocument`. See
[InvoiceDescriptor.md](InvoiceDescriptor.md).

## What the migration gives, and what it costs

### It gives

1. **UBL.** This is the strongest reason. Pacioli cannot read a UBL invoice today. The
   constructor of `InvoiceWriter` calls `FeRDInvoice.Load`, and that call fails on a UBL
   document. About one half of the German XRechnung documents use UBL. With `En16931`,
   one renderer serves both syntaxes.
2. **No external dependency.** The parser becomes part of the solution. A change of the
   renderer and a change of the parser go into one commit.
3. **A flat model.** A section reads `invoice.Seller.Name` and not
   `invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.SellerTradeParty.Name`.
   Each property carries its BT or BG number.

### It costs

The model of `En16931` holds the **core** of EN 16931. The renderer of Pacioli shows much
more than the core: it shows the EXTENDED profile of ZUGFeRD. A direct exchange of the
two models makes the page poorer.

The test case `XmlSample_AllValuesAreVisible` measures this. Phase 0 made two baselines.
The files are in [doc/baseline](baseline).

The samples of the specification, 61 XML documents:

| Result | Values | Part |
|---|---:|---:|
| Values in the XML input | 7517 | 100,0 % |
| On the page | 5641 | 75,0 % |
| Not rendered, by design (codes, technical data) | 1876 | 25,0 % |
| **Known gap of the renderer** | **0** | **0,0 %** |
| Missing, without an entry (test failure) | 0 | 0,0 % |

The samples and the CII part of the ZUGFeRD corpus, 101 XML documents:

| Result | Values | Part |
|---|---:|---:|
| Values in the XML input | 11012 | 100,0 % |
| On the page | 8204 | 74,5 % |
| Not rendered, by design (codes, technical data) | 2807 | 25,5 % |
| **Known gap of the renderer** | **1** | **0,0 %** |
| Missing, without an entry (test failure) | 0 | 0,0 % |

**About three quarters of all values of the XML input are on the page, and the renderer
has one known gap.** This is the level that the migration must keep. (After phase 6 the
test run also holds the UBL samples; see that phase for the numbers of the wider base.) The next chapter
names each value that a direct exchange would remove.

> An earlier report gave 79,1 % for the samples of the specification. That number was too
> high. The test looked for the tax category code, and a code with one letter, for
> example `S`, agrees with almost each page. Phase 0 put `CategoryCode` in the list of
> the values that the page does not show by design, thus the number went to 75,0 %. The
> page did not change.

## The three strategies

| | Strategy | Result |
|---|---|---|
| **A** | Exchange the model. The renderer reads `En16931` only. | Small work. The page loses much data. The count of the known gaps goes from 0 to more than 30. |
| **B** | Keep both. `Vit.FeRD` for CII, `En16931` for UBL. | The page keeps its data. But each section exists two times, and the external dependency stays. |
| **C** | Give `En16931` an extension layer for the terms that go over the core. Then the renderer reads `En16931` only. | The page keeps its data. The dependency goes away. UBL comes. More work in `En16931`. |

**The recommendation is C.** A and B do not reach the goal:

- A removes the dependency, but it makes the product worse. The user of Pacioli reads an
  invoice to control it. A missing discount term or a missing advance payment is a
  defect, not a detail.
- B keeps the dependency. Then the migration has no reason.

C is more work, but the work is known: [the gap](#the-gap-between-the-two-models) names
each group that the extension layer needs. The renderer uses 15 such groups, not the full
EXTENDED profile.

## The gap between the two models

The renderer reads these members of `Vit.FeRD.ExtendedDocument`. The column *In
`En16931`* tells you if the new model holds the same data.

> **Phase 4 closed each gap of the tables below.** A row with **no** now has its place in
> `En16931.Model.Extended`; the tables stay as they are, because they name the work of
> phase 5: a row with **no** is a value that a section must take from
> `Extensions`, and a row with **yes** is a value that a section takes from the core.
> `ModelReachabilityTests` controls that no value is left.

### Document

| Data | In `En16931` | Used by |
|---|---|---|
| `ExchangedDocument.Id`, `TypeCode`, `IssueDateTime` | yes | `HeaderSection` |
| `ExchangedDocument.Name` (title of the document) | **no** | `HeaderSection`, `NotesSection` |
| `IncludedNote.SubjectCode`, `Content` | yes | `NotesSection` |
| `IncludedNote.ContentCode` | **no** | `NotesSection` |

### Party

| Data | In `En16931` | Used by |
|---|---|---|
| Name, address, VAT number | yes | `PartyBox` |
| `Description` (free text of the party) | yes (seller only, BT-33) | `PartyBox`, `TradeAgreementSection` |
| **More than one** `DefinedTradeContact` | **no** (one contact only) | `PartyBox` |
| Contact `DepartmentName` | **no** | `InvoiceText.AppendContact` |
| Contact `FaxUniversalCommunication` | **no** | `InvoiceText.AppendContact` |
| `Id` and `GlobalId` apart, with the scheme | partly (one list, mixed) | `PartyBox`, `ReferencesSection` |
| `SpecifiedLegalOrganization.PostalTradeAddress` | **no** | `PartyBox`, `InvoiceText.FormatLegalOrg` |
| `InvoiceeTradeParty` | **no** | `PartiesSection` |
| `ShipFromTradeParty` | **no** | `PartiesSection`, `TradeAgreementSection` |
| `ShipToTradeParty` | yes | `PartiesSection` |

### Head of the transaction

| Data | In `En16931` | Used by |
|---|---|---|
| Buyer reference, contract number, order number | yes | `ReferencesSection`, `TradeAgreementSection` |
| Date of the order (`BuyerOrderReferencedDocument.FormattedIssueDateTime`) | **no** | `ReferencesSection` |
| `SpecifiedProcuringProject.Name` | **no** (the ID only) | `ReferencesSection`, `TradeAgreementSection` |
| `QuotationReferencedDocument` | **no** | `TradeAgreementSection` |
| `DeliveryNoteReferencedDocument` | **no** | `TradeAgreementSection`, `LineItemsSection` |
| `DespatchAdviceReferencedDocument` name and date | **no** (the ID only) | `TradeAgreementSection` |
| `RelatedSupplyChainConsignment` (mode of transport) | **no** | `TradeAgreementSection` |
| `AdditionalReferencedDocument`: date, `LineId`, `ReferenceTypeCode` | **no** | `TradeAgreementSection`, `InvoiceText.FormatReferencedDocument` |
| `ReceivableSpecifiedTradeAccountingAccount.TypeCode` | **no** | `TradeAgreementSection` |
| `InvoiceReferencedDocument`: name, URI, type, position | **no** (number and date only) | `TradeAgreementSection` |
| `SpecifiedAdvancePayment` (amount, day, tax, invoice) | **no** | `TotalsSection`, `InvoiceText.FormatAdvancePayment` |
| `SpecifiedLogisticsServiceCharge` (transport costs) | **no** | `AllowanceChargeSection` |
| `TaxApplicableTradeCurrencyExchange` (exchange rate) | **no** | `TaxSummarySection` |
| **More than one** `SpecifiedTradePaymentTerms` | **no** (the first one only) | `PaymentTermsSection` |
| `ApplicableTradePaymentDiscountTerms` (Skonto) | **no** | `PaymentTermsSection` |
| `ApplicableTradePaymentPenaltyTerms` (Verzugszinsen) | **no** | `TradeAgreementSection` |
| Tax: `TypeCode`, `LineTotalBasisAmount`, `AllowanceChargeBasisAmount`, `TaxPointDate` per entry | **no** | `TaxSummarySection`, `InvoiceText.FormatTradeTax` |
| Payer account: name and BIC | **no** (the IBAN only) | `BankAccountsSection` |

### Position

| Data | In `En16931` | Used by |
|---|---|---|
| Number, name, description, quantity, prices, sum | yes | `LineItemsSection` |
| Delivery date of the position | **no** | `LineItemsSection` |
| **More than one** note, and `ContentCode` | **no** (one text only) | `LineItemsSection` |
| `IndividualTradeProductInstance` (serial number, batch) | **no** | `LineItemsSection` |
| `IncludedReferencedProduct` (parts of a set) | **no** | `LineItemsSection` |
| `DesignatedProductClassification.ClassName` | **no** (the code only) | `LineItemsSection` |
| `ShipToTradeParty` of the position | **no** | `LineItemsSection` |
| `AdditionalReferencedDocument` of the position, **with the attachment** | **no** | `AttachmentsSection`, `LineItemsSection` |
| Order, contract and quotation document of the position | **no** (order line ID only) | `LineItemsSection` |
| `ReceivableSpecifiedTradeAccountingAccount` of the position | yes (the ID only) | `LineItemsSection` |
| Full tax of the position (amount, basis, exemption) | **no** (category and rate only) | `LineItemsSection` |
| Basis quantity of the net price **and** of the gross price | **no** (one value only) | `LineItemsSection` |
| Discount on the gross price with percent, basis and reason | **no** (the amount only) | `LineItemsSection` |

### Two more parts that `En16931` does not hold

1. ~~**The code lists.**~~ Closed in phase 2: the 24 lists are now in `En16931.Codes`.
2. ~~**The attachment of a PDF file.**~~ Closed in phase 3: `EInvoiceExtractor` reads the
   name tree of the embedded files and asks `InvoiceReader.DetectSyntax`.

## The plan

Seven phases. Each phase ends with a green test run. The test case
`XmlSample_AllValuesAreVisible` is the measure: **its count of known gaps must stay 1,
and the part of the values on the page must not go below 74,5 %** with the configuration
of `testsettings.json` (the samples and the corpus).

The file `Content\XmlValueExclusions.cs` holds the entries of the kind `KnownGap`. Use
this mechanism during the migration: if a phase must remove a value from the page for a
short time, add a `KnownGap` entry with the reason and with the number of the phase. The
report then counts the debt of the migration, and the debt must be 0 at the end.

### Phase 0 — Prepare — **done on 2026-08-10**

| Step | File | Result |
|---|---|---|
| Put `src\En16931` into git. | — | done |
| Remove the unused package reference. `Pacioli.CLI` and `Pacioli.WindowsApp` named `Vit.FeRD.ExtendedDocument`, but no file of the two projects used it. | `Pacioli.CLI.csproj`, `Pacioli.WindowsApp.csproj` | done. The package is now in `Pacioli.Pdf` and in `Pacioli.CLI.Test` only. |
| Make the project `En16931.Test`. | `src\En16931.Test` | 235 test cases, all green |
| Give the sample test more than one samples directory. | `TestSettings.cs`, `SampleLocator.cs`, `testsettings.json` | done, with the key `samplesDirectories` |
| Add the corpus of ZUGFeRD. | `testsettings.json` | done. The CII samples go from 61 to 101. |
| Run the sample test and keep the report as the baseline. | `doc\baseline` | two baselines, see above |

**Result:** the baseline is fixed, the dependency is in two projects, and the test base is
wider than before.

#### What the corpus holds

The corpus of ZUGFeRD ([github.com/ZUGFeRD/corpus](https://github.com/ZUGFeRD/corpus))
holds documents of the practice. The test case `SampleCorpusTests.Corpus_Survey` counts
them, together with the samples of the specification:

| | XML documents |
|---|---:|
| CII | 103 |
| **UBL** | **31** |
| Syntax not known | 0 |
| Not an EN 16931 invoice (FatturaPA) | 15 |
| Not well formed XML | 0 |

**`En16931` reads each one of the 134 invoices into the model, without an exception.**
The 31 UBL documents are the test base of phase 6. 28 of them have the same name as a
CII document of the corpus, because a converter made them from that document. Thus phase 6
can render the two documents of one invoice and compare the two pages.

The corpus also holds material that Pacioli does not read. `excludeDirectories` in
`testsettings.json` names each directory, with the reason.

#### What the corpus showed about the renderer

The wider test base gave three findings. All three are older than the migration.

| Finding | Kind | What happened |
|---|---|---|
| The page does not show the tax category code, for example `AE`. | by design | New entry `CategoryCode` in `XmlValueExclusions`. The page shows the text of the code. The old entry was missing, and a code with one letter (`S`) hid it. |
| The page does not show a VATEX code, for example `VATEX-EU-O`. | by design | New entry for the code of the head. The tax table shows the text of the code. |
| The page does not show BT-15, the reference of the receiving advice. | **known gap** | New entry of the kind `KnownGap`. `TradeAgreementSection` shows the despatch advice (BT-16) only. This is a task for the renderer. |

### Phase 1 — Keep the test independent

The test project `Pacioli.CLI.Test` reads the sample data with `Vit.FeRD.ExtendedDocument`
(`Content\InvoiceLoader.cs`). The README of the project says: *the test reads the invoice
data with the same library as the renderer, thus the content check tests the render step;
it does not test the parse step.*

**Keep `Vit.FeRD.ExtendedDocument` in the test project until the last phase.** When the
renderer reads with `En16931` and the test reads with `Vit.FeRD`, the test case
`Sample_RenderedPdf_ShowsKeyElements` compares the two parsers. It then tests the parse
step too. This is the strongest control that the migration can have, and it costs nothing.

**Result:** no change of the code. A decision, and a note in the README of the test.

### Phase 2 — The code lists — **done on 2026-08-10**

The lists came by a port from the source of the package
(`...\FeRD Spezifikation\lib\Vit.FeRD.ExtendedDocument`) into the namespace
`En16931.Codes`. Four files: `CodeList`, `CodeLists`, `CodeListData.g.cs` (the workbook of
the specification) and `CodeListDataDe` (the German table). The public form did not change:
`GetName(code, language)`, `Describe(code, language)`, `CodeLanguage.German`.

| Step | Result |
|---|---|
| Port the four files, namespace `En16931.Codes`. | 24 lists |
| Write the doc comments new. The old ones named the classes of the CII model. | Each list now names its business term, for example "BT-3, the invoice type code". |
| `CodeLists.ReasonFor` took a `TradeAllowanceCharge` of the old model. | It now takes `Model.AllowanceCharge` and reads `IsCharge`. |
| Give `Pacioli.Pdf` a project reference to `En16931`. | done |
| Point the nine files that use a code list to the new class. | done, with `using CodeLists = En16931.Codes.CodeLists;` |

Both libraries have a class `CodeList`, `CodeLists` and `CodeLanguage`. `Pacioli.Pdf` holds
the model of the old library and the lists of the new one at the same time, thus each file
needs an alias. The aliases go away with the last section in phase 5.

**Test:** `CodeListTests` (the form of the lists, 24 lists) and `CodeCoverageTests`.
`CodeCoverageTests` reads each sample into the model, takes each value that is a code, and
looks for it in the list of that code. It works from the model, thus one test covers CII
and UBL. **Each code of the 134 invoices of the samples and of the corpus gives a name.**

#### One hole in the official list

`CodeCoverageTests` found one code that documents of the practice use and that the
workbook does not give:

| Code | List | Where |
|---|---|---|
| `9958` German Leitweg ID | EAS, the scheme of an electronic address | `EN16931_ElektronischeAdresse` and `EN16931_RechnungsUebertragung` of the corpus, in both syntaxes |

The EAS list does not give 9958 any more, because the Leitweg-ID belongs to BT-10 today.
German documents still use it. Without an entry, a page shows `9958` in the place of a
name.

`CodeListData.g.cs` comes from a program, thus an entry by hand would go away with the
next run. The new file `CodeListDataExtra.cs` is the place for such an entry. Each entry
needs a reason and a document that holds the code. **An extra entry fills a hole only:**
the parser gives the name of the specification priority, thus an extra entry can never
change an official name.

The media type of an attachment (BT-125-1) is not part of this test. The specification
allows six media types, and the corpus holds `application/octet-stream`. That is not a
hole in a list: a media type is already a readable text. The question belongs to a
validation.

**Result:** `CodeText.cs`, `TaxTypesExtensions.cs`, `TaxCategoryCodesExtensions.cs`,
`QuantityCodesExtensions.cs` and five sections take their code lists from `En16931`.
The rendered page did not change: the report of the samples and of the corpus agrees with
the baseline of phase 0, value for value.

### Phase 3 — The attachment of the PDF file — **done on 2026-08-10**

`ZugferdReader` and `ZugferdExtractor` went away. The new class is
`Pacioli.Pdf\ERechnung\EInvoiceExtractor.cs`. It has a new name because it also finds a
UBL attachment.

The old way walked through each object of the PDF file and gave each stream to
`FeRDInvoice.Load`. A call without an exception was the answer. The new way reads the name
tree of the embedded files (`PdfCatalog.GetNameTree(PdfName.EmbeddedFiles)`), takes the
file name from `UF` or from `F`, and asks `InvoiceReader.DetectSyntax`. That test looks at
the root element only, thus it raises no exception and it is fast.

A PDF file can hold more than one attachment: the invoice, and an enclosure, for example a
bill of quantities. The class puts the attachments in an order and takes the first one
that is an invoice:

| Group | Attachment |
|---:|---|
| 1 | A name of the specification: `factur-x.xml`, `zugferd-invoice.xml`, `xrechnung.xml`, `cii.xml` |
| 2 | An other attachment with the ending `.xml` |
| 3 | Each other attachment |

Inside a group, the order of the document stays.

**Test:** `PdfAttachmentTests` gives each PDF sample to the old way and to the new way and
compares the bytes. The old way is a copy in the test project
(`Content\LegacyPdfAttachmentReader.cs`); it uses the old library, as the test project
does anyway until phase 7. **The new way finds the same bytes as the old way, for each of
the 165 PDF samples.** Two more test cases control that the extractor names the attachment
and the syntax, and that a file that is not a PDF file raises no exception.

**Result:** `Pacioli.Pdf` reads a PDF attachment without the old library, and the step is
faster: the two ways together need 3 seconds for 165 files. `EInvoiceExtractor` also gives
the name of the attachment and the syntax, which the old class did not give. The rendered
page did not change.

### Phase 4 — The extension layer of `En16931` — **done on 2026-08-10**

The 15 groups of [the gap](#the-gap-between-the-two-models) are in the model. The new
namespace is `En16931.Model.Extended`, in three files.

#### The form

A type of the core carries an `Extensions` property. The property is `null` if the
document gives no such data:

```csharp
invoice.Seller.Name                      // BT-27, the core
invoice.Seller.Extensions?.Contacts      // each contact, over the core
line.Item.Extensions?.Instances          // the serial numbers
```

**The core keeps its form.** Each property of the core still carries its BT or BG number,
and the plan of phase 5 does not change: a section reads the core for a business term, and
the extension for the rest. Eleven types of the core have the property today: `Invoice`,
`InvoiceNote`, `Party`, `Contact`, `PrecedingInvoiceReference`, `SupportingDocument`,
`PaymentInstructions`, `InvoiceLine`, `LineItem`, `PriceDetails`, and `VatBreakdown`
(there the property has the name `Detail`, because it holds the full tax of that entry).

The reading sits in `CiiInvoiceParser.Extended.cs`, a second part of the parser class.
Each method there takes the same XML element as the method of the core that calls it, thus
a value can never come to the wrong object. The UBL parser fills the extensions in
phase 6, where UBL also has the data.

#### The control

`ModelReachabilityTests` in `Pacioli.CLI.Test` is the acceptance test of this phase. For
each XML sample it reads every value of the document, reads the same document with
`En16931`, and walks the model with reflection. **Each value that the page shows today
must be in the model.**

"The page shows it today" means: no entry of `XmlValueExclusions` applies to it. The test
case `XmlSample_AllValuesAreVisible` passes with 0 missing values, thus a value without an
entry is on the page.

**All 101 XML samples pass.** Phase 5 can move a section without loss.

#### What the test found

The first run gave 10 samples with a gap. Two were missing groups, and one was a defect of
the parser:

| Finding | What happened |
|---|---|
| The ship-to party loses its identifiers, its tax registrations and its contacts. | The core holds the name, the location and the address of this party (BG-13 and BG-15). `InvoiceExtensions.ShipTo` now holds the full party. |
| `PackageQuantity` of a position has no place. | New in `InvoiceLineExtensions`, together with `ChargeFreeQuantity`. |
| **A document with two invoiced object identifiers loses the first one.** | The parser wrote `invoice.InvoicedObjectIdentifier = ...` in a loop. BT-18 gives one identifier, but the samples X14 and E20 name two objects with the type code 130. The core now keeps the first (`??=`), and `InvoiceExtensions.InvoicedObjectIdentifiers` keeps each one. |

The last one is a defect of `En16931` that no other test saw, because the model was
consistent with itself. The renderer of today shows both values.

#### The 15 groups

1. Title of the document, and the content code of a note.
2. More than one contact per party, with department and fax.
3. Invoicee party and ship-from party.
4. Address of the legal organisation.
5. Referenced documents with date, position and type code (order, contract, quotation,
   delivery note, despatch advice, and the additional documents).
6. Name of the project.
7. Mode of transport.
8. Advance payments.
9. Logistics service charges.
10. Exchange rate of the tax.
11. More than one payment term, with discount terms and penalty terms.
12. Full tax data on the head and on the position.
13. Full data of the position: delivery date, notes, serial numbers, included products,
    name of the classification, ship-to, referenced documents with attachment.
14. Basis quantity of the net price and of the gross price apart.
15. Discount on the gross price with percent, basis and reason.

`ExtensionsTests` in `En16931.Test` names the shape of each group, with a fixture that
holds all 15. `ModelReachabilityTests` controls the same over 101 documents of the
practice.

**Result:** `En16931` can carry the full page. No file of `Pacioli.Pdf` changed.

### Phase 5 — The renderer, section by section — **done on 2026-08-10**

All fourteen sections read the model of `En16931`. **`Pacioli.Pdf` does not use
`Vit.FeRD.ExtendedDocument` any more**, and the package is out of its project file.

`InvoiceContext` was the seam: it carried the old model and the new one at the same time,
and `InvoiceWriter` read the document two times. With the last section both went away.
`InvoiceContext` also lost its short ways into the tree (`Agreement`, `Delivery`,
`Settlement`, `Summation`): the new model is flat, thus a section reads `Core.Seller.Name`
and needs no help.

The text steps moved from `InvoiceText.cs` to the new `CoreText.cs`, each one with its
first caller. `InvoiceText.cs` is gone. Two sections that stood in no list went with it:
`TransactionSection` and `FurtherDocumentsSection` were dead code.

#### What the migration found

| Finding | What happened |
|---|---|
| `En16931` trims the text of an element; the old library keeps it as it stands. The name of a seller ends with a line break and spaces in one sample, thus the old page held a blank line. | The trimming stays: the text of a business term must not carry the spaces of the XML. The page then became two lines shorter, a page break went through a row of the tax table, and the text of an exemption stood with one part on each page. The table now has `KeepTogether`. A split row was a defect of the renderer, and no sample showed it before. |
| The head of the invoice names its documents in one group, and the parser splits that group by the type code: 130 is the invoiced object (BT-18), 50 is the tender or lot (BT-17), each other code is a supporting document (BG-24). The table of the documents showed 13 values less. | The core keeps its split, because it follows EN 16931. `InvoiceExtensions.AdditionalDocuments` now keeps the group as the document gives it, and the table reads that list. |

#### The control

The test project still reads the sample data with `Vit.FeRD.ExtendedDocument`, as phase 1
decided. **Thus `Sample_RenderedPdf_ShowsKeyElements` now compares the two parsers**: the
renderer reads with `En16931`, the test reads with the old library, and each key element
must agree. It passes for each of the 165 samples.

**Result:** 1069 test cases green. The report agrees with the baseline of phase 0, value
for value: 8204 values on the page, 74,5 %, one known gap.

### Phase 6 — UBL — **done on 2026-08-10**

**Pacioli reads a UBL invoice.** This is the goal of the migration.

The renderer needed no change: it reads `InvoiceReader`, and that class knows both
syntaxes since phase 0. `EInvoiceExtractor` finds a UBL attachment in a PDF file since
phase 3. The work of this phase was the parser and the test.

#### The parser

`UblInvoiceParser.Extended.cs` fills the extension layer: the tax registrations of a party
with their scheme, the identifiers under a scheme, the payment terms with their discount
and their penalty, the despatch advice, the deliver to party, the documents of the head
and of a position, the full tax of a position, and the allowances on the price.

UBL does not hold each group that CII holds: it has no title of the document, no content
code of a note, no invoicee party, no ship-from party and no logistics service charge.
Such a property stays null, and the page then shows nothing for it.

#### The test base

`excludeDirectories` lost the names `UBL`, `PEPPOL` and `other`. The test run went from
101 to 132 XML samples, and from 11012 to 1366385 values — the corpus holds two stress
documents of PEPPOL with 26813 and 63404 positions.

| Result, samples and corpus, 132 XML documents | Values | Part |
|---|---:|---:|
| Values in the XML input | 1366385 | 100,0 % |
| On the page | 1182555 | 86,5 % |
| Not rendered, by design | 183828 | 13,5 % |
| **Known gap of the renderer** | **2** | **0,0 %** |
| Missing, without an entry (test failure) | 0 | 0,0 % |

**The CII path did not change.** With the configuration of phase 5 the report gives 11012
values, 8204 on the page, 74,5 %, and one known gap — value for value the baseline of
phase 0.

#### What the first run showed

The first run with the UBL samples gave 80969 missing values. Three causes, in this order
of size:

| Cause | Values | What happened |
|---|---:|---|
| The matcher knew the date of CII only. It reads `20200305`, and UBL writes `2020-03-05`. | ~53000 | `XmlValueMatcher` now knows both forms. This was a hole in the test, not in the renderer. |
| The two syntaxes give the same term a different name, thus the entries of `XmlValueExclusions` did not apply. | ~27000 | New entries for `TaxScheme/ID`, `InvoiceTypeCode`, `ChargeIndicator`, `ProfileID`, `CustomizationID`, and the others. |
| The UBL parser filled no extension. | 132 | `UblInvoiceParser.Extended.cs`. |

#### Two more findings

| Finding | What happened |
|---|---|
| **The control of phase 1 has a limit.** `Sample_RenderedPdf_ShowsKeyElements` reads the expected values with `Vit.FeRD.ExtendedDocument`, and that library reads CII only. It failed for each of the 30 UBL samples. | The test case now gives the result "ignored" for a document that the old library cannot read, with the reason. The control keeps its full value for the CII samples. `XmlSample_AllValuesAreVisible` still checks each value of a UBL document, because it works from the XML and not from a model. |
| **The renderer needs minutes for a very big invoice.** `Large_Invoice_sample2.xml` of PEPPOL has 63404 positions in 63 MB, and it does not render in 120 seconds. The sample with 26813 positions does. | `timeoutSeconds` is now 600, with the numbers in `testsettings.json`. The value is far over a normal invoice, but the limit is now measured and not unknown. |
| **BT-159, the country of origin of an article, is on no page.** | A new entry of the kind `KnownGap`. A CII document gives the same term, and a country code of two letters agrees with almost each page, thus this gap stayed hidden until the UBL samples came. |

### Phase 7 — Remove the dependency — **in part**

| Step | File | State |
|---|---|---|
| Remove the package from `Pacioli.Pdf`. | `Pacioli.Pdf.csproj` | **done in phase 5** |
| Mark the document of the old model as history. | `doc\CrossIndustryInvoice.md` | **done** |
| Write the document of the new model. | `doc\En16931.md` | **done** |
| Decide about the test project. | `Pacioli.CLI.Test.csproj` | open |
| Give the version number of Pacioli a step. | `Pacioli.WindowsApp.csproj`, `version.json` | open |

#### The decision about the test project

`Pacioli.CLI.Test` still holds `Vit.FeRD.ExtendedDocument`. Two ways:

- **Keep it.** The renderer reads with `En16931`, the test reads with the old library, and
  `Sample_RenderedPdf_ShowsKeyElements` compares the two. A defect of the new parser then
  shows itself, and no other test can find such a defect. The cost is one package in one
  test project, and the control works for a CII document only.
- **Remove it.** Then `InvoiceElementRules` must take its values from `En16931`, and the
  content check tests the render step alone, as it did before the migration.

**The recommendation is to keep it.** The package costs nothing at run time, and it is the
only control that looks at the parser from the outside. `Content\LegacyPdfAttachmentReader.cs`
uses it for the same reason: it guards the step that takes the invoice out of a PDF file.

## Risks

| Risk | Effect | What to do |
|---|---|---|
| The extension layer grows to a copy of the EXTENDED profile. | The work does not end. | Add a group only if a section uses it. The list of phase 4 is complete for the renderer of today. |
| ~~The code lists are much work.~~ | — | Closed in phase 2. The source of the package was available, thus the tables came by a port. |
| The parser of `En16931` has a defect that the model hides. | The page shows a wrong value, and no test sees it. | Keep `Vit.FeRD` in the test project (phase 1). The two parsers then control each other. |
| ~~No UBL samples.~~ | — | Closed in phase 0. The corpus gives 31 UBL documents. |
| A sub invoice line (`ParentLineId`, `LineStatusCode`) has no place in the flat model. | The samples X02 and X03 lose their structure. | The renderer of today does not show the structure either. Keep the flat form, and put the parent ID in the extension. |
| The migration takes long, and the branch goes far from `main`. | A merge is difficult. | Each phase is one commit and one merge. Phase 0 to 4 change no behaviour, thus they can go to `main` early. |

## The files that change

| Project | Files | State |
|---|---|---|
| `En16931` | 11 for the parser, 5 for the code lists, 3 for the extension layer | the model is complete for the renderer |
| `En16931.Test` | 9 | 502 test cases |
| `Pacioli.Pdf` | 24 of 31 files | reads `En16931` only; `InvoiceText.cs` and two dead sections are gone |
| `Pacioli.CLI` | `Pacioli.CLI.csproj` only | done in phase 0 |
| `Pacioli.WindowsApp` | `Pacioli.WindowsApp.csproj` only | done in phase 0 |
| `Pacioli.CLI.Test` | 9 files | the samples directories, the exclusions, the control of the PDF step, and the reachability of the model |
| `doc` | 3 files, and the baselines | — |

The public interface of `Pacioli.Pdf` does not change. `Pacioli.WindowsApp` and
`Pacioli.CLI` use `InvoiceWriter(fileName)`, `Write`, `GetXml`, `GetAttachments`,
`IsZugferd` and `ZugferdExtractor`. Keep these names, or change `IsZugferd` and
`ZugferdExtractor` in phase 6, when the tool also reads UBL.
