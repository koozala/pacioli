using Pacioli.Language.Resources;
using Pacioli.Pdf.ERechnung;
using Pacioli.Pdf.Invoice;
using Pacioli.Preview.ImageGeneration;
using Pacioli.WindowsApp.DocumentInfo;
using Pacioli.WindowsApp.Util;
using Pacioli.WindowsApp.Validation;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace Pacioli.WindowsApp.Controls
{
    public partial class RecordPanel : UserControl
    {
        /* Tells the parent that the user wants to close this document. */
        public event EventHandler? CloseRequested;

        private readonly ToolTip closeButtonTip = new ToolTip();

        /* The file that the user opened. The online validation sends this file. */
        private string? sourceFile = null;

        /* The window of the validation result, and the window of the document information. */
        private readonly OwnedWindow resultWindow = new OwnedWindow();
        private readonly OwnedWindow informationWindow = new OwnedWindow();

        /*
         * The meta information of the document: standard, profile, XML, file and XMP.
         * The panel collects it once, when it reads the file.
         */
        public DocumentInformation? Information { get; private set; } = null;

        /*
         * The result of the last online validation of this document, or null.
         *
         * The panel keeps the result as long as the tab of the document is open. Thus the
         * user can see it again without a new call, which costs units of the quota.
         */
        public ValidationReport? LastValidation { get; private set; } = null;

        /* True while a call of W System is not complete. */
        public bool IsValidating { get; private set; } = false;

        public RecordPanel()
        {
            InitializeComponent();

            FF_CloseButton.Text = Resources.Schliessen;
            closeButtonTip.SetToolTip(FF_CloseButton, Resources.DokumentSchliessen);

            FF_ValidateButton.Text = Resources.PruefenOnline;
            closeButtonTip.SetToolTip(FF_ValidateButton, Resources.PruefenOnlineTip);

            FF_ShowValidationButton.Text = Resources.Pruefergebnis;
            closeButtonTip.SetToolTip(FF_ShowValidationButton, Resources.PruefergebnisTip);
        }

        /*
         * The quota is the same for all documents. The panel listens to UsageMonitor
         * while it has a window handle. Thus a panel that never comes on the screen, for
         * example because SetFile failed, does not stay in the list of the event.
         */
        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            UsageMonitor.Changed += UsageMonitor_Changed;
            UpdateUsageLabel();
        }

        protected override void OnHandleDestroyed(EventArgs e)
        {
            UsageMonitor.Changed -= UsageMonitor_Changed;
            base.OnHandleDestroyed(e);
        }

        public TableLayoutPanel FF_TablePanel
        {
            get { return F_TablePanel; }
        }

        public TextBox FF_FileNameTb
        {
            get { return F_FileNameTb; }
        }

        public Label FF_AttachmentInfoLbl
        {
            get { return attachmentInfoLbl; }
        }

        public Panel FF_DragDropPanel
        {
            get { return F_DragDropPanel; }
        }

        [DefaultValue(null)]
        public PdfToImageConverter? RenderedDoc { get; set; } = null;

        [DefaultValue(null)]
        public PdfToImageConverter? Original { get; set; } = null;

        [DefaultValue(null)]
        public string? PdfPath { get; set; } = null;

        DocViewPanel? docPanelDerived = null;
        DocViewPanel? docPanelOriginal = null;
        XmlViewPanel? docPanelXml = null;

        bool ShowXml = false;

        /* The part of the width of one view. Each view that is open gets the same part. */
        private const float ColumnWidth = 30.0f;

        public DocViewPanel? DocPanelDerived { get { return docPanelDerived; } }
        public DocViewPanel? DocPanelOriginal { get { return docPanelOriginal; } }

        public void SetFile(string fileName, string attachmentFolder)
        {
            /* Layout */
            docPanelDerived = new DocViewPanel(Resources.title_Content);
            docPanelOriginal = new DocViewPanel("ZUGFeRD PDF");
            docPanelXml = new XmlViewPanel();
            docPanelOriginal.FF_TitlePanel.BackColor = Color.Lavender;

            FF_TablePanel.Controls.Add(docPanelDerived);
            FF_TablePanel.SetCellPosition(docPanelDerived, new TableLayoutPanelCellPosition(1, 1));
            FF_TablePanel.SetRowSpan(docPanelDerived, 2);
            docPanelDerived.Dock = DockStyle.Fill;

            FF_TablePanel.Controls.Add(docPanelOriginal);
            FF_TablePanel.SetCellPosition(docPanelOriginal, new TableLayoutPanelCellPosition(2, 1));
            FF_TablePanel.SetRowSpan(docPanelOriginal, 2);
            docPanelOriginal.Dock = DockStyle.Fill;

            FF_TablePanel.Controls.Add(docPanelXml);
            FF_TablePanel.SetCellPosition(docPanelXml, new TableLayoutPanelCellPosition(3, 1));
            FF_TablePanel.SetRowSpan(docPanelXml, 2);
            docPanelXml.Dock = DockStyle.Fill;

            /* Content */

            PdfPath = Path.GetTempFileName();
            FF_FileNameTb.Text = fileName;
            sourceFile = fileName;

            /* Convert to PDF and write to temp file */
            InvoiceWriter writer = new InvoiceWriter(fileName);
            writer.Write(PdfPath);

            PopulateAttachmentsPanel(writer.GetAttachments());

            docPanelDerived.SetDocument(PdfPath);

            if (writer.IsZugferd)
            {
                Original = new PdfToImageConverter(fileName);
                docPanelOriginal.SetDocument(fileName);
            }

            /* XML View */
            Information = DocumentInformation.Create(fileName, writer);
            docPanelXml.LoadXml(writer.GetXml());

            RenderedDoc = new PdfToImageConverter(PdfPath);
            UpdateImage();
            UpdateColumns();
        }

        /*
         * Gives each view its part of the width.
         *
         * The panel has one column for the rendered document, one for the original PDF,
         * and one for the XML. A document in XML form has no original PDF, thus that
         * column stays empty: the rendered document then takes the width of both columns.
         * If the user asks for the XML, the XML takes the second part, and the rendered
         * document and the XML get the same width.
         */
        private void UpdateColumns()
        {
            bool hasOriginal = Original != null;

            FF_TablePanel.ColumnStyles[1].Width = hasOriginal || ShowXml
                ? ColumnWidth
                : ColumnWidth * 2.0f;

            FF_TablePanel.ColumnStyles[2].Width = hasOriginal ? ColumnWidth : 0.0f;
            FF_TablePanel.ColumnStyles[3].Width = ShowXml ? ColumnWidth : 0.0f;
        }

        private void PopulateAttachmentsPanel(IReadOnlyList<AttachmentDescriptor> attachments)
        {
            F_AttachmentsPanel.Controls.Clear();

            attachmentInfoLbl.Text = attachments.Count == 0
                ? Resources.attachmentsNone
                : string.Format(Resources.attachmentsTxt, attachments.Count);
            F_AttachmentsPanel.Controls.Add(attachmentInfoLbl);

            foreach (var att in attachments)
            {
                /*
                 * An invoice can name a document without the data of its file. Such an
                 * attachment gets a plain label, and not a link. Before, it got a link
                 * that did nothing when the user clicked it.
                 */
                if (AttachmentStore.HasContent(att))
                {
                    var link = new LinkLabel
                    {
                        Text = BuildAttachmentLabel(att),
                        AutoSize = true,
                        //MaximumSize = new Size(F_AttachmentsPanel.Width - 10, 0),
                        Tag = att
                    };
                    link.LinkClicked += AttachmentLink_Clicked;
                    F_AttachmentsPanel.Controls.Add(link);
                }
                else
                {
                    F_AttachmentsPanel.Controls.Add(new Label
                    {
                        Text = $"{BuildAttachmentLabel(att)} ({Resources.NurVerweis})",
                        AutoSize = true
                    });
                }
            }
        }

        /* The text of one attachment: its name, its file name, and its number. */
        private static string BuildAttachmentLabel(AttachmentDescriptor att)
        {
            string name = !string.IsNullOrWhiteSpace(att.Name) ? att.Name! : att.FileName;

            if (string.IsNullOrWhiteSpace(name))
            {
                name = att.ReferenceId ?? string.Empty;
            }
            else if (!string.IsNullOrWhiteSpace(att.ReferenceId))
            {
                name = $"{name} ({att.ReferenceId})";
            }

            return string.IsNullOrWhiteSpace(name) ? Resources.Anlage : name;
        }

        private void AttachmentLink_Clicked(object? sender, LinkLabelLinkClickedEventArgs e)
        {
            var att = (AttachmentDescriptor)((LinkLabel)sender!).Tag!;

            try
            {
                string path = AttachmentStore.Save(att);
                AttachmentStore.Open(path);
            }
            catch (Exception ex)
            {
                /* A file that the system cannot open must not stop the application. */
                MessageBox.Show(
                    $"{Resources.AnlageNichtGeoeffnet}{Environment.NewLine}{Environment.NewLine}{ex.Message}",
                    Resources.invoice, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void UpdateImage()
        {
            if (RenderedDoc != null)
            {
                docPanelDerived!.Visible = true;
            }

            if (Original != null)
            {
                docPanelOriginal!.Visible = true;
            }
            else
            {
                docPanelOriginal!.Visible = false;
            }
        }

        public void Cleanup()
        {
            /* The windows belong to the document, thus they close with the document. */
            resultWindow.Close();
            informationWindow.Close();

            docPanelDerived!.Cleanup();
            if (Original != null)
            {
                docPanelOriginal!.Cleanup();
            }
        }

        /*
         * The panel does not close itself, because the tab page that holds it belongs to
         * the main window. Thus the panel gives the wish to the main window only.
         */
        private void FF_CloseButton_Click(object sender, EventArgs e)
        {
            CloseRequested?.Invoke(this, EventArgs.Empty);
        }

        private void FF_ShowXmlButton_Click(object sender, EventArgs e)
        {
            ShowXml = !ShowXml;

            UpdateColumns();
        }

        private async void FF_ValidateButton_Click(object sender, EventArgs e)
        {
            await ValidateOnlineAsync();
        }

        private void FF_ShowValidationButton_Click(object sender, EventArgs e)
        {
            ShowValidationResult();
        }

        /*
         * Validates the document with W System, and shows the result in a window.
         *
         * 1. If no API key is in the database, the user enters one.
         * 2. The program sends the file (XML or PDF) to POST /einvoice/validate.
         * 3. The panel keeps the result and shows it.
         * 4. The program asks GET /billing/usage for the quota, also after an error.
         *    All open documents then show the units that remain.
         */
        public async Task ValidateOnlineAsync()
        {
            if (IsValidating || sourceFile == null)
            {
                return;
            }

            Form? owner = FindForm();
            var db = new Pacioli.Config.Persistence.ConfigDb();

            string? apiKey = ApiKeyStore.Read(db);
            if (apiKey == null)
            {
                if (!ApiKeyForm.Edit(owner))
                {
                    return;
                }
                apiKey = ApiKeyStore.Read(db);
                if (apiKey == null)
                {
                    return;
                }
            }

            byte[] content;
            try
            {
                content = await File.ReadAllBytesAsync(sourceFile);
            }
            catch (Exception ex) when (ex is IOException || ex is UnauthorizedAccessException)
            {
                MessageBox.Show(owner, string.Format(Resources.msgValidationFileNotRead, Environment.NewLine, ex.Message),
                    Resources.PruefenOnline, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var client = new WSystemClient(apiKey);
            SetValidating(true);

            try
            {
                ValidationReport report = await client.ValidateAsync(content, Path.GetFileName(sourceFile));

                /* The user can close the tab during the call. */
                if (IsDisposed)
                {
                    return;
                }

                LastValidation = report;
                UpdateValidationButton();

                /* A window of an earlier result shows old data, thus it goes away. */
                resultWindow.Close();
                ShowValidationResult();
            }
            catch (WSystemException ex)
            {
                if (!IsDisposed)
                {
                    ShowFailure(owner, ex);
                }
            }
            finally
            {
                UsageInfo? usage = await client.TryGetUsageAsync();
                UsageMonitor.Update(usage);

                if (!IsDisposed)
                {
                    SetValidating(false);
                }
            }
        }

        /* Shows the result of the last validation. If its window is open, it comes to the front. */
        public void ShowValidationResult()
        {
            if (LastValidation == null)
            {
                return;
            }

            ValidationReport report = LastValidation;
            resultWindow.Show(() => new ValidationResultForm(report), FindForm());
        }

        /* Shows the meta information of the document. If its window is open, it comes to the front. */
        public void ShowDocumentInformation()
        {
            if (Information == null)
            {
                return;
            }

            DocumentInformation information = Information;
            informationWindow.Show(() => new DocumentInformationForm(information), FindForm());
        }

        private static void ShowFailure(IWin32Window? owner, WSystemException ex)
        {
            string message = ex.Failure switch
            {
                WSystemFailure.InvalidKey => Resources.msgValidationKeyRejected,
                WSystemFailure.QuotaExceeded => Resources.msgValidationQuota,
                WSystemFailure.Network => string.Format(Resources.msgValidationNoNetwork, Environment.NewLine, ex.Message),
                _ => string.Format(Resources.msgValidationFailed, Environment.NewLine, ex.Message)
            };

            MessageBox.Show(owner, message, Resources.PruefenOnline, MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void SetValidating(bool validating)
        {
            IsValidating = validating;
            FF_ValidateButton.Enabled = !validating;
            FF_ValidateButton.Text = validating ? Resources.PruefenLaeuft : Resources.PruefenOnline;
            UseWaitCursor = validating;
        }

        /* The button of the result has the color of the result: green if valid, red if not. */
        private void UpdateValidationButton()
        {
            FF_ShowValidationButton.Enabled = LastValidation != null;
            FF_ShowValidationButton.BackColor = LastValidation == null
                ? Color.SlateGray
                : LastValidation.Valid ? Color.SeaGreen : Color.Firebrick;
        }

        private void UsageMonitor_Changed(object? sender, EventArgs e)
        {
            UpdateUsageLabel();
        }

        /* The units that remain, at the right side of the row of buttons. */
        private void UpdateUsageLabel()
        {
            UsageInfo? usage = UsageMonitor.Current;

            if (usage == null)
            {
                FF_UsageLabel.Text = UsageMonitor.HasQueried ? Resources.UsageUnknown : string.Empty;
                closeButtonTip.SetToolTip(FF_UsageLabel, null);
                return;
            }

            FF_UsageLabel.Text = string.Format(Resources.UsageRemaining, usage.Remaining, usage.Quota);
            closeButtonTip.SetToolTip(FF_UsageLabel,
                string.Format(Resources.UsageTip, usage.Plan, usage.Period, usage.ValidateWeight));
        }
    }
}
