using iText.Commons.Utils;
using Microsoft.Web.WebView2.Core;
using Pacioli.Config.Persistence;
using Pacioli.Language.Resources;
using Pacioli.Pdf.Invoice;
using Pacioli.Preview.ImageGeneration;
using Pacioli.Updater.AutoUpdate;
using Pacioli.WindowsApp.Controls;
using Pacioli.WindowsApp.Util;
using Pacioli.WindowsApp.Validation;
using System.Diagnostics;
using System.Drawing.Printing;
using System.Globalization;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Windows.Forms;

namespace Pacioli.WindowsApp
{
    public partial class MainForm : Form
    {
        /*
         * The preferences of the user. The field held no value before, because each
         * handler made a ConfigDb of its own in a local variable with the same name. The
         * save of a PDF file then went to the field, and it ended with a
         * NullReferenceException.
         */
        readonly ConfigDb cdb = new ConfigDb();

        UserPreferences preferences = UserPreferences.LoadPreferences();

        /*
         * The place of the tab control in the table, and the two things that it holds: the
         * tab control with the open documents, and the logo of the start screen. Only one
         * of the two is visible.
         */
        private readonly Panel contentHost = new Panel();
        private readonly LogoPanel logoPanel = new LogoPanel();

        public MainForm()
        {
            InitializeComponent();
            InitializePacioli();

            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Refresh();

            hinweisFeld2.Dock = DockStyle.Fill;
            hinweisFeld2.Refresh();

            this.Refresh();
        }

        public MainForm(string[] args)
        {
            InitializeComponent();
            InitializePacioli();

            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Refresh();

            hinweisFeld2.Dock = DockStyle.Fill;
            hinweisFeld2.Refresh();

            this.Refresh();

            foreach (var file in args)
            {
                AddFile(file);
            }
        }

        private void InitializePacioli()
        {
            this.WindowState = FormWindowState.Maximized;

            F_TabControl.Controls.Clear();
            F_TabControl.AllowDrop = true;
            F_TabControl.DragEnter += MainForm_DragEnter;
            F_TabControl.DragDrop += MainForm_DragDrop;

            /* The cross at the right side of each tab header closes that document. */
            TabCloseIcon closeIcons = new TabCloseIcon(F_TabControl);
            closeIcons.CloseRequested += (sender, page) => CloseDocument(page);

            InitializeStartScreen();
            InitializeValidationMenu();
            InitializeDocumentInformationMenu();
        }

        private readonly ToolStripMenuItem documentInformationMenuItem = new ToolStripMenuItem();

        /*
         * "Datei > Dokumentinformationen" (Ctrl+I) shows the meta information of the
         * document in the selected tab: the declared standard and profile, the XML, the
         * file, and for a PDF file the XMP metadata.
         *
         * The entry stays enabled, as "Speichern" does, because a disabled entry does not
         * take its shortcut. Without a document, the entry gives a message.
         */
        private void InitializeDocumentInformationMenu()
        {
            documentInformationMenuItem.Text = Resources.menuDokumentinformationen;
            documentInformationMenuItem.ShortcutKeys = Keys.Control | Keys.I;
            documentInformationMenuItem.Click += (sender, e) =>
            {
                RecordPanel? rp = SelectedRecordPanel();
                if (rp == null)
                {
                    MessageBox.Show(Resources.msgNoActiveDocument);
                    return;
                }
                rp.ShowDocumentInformation();
            };

            int position = dateiToolStripMenuItem.DropDownItems.IndexOf(speichernToolStripMenuItem) + 1;
            dateiToolStripMenuItem.DropDownItems.Insert(position, documentInformationMenuItem);
        }

        private readonly ToolStripMenuItem validationMenuItem = new ToolStripMenuItem();
        private readonly ToolStripMenuItem validateOnlineMenuItem = new ToolStripMenuItem();
        private readonly ToolStripMenuItem showValidationMenuItem = new ToolStripMenuItem();
        private readonly ToolStripMenuItem apiKeyMenuItem = new ToolStripMenuItem();

        /*
         * The menu of the online validation (W System), between "Datei" and the menu of
         * the program. The program makes the menu in code, because its texts come from
         * the resources of Pacioli.Language, as the texts of RecordPanel do.
         *
         * Each entry works on the document of the selected tab. The menu sets the state
         * of its entries each time it opens.
         */
        private void InitializeValidationMenu()
        {
            validationMenuItem.Text = Resources.menuPruefung;

            validateOnlineMenuItem.Text = Resources.menuPruefenOnline;
            validateOnlineMenuItem.Click += async (sender, e) =>
            {
                RecordPanel? rp = SelectedRecordPanel();
                if (rp != null)
                {
                    await rp.ValidateOnlineAsync();
                }
            };

            showValidationMenuItem.Text = Resources.menuPruefergebnisAnzeigen;
            showValidationMenuItem.Click += (sender, e) => SelectedRecordPanel()?.ShowValidationResult();

            apiKeyMenuItem.Text = Resources.menuApiKey;
            apiKeyMenuItem.Click += (sender, e) => ApiKeyForm.Edit(this);

            validationMenuItem.DropDownItems.AddRange(new ToolStripItem[]
            {
                validateOnlineMenuItem,
                showValidationMenuItem,
                new ToolStripSeparator(),
                apiKeyMenuItem
            });

            validationMenuItem.DropDownOpening += (sender, e) =>
            {
                RecordPanel? rp = SelectedRecordPanel();
                validateOnlineMenuItem.Enabled = rp != null && !rp.IsValidating;
                showValidationMenuItem.Enabled = rp?.LastValidation != null;
            };

            menuStrip1.Items.Insert(menuStrip1.Items.IndexOf(aboutToolStripMenuItem), validationMenuItem);
        }

        /* The panel of the document in the selected tab, or null if no document is open. */
        private RecordPanel? SelectedRecordPanel()
        {
            return F_TabControl.SelectedTab?.Controls.OfType<RecordPanel>().FirstOrDefault();
        }

        /*
         * Puts the logo on the screen that the user sees at the start.
         *
         * The tab control holds no page before the user opens the first document, thus the
         * screen is empty. A host panel takes the place of the tab control in the table,
         * and holds both the tab control and the logo. The application shows the logo while
         * no document is open, and the tab control after that.
         *
         * The logo takes the drag and drop of the tab control, because the user drops a
         * document on the start screen too.
         */
        private void InitializeStartScreen()
        {
            TableLayoutPanelCellPosition cell = tableLayoutPanel1.GetCellPosition(F_TabControl);

            contentHost.Dock = DockStyle.Fill;
            contentHost.Margin = F_TabControl.Margin;

            tableLayoutPanel1.Controls.Remove(F_TabControl);

            F_TabControl.Dock = DockStyle.Fill;
            logoPanel.Dock = DockStyle.Fill;

            logoPanel.AllowDrop = true;
            logoPanel.DragEnter += MainForm_DragEnter;
            logoPanel.DragDrop += MainForm_DragDrop;

            contentHost.Controls.Add(F_TabControl);
            contentHost.Controls.Add(logoPanel);

            tableLayoutPanel1.Controls.Add(contentHost, cell.Column, cell.Row);

            F_TabControl.ControlAdded += (sender, e) => UpdateStartScreen();

            /*
             * At the moment of ControlRemoved, the tab control still counts the page that
             * goes away. Thus the check waits for the end of the removal.
             */
            F_TabControl.ControlRemoved += (sender, e) => BeginInvoke(new Action(UpdateStartScreen));

            UpdateStartScreen();
        }

        /* Shows the logo while no document is open, and the documents after that. */
        private void UpdateStartScreen()
        {
            bool empty = F_TabControl.TabCount == 0;

            logoPanel.Visible = empty;
            F_TabControl.Visible = !empty;
        }

        private void öffnenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UserPreferences preferences = cdb.ReadPreferences();

            openFileDialog1.FileName = string.Empty;
            openFileDialog1.InitialDirectory = preferences.DefaultFolder;
            openFileDialog1.Multiselect = true;
            DialogResult dr = openFileDialog1.ShowDialog();

            if (dr == DialogResult.OK)
            {
                foreach (var fileName in openFileDialog1.FileNames)
                {
                    AddFile(fileName);
                }
            }

            var path = Path.GetDirectoryName(openFileDialog1.FileName);
            if (path != null)
            {
                /* The folder of the last open is a comfort. See the save of the PDF file. */
                preferences.DefaultFolder = path;
                cdb.TrySavePreferences(preferences);
            }
        }

        private void AddFile(string fileName)
        {
            RecordPanel rp = new RecordPanel();
            try
            {
                rp.SetFile(fileName, preferences.AttachmentOutputFolder);
                TabPage page = new TabPage();
                page.Text = Path.GetFileName(fileName);

                /* Add RecordPanel to TabPage */

                rp.Dock = DockStyle.Fill;
                page.Controls.Add(rp);
                F_TabControl.Controls.Add(page);
                F_TabControl.SelectedTab = page;

                rp.CloseRequested += (sender, args) => CloseDocument(page);

                rp.AllowDrop = true;
                rp.DragEnter += MainForm_DragEnter;
                rp.DragDrop += MainForm_DragDrop;

                rp.FF_TablePanel.AllowDrop = true;
                rp.FF_TablePanel.DragEnter += MainForm_DragEnter;
                rp.FF_TablePanel.DragDrop += MainForm_DragDrop;

                rp.DocPanelDerived.AllowDrop = true;
                rp.DocPanelDerived.DragEnter += MainForm_DragEnter;
                rp.DocPanelDerived.DragDrop += MainForm_DragDrop;

                rp.DocPanelOriginal.AllowDrop = true;
                rp.DocPanelOriginal.DragEnter += MainForm_DragEnter;
                rp.DocPanelOriginal.DragDrop -= MainForm_DragDrop;

                rp.FF_DragDropPanel.AllowDrop = true;
                rp.FF_DragDropPanel.DragEnter += MainForm_DragEnter;
                rp.FF_DragDropPanel.DragDrop += MainForm_DragDrop;
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format(Resources.errorFileReadMessage, fileName, ex.Message));
                return;
            }
        }

        /*
         * Closes one document. The other documents stay open.
         *
         * The panel of the document gives its files and its browser free before the page
         * goes away, because the application keeps a rendered document in a file of the
         * temporary folder. The tab control then shows the page at the side, and the start
         * screen comes back if this page was the last one.
         */
        private void CloseDocument(TabPage page)
        {
            string? temporaryPdf = null;

            foreach (Control control in page.Controls)
            {
                if (control is RecordPanel rp)
                {
                    temporaryPdf = rp.PdfPath;
                    rp.Cleanup();
                }
            }

            F_TabControl.Controls.Remove(page);
            page.Dispose();

            DeleteTemporaryFile(temporaryPdf);
        }

        /*
         * The keyboard shortcuts of the main window.
         *
         * Ctrl+W closes the document that the user sees. The form gets the key also if the
         * focus is in the browser of the PDF, because the WebView2 control gives its
         * accelerator keys to ProcessCmdKey of its parents.
         */
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.Control | Keys.W))
            {
                CloseSelectedDocument();
                return true;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        /*
         * Closes the document of the selected tab. If no document is open, nothing occurs.
         *
         * The close waits for the end of the key event. The event can come from the browser
         * of the document, and the close removes that browser.
         */
        private void CloseSelectedDocument()
        {
            TabPage? page = F_TabControl.SelectedTab;
            if (page == null)
            {
                return;
            }

            BeginInvoke(new Action(() =>
            {
                if (!page.IsDisposed && F_TabControl.TabPages.Contains(page))
                {
                    CloseDocument(page);
                }
            }));
        }

        /*
         * Gives the temporary file of a rendered document free.
         *
         * The application writes the PDF of each document into a file of the temporary
         * folder. The browser of the panel can still hold that file at this moment, thus a
         * failure is of no importance.
         */
        private static void DeleteTemporaryFile(string? path)
        {
            if (string.IsNullOrEmpty(path))
            {
                return;
            }

            try
            {
                File.Delete(path);
            }
            catch (IOException)
            {
            }
            catch (UnauthorizedAccessException)
            {
            }
        }

        private void beendenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }


        private void speichernToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var currentPage = F_TabControl.SelectedTab;

            if (currentPage == null)
            {
                MessageBox.Show(Resources.msgNoActiveDocument);
                return;
            }

            RecordPanel rp = (RecordPanel)currentPage.Controls[0];

            if (rp.RenderedDoc == null)
            {
                MessageBox.Show(Resources.msgNoDocumentLoaded);
                return;
            }

            var pref = cdb.ReadPreferences();

            saveFileDialog1.InitialDirectory = pref.AttachmentOutputFolder;
            saveFileDialog1.FileName = Path.GetFileNameWithoutExtension(rp.FF_FileNameTb.Text) + ".pdf";
            var result = saveFileDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                /*
                 * The file comes first. The folder of the last save is a comfort, thus a
                 * database that does not answer must not take the PDF file with it.
                 */
                File.WriteAllBytes(saveFileDialog1.FileName, rp.RenderedDoc.PdfData!);

                string? folder = Path.GetDirectoryName(saveFileDialog1.FileName);
                if (folder != null)
                {
                    pref.AttachmentOutputFolder = folder;
                    cdb.TrySavePreferences(pref);
                }

                if (pref.OpenAfterSave)
                {
                    ProcessStartInfo info = new ProcessStartInfo
                    {
                        FileName = saveFileDialog1.FileName,
                        UseShellExecute = true
                    };
                    Process.Start(info);
                }
            }
        }

        private async void überPacioliToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string version = VersionInformation.GetVersion();
            var repo = await PacioliRepository.CreateAsync();

            /* A server without an answer names no version, thus the message says so. */
            string available = repo.HasVersion ? repo.VersionName : Resources.VersionUnbekannt;

            MessageBox.Show(string.Format(Resources.msgInfoText, version, available, repo.DownloadUrl));
        }

        private void einstellungenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var settingsForm = new AppSettingsForm();
            settingsForm.ShowDialog();
        }

        private async void aufNeueVersionPrüfenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var check = await UpdateCheck.CreateAsync();
            await check.ExecuteAsync(false);
        }

        private void MainForm_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data!.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.All;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        private void MainForm_DragDrop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data!.GetData(DataFormats.FileDrop, false);
            StringBuilder sb = new StringBuilder();
            foreach (var fn in files)
            {
                AddFile(fn);
            }
        }

        private void MainForm_SizeChanged(object sender, EventArgs e)
        {
            var currentPage = F_TabControl.SelectedTab;
            if (currentPage == null)
            {
                return;
            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            foreach (TabPage tp in F_TabControl.Controls)
            {
                foreach (var rp in tp.Controls)
                {
                    if (rp.GetType() == typeof(RecordPanel))
                    {
                        ((RecordPanel)rp).Cleanup();
                    }
                }
            }
        }

        private void applikationBewertenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var ratingForm = new RatingForm();
            ratingForm.ShowDialog();
        }

        private void hinweisFeld2_Load(object sender, EventArgs e)
        {
        }
    }
}

