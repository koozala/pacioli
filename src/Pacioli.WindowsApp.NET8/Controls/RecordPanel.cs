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

        public Converter? converter { get; set; } = null;
        public Converter? original { get; set; } = null;
        public string? pdfPath { get; set; } = null;

        DocViewPanel? docPanelDerived = null;
        DocViewPanel? docPanelOriginal = null;

        public DocViewPanel? DocPanelDerived { get { return docPanelDerived; } }
        public DocViewPanel? DocPanelOriginal { get { return docPanelOriginal; } }

        public void SetFile(string fileName, string attachmentFolder)
        {
            /* Layout */
            docPanelDerived = new DocViewPanel("E-Rechnung");
            docPanelOriginal = new DocViewPanel("ZUGFeRD PDF");
            docPanelOriginal.FF_TitlePanel.BackColor = Color.Lavender;

            FF_TablePanel.Controls.Add(docPanelDerived);
            FF_TablePanel.SetCellPosition(docPanelDerived, new TableLayoutPanelCellPosition(1, 1));
            docPanelDerived.Dock = DockStyle.Fill;

            FF_TablePanel.Controls.Add(docPanelOriginal);
            FF_TablePanel.SetCellPosition(docPanelOriginal, new TableLayoutPanelCellPosition(2, 1));
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

            docPanelDerived.SetDocument(pdfPath);

            if (writer.IsZugferd)
            {
                original = new Converter(fileName);
                docPanelOriginal.SetDocument(fileName);
            }

            converter = new Converter(pdfPath);
            UpdateImage();
        }

        private void UpdateImage()
        {
            if (converter != null)
            {
                docPanelDerived!.Visible = true;
                docPanelDerived.FF_Picture.SizeMode = PictureBoxSizeMode.Zoom;
            }

            if (original != null)
            {
                docPanelOriginal!.Visible = true;
                docPanelOriginal.FF_Picture.SizeMode = PictureBoxSizeMode.Zoom;
            }
            else
            {
                docPanelOriginal!.Visible = false;
            }
        }


    }
}
