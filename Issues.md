# Issues

## Date of delivery or service in the PDF

Status of 2026-11-08.

### Done

- The new section `ServiceDateSection` writes a box "Liefer-/Leistungsdatum" below the
  parties. It shows the delivery date (BT-72) and the invoicing period (BG-14). If the
  document gives neither, the box tells that the positions give the date, or that the
  document gives no date.
- The boxes of the delivery and of the settlement do not show these two dates any more.

### Open

1. **Delivery or service.** The data does not tell if the invoice is for goods or for a
   service. Thus the box has the title "Liefer-/Leistungsdatum" and keeps the labels
   "Lieferdatum" (BT-72) and "Abrechnungszeitraum" (BG-14).
2. **Free text.** Some invoices give the date only in a note (BT-22) or in the text of
   a position. The box then says "kein Liefer- oder Leistungsdatum", although the date
   is in a text.

## Document information (Datei > Dokumentinformationen, Ctrl+I)

Status of 2026-11-08.

### Done

- The window shows the declared standard and profile (BT-24, BT-23), the XML syntax,
  the root element and its namespaces, the file, and for a PDF file the XMP metadata.
- The catalog of the identifiers and the comparison of the declarations are in
  `src/En16931/Conformance/`, with tests in `src/En16931.Test/ConformanceTests.cs`. The
  reader of the XMP metadata is `src/Pacioli.Pdf/ERechnung/PdfInvoiceMetadata.cs`.
- A run over the 122 documents of the ZUGFeRD 2.5.2 samples gave no disagreement.

### Open

1. **Samples without Factur-X XMP.** 51 PDF files of the official ZUGFeRD 2.5.2 samples
   (all PDF/A-3U files, producer "WeasyPrint 66.0", for example `E05_01_Einfach_fx.pdf`)
   have no ZUGFeRD or Factur-X properties in the XMP metadata. The window says so, but
   the agreed rules do not make it a disagreement. Decide if a missing declaration in
   the XMP is a disagreement.
2. **ZUGFeRD 1.0 cannot be opened.** The parser reads only CII D16B
   (`rsm:CrossIndustryInvoice`) and UBL. A ZUGFeRD 1.0 document
   (`rsm:CrossIndustryDocument`) gives an error when the user opens it. Thus the
   ZUGFeRD 1.0 entries of the catalog apply only to the XMP check.
3. **Other national CIUS.** The catalog knows no CIUS other than XRechnung (for example
   NLCIUS, CIUS-IT). Such an identifier shows as "nicht erkannt".
4. **Declaration, not proof.** The window shows declarations only. It does not use the
   result of the online validation.
5. **XRechnung in ZUGFeRD 2.1 PDF files.** Three files of the corpus
   (`zugferd_2p1_EN16931_*_XRechnung*.pdf`) have XRechnung in BT-24 and the XMP level
   "EN 16931". The window shows a disagreement, because the level "XRECHNUNG" is the
   level since ZUGFeRD 2.1.1. Decide if "EN 16931" is also correct for XRechnung.
6. **Early Factur-X identifiers.** Eight files of the corpus (Factur-X 1.0 of 2017,
   Mustang 2017) have the form `urn:cen.eu:en16931:2017:compliant:factur-x.eu:1p0:basic`,
   with ":" in place of "#". The catalog does not know this form ("nicht erkannt").
7. **Factur-X with UBL.** `FX-With-UBL-REC50304330.pdf` has Factur-X XMP metadata and a
   UBL invoice. The window shows this as a disagreement (correct).

## Online validation with W System

Status of 2026-11-08.

### Done

- The menu "Prüfung" and the buttons "Online prüfen" and "Prüfergebnis" of each
  document send the file (XML or ZUGFeRD PDF) to `POST https://api.wsystem.dev/einvoice/validate`.
  See `src/Pacioli.WindowsApp/Validation/`.
- The API key is in the table `ApiKeys` of `pacioli.db`. Windows encrypts the key for
  the current user (DPAPI). The table `UserPreferences` does not change.
- The result shows in a window (WebView2, no script). The document keeps the result
  until the user closes its tab.
- After each validation, `GET /billing/usage` gives the quota. All open documents show
  the remaining units at the right side of the row of buttons.

### Open

1. **No test project for the Windows app.** The parser of the answer
   (`ValidationReport`, `UsageInfo`) has no unit test in the solution. A manual check
   with a saved answer and with one real call was successful.
2. **No schema of the answer.** The OpenAPI document of W System gives no schema for
   the answer of `/einvoice/validate`. The parser reads each field by itself. If the
   service changes the names of the fields, the window shows less data.
3. **Peppol BIS 3.** W System does not check the rules PEPPOL-EN16931-* yet.
4. **Data protection.** Each validation sends the full invoice to an external service.
   The key dialog tells this to the user. A company can need a written agreement
   (DPA: https://wsystem.dev/legal/dpa).
5. **Language change.** The texts of the menu "Prüfung" come from the resources at the
   start of the program. After a change of the language, they change at the next start
   only. This is the same for the other menus.
6. **Quota at the start.** The program asks for the quota only after a validation. Before
   the first validation, the row of buttons shows no quota.

## Payment terms and discounts (Skonto)

Status of 2026-10-31.

### Done

- The parsers read the discounts of XRechnung from the text of the payment terms
  (BT-20). The form is `#SKONTO#TAGE=7#PROZENT=3.00#` with an optional
  `BASISBETRAG=100.00#` (rule BR-DE-18). A term can have more than one discount line.
  See `src/En16931/Parsing/XRechnungDiscounts.cs`.
- The PDF shows each discount on its own line, with the last day of the discount, for
  example "Skonto: 3,00% 10 Tage (bis 25.11.2024)".

### Open

1. **Title of the box.** The box of a payment term always has the title "Fälligkeit"
   (`PaymentTermsSection`). This also applies if the term has no due date, and holds
   only a text or a discount. A better title is "Zahlungsbedingungen".
2. **Due date in the Windows app.** The Windows app shows the due date (BT-9) only in
   the PDF. An overview of the invoices could show it in a column.
3. **Start day of the discount days.** If a discount has no start day (`BasisDate`),
   the PDF counts the days from the invoice date (BT-2). This is the rule of XRechnung.
   For a discount of CII EXTENDED without `BasisDateTime`, the PDF uses the same rule.
   Make sure that this agrees with the ZUGFeRD specification.
4. **Coverage report.** The content check does not look for the full text of a payment
   term that holds a `#SKONTO#` line, because the PDF shows the discount in a different
   form (`XmlValueExclusions`). The free text of such a term is on the page, but the
   check does not control it.
5. **Penalty of XRechnung.** XRechnung specifies no text form for a penalty
   (Verzugszinsen). If a sender writes one, for example `#VERZUG#...`, the PDF shows it
   as text.
