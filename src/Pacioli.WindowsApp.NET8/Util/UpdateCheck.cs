using Pacioli.Updater.AutoUpdate;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Pacioli.WindowsApp.NET8.Util
{
    public class UpdateCheck
    {
        public UpdateCheck() { }

        public void Execute(bool silent)
        {
            try
            {
                ExecuteGo(silent);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fehler beim Kontaktieren des Servers: {ex.Message}", "Fehler", MessageBoxButtons.OK);
            }
        }

        public void ExecuteGo(bool silent)
        {
            var repository = new PacioliRepository();
            if (IsGreaterVersion(VersionInformation.GetVersion(), repository.VersionName))
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

        bool IsGreaterVersion(string v1, string v2)
        {
            ReadVersion(v1, out int major1, out int minor1, out int patch1);
            ReadVersion(v2, out int major2, out int minor2, out int patch2);
            if (major2 > major1) return true;
            if (major2 == major1 && minor2 > minor1) return true;
            if (major2 == major1 && minor2 == minor1 && patch2 > patch1) return true;
            return false;
        }

        void ReadVersion(string v, out int major, out int minor, out int patch)
        {
            Regex r = new Regex(@"(\d+)\.(\d+)\.(\d+)");
            Match m = r.Match(v);
            major = int.Parse(m.Groups[1].Value);
            minor = int.Parse(m.Groups[2].Value);
            patch = int.Parse(m.Groups[3].Value);
        }
    }
}
