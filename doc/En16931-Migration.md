# Migration to the project En16931

This document is a plan. It tells you how Pacioli can replace the external package
`Vit.FeRD.ExtendedDocument` with the new project `src\En16931`.

- Made on: 2026-08-10
- Branch: `EN16931`
- State: phase 0, 2, 3 and 4 are done. `Pacioli.Pdf` takes its code lists from `En16931`,
  and it reads the attachment of a PDF file without the old library. `En16931` can now
  carry the full page. The model of the renderer is still `Vit.FeRD.ExtendedDocument`;
  phase 5 exchanges it, section by section.

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
has one known gap.** This is the level that the migration must keep. The next chapter
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

### Phase 5 — The renderer, section by section

`InvoiceContext` is the seam. Give it a second property:

```csharp
public CrossIndustryInvoice Invoice { get; }   // old, goes away at the end
public En16931.Model.Invoice Core { get; }     // new
```

`InvoiceWriter` then reads the document two times, one time with each library. The cost is
small, and each section can move alone.

Move the sections in this order. The order goes from the simple section to the difficult
section:

| Order | Section | Note |
|---:|---|---|
| 1 | `BuyerElectronicAddressSection` | 22 lines |
| 2 | `HeaderSection` | needs the title of the document |
| 3 | `ContactsSection` | needs more than one contact |
| 4 | `TotalsSection` | needs the advance payments |
| 5 | `ReferencesSection` | needs the date of the order |
| 6 | `PartiesSection` + `PartyBox` | needs the invoicee and the ship-from party |
| 7 | `BankAccountsSection`, `PaymentMeansSection` | |
| 8 | `PaymentTermsSection` | needs more than one term, and the discount terms |
| 9 | `TaxSummarySection` | needs the full tax data and the exchange rate |
| 10 | `AllowanceChargeSection` + `TradeAllowanceChargesTab` | needs the service charges |
| 11 | `NotesSection` | needs the content code |
| 12 | `AttachmentsSection` | needs the attachments of a position |
| 13 | `LineItemsSection` | the biggest section, 215 lines |
| 14 | `TradeAgreementSection` | the biggest section, 295 lines |

`InvoiceText.cs` (319 lines) holds the text steps of more than one section. Move a method
of this class together with its first caller.

**Test:** run `dotnet test src\Pacioli.CLI.Test\Pacioli.CLI.Test.csproj` after **each**
section. The part of the values on the page must not go down.

**Result:** `Pacioli.Pdf` does not use the package any more. Remove the property
`InvoiceContext.Invoice` and the second read in `InvoiceWriter`.

### Phase 6 — UBL

1. `InvoiceWriter` gives a UBL file to `InvoiceReader` too.
2. `EInvoiceExtractor` finds a UBL attachment in a PDF file.
3. Take the names `UBL`, `PEPPOL` and `other` out of `excludeDirectories` in
   `testsettings.json`. The 31 UBL documents of the corpus then go into the test run.
   28 of them have a CII document with the same name, thus you can compare the two pages
   of one invoice.
4. Control the code lists with the codes of UBL. UBL uses other lists in some places,
   for example the scheme of the electronic address.
5. Control the resources: `TypeCodeMapping` and `QuantityCodesExtensions` read a resource
   with the code in its name. A UBL document can bring a code that has no resource.

**Test:** a new report for the UBL samples. The first run gives the gaps of the UBL path.
Close them as in phase 5.

**Result:** Pacioli reads a UBL invoice. This is the goal of the migration.

### Phase 7 — Remove the dependency

| Step | File |
|---|---|
| Remove the package from `Pacioli.Pdf`. | `Pacioli.Pdf.csproj` |
| Decide about the test project. Keep the package as an independent control, or remove it and move the rules to `En16931`. | `Pacioli.CLI.Test.csproj` |
| Mark the document of the old model as history, as with `InvoiceDescriptor.md`. | `doc\CrossIndustryInvoice.md` |
| Write the document of the new model. | `doc\En16931.md` (new) |
| Give the version number of Pacioli a step. | `Pacioli.WindowsApp.csproj`, `version.json` |

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
| `Pacioli.Pdf` | 24 of 34 files | 10 files use the new code lists, and `EInvoiceExtractor` is new; the model comes in phase 5 |
| `Pacioli.CLI` | `Pacioli.CLI.csproj` only | done in phase 0 |
| `Pacioli.WindowsApp` | `Pacioli.WindowsApp.csproj` only | done in phase 0 |
| `Pacioli.CLI.Test` | 9 files | the samples directories, the exclusions, the control of the PDF step, and the reachability of the model |
| `doc` | 3 files, and the baselines | — |

The public interface of `Pacioli.Pdf` does not change. `Pacioli.WindowsApp` and
`Pacioli.CLI` use `InvoiceWriter(fileName)`, `Write`, `GetXml`, `GetAttachments`,
`IsZugferd` and `ZugferdExtractor`. Keep these names, or change `IsZugferd` and
`ZugferdExtractor` in phase 6, when the tool also reads UBL.
