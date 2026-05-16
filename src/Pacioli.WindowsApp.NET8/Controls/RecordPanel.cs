using Pacioli.Language.Resources;
using Pacioli.Pdf.ERechnung;
using Pacioli.Pdf.Invoice;
using Pacioli.Preview.ImageGeneration;
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

        public PdfToImageConverter? renderedDoc { get; set; } = null;
        public PdfToImageConverter? original { get; set; } = null;
        public string? pdfPath { get; set; } = null;

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

            pdfPath = Path.GetTempFileName();
            FF_FileNameTb.Text = fileName;

            /* Convert to PDF and write to temp file */
            InvoiceWriter writer = new InvoiceWriter(fileName);
            writer.Write(pdfPath);

            PopulateAttachmentsPanel(writer.GetAttachments());

            docPanelDerived.SetDocument(pdfPath);

            if (writer.IsZugferd)
            {
                original = new PdfToImageConverter(fileName);
                docPanelOriginal.SetDocument(fileName);
            }

            /* XML View */
            docPanelXml.LoadXml(writer.GetXml());

            renderedDoc = new PdfToImageConverter(pdfPath);
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
                string label = !string.IsNullOrWhiteSpace(att.ReferencedDocument?.Name)
                    ? att.ReferencedDocument!.Name
                    : att.FileName;
                var link = new LinkLabel
                {
                    Text = label,
                    AutoSize = true,
                    //MaximumSize = new Size(F_AttachmentsPanel.Width - 10, 0),
                    Tag = att
                };
                link.LinkClicked += AttachmentLink_Clicked;
                F_AttachmentsPanel.Controls.Add(link);
            }
        }

        private void AttachmentLink_Clicked(object? sender, LinkLabelLinkClickedEventArgs e)
        {
            var att = (AttachmentDescriptor)((LinkLabel)sender!).Tag!;
            var data = att.ReferencedDocument?.AttachmentBinaryObject;
            if (data == null) return;

            var tempPath = Path.Combine(Path.GetTempPath(), att.FileName);
            File.WriteAllBytes(tempPath, data);
            Process.Start(new ProcessStartInfo(tempPath) { UseShellExecute = true });
        }

        private void UpdateImage()
        {
            if (renderedDoc != null)
            {
                docPanelDerived!.Visible = true;
            }

            if (original != null)
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
            if (original != null)
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
