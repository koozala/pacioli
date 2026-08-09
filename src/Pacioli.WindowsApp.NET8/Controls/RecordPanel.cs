using Pacioli.Language.Resources;
using Pacioli.Pdf.ERechnung;
using Pacioli.Pdf.Invoice;
using Pacioli.Preview.ImageGeneration;
using Pacioli.WindowsApp.NET8.Util;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace Pacioli.WindowsApp.NET8.Controls
{
    public partial class RecordPanel : UserControl
    {
        public RecordPanel()
        {
            InitializeComponent();
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

            FF_TablePanel.ColumnStyles[3].Width = 0; // Hide XML view by default

            /* Content */

            PdfPath = Path.GetTempFileName();
            FF_FileNameTb.Text = fileName;

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
            docPanelXml.LoadXml(writer.GetXml());

            RenderedDoc = new PdfToImageConverter(PdfPath);
            UpdateImage();
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
            docPanelDerived!.Cleanup();
            if (Original != null)
            {
                docPanelOriginal!.Cleanup();
            }
        }

        private void FF_ShowXmlButton_Click(object sender, EventArgs e)
        {
            ShowXml = !ShowXml;

            if (ShowXml)
            {
                FF_TablePanel.ColumnStyles[3].Width = 33; // Show XML view
            }
            else
            {
                FF_TablePanel.ColumnStyles[3].Width = 0; // Hide XML view by default
            }
        }
    }
}
