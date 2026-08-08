using System.Collections.Generic;

namespace Pacioli.Pdf.Invoice.Sections
{
    /*
     * The parts of the invoice, in the order of the page.
     *
     * HOW TO CHANGE THE CONTENT OF THE PAGE
     *
     *   Add a part      Write a class with the interface IInvoiceSection, then put it in
     *                   the list at the correct position.
     *   Remove a part   Take the entry out of the list, or remove it from
     *                   InvoiceWriter.Sections before you call Write.
     *   Move a part     Move the entry in the list.
     *
     * InvoiceWriter.Sections holds this list. You can change the list of one writer
     * without a change of this class:
     *
     *     var writer = new InvoiceWriter(fileName);
     *     writer.Sections.RemoveAll(section => section.Name == "Notes");
     *     writer.Write(outputFileName);
     */
    public static class InvoiceSections
    {
        /* Makes the list of the parts. Each call gives a new list. */
        public static List<IInvoiceSection> Default() => new List<IInvoiceSection>
        {
            new HeaderSection(),
            new PartiesSection(),
            new ReferencesSection(),
            new LineItemsSection(),
            new AttachmentsSection(),
            new TaxSummarySection(),
            new AllowanceChargeSection(),
            new TotalsSection(),
            new BankAccountsSection(),
            new PaymentMeansSection(),
            new BuyerElectronicAddressSection(),
            new PaymentTermsSection(),
            new ContactsSection(),
            new NotesSection(),
        };
    }
}
