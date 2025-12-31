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
            resources.ApplyResources(menuStrip1, "menuStrip1");
            menuStrip1.ImageScalingSize = new Size(24, 24);
            menuStrip1.Items.AddRange(new ToolStripItem[] { dateiToolStripMenuItem, aboutToolStripMenuItem });
            menuStrip1.Name = "menuStrip1";
            // 
            // dateiToolStripMenuItem
            // 
            resources.ApplyResources(dateiToolStripMenuItem, "dateiToolStripMenuItem");
            dateiToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { öffnenToolStripMenuItem, speichernToolStripMenuItem, einstellungenToolStripMenuItem, beendenToolStripMenuItem });
            dateiToolStripMenuItem.Name = "dateiToolStripMenuItem";
            // 
            // öffnenToolStripMenuItem
            // 
            resources.ApplyResources(öffnenToolStripMenuItem, "öffnenToolStripMenuItem");
            öffnenToolStripMenuItem.Name = "öffnenToolStripMenuItem";
            öffnenToolStripMenuItem.Click += öffnenToolStripMenuItem_Click;
            // 
            // speichernToolStripMenuItem
            // 
            resources.ApplyResources(speichernToolStripMenuItem, "speichernToolStripMenuItem");
            speichernToolStripMenuItem.Name = "speichernToolStripMenuItem";
            speichernToolStripMenuItem.Click += speichernToolStripMenuItem_Click;
            // 
            // einstellungenToolStripMenuItem
            // 
            resources.ApplyResources(einstellungenToolStripMenuItem, "einstellungenToolStripMenuItem");
            einstellungenToolStripMenuItem.Name = "einstellungenToolStripMenuItem";
            einstellungenToolStripMenuItem.Click += einstellungenToolStripMenuItem_Click;
            // 
            // beendenToolStripMenuItem
            // 
            resources.ApplyResources(beendenToolStripMenuItem, "beendenToolStripMenuItem");
            beendenToolStripMenuItem.Name = "beendenToolStripMenuItem";
            beendenToolStripMenuItem.Click += beendenToolStripMenuItem_Click;
            // 
            // aboutToolStripMenuItem
            // 
            resources.ApplyResources(aboutToolStripMenuItem, "aboutToolStripMenuItem");
            aboutToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { überPacioliToolStripMenuItem, aufNeueVersionPrüfenToolStripMenuItem });
            aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            // 
            // überPacioliToolStripMenuItem
            // 
            resources.ApplyResources(überPacioliToolStripMenuItem, "überPacioliToolStripMenuItem");
            überPacioliToolStripMenuItem.Name = "überPacioliToolStripMenuItem";
            überPacioliToolStripMenuItem.Click += überPacioliToolStripMenuItem_Click;
            // 
            // aufNeueVersionPrüfenToolStripMenuItem
            // 
            resources.ApplyResources(aufNeueVersionPrüfenToolStripMenuItem, "aufNeueVersionPrüfenToolStripMenuItem");
            aufNeueVersionPrüfenToolStripMenuItem.Name = "aufNeueVersionPrüfenToolStripMenuItem";
            aufNeueVersionPrüfenToolStripMenuItem.Click += aufNeueVersionPrüfenToolStripMenuItem_Click;
            // 
            // openFileDialog1
            // 
            openFileDialog1.FileName = "openFileDialog1";
            resources.ApplyResources(openFileDialog1, "openFileDialog1");
            // 
            // saveFileDialog1
            // 
            resources.ApplyResources(saveFileDialog1, "saveFileDialog1");
            // 
            // F_TabControl
            // 
            resources.ApplyResources(F_TabControl, "F_TabControl");
            F_TabControl.Controls.Add(tabPage1);
            F_TabControl.Controls.Add(tabPage2);
            F_TabControl.Name = "F_TabControl";
            F_TabControl.SelectedIndex = 0;
            // 
            // tabPage1
            // 
            resources.ApplyResources(tabPage1, "tabPage1");
            tabPage1.Name = "tabPage1";
            tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            resources.ApplyResources(tabPage2, "tabPage2");
            tabPage2.Name = "tabPage2";
            tabPage2.UseVisualStyleBackColor = true;
            // 
            // MainForm
            // 
            resources.ApplyResources(this, "$this");
            AllowDrop = true;
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(F_TabControl);
            Controls.Add(menuStrip1);
            MainMenuStrip = menuStrip1;
            Name = "MainForm";
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

