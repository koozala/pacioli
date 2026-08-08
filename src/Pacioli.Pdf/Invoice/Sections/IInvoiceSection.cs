using iText.Layout;

namespace Pacioli.Pdf.Invoice.Sections
{
    /*
     * One part of the content of the rendered invoice.
     *
     * InvoiceWriter holds an ordered list of sections. It gives the document and the data
     * to each section, one after the other. Thus you can add a part, remove a part, or
     * change the order of the parts, without a change of InvoiceWriter.
     *
     * A section must write nothing if the invoice has no data for it.
     */
    public interface IInvoiceSection
    {
        /* A short name. The list InvoiceSections.Default uses it, and a report can show it. */
        string Name { get; }

        /* Writes the part to the document. */
        void Write(Document doc, InvoiceContext context);
    }
}
