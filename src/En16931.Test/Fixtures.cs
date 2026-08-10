namespace En16931.Test;

/*
 * Two small invoices, one in each syntax, with the same business content.
 *
 * The documents are written by hand. They are not a sample of the specification, thus
 * the repository holds no file of a specification and no condition of use applies.
 *
 * The two documents carry the same values. Thus a test can read both and compare the
 * result: the model must not depend on the syntax. See SyntaxAgreementTests.
 */
internal static class Fixtures
{
    /* A Cross Industry Invoice, the syntax of ZUGFeRD and Factur-X. */
    public const string Cii = """
        <?xml version="1.0" encoding="UTF-8"?>
        <rsm:CrossIndustryInvoice
            xmlns:rsm="urn:un:unece:uncefact:data:standard:CrossIndustryInvoice:100"
            xmlns:ram="urn:un:unece:uncefact:data:standard:ReusableAggregateBusinessInformationEntity:100"
            xmlns:udt="urn:un:unece:uncefact:data:standard:UnqualifiedDataType:100">
          <rsm:ExchangedDocumentContext>
            <ram:GuidelineSpecifiedDocumentContextParameter>
              <ram:ID>urn:cen.eu:en16931:2017</ram:ID>
            </ram:GuidelineSpecifiedDocumentContextParameter>
          </rsm:ExchangedDocumentContext>
          <rsm:ExchangedDocument>
            <ram:ID>R-2026-0815</ram:ID>
            <ram:TypeCode>380</ram:TypeCode>
            <ram:IssueDateTime>
              <udt:DateTimeString format="102">20260315</udt:DateTimeString>
            </ram:IssueDateTime>
            <ram:IncludedNote>
              <ram:Content>Zahlbar ohne Abschlag.</ram:Content>
              <ram:SubjectCode>AAI</ram:SubjectCode>
            </ram:IncludedNote>
          </rsm:ExchangedDocument>
          <rsm:SupplyChainTradeTransaction>
            <ram:IncludedSupplyChainTradeLineItem>
              <ram:AssociatedDocumentLineDocument>
                <ram:LineID>1</ram:LineID>
              </ram:AssociatedDocumentLineDocument>
              <ram:SpecifiedTradeProduct>
                <ram:SellerAssignedID>ART-4711</ram:SellerAssignedID>
                <ram:Name>Trennscheibe</ram:Name>
              </ram:SpecifiedTradeProduct>
              <ram:SpecifiedLineTradeAgreement>
                <ram:NetPriceProductTradePrice>
                  <ram:ChargeAmount>9.50</ram:ChargeAmount>
                </ram:NetPriceProductTradePrice>
              </ram:SpecifiedLineTradeAgreement>
              <ram:SpecifiedLineTradeDelivery>
                <ram:BilledQuantity unitCode="H87">20.0</ram:BilledQuantity>
              </ram:SpecifiedLineTradeDelivery>
              <ram:SpecifiedLineTradeSettlement>
                <ram:ApplicableTradeTax>
                  <ram:TypeCode>VAT</ram:TypeCode>
                  <ram:CategoryCode>S</ram:CategoryCode>
                  <ram:RateApplicablePercent>19.00</ram:RateApplicablePercent>
                </ram:ApplicableTradeTax>
                <ram:SpecifiedTradeSettlementLineMonetarySummation>
                  <ram:LineTotalAmount>190.00</ram:LineTotalAmount>
                </ram:SpecifiedTradeSettlementLineMonetarySummation>
              </ram:SpecifiedLineTradeSettlement>
            </ram:IncludedSupplyChainTradeLineItem>
            <ram:ApplicableHeaderTradeAgreement>
              <ram:BuyerReference>04011000-12345-34</ram:BuyerReference>
              <ram:SellerTradeParty>
                <ram:Name>Muster GmbH</ram:Name>
                <ram:DefinedTradeContact>
                  <ram:PersonName>Erika Musterfrau</ram:PersonName>
                  <ram:EmailURIUniversalCommunication>
                    <ram:URIID>erika@muster.example</ram:URIID>
                  </ram:EmailURIUniversalCommunication>
                </ram:DefinedTradeContact>
                <ram:PostalTradeAddress>
                  <ram:PostcodeCode>38226</ram:PostcodeCode>
                  <ram:LineOne>Marktstr. 153</ram:LineOne>
                  <ram:CityName>Salzgitter</ram:CityName>
                  <ram:CountryID>DE</ram:CountryID>
                </ram:PostalTradeAddress>
                <ram:SpecifiedTaxRegistration>
                  <ram:ID schemeID="VA">DE123456788</ram:ID>
                </ram:SpecifiedTaxRegistration>
              </ram:SellerTradeParty>
              <ram:BuyerTradeParty>
                <ram:Name>Kunde AG</ram:Name>
                <ram:PostalTradeAddress>
                  <ram:PostcodeCode>10117</ram:PostcodeCode>
                  <ram:LineOne>Unter den Linden 1</ram:LineOne>
                  <ram:CityName>Berlin</ram:CityName>
                  <ram:CountryID>DE</ram:CountryID>
                </ram:PostalTradeAddress>
              </ram:BuyerTradeParty>
              <ram:BuyerOrderReferencedDocument>
                <ram:IssuerAssignedID>B-2026-0042</ram:IssuerAssignedID>
              </ram:BuyerOrderReferencedDocument>
            </ram:ApplicableHeaderTradeAgreement>
            <ram:ApplicableHeaderTradeDelivery>
              <ram:ActualDeliverySupplyChainEvent>
                <ram:OccurrenceDateTime>
                  <udt:DateTimeString format="102">20260312</udt:DateTimeString>
                </ram:OccurrenceDateTime>
              </ram:ActualDeliverySupplyChainEvent>
            </ram:ApplicableHeaderTradeDelivery>
            <ram:ApplicableHeaderTradeSettlement>
              <ram:PaymentReference>R-2026-0815</ram:PaymentReference>
              <ram:InvoiceCurrencyCode>EUR</ram:InvoiceCurrencyCode>
              <ram:SpecifiedTradeSettlementPaymentMeans>
                <ram:TypeCode>58</ram:TypeCode>
                <ram:PayeePartyCreditorFinancialAccount>
                  <ram:IBANID>DE02120300000000202051</ram:IBANID>
                  <ram:AccountName>Muster GmbH</ram:AccountName>
                </ram:PayeePartyCreditorFinancialAccount>
                <ram:PayeeSpecifiedCreditorFinancialInstitution>
                  <ram:BICID>BYLADEM1001</ram:BICID>
                </ram:PayeeSpecifiedCreditorFinancialInstitution>
              </ram:SpecifiedTradeSettlementPaymentMeans>
              <ram:ApplicableTradeTax>
                <ram:CalculatedAmount>36.10</ram:CalculatedAmount>
                <ram:TypeCode>VAT</ram:TypeCode>
                <ram:BasisAmount>190.00</ram:BasisAmount>
                <ram:CategoryCode>S</ram:CategoryCode>
                <ram:RateApplicablePercent>19.00</ram:RateApplicablePercent>
              </ram:ApplicableTradeTax>
              <ram:SpecifiedTradePaymentTerms>
                <ram:Description>Zahlbar bis zum 14.04.2026.</ram:Description>
                <ram:DueDateDateTime>
                  <udt:DateTimeString format="102">20260414</udt:DateTimeString>
                </ram:DueDateDateTime>
              </ram:SpecifiedTradePaymentTerms>
              <ram:SpecifiedTradeSettlementHeaderMonetarySummation>
                <ram:LineTotalAmount>190.00</ram:LineTotalAmount>
                <ram:TaxBasisTotalAmount>190.00</ram:TaxBasisTotalAmount>
                <ram:TaxTotalAmount currencyID="EUR">36.10</ram:TaxTotalAmount>
                <ram:GrandTotalAmount>226.10</ram:GrandTotalAmount>
                <ram:DuePayableAmount>226.10</ram:DuePayableAmount>
              </ram:SpecifiedTradeSettlementHeaderMonetarySummation>
            </ram:ApplicableHeaderTradeSettlement>
          </rsm:SupplyChainTradeTransaction>
        </rsm:CrossIndustryInvoice>
        """;

    /* The same invoice as a UBL 2.1 Invoice. */
    public const string Ubl = """
        <?xml version="1.0" encoding="UTF-8"?>
        <Invoice xmlns="urn:oasis:names:specification:ubl:schema:xsd:Invoice-2"
                 xmlns:cac="urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2"
                 xmlns:cbc="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2">
          <cbc:CustomizationID>urn:cen.eu:en16931:2017</cbc:CustomizationID>
          <cbc:ID>R-2026-0815</cbc:ID>
          <cbc:IssueDate>2026-03-15</cbc:IssueDate>
          <cbc:DueDate>2026-04-14</cbc:DueDate>
          <cbc:InvoiceTypeCode>380</cbc:InvoiceTypeCode>
          <cbc:Note>#AAI#Zahlbar ohne Abschlag.</cbc:Note>
          <cbc:DocumentCurrencyCode>EUR</cbc:DocumentCurrencyCode>
          <cbc:BuyerReference>04011000-12345-34</cbc:BuyerReference>
          <cac:OrderReference>
            <cbc:ID>B-2026-0042</cbc:ID>
          </cac:OrderReference>
          <cac:AccountingSupplierParty>
            <cac:Party>
              <cac:PostalAddress>
                <cbc:StreetName>Marktstr. 153</cbc:StreetName>
                <cbc:CityName>Salzgitter</cbc:CityName>
                <cbc:PostalZone>38226</cbc:PostalZone>
                <cac:Country>
                  <cbc:IdentificationCode>DE</cbc:IdentificationCode>
                </cac:Country>
              </cac:PostalAddress>
              <cac:PartyTaxScheme>
                <cbc:CompanyID>DE123456788</cbc:CompanyID>
                <cac:TaxScheme>
                  <cbc:ID>VAT</cbc:ID>
                </cac:TaxScheme>
              </cac:PartyTaxScheme>
              <cac:PartyLegalEntity>
                <cbc:RegistrationName>Muster GmbH</cbc:RegistrationName>
              </cac:PartyLegalEntity>
              <cac:Contact>
                <cbc:Name>Erika Musterfrau</cbc:Name>
                <cbc:ElectronicMail>erika@muster.example</cbc:ElectronicMail>
              </cac:Contact>
            </cac:Party>
          </cac:AccountingSupplierParty>
          <cac:AccountingCustomerParty>
            <cac:Party>
              <cac:PostalAddress>
                <cbc:StreetName>Unter den Linden 1</cbc:StreetName>
                <cbc:CityName>Berlin</cbc:CityName>
                <cbc:PostalZone>10117</cbc:PostalZone>
                <cac:Country>
                  <cbc:IdentificationCode>DE</cbc:IdentificationCode>
                </cac:Country>
              </cac:PostalAddress>
              <cac:PartyLegalEntity>
                <cbc:RegistrationName>Kunde AG</cbc:RegistrationName>
              </cac:PartyLegalEntity>
            </cac:Party>
          </cac:AccountingCustomerParty>
          <cac:Delivery>
            <cbc:ActualDeliveryDate>2026-03-12</cbc:ActualDeliveryDate>
          </cac:Delivery>
          <cac:PaymentMeans>
            <cbc:PaymentMeansCode>58</cbc:PaymentMeansCode>
            <cbc:PaymentID>R-2026-0815</cbc:PaymentID>
            <cac:PayeeFinancialAccount>
              <cbc:ID>DE02120300000000202051</cbc:ID>
              <cbc:Name>Muster GmbH</cbc:Name>
              <cac:FinancialInstitutionBranch>
                <cbc:ID>BYLADEM1001</cbc:ID>
              </cac:FinancialInstitutionBranch>
            </cac:PayeeFinancialAccount>
          </cac:PaymentMeans>
          <cac:PaymentTerms>
            <cbc:Note>Zahlbar bis zum 14.04.2026.</cbc:Note>
          </cac:PaymentTerms>
          <cac:TaxTotal>
            <cbc:TaxAmount currencyID="EUR">36.10</cbc:TaxAmount>
            <cac:TaxSubtotal>
              <cbc:TaxableAmount currencyID="EUR">190.00</cbc:TaxableAmount>
              <cbc:TaxAmount currencyID="EUR">36.10</cbc:TaxAmount>
              <cac:TaxCategory>
                <cbc:ID>S</cbc:ID>
                <cbc:Percent>19.00</cbc:Percent>
                <cac:TaxScheme>
                  <cbc:ID>VAT</cbc:ID>
                </cac:TaxScheme>
              </cac:TaxCategory>
            </cac:TaxSubtotal>
          </cac:TaxTotal>
          <cac:LegalMonetaryTotal>
            <cbc:LineExtensionAmount currencyID="EUR">190.00</cbc:LineExtensionAmount>
            <cbc:TaxExclusiveAmount currencyID="EUR">190.00</cbc:TaxExclusiveAmount>
            <cbc:TaxInclusiveAmount currencyID="EUR">226.10</cbc:TaxInclusiveAmount>
            <cbc:PayableAmount currencyID="EUR">226.10</cbc:PayableAmount>
          </cac:LegalMonetaryTotal>
          <cac:InvoiceLine>
            <cbc:ID>1</cbc:ID>
            <cbc:InvoicedQuantity unitCode="H87">20.0</cbc:InvoicedQuantity>
            <cbc:LineExtensionAmount currencyID="EUR">190.00</cbc:LineExtensionAmount>
            <cac:Item>
              <cbc:Name>Trennscheibe</cbc:Name>
              <cac:SellersItemIdentification>
                <cbc:ID>ART-4711</cbc:ID>
              </cac:SellersItemIdentification>
              <cac:ClassifiedTaxCategory>
                <cbc:ID>S</cbc:ID>
                <cbc:Percent>19.00</cbc:Percent>
                <cac:TaxScheme>
                  <cbc:ID>VAT</cbc:ID>
                </cac:TaxScheme>
              </cac:ClassifiedTaxCategory>
            </cac:Item>
            <cac:Price>
              <cbc:PriceAmount currencyID="EUR">9.50</cbc:PriceAmount>
            </cac:Price>
          </cac:InvoiceLine>
        </Invoice>
        """;

    /* A UBL CreditNote. It uses other names for the type code and for a position. */
    public const string UblCreditNote = """
        <?xml version="1.0" encoding="UTF-8"?>
        <CreditNote xmlns="urn:oasis:names:specification:ubl:schema:xsd:CreditNote-2"
                    xmlns:cac="urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2"
                    xmlns:cbc="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2">
          <cbc:CustomizationID>urn:cen.eu:en16931:2017</cbc:CustomizationID>
          <cbc:ID>G-2026-0001</cbc:ID>
          <cbc:IssueDate>2026-03-20</cbc:IssueDate>
          <cbc:CreditNoteTypeCode>381</cbc:CreditNoteTypeCode>
          <cbc:DocumentCurrencyCode>EUR</cbc:DocumentCurrencyCode>
          <cac:CreditNoteLine>
            <cbc:ID>1</cbc:ID>
            <cbc:CreditedQuantity unitCode="H87">2.0</cbc:CreditedQuantity>
            <cbc:LineExtensionAmount currencyID="EUR">19.00</cbc:LineExtensionAmount>
            <cac:Item>
              <cbc:Name>Trennscheibe</cbc:Name>
            </cac:Item>
            <cac:Price>
              <cbc:PriceAmount currencyID="EUR">9.50</cbc:PriceAmount>
            </cac:Price>
          </cac:CreditNoteLine>
        </CreditNote>
        """;
}
