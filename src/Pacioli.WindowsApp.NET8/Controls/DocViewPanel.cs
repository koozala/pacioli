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
        public DocViewPanel(string title)
        {
            InitializeComponent();

            string userDataFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            var e = CoreWebView2Environment.CreateAsync(userDataFolder: userDataFolder).Result;
            var task = F_WebView.EnsureCoreWebView2Async(environment: e);

            F_TopLabel.Text = title;
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

        public void SetDocument(string fileName)
        {
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
