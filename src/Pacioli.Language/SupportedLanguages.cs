using Pacioli.Language.Languages;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Pacioli.Language
{
    public class SupportedLanguages
    {
        public List<LanguageDescriptor> Languages { get; set; }

        public SupportedLanguages() { 
            Languages = new List<LanguageDescriptor>();
            Languages.Add(new LanguageDescriptor()); // System Default
            Languages.Add(new LanguageDescriptor("de", "Deutsch"));
            Languages.Add(new LanguageDescriptor("en", "English"));
            Languages.Add(new LanguageDescriptor("fr", "Francais")); // TBD
        }
    }
}
