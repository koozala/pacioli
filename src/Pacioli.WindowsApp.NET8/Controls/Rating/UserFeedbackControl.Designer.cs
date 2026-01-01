namespace Pacioli.WindowsApp.NET8.Controls.Rating
{
    partial class UserFeedbackControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UserFeedbackControl));
            label1 = new Label();
            F_Senden = new Button();
            F_Anmerkungen = new TextBox();
            SuspendLayout();
            // 
            // label1
            // 
            resources.ApplyResources(label1, "label1");
            label1.Name = "label1";
            // 
            // F_Senden
            // 
            resources.ApplyResources(F_Senden, "F_Senden");
            F_Senden.Name = "F_Senden";
            F_Senden.UseVisualStyleBackColor = true;
            F_Senden.Click += F_Senden_Click;
            // 
            // F_Anmerkungen
            // 
            resources.ApplyResources(F_Anmerkungen, "F_Anmerkungen");
            F_Anmerkungen.Name = "F_Anmerkungen";
            // 
            // UserFeedbackControl
            // 
            resources.ApplyResources(this, "$this");
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(F_Anmerkungen);
            Controls.Add(F_Senden);
            Controls.Add(label1);
            Name = "UserFeedbackControl";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private Button F_Senden;
        private TextBox F_Anmerkungen;
    }
}
