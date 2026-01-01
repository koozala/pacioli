namespace Pacioli.WindowsApp.NET8
{
    partial class RatingForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RatingForm));
            panel1 = new Panel();
            F_UserFeedbackControl = new Pacioli.WindowsApp.NET8.Controls.Rating.UserFeedbackControl();
            F_InitalQuestion = new Pacioli.WindowsApp.NET8.Controls.Rating.InitialQuestion();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // panel1
            // 
            resources.ApplyResources(panel1, "panel1");
            panel1.Controls.Add(F_UserFeedbackControl);
            panel1.Controls.Add(F_InitalQuestion);
            panel1.Name = "panel1";
            // 
            // F_UserFeedbackControl
            // 
            resources.ApplyResources(F_UserFeedbackControl, "F_UserFeedbackControl");
            F_UserFeedbackControl.Name = "F_UserFeedbackControl";
            // 
            // F_InitalQuestion
            // 
            resources.ApplyResources(F_InitalQuestion, "F_InitalQuestion");
            F_InitalQuestion.Name = "F_InitalQuestion";
            // 
            // RatingForm
            // 
            resources.ApplyResources(this, "$this");
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(panel1);
            Name = "RatingForm";
            panel1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Panel panel1;
        private Controls.Rating.InitialQuestion F_InitalQuestion;
        private Controls.Rating.UserFeedbackControl F_UserFeedbackControl;
    }
}