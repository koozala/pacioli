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
            resources.ApplyResources(label1, "label1");
            label1.Name = "label1";
            // 
            // attDestTb
            // 
            resources.ApplyResources(attDestTb, "attDestTb");
            attDestTb.Name = "attDestTb";
            // 
            // selectFolderBtn
            // 
            resources.ApplyResources(selectFolderBtn, "selectFolderBtn");
            selectFolderBtn.Name = "selectFolderBtn";
            selectFolderBtn.UseVisualStyleBackColor = true;
            selectFolderBtn.Click += selectFolderBtn_Click;
            // 
            // saveBtn
            // 
            resources.ApplyResources(saveBtn, "saveBtn");
            saveBtn.Name = "saveBtn";
            saveBtn.UseVisualStyleBackColor = true;
            saveBtn.Click += saveBtn_Click;
            // 
            // languageLbl
            // 
            resources.ApplyResources(languageLbl, "languageLbl");
            languageLbl.Name = "languageLbl";
            // 
            // languageCombo
            // 
            languageCombo.FormattingEnabled = true;
            resources.ApplyResources(languageCombo, "languageCombo");
            languageCombo.Name = "languageCombo";
            // 
            // label2
            // 
            resources.ApplyResources(label2, "label2");
            label2.Name = "label2";
            // 
            // F_OpenAfterSave
            // 
            resources.ApplyResources(F_OpenAfterSave, "F_OpenAfterSave");
            F_OpenAfterSave.Name = "F_OpenAfterSave";
            F_OpenAfterSave.UseVisualStyleBackColor = true;
            // 
            // AppSettingsForm
            // 
            resources.ApplyResources(this, "$this");
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(F_OpenAfterSave);
            Controls.Add(label2);
            Controls.Add(languageCombo);
            Controls.Add(languageLbl);
            Controls.Add(saveBtn);
            Controls.Add(selectFolderBtn);
            Controls.Add(attDestTb);
            Controls.Add(label1);
            Name = "AppSettingsForm";
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