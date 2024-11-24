using Pacioli.Preview.ImageGeneration;
using Pacioli.WindowsApp.NET8.Util;
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
    public partial class DocViewerForm : Form
    {
        public Converter Converter { get; set; }
        int pageNumber;
        Image? loadedImage = null;

        int ZoomLevel = 1;
        int OffsetX = 0;
        int OffsetY = 0;

        public DocViewerForm()
        {
            InitializeComponent();
            pageNumber = 0;
            F_PictureBox.MouseWheel += F_PictureBox_MouseWheel;
        }

        private void F_PictureBox_MouseWheel(object? sender, MouseEventArgs e)
        {
            ZoomLevel += e.Delta;
            UpdateImage();
        }

        Image ReworkImage(Image source)
        {
            double factor = 1.0 + ZoomLevel / 10f;
            Bitmap newBm = new Bitmap(source.Width, source.Height);
            Graphics g = Graphics.FromImage(newBm);

            Bitmap bmp2 = new Bitmap(source, new Size((int)(source.Width * factor), (int)(source.Height * factor)));
            g.DrawImage(bmp2, 0, 0);

            return (Image)newBm;
        }

        public void SetFile(string fileName)
        {
            Converter = new Converter(fileName);
            UpdateImage();
        }

        private void UpdateImage()
        {
            if (Converter != null)
            {
                using (var imgStream = Converter.ConvertToStream(ref pageNumber))
                {
                    loadedImage = Image.FromStream(imgStream);
                    imgStream.Close();
                }

                F_PictureBox.SizeMode = PictureBoxSizeMode.Zoom;
                F_PictureBox.Image = ReworkImage(loadedImage);

                F_PageNo.Text = (pageNumber + 1).ToString();
            }
        }

        private void F_PageUp_Click(object sender, EventArgs e)
        {
            pageNumber++;
            UpdateImage();
        }

        private void F_PageDown_Click(object sender, EventArgs e)
        {
            pageNumber--;
            UpdateImage();
        }
    }
}
