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
    public partial class InitialQuestion : UserControl
    {
        public enum Rating
        {
            Gut,
            Neutral,
            Schlecht
        }

        public Action<Rating>? OnRatingSelected = null;

        public InitialQuestion()
        {
            InitializeComponent();
        }

        private void F_Gut_Click(object sender, EventArgs e)
        {
            if (OnRatingSelected != null)
            {
                OnRatingSelected(Rating.Gut);
            }
        }

        private void F_Schlecht_Click(object sender, EventArgs e)
        {
            if (OnRatingSelected != null)
            {
                OnRatingSelected(Rating.Schlecht);
            }
        }

        private void F_Ohne_Click(object sender, EventArgs e)
        {
            if (OnRatingSelected != null)
            {
                OnRatingSelected(Rating.Neutral);
            }
        }
    }
}
