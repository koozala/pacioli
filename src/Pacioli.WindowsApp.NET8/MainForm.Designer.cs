namespace Pacioli.WindowsApp.NET8
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            menuStrip1 = new MenuStrip();
            dateiToolStripMenuItem = new ToolStripMenuItem();
            öffnenToolStripMenuItem = new ToolStripMenuItem();
            speichernToolStripMenuItem = new ToolStripMenuItem();
            einstellungenToolStripMenuItem = new ToolStripMenuItem();
            beendenToolStripMenuItem = new ToolStripMenuItem();
            aboutToolStripMenuItem = new ToolStripMenuItem();
            überPacioliToolStripMenuItem = new ToolStripMenuItem();
            tableLayoutPanel1 = new TableLayoutPanel();
            fileNameTb = new TextBox();
            flowLayoutPanel2 = new FlowLayoutPanel();
            attachmentInfoLbl = new Label();
            openFileDialog1 = new OpenFileDialog();
            saveFileDialog1 = new SaveFileDialog();
            aufNeueVersionPrüfenToolStripMenuItem = new ToolStripMenuItem();
            menuStrip1.SuspendLayout();
            tableLayoutPanel1.SuspendLayout();
            flowLayoutPanel2.SuspendLayout();
            SuspendLayout();
            // 
            // menuStrip1
            // 
            menuStrip1.ImageScalingSize = new Size(24, 24);
            menuStrip1.Items.AddRange(new ToolStripItem[] { dateiToolStripMenuItem, aboutToolStripMenuItem });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Padding = new Padding(7, 2, 0, 2);
            menuStrip1.Size = new Size(1351, 33);
            menuStrip1.TabIndex = 0;
            menuStrip1.Text = "menuStrip1";
            // 
            // dateiToolStripMenuItem
            // 
            dateiToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { öffnenToolStripMenuItem, speichernToolStripMenuItem, einstellungenToolStripMenuItem, beendenToolStripMenuItem });
            dateiToolStripMenuItem.Name = "dateiToolStripMenuItem";
            dateiToolStripMenuItem.Size = new Size(69, 29);
            dateiToolStripMenuItem.Text = "Datei";
            // 
            // öffnenToolStripMenuItem
            // 
            öffnenToolStripMenuItem.Name = "öffnenToolStripMenuItem";
            öffnenToolStripMenuItem.Size = new Size(240, 34);
            öffnenToolStripMenuItem.Text = "Öffnen";
            öffnenToolStripMenuItem.Click += öffnenToolStripMenuItem_Click;
            // 
            // speichernToolStripMenuItem
            // 
            speichernToolStripMenuItem.Name = "speichernToolStripMenuItem";
            speichernToolStripMenuItem.Size = new Size(240, 34);
            speichernToolStripMenuItem.Text = "PDF Speichern...";
            speichernToolStripMenuItem.Click += speichernToolStripMenuItem_Click;
            // 
            // einstellungenToolStripMenuItem
            // 
            einstellungenToolStripMenuItem.Name = "einstellungenToolStripMenuItem";
            einstellungenToolStripMenuItem.Size = new Size(240, 34);
            einstellungenToolStripMenuItem.Text = "Einstellungen";
            einstellungenToolStripMenuItem.Click += einstellungenToolStripMenuItem_Click;
            // 
            // beendenToolStripMenuItem
            // 
            beendenToolStripMenuItem.Name = "beendenToolStripMenuItem";
            beendenToolStripMenuItem.Size = new Size(240, 34);
            beendenToolStripMenuItem.Text = "Beenden";
            beendenToolStripMenuItem.Click += beendenToolStripMenuItem_Click;
            // 
            // aboutToolStripMenuItem
            // 
            aboutToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { überPacioliToolStripMenuItem, aufNeueVersionPrüfenToolStripMenuItem });
            aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            aboutToolStripMenuItem.Size = new Size(122, 29);
            aboutToolStripMenuItem.Text = "Information";
            // 
            // überPacioliToolStripMenuItem
            // 
            überPacioliToolStripMenuItem.Name = "überPacioliToolStripMenuItem";
            überPacioliToolStripMenuItem.Size = new Size(305, 34);
            überPacioliToolStripMenuItem.Text = "Über Pacioli";
            überPacioliToolStripMenuItem.Click += überPacioliToolStripMenuItem_Click;
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.AutoSize = true;
            tableLayoutPanel1.ColumnCount = 4;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 44F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            tableLayoutPanel1.Controls.Add(fileNameTb, 1, 2);
            tableLayoutPanel1.Controls.Add(flowLayoutPanel2, 3, 1);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(0, 33);
            tableLayoutPanel1.Margin = new Padding(3, 4, 3, 4);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 3;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 60F));
            tableLayoutPanel1.Size = new Size(1351, 1055);
            tableLayoutPanel1.TabIndex = 1;
            // 
            // fileNameTb
            // 
            tableLayoutPanel1.SetColumnSpan(fileNameTb, 3);
            fileNameTb.Dock = DockStyle.Fill;
            fileNameTb.Location = new Point(47, 999);
            fileNameTb.Margin = new Padding(3, 4, 3, 4);
            fileNameTb.Multiline = true;
            fileNameTb.Name = "fileNameTb";
            fileNameTb.ReadOnly = true;
            fileNameTb.Size = new Size(1301, 52);
            fileNameTb.TabIndex = 5;
            // 
            // flowLayoutPanel2
            // 
            flowLayoutPanel2.Controls.Add(attachmentInfoLbl);
            flowLayoutPanel2.Dock = DockStyle.Fill;
            flowLayoutPanel2.FlowDirection = FlowDirection.TopDown;
            flowLayoutPanel2.Location = new Point(1091, 53);
            flowLayoutPanel2.Name = "flowLayoutPanel2";
            flowLayoutPanel2.Size = new Size(257, 939);
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
            // openFileDialog1
            // 
            openFileDialog1.FileName = "openFileDialog1";
            // 
            // aufNeueVersionPrüfenToolStripMenuItem
            // 
            aufNeueVersionPrüfenToolStripMenuItem.Name = "aufNeueVersionPrüfenToolStripMenuItem";
            aufNeueVersionPrüfenToolStripMenuItem.Size = new Size(305, 34);
            aufNeueVersionPrüfenToolStripMenuItem.Text = "Auf neue Version prüfen";
            aufNeueVersionPrüfenToolStripMenuItem.Click += aufNeueVersionPrüfenToolStripMenuItem_Click;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1351, 1088);
            Controls.Add(tableLayoutPanel1);
            Controls.Add(menuStrip1);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MainMenuStrip = menuStrip1;
            Margin = new Padding(3, 4, 3, 4);
            Name = "MainForm";
            Text = "Pacioli - ERechnung zu PDF";
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel1.PerformLayout();
            flowLayoutPanel2.ResumeLayout(false);
            flowLayoutPanel2.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem dateiToolStripMenuItem;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.ToolStripMenuItem öffnenToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem beendenToolStripMenuItem;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.TextBox fileNameTb;
        private System.Windows.Forms.ToolStripMenuItem speichernToolStripMenuItem;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private ToolStripMenuItem aboutToolStripMenuItem;
        private ToolStripMenuItem überPacioliToolStripMenuItem;
        private ToolStripMenuItem einstellungenToolStripMenuItem;
        private FlowLayoutPanel flowLayoutPanel2;
        private Label attachmentInfoLbl;
        private ToolStripMenuItem aufNeueVersionPrüfenToolStripMenuItem;
    }
}

