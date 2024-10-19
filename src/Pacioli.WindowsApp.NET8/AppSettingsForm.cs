using Pacioli.Config.Persistence;
using Pacioli.Language;
using Pacioli.Language.Languages;
using Pacioli.Language.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Pacioli.WindowsApp.NET8
{
    public partial class AppSettingsForm : Form
    {
        public AppSettingsForm()
        {
            InitializeComponent();
            var cdb = new ConfigDb();
            var pref = cdb.ReadPreferences();
            attDestTb.Text = pref.AttachmentOutputFolder;
            folderBrowserDialog1.SelectedPath = pref.AttachmentOutputFolder;
            SupportedLanguages languages = new SupportedLanguages();
            languageCombo.Items.Clear();
            foreach (var lang in languages.Languages)
            {
                languageCombo.Items.Add(lang);
            }
            var selectedLanguage = languages.Languages.FirstOrDefault(x => x.Code == pref.LanguageCode);
            if (selectedLanguage != null)
            {
                languageCombo.SelectedItem = selectedLanguage;
            }
            F_OpenAfterSave.Checked = pref.OpenAfterSave;
        }

        private void selectFolderBtn_Click(object sender, EventArgs e)
        {
            var r = folderBrowserDialog1.ShowDialog();
            if (r == DialogResult.OK)
            {
                attDestTb.Text = folderBrowserDialog1.SelectedPath;
            }
        }

        private void saveBtn_Click(object sender, EventArgs e)
        {
            var cdb = new ConfigDb();
            var pref = cdb.ReadPreferences();
            pref.AttachmentOutputFolder = folderBrowserDialog1.SelectedPath;
            if (languageCombo.SelectedItem != null)
            {
                pref.LanguageCode = ((LanguageDescriptor)languageCombo.SelectedItem).Code;
            }
            pref.OpenAfterSave = F_OpenAfterSave.Checked;
            cdb.SavePreferences(pref);
            this.Close();

            pref.SetCulture();
        }
    }
}
