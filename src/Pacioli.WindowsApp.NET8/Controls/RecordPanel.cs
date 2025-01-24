using Pacioli.Language.Resources;
using Pacioli.Pdf.Invoice;
using Pacioli.Preview.ImageGeneration;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.Contracts;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public string? pdfPath { get; set; } = null;

        DocViewPanel? docPanelDerived = null;
        DocViewPanel? docPanelOriginal = null;

        bool ShowOriginal = false;

        public DocViewPanel? DocPanelDerived { get { return docPanelDerived; } }
        public DocViewPanel? DocPanelOriginal { get { return docPanelOriginal; } }

        public void SetFile(string fileName, string attachmentFolder)
        {
            /* Layout */
            docPanelDerived = new DocViewPanel("E-Rechnung", fileName, attachmentFolder, false);
            docPanelOriginal = new DocViewPanel("ZUGFeRD PDF (Original)", fileName, attachmentFolder, true);
            docPanelOriginal.FF_SwitchView.Visible = false;
            docPanelOriginal.FF_TitlePanel.BackColor = Color.Lavender;

            FF_TablePanel.Controls.Add(docPanelDerived);
            FF_TablePanel.SetCellPosition(docPanelDerived, new TableLayoutPanelCellPosition(1, 1));
            FF_TablePanel.SetRowSpan(docPanelDerived, 2);
            docPanelDerived.Dock = DockStyle.Fill;

            FF_TablePanel.Controls.Add(docPanelOriginal);
            FF_TablePanel.SetCellPosition(docPanelOriginal, new TableLayoutPanelCellPosition(2, 1));
            FF_TablePanel.SetRowSpan(docPanelOriginal, 2);
            docPanelOriginal.Dock = DockStyle.Fill;

            /* Content */

            pdfPath = Path.GetTempFileName();
            FF_FileNameTb.Text = fileName;

            /* Convert to PDF and write to temp file */
            Writer writer = new Writer(fileName, attachmentFolder);
            writer.Write(pdfPath);
            int countAttachments = writer.GetAttachmentCount();
            string aInfo = Resources.attachmentsNone;
            if (countAttachments > 0)
            {
                aInfo = string.Format(Resources.attachmentsTxt, countAttachments);
            }
            FF_AttachmentInfoLbl.Text = aInfo;

            docPanelDerived.Render();

            if (writer.IsZugferd)
            {
                ShowOriginal = true;
                docPanelOriginal.Render();
            }

            UpdateImage();
        }

        private void UpdateImage()
        {
            docPanelDerived!.Visible = true;
            docPanelOriginal!.Visible = ShowOriginal;
        }

        public void Cleanup()
        {
            docPanelDerived!.Cleanup();
            if (ShowOriginal)
            {
                docPanelOriginal!.Cleanup();
            }
        }

    }
}
