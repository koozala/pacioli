using Pacioli.Language.Resources;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pacioli.Config.Persistence
{
    public class UserPreferences
    {
        public string UserName { get; set; }
        public string DefaultFolder { get; set; }
        public string LanguageCode { get; set; }
        public string AttachmentOutputFolder { get; set; }
        public bool OpenAfterSave { get; set; }
        public bool PerformUpdateCheck { get; set; }
        public bool SendRatingData { get; set; }

        public static UserPreferences LoadPreferences()
        {
            ConfigDb cdb = new ConfigDb();
            UserPreferences preferences = cdb.ReadPreferences();
            preferences.SetCulture();
            return preferences;
        }

        public UserPreferences()
        {
            UserName = Environment.UserName;
            DefaultFolder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            AttachmentOutputFolder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            LanguageCode = CultureInfo.CurrentCulture.TwoLetterISOLanguageName;
            OpenAfterSave = true;
            PerformUpdateCheck = true;
            SendRatingData = true;
        }

        /*
         * Takes the language of the preferences.
         *
         * A code that no culture knows must not stop the program. The database can hold
         * such a code: a version before wrote it, or a person changed the file by hand.
         * The language of the computer then applies, as with the code "System".
         */
        public void SetCulture()
        {
            CultureInfo culture = CultureOf(LanguageCode);

            CultureInfo.CurrentCulture = culture;
            CultureInfo.CurrentUICulture = culture;
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;
            Resources.Culture = culture;
        }

        static CultureInfo CultureOf(string languageCode)
        {
            if (string.IsNullOrWhiteSpace(languageCode) || languageCode == "System")
            {
                return CultureInfo.InstalledUICulture;
            }

            try
            {
                return new CultureInfo(languageCode);
            }
            catch (CultureNotFoundException)
            {
                return CultureInfo.InstalledUICulture;
            }
        }
    }
}
