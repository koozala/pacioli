﻿using iText.Commons.Utils;
using Microsoft.Web.WebView2.Core;
using Pacioli.Config.Persistence;
using Pacioli.Language.Resources;
using Pacioli.Pdf.Invoice;
using Pacioli.Preview.ImageGeneration;
using Pacioli.Updater.AutoUpdate;
using Pacioli.WindowsApp.NET8.Controls;
using Pacioli.WindowsApp.NET8.Util;
using System.Diagnostics;
using System.Drawing.Printing;
using System.Globalization;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Windows.Forms;

namespace Pacioli.WindowsApp.NET8
{
    public partial class MainForm : Form
    {
        ConfigDb cdb;
        UserPreferences preferences;

        public MainForm()
        {
            InitializeComponent();
            InitializePacioli();
        }

        public MainForm(string[] args)
        {
            InitializeComponent();
            InitializePacioli();

            foreach (var file in args)
            {
                AddFile(file);
            }
        }

        private void InitializePacioli()
        {
            this.WindowState = FormWindowState.Maximized;

            F_TabControl.Controls.Clear();
            F_TabControl.AllowDrop = true;
            F_TabControl.DragEnter += MainForm_DragEnter;
            F_TabControl.DragDrop += MainForm_DragDrop;

            cdb = new ConfigDb();
            preferences = cdb.ReadPreferences();
            preferences.SetCulture();
        }

        private void öffnenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ConfigDb cdb = new ConfigDb();
            UserPreferences preferences = cdb.ReadPreferences();

            openFileDialog1.FileName = string.Empty;
            openFileDialog1.InitialDirectory = preferences.DefaultFolder;
            openFileDialog1.Multiselect = true;
            DialogResult dr = openFileDialog1.ShowDialog();

            if (dr == DialogResult.OK)
            {
                foreach (var fileName in openFileDialog1.FileNames)
                {
                    AddFile(fileName);
                }
            }

            var path = Path.GetDirectoryName(openFileDialog1.FileName);
            if (path != null)
            {
                preferences.DefaultFolder = path;
                cdb.SavePreferences(preferences);
            }
        }

        private void AddFile(string fileName)
        {
            RecordPanel rp = new RecordPanel();
            try
            {
                rp.SetFile(fileName, preferences.AttachmentOutputFolder);
                TabPage page = new TabPage();
                page.Text = Path.GetFileName(fileName);

                rp.Dock = DockStyle.Fill;
                page.Controls.Add(rp);
                F_TabControl.Controls.Add(page);
                F_TabControl.SelectedTab = page;

                rp.AllowDrop = true;
                rp.DragEnter += MainForm_DragEnter;
                rp.DragDrop += MainForm_DragDrop;

                rp.FF_TablePanel.AllowDrop = true;
                rp.FF_TablePanel.DragEnter += MainForm_DragEnter;
                rp.FF_TablePanel.DragDrop += MainForm_DragDrop;

                rp.DocPanelDerived.AllowDrop = true;
                rp.DocPanelDerived.DragEnter += MainForm_DragEnter;
                rp.DocPanelDerived.DragDrop += MainForm_DragDrop;

                rp.DocPanelOriginal.AllowDrop = true;
                rp.DocPanelOriginal.DragEnter += MainForm_DragEnter;
                rp.DocPanelOriginal.DragDrop -= MainForm_DragDrop;

                rp.FF_DragDropPanel.AllowDrop = true;
                rp.FF_DragDropPanel.DragEnter += MainForm_DragEnter;
                rp.FF_DragDropPanel.DragDrop += MainForm_DragDrop;
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format(Resources.errorFileReadMessage, fileName, ex.Message));
                return;
            }
        }

        private void beendenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }


        private void speichernToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var currentPage = F_TabControl.SelectedTab;

            if (currentPage == null)
            {
                MessageBox.Show(Resources.msgNoActiveDocument);
                return;
            }

            RecordPanel rp = (RecordPanel)currentPage.Controls[0];

            if (rp.converter == null)
            {
                MessageBox.Show(Resources.msgNoDocumentLoaded);
                return;
            }

            var pref = cdb.ReadPreferences();

            saveFileDialog1.InitialDirectory = pref.AttachmentOutputFolder;
            saveFileDialog1.FileName = Path.GetFileNameWithoutExtension(rp.FF_FileNameTb.Text) + ".pdf";
            var result = saveFileDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                pref.AttachmentOutputFolder = Path.GetDirectoryName(saveFileDialog1.FileName)!;
                cdb.SavePreferences(pref);

                File.WriteAllBytes(saveFileDialog1.FileName, rp.converter.PdfData!);

                if (pref.OpenAfterSave)
                {
                    ProcessStartInfo info = new ProcessStartInfo
                    {
                        FileName = saveFileDialog1.FileName,
                        UseShellExecute = true
                    };
                    Process.Start(info);
                }
            }
        }

        private void überPacioliToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string version = VersionInformation.GetVersion();
            var repo = new PacioliRepository();

            MessageBox.Show(string.Format(Resources.msgInfoText, version, repo.VersionName, repo.DownloadUrl));
        }

        private void einstellungenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var settingsForm = new AppSettingsForm();
            settingsForm.ShowDialog();
        }

        private void aufNeueVersionPrüfenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var check = new UpdateCheck();
            check.Execute(false);
        }

        private void MainForm_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data!.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.All;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        private void MainForm_DragDrop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data!.GetData(DataFormats.FileDrop, false);
            StringBuilder sb = new StringBuilder();
            foreach (var fn in files)
            {
                AddFile(fn);
            }
        }

        private void MainForm_SizeChanged(object sender, EventArgs e)
        {
            var currentPage = F_TabControl.SelectedTab;
            if (currentPage == null)
            {
                return;
            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            foreach (TabPage tp in F_TabControl.Controls)
            {
                foreach (var rp in tp.Controls)
                {
                    if (rp.GetType() == typeof(RecordPanel))
                    {
                        ((RecordPanel)rp).Cleanup();
                    }
                }
            }
        }
    }
}

