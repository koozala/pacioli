# The class InvoiceDescriptor

> **Pacioli does not use this library any more.** The parser is now
> `Vit.FeRD.ExtendedDocument`. For the model that Pacioli uses today, see
> [CrossIndustryInvoice.md](CrossIndustryInvoice.md). This document stays for the history,
> and for a comparison of the two models.

`InvoiceDescriptor` is the data model of an electronic invoice. Pacioli used it for all
invoice data. The class comes from the library `ZUGFeRD-csharp` of s2industries.

- Namespace: `s2industries.ZUGFeRD`
- Assembly: `s2industries.ZUGFeRD.dll`, version **18.0.0**
- Package: `ZUGFeRD-csharp` 18.0.0

`InvoiceDescriptor.Load()` makes the object. It reads an XML file, or a stream with the
XML data from a ZUGFeRD PDF file. `Pacioli.Pdf` then writes the data to a PDF page. For
more data about this step, see [the note at the end](#which-data-pacioli-shows).

> This document shows the structure of version 18.0.0. If you change the version of the
> package, examine the diagrams again.

## Contents

- [Overview](#overview)
- [Data of the document](#data-of-the-document)
- [Parties](#parties)
- [Positions](#positions)
- [Tax, allowances, and charges](#tax-allowances-and-charges)
- [Payment](#payment)
- [Referenced documents](#referenced-documents)
- [How to read a list](#how-to-read-a-list)
- [Which data Pacioli shows](#which-data-pacioli-shows)

## Overview

The diagram shows the parts of an invoice. It does not show the simple fields. The
sections after it show the fields.

```mermaid
classDiagram
    direction LR

    class InvoiceDescriptor
    class Party
    class TradeLineItem
    class Tax
    class AbstractTradeAllowanceCharge
    class ServiceCharge
    class PaymentTerms
    class PaymentMeans
    class BankAccount
    class Note
    class Contact
    class ElectronicAddress
    class TaxRegistration
    class SpecifiedProcuringProject
    class ReceivableSpecifiedTradeAccountingAccount
    class BaseReferencedDocument

    InvoiceDescriptor "1" --> "0..9" Party : Seller Buyer ShipTo and more
    InvoiceDescriptor "1" --> "0..*" TradeLineItem : the positions
    InvoiceDescriptor "1" --> "0..*" Tax : the tax rates
    InvoiceDescriptor "1" --> "0..*" AbstractTradeAllowanceCharge : allowances and charges
    InvoiceDescriptor "1" --> "0..*" ServiceCharge : transport costs
    InvoiceDescriptor "1" --> "0..*" PaymentTerms : payment terms
    InvoiceDescriptor "1" --> "0..1" PaymentMeans : how to pay
    InvoiceDescriptor "1" --> "0..*" BankAccount : Creditor and Debitor accounts
    InvoiceDescriptor "1" --> "0..*" Note : the notes
    InvoiceDescriptor "1" --> "0..5" Contact : SellerContact BuyerContact and more
    InvoiceDescriptor "1" --> "0..2" ElectronicAddress : Seller and Buyer address
    InvoiceDescriptor "1" --> "0..*" TaxRegistration : tax numbers of the parties
    InvoiceDescriptor "1" --> "0..1" SpecifiedProcuringProject : the project
    InvoiceDescriptor "1" --> "0..*" ReceivableSpecifiedTradeAccountingAccount : cost centres
    InvoiceDescriptor "1" --> "0..*" BaseReferencedDocument : order contract and more
```

## Data of the document

These fields hold the data of the invoice itself, and the totals.

```mermaid
classDiagram
    class InvoiceDescriptor {
        +String InvoiceNo
        +DateTime? InvoiceDate
        +InvoiceType Type
        +String Name
        +Profile Profile
        +CurrencyCodes Currency
        +CurrencyCodes? TaxCurrency
        +Boolean IsTest
        +String BusinessProcess
        +String OrderNo
        +DateTime? OrderDate
        +String ReferenceOrderNo
        +String SellerReferenceNo
        +String PaymentReference
        +DateTime? ActualDeliveryDate
        +DateTime? BillingPeriodStart
        +DateTime? BillingPeriodEnd
        +TradeDeliveryTermCodes? ApplicableTradeDeliveryTermsCode
        +TransportModeCodes? TransportMode
        +Decimal? LineTotalAmount
        +Decimal? ChargeTotalAmount
        +Decimal? AllowanceTotalAmount
        +Decimal? TaxBasisAmount
        +Decimal? TaxTotalAmount
        +Decimal? GrandTotalAmount
        +Decimal? TotalPrepaidAmount
        +Decimal? RoundingAmount
        +Decimal? DuePayableAmount
    }
```

The nine amounts make the summary of the invoice. `SetTotals()` writes all of them
together.

`Profile` tells which ZUGFeRD profile the document uses: `Minimum`, `BasicWL`, `Basic`,
`Comfort`, `Extended`, `XRechnung`, and more. A profile with few fields, for example
`Minimum`, leaves most fields empty.

## Parties

A party is a company or a person. One invoice has more than one party. Each party has the
same structure.

```mermaid
classDiagram
    direction LR

    class InvoiceDescriptor {
        +Party Seller
        +Party Buyer
        +Party Invoicee
        +Party Invoicer
        +Party Payee
        +Party ShipTo
        +Party UltimateShipTo
        +Party ShipFrom
        +Party SellerTaxRepresentative
        +Contact SellerContact
        +Contact BuyerContact
        +Contact InvoicerContact
        +Contact ShipToContact
        +Contact UltimateShipToContact
        +ElectronicAddress SellerElectronicAddress
        +ElectronicAddress BuyerElectronicAddress
        +List~TaxRegistration~ SellerTaxRegistration
        +List~TaxRegistration~ BuyerTaxRegistration
        +List~TaxRegistration~ SellerTaxRepresentativeTaxRegistration
    }

    class Party {
        +GlobalID ID
        +GlobalID GlobalID
        +String Name
        +String Description
        +String ContactName
        +String Street
        +String Street2
        +String AddressLine3
        +String Postcode
        +String City
        +CountryCodes? Country
        +String CountrySubdivisionName
        +LegalOrganization SpecifiedLegalOrganization
    }

    class Contact {
        +String Name
        +String OrgUnit
        +String EmailAddress
        +String PhoneNo
        +String FaxNo
    }

    class ElectronicAddress {
        +String Address
        +ElectronicAddressSchemeIdentifiers ElectronicAddressSchemeID
    }

    class TaxRegistration {
        +String No
        +TaxRegistrationSchemeID SchemeID
    }

    class GlobalID {
        +String ID
        +GlobalIDSchemeIdentifiers? SchemeID
    }

    class LegalOrganization {
        +GlobalID ID
        +String TradingBusinessName
    }

    InvoiceDescriptor --> Party
    InvoiceDescriptor --> Contact
    InvoiceDescriptor --> ElectronicAddress
    InvoiceDescriptor --> TaxRegistration
    Party --> GlobalID : ID and GlobalID
    Party --> LegalOrganization : SpecifiedLegalOrganization
    LegalOrganization --> GlobalID : ID
```

`TaxRegistration` holds the tax number and the VAT ID. The `SchemeID` tells the kind:
`FC` is the tax number, `VA` is the VAT ID.

## Positions

`TradeLineItem` is one position of the invoice. `GetTradeLineItems()` gives the list.

```mermaid
classDiagram
    direction LR

    class TradeLineItem {
        +String Name
        +String Description
        +GlobalID GlobalID
        +String SellerAssignedID
        +String BuyerAssignedID
        +String IndustryAssignedID
        +String ModelID
        +String ModelName
        +String BrandName
        +String BatchID
        +Decimal BilledQuantity
        +QuantityCodes? UnitCode
        +Decimal? ChargeFreeQuantity
        +Decimal? PackageQuantity
        +Decimal NetUnitPrice
        +Decimal? GrossUnitPrice
        +Decimal? NetQuantity
        +Decimal? GrossQuantity
        +Decimal? LineTotalAmount
        +Decimal TaxPercent
        +TaxTypes? TaxType
        +TaxCategoryCodes? TaxCategoryCode
        +String TaxExemptionReason
        +DateTime? ActualDeliveryDate
        +DateTime? BillingPeriodStart
        +DateTime? BillingPeriodEnd
        +CountryCodes? OriginTradeCountry
        +Party ShipTo
        +Party UltimateShipTo
    }

    class AssociatedDocument {
        +String LineID
        +String ParentLineID
        +List~Note~ Notes
        +LineStatusCodes? LineStatusCode
        +LineStatusReasonCodes? LineStatusReasonCode
    }

    class ApplicableProductCharacteristic {
        +String Description
        +String Value
    }

    class DesignatedProductClassification {
        +String ClassCode
        +String ClassName
        +String ListVersionID
        +DesignatedProductClassificationClassCodes? ListID
    }

    class IncludedReferencedProduct {
        +String Name
        +String Description
        +Decimal? UnitQuantity
        +QuantityCodes? UnitCode
        +GlobalID GlobalID
        +String SellerAssignedID
        +String BuyerAssignedID
    }

    TradeLineItem --> "1" AssociatedDocument : AssociatedDocument
    TradeLineItem --> "0..*" ApplicableProductCharacteristic : ApplicableProductCharacteristics
    TradeLineItem --> "0..*" DesignatedProductClassification : DesignatedProductClassifications
    TradeLineItem --> "0..*" IncludedReferencedProduct : IncludedReferencedProducts
    TradeLineItem --> "0..*" AbstractTradeAllowanceCharge : TradeAllowanceCharges
    TradeLineItem --> "0..*" ReceivableSpecifiedTradeAccountingAccount : Cost centres
    TradeLineItem --> "0..*" AdditionalReferencedDocument : AdditionalReferencedDocuments
    TradeLineItem --> "0..1" BuyerOrderReferencedDocument : BuyerOrderReferencedDocument
    TradeLineItem --> "0..1" DeliveryNoteReferencedDocument : DeliveryNoteReferencedDocument
    TradeLineItem --> "0..1" ContractReferencedDocument : ContractReferencedDocument
    AssociatedDocument --> "0..*" Note : Notes
```

`AssociatedDocument.LineID` is the position number from the XML file.
`ParentLineID` makes a sub position: it names the position above.

A position holds two lists of allowances and charges: `TradeAllowanceCharges` and
`SpecifiedTradeAllowanceCharges`. The first list applies to the position. The second list
applies to the gross price.

## Tax, allowances, and charges

```mermaid
classDiagram
    direction TB

    class Tax {
        +Decimal Percent
        +Decimal BasisAmount
        +Decimal TaxAmount
        +TaxTypes? TypeCode
        +TaxCategoryCodes? CategoryCode
        +Decimal? AllowanceChargeBasisAmount
        +Decimal? LineTotalBasisAmount
        +String ExemptionReason
        +TaxExemptionReasonCodes? ExemptionReasonCode
        +DateTime? TaxPointDate
        +DateTypeCodes? DueDateTypeCode
    }

    class Charge {
        <<abstract>>
        +Decimal Amount
        +Tax Tax
    }

    class AbstractTradeAllowanceCharge {
        <<abstract>>
        +Boolean ChargeIndicator
        +String Reason
        +Decimal ActualAmount
        +Decimal? BasisAmount
        +Decimal? ChargePercentage
        +CurrencyCodes Currency
    }

    class TradeAllowance {
        +AllowanceReasonCodes? ReasonCode
    }

    class TradeCharge {
        +ChargeReasonCodes? ReasonCode
    }

    class ServiceCharge {
        +String Description
    }

    Charge <|-- AbstractTradeAllowanceCharge
    Charge <|-- ServiceCharge
    AbstractTradeAllowanceCharge <|-- TradeAllowance
    AbstractTradeAllowanceCharge <|-- TradeCharge
    Charge --> "0..1" Tax : Tax
```

`ChargeIndicator` tells the direction. `true` is a charge, `false` is an allowance. The
two subclasses give the reason code for each direction.

Use `GetTradeAllowances()` and `GetTradeCharges()` to get one kind only.

## Payment

```mermaid
classDiagram
    direction LR

    class PaymentMeans {
        +PaymentMeansTypeCodes? TypeCode
        +String Information
        +String SEPACreditorIdentifier
        +String SEPAMandateReference
    }

    class FinancialCard {
        +String Id
        +String CardholderName
    }

    class BankAccount {
        +String Name
        +String IBAN
        +String BIC
        +String Bankleitzahl
        +String BankName
        +String ID
    }

    class PaymentTerms {
        +String Description
        +DateTime? DueDate
        +Int32? DueDays
        +DateTime? MaturityDate
        +Decimal? Percentage
        +Decimal? BaseAmount
        +Decimal? ActualAmount
        +PaymentTermsType? PaymentTermsType
    }

    InvoiceDescriptor --> "0..1" PaymentMeans : PaymentMeans
    InvoiceDescriptor --> "0..*" BankAccount : CreditorBankAccounts
    InvoiceDescriptor --> "0..*" BankAccount : DebitorBankAccounts
    InvoiceDescriptor --> "0..*" PaymentTerms : PaymentTerms
    PaymentMeans --> "0..1" FinancialCard : FinancialCard
```

`CreditorBankAccounts` holds the accounts of the seller. `DebitorBankAccounts` holds the
accounts of the buyer, for a direct debit.

`PaymentTerms` with a `Percentage` and a `DueDays` value gives the discount terms
(Skonto).

## Referenced documents

All referenced documents have the same base class.

```mermaid
classDiagram
    direction TB

    class BaseReferencedDocument {
        +String ID
        +DateTime? IssueDateTime
    }

    class AdditionalReferencedDocument {
        +String Name
        +String Filename
        +Byte[] AttachmentBinaryObject
        +String MimeType
        +String URIID
        +String LineID
        +AdditionalReferencedDocumentTypeCode? TypeCode
        +ReferenceTypeCodes? ReferenceTypeCode
    }

    class BuyerOrderReferencedDocument {
        +String LineID
    }

    class ContractReferencedDocument {
        +String LineID
    }

    class DeliveryNoteReferencedDocument {
        +String LineID
    }

    class InvoiceReferencedDocument {
        +InvoiceType? TypeCode
    }

    class SellerOrderReferencedDocument
    class DespatchAdviceReferencedDocument

    BaseReferencedDocument <|-- AdditionalReferencedDocument
    BaseReferencedDocument <|-- BuyerOrderReferencedDocument
    BaseReferencedDocument <|-- SellerOrderReferencedDocument
    BaseReferencedDocument <|-- ContractReferencedDocument
    BaseReferencedDocument <|-- DeliveryNoteReferencedDocument
    BaseReferencedDocument <|-- DespatchAdviceReferencedDocument
    BaseReferencedDocument <|-- InvoiceReferencedDocument
```

`AdditionalReferencedDocument` holds an attachment. `AttachmentBinaryObject` holds the
data of the file. Pacioli writes this data to a temporary file, and it opens the file when
the user clicks the link.

## How to read a list

Some lists have a `Get...` method. Use the method, not the property. A property with an
underscore at its start, for example `_InvoiceReferencedDocuments`, is not for your code.

| Method | Gives |
|---|---|
| `GetTradeLineItems()` | The positions. |
| `GetApplicableTradeTaxes()` | The tax rates of the document. |
| `GetTradeAllowanceCharges()` | The allowances and the charges together. |
| `GetTradeAllowances()` | The allowances only. |
| `GetTradeCharges()` | The charges only. |
| `GetTradePaymentTerms()` | The payment terms. |
| `GetCreditorFinancialAccounts()` | The bank accounts of the seller. |
| `GetDebitorFinancialAccounts()` | The bank accounts of the buyer. |
| `GetAdditionalReferencedDocuments()` | The attachments and the other documents. |
| `GetNotes()` | The notes of the document. |
| `GetLogisticsServiceCharges()` | The transport costs. |
| `GetSellerTaxRegistration()` | The tax numbers of the seller. |
| `GetBuyerTaxRegistration()` | The tax numbers of the buyer. |
| `GetInvoiceReferencedDocuments()` | The invoices that this document corrects. |
| `GetReceivableSpecifiedTradeAccountingAccounts()` | The cost centres. |

## Which data Pacioli shows

`InvoiceWriter` in the project `Pacioli.Pdf` writes the data to the PDF page. It does not
write all fields.

The test project `Pacioli.CLI.NET8.Test` measures this. It renders each sample document,
then it looks for each value of the XML input on the page. The test makes a report:

```
src\TestResults\xml-value-report.md
```

With the FeRD samples of version 2.5.2, the page shows 73 % of the values. 21 % are codes
and technical data that the page must not show in this form. The renderer does not show
the last 6 %.

The largest groups that the renderer does not show are:

| Element | Class in this document |
|---|---|
| The tax number and the VAT ID of a party | `TaxRegistration` |
| The position number from the XML | `AssociatedDocument.LineID` |
| The global ID of the seller | `Party.GlobalID` |
| A payment before this invoice | not in the model of the renderer |
| The order number of the seller | `SellerOrderReferencedDocument` |
| The electronic address of the seller | `ElectronicAddress` |

For the full list with the reasons, see the report, and see
`XmlValueExclusions.All` in the test project.
