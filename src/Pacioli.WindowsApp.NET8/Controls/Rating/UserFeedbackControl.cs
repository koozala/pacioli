using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Pacioli.WindowsApp.NET8.Controls.Rating
{
    public partial class UserFeedbackControl : UserControl
    {

        public Action<int, string>? SubmitFeedbackAction = null;

        public UserFeedbackControl()
        {
            InitializeComponent();

            F_Anmerkungen.Focus();
        }

        public void Start()
        {
            F_Anmerkungen.Focus();
        }

        private void F_Senden_Click(object sender, EventArgs e)
        {
            if (SubmitFeedbackAction != null)
            {
                SubmitFeedbackAction(1, F_Anmerkungen.Text);
            }
        }

        private void F_NichtSenden_Click(object sender, EventArgs e)
        {
            if (SubmitFeedbackAction != null)
            {
                SubmitFeedbackAction(0, string.Empty);
            }
        }
    }
}
