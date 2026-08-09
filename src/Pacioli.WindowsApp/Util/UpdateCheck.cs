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

        private UpdateCheck()
        {
        }

        public static async Task<UpdateCheck> CreateAsync()
        {
            UpdateCheck updateCheck = new UpdateCheck();
            updateCheck.pacioliRepository = await PacioliRepository.CreateAsync();
            return updateCheck;
        }

        public async Task<bool> NewVersionAvailable()
        {
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
            if (await NewVersionAvailable())
            {
                var result = MessageBox.Show(string.Format(Resources.msgUpdateAsk, VersionInformation.GetVersion(), LatestVersion), Resources.msgUpdateAnnounce, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    await PerformUpdate();
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
