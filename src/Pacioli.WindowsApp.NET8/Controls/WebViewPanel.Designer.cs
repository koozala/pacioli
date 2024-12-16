namespace Pacioli.WindowsApp.NET8.Controls
{
    partial class WebViewPanel
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
            F_WebView = new Microsoft.Web.WebView2.WinForms.WebView2();
            tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)F_WebView).BeginInit();
            SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 3;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20F));
            tableLayoutPanel1.Controls.Add(F_WebView, 1, 1);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(0, 0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 3;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel1.Size = new Size(1092, 1018);
            tableLayoutPanel1.TabIndex = 0;
            // 
            // F_WebView
            // 
            F_WebView.AllowExternalDrop = true;
            F_WebView.CreationProperties = null;
            F_WebView.DefaultBackgroundColor = Color.White;
            F_WebView.Dock = DockStyle.Fill;
            F_WebView.Location = new Point(23, 23);
            F_WebView.Name = "F_WebView";
            F_WebView.Size = new Size(1046, 972);
            F_WebView.Source = new Uri("https://www.bing.com", UriKind.Absolute);
            F_WebView.TabIndex = 0;
            F_WebView.ZoomFactor = 1D;
            // 
            // WebViewPanel
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(tableLayoutPanel1);
            Name = "WebViewPanel";
            Size = new Size(1092, 1018);
            tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)F_WebView).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel tableLayoutPanel1;
        private Microsoft.Web.WebView2.WinForms.WebView2 F_WebView;
    }
}
