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

        public DocViewerForm(string fileName)
        {
            InitializeComponent();
            Converter = new Converter(fileName);
            pageNumber = 0;
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
                F_PictureBox.Image = loadedImage;

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
