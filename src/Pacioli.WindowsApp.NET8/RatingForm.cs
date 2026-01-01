using Pacioli.WindowsApp.NET8.Controls.Rating;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Pacioli.WindowsApp.NET8
{
    public partial class RatingForm : Form
    {
        private InitialQuestion.Rating userRating = InitialQuestion.Rating.Neutral;
        private string userText = string.Empty;

        public RatingForm()
        {
            InitializeComponent();

            panel1.AutoSize = true;

            F_InitalQuestion.Visible = true;
            F_UserFeedbackControl.Visible = false;

            F_InitalQuestion.OnRatingSelected += F_InitalQuestion_OnRatingSelected;
            F_UserFeedbackControl.SubmitFeedbackAction += F_UserFeedbackControl_SubmitFeedbackAction;
        }

        private void F_InitalQuestion_OnRatingSelected(InitialQuestion.Rating rating)
        {
            userRating = rating;
            if (rating == InitialQuestion.Rating.Gut)
            {
                F_InitalQuestion.Visible = false;
                F_UserFeedbackControl.Visible = true;
                F_UserFeedbackControl.Start();
            }
            else if (rating == InitialQuestion.Rating.Schlecht)
            {
                F_InitalQuestion.Visible = false;
                F_UserFeedbackControl.Visible = true;
                F_UserFeedbackControl.Start();
            }
            else
            {
                this.Close();
            }
        }
        private void F_UserFeedbackControl_SubmitFeedbackAction(int action, string text)
        {
            if (action == 1)
            {
                // Submit feedback to API

                string apiUrl = "https://pacioliusage.edv-vogt.de/api/UserFeedback";

                FeedbackItem item = new FeedbackItem
                {
                    Rating = userRating == InitialQuestion.Rating.Gut ? 5 : 1,
                    FeedbackText = text
                };

                Task.Run(() =>
                {
                    try
                    {
                        using (HttpClient client = new HttpClient())
                        {
                            client.BaseAddress = new Uri(apiUrl);
                            client.DefaultRequestHeaders.Add("User-Agent", "Pacioli-koozala");

                            var content = JsonContent.Create(item);
                            var result = client.PostAsync(apiUrl, content).Result;
                        }
                    }
                    catch { }
                });

            }
            this.Close();
        }

    }
}


public class FeedbackItem
{
    public int Rating { get; set; }
    public string FeedbackText { get; set; }
}
