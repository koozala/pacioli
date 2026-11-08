using Pacioli.Config.Persistence;
using Pacioli.Language.Resources;

namespace Pacioli.WindowsApp.Validation
{
    /*
     * The dialog where the user enters the API key of W System.
     *
     * The form has no designer file: it has a few controls only, and each text comes from
     * the resources of Pacioli.Language.
     */
    public sealed class ApiKeyForm : Form
    {
        readonly TextBox keyBox = new TextBox();
        readonly CheckBox showKey = new CheckBox();
        readonly Button saveButton = new Button();
        readonly Button removeButton = new Button();
        readonly Button cancelButton = new Button();

        readonly ConfigDb db;

        /*
         * Shows the dialog. Gives true if a key is in the database when the dialog closes
         * with "Save".
         */
        public static bool Edit(IWin32Window? owner)
        {
            using var form = new ApiKeyForm(new ConfigDb());
            return form.ShowDialog(owner) == DialogResult.OK;
        }

        ApiKeyForm(ConfigDb db)
        {
            this.db = db;

            Text = Resources.ApiKeyTitle;
            Font = SystemFonts.MessageBoxFont ?? Font;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            StartPosition = FormStartPosition.CenterParent;
            MinimizeBox = false;
            MaximizeBox = false;
            ShowInTaskbar = false;
            AutoSize = true;
            AutoSizeMode = AutoSizeMode.GrowAndShrink;
            Padding = new Padding(12);

            int textWidth = Font.Height * 32;

            var layout = new TableLayoutPanel
            {
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                ColumnCount = 1,
                Dock = DockStyle.Fill
            };

            var prompt = new Label
            {
                AutoSize = true,
                MaximumSize = new Size(textWidth, 0),
                Text = Resources.ApiKeyPrompt,
                Margin = new Padding(0, 0, 0, 8)
            };

            keyBox.Width = textWidth;
            keyBox.UseSystemPasswordChar = true;
            keyBox.Text = ApiKeyStore.Read(db) ?? string.Empty;
            keyBox.TextChanged += (sender, e) => UpdateButtons();

            showKey.AutoSize = true;
            showKey.Text = Resources.ApiKeyShow;
            showKey.CheckedChanged += (sender, e) => keyBox.UseSystemPasswordChar = !showKey.Checked;

            var privacy = new Label
            {
                AutoSize = true,
                MaximumSize = new Size(textWidth, 0),
                Text = Resources.ApiKeyPrivacy,
                ForeColor = SystemColors.GrayText,
                Margin = new Padding(0, 8, 0, 8)
            };

            var buttons = new FlowLayoutPanel
            {
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                FlowDirection = FlowDirection.RightToLeft,
                Dock = DockStyle.Fill,
                Margin = new Padding(0)
            };

            foreach (Button button in new[] { cancelButton, saveButton, removeButton })
            {
                button.AutoSize = true;
                button.MinimumSize = new Size(Font.Height * 6, 0);
                buttons.Controls.Add(button);
            }

            saveButton.Text = Resources.Speichern;
            saveButton.Click += SaveButton_Click;

            removeButton.Text = Resources.ApiKeyRemove;
            removeButton.Click += RemoveButton_Click;

            cancelButton.Text = Resources.Abbrechen;
            cancelButton.DialogResult = DialogResult.Cancel;

            AcceptButton = saveButton;
            CancelButton = cancelButton;

            layout.Controls.Add(prompt);
            layout.Controls.Add(keyBox);
            layout.Controls.Add(showKey);
            layout.Controls.Add(privacy);
            layout.Controls.Add(buttons);
            Controls.Add(layout);

            UpdateButtons();
        }

        void UpdateButtons()
        {
            saveButton.Enabled = !string.IsNullOrWhiteSpace(keyBox.Text);
            removeButton.Enabled = ApiKeyStore.Read(db) != null;
        }

        void SaveButton_Click(object? sender, EventArgs e)
        {
            if (Store(keyBox.Text))
            {
                DialogResult = DialogResult.OK;
            }
        }

        void RemoveButton_Click(object? sender, EventArgs e)
        {
            if (Store(null))
            {
                DialogResult = DialogResult.Cancel;
            }
        }

        /* Here the user asked for the step, thus an error gets a message. */
        bool Store(string? key)
        {
            try
            {
                ApiKeyStore.Save(db, key);
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, string.Format(Resources.msgApiKeyNotSaved, Environment.NewLine, ex.Message),
                    Resources.ApiKeyTitle, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
        }
    }
}
