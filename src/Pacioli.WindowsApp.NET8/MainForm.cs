using Pacioli.Pdf.Invoice;
using Pacioli.Preview.ImageGeneration;
using Pacioli.WindowsApp.NET8.Util;

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
        }

        private void öffnenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog1.FileName = string.Empty;
            openFileDialog1.InitialDirectory = @"C:\wrk\repos\Rechnungsdruck_AS400_PDF\3rd party\ZUGFeRD-csharp\demodata\zugferd21";
            DialogResult dr = openFileDialog1.ShowDialog();

            if (dr == DialogResult.OK)
            {
                pdfPath = Path.GetTempFileName();
                string imgPath = Path.GetTempFileName();
                fileNameTb.Text = openFileDialog1.FileName;
                try
                {
                    Writer.Write(openFileDialog1.FileName, pdfPath);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Fehler beim Lesen der Datei");
                    return;
                }
                pageNumber = 0;
                converter = new Converter(pdfPath);
                UpdateImage();


                //converter.Convert(pdfPath, imgPath);
                //loadedImage = Image.FromFile(imgPath);
                //var resized = ImageResize.ResizeImage(loadedImage, pictureBox.Width, pictureBox.Height);
                //pictureBox.Image = resized;
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
                string imgPath = Path.GetTempFileName();
                pageNumber = converter.Convert(pdfPath!, imgPath, pageNumber);
                loadedImage = Image.FromFile(imgPath);

                float r = converter.PageSize.Width / converter.PageSize.Height;
                int width = (int)(r * pictureBox.Height);

                var resized = ImageResize.ResizeImage(loadedImage, width, pictureBox.Height);
                pictureBox.SizeMode = PictureBoxSizeMode.CenterImage;
                pictureBox.Image = resized;
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

        private void MainForm_SizeChanged(object sender, EventArgs e)
        {
            UpdateImage();
        }

        private void überPacioliToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Pacioli verarbeitet elektronische Rechnungen und transformiert sie in PDF-Dateien.\n\nMehr Informationen im Internet: https://github.com/koozala/pacioli", "Pacioli");
        }
    }
}
