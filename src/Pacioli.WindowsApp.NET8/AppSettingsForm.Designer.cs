namespace Pacioli.WindowsApp.NET8
{
    partial class AppSettingsForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AppSettingsForm));
            label1 = new Label();
            attDestTb = new TextBox();
            selectFolderBtn = new Button();
            saveBtn = new Button();
            folderBrowserDialog1 = new FolderBrowserDialog();
            languageLbl = new Label();
            languageCombo = new ComboBox();
            label2 = new Label();
            F_OpenAfterSave = new CheckBox();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(27, 39);
            label1.Name = "label1";
            label1.Size = new Size(270, 25);
            label1.TabIndex = 0;
            label1.Text = "Ausgabeverzeichnis für Anhänge";
            // 
            // attDestTb
            // 
            attDestTb.Location = new Point(323, 36);
            attDestTb.Name = "attDestTb";
            attDestTb.Size = new Size(387, 31);
            attDestTb.TabIndex = 1;
            // 
            // selectFolderBtn
            // 
            selectFolderBtn.Location = new Point(729, 36);
            selectFolderBtn.Name = "selectFolderBtn";
            selectFolderBtn.Size = new Size(37, 34);
            selectFolderBtn.TabIndex = 2;
            selectFolderBtn.Text = "...";
            selectFolderBtn.UseVisualStyleBackColor = true;
            selectFolderBtn.Click += selectFolderBtn_Click;
            // 
            // saveBtn
            // 
            saveBtn.Location = new Point(30, 260);
            saveBtn.Name = "saveBtn";
            saveBtn.Size = new Size(112, 34);
            saveBtn.TabIndex = 3;
            saveBtn.Text = "Speichern";
            saveBtn.UseVisualStyleBackColor = true;
            saveBtn.Click += saveBtn_Click;
            // 
            // languageLbl
            // 
            languageLbl.AutoSize = true;
            languageLbl.Location = new Point(30, 98);
            languageLbl.Name = "languageLbl";
            languageLbl.Size = new Size(75, 25);
            languageLbl.TabIndex = 4;
            languageLbl.Text = "Sprache";
            // 
            // languageCombo
            // 
            languageCombo.FormattingEnabled = true;
            languageCombo.Location = new Point(323, 95);
            languageCombo.Name = "languageCombo";
            languageCombo.Size = new Size(387, 33);
            languageCombo.TabIndex = 5;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(27, 161);
            label2.Name = "label2";
            label2.Size = new Size(153, 25);
            label2.TabIndex = 6;
            label2.Text = "Als PDF speichern";
            // 
            // F_OpenAfterSave
            // 
            F_OpenAfterSave.AutoSize = true;
            F_OpenAfterSave.Location = new Point(323, 160);
            F_OpenAfterSave.Name = "F_OpenAfterSave";
            F_OpenAfterSave.Size = new Size(308, 29);
            F_OpenAfterSave.TabIndex = 7;
            F_OpenAfterSave.Text = "PDF Viewer nach Speichern öffnen";
            F_OpenAfterSave.UseVisualStyleBackColor = true;
            // 
            // AppSettingsForm
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(803, 327);
            Controls.Add(F_OpenAfterSave);
            Controls.Add(label2);
            Controls.Add(languageCombo);
            Controls.Add(languageLbl);
            Controls.Add(saveBtn);
            Controls.Add(selectFolderBtn);
            Controls.Add(attDestTb);
            Controls.Add(label1);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "AppSettingsForm";
            Text = "Einstellungen";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private TextBox attDestTb;
        private Button selectFolderBtn;
        private Button saveBtn;
        private FolderBrowserDialog folderBrowserDialog1;
        private Label languageLbl;
        private ComboBox languageCombo;
        private Label label2;
        private CheckBox F_OpenAfterSave;
    }
}