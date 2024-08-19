
namespace Pacioli.Pdf.Invoice
{
    public class Writer
    {
        public static void Write(string inputFile, string outputFile, string attachmentsTargetPath)
        {
            new InvoiceWriter(inputFile, attachmentsTargetPath).Write(outputFile); 
        }
    }
}
