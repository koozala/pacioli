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
using static iText.IO.Util.IntHashtable;

namespace Pacioli.WindowsApp.NET8.Controls
{
    public partial class DocViewPanel : UserControl
    {
        Converter? converter = null;
        int pageNumber = 0;
        Image? loadedImage = null;

        int deltaCount = 0;
        double zoom = 1.0;
        double centerX = 0.5;
        double centerY = 0.5;
        bool mouseDown = false;
        int mouseStartX = 0;
        int mouseStartY = 0;


        public DocViewPanel(string title)
        {
            InitializeComponent();

            F_TopLabel.Text = title;
            F_Left.Text = "\u25C0";
            F_Right.Text = "\u25B6";

            F_PictureBox.MouseWheel += F_PictureBox_MouseWheel;
            F_PictureBox.MouseDown += F_PictureBox_MouseDown;
            F_PictureBox.MouseUp += F_PictureBox_MouseUp;
            F_PictureBox.MouseMove += F_PictureBox_MouseMove;
        }

        public void Reset()
        {
            deltaCount = 0;
            zoom = 1.0;
            centerX = 0.5;
            centerY = 0.5;
            mouseDown = false;
        }

        private void F_PictureBox_MouseMove(object? sender, MouseEventArgs e)
        {
            if (mouseDown)
            {
                int deltaX = e.X - mouseStartX;
                int deltaY = e.Y - mouseStartY;

                mouseStartX = e.X;
                mouseStartY = e.Y;

                double shiftX = deltaX / (F_PictureBox.Width * zoom);
                double shiftY = deltaY / (F_PictureBox.Height * zoom);

                centerX = Clamp(centerX - shiftX, 0.0, 1.0);
                centerY = Clamp(centerY - shiftY, 0.0, 1.0);

                UpdateImage();

                //F_Info.Text = $"mouse delta={deltaX}x{deltaY} c={centerX}x{centerY}";
            }
        }

        private double Clamp(double x, double low, double high)
        {
            if (x < low) return low;
            if (x > high) return high;
            return x;
        }

        private void F_PictureBox_MouseUp(object? sender, MouseEventArgs e)
        {
            mouseDown = false;
        }

        private void F_PictureBox_MouseDown(object? sender, MouseEventArgs e)
        {
            mouseDown = true;
            mouseStartX = e.X;
            mouseStartY = e.Y;
        }


        private void F_PictureBox_MouseWheel(object? sender, MouseEventArgs e)
        {
            deltaCount += e.Delta;
            if (deltaCount < 0) deltaCount = 0;
            zoom = Math.Pow(1.0 + deltaCount / 1000.0, 2);
            UpdateImage();
        }


        public TableLayoutPanel FF_TitlePanel
        {
            get
            {
                return F_TitlePanel;
            }
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
            Reset();
            UpdateImage();
        }

        private Image ScaleImage(Image sourceImage)
        {
            int showWidth = (int)(sourceImage.Width / zoom);
            int showHeight = (int)(sourceImage.Height / zoom);

            int left = (int)(sourceImage.Width * centerX - showWidth / 2.0);
            int top = (int)(sourceImage.Height * centerY - showHeight / 2.0);

            if (left > sourceImage.Width - showWidth) left = sourceImage.Width - showWidth;
            if (top > sourceImage.Height - showHeight) top = sourceImage.Height - showHeight;
            if (left < 0) left = 0;
            if (top < 0) top = 0;

            // re-align center
            centerX = (1.0 * left + showWidth / 2.0) / sourceImage.Width;
            centerY = (1.0 * top + showHeight / 2.0) / sourceImage.Height;

            Rectangle r = new Rectangle(0, 0, F_PictureBox.Width, F_PictureBox.Height);
            Bitmap m = new Bitmap(F_PictureBox.Width, F_PictureBox.Height);
            Graphics g = Graphics.FromImage(m);
            g.DrawImage(sourceImage, r, left, top, showWidth, showHeight, GraphicsUnit.Pixel);
            return m;
        }

        private void UpdateImage()
        {
            if (converter == null)
            {
                return;
            }
            //using (var imgStream = converter.ConvertToStream(ref pageNumber))
            //{
            //    loadedImage = Image.FromStream(imgStream);
            //    imgStream.Close();
            //}
            loadedImage = converter.GetPage(ref pageNumber);

            F_PictureBox.Image = ScaleImage(loadedImage);
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
