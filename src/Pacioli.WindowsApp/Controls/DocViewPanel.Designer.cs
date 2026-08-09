namespace Pacioli.WindowsApp.Controls
{
    partial class DocViewPanel
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
            F_TitlePanel = new TableLayoutPanel();
            F_TopLabel = new Label();
            F_WebView = new Microsoft.Web.WebView2.WinForms.WebView2();
            tableLayoutPanel1.SuspendLayout();
            F_TitlePanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)F_WebView).BeginInit();
            SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 1;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.Controls.Add(F_TitlePanel, 0, 0);
            tableLayoutPanel1.Controls.Add(F_WebView, 0, 1);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(0, 0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 3;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 60F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 60F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel1.Size = new Size(933, 1047);
            tableLayoutPanel1.TabIndex = 0;
            // 
            // F_TitlePanel
            // 
            F_TitlePanel.BackColor = Color.Moccasin;
            F_TitlePanel.ColumnCount = 1;
            F_TitlePanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            F_TitlePanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20F));
            F_TitlePanel.Controls.Add(F_TopLabel, 0, 0);
            F_TitlePanel.Dock = DockStyle.Fill;
            F_TitlePanel.Location = new Point(3, 3);
            F_TitlePanel.Name = "F_TitlePanel";
            F_TitlePanel.RowCount = 1;
            F_TitlePanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            F_TitlePanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            F_TitlePanel.Size = new Size(927, 54);
            F_TitlePanel.TabIndex = 3;
            // 
            // F_TopLabel
            // 
            F_TopLabel.Anchor = AnchorStyles.None;
            F_TopLabel.AutoSize = true;
            F_TopLabel.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            F_TopLabel.Location = new Point(356, 11);
            F_TopLabel.Name = "F_TopLabel";
            F_TopLabel.Size = new Size(215, 32);
            F_TopLabel.TabIndex = 1;
            F_TopLabel.Text = "<Keine Auswahl>";
            F_TopLabel.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // F_WebView
            // 
            F_WebView.AllowExternalDrop = true;
            F_WebView.CreationProperties = null;
            F_WebView.DefaultBackgroundColor = Color.White;
            F_WebView.Dock = DockStyle.Fill;
            F_WebView.Location = new Point(3, 63);
            F_WebView.Name = "F_WebView";
            F_WebView.Size = new Size(927, 921);
            F_WebView.TabIndex = 4;
            F_WebView.ZoomFactor = 1D;
            // 
            // DocViewPanel
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(tableLayoutPanel1);
            Name = "DocViewPanel";
            Size = new Size(933, 1047);
            tableLayoutPanel1.ResumeLayout(false);
            F_TitlePanel.ResumeLayout(false);
            F_TitlePanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)F_WebView).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel tableLayoutPanel1;
        private Label F_TopLabel;
        private TableLayoutPanel F_TitlePanel;
        private Microsoft.Web.WebView2.WinForms.WebView2 F_WebView;
    }
}
