using En16931.Model;

namespace En16931.Test;

/*
 * The data that goes over the core of EN 16931.
 *
 * The fixture holds the groups that the renderer of Pacioli shows and that EN 16931 has
 * no business term for. The reachability test of Pacioli.CLI.Test controls the same over
 * 101 documents of the practice; this test names the shape of each group, thus a change
 * of the model shows itself here.
 */
[TestFixture]
public class ExtensionsTests
{
    private const string Extended = """
        <?xml version="1.0" encoding="UTF-8"?>
        <rsm:CrossIndustryInvoice
            xmlns:rsm="urn:un:unece:uncefact:data:standard:CrossIndustryInvoice:100"
            xmlns:ram="urn:un:unece:uncefact:data:standard:ReusableAggregateBusinessInformationEntity:100"
            xmlns:qdt="urn:un:unece:uncefact:data:standard:QualifiedDataType:100"
            xmlns:udt="urn:un:unece:uncefact:data:standard:UnqualifiedDataType:100">
          <rsm:ExchangedDocument>
            <ram:ID>R-1</ram:ID>
            <ram:Name>RECHNUNG</ram:Name>
            <ram:TypeCode>380</ram:TypeCode>
            <ram:IncludedNote>
              <ram:Content>Ein Hinweis.</ram:Content>
              <ram:SubjectCode>AAI</ram:SubjectCode>
              <ram:ContentCode>ST3</ram:ContentCode>
            </ram:IncludedNote>
          </rsm:ExchangedDocument>
          <rsm:SupplyChainTradeTransaction>
            <ram:IncludedSupplyChainTradeLineItem>
              <ram:AssociatedDocumentLineDocument>
                <ram:LineID>1</ram:LineID>
                <ram:IncludedNote><ram:Content>Zur Position.</ram:Content></ram:IncludedNote>
              </ram:AssociatedDocumentLineDocument>
              <ram:SpecifiedTradeProduct>
                <ram:Name>Kaffeeset</ram:Name>
                <ram:IndustryAssignedID>IND-9</ram:IndustryAssignedID>
                <ram:DesignatedProductClassification>
                  <ram:ClassCode listID="TST">0815</ram:ClassCode>
                  <ram:ClassName>Zolltarifnummer</ram:ClassName>
                </ram:DesignatedProductClassification>
                <ram:IndividualTradeProductInstance>
                  <ram:BatchID>CHARGE-7</ram:BatchID>
                  <ram:SupplierAssignedSerialID>SN-4711</ram:SupplierAssignedSerialID>
                </ram:IndividualTradeProductInstance>
                <ram:IncludedReferencedProduct>
                  <ram:Name>Kaffeetasse</ram:Name>
                  <ram:UnitQuantity unitCode="H87">6.0</ram:UnitQuantity>
                </ram:IncludedReferencedProduct>
              </ram:SpecifiedTradeProduct>
              <ram:SpecifiedLineTradeAgreement>
                <ram:GrossPriceProductTradePrice>
                  <ram:ChargeAmount>12.00</ram:ChargeAmount>
                  <ram:BasisQuantity unitCode="H87">10.0</ram:BasisQuantity>
                  <ram:AppliedTradeAllowanceCharge>
                    <ram:ChargeIndicator><udt:Indicator>false</udt:Indicator></ram:ChargeIndicator>
                    <ram:CalculationPercent>10.00</ram:CalculationPercent>
                    <ram:ActualAmount>1.20</ram:ActualAmount>
                    <ram:Reason>Mengenrabatt</ram:Reason>
                  </ram:AppliedTradeAllowanceCharge>
                </ram:GrossPriceProductTradePrice>
                <ram:NetPriceProductTradePrice>
                  <ram:ChargeAmount>10.80</ram:ChargeAmount>
                  <ram:BasisQuantity unitCode="H87">1.0</ram:BasisQuantity>
                </ram:NetPriceProductTradePrice>
              </ram:SpecifiedLineTradeAgreement>
              <ram:SpecifiedLineTradeDelivery>
                <ram:BilledQuantity unitCode="H87">5.0</ram:BilledQuantity>
                <ram:PackageQuantity unitCode="XPK">2.0</ram:PackageQuantity>
                <ram:ActualDeliverySupplyChainEvent>
                  <ram:OccurrenceDateTime>
                    <udt:DateTimeString format="102">20260310</udt:DateTimeString>
                  </ram:OccurrenceDateTime>
                </ram:ActualDeliverySupplyChainEvent>
              </ram:SpecifiedLineTradeDelivery>
              <ram:SpecifiedLineTradeSettlement>
                <ram:ApplicableTradeTax>
                  <ram:TypeCode>VAT</ram:TypeCode>
                  <ram:CategoryCode>S</ram:CategoryCode>
                  <ram:RateApplicablePercent>19.00</ram:RateApplicablePercent>
                  <ram:CalculatedAmount>10.26</ram:CalculatedAmount>
                </ram:ApplicableTradeTax>
              </ram:SpecifiedLineTradeSettlement>
            </ram:IncludedSupplyChainTradeLineItem>
            <ram:ApplicableHeaderTradeAgreement>
              <ram:SellerTradeParty>
                <ram:Name>Muster GmbH</ram:Name>
                <ram:GlobalID schemeID="0088">4000001000005</ram:GlobalID>
                <ram:DefinedTradeContact>
                  <ram:PersonName>Erika Musterfrau</ram:PersonName>
                  <ram:DepartmentName>Buchhaltung</ram:DepartmentName>
                  <ram:FaxUniversalCommunication>
                    <ram:CompleteNumber>+49 100 200</ram:CompleteNumber>
                  </ram:FaxUniversalCommunication>
                </ram:DefinedTradeContact>
                <ram:DefinedTradeContact>
                  <ram:PersonName>Max Muster</ram:PersonName>
                </ram:DefinedTradeContact>
                <ram:SpecifiedLegalOrganization>
                  <ram:ID>HRB 4711</ram:ID>
                  <ram:PostalTradeAddress><ram:CityName>Salzgitter</ram:CityName></ram:PostalTradeAddress>
                </ram:SpecifiedLegalOrganization>
              </ram:SellerTradeParty>
              <ram:SpecifiedProcuringProject>
                <ram:ID>PR-9</ram:ID>
                <ram:Name>Neubau</ram:Name>
              </ram:SpecifiedProcuringProject>
              <ram:QuotationReferencedDocument>
                <ram:IssuerAssignedID>ANG-5</ram:IssuerAssignedID>
              </ram:QuotationReferencedDocument>
            </ram:ApplicableHeaderTradeAgreement>
            <ram:ApplicableHeaderTradeDelivery>
              <ram:ShipToTradeParty>
                <ram:Name>Baustelle Nord</ram:Name>
                <ram:GlobalID schemeID="0088">4304171088093</ram:GlobalID>
              </ram:ShipToTradeParty>
              <ram:ShipFromTradeParty><ram:Name>Lager Süd</ram:Name></ram:ShipFromTradeParty>
              <ram:RelatedSupplyChainConsignment>
                <ram:SpecifiedLogisticsTransportMovement>
                  <ram:ModeCode>3</ram:ModeCode>
                </ram:SpecifiedLogisticsTransportMovement>
              </ram:RelatedSupplyChainConsignment>
            </ram:ApplicableHeaderTradeDelivery>
            <ram:ApplicableHeaderTradeSettlement>
              <ram:InvoiceCurrencyCode>EUR</ram:InvoiceCurrencyCode>
              <ram:InvoiceeTradeParty><ram:Name>Zentrale AG</ram:Name></ram:InvoiceeTradeParty>
              <ram:SpecifiedLogisticsServiceCharge>
                <ram:Description>Fracht</ram:Description>
                <ram:AppliedAmount>15.00</ram:AppliedAmount>
              </ram:SpecifiedLogisticsServiceCharge>
              <ram:SpecifiedTradePaymentTerms>
                <ram:Description>Zahlbar in 30 Tagen.</ram:Description>
                <ram:ApplicableTradePaymentDiscountTerms>
                  <ram:BasisPeriodMeasure unitCode="DAY">14</ram:BasisPeriodMeasure>
                  <ram:CalculationPercent>2.00</ram:CalculationPercent>
                </ram:ApplicableTradePaymentDiscountTerms>
                <ram:ApplicableTradePaymentPenaltyTerms>
                  <ram:CalculationPercent>5.00</ram:CalculationPercent>
                </ram:ApplicableTradePaymentPenaltyTerms>
              </ram:SpecifiedTradePaymentTerms>
              <ram:SpecifiedTradePaymentTerms>
                <ram:Description>Ein zweiter Zahlungsbedingungssatz.</ram:Description>
              </ram:SpecifiedTradePaymentTerms>
              <ram:SpecifiedAdvancePayment>
                <ram:PaidAmount>50.00</ram:PaidAmount>
                <ram:FormattedReceivedDateTime>
                  <qdt:DateTimeString format="102">20260201</qdt:DateTimeString>
                </ram:FormattedReceivedDateTime>
              </ram:SpecifiedAdvancePayment>
              <ram:TaxApplicableTradeCurrencyExchange>
                <ram:SourceCurrencyCode>EUR</ram:SourceCurrencyCode>
                <ram:TargetCurrencyCode>CHF</ram:TargetCurrencyCode>
                <ram:ConversionRate>0.95</ram:ConversionRate>
              </ram:TaxApplicableTradeCurrencyExchange>
            </ram:ApplicableHeaderTradeSettlement>
          </rsm:SupplyChainTradeTransaction>
        </rsm:CrossIndustryInvoice>
        """;

    private static Invoice Invoice => InvoiceReader.ReadXml(Extended);

    [Test]
    public void ReadsTheTitleOfTheDocument()
    {
        Assert.That(Invoice.Extensions?.Title, Is.EqualTo("RECHNUNG"));
    }

    [Test]
    public void ReadsTheContentCodeOfANote()
    {
        var note = Invoice.Notes.Single();

        Assert.That(note.SubjectCode, Is.EqualTo("AAI"));
        Assert.That(note.Extensions?.ContentCode, Is.EqualTo("ST3"));
    }

    [Test]
    public void ReadsTheInvoiceeAndTheShipFromAndTheShipToParty()
    {
        var extensions = Invoice.Extensions;

        Assert.That(extensions?.Invoicee?.Name, Is.EqualTo("Zentrale AG"));
        Assert.That(extensions?.ShipFrom?.Name, Is.EqualTo("Lager Süd"));

        /* The core holds the name of the ship-to party, the extension its full data. */
        Assert.That(Invoice.Delivery?.PartyName, Is.EqualTo("Baustelle Nord"));
        Assert.That(extensions?.ShipTo?.Extensions?.GlobalIdentifiers.Single().Value,
            Is.EqualTo("4304171088093"));
    }

    [Test]
    public void ReadsMoreThanOneContact()
    {
        var seller = Invoice.Seller;

        /* The core holds the first contact, as EN 16931 gives one per party. */
        Assert.That(seller?.Contact?.Name, Is.EqualTo("Erika Musterfrau"));

        Assert.That(seller?.Extensions?.Contacts.Select(contact => contact.Name),
            Is.EqualTo(new[] { "Erika Musterfrau", "Max Muster" }));
        Assert.That(seller?.Extensions?.Contacts[0].Extensions?.DepartmentName,
            Is.EqualTo("Buchhaltung"));
        Assert.That(seller?.Extensions?.Contacts[0].Extensions?.Fax, Is.EqualTo("+49 100 200"));
    }

    [Test]
    public void ReadsTheAddressOfTheLegalOrganisation()
    {
        Assert.That(Invoice.Seller?.Extensions?.LegalOrganizationAddress?.City,
            Is.EqualTo("Salzgitter"));
    }

    [Test]
    public void ReadsTheNameOfTheProject()
    {
        Assert.That(Invoice.ProjectReference, Is.EqualTo("PR-9"));
        Assert.That(Invoice.Extensions?.ProjectName, Is.EqualTo("Neubau"));
    }

    [Test]
    public void ReadsTheQuotation()
    {
        Assert.That(Invoice.Extensions?.Quotation?.Reference, Is.EqualTo("ANG-5"));
    }

    [Test]
    public void ReadsTheModeOfTransport()
    {
        Assert.That(Invoice.Extensions?.TransportModeCodes, Is.EqualTo(new[] { "3" }));
    }

    [Test]
    public void ReadsEachPaymentTermWithItsDiscountAndItsPenalty()
    {
        var terms = Invoice.Extensions?.PaymentTerms;

        Assert.That(terms, Has.Count.EqualTo(2));
        Assert.That(terms![0].Discounts, Has.Count.EqualTo(1));
        Assert.That(terms[0].Discounts[0].Percentage, Is.EqualTo(2.00m));
        Assert.That(terms[0].Discounts[0].BasisPeriodDays, Is.EqualTo(14m));
        Assert.That(terms[0].Penalty?.Percentage, Is.EqualTo(5.00m));
        Assert.That(terms[1].Description, Is.EqualTo("Ein zweiter Zahlungsbedingungssatz."));

        /* The core holds the text of the first term (BT-20). */
        Assert.That(Invoice.PaymentTerms, Is.EqualTo("Zahlbar in 30 Tagen."));
    }

    [Test]
    public void ReadsTheAdvancePayment()
    {
        var payment = Invoice.Extensions?.AdvancePayments.Single();

        Assert.That(payment?.PaidAmount?.Value, Is.EqualTo(50.00m));
        Assert.That(payment?.ReceivedDate, Is.EqualTo(new DateOnly(2026, 2, 1)));
    }

    [Test]
    public void ReadsTheLogisticsServiceCharge()
    {
        var charge = Invoice.Extensions?.LogisticsServiceCharges.Single();

        Assert.That(charge?.Description, Is.EqualTo("Fracht"));
        Assert.That(charge?.Amount?.Value, Is.EqualTo(15.00m));
    }

    [Test]
    public void ReadsTheExchangeRateOfTheTax()
    {
        var exchange = Invoice.Extensions?.TaxCurrencyExchange;

        Assert.That(exchange?.SourceCurrencyCode, Is.EqualTo("EUR"));
        Assert.That(exchange?.TargetCurrencyCode, Is.EqualTo("CHF"));
        Assert.That(exchange?.Rate, Is.EqualTo(0.95m));
    }

    [Test]
    public void ReadsTheDataOfThePosition()
    {
        var line = Invoice.Lines.Single();
        var extensions = line.Extensions;

        Assert.That(extensions?.ActualDeliveryDate, Is.EqualTo(new DateOnly(2026, 3, 10)));
        Assert.That(extensions?.PackageQuantity?.Value, Is.EqualTo(2.0m));
        Assert.That(extensions?.PackageQuantity?.UnitCode, Is.EqualTo("XPK"));
        Assert.That(extensions?.Notes.Single().Text, Is.EqualTo("Zur Position."));
        Assert.That(extensions?.Tax.Single().CalculatedAmount?.Value, Is.EqualTo(10.26m));
    }

    [Test]
    public void ReadsTheSerialNumberAndTheIncludedProduct()
    {
        var item = Invoice.Lines.Single().Item.Extensions;

        Assert.That(item?.IndustryIdentifier, Is.EqualTo("IND-9"));
        Assert.That(item?.Instances.Single().SerialId, Is.EqualTo("SN-4711"));
        Assert.That(item?.Instances.Single().BatchId, Is.EqualTo("CHARGE-7"));
        Assert.That(item?.IncludedProducts.Single().Name, Is.EqualTo("Kaffeetasse"));
        Assert.That(item?.IncludedProducts.Single().UnitQuantity?.Value, Is.EqualTo(6.0m));
        Assert.That(item?.ClassificationNames.Single(), Is.EqualTo("Zolltarifnummer"));
    }

    [Test]
    public void ReadsTheTwoBaseQuantitiesAndTheDiscountOnTheGrossPrice()
    {
        var price = Invoice.Lines.Single().Price;

        Assert.That(price.NetPrice, Is.EqualTo(10.80m));
        Assert.That(price.Extensions?.NetBaseQuantity?.Value, Is.EqualTo(1.0m));
        Assert.That(price.Extensions?.GrossBaseQuantity?.Value, Is.EqualTo(10.0m));

        var allowance = price.Extensions?.GrossPriceAllowances.Single();
        Assert.That(allowance?.Percentage, Is.EqualTo(10.00m));
        Assert.That(allowance?.Amount, Is.EqualTo(1.20m));
        Assert.That(allowance?.Reason, Is.EqualTo("Mengenrabatt"));
    }

    /*
     * A group that the document does not give must leave its extension null. The UBL
     * fixture gives a payment term, thus the invoice has extensions; but it gives no
     * group that only CII holds, and it gives nothing over the core for its position.
     */
    [Test]
    public void ADocumentGivesNoExtensionForAGroupThatItDoesNotHold()
    {
        var invoice = InvoiceReader.ReadXml(Fixtures.Ubl);

        Assert.That(invoice.Lines.Single().Extensions, Is.Null);
        Assert.That(invoice.Lines.Single().Item.Extensions, Is.Null);
        Assert.That(invoice.Lines.Single().Price.Extensions, Is.Null);
        Assert.That(invoice.Notes.Single().Extensions, Is.Null);
    }

    /*
     * UBL does not hold each group that CII holds. Such a group stays empty, and the
     * renderer then shows nothing for it.
     */
    [Test]
    public void AUblDocumentHasNoGroupThatOnlyCiiHolds()
    {
        var extensions = InvoiceReader.ReadXml(Fixtures.Ubl).Extensions;

        Assert.That(extensions, Is.Not.Null, "The fixture gives a payment term.");
        Assert.That(extensions!.Title, Is.Null);
        Assert.That(extensions.Invoicee, Is.Null);
        Assert.That(extensions.LogisticsServiceCharges, Is.Empty);
        Assert.That(extensions.AdvancePayments, Is.Empty);
        Assert.That(extensions.TransportModeCodes, Is.Empty);
    }

    /* The groups that both syntaxes hold must read the same from both. */
    [Test]
    public void AUblDocumentGivesThePaymentTermAndTheTaxRegistration()
    {
        var invoice = InvoiceReader.ReadXml(Fixtures.Ubl);

        var term = invoice.Extensions!.PaymentTerms.Single();
        Assert.That(term.Description, Is.EqualTo("Zahlbar bis zum 14.04.2026."));
        Assert.That(term.DueDate, Is.EqualTo(new DateOnly(2026, 4, 14)));

        var registration = invoice.Seller?.Extensions?.TaxRegistrations.Single();
        Assert.That(registration?.Value, Is.EqualTo("DE123456788"));
        Assert.That(registration?.SchemeId, Is.EqualTo("VAT"));
    }
}
