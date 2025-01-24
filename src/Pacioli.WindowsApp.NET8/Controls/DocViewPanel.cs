using Microsoft.Web.WebView2.Core;
using Microsoft.Web.WebView2.WinForms;
using Pacioli.Language.Resources;
using Pacioli.Pdf.Invoice;
using Pacioli.Preview.ImageGeneration;
using Pacioli.WindowsApp.NET8.Render;
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
        private bool IsSimpleView = false;
        private SimpleHtmlRenderer Simple = new SimpleHtmlRenderer();
        private PdfRenderer Pdf = new PdfRenderer();
        private string OriginalFileName;
        private string AttachmentFolder;
        private bool ShowOriginal;

        public DocViewPanel(string title, string fileName, string attachmentFolder, bool showOriginal)
        {
            InitializeComponent();

            OriginalFileName = fileName;
            AttachmentFolder = attachmentFolder;
            ShowOriginal = showOriginal;

            string userDataFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            var e = CoreWebView2Environment.CreateAsync(userDataFolder: userDataFolder).Result;
            var task = F_WebView.EnsureCoreWebView2Async(environment: e);

            F_TopLabel.Text = title;
            F_SwitchView.Text = "Simple View";
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

        public Button FF_SwitchView
        {
            get
            {
                return F_SwitchView;
            }
        }

        public void Cleanup()
        {
            if (FF_WebView.CoreWebView2 != null)
            {
                var profile = FF_WebView.CoreWebView2.Profile;
                var task = profile.ClearBrowsingDataAsync(CoreWebView2BrowsingDataKinds.AllProfile);
            }
        }

        public void Render()
        {
            if (ShowOriginal)
            {
                FF_WebView.Source = new Uri(OriginalFileName);
            }
            else
            {
                if (IsSimpleView)
                {
                    F_SwitchView.Text = "PDF View";
                    //string tempFile = Path.GetTempFileName();
                    //Simple.Render(OriginalFileName);
                    //FF_WebView.Source = new Uri(Simple.GetHtmlFileName());
                    FF_WebView.Source = new Uri(Pdf.GetHtmlPath());
                }
                else
                {
                    F_SwitchView.Text = "Simple View";
                    var pdfPath = Path.GetTempFileName();
                    Pdf.Render(OriginalFileName, AttachmentFolder);
                    FF_WebView.Source = new Uri(Pdf.GetPdfPath());
                }
            }
        }

        private void F_SwitchView_Click(object sender, EventArgs e)
        {
            IsSimpleView = !IsSimpleView;
            Render();
        }
    }
}
