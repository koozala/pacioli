using Pacioli.Updater.AutoUpdate;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pacioli.WindowsApp.NET8.Util
{
    public class UpdateCheck
    {
        public UpdateCheck() { }

        public void Execute(bool silent)
        {
            var repository = new PacioliRepository();
            if (VersionInformation.GetVersion() != repository.VersionName)
            {
                var result = MessageBox.Show($"Version {VersionInformation.GetVersion()} ist installiert. Eine aktuellere Version {repository.VersionName} von Pacioli ist verfügbar. Update durchführen?", "Neue Pacioli-Version verfügbar", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    repository.ExecuteSetup();
                    Application.Exit();
                    Process.GetCurrentProcess().Kill();
                }
            }
            else if (!silent)
            {
                MessageBox.Show($"Die aktuellste Version {VersionInformation.GetVersion()} ist installiert", "Keine neue Version", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
        }
    }
}
