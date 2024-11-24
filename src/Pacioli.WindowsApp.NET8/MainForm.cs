using iText.Commons.Utils;
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
using System.Windows.Forms;

namespace Pacioli.WindowsApp.NET8
{
    public partial class MainForm : Form
    {
        string? pdfPath = null;
        int pageNumber = 0;
        Converter? converter = null;
        Converter? original = null;
        DocViewerForm docViewer;

        DocViewPanel docPanelDerived;
        DocViewPanel docPanelOriginal;

        ConfigDb cdb;

        public MainForm()
        {
            InitializeComponent();

            docPanelDerived = new DocViewPanel("E-Rechnung");
            docPanelOriginal = new DocViewPanel("ZUGFeRD PDF");
            docPanelOriginal.FF_TitlePanel.BackColor = Color.Lavender;

            tableLayoutPanel1.Controls.Add(docPanelDerived);
            tableLayoutPanel1.SetCellPosition(docPanelDerived, new TableLayoutPanelCellPosition(1, 1));
            docPanelDerived.Dock = DockStyle.Fill;
            tableLayoutPanel1.Controls.Add(docPanelOriginal);
            tableLayoutPanel1.SetCellPosition(docPanelOriginal, new TableLayoutPanelCellPosition(2, 1));
            docPanelOriginal.Dock = DockStyle.Fill;

            cdb = new ConfigDb();
            UserPreferences preferences = cdb.ReadPreferences();
            preferences.SetCulture();
        }

        private void öffnenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ConfigDb cdb = new ConfigDb();
            UserPreferences preferences = cdb.ReadPreferences();

            openFileDialog1.FileName = string.Empty;
            openFileDialog1.InitialDirectory = preferences.DefaultFolder;
            DialogResult dr = openFileDialog1.ShowDialog();

            if (dr == DialogResult.OK)
            {
                pdfPath = Path.GetTempFileName();
                fileNameTb.Text = openFileDialog1.FileName;
                preferences.DefaultFolder = Path.GetDirectoryName(openFileDialog1.FileName)!;
                cdb.SavePreferences(preferences);

                original = null;

                try
                {
                    /* Convert to PDF and write to temp file */
                    Writer writer = new Writer(openFileDialog1.FileName, preferences.AttachmentOutputFolder);
                    writer.Write(pdfPath);
                    int countAttachments = writer.GetAttachmentCount();
                    string aInfo = Resources.attachmentsNone;
                    if (countAttachments > 0)
                    {
                        aInfo = string.Format(Resources.attachmentsTxt, countAttachments);
                    }
                    attachmentInfoLbl.Text = aInfo;

                    docPanelDerived.SetDocument(pdfPath);

                    if (writer.IsZugferd)
                    {
                        //if (docViewer == null || docViewer.IsDisposed)
                        //{
                        //    docViewer = new DocViewerForm();
                        //}
                        //docViewer.SetFile(openFileDialog1.FileName);
                        //docViewer.Show();
                        //docViewer.Top = this.Top;
                        //docViewer.Left = this.Left + this.Width;
                        //docViewer.Height = this.Height;
                        //docViewer.Width = this.Width;

                        original = new Converter(openFileDialog1.FileName);
                        docPanelOriginal.SetDocument(openFileDialog1.FileName);
                    }
                    else if (docViewer != null && !docViewer.IsDisposed)
                    {
                        docViewer.Close();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Fehler beim Lesen der Datei: {ex.Message}");
                    return;
                }
                pageNumber = 0;
                converter = new Converter(pdfPath);
                UpdateImage();
            }
        }

        private void beendenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void UpdateImage()
        {
            if (converter != null)
            {
                docPanelDerived.Visible = true;
                docPanelDerived.FF_Picture.SizeMode = PictureBoxSizeMode.Zoom;
            }

            if (original != null)
            {
                docPanelOriginal.Visible = true;
                docPanelOriginal.FF_Picture.SizeMode = PictureBoxSizeMode.Zoom;
            }
            else
            {
                //pbOriginal.Image = null;
                docPanelOriginal.Visible = false;
            }
        }


        private void speichernToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (converter == null)
            {
                MessageBox.Show("Kein Dokument geladen");
                return;
            }

            var pref = cdb.ReadPreferences();

            saveFileDialog1.InitialDirectory = pref.AttachmentOutputFolder;
            saveFileDialog1.FileName = Path.GetFileNameWithoutExtension(fileNameTb.Text) + ".pdf";
            var result = saveFileDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                pref.AttachmentOutputFolder = Path.GetDirectoryName(saveFileDialog1.FileName)!;
                cdb.SavePreferences(pref);

                File.WriteAllBytes(saveFileDialog1.FileName, converter.PdfData!);

                if (pref.OpenAfterSave)
                {
                    ProcessStartInfo info = new ProcessStartInfo
                    {
                        FileName = saveFileDialog1.FileName,
                        UseShellExecute = true
                    };
                    Process.Start(info);
                    //Process.Start(saveFileDialog1.FileName);
                }
            }
        }

        private void überPacioliToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string version = VersionInformation.GetVersion();
            var repo = new PacioliRepository();

            MessageBox.Show($"Pacioli verarbeitet elektronische Rechnungen und transformiert sie in PDF-Dateien.\nVersion: {version}\nVerfügbare Version: {repo.VersionName}\n{repo.DownloadUrl}\n\nMehr Informationen im Internet: https://github.com/koozala/pacioli", "Pacioli");
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
    }
}
