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
    public partial class WebViewPanel : UserControl
    {
        public WebViewPanel()
        {
            InitializeComponent();
            F_WebView.Size = new System.Drawing.Size(200, 200);
        }

        public void SetUrl(string url)
        {
            F_WebView.Source = new Uri(url);
            //F_WebView.CoreWebView2.Navigate(url);
        }
    }
}
