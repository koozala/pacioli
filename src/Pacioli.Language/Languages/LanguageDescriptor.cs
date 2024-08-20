using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Pacioli.Language.Languages
{
    public class LanguageDescriptor
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public CultureInfo Info { get; set; }

        public LanguageDescriptor(string code, string name)
        {

            Code = code; 
            Name = name;
            Info = new CultureInfo(code);
        }

        public override string ToString()
        {
            return $"{Info.Name} - {Info.NativeName}";
        }
    }
}
