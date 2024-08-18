using Pacioli.WindowsApp.NET8.Config;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
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
            cdb.SavePreferences(pref);
            this.Close();
        }
    }
}
