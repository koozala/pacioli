using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pacioli.CLI.NET8.Headless
{
    internal class Arguments
    {
        public string? InputFile { get; set; } = null;
        public string? OutputFile { get; set; } = null;

        public Arguments(string[] args)
        {
            int i = 0;
            while (i < args.Length)
            {
                if (args[i] == "-in")
                {
                    InputFile = args[i + 1];
                    i += 2;
                }
                else if (args[i] == "-out")
                {
                    OutputFile = args[i + 1];
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
