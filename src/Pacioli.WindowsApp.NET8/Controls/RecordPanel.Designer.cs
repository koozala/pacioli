namespace Pacioli.WindowsApp.NET8.Controls
{
    partial class RecordPanel
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
            F_TablePanel = new TableLayoutPanel();
            F_FileNameTb = new TextBox();
            flowLayoutPanel2 = new FlowLayoutPanel();
            attachmentInfoLbl = new Label();
            F_DragDropPanel = new Panel();
            F_DragDropLabel = new Label();
            F_TablePanel.SuspendLayout();
            flowLayoutPanel2.SuspendLayout();
            F_DragDropPanel.SuspendLayout();
            SuspendLayout();
            // 
            // F_TablePanel
            // 
            F_TablePanel.AllowDrop = true;
            F_TablePanel.AutoSize = true;
            F_TablePanel.ColumnCount = 4;
            F_TablePanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 44F));
            F_TablePanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40F));
            F_TablePanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40F));
            F_TablePanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            F_TablePanel.Controls.Add(F_FileNameTb, 1, 3);
            F_TablePanel.Controls.Add(flowLayoutPanel2, 3, 1);
            F_TablePanel.Controls.Add(F_DragDropPanel, 3, 2);
            F_TablePanel.Dock = DockStyle.Fill;
            F_TablePanel.Location = new Point(0, 0);
            F_TablePanel.Margin = new Padding(3, 4, 3, 4);
            F_TablePanel.Name = "F_TablePanel";
            F_TablePanel.RowCount = 4;
            F_TablePanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));
            F_TablePanel.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            F_TablePanel.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            F_TablePanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            F_TablePanel.Size = new Size(1661, 1162);
            F_TablePanel.TabIndex = 2;
            // 
            // F_FileNameTb
            // 
            F_TablePanel.SetColumnSpan(F_FileNameTb, 3);
            F_FileNameTb.Dock = DockStyle.Fill;
            F_FileNameTb.Location = new Point(47, 1126);
            F_FileNameTb.Margin = new Padding(3, 4, 3, 4);
            F_FileNameTb.Multiline = true;
            F_FileNameTb.Name = "F_FileNameTb";
            F_FileNameTb.ReadOnly = true;
            F_FileNameTb.Size = new Size(1611, 32);
            F_FileNameTb.TabIndex = 5;
            // 
            // flowLayoutPanel2
            // 
            flowLayoutPanel2.BorderStyle = BorderStyle.FixedSingle;
            flowLayoutPanel2.Controls.Add(attachmentInfoLbl);
            flowLayoutPanel2.Dock = DockStyle.Fill;
            flowLayoutPanel2.FlowDirection = FlowDirection.TopDown;
            flowLayoutPanel2.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            flowLayoutPanel2.Location = new Point(1339, 53);
            flowLayoutPanel2.Name = "flowLayoutPanel2";
            flowLayoutPanel2.Size = new Size(319, 530);
            flowLayoutPanel2.TabIndex = 8;
            // 
            // attachmentInfoLbl
            // 
            attachmentInfoLbl.AutoSize = true;
            attachmentInfoLbl.Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Point, 0);
            attachmentInfoLbl.Location = new Point(3, 0);
            attachmentInfoLbl.Name = "attachmentInfoLbl";
            attachmentInfoLbl.Size = new Size(159, 30);
            attachmentInfoLbl.TabIndex = 0;
            attachmentInfoLbl.Text = "Keine Anhänge";
            // 
            // F_DragDropPanel
            // 
            F_DragDropPanel.BackColor = SystemColors.ScrollBar;
            F_DragDropPanel.BorderStyle = BorderStyle.FixedSingle;
            F_DragDropPanel.Controls.Add(F_DragDropLabel);
            F_DragDropPanel.Dock = DockStyle.Fill;
            F_DragDropPanel.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            F_DragDropPanel.Location = new Point(1339, 589);
            F_DragDropPanel.Name = "F_DragDropPanel";
            F_DragDropPanel.Size = new Size(319, 530);
            F_DragDropPanel.TabIndex = 9;
            // 
            // F_DragDropLabel
            // 
            F_DragDropLabel.AutoSize = true;
            F_DragDropLabel.Dock = DockStyle.Fill;
            F_DragDropLabel.Location = new Point(0, 0);
            F_DragDropLabel.Name = "F_DragDropLabel";
            F_DragDropLabel.Size = new Size(209, 32);
            F_DragDropLabel.TabIndex = 0;
            F_DragDropLabel.Text = "Drag && Drop Here";
            // 
            // RecordPanel
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(F_TablePanel);
            Name = "RecordPanel";
            Size = new Size(1661, 1162);
            F_TablePanel.ResumeLayout(false);
            F_TablePanel.PerformLayout();
            flowLayoutPanel2.ResumeLayout(false);
            flowLayoutPanel2.PerformLayout();
            F_DragDropPanel.ResumeLayout(false);
            F_DragDropPanel.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TableLayoutPanel F_TablePanel;
        private TextBox F_FileNameTb;
        private FlowLayoutPanel flowLayoutPanel2;
        private Label attachmentInfoLbl;
        private Panel F_DragDropPanel;
        private Label F_DragDropLabel;
    }
}
