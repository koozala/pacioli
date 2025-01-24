
using Pacioli.Pdf.ERechnung;

namespace Pacioli.Pdf.Invoice
{
    public class Writer
    {
        public bool IsZugferd { get { return _isZugferd; } }

        public byte[]? XmlData { get { return xmlData; } }

        private bool _isZugferd = false;

        private byte[]? xmlData;


        public static void Write(string inputFile, string outputFile, string attachmentsTargetPath)
        {
            new InvoiceWriter(inputFile, attachmentsTargetPath).Write(outputFile);
        }

        InvoiceWriter writer;

        public Writer(string inputFile, string attachmentsTargetPath)
        {
            writer = Reader.Open(inputFile, attachmentsTargetPath, out _isZugferd, out xmlData);
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
