using Pacioli.Language.Resources;
using Pacioli.Pdf.Invoice;
using Pacioli.Preview.ImageGeneration;
using Pacioli.WindowsApp.NET8.Config;
using Pacioli.WindowsApp.NET8.Util;
using System.Globalization;
using System.Windows.Forms;

namespace Pacioli.WindowsApp.NET8
{
    public partial class MainForm : Form
    {
        Image? loadedImage = null;
        string? pdfPath = null;
        int pageNumber = 0;
        Converter? converter = null;

        public MainForm()
        {
            InitializeComponent();
            button1.Text = "\u25C0";
            button2.Text = "\u25B6";

            ConfigDb cdb = new ConfigDb();
            UserPreferences preferences = cdb.ReadPreferences();
            CultureInfo.CurrentCulture = new CultureInfo(preferences.LanguageCode);
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
                string imgPath = Path.GetTempFileName();
                fileNameTb.Text = openFileDialog1.FileName;
                preferences.DefaultFolder = Path.GetDirectoryName(openFileDialog1.FileName)!;
                cdb.SavePreferences(preferences);
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

                    if (writer.IsZugferd)
                    {
                        DocViewerForm dcf = new DocViewerForm(openFileDialog1.FileName);
                        dcf.Show();
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

        private void button1_Click(object sender, EventArgs e)
        {
            pageNumber--;
            UpdateImage();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            pageNumber++;
            UpdateImage();
        }

        private void UpdateImage()
        {
            if (converter != null)
            {
                using (var imgStream = converter.ConvertToStream(ref pageNumber))
                {
                    loadedImage = Image.FromStream(imgStream);
                    imgStream.Close();
                }

                pictureBox.SizeMode = PictureBoxSizeMode.Zoom;
                pictureBox.Image = loadedImage;

                pageNoTb.Text = (pageNumber + 1).ToString();
            }
        }


        private void speichernToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (converter == null)
            {
                MessageBox.Show("Kein Dokument geladen");
                return;
            }

            saveFileDialog1.InitialDirectory = openFileDialog1.InitialDirectory;
            saveFileDialog1.FileName = Path.GetFileNameWithoutExtension(fileNameTb.Text) + ".pdf";
            var result = saveFileDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                File.WriteAllBytes(saveFileDialog1.FileName, converter.PdfData!);
            }
        }

        private void überPacioliToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Pacioli verarbeitet elektronische Rechnungen und transformiert sie in PDF-Dateien.\n\nMehr Informationen im Internet: https://github.com/koozala/pacioli", "Pacioli");
        }

        private void einstellungenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var settingsForm = new AppSettingsForm();
            settingsForm.ShowDialog();
        }
    }
}
