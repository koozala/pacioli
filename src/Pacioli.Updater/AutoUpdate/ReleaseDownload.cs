using System.Text.Json;

namespace Pacioli.Updater.AutoUpdate
{
    public class ReleaseDownload
    {
        private const string GithubApiReleaseUrl = "https://api.github.com/repos/koozala/pacioli/releases/latest/assets";

        public async Task<string[]> DownloadLatestVersionAsync()
        {
            using var client = new HttpClient();
            client.DefaultRequestHeaders.Add("User-Agent", "Pacioli-koozala");
            var response = await client.GetAsync(GithubApiReleaseUrl);
            var json = await response.Content.ReadAsStringAsync();
            Console.WriteLine(json);
            var assets = JsonSerializer.Deserialize<List<AssetInfo>>(json);
            return assets!.Select(x => x.browser_download_url).ToArray();
        }
    }

    public class AssetInfo
    {
        public string browser_download_url { get; set; } = string.Empty;
    }
}
