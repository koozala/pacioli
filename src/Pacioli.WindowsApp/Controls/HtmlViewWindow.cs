using Microsoft.Web.WebView2.Core;
using Microsoft.Web.WebView2.WinForms;

namespace Pacioli.WindowsApp.Controls
{
    /*
     * A window that shows one HTML page in a WebView2 browser.
     *
     * The page can hold text of the document or of an external service, thus the browser
     * runs no script and opens no other page. Escape closes the window.
     *
     * The validation result and the document information use this window.
     */
    public class HtmlViewWindow : Form
    {
        readonly WebView2 browser = new WebView2();
        readonly string html;
        bool contentShown;

        public HtmlViewWindow(string title, string html)
        {
            this.html = html;

            Text = title;
            Icon = Application.OpenForms.Count > 0 ? Application.OpenForms[0]!.Icon : Icon;
            StartPosition = FormStartPosition.CenterParent;
            ShowInTaskbar = false;
            Size = new Size(LogicalToDeviceUnits(1000), LogicalToDeviceUnits(760));
            KeyPreview = true;

            browser.Dock = DockStyle.Fill;
            Controls.Add(browser);

            Load += HtmlViewWindow_Load;
        }

        async void HtmlViewWindow_Load(object? sender, EventArgs e)
        {
            try
            {
                /* The same folder as DocViewPanel, because one folder takes one set of options. */
                string userDataFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                CoreWebView2Environment environment = await CoreWebView2Environment.CreateAsync(userDataFolder: userDataFolder);
                await browser.EnsureCoreWebView2Async(environment);
            }
            catch (Exception ex)
            {
                if (!IsDisposed)
                {
                    MessageBox.Show(this, ex.Message, Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Close();
                }
                return;
            }

            if (IsDisposed)
            {
                return;
            }

            CoreWebView2Settings settings = browser.CoreWebView2.Settings;
            settings.IsScriptEnabled = false;
            settings.IsWebMessageEnabled = false;
            settings.AreDevToolsEnabled = false;
            settings.IsStatusBarEnabled = false;

            browser.CoreWebView2.NavigationStarting += (s, args) =>
            {
                /* The first navigation shows the page. Each other one stays blocked. */
                if (contentShown)
                {
                    args.Cancel = true;
                }
                contentShown = true;
            };
            browser.CoreWebView2.NewWindowRequested += (s, args) => args.Handled = true;

            browser.CoreWebView2.NavigateToString(html);
        }

        /* Escape closes the window. The browser gives its keys to this method too. */
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Escape)
            {
                Close();
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
    }

    /*
     * One window of a document, for example the validation result.
     *
     * The document has at most one such window. A second Show brings the open window to
     * the front. Close closes the window, for example when the tab of the document closes.
     */
    public sealed class OwnedWindow
    {
        Form? form;

        public bool IsOpen => form != null && !form.IsDisposed;

        public void Show(Func<Form> create, IWin32Window? owner)
        {
            if (form != null && !form.IsDisposed)
            {
                if (form.WindowState == FormWindowState.Minimized)
                {
                    form.WindowState = FormWindowState.Normal;
                }
                form.Activate();
                return;
            }

            Form created = create();
            created.FormClosed += (sender, args) =>
            {
                if (ReferenceEquals(form, created))
                {
                    form = null;
                }
            };
            form = created;
            created.Show(owner);
        }

        public void Close()
        {
            if (form != null && !form.IsDisposed)
            {
                form.Close();
            }
            form = null;
        }
    }
}
