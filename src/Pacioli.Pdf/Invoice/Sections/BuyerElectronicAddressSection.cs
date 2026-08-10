using iText.Layout;
using iText.Layout.Element;
using En16931.Model;
using Pacioli.Language.Resources;
using CodeLists = En16931.Codes.CodeLists;

namespace Pacioli.Pdf.Invoice.Sections
{
    /* The electronic address of the buyer, with the code of its scheme. */
    internal sealed class BuyerElectronicAddressSection : IInvoiceSection
    {
        public string Name => "Electronic address of the buyer";

        public void Write(Document doc, InvoiceContext context)
        {
            Identifier? address = context.Core.Buyer?.ElectronicAddress;

            if (address != null)
            {
                doc.Add(new Paragraph($"{Resources.buyerElectronicAddress}: {CodeText.Name(CodeLists.ElectronicAddressScheme, address.SchemeId)} {address.Value}"));
            }
        }
    }
}
