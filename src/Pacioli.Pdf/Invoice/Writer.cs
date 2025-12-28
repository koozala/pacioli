
using Pacioli.Pdf.ERechnung;
using System;

namespace Pacioli.Pdf.Invoice
{
    public class Writer
    {
        public bool IsZugferd { get { return _isZugferd; } }

        private bool _isZugferd = false;

        InvoiceWriter? writer = null;


        public static void Write(string inputFile, string outputFile, string attachmentsTargetPath)
        {
            new InvoiceWriter(inputFile, attachmentsTargetPath).Write(outputFile);
        }

        public Writer(string inputFile, string attachmentsTargetPath)
        {
            writer = Reader.Open(inputFile, attachmentsTargetPath, out _isZugferd);
        }

        public int GetAttachmentCount()
        {
            if (writer == null)
            {
                throw new InvalidOperationException("Writer not initialized.");
            }
            return writer.CountAttachments();
        }

        public void Write(string outputFile)
        {
            if (writer == null)
            {
                throw new InvalidOperationException("Writer not initialized.");
            }
            writer.Write(outputFile);
        }

    }
}
