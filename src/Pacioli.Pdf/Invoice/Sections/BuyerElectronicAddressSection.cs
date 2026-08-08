using iText.Layout;
using iText.Layout.Element;
using Vit.FeRD.ExtendedDocument.Model;

namespace Pacioli.Pdf.Invoice.Sections
{
    /* The electronic address of the buyer, with the code of its scheme. */
    internal sealed class BuyerElectronicAddressSection : IInvoiceSection
    {
        public string Name => "Electronic address of the buyer";

        public void Write(Document doc, InvoiceContext context)
        {
            UniversalCommunication? buyerAddress = context.Agreement?.BuyerTradeParty?.UriUniversalCommunication;

            if (buyerAddress?.UriId != null)
            {
                doc.Add(new Paragraph($"{buyerAddress.UriId.SchemeId} {buyerAddress.UriId.Value}"));
            }
        }
    }
}
