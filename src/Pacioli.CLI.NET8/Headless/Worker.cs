using Pacioli.Pdf.ERechnung;
using Pacioli.Pdf.Invoice;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pacioli.CLI.NET8.Headless
{
    public class Worker
    {
        Arguments Args;

        public Worker(string[] args)
        {
            Args = new Arguments(args);
        }

        public void Usage()
        {
            Console.WriteLine("Usage:");
            Console.WriteLine("pacioli-cmd.exe -in <file> [-output-folder <folder>] [-genpdf [-pdfname <file>]] [-xmlname <file>]");
            Console.WriteLine(" -?                        Show usage");
            Console.WriteLine(" -in <file>                Read input from file, could be a XML or a PDF file (Zugferd input)");
            Console.WriteLine(" -genpdf                   Generate PDF rendering; if no pdfname is given, the filename will be generated");
            Console.WriteLine(" -output-folder <folder>   Where to put output files");
            Console.WriteLine(" -pdfname <file>           Name of the generated PDF file");
            Console.WriteLine(" -xmlname <file>           If given, the XML file will be copied/extracted");
        }

        public void Run()
        {
            if (Args.NoArgs || (Args.Help ?? false) || Args.InputFile == null)
            {
                Usage();
                return;
            }

            ZugferdExtractor zugferdExtractor = new ZugferdExtractor(Args.InputFile);

            if (Args.GenPdf ?? false)
            {
                string p1 = Args.OutputFolder ?? ".";
                string p2 = Args.PdfName ?? "blah.pdf";
                InvoiceWriter writer = new InvoiceWriter(Args.InputFile, null);
                writer.Write(Path.Combine(p1, p2));
            }

            if (Args.XmlName != null)
            {
                string p1 = Args.OutputFolder ?? ".";

                if (zugferdExtractor.IsZugferdFile && zugferdExtractor.XmlDataStream != null)
                {
                    using (FileStream fs = new FileStream(Path.Combine(p1, Args.XmlName ?? "boo.xml"), FileMode.Create, FileAccess.Write))
                    {
                        zugferdExtractor.XmlDataStream.CopyTo(fs);
                    }
                }
                else
                {
                    // just copy the input file

                    using (FileStream inf = new FileStream(Args.InputFile, FileMode.Open, FileAccess.Read))
                    using (FileStream outf = new FileStream(Path.Combine(p1, Args.XmlName ?? "boo.xml"), FileMode.Create, FileAccess.Write))
                    {
                        inf.CopyTo(outf);
                    }
                }
            }

            //InvoiceWriter writer = new InvoiceWriter(Args.InputFile, null);
            //writer.Write(Args.OutputFile);
        }
    }
}
