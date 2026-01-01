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
    public partial class HinweisFeld : UserControl
    {
        public HinweisFeld()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var ratingForm = new RatingForm();
            ratingForm.ShowDialog();
        }
    }
}
