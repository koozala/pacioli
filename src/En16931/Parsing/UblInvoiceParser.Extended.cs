using System.Xml.Linq;
using En16931.Model;
using En16931.Model.Extended;
using static En16931.Parsing.Namespaces;

namespace En16931.Parsing;

/// <summary>
/// The part of the UBL parser that reads the data over the core of EN 16931.
/// </summary>
/// <remarks>
/// <para>
/// UBL does not hold each group that CII holds: it has no title of the document, no
/// content code of a note, no invoicee party and no logistics service charge. Such a
/// property stays null for a UBL document.
/// </para>
/// <para>
/// The groups below exist in both syntaxes. A reader of the model must not see which
/// syntax the document had.
/// </para>
/// </remarks>
public sealed partial class UblInvoiceParser
{
    /// <summary>The extensions of the invoice. The method makes them on the first call.</summary>
    private static InvoiceExtensions Ext(Invoice invoice) =>
        invoice.Extensions ??= new InvoiceExtensions();

    private static void ReadExtensions(XElement root, Invoice invoice)
    {
        ReadDeliveryExtensions(root.El(Cac + "Delivery"), invoice);
        ReadDocumentExtensions(root, invoice);
        ReadPaymentTermExtensions(root, invoice);
    }

    /*
     * The party that gets the goods. The core holds its name, its location and its
     * address (BG-13 and BG-15); the renderer shows the party as a whole.
     */
    private static void ReadDeliveryExtensions(XElement? delivery, Invoice invoice)
    {
        if (delivery is null) return;

        var location = delivery.El(Cac + "DeliveryLocation");
        var name = delivery.Path(Cac + "DeliveryParty", Cac + "PartyName", Cbc + "Name").Text();
        var address = ReadAddress(location.El(Cac + "Address"));
        var id = ReadIdentifier(location.El(Cbc + "ID"));

        if (name is null && address is null && id is null) return;

        var party = new Party
        {
            Name = name,
            PostalAddress = address
        };

        if (id is not null) party.Identifiers.Add(id);

        Ext(invoice).ShipTo = party;
    }

    private static void ReadDocumentExtensions(XElement root, Invoice invoice)
    {
        var extensions = Ext(invoice);

        extensions.DespatchAdvice = ReadReferencedDocument(root.El(Cac + "DespatchDocumentReference"));
        extensions.DeliveryNote = ReadReferencedDocument(root.El(Cac + "ReceiptDocumentReference"));

        /* The group as the document gives it, before the split by the type code. */
        foreach (var reference in root.Els(Cac + "AdditionalDocumentReference"))
        {
            var document = ReadReferencedDocument(reference);
            if (document is not null) extensions.AdditionalDocuments.Add(document);
        }
    }

    /*
     * The payment terms. UBL gives the text of a term, and the day of the payment stands
     * on the document (BT-9). A term can also give a discount or a penalty as a
     * percentage.
     */
    private static void ReadPaymentTermExtensions(XElement root, Invoice invoice)
    {
        var terms = root.Els(Cac + "PaymentTerms").ToList();
        var mandate = root.Path(Cac + "PaymentMeans", Cac + "PaymentMandate", Cbc + "ID").Text();

        /*
         * BT-9 stands on the document. If the document gives a due date but no payment
         * term, the model must still carry the date, because the page shows it with the
         * terms.
         */
        if (terms.Count == 0 && mandate is null && invoice.PaymentDueDate is null) return;

        var extensions = Ext(invoice);
        var first = true;

        foreach (var term in terms)
        {
            var discount = term.El(Cbc + "SettlementDiscountPercent").Dec();
            var penalty = term.El(Cbc + "PenaltySurchargePercent").Dec();

            var paymentTerm = new PaymentTerm
            {
                Description = term.El(Cbc + "Note").Text(),

                /* BT-9 stands on the document, thus it belongs to the first term. */
                DueDate = first ? invoice.PaymentDueDate : null,
                DirectDebitMandateReference = first ? mandate : null,

                Penalty = penalty is null ? null : new PaymentTermCondition
                {
                    Percentage = penalty
                }
            };

            if (discount is not null)
            {
                paymentTerm.Discounts.Add(new PaymentTermCondition
                {
                    Percentage = discount,
                    Amount = ReadAmount(term.El(Cbc + "Amount"))
                });
            }

            extensions.PaymentTerms.Add(paymentTerm);

            first = false;
        }

        if (extensions.PaymentTerms.Count == 0)
        {
            extensions.PaymentTerms.Add(new PaymentTerm
            {
                DueDate = invoice.PaymentDueDate,
                DirectDebitMandateReference = mandate
            });
        }
    }

    /*
     * The tax registrations of a party, each one with its scheme. The core holds one VAT
     * identifier and one other registration (BT-31 and BT-32), and it loses the scheme.
     */
    private static PartyExtensions? ReadPartyExtensions(XElement party)
    {
        var extensions = new PartyExtensions();

        foreach (var scheme in party.Els(Cac + "PartyTaxScheme"))
        {
            var value = scheme.El(Cbc + "CompanyID").Text();
            if (value is null) continue;

            extensions.TaxRegistrations.Add(
                new Identifier(value, scheme.Path(Cac + "TaxScheme", Cbc + "ID").Text()));
        }

        /* An identifier under a scheme, for example a GLN. */
        foreach (var identification in party.Els(Cac + "PartyIdentification"))
        {
            var identifier = ReadIdentifier(identification.El(Cbc + "ID"));
            if (identifier?.SchemeId is not null
                && !string.Equals(identifier.SchemeId, "SEPA", StringComparison.OrdinalIgnoreCase))
            {
                extensions.GlobalIdentifiers.Add(identifier);
            }
        }

        return extensions.TaxRegistrations.Count == 0 && extensions.GlobalIdentifiers.Count == 0
            ? null
            : extensions;
    }

    private static ReferencedDocument? ReadReferencedDocument(XElement? document)
    {
        if (document is null) return null;

        var attachment = document.El(Cac + "Attachment");

        return new ReferencedDocument
        {
            Reference = document.El(Cbc + "ID").Text(),
            Name = document.El(Cbc + "DocumentDescription").Text(),
            IssueDate = document.El(Cbc + "IssueDate").IsoDate(),
            Uri = attachment.Path(Cac + "ExternalReference", Cbc + "URI").Text(),
            LineId = document.El(Cbc + "LineID").Text(),
            ReferenceTypeCode = document.El(Cbc + "DocumentTypeCode").Text()
        };
    }

    /* The full tax of one entry, for the VAT breakdown and for a position. */
    private static TaxDetail ReadTaxDetail(XElement subtotal)
    {
        var category = subtotal.El(Cac + "TaxCategory");

        return new TaxDetail
        {
            TypeCode = category.Path(Cac + "TaxScheme", Cbc + "ID").Text(),
            CategoryCode = category.El(Cbc + "ID").Text(),
            RatePercent = category.El(Cbc + "Percent").Dec(),
            CalculatedAmount = ReadAmount(subtotal.El(Cbc + "TaxAmount")),
            BasisAmount = ReadAmount(subtotal.El(Cbc + "TaxableAmount")),
            ExemptionReason = category.El(Cbc + "TaxExemptionReason").Text(),
            ExemptionReasonCode = category.El(Cbc + "TaxExemptionReasonCode").Text()
        };
    }

    private static InvoiceLineExtensions? ReadLineExtensions(XElement line, bool isCreditNote)
    {
        var extensions = new InvoiceLineExtensions();

        var note = line.El(Cbc + "Note").Text();
        if (note is not null)
        {
            extensions.Notes.Add(new InvoiceNote { Text = note });
        }

        foreach (var total in line.Els(Cac + "TaxTotal"))
        {
            foreach (var subtotal in total.Els(Cac + "TaxSubtotal"))
            {
                extensions.Tax.Add(ReadTaxDetail(subtotal));
            }
        }

        foreach (var reference in line.Els(Cac + "DocumentReference"))
        {
            var document = ReadReferencedDocument(reference);
            if (document is null) continue;

            extensions.Documents.Add(new SupportingDocument
            {
                Reference = document.Reference,
                Description = document.Name,
                ExternalLocation = document.Uri,
                Extensions = new SupportingDocumentExtensions
                {
                    IssueDate = document.IssueDate,
                    LineId = document.LineId,
                    TypeCode = document.ReferenceTypeCode
                }
            });
        }

        extensions.PurchaseOrder = ReadReferencedDocument(line.El(Cac + "OrderLineReference"));

        bool used = extensions.Notes.Count > 0
                    || extensions.Tax.Count > 0
                    || extensions.Documents.Count > 0
                    || extensions.PurchaseOrder is not null;

        return used ? extensions : null;
    }

    /*
     * UBL gives one base quantity for the price. The gross price and its discount stand
     * in the group AllowanceCharge of the price.
     */
    private static PriceExtensions? ReadPriceExtensions(XElement? price)
    {
        if (price is null) return null;

        var extensions = new PriceExtensions
        {
            NetBaseQuantity = ReadQuantity(price.El(Cbc + "BaseQuantity"))
        };

        foreach (var allowance in price.Els(Cac + "AllowanceCharge"))
        {
            extensions.GrossPriceAllowances.Add(ReadAllowanceCharge(allowance, documentLevel: false));
        }

        return extensions.NetBaseQuantity is null && extensions.GrossPriceAllowances.Count == 0
            ? null
            : extensions;
    }

    private static Amount? ReadAmount(XElement? element)
    {
        var value = element.Dec();
        return value is null ? null : new Amount(value.Value, element.Attr("currencyID"));
    }
}
