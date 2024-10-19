namespace Pacioli.WindowsApp.NET8.Controls
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
            F_PictureBox = new PictureBox();
            flowLayoutPanel1 = new FlowLayoutPanel();
            F_Left = new Button();
            F_PageNo = new Label();
            F_Right = new Button();
            tableLayoutPanel2 = new TableLayoutPanel();
            F_TopLabel = new Label();
            tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)F_PictureBox).BeginInit();
            flowLayoutPanel1.SuspendLayout();
            tableLayoutPanel2.SuspendLayout();
            SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 1;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.Controls.Add(F_PictureBox, 0, 1);
            tableLayoutPanel1.Controls.Add(flowLayoutPanel1, 0, 2);
            tableLayoutPanel1.Controls.Add(tableLayoutPanel2, 0, 0);
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
            // F_PictureBox
            // 
            F_PictureBox.Dock = DockStyle.Fill;
            F_PictureBox.Location = new Point(3, 63);
            F_PictureBox.Name = "F_PictureBox";
            F_PictureBox.Size = new Size(927, 921);
            F_PictureBox.SizeMode = PictureBoxSizeMode.Zoom;
            F_PictureBox.TabIndex = 0;
            F_PictureBox.TabStop = false;
            // 
            // flowLayoutPanel1
            // 
            flowLayoutPanel1.Anchor = AnchorStyles.None;
            flowLayoutPanel1.Controls.Add(F_Left);
            flowLayoutPanel1.Controls.Add(F_PageNo);
            flowLayoutPanel1.Controls.Add(F_Right);
            flowLayoutPanel1.Location = new Point(347, 990);
            flowLayoutPanel1.Name = "flowLayoutPanel1";
            flowLayoutPanel1.Size = new Size(239, 54);
            flowLayoutPanel1.TabIndex = 2;
            // 
            // F_Left
            // 
            F_Left.Anchor = AnchorStyles.Left;
            F_Left.Location = new Point(3, 3);
            F_Left.Name = "F_Left";
            F_Left.Size = new Size(74, 51);
            F_Left.TabIndex = 0;
            F_Left.Text = "<<";
            F_Left.UseVisualStyleBackColor = true;
            F_Left.Click += F_Left_Click;
            // 
            // F_PageNo
            // 
            F_PageNo.Anchor = AnchorStyles.Left;
            F_PageNo.AutoSize = true;
            F_PageNo.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            F_PageNo.Location = new Point(83, 12);
            F_PageNo.Name = "F_PageNo";
            F_PageNo.Size = new Size(69, 32);
            F_PageNo.TabIndex = 1;
            F_PageNo.Text = "Page";
            // 
            // F_Right
            // 
            F_Right.Anchor = AnchorStyles.None;
            F_Right.Location = new Point(158, 3);
            F_Right.Name = "F_Right";
            F_Right.Size = new Size(72, 51);
            F_Right.TabIndex = 2;
            F_Right.Text = ">>";
            F_Right.UseVisualStyleBackColor = true;
            F_Right.Click += F_Right_Click;
            // 
            // tableLayoutPanel2
            // 
            tableLayoutPanel2.BackColor = Color.Moccasin;
            tableLayoutPanel2.ColumnCount = 1;
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20F));
            tableLayoutPanel2.Controls.Add(F_TopLabel, 0, 0);
            tableLayoutPanel2.Dock = DockStyle.Fill;
            tableLayoutPanel2.Location = new Point(3, 3);
            tableLayoutPanel2.Name = "tableLayoutPanel2";
            tableLayoutPanel2.RowCount = 1;
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel2.Size = new Size(927, 54);
            tableLayoutPanel2.TabIndex = 3;
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
            // DocViewPanel
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(tableLayoutPanel1);
            Name = "DocViewPanel";
            Size = new Size(933, 1047);
            tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)F_PictureBox).EndInit();
            flowLayoutPanel1.ResumeLayout(false);
            flowLayoutPanel1.PerformLayout();
            tableLayoutPanel2.ResumeLayout(false);
            tableLayoutPanel2.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel tableLayoutPanel1;
        private PictureBox F_PictureBox;
        private Label F_TopLabel;
        private FlowLayoutPanel flowLayoutPanel1;
        private Button F_Left;
        private Label F_PageNo;
        private Button F_Right;
        private TableLayoutPanel tableLayoutPanel2;
    }
}
