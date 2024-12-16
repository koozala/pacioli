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
            aufNeueVersionPrüfenToolStripMenuItem = new ToolStripMenuItem();
            openFileDialog1 = new OpenFileDialog();
            saveFileDialog1 = new SaveFileDialog();
            F_TabControl = new TabControl();
            tabPage1 = new TabPage();
            tabPage2 = new TabPage();
            menuStrip1.SuspendLayout();
            F_TabControl.SuspendLayout();
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
            // aufNeueVersionPrüfenToolStripMenuItem
            // 
            aufNeueVersionPrüfenToolStripMenuItem.Name = "aufNeueVersionPrüfenToolStripMenuItem";
            aufNeueVersionPrüfenToolStripMenuItem.Size = new Size(305, 34);
            aufNeueVersionPrüfenToolStripMenuItem.Text = "Auf neue Version prüfen";
            aufNeueVersionPrüfenToolStripMenuItem.Click += aufNeueVersionPrüfenToolStripMenuItem_Click;
            // 
            // openFileDialog1
            // 
            openFileDialog1.FileName = "openFileDialog1";
            // 
            // F_TabControl
            // 
            F_TabControl.Controls.Add(tabPage1);
            F_TabControl.Controls.Add(tabPage2);
            F_TabControl.Dock = DockStyle.Fill;
            F_TabControl.Font = new Font("Segoe UI", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            F_TabControl.Location = new Point(0, 33);
            F_TabControl.Name = "F_TabControl";
            F_TabControl.Padding = new Point(9, 6);
            F_TabControl.SelectedIndex = 0;
            F_TabControl.Size = new Size(1351, 1055);
            F_TabControl.TabIndex = 2;
            // 
            // tabPage1
            // 
            tabPage1.Location = new Point(4, 43);
            tabPage1.Name = "tabPage1";
            tabPage1.Padding = new Padding(3);
            tabPage1.Size = new Size(1343, 1008);
            tabPage1.TabIndex = 0;
            tabPage1.Text = "tabPage1";
            tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            tabPage2.Location = new Point(4, 43);
            tabPage2.Name = "tabPage2";
            tabPage2.Padding = new Padding(3);
            tabPage2.Size = new Size(1343, 1008);
            tabPage2.TabIndex = 1;
            tabPage2.Text = "tabPage2";
            tabPage2.UseVisualStyleBackColor = true;
            // 
            // MainForm
            // 
            AllowDrop = true;
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1351, 1088);
            Controls.Add(F_TabControl);
            Controls.Add(menuStrip1);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MainMenuStrip = menuStrip1;
            Margin = new Padding(3, 4, 3, 4);
            Name = "MainForm";
            Text = "Pacioli - ERechnung zu PDF";
            FormClosing += MainForm_FormClosing;
            SizeChanged += MainForm_SizeChanged;
            DragDrop += MainForm_DragDrop;
            DragEnter += MainForm_DragEnter;
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            F_TabControl.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem dateiToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem öffnenToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem beendenToolStripMenuItem;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.ToolStripMenuItem speichernToolStripMenuItem;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private ToolStripMenuItem aboutToolStripMenuItem;
        private ToolStripMenuItem überPacioliToolStripMenuItem;
        private ToolStripMenuItem einstellungenToolStripMenuItem;
        private ToolStripMenuItem aufNeueVersionPrüfenToolStripMenuItem;
        private TabControl F_TabControl;
        private TabPage tabPage1;
        private TabPage tabPage2;
    }
}

