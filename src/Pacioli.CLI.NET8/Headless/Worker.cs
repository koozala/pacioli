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
            Console.WriteLine("  pacioli.exe -in <inputFile> -out <outputFile>");
        }

        public void Run()
        {
            Console.WriteLine($"In: {Args.InputFile} Out: {Args.OutputFile}");
            if (Args.InputFile == null || Args.OutputFile == null)
            {
                Usage();
                return;
            }
            InvoiceWriter writer = new InvoiceWriter(Args.InputFile, null);
            writer.Write(Args.OutputFile);
        }
    }
}
