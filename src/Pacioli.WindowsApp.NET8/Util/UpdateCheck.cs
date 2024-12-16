using Pacioli.Language.Resources;
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
                MessageBox.Show(string.Format(Resources.msgServerErrorText, ex.Message), Resources.error, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void ExecuteGo(bool silent)
        {
            var repository = new PacioliRepository();
            if (IsGreaterVersion(VersionInformation.GetVersion(), repository.VersionName))
            {
                var result = MessageBox.Show(string.Format(Resources.msgUpdateAsk, VersionInformation.GetVersion(), repository.VersionName), Resources.msgUpdateAnnounce, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    repository.ExecuteSetup();
                    Application.Exit();
                    Process.GetCurrentProcess().Kill();
                }
            }
            else if (!silent)
            {
                MessageBox.Show(string.Format(Resources.msgCurrentInfo, VersionInformation.GetVersion()), Resources.msgCurrentAnnounce, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
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
