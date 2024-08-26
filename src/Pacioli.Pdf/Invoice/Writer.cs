
namespace Pacioli.Pdf.Invoice
{
    public class Writer
    {
        public static void Write(string inputFile, string outputFile, string attachmentsTargetPath)
        {
            new InvoiceWriter(inputFile, attachmentsTargetPath).Write(outputFile);
        }

        InvoiceWriter writer;

        public Writer(string inputFile, string attachmentsTargetPath)
        {
            writer = new InvoiceWriter(inputFile, attachmentsTargetPath);
        }

        public int GetAttachmentCount()
        {
            return writer.CountAttachments();
        }

        public void Write(string outputFile)
        {

            writer.Write(outputFile);
        }

    }
}
