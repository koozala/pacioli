using Pacioli.Language.Resources;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pacioli.WindowsApp.NET8.Config
{
    public class UserPreferences
    {
        public string UserName { get; set; }
        public string DefaultFolder { get; set; }
        public string LanguageCode { get; set; }
        public string AttachmentOutputFolder { get; set; }
        public bool OpenAfterSave { get; set; }

        public UserPreferences()
        {
            UserName = Environment.UserName;
            DefaultFolder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            AttachmentOutputFolder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            LanguageCode = CultureInfo.CurrentCulture.TwoLetterISOLanguageName;
            OpenAfterSave = true;
        }

        public void SetCulture()
        {
            if (LanguageCode == "System")
            {
                CultureInfo.CurrentCulture = CultureInfo.InstalledUICulture;
                CultureInfo.CurrentUICulture = CultureInfo.InstalledUICulture;
                Thread.CurrentThread.CurrentCulture = CultureInfo.InstalledUICulture;
                Thread.CurrentThread.CurrentUICulture = CultureInfo.InstalledUICulture;
                Resources.Culture = CultureInfo.InstalledUICulture;
            }
            else
            {
                CultureInfo.CurrentCulture = new CultureInfo(LanguageCode);
                Thread.CurrentThread.CurrentCulture = new CultureInfo(LanguageCode);
                Thread.CurrentThread.CurrentUICulture = new CultureInfo(LanguageCode);
                Resources.Culture = new CultureInfo(LanguageCode);
            }
        }
    }
}
