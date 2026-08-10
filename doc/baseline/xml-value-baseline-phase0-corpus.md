<!--
  Baseline of the migration to En16931, phase 0 — the samples and the corpus.
  Samples: FeRD 2.5.2 plus the CII part of the ZUGFeRD corpus, 101 XML documents.
  This is the configuration of testsettings.json. It is the wider net.
  See ../En16931-Migration.md.
-->

# XML value coverage report

This report tells you which values of the XML input are not on the rendered PDF.
The test case `XmlSample_AllValuesAreVisible` makes it.

- Made on: 2026-08-10 09:19:31
- Samples directory: `E:\_wrk\Projekte\E-Rechnungsdruck_AS400_PDF\FeRD Spezifikation\Version-2.5.2\Beispiele | E:\_wrk\Projekte\E-Rechnungsdruck_AS400_PDF\corpus`
- CLI tool: `C:\wrk\repos\Pacioli ERechnung\github\pacioli\src\Pacioli.CLI\bin\Debug\net10.0\pacioli-cmd.exe`
- XML samples: 101

The section [Each sample in detail](#each-sample-in-detail) names, for each
sample, the elements that are not on the page.

## Summary

| Result | Values | Part |
|---|---:|---:|
| Values in the XML input | 11012 | 100,0 % |
| On the page | 8204 | 74,5 % |
| Not rendered, by design | 2807 | 25,5 % |
| Known gap of the renderer | 1 | 0,0 % |
| Missing, without an entry (test failure) | 0 | 0,0 % |

## Values that are missing, and that have no entry

None. Each value is on the page, or an entry in `XmlValueExclusions.All` applies to it.

## Known gaps of the renderer

The renderer does not show 1 value(s), in 1 element(s).
Each line is a task for the renderer. If the renderer starts to show a value,
remove the entry from `XmlValueExclusions.All`.

| Values | Samples | Element | Reason |
|---:|---:|---|---|
| 1 | 1 | `/ReceivingAdviceReferencedDocument/IssuerAssignedID` | BT-15, the reference of the receiving advice. The page shows the despatch advice (BT-16) only. See TradeAgreementSection. |

### Examples

**`/ReceivingAdviceReferencedDocument/IssuerAssignedID`** — BT-15, the reference of the receiving advice. The page shows the despatch advice (BT-16) only. See TradeAgreementSection.

- `ghi` in corpus\XML-Rechnung\CII\not_validating_full_invoice_based_onTest_EeISI_300_CENfullmodel.cii.xml, line 289

## Elements that the page does not show, by design

The page does not show 2807 value(s), in 17 entry/entries. These values are codes and technical data.
The page shows their meaning in an other form, or it does not need them.

| Values | Samples | Entry | Reason |
|---:|---:|---|---|
| 692 | 101 | `TypeCode` | A code, for example "380" or "VAT". The page shows the text of the code. |
| 476 | 99 | `CategoryCode` | A code of a tax category, for example "AE". The page shows the text of the code. The category "S" is the usual rate, thus the page shows nothing for it. |
| 446 | 101 | `@schemeID` | The code of an ID scheme, for example "VA". |
| 441 | 97 | `@unitCode` | The code of a unit, for example "H87". The page shows the name of the unit. |
| 427 | 101 | `@format` | The format code of a date, for example "102". The page shows the date. |
| 101 | 101 | `/ExchangedDocumentContext/GuidelineSpecifiedDocumentContextParameter/ID` | The ID of the ZUGFeRD profile, for example "urn:factur-x.eu:1p0:minimum". |
| 74 | 21 | `Indicator` | A true or false value. The page shows a text, for example "Zuschlag". |
| 39 | 6 | `LineStatusReasonCode` | A code of a line status. |
| 25 | 8 | `ReferenceTypeCode` | A code of a reference type. |
| 20 | 15 | `@mimeCode` | The media type of an attachment. |
| 19 | 14 | `AttachmentBinaryObject` | The data of an attachment, in base64 form. |
| 17 | 17 | `/ExchangedDocumentContext/BusinessProcessSpecifiedDocumentContextParameter/ID` | The ID of the business process. |
| 8 | 2 | `ReasonCode` | A code of a reason. The page shows the text of the reason. |
| 7 | 2 | `@listID` | The ID of a code list. |
| 7 | 2 | `@listVersionID` | The version of a code list. |
| 7 | 2 | `ClassCode` | A code of a product classification. |
| 1 | 1 | `/ApplicableHeaderTradeSettlement/ApplicableTradeTax/ExemptionReasonCode` | A code of a VATEX exemption reason, for example "VATEX-EU-O". The tax table shows the text of the code. |

## Each sample

| Sample | Values | On the page | Not rendered | Known gap | Missing |
|---|---:|---:|---:|---:|---:|
| Beispiele\0. MINIMUM\MINIMUM_Buchungshilfe\MINIMUM_Buchungshilfe.xml | 18 | 13 | 5 | 0 | 0 |
| Beispiele\0. MINIMUM\MINIMUM_Rechnung\MINIMUM_Rechnung.xml | 18 | 13 | 5 | 0 | 0 |
| Beispiele\1. BASIC WL\BASIC-WL_Buchungshilfe\BASIC-WL_Buchungshilfe.xml | 39 | 31 | 8 | 0 | 0 |
| Beispiele\1. BASIC WL\BASIC-WL_Einfach\BASIC-WL_Einfach.xml | 39 | 31 | 8 | 0 | 0 |
| Beispiele\2. BASIC\B01_Einfach\B01_01_Einfach.xml | 58 | 43 | 15 | 0 | 0 |
| Beispiele\2. BASIC\B02_Rechnungskorrektur\B02_01_Rechnungskorrektur.xml | 94 | 68 | 26 | 0 | 0 |
| Beispiele\2. BASIC\B03_Taxifahrt\B03_01_Taxifahrt.xml | 60 | 45 | 15 | 0 | 0 |
| Beispiele\3. EN16931\E01_1_Teilrechnung\E01_01_1_Teilrechnung.xml | 139 | 102 | 37 | 0 | 0 |
| Beispiele\3. EN16931\E02_2_Teilrechnung\E02_01_2_Teilrechnung.xml | 64 | 51 | 13 | 0 | 0 |
| Beispiele\3. EN16931\E03_AbweichenderZahlungsempf\E03_01_AbweichenderZahlungsempf.xml | 88 | 68 | 20 | 0 | 0 |
| Beispiele\3. EN16931\E04_Betriebskostenabrechnung_NEU\E04_01_Betriebskostenabrechnung_NEU.xml | 534 | 440 | 94 | 0 | 0 |
| Beispiele\3. EN16931\E05_Einfach\E05_01_Einfach.xml | 79 | 60 | 19 | 0 | 0 |
| Beispiele\3. EN16931\E06_Einfach_DueDate\E06_01_Einfach_DueDate.xml | 80 | 60 | 20 | 0 | 0 |
| Beispiele\3. EN16931\E07_Einfach_negativePaymentDue\E07_01_Einfach_negativePaymentDue.xml | 78 | 59 | 19 | 0 | 0 |
| Beispiele\3. EN16931\E08_Elektron_Anlage_XML\E08_01_Elektron_Anlage_XML.xml | 84 | 66 | 18 | 0 | 0 |
| Beispiele\3. EN16931\E09_ElektronischeAdresse\E09_01_ElektronischeAdresse.xml | 111 | 83 | 28 | 0 | 0 |
| Beispiele\3. EN16931\E10_Gutschrift\E10_01_Gutschrift.xml | 85 | 63 | 22 | 0 | 0 |
| Beispiele\3. EN16931\E11_Haftpflichtversicherung_Versicherungssteuer\E11_01_Haftpflichtversicherung_Versicherungssteuer.xml | 73 | 56 | 17 | 0 | 0 |
| Beispiele\3. EN16931\E12_Innergemeinschaftliche_Lieferungen\E12_01_Innergemeinschaftliche_Lieferungen.xml | 106 | 78 | 28 | 0 | 0 |
| Beispiele\3. EN16931\E13_Kleinunternehmer_ohneUStId\E13_01_Kleinunternehmer_ohneUStId.xml | 63 | 48 | 15 | 0 | 0 |
| Beispiele\3. EN16931\E14_Kraftfahrversicherung\E14_01_Kraftfahrversicherung.xml | 72 | 57 | 15 | 0 | 0 |
| Beispiele\3. EN16931\E15_Miete\E15_01_Miete.xml | 105 | 78 | 27 | 0 | 0 |
| Beispiele\3. EN16931\E16_OEPNV\E16_01_OEPNV.xml | 63 | 51 | 12 | 0 | 0 |
| Beispiele\3. EN16931\E17_Photovoltaik\E17_01_Photovoltaik.xml | 74 | 57 | 17 | 0 | 0 |
| Beispiele\3. EN16931\E18_Physiotherapeut\E18_01_Physiotherapeut.xml | 73 | 56 | 17 | 0 | 0 |
| Beispiele\3. EN16931\E19_Rabatte\E19_01_Rabatte.xml | 135 | 98 | 37 | 0 | 0 |
| Beispiele\3. EN16931\E20_Rechnungskorrektur\E20_01_Rechnungskorrektur.xml | 106 | 75 | 31 | 0 | 0 |
| Beispiele\3. EN16931\E21_RechnungsUebertragung\E22_01_RechnungsUebertragung.xml | 108 | 80 | 28 | 0 | 0 |
| Beispiele\3. EN16931\E22_Reisekosten\E22_01_Reisekosten.xml | 98 | 76 | 22 | 0 | 0 |
| Beispiele\3. EN16931\E23_Sachversicherung_berechneter_Steuersatz\E23_01_Sachversicherung_berechneter_Steuersatz.xml | 53 | 42 | 11 | 0 | 0 |
| Beispiele\3. EN16931\E24_SEPA_Prenotification\E24_01_SEPA_Prenotification.xml | 81 | 61 | 20 | 0 | 0 |
| Beispiele\3. EN16931\E25_steuerbefreite_Auslandslieferung\E25_01_steuerbefreite_Auslandslieferung.xml | 89 | 71 | 18 | 0 | 0 |
| Beispiele\4. EXTENDED\X01_Dauermietrechnung_u_Nebenkostenabrechnung\X01_01_600700800900_Dauermietrechnung_unbefristet_.xml | 134 | 101 | 33 | 0 | 0 |
| Beispiele\4. EXTENDED\X01_Dauermietrechnung_u_Nebenkostenabrechnung\X01_02_600700801000_NK_Abrechnung_zur_Dauermietrechnung.xml | 162 | 119 | 43 | 0 | 0 |
| Beispiele\4. EXTENDED\X01_Dauermietrechnung_u_Nebenkostenabrechnung\X01_03_7600700800900_Dauermietrechnung_KorrekturRE_.xml | 101 | 79 | 22 | 0 | 0 |
| Beispiele\4. EXTENDED\X02_SubInvoiceLines_Buero_Material_Bsp3\X02_01_SubInvoiceLines_Buero_Material_Bsp3__.xml | 205 | 153 | 52 | 0 | 0 |
| Beispiele\4. EXTENDED\X03_Abschlagsrechnung_SubInvoiceLine_u_LV_Nr\X03_01_Abschlagsrechnung_SubInvoiceLine_u_LV_Nr.xml | 176 | 131 | 45 | 0 | 0 |
| Beispiele\4. EXTENDED\X04_Sammelrechnung_3_Bestellungen\X04_01_Sammelrechnung_3_Bestellungen.xml | 332 | 237 | 95 | 0 | 0 |
| Beispiele\4. EXTENDED\X05_Bau_Rechnungen Abschlag_SchlussRechnung\X05_01_einfach_Bau__1_Abschlagsrechnung.xml | 111 | 91 | 20 | 0 | 0 |
| Beispiele\4. EXTENDED\X05_Bau_Rechnungen Abschlag_SchlussRechnung\X05_02_einfach_Bau__2_Abschlagsrechnung.xml | 139 | 109 | 30 | 0 | 0 |
| Beispiele\4. EXTENDED\X05_Bau_Rechnungen Abschlag_SchlussRechnung\X05_03_einfach_Bau_Schlussrechnung_mit_Steuer_Zusammenstellung.xml | 181 | 133 | 48 | 0 | 0 |
| Beispiele\4. EXTENDED\X06_Fallschutz-Set_SubInvoiceLine_Bsp5\X06_01_Fallschutz-Set_SubInvoiceLine_Bsp5.xml | 161 | 116 | 45 | 0 | 0 |
| Beispiele\4. EXTENDED\X07_Fremdwaehrung\X07_01_Fremdwaehrung.xml | 144 | 109 | 35 | 0 | 0 |
| Beispiele\4. EXTENDED\X08_Herkunftsland_Zolltarif-Nummer__HS___UNSPSC__TST___eCl_ss__STQ\X08_01_Herkunftsland_Zolltarif-Nummer__HS___UNSPSC__TST___eCl_ss__STQ.xml | 316 | 212 | 104 | 0 | 0 |
| Beispiele\4. EXTENDED\X09_InnergemeinschLieferungMehrereBestellungen\X09_01_InnergemeinschLieferungMehrereBestellungen.xml | 124 | 94 | 30 | 0 | 0 |
| Beispiele\4. EXTENDED\X10_Kleinunternehmer_ohneUStId\X10_01_Kleinunternehmer_ohneUStId.xml | 69 | 54 | 15 | 0 | 0 |
| Beispiele\4. EXTENDED\X11_Kostenrechnung\X11_01_Kostenrechnung.xml | 170 | 130 | 40 | 0 | 0 |
| Beispiele\4. EXTENDED\X12_Maschinen-Serial_NR__BT-X-307\X12_01_Maschinen-Serial_NR__BT-X-307.xml | 104 | 82 | 22 | 0 | 0 |
| Beispiele\4. EXTENDED\X13_Projektabschlussrechnung\X13_01_Projektabschlussrechnung.xml | 118 | 83 | 35 | 0 | 0 |
| Beispiele\4. EXTENDED\X14_Rechnungskorrektur\X14_01_Rechnungskorrektur.xml | 153 | 112 | 41 | 0 | 0 |
| Beispiele\4. EXTENDED\X15_steuerfreie_Innergemeinschaftliche_Lieferung\X15_01_steuerfreie_Innergemeinschaftliche_Lieferung.xml | 173 | 123 | 50 | 0 | 0 |
| Beispiele\4. EXTENDED\X16_Stornogutschrift_zur_Rechnung\X16_01_Stornogutschrift_zu_RE.xml | 138 | 101 | 37 | 0 | 0 |
| Beispiele\4. EXTENDED\X16_Stornogutschrift_zur_Rechnung\X16_02_Rechnung_für_Storno_Gutschrift_.xml | 144 | 106 | 38 | 0 | 0 |
| Beispiele\4. EXTENDED\X17_SubInvoiceLines_Hardware_Bsp2\X17_01_SubInvoiceLines_Hardware_Bsp2.xml | 173 | 131 | 42 | 0 | 0 |
| Beispiele\4. EXTENDED\X18_Subinvoiceline Kaffee Bundle Set Bsp 4\X18_01_SubInvoiceLines_Kaffee_Bundle_Set_Bsp4___.xml | 177 | 136 | 41 | 0 | 0 |
| Beispiele\4. EXTENDED\X19_Warenrechnung\X19_01_Warenrechnung.xml | 258 | 188 | 70 | 0 | 0 |
| Beispiele\4. EXTENDED\X20_SubInvoiceLines_Buero_Material_Bsp3__\X20_01_SubInvoiceLines_Buero_Material_Bsp3__.xml | 205 | 153 | 52 | 0 | 0 |
| Beispiele\5. XRECHNUNG\XRECHNUNG_Betriebskostenabrechnung\XRECHNUNG_Betriebskostenabrechnung.xml | 77 | 58 | 19 | 0 | 0 |
| Beispiele\5. XRECHNUNG\XRECHNUNG_Einfach\XRECHNUNG_Einfach.xml | 89 | 65 | 24 | 0 | 0 |
| Beispiele\5. XRECHNUNG\XRECHNUNG_Elektron\XRECHNUNG_Elektron.xml | 91 | 66 | 25 | 0 | 0 |
| Beispiele\5. XRECHNUNG\XRECHNUNG_Reisekostenabrechnung\XRECHNUNG_Reisekostenabrechnung.xml | 125 | 89 | 36 | 0 | 0 |
| corpus\XML-Rechnung\CII\EN16931_1_Teilrechnung.cii.xml | 131 | 94 | 37 | 0 | 0 |
| corpus\XML-Rechnung\CII\EN16931_2_Teilrechnung.cii.xml | 59 | 46 | 13 | 0 | 0 |
| corpus\XML-Rechnung\CII\EN16931_AbweichenderZahlungsempf.cii.xml | 79 | 59 | 20 | 0 | 0 |
| corpus\XML-Rechnung\CII\EN16931_Betriebskostenabrechnung.cii.xml | 64 | 48 | 16 | 0 | 0 |
| corpus\XML-Rechnung\CII\EN16931_Einfach.cii.xml | 75 | 56 | 19 | 0 | 0 |
| corpus\XML-Rechnung\CII\EN16931_Einfach_DueDate.cii.xml | 76 | 56 | 20 | 0 | 0 |
| corpus\XML-Rechnung\CII\EN16931_Einfach_negativePaymentDue.cii.xml | 74 | 55 | 19 | 0 | 0 |
| corpus\XML-Rechnung\CII\EN16931_Elektron.cii.xml | 76 | 59 | 17 | 0 | 0 |
| corpus\XML-Rechnung\CII\EN16931_ElektronischeAdresse.cii.xml | 108 | 80 | 28 | 0 | 0 |
| corpus\XML-Rechnung\CII\EN16931_Gutschrift.cii.xml | 77 | 57 | 20 | 0 | 0 |
| corpus\XML-Rechnung\CII\EN16931_Haftpflichtversicherung_Versicherungssteuer.cii.xml | 60 | 47 | 13 | 0 | 0 |
| corpus\XML-Rechnung\CII\EN16931_Innergemeinschaftliche_Lieferungen.cii.xml | 106 | 78 | 28 | 0 | 0 |
| corpus\XML-Rechnung\CII\EN16931_Kraftfahrversicherung_Bruttopreise.cii.xml | 62 | 49 | 13 | 0 | 0 |
| corpus\XML-Rechnung\CII\EN16931_Miete.cii.xml | 105 | 78 | 27 | 0 | 0 |
| corpus\XML-Rechnung\CII\EN16931_OEPNV.cii.xml | 56 | 44 | 12 | 0 | 0 |
| corpus\XML-Rechnung\CII\EN16931_Physiotherapeut.cii.xml | 59 | 45 | 14 | 0 | 0 |
| corpus\XML-Rechnung\CII\EN16931_Rabatte.cii.xml | 129 | 92 | 37 | 0 | 0 |
| corpus\XML-Rechnung\CII\EN16931_Rechnungskorrektur.cii.xml | 106 | 75 | 31 | 0 | 0 |
| corpus\XML-Rechnung\CII\EN16931_RechnungsUebertragung.cii.xml | 108 | 80 | 28 | 0 | 0 |
| corpus\XML-Rechnung\CII\EN16931_Reisekostenabrechnung.cii.xml | 105 | 79 | 26 | 0 | 0 |
| corpus\XML-Rechnung\CII\EN16931_Sachversicherung_berechneter_Steuersatz.cii.xml | 53 | 42 | 11 | 0 | 0 |
| corpus\XML-Rechnung\CII\EN16931_SEPA_Prenotification.cii.xml | 81 | 61 | 20 | 0 | 0 |
| corpus\XML-Rechnung\CII\not_validating_full_invoice_based_onTest_EeISI_300_CENfullmodel.cii.xml | 223 | 161 | 61 | 1 | 0 |
| corpus\XML-Rechnung\CII\XRECHNUNG_Betriebskostenabrechnung.cii.xml | 84 | 61 | 23 | 0 | 0 |
| corpus\XML-Rechnung\CII\XRECHNUNG_Einfach.cii.xml | 89 | 66 | 23 | 0 | 0 |
| corpus\XML-Rechnung\CII\XRECHNUNG_Elektron.cii.xml | 93 | 68 | 25 | 0 | 0 |
| corpus\XML-Rechnung\CII\XRECHNUNG_Reisekostenabrechnung.cii.xml | 127 | 92 | 35 | 0 | 0 |
| corpus\ZUGFeRDv2\correct\Mustangproject\hate1.xml | 75 | 56 | 19 | 0 | 0 |
| corpus\ZUGFeRDv2\correct\Mustangproject\testout-XR-Edge.xml | 74 | 53 | 21 | 0 | 0 |
| corpus\ZUGFeRDv2\correct\Mustangproject\testout-XR.xml | 62 | 47 | 15 | 0 | 0 |
| corpus\ZUGFeRDv2\correct\Mustangproject\testout-ZF2PushAllowances.xml | 60 | 43 | 17 | 0 | 0 |
| corpus\ZUGFeRDv2\correct\Mustangproject\testout-ZF2PushChargesAllowances.xml | 92 | 63 | 29 | 0 | 0 |
| corpus\ZUGFeRDv2\correct\Mustangproject\testout-ZF2PushCorrection.xml | 86 | 60 | 26 | 0 | 0 |
| corpus\ZUGFeRDv2\correct\Mustangproject\testout-ZF2PushCreditNote.xml | 92 | 64 | 28 | 0 | 0 |
| corpus\ZUGFeRDv2\correct\Mustangproject\testout-ZF2PushEdge.xml | 88 | 63 | 25 | 0 | 0 |
| corpus\ZUGFeRDv2\correct\Mustangproject\testout-ZF2PushItemChargesAllowances.xml | 110 | 77 | 33 | 0 | 0 |
| corpus\ZUGFeRDv2\correct\Mustangproject\testout-ZF2PushRelativeChargesAllowances.xml | 90 | 63 | 27 | 0 | 0 |
| corpus\ZUGFeRDv2\correct\Mustangproject\testout-ZF2PushReverseCharge.xml | 105 | 76 | 29 | 0 | 0 |
| corpus\ZUGFeRDv2\correct\xml\ZUGFeRD-invoice_rabatte_3_abschlag_duepayableamount.xml | 48 | 35 | 13 | 0 | 0 |
| corpus\ZUGFeRDv2\correct\xml\ZUGFeRD-invoice_rabatte_4_abschlag_taxbasistotalamount.xml | 48 | 35 | 13 | 0 | 0 |

## Each sample in detail

The elements of a sample that are not on the page. A sample without a row
shows each of its values.

### Beispiele\0. MINIMUM\MINIMUM_Buchungshilfe\MINIMUM_Buchungshilfe.xml

13 of 18 values are on the page.

| Values | Kind | Element |
|---:|---|---|
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/SellerTradeParty/SpecifiedTaxRegistration/ID@schemeID` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocument/IssueDateTime/DateTimeString@format` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocument/TypeCode` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocumentContext/GuidelineSpecifiedDocumentContextParameter/ID` |

### Beispiele\0. MINIMUM\MINIMUM_Rechnung\MINIMUM_Rechnung.xml

13 of 18 values are on the page.

| Values | Kind | Element |
|---:|---|---|
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/SellerTradeParty/SpecifiedTaxRegistration/ID@schemeID` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocument/IssueDateTime/DateTimeString@format` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocument/TypeCode` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocumentContext/GuidelineSpecifiedDocumentContextParameter/ID` |

### Beispiele\1. BASIC WL\BASIC-WL_Buchungshilfe\BASIC-WL_Buchungshilfe.xml

31 of 39 values are on the page.

| Values | Kind | Element |
|---:|---|---|
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocument/IssueDateTime/DateTimeString@format` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocument/TypeCode` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocumentContext/GuidelineSpecifiedDocumentContextParameter/ID` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/SellerTradeParty/SpecifiedTaxRegistration/ID@schemeID` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeDelivery/ActualDeliverySupplyChainEvent/OccurrenceDateTime/DateTimeString@format` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/ApplicableTradeTax/CategoryCode` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/ApplicableTradeTax/TypeCode` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/SpecifiedTradePaymentTerms/DueDateDateTime/DateTimeString@format` |

### Beispiele\1. BASIC WL\BASIC-WL_Einfach\BASIC-WL_Einfach.xml

31 of 39 values are on the page.

| Values | Kind | Element |
|---:|---|---|
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocument/IssueDateTime/DateTimeString@format` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocument/TypeCode` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocumentContext/GuidelineSpecifiedDocumentContextParameter/ID` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/SellerTradeParty/SpecifiedTaxRegistration/ID@schemeID` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeDelivery/ActualDeliverySupplyChainEvent/OccurrenceDateTime/DateTimeString@format` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/ApplicableTradeTax/CategoryCode` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/ApplicableTradeTax/TypeCode` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/SpecifiedTradePaymentTerms/DueDateDateTime/DateTimeString@format` |

### Beispiele\2. BASIC\B01_Einfach\B01_01_Einfach.xml

43 of 58 values are on the page.

| Values | Kind | Element |
|---:|---|---|
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/SellerTradeParty/SpecifiedTaxRegistration/ID@schemeID` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocument/IssueDateTime/DateTimeString@format` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocument/TypeCode` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocumentContext/GuidelineSpecifiedDocumentContextParameter/ID` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/BuyerTradeParty/GlobalID@schemeID` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/SellerTradeParty/GlobalID@schemeID` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeDelivery/ActualDeliverySupplyChainEvent/OccurrenceDateTime/DateTimeString@format` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/ApplicableTradeTax/CategoryCode` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/ApplicableTradeTax/TypeCode` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/SpecifiedTradePaymentTerms/DueDateDateTime/DateTimeString@format` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeDelivery/BilledQuantity@unitCode` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeSettlement/ApplicableTradeTax/CategoryCode` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeSettlement/ApplicableTradeTax/TypeCode` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedTradeProduct/GlobalID@schemeID` |

### Beispiele\2. BASIC\B02_Rechnungskorrektur\B02_01_Rechnungskorrektur.xml

68 of 94 values are on the page.

| Values | Kind | Element |
|---:|---|---|
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/ApplicableTradeTax/CategoryCode` |
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/ApplicableTradeTax/TypeCode` |
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/SpecifiedTradeAllowanceCharge/CategoryTradeTax/CategoryCode` |
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/SpecifiedTradeAllowanceCharge/CategoryTradeTax/TypeCode` |
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/SpecifiedTradeAllowanceCharge/ChargeIndicator/Indicator` |
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeDelivery/BilledQuantity@unitCode` |
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeSettlement/ApplicableTradeTax/CategoryCode` |
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeSettlement/ApplicableTradeTax/TypeCode` |
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedTradeProduct/GlobalID@schemeID` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocument/IssueDateTime/DateTimeString@format` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocument/TypeCode` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocumentContext/GuidelineSpecifiedDocumentContextParameter/ID` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/BuyerTradeParty/GlobalID@schemeID` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/SellerTradeParty/GlobalID@schemeID` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/SellerTradeParty/SpecifiedTaxRegistration/ID@schemeID` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeDelivery/ActualDeliverySupplyChainEvent/OccurrenceDateTime/DateTimeString@format` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeDelivery/ShipToTradeParty/GlobalID@schemeID` |

### Beispiele\2. BASIC\B03_Taxifahrt\B03_01_Taxifahrt.xml

45 of 60 values are on the page.

| Values | Kind | Element |
|---:|---|---|
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeDelivery/BilledQuantity@unitCode` |
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeSettlement/ApplicableTradeTax/CategoryCode` |
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeSettlement/ApplicableTradeTax/TypeCode` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocument/IssueDateTime/DateTimeString@format` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocument/TypeCode` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocumentContext/GuidelineSpecifiedDocumentContextParameter/ID` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/SellerTradeParty/GlobalID@schemeID` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/SellerTradeParty/SpecifiedTaxRegistration/ID@schemeID` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeDelivery/ActualDeliverySupplyChainEvent/OccurrenceDateTime/DateTimeString@format` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/ApplicableTradeTax/CategoryCode` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/ApplicableTradeTax/TypeCode` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/SpecifiedTradePaymentTerms/DueDateDateTime/DateTimeString@format` |

### Beispiele\3. EN16931\E01_1_Teilrechnung\E01_01_1_Teilrechnung.xml

102 of 139 values are on the page.

| Values | Kind | Element |
|---:|---|---|
| 4 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeDelivery/BilledQuantity@unitCode` |
| 4 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeSettlement/ApplicableTradeTax/CategoryCode` |
| 4 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeSettlement/ApplicableTradeTax/TypeCode` |
| 4 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedTradeProduct/GlobalID@schemeID` |
| 3 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/SpecifiedTradeAllowanceCharge/CategoryTradeTax/CategoryCode` |
| 3 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/SpecifiedTradeAllowanceCharge/CategoryTradeTax/TypeCode` |
| 3 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/SpecifiedTradeAllowanceCharge/ChargeIndicator/Indicator` |
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/SellerTradeParty/SpecifiedTaxRegistration/ID@schemeID` |
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/ApplicableTradeTax/CategoryCode` |
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/ApplicableTradeTax/TypeCode` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocument/IssueDateTime/DateTimeString@format` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocument/TypeCode` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocumentContext/GuidelineSpecifiedDocumentContextParameter/ID` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/SellerTradeParty/GlobalID@schemeID` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeDelivery/ActualDeliverySupplyChainEvent/OccurrenceDateTime/DateTimeString@format` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeAgreement/GrossPriceProductTradePrice/AppliedTradeAllowanceCharge/ChargeIndicator/Indicator` |

### Beispiele\3. EN16931\E02_2_Teilrechnung\E02_01_2_Teilrechnung.xml

51 of 64 values are on the page.

| Values | Kind | Element |
|---:|---|---|
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/SellerTradeParty/SpecifiedTaxRegistration/ID@schemeID` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocument/IssueDateTime/DateTimeString@format` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocument/TypeCode` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocumentContext/GuidelineSpecifiedDocumentContextParameter/ID` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/SellerTradeParty/GlobalID@schemeID` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeDelivery/ActualDeliverySupplyChainEvent/OccurrenceDateTime/DateTimeString@format` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/ApplicableTradeTax/CategoryCode` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/ApplicableTradeTax/TypeCode` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeDelivery/BilledQuantity@unitCode` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeSettlement/ApplicableTradeTax/CategoryCode` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeSettlement/ApplicableTradeTax/TypeCode` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedTradeProduct/GlobalID@schemeID` |

### Beispiele\3. EN16931\E03_AbweichenderZahlungsempf\E03_01_AbweichenderZahlungsempf.xml

68 of 88 values are on the page.

| Values | Kind | Element |
|---:|---|---|
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/SellerTradeParty/SpecifiedTaxRegistration/ID@schemeID` |
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/ApplicableTradeTax/CategoryCode` |
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/ApplicableTradeTax/TypeCode` |
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeDelivery/BilledQuantity@unitCode` |
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeSettlement/ApplicableTradeTax/CategoryCode` |
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeSettlement/ApplicableTradeTax/TypeCode` |
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedTradeProduct/GlobalID@schemeID` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocument/IssueDateTime/DateTimeString@format` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocument/TypeCode` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocumentContext/GuidelineSpecifiedDocumentContextParameter/ID` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/SellerTradeParty/GlobalID@schemeID` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeDelivery/ActualDeliverySupplyChainEvent/OccurrenceDateTime/DateTimeString@format` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/SpecifiedTradeSettlementPaymentMeans/TypeCode` |

### Beispiele\3. EN16931\E04_Betriebskostenabrechnung_NEU\E04_01_Betriebskostenabrechnung_NEU.xml

440 of 534 values are on the page.

| Values | Kind | Element |
|---:|---|---|
| 27 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeDelivery/BilledQuantity@unitCode` |
| 27 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeSettlement/ApplicableTradeTax/CategoryCode` |
| 27 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeSettlement/ApplicableTradeTax/TypeCode` |
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/SellerTradeParty/SpecifiedTaxRegistration/ID@schemeID` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocument/IssueDateTime/DateTimeString@format` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocument/TypeCode` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocumentContext/GuidelineSpecifiedDocumentContextParameter/ID` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/AdditionalReferencedDocument/AttachmentBinaryObject` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/AdditionalReferencedDocument/AttachmentBinaryObject@mimeCode` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/AdditionalReferencedDocument/TypeCode` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/ApplicableTradeTax/CategoryCode` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/ApplicableTradeTax/TypeCode` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/BillingSpecifiedPeriod/EndDateTime/DateTimeString@format` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/BillingSpecifiedPeriod/StartDateTime/DateTimeString@format` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/SpecifiedTradePaymentTerms/DueDateDateTime/DateTimeString@format` |

### Beispiele\3. EN16931\E05_Einfach\E05_01_Einfach.xml

60 of 79 values are on the page.

| Values | Kind | Element |
|---:|---|---|
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/SellerTradeParty/SpecifiedTaxRegistration/ID@schemeID` |
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/ApplicableTradeTax/CategoryCode` |
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/ApplicableTradeTax/TypeCode` |
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeDelivery/BilledQuantity@unitCode` |
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeSettlement/ApplicableTradeTax/CategoryCode` |
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeSettlement/ApplicableTradeTax/TypeCode` |
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedTradeProduct/GlobalID@schemeID` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocument/IssueDateTime/DateTimeString@format` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocument/TypeCode` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocumentContext/GuidelineSpecifiedDocumentContextParameter/ID` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/SellerTradeParty/GlobalID@schemeID` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeDelivery/ActualDeliverySupplyChainEvent/OccurrenceDateTime/DateTimeString@format` |

### Beispiele\3. EN16931\E06_Einfach_DueDate\E06_01_Einfach_DueDate.xml

60 of 80 values are on the page.

| Values | Kind | Element |
|---:|---|---|
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/SellerTradeParty/SpecifiedTaxRegistration/ID@schemeID` |
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/ApplicableTradeTax/CategoryCode` |
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/ApplicableTradeTax/TypeCode` |
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeDelivery/BilledQuantity@unitCode` |
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeSettlement/ApplicableTradeTax/CategoryCode` |
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeSettlement/ApplicableTradeTax/TypeCode` |
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedTradeProduct/GlobalID@schemeID` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocument/IssueDateTime/DateTimeString@format` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocument/TypeCode` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocumentContext/GuidelineSpecifiedDocumentContextParameter/ID` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/SellerTradeParty/GlobalID@schemeID` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeDelivery/ActualDeliverySupplyChainEvent/OccurrenceDateTime/DateTimeString@format` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/SpecifiedTradePaymentTerms/DueDateDateTime/DateTimeString@format` |

### Beispiele\3. EN16931\E07_Einfach_negativePaymentDue\E07_01_Einfach_negativePaymentDue.xml

59 of 78 values are on the page.

| Values | Kind | Element |
|---:|---|---|
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/SellerTradeParty/SpecifiedTaxRegistration/ID@schemeID` |
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/ApplicableTradeTax/CategoryCode` |
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/ApplicableTradeTax/TypeCode` |
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeDelivery/BilledQuantity@unitCode` |
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeSettlement/ApplicableTradeTax/CategoryCode` |
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeSettlement/ApplicableTradeTax/TypeCode` |
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedTradeProduct/GlobalID@schemeID` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocument/IssueDateTime/DateTimeString@format` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocument/TypeCode` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocumentContext/GuidelineSpecifiedDocumentContextParameter/ID` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/SellerTradeParty/GlobalID@schemeID` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeDelivery/ActualDeliverySupplyChainEvent/OccurrenceDateTime/DateTimeString@format` |

### Beispiele\3. EN16931\E08_Elektron_Anlage_XML\E08_01_Elektron_Anlage_XML.xml

66 of 84 values are on the page.

| Values | Kind | Element |
|---:|---|---|
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeDelivery/BilledQuantity@unitCode` |
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeSettlement/ApplicableTradeTax/CategoryCode` |
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeSettlement/ApplicableTradeTax/TypeCode` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocument/IssueDateTime/DateTimeString@format` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocument/TypeCode` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocumentContext/BusinessProcessSpecifiedDocumentContextParameter/ID` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocumentContext/GuidelineSpecifiedDocumentContextParameter/ID` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/AdditionalReferencedDocument/AttachmentBinaryObject` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/AdditionalReferencedDocument/AttachmentBinaryObject@mimeCode` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/AdditionalReferencedDocument/TypeCode` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/SellerTradeParty/SpecifiedTaxRegistration/ID@schemeID` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeDelivery/ActualDeliverySupplyChainEvent/OccurrenceDateTime/DateTimeString@format` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/ApplicableTradeTax/CategoryCode` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/ApplicableTradeTax/TypeCode` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/SpecifiedTradeSettlementPaymentMeans/TypeCode` |

### Beispiele\3. EN16931\E09_ElektronischeAdresse\E09_01_ElektronischeAdresse.xml

83 of 111 values are on the page.

| Values | Kind | Element |
|---:|---|---|
| 6 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeDelivery/BilledQuantity@unitCode` |
| 6 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeSettlement/ApplicableTradeTax/CategoryCode` |
| 6 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeSettlement/ApplicableTradeTax/TypeCode` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocument/IssueDateTime/DateTimeString@format` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocument/TypeCode` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocumentContext/GuidelineSpecifiedDocumentContextParameter/ID` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/BuyerTradeParty/URIUniversalCommunication/URIID@schemeID` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/SellerTradeParty/GlobalID@schemeID` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/SellerTradeParty/SpecifiedTaxRegistration/ID@schemeID` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/SellerTradeParty/URIUniversalCommunication/URIID@schemeID` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeDelivery/ActualDeliverySupplyChainEvent/OccurrenceDateTime/DateTimeString@format` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/ApplicableTradeTax/CategoryCode` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/ApplicableTradeTax/TypeCode` |

### Beispiele\3. EN16931\E10_Gutschrift\E10_01_Gutschrift.xml

63 of 85 values are on the page.

| Values | Kind | Element |
|---:|---|---|
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/SellerTradeParty/SpecifiedTaxRegistration/ID@schemeID` |
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/ApplicableTradeTax/CategoryCode` |
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/ApplicableTradeTax/TypeCode` |
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeDelivery/BilledQuantity@unitCode` |
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeSettlement/ApplicableTradeTax/CategoryCode` |
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeSettlement/ApplicableTradeTax/TypeCode` |
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedTradeProduct/GlobalID@schemeID` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocument/IssueDateTime/DateTimeString@format` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocument/TypeCode` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocumentContext/GuidelineSpecifiedDocumentContextParameter/ID` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/BuyerTradeParty/SpecifiedTaxRegistration/ID@schemeID` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/SellerTradeParty/GlobalID@schemeID` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeDelivery/ActualDeliverySupplyChainEvent/OccurrenceDateTime/DateTimeString@format` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/SpecifiedTradePaymentTerms/DueDateDateTime/DateTimeString@format` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/SpecifiedTradeSettlementPaymentMeans/TypeCode` |

### Beispiele\3. EN16931\E11_Haftpflichtversicherung_Versicherungssteuer\E11_01_Haftpflichtversicherung_Versicherungssteuer.xml

56 of 73 values are on the page.

| Values | Kind | Element |
|---:|---|---|
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeDelivery/BilledQuantity@unitCode` |
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeSettlement/ApplicableTradeTax/CategoryCode` |
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeSettlement/ApplicableTradeTax/TypeCode` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocument/IssueDateTime/DateTimeString@format` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocument/TypeCode` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocumentContext/GuidelineSpecifiedDocumentContextParameter/ID` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/SellerTradeParty/SpecifiedTaxRegistration/ID@schemeID` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/ApplicableTradeTax/CategoryCode` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/ApplicableTradeTax/TypeCode` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/BillingSpecifiedPeriod/EndDateTime/DateTimeString@format` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/BillingSpecifiedPeriod/StartDateTime/DateTimeString@format` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/SpecifiedTradePaymentTerms/DueDateDateTime/DateTimeString@format` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/SpecifiedTradeSettlementPaymentMeans/TypeCode` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeSettlement/SpecifiedTradeAllowanceCharge/ChargeIndicator/Indicator` |

### Beispiele\3. EN16931\E12_Innergemeinschaftliche_Lieferungen\E12_01_Innergemeinschaftliche_Lieferungen.xml

78 of 106 values are on the page.

| Values | Kind | Element |
|---:|---|---|
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeAgreement/GrossPriceProductTradePrice/BasisQuantity@unitCode` |
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeAgreement/NetPriceProductTradePrice/BasisQuantity@unitCode` |
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeDelivery/BilledQuantity@unitCode` |
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeSettlement/ApplicableTradeTax/CategoryCode` |
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeSettlement/ApplicableTradeTax/TypeCode` |
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeSettlement/BillingSpecifiedPeriod/EndDateTime/DateTimeString@format` |
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeSettlement/BillingSpecifiedPeriod/StartDateTime/DateTimeString@format` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocument/IssueDateTime/DateTimeString@format` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocument/TypeCode` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocumentContext/BusinessProcessSpecifiedDocumentContextParameter/ID` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocumentContext/GuidelineSpecifiedDocumentContextParameter/ID` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/BuyerTradeParty/SpecifiedTaxRegistration/ID@schemeID` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/SellerTaxRepresentativeTradeParty/SpecifiedTaxRegistration/ID@schemeID` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/SellerTradeParty/SpecifiedTaxRegistration/ID@schemeID` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/ApplicableTradeTax/CategoryCode` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/ApplicableTradeTax/TypeCode` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/BillingSpecifiedPeriod/EndDateTime/DateTimeString@format` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/BillingSpecifiedPeriod/StartDateTime/DateTimeString@format` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/PayeeTradeParty/GlobalID@schemeID` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/SpecifiedTradePaymentTerms/DueDateDateTime/DateTimeString@format` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/SpecifiedTradeSettlementPaymentMeans/TypeCode` |

### Beispiele\3. EN16931\E13_Kleinunternehmer_ohneUStId\E13_01_Kleinunternehmer_ohneUStId.xml

48 of 63 values are on the page.

| Values | Kind | Element |
|---:|---|---|
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocument/IssueDateTime/DateTimeString@format` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocument/TypeCode` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocumentContext/GuidelineSpecifiedDocumentContextParameter/ID` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/BuyerTradeParty/URIUniversalCommunication/URIID@schemeID` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/SellerTradeParty/SpecifiedTaxRegistration/ID@schemeID` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/SellerTradeParty/URIUniversalCommunication/URIID@schemeID` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeDelivery/ActualDeliverySupplyChainEvent/OccurrenceDateTime/DateTimeString@format` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/ApplicableTradeTax/CategoryCode` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/ApplicableTradeTax/TypeCode` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/SpecifiedTradePaymentTerms/DueDateDateTime/DateTimeString@format` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/SpecifiedTradeSettlementPaymentMeans/TypeCode` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeAgreement/NetPriceProductTradePrice/BasisQuantity@unitCode` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeDelivery/BilledQuantity@unitCode` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeSettlement/ApplicableTradeTax/CategoryCode` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeSettlement/ApplicableTradeTax/TypeCode` |

### Beispiele\3. EN16931\E14_Kraftfahrversicherung\E14_01_Kraftfahrversicherung.xml

57 of 72 values are on the page.

| Values | Kind | Element |
|---:|---|---|
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeDelivery/BilledQuantity@unitCode` |
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeSettlement/ApplicableTradeTax/CategoryCode` |
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeSettlement/ApplicableTradeTax/TypeCode` |
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeSettlement/SpecifiedTradeAllowanceCharge/ChargeIndicator/Indicator` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocument/IssueDateTime/DateTimeString@format` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocument/TypeCode` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocumentContext/GuidelineSpecifiedDocumentContextParameter/ID` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/SellerTradeParty/SpecifiedTaxRegistration/ID@schemeID` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeDelivery/ActualDeliverySupplyChainEvent/OccurrenceDateTime/DateTimeString@format` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/ApplicableTradeTax/CategoryCode` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/ApplicableTradeTax/TypeCode` |

### Beispiele\3. EN16931\E15_Miete\E15_01_Miete.xml

78 of 105 values are on the page.

| Values | Kind | Element |
|---:|---|---|
| 6 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeDelivery/BilledQuantity@unitCode` |
| 6 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeSettlement/ApplicableTradeTax/CategoryCode` |
| 6 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeSettlement/ApplicableTradeTax/TypeCode` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocument/IssueDateTime/DateTimeString@format` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocument/TypeCode` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocumentContext/BusinessProcessSpecifiedDocumentContextParameter/ID` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocumentContext/GuidelineSpecifiedDocumentContextParameter/ID` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/SellerTradeParty/GlobalID@schemeID` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/SellerTradeParty/SpecifiedTaxRegistration/ID@schemeID` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeDelivery/ActualDeliverySupplyChainEvent/OccurrenceDateTime/DateTimeString@format` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/ApplicableTradeTax/CategoryCode` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/ApplicableTradeTax/TypeCode` |

### Beispiele\3. EN16931\E16_OEPNV\E16_01_OEPNV.xml

51 of 63 values are on the page.

| Values | Kind | Element |
|---:|---|---|
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocument/IssueDateTime/DateTimeString@format` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocument/TypeCode` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocumentContext/GuidelineSpecifiedDocumentContextParameter/ID` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/SellerTradeParty/SpecifiedTaxRegistration/ID@schemeID` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeDelivery/ActualDeliverySupplyChainEvent/OccurrenceDateTime/DateTimeString@format` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/ApplicableTradeTax/CategoryCode` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/ApplicableTradeTax/TypeCode` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/SpecifiedTradePaymentTerms/DueDateDateTime/DateTimeString@format` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/SpecifiedTradeSettlementPaymentMeans/TypeCode` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeDelivery/BilledQuantity@unitCode` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeSettlement/ApplicableTradeTax/CategoryCode` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeSettlement/ApplicableTradeTax/TypeCode` |

### Beispiele\3. EN16931\E17_Photovoltaik\E17_01_Photovoltaik.xml

57 of 74 values are on the page.

| Values | Kind | Element |
|---:|---|---|
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/SellerTradeParty/SpecifiedTaxRegistration/ID@schemeID` |
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeDelivery/BilledQuantity@unitCode` |
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeSettlement/ApplicableTradeTax/CategoryCode` |
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeSettlement/ApplicableTradeTax/TypeCode` |
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedTradeProduct/GlobalID@schemeID` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocument/IssueDateTime/DateTimeString@format` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocument/TypeCode` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocumentContext/GuidelineSpecifiedDocumentContextParameter/ID` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/SellerTradeParty/GlobalID@schemeID` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeDelivery/ActualDeliverySupplyChainEvent/OccurrenceDateTime/DateTimeString@format` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/ApplicableTradeTax/CategoryCode` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/ApplicableTradeTax/TypeCode` |

### Beispiele\3. EN16931\E18_Physiotherapeut\E18_01_Physiotherapeut.xml

56 of 73 values are on the page.

| Values | Kind | Element |
|---:|---|---|
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeDelivery/BilledQuantity@unitCode` |
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeSettlement/ApplicableTradeTax/CategoryCode` |
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeSettlement/ApplicableTradeTax/TypeCode` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocument/IssueDateTime/DateTimeString@format` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocument/TypeCode` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocumentContext/GuidelineSpecifiedDocumentContextParameter/ID` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/SellerTradeParty/SpecifiedTaxRegistration/ID@schemeID` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeDelivery/ActualDeliverySupplyChainEvent/OccurrenceDateTime/DateTimeString@format` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/ApplicableTradeTax/CategoryCode` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/ApplicableTradeTax/TypeCode` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/BillingSpecifiedPeriod/EndDateTime/DateTimeString@format` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/BillingSpecifiedPeriod/StartDateTime/DateTimeString@format` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/SpecifiedTradePaymentTerms/DueDateDateTime/DateTimeString@format` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/SpecifiedTradeSettlementPaymentMeans/TypeCode` |

### Beispiele\3. EN16931\E19_Rabatte\E19_01_Rabatte.xml

98 of 135 values are on the page.

| Values | Kind | Element |
|---:|---|---|
| 4 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeDelivery/BilledQuantity@unitCode` |
| 4 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeSettlement/ApplicableTradeTax/CategoryCode` |
| 4 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeSettlement/ApplicableTradeTax/TypeCode` |
| 4 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedTradeProduct/GlobalID@schemeID` |
| 3 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/SpecifiedTradeAllowanceCharge/CategoryTradeTax/CategoryCode` |
| 3 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/SpecifiedTradeAllowanceCharge/CategoryTradeTax/TypeCode` |
| 3 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/SpecifiedTradeAllowanceCharge/ChargeIndicator/Indicator` |
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/SellerTradeParty/SpecifiedTaxRegistration/ID@schemeID` |
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/ApplicableTradeTax/CategoryCode` |
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/ApplicableTradeTax/TypeCode` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocument/IssueDateTime/DateTimeString@format` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocument/TypeCode` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocumentContext/GuidelineSpecifiedDocumentContextParameter/ID` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/SellerTradeParty/GlobalID@schemeID` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeDelivery/ActualDeliverySupplyChainEvent/OccurrenceDateTime/DateTimeString@format` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeAgreement/GrossPriceProductTradePrice/AppliedTradeAllowanceCharge/ChargeIndicator/Indicator` |

### Beispiele\3. EN16931\E20_Rechnungskorrektur\E20_01_Rechnungskorrektur.xml

75 of 106 values are on the page.

| Values | Kind | Element |
|---:|---|---|
| 4 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/SpecifiedTradeAllowanceCharge/CategoryTradeTax/CategoryCode` |
| 4 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/SpecifiedTradeAllowanceCharge/CategoryTradeTax/TypeCode` |
| 4 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/SpecifiedTradeAllowanceCharge/ChargeIndicator/Indicator` |
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/ApplicableTradeTax/CategoryCode` |
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/ApplicableTradeTax/TypeCode` |
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeDelivery/BilledQuantity@unitCode` |
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeSettlement/ApplicableTradeTax/CategoryCode` |
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeSettlement/ApplicableTradeTax/TypeCode` |
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedTradeProduct/GlobalID@schemeID` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocument/IssueDateTime/DateTimeString@format` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocument/TypeCode` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocumentContext/GuidelineSpecifiedDocumentContextParameter/ID` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/SellerTradeParty/GlobalID@schemeID` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/SellerTradeParty/SpecifiedTaxRegistration/ID@schemeID` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeDelivery/ActualDeliverySupplyChainEvent/OccurrenceDateTime/DateTimeString@format` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeAgreement/GrossPriceProductTradePrice/AppliedTradeAllowanceCharge/ChargeIndicator/Indicator` |

### Beispiele\3. EN16931\E21_RechnungsUebertragung\E22_01_RechnungsUebertragung.xml

80 of 108 values are on the page.

| Values | Kind | Element |
|---:|---|---|
| 6 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeDelivery/BilledQuantity@unitCode` |
| 6 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeSettlement/ApplicableTradeTax/CategoryCode` |
| 6 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeSettlement/ApplicableTradeTax/TypeCode` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocument/IssueDateTime/DateTimeString@format` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocument/TypeCode` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocumentContext/GuidelineSpecifiedDocumentContextParameter/ID` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/BuyerTradeParty/URIUniversalCommunication/URIID@schemeID` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/SellerTradeParty/GlobalID@schemeID` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/SellerTradeParty/SpecifiedTaxRegistration/ID@schemeID` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/SellerTradeParty/URIUniversalCommunication/URIID@schemeID` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeDelivery/ActualDeliverySupplyChainEvent/OccurrenceDateTime/DateTimeString@format` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/ApplicableTradeTax/CategoryCode` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/ApplicableTradeTax/TypeCode` |

### Beispiele\3. EN16931\E22_Reisekosten\E22_01_Reisekosten.xml

76 of 98 values are on the page.

| Values | Kind | Element |
|---:|---|---|
| 4 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeDelivery/BilledQuantity@unitCode` |
| 4 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeSettlement/ApplicableTradeTax/CategoryCode` |
| 4 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeSettlement/ApplicableTradeTax/TypeCode` |
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/SellerTradeParty/SpecifiedTaxRegistration/ID@schemeID` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocument/IssueDateTime/DateTimeString@format` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocument/TypeCode` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocumentContext/GuidelineSpecifiedDocumentContextParameter/ID` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/SellerTradeParty/GlobalID@schemeID` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/ApplicableTradeTax/CategoryCode` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/ApplicableTradeTax/TypeCode` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/BillingSpecifiedPeriod/EndDateTime/DateTimeString@format` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/BillingSpecifiedPeriod/StartDateTime/DateTimeString@format` |

### Beispiele\3. EN16931\E23_Sachversicherung_berechneter_Steuersatz\E23_01_Sachversicherung_berechneter_Steuersatz.xml

42 of 53 values are on the page.

| Values | Kind | Element |
|---:|---|---|
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocument/IssueDateTime/DateTimeString@format` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocument/TypeCode` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocumentContext/GuidelineSpecifiedDocumentContextParameter/ID` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/SellerTradeParty/GlobalID@schemeID` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/SellerTradeParty/SpecifiedTaxRegistration/ID@schemeID` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeDelivery/ActualDeliverySupplyChainEvent/OccurrenceDateTime/DateTimeString@format` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/ApplicableTradeTax/CategoryCode` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/ApplicableTradeTax/TypeCode` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeDelivery/BilledQuantity@unitCode` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeSettlement/ApplicableTradeTax/CategoryCode` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeSettlement/ApplicableTradeTax/TypeCode` |

### Beispiele\3. EN16931\E24_SEPA_Prenotification\E24_01_SEPA_Prenotification.xml

61 of 81 values are on the page.

| Values | Kind | Element |
|---:|---|---|
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/SellerTradeParty/SpecifiedTaxRegistration/ID@schemeID` |
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/ApplicableTradeTax/CategoryCode` |
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/ApplicableTradeTax/TypeCode` |
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeDelivery/BilledQuantity@unitCode` |
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeSettlement/ApplicableTradeTax/CategoryCode` |
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeSettlement/ApplicableTradeTax/TypeCode` |
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedTradeProduct/GlobalID@schemeID` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocument/IssueDateTime/DateTimeString@format` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocument/TypeCode` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocumentContext/GuidelineSpecifiedDocumentContextParameter/ID` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/SellerTradeParty/GlobalID@schemeID` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeDelivery/ActualDeliverySupplyChainEvent/OccurrenceDateTime/DateTimeString@format` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/SpecifiedTradeSettlementPaymentMeans/TypeCode` |

### Beispiele\3. EN16931\E25_steuerbefreite_Auslandslieferung\E25_01_steuerbefreite_Auslandslieferung.xml

71 of 89 values are on the page.

| Values | Kind | Element |
|---:|---|---|
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/SellerTradeParty/SpecifiedTaxRegistration/ID@schemeID` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocument/IssueDateTime/DateTimeString@format` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocument/TypeCode` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocumentContext/GuidelineSpecifiedDocumentContextParameter/ID` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/BuyerTradeParty/SpecifiedTaxRegistration/ID@schemeID` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/SellerTradeParty/URIUniversalCommunication/URIID@schemeID` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/ApplicableTradeTax/CategoryCode` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/ApplicableTradeTax/TypeCode` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/SpecifiedTradePaymentTerms/DueDateDateTime/DateTimeString@format` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/SpecifiedTradeSettlementPaymentMeans/TypeCode` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeAgreement/GrossPriceProductTradePrice/BasisQuantity@unitCode` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeAgreement/NetPriceProductTradePrice/BasisQuantity@unitCode` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeDelivery/BilledQuantity@unitCode` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeSettlement/ApplicableTradeTax/CategoryCode` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeSettlement/ApplicableTradeTax/TypeCode` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeSettlement/BillingSpecifiedPeriod/EndDateTime/DateTimeString@format` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedTradeProduct/GlobalID@schemeID` |

### Beispiele\4. EXTENDED\X01_Dauermietrechnung_u_Nebenkostenabrechnung\X01_01_600700800900_Dauermietrechnung_unbefristet_.xml

101 of 134 values are on the page.

| Values | Kind | Element |
|---:|---|---|
| 3 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeAgreement/BuyerOrderReferencedDocument/FormattedIssueDateTime/DateTimeString@format` |
| 3 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeAgreement/NetPriceProductTradePrice/BasisQuantity@unitCode` |
| 3 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeDelivery/BilledQuantity@unitCode` |
| 3 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeSettlement/ApplicableTradeTax/CategoryCode` |
| 3 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeSettlement/ApplicableTradeTax/TypeCode` |
| 3 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeSettlement/BillingSpecifiedPeriod/StartDateTime/DateTimeString@format` |
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/SellerTradeParty/SpecifiedTaxRegistration/ID@schemeID` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocument/IssueDateTime/DateTimeString@format` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocument/TypeCode` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocumentContext/GuidelineSpecifiedDocumentContextParameter/ID` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/AdditionalReferencedDocument/AttachmentBinaryObject` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/AdditionalReferencedDocument/AttachmentBinaryObject@mimeCode` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/AdditionalReferencedDocument/TypeCode` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/SellerOrderReferencedDocument/FormattedIssueDateTime/DateTimeString@format` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/SellerTradeParty/GlobalID@schemeID` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/SellerTradeParty/URIUniversalCommunication/URIID@schemeID` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/ApplicableTradeTax/CategoryCode` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/ApplicableTradeTax/TypeCode` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/SpecifiedTradePaymentTerms/DueDateDateTime/DateTimeString@format` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/SpecifiedTradeSettlementPaymentMeans/TypeCode` |

### Beispiele\4. EXTENDED\X01_Dauermietrechnung_u_Nebenkostenabrechnung\X01_02_600700801000_NK_Abrechnung_zur_Dauermietrechnung.xml

119 of 162 values are on the page.

| Values | Kind | Element |
|---:|---|---|
| 4 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeAgreement/BuyerOrderReferencedDocument/FormattedIssueDateTime/DateTimeString@format` |
| 4 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeAgreement/NetPriceProductTradePrice/BasisQuantity@unitCode` |
| 4 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeDelivery/BilledQuantity@unitCode` |
| 4 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeSettlement/ApplicableTradeTax/CategoryCode` |
| 4 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeSettlement/ApplicableTradeTax/TypeCode` |
| 4 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeSettlement/BillingSpecifiedPeriod/EndDateTime/DateTimeString@format` |
| 4 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeSettlement/BillingSpecifiedPeriod/StartDateTime/DateTimeString@format` |
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/SellerTradeParty/SpecifiedTaxRegistration/ID@schemeID` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocument/IssueDateTime/DateTimeString@format` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocument/TypeCode` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocumentContext/GuidelineSpecifiedDocumentContextParameter/ID` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/AdditionalReferencedDocument/AttachmentBinaryObject` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/AdditionalReferencedDocument/AttachmentBinaryObject@mimeCode` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/AdditionalReferencedDocument/TypeCode` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/SellerOrderReferencedDocument/FormattedIssueDateTime/DateTimeString@format` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/SellerTradeParty/GlobalID@schemeID` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/SellerTradeParty/URIUniversalCommunication/URIID@schemeID` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/ApplicableTradeTax/CategoryCode` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/ApplicableTradeTax/TypeCode` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/SpecifiedTradePaymentTerms/DueDateDateTime/DateTimeString@format` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/SpecifiedTradeSettlementPaymentMeans/TypeCode` |

### Beispiele\4. EXTENDED\X01_Dauermietrechnung_u_Nebenkostenabrechnung\X01_03_7600700800900_Dauermietrechnung_KorrekturRE_.xml

79 of 101 values are on the page.

| Values | Kind | Element |
|---:|---|---|
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/SellerTradeParty/SpecifiedTaxRegistration/ID@schemeID` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocument/IssueDateTime/DateTimeString@format` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocument/TypeCode` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocumentContext/GuidelineSpecifiedDocumentContextParameter/ID` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/AdditionalReferencedDocument/AttachmentBinaryObject` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/AdditionalReferencedDocument/AttachmentBinaryObject@mimeCode` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/AdditionalReferencedDocument/TypeCode` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/SellerOrderReferencedDocument/FormattedIssueDateTime/DateTimeString@format` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/SellerTradeParty/GlobalID@schemeID` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/SellerTradeParty/URIUniversalCommunication/URIID@schemeID` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/ApplicableTradeTax/CategoryCode` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/ApplicableTradeTax/TypeCode` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/SpecifiedTradePaymentTerms/DueDateDateTime/DateTimeString@format` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/SpecifiedTradeSettlementPaymentMeans/TypeCode` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeAgreement/BuyerOrderReferencedDocument/FormattedIssueDateTime/DateTimeString@format` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeAgreement/NetPriceProductTradePrice/BasisQuantity@unitCode` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeDelivery/BilledQuantity@unitCode` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeSettlement/ApplicableTradeTax/CategoryCode` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeSettlement/ApplicableTradeTax/TypeCode` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeSettlement/BillingSpecifiedPeriod/EndDateTime/DateTimeString@format` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeSettlement/BillingSpecifiedPeriod/StartDateTime/DateTimeString@format` |

### Beispiele\4. EXTENDED\X02_SubInvoiceLines_Buero_Material_Bsp3\X02_01_SubInvoiceLines_Buero_Material_Bsp3__.xml

153 of 205 values are on the page.

| Values | Kind | Element |
|---:|---|---|
| 8 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/AssociatedDocumentLineDocument/LineStatusReasonCode` |
| 8 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeDelivery/BilledQuantity@unitCode` |
| 8 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeSettlement/ApplicableTradeTax/CategoryCode` |
| 8 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeSettlement/ApplicableTradeTax/TypeCode` |
| 8 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedTradeProduct/GlobalID@schemeID` |
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/SpecifiedTradePaymentTerms/DueDateDateTime/DateTimeString@format` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocument/IssueDateTime/DateTimeString@format` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocument/TypeCode` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocumentContext/GuidelineSpecifiedDocumentContextParameter/ID` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/AdditionalReferencedDocument/TypeCode` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/BuyerTradeParty/SpecifiedTaxRegistration/ID@schemeID` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/SellerTradeParty/SpecifiedTaxRegistration/ID@schemeID` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeDelivery/ActualDeliverySupplyChainEvent/OccurrenceDateTime/DateTimeString@format` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/ApplicableTradeTax/CategoryCode` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/ApplicableTradeTax/TypeCode` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/SpecifiedTradeSettlementPaymentMeans/TypeCode` |

### Beispiele\4. EXTENDED\X03_Abschlagsrechnung_SubInvoiceLine_u_LV_Nr\X03_01_Abschlagsrechnung_SubInvoiceLine_u_LV_Nr.xml

131 of 176 values are on the page.

| Values | Kind | Element |
|---:|---|---|
| 6 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/AssociatedDocumentLineDocument/LineStatusReasonCode` |
| 6 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeDelivery/BilledQuantity@unitCode` |
| 6 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeSettlement/ApplicableTradeTax/CategoryCode` |
| 6 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeSettlement/ApplicableTradeTax/TypeCode` |
| 3 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeAgreement/AdditionalReferencedDocument/ReferenceTypeCode` |
| 3 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeAgreement/AdditionalReferencedDocument/TypeCode` |
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/SpecifiedTradePaymentTerms/DueDateDateTime/DateTimeString@format` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocument/IssueDateTime/DateTimeString@format` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocument/TypeCode` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocumentContext/GuidelineSpecifiedDocumentContextParameter/ID` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/AdditionalReferencedDocument/TypeCode` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/BuyerTradeParty/SpecifiedTaxRegistration/ID@schemeID` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/SellerTradeParty/SpecifiedTaxRegistration/ID@schemeID` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeDelivery/ActualDeliverySupplyChainEvent/OccurrenceDateTime/DateTimeString@format` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/ApplicableTradeTax/CategoryCode` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/ApplicableTradeTax/TypeCode` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/BillingSpecifiedPeriod/EndDateTime/DateTimeString@format` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/BillingSpecifiedPeriod/StartDateTime/DateTimeString@format` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/SpecifiedTradeSettlementPaymentMeans/TypeCode` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeAgreement/NetPriceProductTradePrice/BasisQuantity@unitCode` |

### Beispiele\4. EXTENDED\X04_Sammelrechnung_3_Bestellungen\X04_01_Sammelrechnung_3_Bestellungen.xml

237 of 332 values are on the page.

| Values | Kind | Element |
|---:|---|---|
| 7 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeAgreement/BuyerOrderReferencedDocument/FormattedIssueDateTime/DateTimeString@format` |
| 7 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeAgreement/GrossPriceProductTradePrice/BasisQuantity@unitCode` |
| 7 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeAgreement/NetPriceProductTradePrice/BasisQuantity@unitCode` |
| 7 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeDelivery/BilledQuantity@unitCode` |
| 7 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeDelivery/DeliveryNoteReferencedDocument/FormattedIssueDateTime/DateTimeString@format` |
| 7 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeSettlement/AdditionalReferencedDocument/ReferenceTypeCode` |
| 7 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeSettlement/AdditionalReferencedDocument/TypeCode` |
| 7 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeSettlement/ApplicableTradeTax/CategoryCode` |
| 7 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeSettlement/ApplicableTradeTax/TypeCode` |
| 7 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeSettlement/BillingSpecifiedPeriod/EndDateTime/DateTimeString@format` |
| 7 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedTradeProduct/GlobalID@schemeID` |
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/SellerTradeParty/SpecifiedTaxRegistration/ID@schemeID` |
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/ApplicableTradeTax/CategoryCode` |
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/ApplicableTradeTax/TypeCode` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocument/IssueDateTime/DateTimeString@format` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocument/TypeCode` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocumentContext/GuidelineSpecifiedDocumentContextParameter/ID` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/BuyerTradeParty/SpecifiedTaxRegistration/ID@schemeID` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/SellerTradeParty/GlobalID@schemeID` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/SellerTradeParty/URIUniversalCommunication/URIID@schemeID` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeDelivery/ActualDeliverySupplyChainEvent/OccurrenceDateTime/DateTimeString@format` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeDelivery/ShipToTradeParty/SpecifiedTaxRegistration/ID@schemeID` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/SpecifiedTradePaymentTerms/ApplicableTradePaymentDiscountTerms/BasisDateTime/DateTimeString@format` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/SpecifiedTradePaymentTerms/ApplicableTradePaymentDiscountTerms/BasisPeriodMeasure@unitCode` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/SpecifiedTradePaymentTerms/DueDateDateTime/DateTimeString@format` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/SpecifiedTradeSettlementPaymentMeans/TypeCode` |

### Beispiele\4. EXTENDED\X05_Bau_Rechnungen Abschlag_SchlussRechnung\X05_01_einfach_Bau__1_Abschlagsrechnung.xml

91 of 111 values are on the page.

| Values | Kind | Element |
|---:|---|---|
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/SpecifiedTradePaymentTerms/DueDateDateTime/DateTimeString@format` |
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeDelivery/BilledQuantity@unitCode` |
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeSettlement/ApplicableTradeTax/CategoryCode` |
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeSettlement/ApplicableTradeTax/TypeCode` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocument/IssueDateTime/DateTimeString@format` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocument/TypeCode` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocumentContext/GuidelineSpecifiedDocumentContextParameter/ID` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/AdditionalReferencedDocument/TypeCode` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/BuyerTradeParty/SpecifiedTaxRegistration/ID@schemeID` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/SellerTradeParty/SpecifiedTaxRegistration/ID@schemeID` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeDelivery/ActualDeliverySupplyChainEvent/OccurrenceDateTime/DateTimeString@format` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/ApplicableTradeTax/CategoryCode` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/ApplicableTradeTax/TypeCode` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/BillingSpecifiedPeriod/EndDateTime/DateTimeString@format` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/BillingSpecifiedPeriod/StartDateTime/DateTimeString@format` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/SpecifiedTradeSettlementPaymentMeans/TypeCode` |

### Beispiele\4. EXTENDED\X05_Bau_Rechnungen Abschlag_SchlussRechnung\X05_02_einfach_Bau__2_Abschlagsrechnung.xml

109 of 139 values are on the page.

| Values | Kind | Element |
|---:|---|---|
| 5 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeDelivery/BilledQuantity@unitCode` |
| 5 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeSettlement/ApplicableTradeTax/CategoryCode` |
| 5 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeSettlement/ApplicableTradeTax/TypeCode` |
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/SpecifiedTradePaymentTerms/DueDateDateTime/DateTimeString@format` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocument/IssueDateTime/DateTimeString@format` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocument/TypeCode` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocumentContext/GuidelineSpecifiedDocumentContextParameter/ID` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/AdditionalReferencedDocument/TypeCode` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/BuyerTradeParty/SpecifiedTaxRegistration/ID@schemeID` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/SellerTradeParty/SpecifiedTaxRegistration/ID@schemeID` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeDelivery/ActualDeliverySupplyChainEvent/OccurrenceDateTime/DateTimeString@format` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/ApplicableTradeTax/CategoryCode` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/ApplicableTradeTax/TypeCode` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/BillingSpecifiedPeriod/EndDateTime/DateTimeString@format` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/BillingSpecifiedPeriod/StartDateTime/DateTimeString@format` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/InvoiceReferencedDocument/FormattedIssueDateTime/DateTimeString@format` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/SpecifiedTradeSettlementPaymentMeans/TypeCode` |

### Beispiele\4. EXTENDED\X05_Bau_Rechnungen Abschlag_SchlussRechnung\X05_03_einfach_Bau_Schlussrechnung_mit_Steuer_Zusammenstellung.xml

133 of 181 values are on the page.

| Values | Kind | Element |
|---:|---|---|
| 7 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeDelivery/BilledQuantity@unitCode` |
| 7 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeSettlement/ApplicableTradeTax/CategoryCode` |
| 7 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeSettlement/ApplicableTradeTax/TypeCode` |
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/AdditionalReferencedDocument/TypeCode` |
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/SpecifiedAdvancePayment/FormattedReceivedDateTime/DateTimeString@format` |
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/SpecifiedAdvancePayment/IncludedTradeTax/CategoryCode` |
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/SpecifiedAdvancePayment/IncludedTradeTax/TypeCode` |
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/SpecifiedAdvancePayment/InvoiceSpecifiedReferencedDocument/FormattedIssueDateTime/DateTimeString@format` |
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/SpecifiedAdvancePayment/InvoiceSpecifiedReferencedDocument/TypeCode` |
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/SpecifiedTradePaymentTerms/DueDateDateTime/DateTimeString@format` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocument/IssueDateTime/DateTimeString@format` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocument/TypeCode` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocumentContext/GuidelineSpecifiedDocumentContextParameter/ID` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/AdditionalReferencedDocument/AttachmentBinaryObject` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/AdditionalReferencedDocument/AttachmentBinaryObject@mimeCode` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/BuyerTradeParty/SpecifiedTaxRegistration/ID@schemeID` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/SellerTradeParty/SpecifiedTaxRegistration/ID@schemeID` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeDelivery/ActualDeliverySupplyChainEvent/OccurrenceDateTime/DateTimeString@format` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/ApplicableTradeTax/CategoryCode` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/ApplicableTradeTax/TypeCode` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/BillingSpecifiedPeriod/EndDateTime/DateTimeString@format` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/BillingSpecifiedPeriod/StartDateTime/DateTimeString@format` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/SpecifiedTradeSettlementPaymentMeans/TypeCode` |

### Beispiele\4. EXTENDED\X06_Fallschutz-Set_SubInvoiceLine_Bsp5\X06_01_Fallschutz-Set_SubInvoiceLine_Bsp5.xml

116 of 161 values are on the page.

| Values | Kind | Element |
|---:|---|---|
| 5 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/AssociatedDocumentLineDocument/LineStatusReasonCode` |
| 5 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeDelivery/BilledQuantity@unitCode` |
| 5 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeSettlement/ApplicableTradeTax/CategoryCode` |
| 5 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeSettlement/ApplicableTradeTax/TypeCode` |
| 5 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedTradeProduct/GlobalID@schemeID` |
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/SellerTradeParty/SpecifiedTaxRegistration/ID@schemeID` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocument/IssueDateTime/DateTimeString@format` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocument/TypeCode` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocumentContext/GuidelineSpecifiedDocumentContextParameter/ID` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/SellerOrderReferencedDocument/FormattedIssueDateTime/DateTimeString@format` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/SellerTradeParty/GlobalID@schemeID` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/SellerTradeParty/URIUniversalCommunication/URIID@schemeID` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeDelivery/ActualDeliverySupplyChainEvent/OccurrenceDateTime/DateTimeString@format` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeDelivery/DeliveryNoteReferencedDocument/FormattedIssueDateTime/DateTimeString@format` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/ApplicableTradeTax/CategoryCode` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/ApplicableTradeTax/TypeCode` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/SpecifiedTradePaymentTerms/DueDateDateTime/DateTimeString@format` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/SpecifiedTradeSettlementPaymentMeans/TypeCode` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeAgreement/GrossPriceProductTradePrice/BasisQuantity@unitCode` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeAgreement/NetPriceProductTradePrice/BasisQuantity@unitCode` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeSettlement/AdditionalReferencedDocument/ReferenceTypeCode` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeSettlement/AdditionalReferencedDocument/TypeCode` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeSettlement/BillingSpecifiedPeriod/EndDateTime/DateTimeString@format` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeSettlement/BillingSpecifiedPeriod/StartDateTime/DateTimeString@format` |

### Beispiele\4. EXTENDED\X07_Fremdwaehrung\X07_01_Fremdwaehrung.xml

109 of 144 values are on the page.

| Values | Kind | Element |
|---:|---|---|
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/SpecifiedTradeAllowanceCharge/CategoryTradeTax/CategoryCode` |
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/SpecifiedTradeAllowanceCharge/CategoryTradeTax/TypeCode` |
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/SpecifiedTradeAllowanceCharge/ChargeIndicator/Indicator` |
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/SpecifiedTradeAllowanceCharge/ReasonCode` |
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/SpecifiedTradePaymentTerms/DueDateDateTime/DateTimeString@format` |
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeSettlement/SpecifiedTradeAllowanceCharge/ChargeIndicator/Indicator` |
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeSettlement/SpecifiedTradeAllowanceCharge/ReasonCode` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocument/IssueDateTime/DateTimeString@format` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocument/TypeCode` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocumentContext/BusinessProcessSpecifiedDocumentContextParameter/ID` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocumentContext/GuidelineSpecifiedDocumentContextParameter/ID` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/BuyerTradeParty/URIUniversalCommunication/URIID@schemeID` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/SellerTaxRepresentativeTradeParty/SpecifiedTaxRegistration/ID@schemeID` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/SellerTradeParty/SpecifiedTaxRegistration/ID@schemeID` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeDelivery/ActualDeliverySupplyChainEvent/OccurrenceDateTime/DateTimeString@format` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeDelivery/ShipToTradeParty/URIUniversalCommunication/URIID@schemeID` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/ApplicableTradeTax/CategoryCode` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/ApplicableTradeTax/TypeCode` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/BillingSpecifiedPeriod/EndDateTime/DateTimeString@format` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/BillingSpecifiedPeriod/StartDateTime/DateTimeString@format` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/PayeeTradeParty/GlobalID@schemeID` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/SpecifiedTradeSettlementPaymentMeans/TypeCode` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/TaxApplicableTradeCurrencyExchange/ConversionRateDateTime/DateTimeString@format` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeAgreement/GrossPriceProductTradePrice/BasisQuantity@unitCode` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeAgreement/NetPriceProductTradePrice/BasisQuantity@unitCode` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeDelivery/BilledQuantity@unitCode` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeSettlement/ApplicableTradeTax/CategoryCode` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeSettlement/ApplicableTradeTax/TypeCode` |

### Beispiele\4. EXTENDED\X08_Herkunftsland_Zolltarif-Nummer__HS___UNSPSC__TST___eCl_ss__STQ\X08_01_Herkunftsland_Zolltarif-Nummer__HS___UNSPSC__TST___eCl_ss__STQ.xml

212 of 316 values are on the page.

| Values | Kind | Element |
|---:|---|---|
| 6 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeAgreement/BuyerOrderReferencedDocument/FormattedIssueDateTime/DateTimeString@format` |
| 6 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeAgreement/GrossPriceProductTradePrice/BasisQuantity@unitCode` |
| 6 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeAgreement/NetPriceProductTradePrice/BasisQuantity@unitCode` |
| 6 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeDelivery/BilledQuantity@unitCode` |
| 6 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeDelivery/DeliveryNoteReferencedDocument/FormattedIssueDateTime/DateTimeString@format` |
| 6 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeSettlement/AdditionalReferencedDocument/ReferenceTypeCode` |
| 6 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeSettlement/AdditionalReferencedDocument/TypeCode` |
| 6 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeSettlement/ApplicableTradeTax/CategoryCode` |
| 6 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeSettlement/ApplicableTradeTax/TypeCode` |
| 6 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeSettlement/BillingSpecifiedPeriod/EndDateTime/DateTimeString@format` |
| 6 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedTradeProduct/DesignatedProductClassification/ClassCode` |
| 6 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedTradeProduct/DesignatedProductClassification/ClassCode@listID` |
| 6 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedTradeProduct/DesignatedProductClassification/ClassCode@listVersionID` |
| 6 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedTradeProduct/GlobalID@schemeID` |
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/SellerTradeParty/SpecifiedTaxRegistration/ID@schemeID` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocument/IssueDateTime/DateTimeString@format` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocument/TypeCode` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocumentContext/GuidelineSpecifiedDocumentContextParameter/ID` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/BuyerOrderReferencedDocument/FormattedIssueDateTime/DateTimeString@format` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/BuyerTradeParty/GlobalID@schemeID` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/BuyerTradeParty/SpecifiedTaxRegistration/ID@schemeID` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/SellerOrderReferencedDocument/FormattedIssueDateTime/DateTimeString@format` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/SellerTradeParty/GlobalID@schemeID` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/SellerTradeParty/URIUniversalCommunication/URIID@schemeID` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeDelivery/ActualDeliverySupplyChainEvent/OccurrenceDateTime/DateTimeString@format` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeDelivery/DeliveryNoteReferencedDocument/FormattedIssueDateTime/DateTimeString@format` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeDelivery/ShipToTradeParty/GlobalID@schemeID` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/ApplicableTradeTax/CategoryCode` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/ApplicableTradeTax/TypeCode` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/SpecifiedTradePaymentTerms/ApplicableTradePaymentDiscountTerms/BasisDateTime/DateTimeString@format` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/SpecifiedTradePaymentTerms/ApplicableTradePaymentDiscountTerms/BasisPeriodMeasure@unitCode` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/SpecifiedTradePaymentTerms/DueDateDateTime/DateTimeString@format` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/SpecifiedTradeSettlementPaymentMeans/TypeCode` |

### Beispiele\4. EXTENDED\X09_InnergemeinschLieferungMehrereBestellungen\X09_01_InnergemeinschLieferungMehrereBestellungen.xml

94 of 124 values are on the page.

| Values | Kind | Element |
|---:|---|---|
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeAgreement/GrossPriceProductTradePrice/BasisQuantity@unitCode` |
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeAgreement/NetPriceProductTradePrice/BasisQuantity@unitCode` |
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeDelivery/BilledQuantity@unitCode` |
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeSettlement/ApplicableTradeTax/CategoryCode` |
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeSettlement/ApplicableTradeTax/TypeCode` |
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeSettlement/BillingSpecifiedPeriod/EndDateTime/DateTimeString@format` |
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeSettlement/BillingSpecifiedPeriod/StartDateTime/DateTimeString@format` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocument/IssueDateTime/DateTimeString@format` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocument/TypeCode` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocumentContext/BusinessProcessSpecifiedDocumentContextParameter/ID` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocumentContext/GuidelineSpecifiedDocumentContextParameter/ID` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/BuyerTradeParty/SpecifiedTaxRegistration/ID@schemeID` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/BuyerTradeParty/URIUniversalCommunication/URIID@schemeID` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/SellerTaxRepresentativeTradeParty/SpecifiedTaxRegistration/ID@schemeID` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/SellerTradeParty/SpecifiedTaxRegistration/ID@schemeID` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeDelivery/ShipToTradeParty/URIUniversalCommunication/URIID@schemeID` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/ApplicableTradeTax/CategoryCode` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/ApplicableTradeTax/TypeCode` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/BillingSpecifiedPeriod/EndDateTime/DateTimeString@format` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/BillingSpecifiedPeriod/StartDateTime/DateTimeString@format` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/PayeeTradeParty/GlobalID@schemeID` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/SpecifiedTradePaymentTerms/DueDateDateTime/DateTimeString@format` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/SpecifiedTradeSettlementPaymentMeans/TypeCode` |

### Beispiele\4. EXTENDED\X10_Kleinunternehmer_ohneUStId\X10_01_Kleinunternehmer_ohneUStId.xml

54 of 69 values are on the page.

| Values | Kind | Element |
|---:|---|---|
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocument/IssueDateTime/DateTimeString@format` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocument/TypeCode` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocumentContext/GuidelineSpecifiedDocumentContextParameter/ID` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/BuyerTradeParty/URIUniversalCommunication/URIID@schemeID` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/SellerTradeParty/SpecifiedTaxRegistration/ID@schemeID` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/SellerTradeParty/URIUniversalCommunication/URIID@schemeID` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeDelivery/ActualDeliverySupplyChainEvent/OccurrenceDateTime/DateTimeString@format` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/ApplicableTradeTax/CategoryCode` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/ApplicableTradeTax/TypeCode` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/SpecifiedTradePaymentTerms/DueDateDateTime/DateTimeString@format` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/SpecifiedTradeSettlementPaymentMeans/TypeCode` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeAgreement/NetPriceProductTradePrice/BasisQuantity@unitCode` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeDelivery/BilledQuantity@unitCode` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeSettlement/ApplicableTradeTax/CategoryCode` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeSettlement/ApplicableTradeTax/TypeCode` |

### Beispiele\4. EXTENDED\X11_Kostenrechnung\X11_01_Kostenrechnung.xml

130 of 170 values are on the page.

| Values | Kind | Element |
|---:|---|---|
| 5 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeDelivery/BilledQuantity@unitCode` |
| 5 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeSettlement/ApplicableTradeTax/CategoryCode` |
| 5 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeSettlement/ApplicableTradeTax/TypeCode` |
| 5 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedTradeProduct/GlobalID@schemeID` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocument/IssueDateTime/DateTimeString@format` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocument/TypeCode` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocumentContext/GuidelineSpecifiedDocumentContextParameter/ID` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocumentContext/TestIndicator/Indicator` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/AdditionalReferencedDocument/TypeCode` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/BuyerTradeParty/GlobalID@schemeID` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/SellerTradeParty/GlobalID@schemeID` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/SellerTradeParty/SpecifiedTaxRegistration/ID@schemeID` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeDelivery/ActualDeliverySupplyChainEvent/OccurrenceDateTime/DateTimeString@format` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeDelivery/ShipToTradeParty/GlobalID@schemeID` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/ApplicableTradeTax/CategoryCode` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/ApplicableTradeTax/TypeCode` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/InvoiceeTradeParty/GlobalID@schemeID` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/SpecifiedLogisticsServiceCharge/AppliedTradeTax/CategoryCode` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/SpecifiedLogisticsServiceCharge/AppliedTradeTax/TypeCode` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/SpecifiedTradeAllowanceCharge/CategoryTradeTax/CategoryCode` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/SpecifiedTradeAllowanceCharge/CategoryTradeTax/TypeCode` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/SpecifiedTradeAllowanceCharge/ChargeIndicator/Indicator` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/SpecifiedTradePaymentTerms/ApplicableTradePaymentDiscountTerms/BasisPeriodMeasure@unitCode` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeAgreement/GrossPriceProductTradePrice/AppliedTradeAllowanceCharge/ChargeIndicator/Indicator` |

### Beispiele\4. EXTENDED\X12_Maschinen-Serial_NR__BT-X-307\X12_01_Maschinen-Serial_NR__BT-X-307.xml

82 of 104 values are on the page.

| Values | Kind | Element |
|---:|---|---|
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/SellerTradeParty/SpecifiedTaxRegistration/ID@schemeID` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocument/IssueDateTime/DateTimeString@format` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocument/TypeCode` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocumentContext/GuidelineSpecifiedDocumentContextParameter/ID` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/SellerOrderReferencedDocument/FormattedIssueDateTime/DateTimeString@format` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/SellerTradeParty/GlobalID@schemeID` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/SellerTradeParty/URIUniversalCommunication/URIID@schemeID` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeDelivery/ActualDeliverySupplyChainEvent/OccurrenceDateTime/DateTimeString@format` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeDelivery/DeliveryNoteReferencedDocument/FormattedIssueDateTime/DateTimeString@format` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/ApplicableTradeTax/CategoryCode` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/ApplicableTradeTax/TypeCode` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/SpecifiedTradePaymentTerms/DueDateDateTime/DateTimeString@format` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/SpecifiedTradeSettlementPaymentMeans/TypeCode` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeAgreement/GrossPriceProductTradePrice/BasisQuantity@unitCode` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeAgreement/NetPriceProductTradePrice/BasisQuantity@unitCode` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeDelivery/BilledQuantity@unitCode` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeDelivery/DeliveryNoteReferencedDocument/FormattedIssueDateTime/DateTimeString@format` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeSettlement/ApplicableTradeTax/CategoryCode` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeSettlement/ApplicableTradeTax/TypeCode` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeSettlement/BillingSpecifiedPeriod/EndDateTime/DateTimeString@format` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedTradeProduct/GlobalID@schemeID` |

### Beispiele\4. EXTENDED\X13_Projektabschlussrechnung\X13_01_Projektabschlussrechnung.xml

83 of 118 values are on the page.

| Values | Kind | Element |
|---:|---|---|
| 4 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/SpecifiedAdvancePayment/FormattedReceivedDateTime/DateTimeString@format` |
| 4 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/SpecifiedAdvancePayment/IncludedTradeTax/CategoryCode` |
| 4 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/SpecifiedAdvancePayment/IncludedTradeTax/TypeCode` |
| 4 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/SpecifiedAdvancePayment/InvoiceSpecifiedReferencedDocument/FormattedIssueDateTime/DateTimeString@format` |
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/SellerTradeParty/SpecifiedTaxRegistration/ID@schemeID` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocument/IssueDateTime/DateTimeString@format` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocument/TypeCode` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocumentContext/GuidelineSpecifiedDocumentContextParameter/ID` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/BuyerTradeParty/SpecifiedTaxRegistration/ID@schemeID` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/SellerTradeParty/GlobalID@schemeID` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/SellerTradeParty/URIUniversalCommunication/URIID@schemeID` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeDelivery/ActualDeliverySupplyChainEvent/OccurrenceDateTime/DateTimeString@format` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/ApplicableTradeTax/CategoryCode` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/ApplicableTradeTax/TypeCode` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/SpecifiedTradePaymentTerms/DueDateDateTime/DateTimeString@format` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/SpecifiedTradeSettlementPaymentMeans/TypeCode` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeAgreement/NetPriceProductTradePrice/BasisQuantity@unitCode` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeDelivery/BilledQuantity@unitCode` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeSettlement/ApplicableTradeTax/CategoryCode` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeSettlement/ApplicableTradeTax/TypeCode` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeSettlement/BillingSpecifiedPeriod/EndDateTime/DateTimeString@format` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeSettlement/BillingSpecifiedPeriod/StartDateTime/DateTimeString@format` |

### Beispiele\4. EXTENDED\X14_Rechnungskorrektur\X14_01_Rechnungskorrektur.xml

112 of 153 values are on the page.

| Values | Kind | Element |
|---:|---|---|
| 4 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/SpecifiedTradeAllowanceCharge/CategoryTradeTax/CategoryCode` |
| 4 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/SpecifiedTradeAllowanceCharge/CategoryTradeTax/TypeCode` |
| 4 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/SpecifiedTradeAllowanceCharge/ChargeIndicator/Indicator` |
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/AdditionalReferencedDocument/TypeCode` |
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/ApplicableTradeTax/CategoryCode` |
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/ApplicableTradeTax/TypeCode` |
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeAgreement/GrossPriceProductTradePrice/AppliedTradeAllowanceCharge/ChargeIndicator/Indicator` |
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeDelivery/BilledQuantity@unitCode` |
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeDelivery/PackageQuantity@unitCode` |
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeSettlement/ApplicableTradeTax/CategoryCode` |
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeSettlement/ApplicableTradeTax/TypeCode` |
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedTradeProduct/GlobalID@schemeID` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocument/IssueDateTime/DateTimeString@format` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocument/TypeCode` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocumentContext/BusinessProcessSpecifiedDocumentContextParameter/ID` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocumentContext/GuidelineSpecifiedDocumentContextParameter/ID` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocumentContext/TestIndicator/Indicator` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/BuyerTradeParty/GlobalID@schemeID` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/SellerTradeParty/GlobalID@schemeID` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/SellerTradeParty/SpecifiedTaxRegistration/ID@schemeID` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeDelivery/ActualDeliverySupplyChainEvent/OccurrenceDateTime/DateTimeString@format` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeDelivery/ShipToTradeParty/GlobalID@schemeID` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/InvoiceeTradeParty/GlobalID@schemeID` |

### Beispiele\4. EXTENDED\X15_steuerfreie_Innergemeinschaftliche_Lieferung\X15_01_steuerfreie_Innergemeinschaftliche_Lieferung.xml

123 of 173 values are on the page.

| Values | Kind | Element |
|---:|---|---|
| 3 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeAgreement/BuyerOrderReferencedDocument/FormattedIssueDateTime/DateTimeString@format` |
| 3 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeAgreement/GrossPriceProductTradePrice/BasisQuantity@unitCode` |
| 3 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeAgreement/NetPriceProductTradePrice/BasisQuantity@unitCode` |
| 3 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeDelivery/BilledQuantity@unitCode` |
| 3 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeSettlement/AdditionalReferencedDocument/ReferenceTypeCode` |
| 3 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeSettlement/AdditionalReferencedDocument/TypeCode` |
| 3 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeSettlement/ApplicableTradeTax/CategoryCode` |
| 3 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeSettlement/ApplicableTradeTax/TypeCode` |
| 3 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeSettlement/BillingSpecifiedPeriod/EndDateTime/DateTimeString@format` |
| 3 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeSettlement/BillingSpecifiedPeriod/StartDateTime/DateTimeString@format` |
| 3 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedTradeProduct/GlobalID@schemeID` |
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/SellerTradeParty/SpecifiedTaxRegistration/ID@schemeID` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocument/IssueDateTime/DateTimeString@format` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocument/TypeCode` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocumentContext/GuidelineSpecifiedDocumentContextParameter/ID` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/BuyerOrderReferencedDocument/FormattedIssueDateTime/DateTimeString@format` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/BuyerTradeParty/SpecifiedTaxRegistration/ID@schemeID` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/SellerOrderReferencedDocument/FormattedIssueDateTime/DateTimeString@format` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/SellerTradeParty/GlobalID@schemeID` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/SellerTradeParty/URIUniversalCommunication/URIID@schemeID` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeDelivery/ActualDeliverySupplyChainEvent/OccurrenceDateTime/DateTimeString@format` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeDelivery/DeliveryNoteReferencedDocument/FormattedIssueDateTime/DateTimeString@format` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeDelivery/ShipToTradeParty/SpecifiedTaxRegistration/ID@schemeID` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/ApplicableTradeTax/CategoryCode` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/ApplicableTradeTax/TypeCode` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/SpecifiedTradePaymentTerms/DueDateDateTime/DateTimeString@format` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/SpecifiedTradeSettlementPaymentMeans/TypeCode` |

### Beispiele\4. EXTENDED\X16_Stornogutschrift_zur_Rechnung\X16_01_Stornogutschrift_zu_RE.xml

101 of 138 values are on the page.

| Values | Kind | Element |
|---:|---|---|
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/SellerTradeParty/SpecifiedTaxRegistration/ID@schemeID` |
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeAgreement/BuyerOrderReferencedDocument/FormattedIssueDateTime/DateTimeString@format` |
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeAgreement/GrossPriceProductTradePrice/BasisQuantity@unitCode` |
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeAgreement/NetPriceProductTradePrice/BasisQuantity@unitCode` |
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeDelivery/BilledQuantity@unitCode` |
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeDelivery/DeliveryNoteReferencedDocument/FormattedIssueDateTime/DateTimeString@format` |
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeSettlement/AdditionalReferencedDocument/ReferenceTypeCode` |
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeSettlement/AdditionalReferencedDocument/TypeCode` |
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeSettlement/ApplicableTradeTax/CategoryCode` |
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeSettlement/ApplicableTradeTax/TypeCode` |
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeSettlement/BillingSpecifiedPeriod/EndDateTime/DateTimeString@format` |
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedTradeProduct/GlobalID@schemeID` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocument/IssueDateTime/DateTimeString@format` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocument/TypeCode` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocumentContext/GuidelineSpecifiedDocumentContextParameter/ID` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/BuyerOrderReferencedDocument/FormattedIssueDateTime/DateTimeString@format` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/BuyerTradeParty/SpecifiedTaxRegistration/ID@schemeID` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/SellerOrderReferencedDocument/FormattedIssueDateTime/DateTimeString@format` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/SellerTradeParty/GlobalID@schemeID` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/SellerTradeParty/URIUniversalCommunication/URIID@schemeID` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeDelivery/ActualDeliverySupplyChainEvent/OccurrenceDateTime/DateTimeString@format` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/ApplicableTradeTax/CategoryCode` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/ApplicableTradeTax/TypeCode` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/SpecifiedTradePaymentTerms/DueDateDateTime/DateTimeString@format` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/SpecifiedTradeSettlementPaymentMeans/TypeCode` |

### Beispiele\4. EXTENDED\X16_Stornogutschrift_zur_Rechnung\X16_02_Rechnung_für_Storno_Gutschrift_.xml

106 of 144 values are on the page.

| Values | Kind | Element |
|---:|---|---|
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/SellerTradeParty/SpecifiedTaxRegistration/ID@schemeID` |
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeAgreement/BuyerOrderReferencedDocument/FormattedIssueDateTime/DateTimeString@format` |
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeAgreement/GrossPriceProductTradePrice/BasisQuantity@unitCode` |
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeAgreement/NetPriceProductTradePrice/BasisQuantity@unitCode` |
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeDelivery/BilledQuantity@unitCode` |
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeDelivery/DeliveryNoteReferencedDocument/FormattedIssueDateTime/DateTimeString@format` |
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeSettlement/AdditionalReferencedDocument/ReferenceTypeCode` |
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeSettlement/AdditionalReferencedDocument/TypeCode` |
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeSettlement/ApplicableTradeTax/CategoryCode` |
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeSettlement/ApplicableTradeTax/TypeCode` |
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeSettlement/BillingSpecifiedPeriod/EndDateTime/DateTimeString@format` |
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedTradeProduct/GlobalID@schemeID` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocument/IssueDateTime/DateTimeString@format` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocument/TypeCode` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocumentContext/GuidelineSpecifiedDocumentContextParameter/ID` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/BuyerOrderReferencedDocument/FormattedIssueDateTime/DateTimeString@format` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/BuyerTradeParty/SpecifiedTaxRegistration/ID@schemeID` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/SellerOrderReferencedDocument/FormattedIssueDateTime/DateTimeString@format` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/SellerTradeParty/GlobalID@schemeID` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/SellerTradeParty/URIUniversalCommunication/URIID@schemeID` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeDelivery/ActualDeliverySupplyChainEvent/OccurrenceDateTime/DateTimeString@format` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeDelivery/DeliveryNoteReferencedDocument/FormattedIssueDateTime/DateTimeString@format` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/ApplicableTradeTax/CategoryCode` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/ApplicableTradeTax/TypeCode` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/SpecifiedTradePaymentTerms/DueDateDateTime/DateTimeString@format` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/SpecifiedTradeSettlementPaymentMeans/TypeCode` |

### Beispiele\4. EXTENDED\X17_SubInvoiceLines_Hardware_Bsp2\X17_01_SubInvoiceLines_Hardware_Bsp2.xml

131 of 173 values are on the page.

| Values | Kind | Element |
|---:|---|---|
| 6 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/AssociatedDocumentLineDocument/LineStatusReasonCode` |
| 6 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeDelivery/BilledQuantity@unitCode` |
| 6 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeSettlement/ApplicableTradeTax/CategoryCode` |
| 6 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeSettlement/ApplicableTradeTax/TypeCode` |
| 6 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedTradeProduct/GlobalID@schemeID` |
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/SpecifiedTradePaymentTerms/DueDateDateTime/DateTimeString@format` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocument/IssueDateTime/DateTimeString@format` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocument/TypeCode` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocumentContext/GuidelineSpecifiedDocumentContextParameter/ID` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/AdditionalReferencedDocument/TypeCode` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/BuyerTradeParty/SpecifiedTaxRegistration/ID@schemeID` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/SellerTradeParty/SpecifiedTaxRegistration/ID@schemeID` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeDelivery/ActualDeliverySupplyChainEvent/OccurrenceDateTime/DateTimeString@format` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/ApplicableTradeTax/CategoryCode` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/ApplicableTradeTax/TypeCode` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/SpecifiedTradeSettlementPaymentMeans/TypeCode` |

### Beispiele\4. EXTENDED\X18_Subinvoiceline Kaffee Bundle Set Bsp 4\X18_01_SubInvoiceLines_Kaffee_Bundle_Set_Bsp4___.xml

136 of 177 values are on the page.

| Values | Kind | Element |
|---:|---|---|
| 6 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/AssociatedDocumentLineDocument/LineStatusReasonCode` |
| 6 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeDelivery/BilledQuantity@unitCode` |
| 6 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedTradeProduct/GlobalID@schemeID` |
| 4 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeSettlement/ApplicableTradeTax/CategoryCode` |
| 4 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeSettlement/ApplicableTradeTax/TypeCode` |
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/ApplicableTradeTax/CategoryCode` |
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/ApplicableTradeTax/TypeCode` |
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/SpecifiedTradePaymentTerms/DueDateDateTime/DateTimeString@format` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocument/IssueDateTime/DateTimeString@format` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocument/TypeCode` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocumentContext/GuidelineSpecifiedDocumentContextParameter/ID` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/AdditionalReferencedDocument/TypeCode` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/BuyerTradeParty/SpecifiedTaxRegistration/ID@schemeID` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/SellerTradeParty/SpecifiedTaxRegistration/ID@schemeID` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeDelivery/ActualDeliverySupplyChainEvent/OccurrenceDateTime/DateTimeString@format` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/SpecifiedTradeSettlementPaymentMeans/TypeCode` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeDelivery/PackageQuantity@unitCode` |

### Beispiele\4. EXTENDED\X19_Warenrechnung\X19_01_Warenrechnung.xml

188 of 258 values are on the page.

| Values | Kind | Element |
|---:|---|---|
| 6 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeDelivery/BilledQuantity@unitCode` |
| 6 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeDelivery/PackageQuantity@unitCode` |
| 6 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeSettlement/ApplicableTradeTax/CategoryCode` |
| 6 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeSettlement/ApplicableTradeTax/TypeCode` |
| 6 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedTradeProduct/GlobalID@schemeID` |
| 4 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/SpecifiedTradeAllowanceCharge/CategoryTradeTax/CategoryCode` |
| 4 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/SpecifiedTradeAllowanceCharge/CategoryTradeTax/TypeCode` |
| 4 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/SpecifiedTradeAllowanceCharge/ChargeIndicator/Indicator` |
| 3 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeAgreement/GrossPriceProductTradePrice/AppliedTradeAllowanceCharge/ChargeIndicator/Indicator` |
| 3 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedTradeProduct/IncludedReferencedProduct/GlobalID@schemeID` |
| 3 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedTradeProduct/IncludedReferencedProduct/UnitQuantity@unitCode` |
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/ApplicableTradeTax/CategoryCode` |
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/ApplicableTradeTax/TypeCode` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocument/IssueDateTime/DateTimeString@format` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocument/TypeCode` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocumentContext/GuidelineSpecifiedDocumentContextParameter/ID` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocumentContext/TestIndicator/Indicator` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/AdditionalReferencedDocument/TypeCode` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/BuyerTradeParty/GlobalID@schemeID` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/SellerTradeParty/GlobalID@schemeID` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/SellerTradeParty/SpecifiedTaxRegistration/ID@schemeID` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/SellerTradeParty/URIUniversalCommunication/URIID@schemeID` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeDelivery/ActualDeliverySupplyChainEvent/OccurrenceDateTime/DateTimeString@format` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeDelivery/ShipToTradeParty/GlobalID@schemeID` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/InvoiceeTradeParty/GlobalID@schemeID` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/SpecifiedLogisticsServiceCharge/AppliedTradeTax/CategoryCode` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/SpecifiedLogisticsServiceCharge/AppliedTradeTax/TypeCode` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/SpecifiedTradePaymentTerms/ApplicableTradePaymentDiscountTerms/BasisPeriodMeasure@unitCode` |

### Beispiele\4. EXTENDED\X20_SubInvoiceLines_Buero_Material_Bsp3__\X20_01_SubInvoiceLines_Buero_Material_Bsp3__.xml

153 of 205 values are on the page.

| Values | Kind | Element |
|---:|---|---|
| 8 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/AssociatedDocumentLineDocument/LineStatusReasonCode` |
| 8 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeDelivery/BilledQuantity@unitCode` |
| 8 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeSettlement/ApplicableTradeTax/CategoryCode` |
| 8 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeSettlement/ApplicableTradeTax/TypeCode` |
| 8 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedTradeProduct/GlobalID@schemeID` |
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/SpecifiedTradePaymentTerms/DueDateDateTime/DateTimeString@format` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocument/IssueDateTime/DateTimeString@format` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocument/TypeCode` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocumentContext/GuidelineSpecifiedDocumentContextParameter/ID` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/AdditionalReferencedDocument/TypeCode` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/BuyerTradeParty/SpecifiedTaxRegistration/ID@schemeID` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/SellerTradeParty/SpecifiedTaxRegistration/ID@schemeID` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeDelivery/ActualDeliverySupplyChainEvent/OccurrenceDateTime/DateTimeString@format` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/ApplicableTradeTax/CategoryCode` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/ApplicableTradeTax/TypeCode` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/SpecifiedTradeSettlementPaymentMeans/TypeCode` |

### Beispiele\5. XRECHNUNG\XRECHNUNG_Betriebskostenabrechnung\XRECHNUNG_Betriebskostenabrechnung.xml

58 of 77 values are on the page.

| Values | Kind | Element |
|---:|---|---|
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/SellerTradeParty/SpecifiedTaxRegistration/ID@schemeID` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocument/IssueDateTime/DateTimeString@format` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocument/TypeCode` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocumentContext/GuidelineSpecifiedDocumentContextParameter/ID` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/AdditionalReferencedDocument/AttachmentBinaryObject` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/AdditionalReferencedDocument/AttachmentBinaryObject@mimeCode` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/AdditionalReferencedDocument/TypeCode` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/SellerTradeParty/GlobalID@schemeID` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/ApplicableTradeTax/CategoryCode` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/ApplicableTradeTax/TypeCode` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/BillingSpecifiedPeriod/EndDateTime/DateTimeString@format` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/BillingSpecifiedPeriod/StartDateTime/DateTimeString@format` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/SpecifiedTradePaymentTerms/DueDateDateTime/DateTimeString@format` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/SpecifiedTradeSettlementPaymentMeans/TypeCode` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeDelivery/BilledQuantity@unitCode` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeSettlement/ApplicableTradeTax/CategoryCode` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeSettlement/ApplicableTradeTax/TypeCode` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedTradeProduct/GlobalID@schemeID` |

### Beispiele\5. XRECHNUNG\XRECHNUNG_Einfach\XRECHNUNG_Einfach.xml

65 of 89 values are on the page.

| Values | Kind | Element |
|---:|---|---|
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/SellerTradeParty/SpecifiedTaxRegistration/ID@schemeID` |
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/ApplicableTradeTax/CategoryCode` |
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/ApplicableTradeTax/TypeCode` |
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeDelivery/BilledQuantity@unitCode` |
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeSettlement/ApplicableTradeTax/CategoryCode` |
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeSettlement/ApplicableTradeTax/TypeCode` |
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedTradeProduct/GlobalID@schemeID` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocument/IssueDateTime/DateTimeString@format` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocument/TypeCode` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocumentContext/BusinessProcessSpecifiedDocumentContextParameter/ID` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocumentContext/GuidelineSpecifiedDocumentContextParameter/ID` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/BuyerTradeParty/URIUniversalCommunication/URIID@schemeID` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/SellerTradeParty/GlobalID@schemeID` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/SellerTradeParty/URIUniversalCommunication/URIID@schemeID` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeDelivery/ActualDeliverySupplyChainEvent/OccurrenceDateTime/DateTimeString@format` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/SpecifiedTradePaymentTerms/DueDateDateTime/DateTimeString@format` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/SpecifiedTradeSettlementPaymentMeans/TypeCode` |

### Beispiele\5. XRECHNUNG\XRECHNUNG_Elektron\XRECHNUNG_Elektron.xml

66 of 91 values are on the page.

| Values | Kind | Element |
|---:|---|---|
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/AdditionalReferencedDocument/AttachmentBinaryObject` |
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/AdditionalReferencedDocument/AttachmentBinaryObject@mimeCode` |
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/AdditionalReferencedDocument/TypeCode` |
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/SellerTradeParty/SpecifiedTaxRegistration/ID@schemeID` |
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeDelivery/BilledQuantity@unitCode` |
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeSettlement/ApplicableTradeTax/CategoryCode` |
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeSettlement/ApplicableTradeTax/TypeCode` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocument/IssueDateTime/DateTimeString@format` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocument/TypeCode` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocumentContext/BusinessProcessSpecifiedDocumentContextParameter/ID` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocumentContext/GuidelineSpecifiedDocumentContextParameter/ID` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/BuyerTradeParty/URIUniversalCommunication/URIID@schemeID` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/SellerTradeParty/GlobalID@schemeID` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/SellerTradeParty/URIUniversalCommunication/URIID@schemeID` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeDelivery/ActualDeliverySupplyChainEvent/OccurrenceDateTime/DateTimeString@format` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/ApplicableTradeTax/CategoryCode` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/ApplicableTradeTax/TypeCode` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/SpecifiedTradeSettlementPaymentMeans/TypeCode` |

### Beispiele\5. XRECHNUNG\XRECHNUNG_Reisekostenabrechnung\XRECHNUNG_Reisekostenabrechnung.xml

89 of 125 values are on the page.

| Values | Kind | Element |
|---:|---|---|
| 4 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeDelivery/BilledQuantity@unitCode` |
| 4 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeSettlement/ApplicableTradeTax/CategoryCode` |
| 4 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeSettlement/ApplicableTradeTax/TypeCode` |
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/AdditionalReferencedDocument/AttachmentBinaryObject` |
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/AdditionalReferencedDocument/AttachmentBinaryObject@mimeCode` |
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/AdditionalReferencedDocument/TypeCode` |
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/SellerTradeParty/SpecifiedTaxRegistration/ID@schemeID` |
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/ApplicableTradeTax/CategoryCode` |
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/ApplicableTradeTax/TypeCode` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocument/IssueDateTime/DateTimeString@format` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocument/TypeCode` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocumentContext/BusinessProcessSpecifiedDocumentContextParameter/ID` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocumentContext/GuidelineSpecifiedDocumentContextParameter/ID` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/BuyerTradeParty/URIUniversalCommunication/URIID@schemeID` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/SellerTradeParty/GlobalID@schemeID` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/SellerTradeParty/URIUniversalCommunication/URIID@schemeID` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeDelivery/ActualDeliverySupplyChainEvent/OccurrenceDateTime/DateTimeString@format` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/BillingSpecifiedPeriod/EndDateTime/DateTimeString@format` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/BillingSpecifiedPeriod/StartDateTime/DateTimeString@format` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/SpecifiedTradePaymentTerms/DueDateDateTime/DateTimeString@format` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/SpecifiedTradeSettlementPaymentMeans/TypeCode` |

### corpus\XML-Rechnung\CII\EN16931_1_Teilrechnung.cii.xml

94 of 131 values are on the page.

| Values | Kind | Element |
|---:|---|---|
| 4 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeDelivery/BilledQuantity@unitCode` |
| 4 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeSettlement/ApplicableTradeTax/CategoryCode` |
| 4 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeSettlement/ApplicableTradeTax/TypeCode` |
| 4 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedTradeProduct/GlobalID@schemeID` |
| 3 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/SpecifiedTradeAllowanceCharge/CategoryTradeTax/CategoryCode` |
| 3 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/SpecifiedTradeAllowanceCharge/CategoryTradeTax/TypeCode` |
| 3 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/SpecifiedTradeAllowanceCharge/ChargeIndicator/Indicator` |
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/SellerTradeParty/SpecifiedTaxRegistration/ID@schemeID` |
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/ApplicableTradeTax/CategoryCode` |
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/ApplicableTradeTax/TypeCode` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocument/IssueDateTime/DateTimeString@format` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocument/TypeCode` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocumentContext/GuidelineSpecifiedDocumentContextParameter/ID` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/SellerTradeParty/GlobalID@schemeID` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeDelivery/ActualDeliverySupplyChainEvent/OccurrenceDateTime/DateTimeString@format` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeAgreement/GrossPriceProductTradePrice/AppliedTradeAllowanceCharge/ChargeIndicator/Indicator` |

### corpus\XML-Rechnung\CII\EN16931_2_Teilrechnung.cii.xml

46 of 59 values are on the page.

| Values | Kind | Element |
|---:|---|---|
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/SellerTradeParty/SpecifiedTaxRegistration/ID@schemeID` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocument/IssueDateTime/DateTimeString@format` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocument/TypeCode` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocumentContext/GuidelineSpecifiedDocumentContextParameter/ID` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/SellerTradeParty/GlobalID@schemeID` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeDelivery/ActualDeliverySupplyChainEvent/OccurrenceDateTime/DateTimeString@format` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/ApplicableTradeTax/CategoryCode` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/ApplicableTradeTax/TypeCode` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeDelivery/BilledQuantity@unitCode` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeSettlement/ApplicableTradeTax/CategoryCode` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeSettlement/ApplicableTradeTax/TypeCode` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedTradeProduct/GlobalID@schemeID` |

### corpus\XML-Rechnung\CII\EN16931_AbweichenderZahlungsempf.cii.xml

59 of 79 values are on the page.

| Values | Kind | Element |
|---:|---|---|
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/SellerTradeParty/SpecifiedTaxRegistration/ID@schemeID` |
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/ApplicableTradeTax/CategoryCode` |
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/ApplicableTradeTax/TypeCode` |
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeDelivery/BilledQuantity@unitCode` |
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeSettlement/ApplicableTradeTax/CategoryCode` |
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeSettlement/ApplicableTradeTax/TypeCode` |
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedTradeProduct/GlobalID@schemeID` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocument/IssueDateTime/DateTimeString@format` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocument/TypeCode` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocumentContext/GuidelineSpecifiedDocumentContextParameter/ID` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/SellerTradeParty/GlobalID@schemeID` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeDelivery/ActualDeliverySupplyChainEvent/OccurrenceDateTime/DateTimeString@format` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/SpecifiedTradeSettlementPaymentMeans/TypeCode` |

### corpus\XML-Rechnung\CII\EN16931_Betriebskostenabrechnung.cii.xml

48 of 64 values are on the page.

| Values | Kind | Element |
|---:|---|---|
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/SellerTradeParty/SpecifiedTaxRegistration/ID@schemeID` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocument/IssueDateTime/DateTimeString@format` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocument/TypeCode` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocumentContext/GuidelineSpecifiedDocumentContextParameter/ID` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/AdditionalReferencedDocument/AttachmentBinaryObject@mimeCode` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/AdditionalReferencedDocument/TypeCode` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/ApplicableTradeTax/CategoryCode` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/ApplicableTradeTax/TypeCode` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/BillingSpecifiedPeriod/EndDateTime/DateTimeString@format` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/BillingSpecifiedPeriod/StartDateTime/DateTimeString@format` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/SpecifiedTradePaymentTerms/DueDateDateTime/DateTimeString@format` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeDelivery/BilledQuantity@unitCode` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeSettlement/ApplicableTradeTax/CategoryCode` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeSettlement/ApplicableTradeTax/TypeCode` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedTradeProduct/GlobalID@schemeID` |

### corpus\XML-Rechnung\CII\EN16931_Einfach.cii.xml

56 of 75 values are on the page.

| Values | Kind | Element |
|---:|---|---|
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/SellerTradeParty/SpecifiedTaxRegistration/ID@schemeID` |
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/ApplicableTradeTax/CategoryCode` |
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/ApplicableTradeTax/TypeCode` |
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeDelivery/BilledQuantity@unitCode` |
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeSettlement/ApplicableTradeTax/CategoryCode` |
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeSettlement/ApplicableTradeTax/TypeCode` |
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedTradeProduct/GlobalID@schemeID` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocument/IssueDateTime/DateTimeString@format` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocument/TypeCode` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocumentContext/GuidelineSpecifiedDocumentContextParameter/ID` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/SellerTradeParty/GlobalID@schemeID` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeDelivery/ActualDeliverySupplyChainEvent/OccurrenceDateTime/DateTimeString@format` |

### corpus\XML-Rechnung\CII\EN16931_Einfach_DueDate.cii.xml

56 of 76 values are on the page.

| Values | Kind | Element |
|---:|---|---|
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/SellerTradeParty/SpecifiedTaxRegistration/ID@schemeID` |
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/ApplicableTradeTax/CategoryCode` |
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/ApplicableTradeTax/TypeCode` |
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeDelivery/BilledQuantity@unitCode` |
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeSettlement/ApplicableTradeTax/CategoryCode` |
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeSettlement/ApplicableTradeTax/TypeCode` |
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedTradeProduct/GlobalID@schemeID` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocument/IssueDateTime/DateTimeString@format` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocument/TypeCode` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocumentContext/GuidelineSpecifiedDocumentContextParameter/ID` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/SellerTradeParty/GlobalID@schemeID` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeDelivery/ActualDeliverySupplyChainEvent/OccurrenceDateTime/DateTimeString@format` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/SpecifiedTradePaymentTerms/DueDateDateTime/DateTimeString@format` |

### corpus\XML-Rechnung\CII\EN16931_Einfach_negativePaymentDue.cii.xml

55 of 74 values are on the page.

| Values | Kind | Element |
|---:|---|---|
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/SellerTradeParty/SpecifiedTaxRegistration/ID@schemeID` |
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/ApplicableTradeTax/CategoryCode` |
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/ApplicableTradeTax/TypeCode` |
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeDelivery/BilledQuantity@unitCode` |
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeSettlement/ApplicableTradeTax/CategoryCode` |
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeSettlement/ApplicableTradeTax/TypeCode` |
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedTradeProduct/GlobalID@schemeID` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocument/IssueDateTime/DateTimeString@format` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocument/TypeCode` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocumentContext/GuidelineSpecifiedDocumentContextParameter/ID` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/SellerTradeParty/GlobalID@schemeID` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeDelivery/ActualDeliverySupplyChainEvent/OccurrenceDateTime/DateTimeString@format` |

### corpus\XML-Rechnung\CII\EN16931_Elektron.cii.xml

59 of 76 values are on the page.

| Values | Kind | Element |
|---:|---|---|
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/AdditionalReferencedDocument/TypeCode` |
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeDelivery/BilledQuantity@unitCode` |
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeSettlement/ApplicableTradeTax/CategoryCode` |
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeSettlement/ApplicableTradeTax/TypeCode` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocument/IssueDateTime/DateTimeString@format` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocument/TypeCode` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocumentContext/BusinessProcessSpecifiedDocumentContextParameter/ID` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocumentContext/GuidelineSpecifiedDocumentContextParameter/ID` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/SellerTradeParty/SpecifiedTaxRegistration/ID@schemeID` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeDelivery/ActualDeliverySupplyChainEvent/OccurrenceDateTime/DateTimeString@format` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/ApplicableTradeTax/CategoryCode` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/ApplicableTradeTax/TypeCode` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/SpecifiedTradeSettlementPaymentMeans/TypeCode` |

### corpus\XML-Rechnung\CII\EN16931_ElektronischeAdresse.cii.xml

80 of 108 values are on the page.

| Values | Kind | Element |
|---:|---|---|
| 6 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeDelivery/BilledQuantity@unitCode` |
| 6 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeSettlement/ApplicableTradeTax/CategoryCode` |
| 6 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeSettlement/ApplicableTradeTax/TypeCode` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocument/IssueDateTime/DateTimeString@format` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocument/TypeCode` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocumentContext/GuidelineSpecifiedDocumentContextParameter/ID` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/BuyerTradeParty/URIUniversalCommunication/URIID@schemeID` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/SellerTradeParty/GlobalID@schemeID` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/SellerTradeParty/SpecifiedTaxRegistration/ID@schemeID` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/SellerTradeParty/URIUniversalCommunication/URIID@schemeID` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeDelivery/ActualDeliverySupplyChainEvent/OccurrenceDateTime/DateTimeString@format` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/ApplicableTradeTax/CategoryCode` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/ApplicableTradeTax/TypeCode` |

### corpus\XML-Rechnung\CII\EN16931_Gutschrift.cii.xml

57 of 77 values are on the page.

| Values | Kind | Element |
|---:|---|---|
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/SellerTradeParty/SpecifiedTaxRegistration/ID@schemeID` |
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/ApplicableTradeTax/CategoryCode` |
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/ApplicableTradeTax/TypeCode` |
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeDelivery/BilledQuantity@unitCode` |
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeSettlement/ApplicableTradeTax/CategoryCode` |
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeSettlement/ApplicableTradeTax/TypeCode` |
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedTradeProduct/GlobalID@schemeID` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocument/IssueDateTime/DateTimeString@format` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocument/TypeCode` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocumentContext/GuidelineSpecifiedDocumentContextParameter/ID` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/BuyerTradeParty/SpecifiedTaxRegistration/ID@schemeID` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/SellerTradeParty/GlobalID@schemeID` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeDelivery/ActualDeliverySupplyChainEvent/OccurrenceDateTime/DateTimeString@format` |

### corpus\XML-Rechnung\CII\EN16931_Haftpflichtversicherung_Versicherungssteuer.cii.xml

47 of 60 values are on the page.

| Values | Kind | Element |
|---:|---|---|
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeDelivery/BilledQuantity@unitCode` |
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeSettlement/ApplicableTradeTax/CategoryCode` |
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeSettlement/ApplicableTradeTax/TypeCode` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocument/IssueDateTime/DateTimeString@format` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocument/TypeCode` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocumentContext/GuidelineSpecifiedDocumentContextParameter/ID` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/SellerTradeParty/SpecifiedTaxRegistration/ID@schemeID` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeDelivery/ActualDeliverySupplyChainEvent/OccurrenceDateTime/DateTimeString@format` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/ApplicableTradeTax/CategoryCode` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/ApplicableTradeTax/TypeCode` |

### corpus\XML-Rechnung\CII\EN16931_Innergemeinschaftliche_Lieferungen.cii.xml

78 of 106 values are on the page.

| Values | Kind | Element |
|---:|---|---|
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeAgreement/GrossPriceProductTradePrice/BasisQuantity@unitCode` |
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeAgreement/NetPriceProductTradePrice/BasisQuantity@unitCode` |
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeDelivery/BilledQuantity@unitCode` |
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeSettlement/ApplicableTradeTax/CategoryCode` |
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeSettlement/ApplicableTradeTax/TypeCode` |
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeSettlement/BillingSpecifiedPeriod/EndDateTime/DateTimeString@format` |
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeSettlement/BillingSpecifiedPeriod/StartDateTime/DateTimeString@format` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocument/IssueDateTime/DateTimeString@format` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocument/TypeCode` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocumentContext/BusinessProcessSpecifiedDocumentContextParameter/ID` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocumentContext/GuidelineSpecifiedDocumentContextParameter/ID` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/BuyerTradeParty/SpecifiedTaxRegistration/ID@schemeID` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/SellerTaxRepresentativeTradeParty/SpecifiedTaxRegistration/ID@schemeID` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/SellerTradeParty/SpecifiedTaxRegistration/ID@schemeID` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/ApplicableTradeTax/CategoryCode` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/ApplicableTradeTax/TypeCode` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/BillingSpecifiedPeriod/EndDateTime/DateTimeString@format` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/BillingSpecifiedPeriod/StartDateTime/DateTimeString@format` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/PayeeTradeParty/GlobalID@schemeID` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/SpecifiedTradePaymentTerms/DueDateDateTime/DateTimeString@format` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/SpecifiedTradeSettlementPaymentMeans/TypeCode` |

### corpus\XML-Rechnung\CII\EN16931_Kraftfahrversicherung_Bruttopreise.cii.xml

49 of 62 values are on the page.

| Values | Kind | Element |
|---:|---|---|
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeDelivery/BilledQuantity@unitCode` |
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeSettlement/ApplicableTradeTax/CategoryCode` |
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeSettlement/ApplicableTradeTax/TypeCode` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocument/IssueDateTime/DateTimeString@format` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocument/TypeCode` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocumentContext/GuidelineSpecifiedDocumentContextParameter/ID` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/SellerTradeParty/SpecifiedTaxRegistration/ID@schemeID` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeDelivery/ActualDeliverySupplyChainEvent/OccurrenceDateTime/DateTimeString@format` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/ApplicableTradeTax/CategoryCode` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/ApplicableTradeTax/TypeCode` |

### corpus\XML-Rechnung\CII\EN16931_Miete.cii.xml

78 of 105 values are on the page.

| Values | Kind | Element |
|---:|---|---|
| 6 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeDelivery/BilledQuantity@unitCode` |
| 6 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeSettlement/ApplicableTradeTax/CategoryCode` |
| 6 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeSettlement/ApplicableTradeTax/TypeCode` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocument/IssueDateTime/DateTimeString@format` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocument/TypeCode` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocumentContext/BusinessProcessSpecifiedDocumentContextParameter/ID` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocumentContext/GuidelineSpecifiedDocumentContextParameter/ID` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/SellerTradeParty/GlobalID@schemeID` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/SellerTradeParty/SpecifiedTaxRegistration/ID@schemeID` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeDelivery/ActualDeliverySupplyChainEvent/OccurrenceDateTime/DateTimeString@format` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/ApplicableTradeTax/CategoryCode` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/ApplicableTradeTax/TypeCode` |

### corpus\XML-Rechnung\CII\EN16931_OEPNV.cii.xml

44 of 56 values are on the page.

| Values | Kind | Element |
|---:|---|---|
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocument/IssueDateTime/DateTimeString@format` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocument/TypeCode` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocumentContext/GuidelineSpecifiedDocumentContextParameter/ID` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/SellerTradeParty/SpecifiedTaxRegistration/ID@schemeID` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeDelivery/ActualDeliverySupplyChainEvent/OccurrenceDateTime/DateTimeString@format` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/ApplicableTradeTax/CategoryCode` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/ApplicableTradeTax/TypeCode` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/SpecifiedTradePaymentTerms/DueDateDateTime/DateTimeString@format` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/SpecifiedTradeSettlementPaymentMeans/TypeCode` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeDelivery/BilledQuantity@unitCode` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeSettlement/ApplicableTradeTax/CategoryCode` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeSettlement/ApplicableTradeTax/TypeCode` |

### corpus\XML-Rechnung\CII\EN16931_Physiotherapeut.cii.xml

45 of 59 values are on the page.

| Values | Kind | Element |
|---:|---|---|
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeDelivery/BilledQuantity@unitCode` |
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeSettlement/ApplicableTradeTax/CategoryCode` |
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeSettlement/ApplicableTradeTax/TypeCode` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocument/IssueDateTime/DateTimeString@format` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocument/TypeCode` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocumentContext/GuidelineSpecifiedDocumentContextParameter/ID` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/SellerTradeParty/SpecifiedTaxRegistration/ID@schemeID` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/ApplicableTradeTax/CategoryCode` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/ApplicableTradeTax/TypeCode` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/SpecifiedTradePaymentTerms/DueDateDateTime/DateTimeString@format` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/SpecifiedTradeSettlementPaymentMeans/TypeCode` |

### corpus\XML-Rechnung\CII\EN16931_Rabatte.cii.xml

92 of 129 values are on the page.

| Values | Kind | Element |
|---:|---|---|
| 4 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeDelivery/BilledQuantity@unitCode` |
| 4 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeSettlement/ApplicableTradeTax/CategoryCode` |
| 4 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeSettlement/ApplicableTradeTax/TypeCode` |
| 4 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedTradeProduct/GlobalID@schemeID` |
| 3 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/SpecifiedTradeAllowanceCharge/CategoryTradeTax/CategoryCode` |
| 3 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/SpecifiedTradeAllowanceCharge/CategoryTradeTax/TypeCode` |
| 3 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/SpecifiedTradeAllowanceCharge/ChargeIndicator/Indicator` |
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/SellerTradeParty/SpecifiedTaxRegistration/ID@schemeID` |
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/ApplicableTradeTax/CategoryCode` |
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/ApplicableTradeTax/TypeCode` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocument/IssueDateTime/DateTimeString@format` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocument/TypeCode` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocumentContext/GuidelineSpecifiedDocumentContextParameter/ID` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/SellerTradeParty/GlobalID@schemeID` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeDelivery/ActualDeliverySupplyChainEvent/OccurrenceDateTime/DateTimeString@format` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeAgreement/GrossPriceProductTradePrice/AppliedTradeAllowanceCharge/ChargeIndicator/Indicator` |

### corpus\XML-Rechnung\CII\EN16931_Rechnungskorrektur.cii.xml

75 of 106 values are on the page.

| Values | Kind | Element |
|---:|---|---|
| 4 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/SpecifiedTradeAllowanceCharge/CategoryTradeTax/CategoryCode` |
| 4 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/SpecifiedTradeAllowanceCharge/CategoryTradeTax/TypeCode` |
| 4 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/SpecifiedTradeAllowanceCharge/ChargeIndicator/Indicator` |
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/ApplicableTradeTax/CategoryCode` |
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/ApplicableTradeTax/TypeCode` |
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeDelivery/BilledQuantity@unitCode` |
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeSettlement/ApplicableTradeTax/CategoryCode` |
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeSettlement/ApplicableTradeTax/TypeCode` |
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedTradeProduct/GlobalID@schemeID` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocument/IssueDateTime/DateTimeString@format` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocument/TypeCode` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocumentContext/GuidelineSpecifiedDocumentContextParameter/ID` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/SellerTradeParty/GlobalID@schemeID` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/SellerTradeParty/SpecifiedTaxRegistration/ID@schemeID` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeDelivery/ActualDeliverySupplyChainEvent/OccurrenceDateTime/DateTimeString@format` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeAgreement/GrossPriceProductTradePrice/AppliedTradeAllowanceCharge/ChargeIndicator/Indicator` |

### corpus\XML-Rechnung\CII\EN16931_RechnungsUebertragung.cii.xml

80 of 108 values are on the page.

| Values | Kind | Element |
|---:|---|---|
| 6 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeDelivery/BilledQuantity@unitCode` |
| 6 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeSettlement/ApplicableTradeTax/CategoryCode` |
| 6 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeSettlement/ApplicableTradeTax/TypeCode` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocument/IssueDateTime/DateTimeString@format` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocument/TypeCode` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocumentContext/GuidelineSpecifiedDocumentContextParameter/ID` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/BuyerTradeParty/URIUniversalCommunication/URIID@schemeID` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/SellerTradeParty/GlobalID@schemeID` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/SellerTradeParty/SpecifiedTaxRegistration/ID@schemeID` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/SellerTradeParty/URIUniversalCommunication/URIID@schemeID` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeDelivery/ActualDeliverySupplyChainEvent/OccurrenceDateTime/DateTimeString@format` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/ApplicableTradeTax/CategoryCode` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/ApplicableTradeTax/TypeCode` |

### corpus\XML-Rechnung\CII\EN16931_Reisekostenabrechnung.cii.xml

79 of 105 values are on the page.

| Values | Kind | Element |
|---:|---|---|
| 4 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeDelivery/BilledQuantity@unitCode` |
| 4 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeSettlement/ApplicableTradeTax/CategoryCode` |
| 4 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeSettlement/ApplicableTradeTax/TypeCode` |
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/AdditionalReferencedDocument/TypeCode` |
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/SellerTradeParty/SpecifiedTaxRegistration/ID@schemeID` |
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/ApplicableTradeTax/CategoryCode` |
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/ApplicableTradeTax/TypeCode` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocument/IssueDateTime/DateTimeString@format` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocument/TypeCode` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocumentContext/GuidelineSpecifiedDocumentContextParameter/ID` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/SellerTradeParty/GlobalID@schemeID` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/BillingSpecifiedPeriod/EndDateTime/DateTimeString@format` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/BillingSpecifiedPeriod/StartDateTime/DateTimeString@format` |

### corpus\XML-Rechnung\CII\EN16931_Sachversicherung_berechneter_Steuersatz.cii.xml

42 of 53 values are on the page.

| Values | Kind | Element |
|---:|---|---|
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocument/IssueDateTime/DateTimeString@format` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocument/TypeCode` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocumentContext/GuidelineSpecifiedDocumentContextParameter/ID` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/SellerTradeParty/GlobalID@schemeID` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/SellerTradeParty/SpecifiedTaxRegistration/ID@schemeID` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeDelivery/ActualDeliverySupplyChainEvent/OccurrenceDateTime/DateTimeString@format` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/ApplicableTradeTax/CategoryCode` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/ApplicableTradeTax/TypeCode` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeDelivery/BilledQuantity@unitCode` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeSettlement/ApplicableTradeTax/CategoryCode` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeSettlement/ApplicableTradeTax/TypeCode` |

### corpus\XML-Rechnung\CII\EN16931_SEPA_Prenotification.cii.xml

61 of 81 values are on the page.

| Values | Kind | Element |
|---:|---|---|
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/SellerTradeParty/SpecifiedTaxRegistration/ID@schemeID` |
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/ApplicableTradeTax/CategoryCode` |
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/ApplicableTradeTax/TypeCode` |
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeDelivery/BilledQuantity@unitCode` |
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeSettlement/ApplicableTradeTax/CategoryCode` |
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeSettlement/ApplicableTradeTax/TypeCode` |
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedTradeProduct/GlobalID@schemeID` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocument/IssueDateTime/DateTimeString@format` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocument/TypeCode` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocumentContext/GuidelineSpecifiedDocumentContextParameter/ID` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/SellerTradeParty/GlobalID@schemeID` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeDelivery/ActualDeliverySupplyChainEvent/OccurrenceDateTime/DateTimeString@format` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/SpecifiedTradeSettlementPaymentMeans/TypeCode` |

### corpus\XML-Rechnung\CII\not_validating_full_invoice_based_onTest_EeISI_300_CENfullmodel.cii.xml

161 of 223 values are on the page.

| Values | Kind | Element |
|---:|---|---|
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/AdditionalReferencedDocument/TypeCode` |
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/SellerTradeParty/GlobalID@schemeID` |
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/SellerTradeParty/SpecifiedTaxRegistration/ID@schemeID` |
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/ApplicableTradeTax/CategoryCode` |
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/ApplicableTradeTax/TypeCode` |
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/SpecifiedTradeAllowanceCharge/CategoryTradeTax/CategoryCode` |
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/SpecifiedTradeAllowanceCharge/CategoryTradeTax/TypeCode` |
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/SpecifiedTradeAllowanceCharge/ChargeIndicator/Indicator` |
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/SpecifiedTradeAllowanceCharge/ReasonCode` |
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeDelivery/BilledQuantity@unitCode` |
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeSettlement/ApplicableTradeTax/CategoryCode` |
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeSettlement/ApplicableTradeTax/TypeCode` |
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeSettlement/SpecifiedTradeAllowanceCharge/ChargeIndicator/Indicator` |
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeSettlement/SpecifiedTradeAllowanceCharge/ReasonCode` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocument/IssueDateTime/DateTimeString@format` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocument/TypeCode` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocumentContext/BusinessProcessSpecifiedDocumentContextParameter/ID` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocumentContext/GuidelineSpecifiedDocumentContextParameter/ID` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/AdditionalReferencedDocument/AttachmentBinaryObject` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/AdditionalReferencedDocument/AttachmentBinaryObject@mimeCode` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/AdditionalReferencedDocument/ReferenceTypeCode` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/BuyerTradeParty/GlobalID@schemeID` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/BuyerTradeParty/SpecifiedLegalOrganization/ID@schemeID` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/BuyerTradeParty/SpecifiedTaxRegistration/ID@schemeID` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/BuyerTradeParty/URIUniversalCommunication/URIID@schemeID` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/SellerTaxRepresentativeTradeParty/SpecifiedTaxRegistration/ID@schemeID` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/SellerTradeParty/URIUniversalCommunication/URIID@schemeID` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeDelivery/ActualDeliverySupplyChainEvent/OccurrenceDateTime/DateTimeString@format` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeDelivery/ShipToTradeParty/GlobalID@schemeID` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/ApplicableTradeTax/ExemptionReasonCode` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/BillingSpecifiedPeriod/EndDateTime/DateTimeString@format` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/BillingSpecifiedPeriod/StartDateTime/DateTimeString@format` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/InvoiceReferencedDocument/FormattedIssueDateTime/DateTimeString@format` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/PayeeTradeParty/GlobalID@schemeID` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/PayeeTradeParty/SpecifiedLegalOrganization/ID@schemeID` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/SpecifiedTradePaymentTerms/DueDateDateTime/DateTimeString@format` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/SpecifiedTradeSettlementPaymentMeans/TypeCode` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeAgreement/GrossPriceProductTradePrice/AppliedTradeAllowanceCharge/ChargeIndicator/Indicator` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeAgreement/GrossPriceProductTradePrice/BasisQuantity@unitCode` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeAgreement/NetPriceProductTradePrice/BasisQuantity@unitCode` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeSettlement/AdditionalReferencedDocument/TypeCode` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeSettlement/BillingSpecifiedPeriod/EndDateTime/DateTimeString@format` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeSettlement/BillingSpecifiedPeriod/StartDateTime/DateTimeString@format` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedTradeProduct/DesignatedProductClassification/ClassCode` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedTradeProduct/DesignatedProductClassification/ClassCode@listID` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedTradeProduct/DesignatedProductClassification/ClassCode@listVersionID` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedTradeProduct/GlobalID@schemeID` |
| 1 | known gap | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeDelivery/ReceivingAdviceReferencedDocument/IssuerAssignedID` |

### corpus\XML-Rechnung\CII\XRECHNUNG_Betriebskostenabrechnung.cii.xml

61 of 84 values are on the page.

| Values | Kind | Element |
|---:|---|---|
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/SellerTradeParty/SpecifiedTaxRegistration/ID@schemeID` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocument/IssueDateTime/DateTimeString@format` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocument/TypeCode` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocumentContext/BusinessProcessSpecifiedDocumentContextParameter/ID` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocumentContext/GuidelineSpecifiedDocumentContextParameter/ID` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/AdditionalReferencedDocument/AttachmentBinaryObject` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/AdditionalReferencedDocument/AttachmentBinaryObject@mimeCode` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/AdditionalReferencedDocument/TypeCode` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/BuyerTradeParty/URIUniversalCommunication/URIID@schemeID` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/SellerTradeParty/GlobalID@schemeID` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/SellerTradeParty/URIUniversalCommunication/URIID@schemeID` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeDelivery/ActualDeliverySupplyChainEvent/OccurrenceDateTime/DateTimeString@format` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/ApplicableTradeTax/CategoryCode` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/ApplicableTradeTax/TypeCode` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/BillingSpecifiedPeriod/EndDateTime/DateTimeString@format` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/BillingSpecifiedPeriod/StartDateTime/DateTimeString@format` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/SpecifiedTradePaymentTerms/DueDateDateTime/DateTimeString@format` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/SpecifiedTradeSettlementPaymentMeans/TypeCode` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeDelivery/BilledQuantity@unitCode` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeSettlement/ApplicableTradeTax/CategoryCode` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeSettlement/ApplicableTradeTax/TypeCode` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedTradeProduct/GlobalID@schemeID` |

### corpus\XML-Rechnung\CII\XRECHNUNG_Einfach.cii.xml

66 of 89 values are on the page.

| Values | Kind | Element |
|---:|---|---|
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/SellerTradeParty/SpecifiedTaxRegistration/ID@schemeID` |
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/ApplicableTradeTax/CategoryCode` |
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/ApplicableTradeTax/TypeCode` |
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeDelivery/BilledQuantity@unitCode` |
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeSettlement/ApplicableTradeTax/CategoryCode` |
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeSettlement/ApplicableTradeTax/TypeCode` |
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedTradeProduct/GlobalID@schemeID` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocument/IssueDateTime/DateTimeString@format` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocument/TypeCode` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocumentContext/BusinessProcessSpecifiedDocumentContextParameter/ID` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocumentContext/GuidelineSpecifiedDocumentContextParameter/ID` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/BuyerTradeParty/URIUniversalCommunication/URIID@schemeID` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/SellerTradeParty/GlobalID@schemeID` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/SellerTradeParty/URIUniversalCommunication/URIID@schemeID` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeDelivery/ActualDeliverySupplyChainEvent/OccurrenceDateTime/DateTimeString@format` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/SpecifiedTradeSettlementPaymentMeans/TypeCode` |

### corpus\XML-Rechnung\CII\XRECHNUNG_Elektron.cii.xml

68 of 93 values are on the page.

| Values | Kind | Element |
|---:|---|---|
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/AdditionalReferencedDocument/AttachmentBinaryObject` |
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/AdditionalReferencedDocument/AttachmentBinaryObject@mimeCode` |
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/AdditionalReferencedDocument/TypeCode` |
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/SellerTradeParty/SpecifiedTaxRegistration/ID@schemeID` |
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeDelivery/BilledQuantity@unitCode` |
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeSettlement/ApplicableTradeTax/CategoryCode` |
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeSettlement/ApplicableTradeTax/TypeCode` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocument/IssueDateTime/DateTimeString@format` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocument/TypeCode` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocumentContext/BusinessProcessSpecifiedDocumentContextParameter/ID` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocumentContext/GuidelineSpecifiedDocumentContextParameter/ID` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/BuyerTradeParty/URIUniversalCommunication/URIID@schemeID` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/SellerTradeParty/GlobalID@schemeID` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/SellerTradeParty/URIUniversalCommunication/URIID@schemeID` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeDelivery/ActualDeliverySupplyChainEvent/OccurrenceDateTime/DateTimeString@format` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/ApplicableTradeTax/CategoryCode` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/ApplicableTradeTax/TypeCode` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/SpecifiedTradeSettlementPaymentMeans/TypeCode` |

### corpus\XML-Rechnung\CII\XRECHNUNG_Reisekostenabrechnung.cii.xml

92 of 127 values are on the page.

| Values | Kind | Element |
|---:|---|---|
| 4 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeDelivery/BilledQuantity@unitCode` |
| 4 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeSettlement/ApplicableTradeTax/CategoryCode` |
| 4 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeSettlement/ApplicableTradeTax/TypeCode` |
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/AdditionalReferencedDocument/AttachmentBinaryObject` |
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/AdditionalReferencedDocument/AttachmentBinaryObject@mimeCode` |
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/AdditionalReferencedDocument/TypeCode` |
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/SellerTradeParty/SpecifiedTaxRegistration/ID@schemeID` |
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/ApplicableTradeTax/CategoryCode` |
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/ApplicableTradeTax/TypeCode` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocument/IssueDateTime/DateTimeString@format` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocument/TypeCode` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocumentContext/BusinessProcessSpecifiedDocumentContextParameter/ID` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocumentContext/GuidelineSpecifiedDocumentContextParameter/ID` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/BuyerTradeParty/URIUniversalCommunication/URIID@schemeID` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/SellerTradeParty/GlobalID@schemeID` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/SellerTradeParty/URIUniversalCommunication/URIID@schemeID` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeDelivery/ActualDeliverySupplyChainEvent/OccurrenceDateTime/DateTimeString@format` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/BillingSpecifiedPeriod/EndDateTime/DateTimeString@format` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/BillingSpecifiedPeriod/StartDateTime/DateTimeString@format` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/SpecifiedTradeSettlementPaymentMeans/TypeCode` |

### corpus\ZUGFeRDv2\correct\Mustangproject\hate1.xml

56 of 75 values are on the page.

| Values | Kind | Element |
|---:|---|---|
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeAgreement/GrossPriceProductTradePrice/BasisQuantity@unitCode` |
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeAgreement/NetPriceProductTradePrice/BasisQuantity@unitCode` |
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeDelivery/BilledQuantity@unitCode` |
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeSettlement/ApplicableTradeTax/CategoryCode` |
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeSettlement/ApplicableTradeTax/TypeCode` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocument/IssueDateTime/DateTimeString@format` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocument/TypeCode` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocumentContext/GuidelineSpecifiedDocumentContextParameter/ID` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/SellerTradeParty/SpecifiedTaxRegistration/ID@schemeID` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeDelivery/ActualDeliverySupplyChainEvent/OccurrenceDateTime/DateTimeString@format` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/ApplicableTradeTax/CategoryCode` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/ApplicableTradeTax/TypeCode` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/SpecifiedTradePaymentTerms/DueDateDateTime/DateTimeString@format` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/SpecifiedTradeSettlementPaymentMeans/TypeCode` |

### corpus\ZUGFeRDv2\correct\Mustangproject\testout-XR-Edge.xml

53 of 74 values are on the page.

| Values | Kind | Element |
|---:|---|---|
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/AdditionalReferencedDocument/AttachmentBinaryObject` |
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/AdditionalReferencedDocument/AttachmentBinaryObject@mimeCode` |
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/AdditionalReferencedDocument/TypeCode` |
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/SellerTradeParty/SpecifiedTaxRegistration/ID@schemeID` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocument/IssueDateTime/DateTimeString@format` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocument/TypeCode` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocumentContext/GuidelineSpecifiedDocumentContextParameter/ID` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeDelivery/ActualDeliverySupplyChainEvent/OccurrenceDateTime/DateTimeString@format` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/ApplicableTradeTax/CategoryCode` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/ApplicableTradeTax/TypeCode` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/SpecifiedTradePaymentTerms/DueDateDateTime/DateTimeString@format` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/SpecifiedTradeSettlementPaymentMeans/TypeCode` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeAgreement/GrossPriceProductTradePrice/BasisQuantity@unitCode` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeAgreement/NetPriceProductTradePrice/BasisQuantity@unitCode` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeDelivery/BilledQuantity@unitCode` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeSettlement/ApplicableTradeTax/CategoryCode` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeSettlement/ApplicableTradeTax/TypeCode` |

### corpus\ZUGFeRDv2\correct\Mustangproject\testout-XR.xml

47 of 62 values are on the page.

| Values | Kind | Element |
|---:|---|---|
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/SellerTradeParty/SpecifiedTaxRegistration/ID@schemeID` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocument/IssueDateTime/DateTimeString@format` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocument/TypeCode` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocumentContext/GuidelineSpecifiedDocumentContextParameter/ID` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeDelivery/ActualDeliverySupplyChainEvent/OccurrenceDateTime/DateTimeString@format` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/ApplicableTradeTax/CategoryCode` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/ApplicableTradeTax/TypeCode` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/SpecifiedTradePaymentTerms/DueDateDateTime/DateTimeString@format` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/SpecifiedTradeSettlementPaymentMeans/TypeCode` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeAgreement/GrossPriceProductTradePrice/BasisQuantity@unitCode` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeAgreement/NetPriceProductTradePrice/BasisQuantity@unitCode` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeDelivery/BilledQuantity@unitCode` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeSettlement/ApplicableTradeTax/CategoryCode` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeSettlement/ApplicableTradeTax/TypeCode` |

### corpus\ZUGFeRDv2\correct\Mustangproject\testout-ZF2PushAllowances.xml

43 of 60 values are on the page.

| Values | Kind | Element |
|---:|---|---|
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/SellerTradeParty/SpecifiedTaxRegistration/ID@schemeID` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocument/IssueDateTime/DateTimeString@format` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocument/TypeCode` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocumentContext/GuidelineSpecifiedDocumentContextParameter/ID` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeDelivery/ActualDeliverySupplyChainEvent/OccurrenceDateTime/DateTimeString@format` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/ApplicableTradeTax/CategoryCode` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/ApplicableTradeTax/TypeCode` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/SpecifiedTradeAllowanceCharge/CategoryTradeTax/CategoryCode` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/SpecifiedTradeAllowanceCharge/CategoryTradeTax/TypeCode` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/SpecifiedTradeAllowanceCharge/ChargeIndicator/Indicator` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeAgreement/GrossPriceProductTradePrice/AppliedTradeAllowanceCharge/ChargeIndicator/Indicator` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeAgreement/GrossPriceProductTradePrice/BasisQuantity@unitCode` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeAgreement/NetPriceProductTradePrice/BasisQuantity@unitCode` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeDelivery/BilledQuantity@unitCode` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeSettlement/ApplicableTradeTax/CategoryCode` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeSettlement/ApplicableTradeTax/TypeCode` |

### corpus\ZUGFeRDv2\correct\Mustangproject\testout-ZF2PushChargesAllowances.xml

63 of 92 values are on the page.

| Values | Kind | Element |
|---:|---|---|
| 3 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeAgreement/GrossPriceProductTradePrice/BasisQuantity@unitCode` |
| 3 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeAgreement/NetPriceProductTradePrice/BasisQuantity@unitCode` |
| 3 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeDelivery/BilledQuantity@unitCode` |
| 3 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeSettlement/ApplicableTradeTax/CategoryCode` |
| 3 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeSettlement/ApplicableTradeTax/TypeCode` |
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/SellerTradeParty/SpecifiedTaxRegistration/ID@schemeID` |
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/SpecifiedTradeAllowanceCharge/CategoryTradeTax/CategoryCode` |
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/SpecifiedTradeAllowanceCharge/CategoryTradeTax/TypeCode` |
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/SpecifiedTradeAllowanceCharge/ChargeIndicator/Indicator` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocument/IssueDateTime/DateTimeString@format` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocument/TypeCode` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocumentContext/GuidelineSpecifiedDocumentContextParameter/ID` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeDelivery/ActualDeliverySupplyChainEvent/OccurrenceDateTime/DateTimeString@format` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/ApplicableTradeTax/CategoryCode` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/ApplicableTradeTax/TypeCode` |

### corpus\ZUGFeRDv2\correct\Mustangproject\testout-ZF2PushCorrection.xml

60 of 86 values are on the page.

| Values | Kind | Element |
|---:|---|---|
| 3 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeAgreement/GrossPriceProductTradePrice/BasisQuantity@unitCode` |
| 3 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeAgreement/NetPriceProductTradePrice/BasisQuantity@unitCode` |
| 3 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeDelivery/BilledQuantity@unitCode` |
| 3 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeSettlement/ApplicableTradeTax/CategoryCode` |
| 3 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeSettlement/ApplicableTradeTax/TypeCode` |
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/SellerTradeParty/SpecifiedTaxRegistration/ID@schemeID` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocument/IssueDateTime/DateTimeString@format` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocument/TypeCode` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocumentContext/GuidelineSpecifiedDocumentContextParameter/ID` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/BuyerTradeParty/SpecifiedTaxRegistration/ID@schemeID` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeDelivery/ActualDeliverySupplyChainEvent/OccurrenceDateTime/DateTimeString@format` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/ApplicableTradeTax/CategoryCode` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/ApplicableTradeTax/TypeCode` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/BillingSpecifiedPeriod/EndDateTime/DateTimeString@format` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/BillingSpecifiedPeriod/StartDateTime/DateTimeString@format` |

### corpus\ZUGFeRDv2\correct\Mustangproject\testout-ZF2PushCreditNote.xml

64 of 92 values are on the page.

| Values | Kind | Element |
|---:|---|---|
| 3 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeAgreement/GrossPriceProductTradePrice/BasisQuantity@unitCode` |
| 3 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeAgreement/NetPriceProductTradePrice/BasisQuantity@unitCode` |
| 3 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeDelivery/BilledQuantity@unitCode` |
| 3 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeSettlement/ApplicableTradeTax/CategoryCode` |
| 3 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeSettlement/ApplicableTradeTax/TypeCode` |
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/SellerTradeParty/SpecifiedTaxRegistration/ID@schemeID` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocument/IssueDateTime/DateTimeString@format` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocument/TypeCode` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocumentContext/GuidelineSpecifiedDocumentContextParameter/ID` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/BuyerTradeParty/SpecifiedTaxRegistration/ID@schemeID` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeDelivery/ActualDeliverySupplyChainEvent/OccurrenceDateTime/DateTimeString@format` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/ApplicableTradeTax/CategoryCode` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/ApplicableTradeTax/TypeCode` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/BillingSpecifiedPeriod/EndDateTime/DateTimeString@format` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/BillingSpecifiedPeriod/StartDateTime/DateTimeString@format` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/SpecifiedTradePaymentTerms/DueDateDateTime/DateTimeString@format` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/SpecifiedTradeSettlementPaymentMeans/TypeCode` |

### corpus\ZUGFeRDv2\correct\Mustangproject\testout-ZF2PushEdge.xml

63 of 88 values are on the page.

| Values | Kind | Element |
|---:|---|---|
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/SellerTradeParty/SpecifiedTaxRegistration/ID@schemeID` |
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/SpecifiedTradeAllowanceCharge/CategoryTradeTax/CategoryCode` |
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/SpecifiedTradeAllowanceCharge/CategoryTradeTax/TypeCode` |
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/SpecifiedTradeAllowanceCharge/ChargeIndicator/Indicator` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocument/IssueDateTime/DateTimeString@format` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocument/TypeCode` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocumentContext/GuidelineSpecifiedDocumentContextParameter/ID` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/BuyerTradeParty/SpecifiedTaxRegistration/ID@schemeID` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeDelivery/ActualDeliverySupplyChainEvent/OccurrenceDateTime/DateTimeString@format` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/ApplicableTradeTax/CategoryCode` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/ApplicableTradeTax/TypeCode` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/BillingSpecifiedPeriod/EndDateTime/DateTimeString@format` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/BillingSpecifiedPeriod/StartDateTime/DateTimeString@format` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeAgreement/GrossPriceProductTradePrice/AppliedTradeAllowanceCharge/ChargeIndicator/Indicator` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeAgreement/GrossPriceProductTradePrice/BasisQuantity@unitCode` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeAgreement/NetPriceProductTradePrice/BasisQuantity@unitCode` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeDelivery/BilledQuantity@unitCode` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeSettlement/ApplicableTradeTax/CategoryCode` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeSettlement/ApplicableTradeTax/TypeCode` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeSettlement/BillingSpecifiedPeriod/EndDateTime/DateTimeString@format` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeSettlement/BillingSpecifiedPeriod/StartDateTime/DateTimeString@format` |

### corpus\ZUGFeRDv2\correct\Mustangproject\testout-ZF2PushItemChargesAllowances.xml

77 of 110 values are on the page.

| Values | Kind | Element |
|---:|---|---|
| 5 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeAgreement/GrossPriceProductTradePrice/AppliedTradeAllowanceCharge/ChargeIndicator/Indicator` |
| 4 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeAgreement/GrossPriceProductTradePrice/BasisQuantity@unitCode` |
| 4 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeAgreement/NetPriceProductTradePrice/BasisQuantity@unitCode` |
| 4 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeDelivery/BilledQuantity@unitCode` |
| 4 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeSettlement/ApplicableTradeTax/CategoryCode` |
| 4 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeSettlement/ApplicableTradeTax/TypeCode` |
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/SellerTradeParty/SpecifiedTaxRegistration/ID@schemeID` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocument/IssueDateTime/DateTimeString@format` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocument/TypeCode` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocumentContext/GuidelineSpecifiedDocumentContextParameter/ID` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeDelivery/ActualDeliverySupplyChainEvent/OccurrenceDateTime/DateTimeString@format` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/ApplicableTradeTax/CategoryCode` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/ApplicableTradeTax/TypeCode` |

### corpus\ZUGFeRDv2\correct\Mustangproject\testout-ZF2PushRelativeChargesAllowances.xml

63 of 90 values are on the page.

| Values | Kind | Element |
|---:|---|---|
| 3 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeAgreement/GrossPriceProductTradePrice/BasisQuantity@unitCode` |
| 3 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeAgreement/NetPriceProductTradePrice/BasisQuantity@unitCode` |
| 3 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeDelivery/BilledQuantity@unitCode` |
| 3 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeSettlement/ApplicableTradeTax/CategoryCode` |
| 3 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeSettlement/ApplicableTradeTax/TypeCode` |
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/SellerTradeParty/SpecifiedTaxRegistration/ID@schemeID` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocument/IssueDateTime/DateTimeString@format` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocument/TypeCode` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocumentContext/GuidelineSpecifiedDocumentContextParameter/ID` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeDelivery/ActualDeliverySupplyChainEvent/OccurrenceDateTime/DateTimeString@format` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/ApplicableTradeTax/CategoryCode` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/ApplicableTradeTax/TypeCode` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/SpecifiedTradeAllowanceCharge/CategoryTradeTax/CategoryCode` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/SpecifiedTradeAllowanceCharge/CategoryTradeTax/TypeCode` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/SpecifiedTradeAllowanceCharge/ChargeIndicator/Indicator` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeAgreement/GrossPriceProductTradePrice/AppliedTradeAllowanceCharge/ChargeIndicator/Indicator` |

### corpus\ZUGFeRDv2\correct\Mustangproject\testout-ZF2PushReverseCharge.xml

76 of 105 values are on the page.

| Values | Kind | Element |
|---:|---|---|
| 4 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeAgreement/GrossPriceProductTradePrice/BasisQuantity@unitCode` |
| 4 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeAgreement/NetPriceProductTradePrice/BasisQuantity@unitCode` |
| 4 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeDelivery/BilledQuantity@unitCode` |
| 4 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeSettlement/ApplicableTradeTax/CategoryCode` |
| 4 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeSettlement/ApplicableTradeTax/TypeCode` |
| 2 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/SellerTradeParty/SpecifiedTaxRegistration/ID@schemeID` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocument/IssueDateTime/DateTimeString@format` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocument/TypeCode` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocumentContext/GuidelineSpecifiedDocumentContextParameter/ID` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/BuyerTradeParty/SpecifiedTaxRegistration/ID@schemeID` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeDelivery/ActualDeliverySupplyChainEvent/OccurrenceDateTime/DateTimeString@format` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/ApplicableTradeTax/CategoryCode` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/ApplicableTradeTax/TypeCode` |

### corpus\ZUGFeRDv2\correct\xml\ZUGFeRD-invoice_rabatte_3_abschlag_duepayableamount.xml

35 of 48 values are on the page.

| Values | Kind | Element |
|---:|---|---|
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocument/IssueDateTime/DateTimeString@format` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocument/TypeCode` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocumentContext/GuidelineSpecifiedDocumentContextParameter/ID` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/SellerTradeParty/SpecifiedTaxRegistration/ID@schemeID` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeDelivery/ActualDeliverySupplyChainEvent/OccurrenceDateTime/DateTimeString@format` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/ApplicableTradeTax/CategoryCode` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/ApplicableTradeTax/TypeCode` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/SpecifiedTradeAllowanceCharge/CategoryTradeTax/CategoryCode` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/SpecifiedTradeAllowanceCharge/CategoryTradeTax/TypeCode` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/SpecifiedTradeAllowanceCharge/ChargeIndicator/Indicator` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeDelivery/BilledQuantity@unitCode` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeSettlement/ApplicableTradeTax/CategoryCode` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeSettlement/ApplicableTradeTax/TypeCode` |

### corpus\ZUGFeRDv2\correct\xml\ZUGFeRD-invoice_rabatte_4_abschlag_taxbasistotalamount.xml

35 of 48 values are on the page.

| Values | Kind | Element |
|---:|---|---|
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocument/IssueDateTime/DateTimeString@format` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocument/TypeCode` |
| 1 | not rendered | `/CrossIndustryInvoice/ExchangedDocumentContext/GuidelineSpecifiedDocumentContextParameter/ID` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeAgreement/SellerTradeParty/SpecifiedTaxRegistration/ID@schemeID` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeDelivery/ActualDeliverySupplyChainEvent/OccurrenceDateTime/DateTimeString@format` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/ApplicableTradeTax/CategoryCode` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/ApplicableTradeTax/TypeCode` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/SpecifiedTradeAllowanceCharge/CategoryTradeTax/CategoryCode` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/SpecifiedTradeAllowanceCharge/CategoryTradeTax/TypeCode` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/ApplicableHeaderTradeSettlement/SpecifiedTradeAllowanceCharge/ChargeIndicator/Indicator` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeDelivery/BilledQuantity@unitCode` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeSettlement/ApplicableTradeTax/CategoryCode` |
| 1 | not rendered | `/CrossIndustryInvoice/SupplyChainTradeTransaction/IncludedSupplyChainTradeLineItem/SpecifiedLineTradeSettlement/ApplicableTradeTax/TypeCode` |

