
namespace Pacioli.Pdf.Invoice
{
    public class Writer
    {
        public static void Write(string inputFile, string outputFile)
        {
            new InvoiceWriter(inputFile).Write(outputFile); 
        }
    }
}
