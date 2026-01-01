namespace Pacioli.WindowsApp.NET8.Controls.Rating
{
    partial class InitialQuestion
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(InitialQuestion));
            flowLayoutPanel1 = new FlowLayoutPanel();
            F_Gut = new Button();
            F_Schlecht = new Button();
            F_Ohne = new Button();
            flowLayoutPanel1.SuspendLayout();
            SuspendLayout();
            // 
            // flowLayoutPanel1
            // 
            flowLayoutPanel1.Controls.Add(F_Gut);
            flowLayoutPanel1.Controls.Add(F_Schlecht);
            flowLayoutPanel1.Controls.Add(F_Ohne);
            resources.ApplyResources(flowLayoutPanel1, "flowLayoutPanel1");
            flowLayoutPanel1.Name = "flowLayoutPanel1";
            // 
            // F_Gut
            // 
            resources.ApplyResources(F_Gut, "F_Gut");
            F_Gut.Name = "F_Gut";
            F_Gut.UseVisualStyleBackColor = true;
            F_Gut.Click += F_Gut_Click;
            // 
            // F_Schlecht
            // 
            resources.ApplyResources(F_Schlecht, "F_Schlecht");
            F_Schlecht.Name = "F_Schlecht";
            F_Schlecht.UseVisualStyleBackColor = true;
            F_Schlecht.Click += F_Schlecht_Click;
            // 
            // F_Ohne
            // 
            resources.ApplyResources(F_Ohne, "F_Ohne");
            F_Ohne.Name = "F_Ohne";
            F_Ohne.UseVisualStyleBackColor = true;
            F_Ohne.Click += F_Ohne_Click;
            // 
            // InitialQuestion
            // 
            resources.ApplyResources(this, "$this");
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(flowLayoutPanel1);
            Name = "InitialQuestion";
            flowLayoutPanel1.ResumeLayout(false);
            flowLayoutPanel1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private FlowLayoutPanel flowLayoutPanel1;
        private Button F_Gut;
        private Button F_Schlecht;
        private Button F_Ohne;
    }
}
