using Pacioli.Language.Resources;
using Pacioli.Updater.AutoUpdate;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace Pacioli.WindowsApp.Util
{
    public class UpdateCheck
    {
        PacioliRepository? pacioliRepository = null;

        public string LatestVersion
        {
            get
            {
                return pacioliRepository?.VersionName ?? string.Empty;
            }
        }

        /* True if the server named its version. */
        public bool LatestVersionKnown => pacioliRepository?.HasVersion ?? false;

        /* Names the reason if the version of the server stays unknown. */
        public string Error => pacioliRepository?.Error ?? string.Empty;

        private UpdateCheck()
        {
        }

        public static async Task<UpdateCheck> CreateAsync()
        {
            UpdateCheck updateCheck = new UpdateCheck();
            updateCheck.pacioliRepository = await PacioliRepository.CreateAsync();
            return updateCheck;
        }

        /*
         * True if the server offers a version above the version of this program.
         *
         * A server without an answer gives no new version. Thus the program keeps quiet if
         * it cannot reach the server, and it never offers an update that it cannot name.
         */
        public bool NewVersionAvailable()
        {
            if (!LatestVersionKnown)
            {
                return false;
            }

            return IsGreaterVersion(VersionInformation.GetVersion(), pacioliRepository!.VersionName);
        }

        public async Task PerformUpdate()
        {
            string executablePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads", PacioliRepository.ExecutableAsset);
            await pacioliRepository!.DownloadLatestVersionAsync(executablePath);
            var processStartInfo = new ProcessStartInfo
            {
                FileName = executablePath,
                UseShellExecute = false,
                CreateNoWindow = true
            };
            Process.Start(processStartInfo);
        }

        public async Task ExecuteAsync(bool silent)
        {
            try
            {
                await ExecuteGoAsync(silent);
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format(Resources.msgServerErrorText, ex.Message), Resources.error, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        async Task ExecuteGoAsync(bool silent)
        {
            if (NewVersionAvailable())
            {
                var result = MessageBox.Show(string.Format(Resources.msgUpdateAsk, VersionInformation.GetVersion(), LatestVersion), Resources.msgUpdateAnnounce, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    await PerformUpdate();
                }
            }
            else if (silent)
            {
                return;
            }
            else if (LatestVersionKnown)
            {
                MessageBox.Show(string.Format(Resources.msgCurrentInfo, VersionInformation.GetVersion()), Resources.msgCurrentAnnounce, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
            else
            {
                /*
                 * The server gave no version. The user asked for the check, thus the
                 * program names the problem. Before, the message told that the newest
                 * version is installed, and that was not true.
                 */
                MessageBox.Show(string.Format(Resources.msgServerErrorText, Error), Resources.error, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        bool IsGreaterVersion(string v1, string v2)
        {
            if (!ReadVersion(v1, out int major1, out int minor1, out int patch1))
            {
                return false;
            }

            if (!ReadVersion(v2, out int major2, out int minor2, out int patch2))
            {
                return false;
            }

            if (major2 > major1) return true;
            if (major2 == major1 && minor2 > minor1) return true;
            if (major2 == major1 && minor2 == minor1 && patch2 > patch1) return true;
            return false;
        }

        /*
         * Takes the three numbers out of a version, for example "v1.28.0". Gives false if
         * the text holds no version. Before, such a text gave an exception.
         */
        bool ReadVersion(string v, out int major, out int minor, out int patch)
        {
            major = 0;
            minor = 0;
            patch = 0;

            Regex r = new Regex(@"(\d+)\.(\d+)\.(\d+)");
            Match m = r.Match(v ?? string.Empty);

            if (!m.Success)
            {
                return false;
            }

            major = int.Parse(m.Groups[1].Value);
            minor = int.Parse(m.Groups[2].Value);
            patch = int.Parse(m.Groups[3].Value);
            return true;
        }
    }
}
