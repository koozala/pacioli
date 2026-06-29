using System.Diagnostics;
using System.Text.Json.Nodes;

namespace Pacioli.Updater.AutoUpdate
{
    public class PacioliRepository
    {
        private const string GithubApiRelease = "https://api.github.com/repos/koozala/pacioli/releases/latest";
        public const string ExecutableAsset = "paciolisetup.exe";

        public string VersionName { get; private set; } = string.Empty;
        public string DownloadUrl { get; private set; } = string.Empty;

        public string ShortVersionName => VersionName.Substring(0, VersionName.LastIndexOf("."));

        private PacioliRepository() { }

        public static async Task<PacioliRepository> CreateAsync()
        {
            var repo = new PacioliRepository();
            using var client = new HttpClient();
            client.DefaultRequestHeaders.Add("User-Agent", "Pacioli-koozala");
            var response = await client.GetAsync(GithubApiRelease);
            var json = await response.Content.ReadAsStringAsync();
            var node = JsonNode.Parse(json);

            repo.VersionName = node!["tag_name"]!.GetValue<string>();
            var assets = node!["assets"]!.AsArray();
            for (int i = 0; i < assets.Count; i++)
            {
                if (assets[i]!["name"]!.ToString() == ExecutableAsset)
                {
                    //repo.DownloadUrl = assets[i]!["browser_download_url"]!.ToString();
                    repo.DownloadUrl = "https://www.epacioli.de/FileDownload/Download?filename=paciolisetup.exe";
                }
            }
            return repo;
        }

        public async Task DownloadLatestVersionAsync(string destinationPath)
        {
            using var client = new HttpClient();
            var response = await client.GetAsync(DownloadUrl);
            response.EnsureSuccessStatusCode();
            using var fileStream = new FileStream(destinationPath, FileMode.Create, FileAccess.Write, FileShare.None);
            await response.Content.CopyToAsync(fileStream);
        }

        public async Task ExecuteSetupAsync()
        {
            var tempPath = Path.Combine(Path.GetTempPath(), ExecutableAsset);
            using var client = new HttpClient();
            client.DefaultRequestHeaders.Add("User-Agent", "Pacioli-koozala");
            var response = await client.GetAsync(DownloadUrl);
            using var fs = new FileStream(tempPath, FileMode.Create, FileAccess.Write);
            await response.Content.CopyToAsync(fs);

            Process.Start(new ProcessStartInfo { FileName = tempPath });
        }
    }
}
