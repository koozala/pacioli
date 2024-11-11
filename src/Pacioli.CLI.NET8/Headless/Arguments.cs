using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pacioli.CLI.NET8.Headless
{
    internal class Arguments
    {
        public bool NoArgs = true;
        public bool? Help {  get; set; } = null;
        public string? InputFile { get; set; } = null;
        public string? OutputFolder { get; set; } = null;
        public bool? GenPdf { get; set; } = null;
        public string? PdfName { get; set; } = null;
        public string? XmlName { get; set; } = null;

        public Arguments(string[] args)
        {
            int i = 0;
            while (i < args.Length)
            {
                NoArgs = false;
                if (args[i] == "-?")
                {
                    Help = true;
                    i++;
                }
                else if (args[i] == "-in")
                {
                    InputFile = args[i + 1];
                    i += 2;
                }
                else if (args[i] == "-output-folder")
                {
                    OutputFolder = args[i + 1];
                    i += 2;
                }
                else if (args[i] == "-genpdf")
                {
                    GenPdf = true;
                    i++;
                }
                else if (args[i] == "-pdfname")
                {
                    PdfName = args[i + 1];
                    i += 2;
                }
                else if (args[i] == "-xmlname")
                {
                    XmlName = args[i + 1];
                    i += 2;
                }
                else
                {
                    throw new ArgumentException($"Unknown parameter: {args[i]}");
                }
            }
        }
    }
}
