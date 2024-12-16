using Microsoft.Web.WebView2.Core;
using Microsoft.Web.WebView2.WinForms;
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
            try
            {
                InitializeComponent();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }

            string userDataFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            var e = CoreWebView2Environment.CreateAsync(userDataFolder: userDataFolder).Result;
            var task = F_WebView.EnsureCoreWebView2Async(environment: e);

            F_TopLabel.Text = title;
        }

        public void Reset()
        {
            deltaCount = 0;
            zoom = 1.0;
            centerX = 0.5;
            centerY = 0.5;
            mouseDown = false;
        }

        private double Clamp(double x, double low, double high)
        {
            if (x < low) return low;
            if (x > high) return high;
            return x;
        }

        public TableLayoutPanel FF_TitlePanel
        {
            get
            {
                return F_TitlePanel;
            }
        }

        public WebView2 FF_WebView
        {
            get
            {
                return F_WebView;
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
            FF_WebView.Source = new Uri(fileName);
        }

        public void Cleanup()
        {
            if (FF_WebView.CoreWebView2 != null)
            {
                var profile = FF_WebView.CoreWebView2.Profile;
                var task = profile.ClearBrowsingDataAsync(CoreWebView2BrowsingDataKinds.AllProfile);
            }
        }

    }
}
