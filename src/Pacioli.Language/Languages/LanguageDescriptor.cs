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
        public bool IsSystem { get; set; }

        public LanguageDescriptor(string code, string name)
        {

            Code = code;
            Name = name;
            Info = new CultureInfo(code);
            IsSystem = false;
        }

        public LanguageDescriptor()
        {
            Code = "System";
            Name = "System Default";
            Info = CultureInfo.InstalledUICulture;
            IsSystem = true;
        }

        public override string ToString()
        {
            if (IsSystem)
            {
                return "System Default";
            }
            else
            {
                return $"{Info.Name} - {Info.NativeName}";
            }
        }
    }
}
