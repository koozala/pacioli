namespace Pacioli.WindowsApp.NET8.Controls
{
    partial class XmlViewPanel
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            tableLayoutPanel1 = new TableLayoutPanel();
            F_XmlTreeView = new TreeView();
            F_XmlViewLabel = new Label();
            tableLayoutPanel1.SuspendLayout();
            SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 1;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.Controls.Add(F_XmlTreeView, 0, 1);
            tableLayoutPanel1.Controls.Add(F_XmlViewLabel, 0, 0);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(0, 0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 3;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            tableLayoutPanel1.Size = new Size(814, 769);
            tableLayoutPanel1.TabIndex = 0;
            // 
            // F_XmlTreeView
            // 
            F_XmlTreeView.Dock = DockStyle.Fill;
            F_XmlTreeView.Location = new Point(3, 53);
            F_XmlTreeView.Name = "F_XmlTreeView";
            F_XmlTreeView.Size = new Size(808, 673);
            F_XmlTreeView.TabIndex = 0;
            // 
            // F_XmlViewLabel
            // 
            F_XmlViewLabel.AutoSize = true;
            F_XmlViewLabel.Dock = DockStyle.Fill;
            F_XmlViewLabel.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            F_XmlViewLabel.Location = new Point(3, 0);
            F_XmlViewLabel.Name = "F_XmlViewLabel";
            F_XmlViewLabel.Size = new Size(808, 50);
            F_XmlViewLabel.TabIndex = 1;
            F_XmlViewLabel.Text = "XML Ansicht";
            F_XmlViewLabel.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // XmlViewPanel
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(tableLayoutPanel1);
            Name = "XmlViewPanel";
            Size = new Size(814, 769);
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel tableLayoutPanel1;
        private TreeView F_XmlTreeView;
        private Label F_XmlViewLabel;
    }
}
