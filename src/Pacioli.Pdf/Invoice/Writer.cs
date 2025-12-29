
using Pacioli.Pdf.ERechnung;
using System;
using System.IO;

namespace Pacioli.Pdf.Invoice
{
    public class Writer
    {
        public bool IsZugferd { get { return _isZugferd; } }

        private bool _isZugferd = false;

        InvoiceWriter? writer = null;

        Reader reader;



        public static void Write(string inputFile, string outputFile, string attachmentsTargetPath)
        {
            new InvoiceWriter(inputFile, attachmentsTargetPath).Write(outputFile);
        }

        public Writer(string inputFile, string attachmentsTargetPath, Stream xmlDataStream)
        {
            reader = new Reader();
            writer = reader.Open(inputFile, attachmentsTargetPath, out _isZugferd, xmlDataStream);
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
