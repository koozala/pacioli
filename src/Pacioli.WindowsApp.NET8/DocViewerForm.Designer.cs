namespace Pacioli.WindowsApp.NET8
{
    partial class DocViewerForm
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            F_Panel = new TableLayoutPanel();
            F_PictureBox = new PictureBox();
            flowLayoutPanel1 = new FlowLayoutPanel();
            F_PageDown = new Button();
            F_PageNo = new TextBox();
            F_PageUp = new Button();
            F_Panel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)F_PictureBox).BeginInit();
            flowLayoutPanel1.SuspendLayout();
            SuspendLayout();
            // 
            // F_Panel
            // 
            F_Panel.ColumnCount = 3;
            F_Panel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 40F));
            F_Panel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            F_Panel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 40F));
            F_Panel.Controls.Add(F_PictureBox, 1, 1);
            F_Panel.Controls.Add(flowLayoutPanel1, 1, 2);
            F_Panel.Dock = DockStyle.Fill;
            F_Panel.Location = new Point(0, 0);
            F_Panel.Name = "F_Panel";
            F_Panel.RowCount = 3;
            F_Panel.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            F_Panel.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            F_Panel.RowStyles.Add(new RowStyle(SizeType.Absolute, 60F));
            F_Panel.Size = new Size(907, 1140);
            F_Panel.TabIndex = 0;
            // 
            // F_PictureBox
            // 
            F_PictureBox.Dock = DockStyle.Fill;
            F_PictureBox.Location = new Point(43, 43);
            F_PictureBox.Name = "F_PictureBox";
            F_PictureBox.Size = new Size(821, 1034);
            F_PictureBox.TabIndex = 0;
            F_PictureBox.TabStop = false;
            // 
            // flowLayoutPanel1
            // 
            flowLayoutPanel1.Anchor = AnchorStyles.None;
            flowLayoutPanel1.Controls.Add(F_PageDown);
            flowLayoutPanel1.Controls.Add(F_PageNo);
            flowLayoutPanel1.Controls.Add(F_PageUp);
            flowLayoutPanel1.Location = new Point(255, 1083);
            flowLayoutPanel1.Name = "flowLayoutPanel1";
            flowLayoutPanel1.Size = new Size(397, 54);
            flowLayoutPanel1.TabIndex = 1;
            // 
            // F_PageDown
            // 
            F_PageDown.Location = new Point(3, 3);
            F_PageDown.Name = "F_PageDown";
            F_PageDown.Size = new Size(112, 34);
            F_PageDown.TabIndex = 0;
            F_PageDown.Text = "<<";
            F_PageDown.UseVisualStyleBackColor = true;
            F_PageDown.Click += F_PageDown_Click;
            // 
            // F_PageNo
            // 
            F_PageNo.Location = new Point(121, 3);
            F_PageNo.Name = "F_PageNo";
            F_PageNo.Size = new Size(150, 31);
            F_PageNo.TabIndex = 1;
            F_PageNo.Text = "0";
            F_PageNo.TextAlign = HorizontalAlignment.Center;
            // 
            // F_PageUp
            // 
            F_PageUp.Location = new Point(277, 3);
            F_PageUp.Name = "F_PageUp";
            F_PageUp.Size = new Size(112, 34);
            F_PageUp.TabIndex = 2;
            F_PageUp.Text = ">>";
            F_PageUp.UseVisualStyleBackColor = true;
            F_PageUp.Click += F_PageUp_Click;
            // 
            // DocViewerForm
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(907, 1140);
            Controls.Add(F_Panel);
            Name = "DocViewerForm";
            Text = "Dokument";
            F_Panel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)F_PictureBox).EndInit();
            flowLayoutPanel1.ResumeLayout(false);
            flowLayoutPanel1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel F_Panel;
        private PictureBox F_PictureBox;
        private FlowLayoutPanel flowLayoutPanel1;
        private Button F_PageDown;
        private TextBox F_PageNo;
        private Button F_PageUp;
    }
}