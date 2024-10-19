using Pacioli.Preview.ImageGeneration;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Pacioli.WindowsApp.NET8.Controls
{
    public partial class DocViewPanel : UserControl
    {
        Converter? converter = null;
        int pageNumber = 0;
        Image? loadedImage = null;

        public DocViewPanel(string title)
        {
            InitializeComponent();

            F_TopLabel.Text = title;
            F_Left.Text = "\u25C0";
            F_Right.Text = "\u25B6";
        }

        public PictureBox FF_Picture
        {
            get
            {
                return F_PictureBox;
            }
            set
            {
                F_PictureBox = value;
            }
        }

        public Label FF_TopLabel
        {
            get
            {
                return F_TopLabel;
            }
            set
            {
                F_TopLabel = value;
            }
        }

        public void SetDocument(string fileName)
        {
            converter = new Converter(fileName);
            pageNumber = 0;
            UpdateImage();
        }

        private void UpdateImage()
        {
            if(converter == null)
            {
                return;
            }
            using (var imgStream = converter.ConvertToStream(ref pageNumber))
            {
                loadedImage = Image.FromStream(imgStream);
                imgStream.Close();
            }

            F_PictureBox.Image = loadedImage;
            F_PageNo.Text = (pageNumber + 1).ToString();
        }

        private void F_Left_Click(object sender, EventArgs e)
        {
            pageNumber--;
            UpdateImage();
        }

        private void F_Right_Click(object sender, EventArgs e)
        {
            pageNumber++;
            UpdateImage();
        }
    }
}
