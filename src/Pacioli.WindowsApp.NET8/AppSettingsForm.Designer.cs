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
            saveBtn.Location = new Point(27, 176);
            saveBtn.Name = "saveBtn";
            saveBtn.Size = new Size(112, 34);
            saveBtn.TabIndex = 3;
            saveBtn.Text = "Speichern";
            saveBtn.UseVisualStyleBackColor = true;
            saveBtn.Click += saveBtn_Click;
            // 
            // AppSettingsForm
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(803, 269);
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
    }
}