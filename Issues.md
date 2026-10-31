# Issues

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
