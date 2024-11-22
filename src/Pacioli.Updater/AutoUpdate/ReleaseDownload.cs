using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Pacioli.Updater.AutoUpdate
{
    public class ReleaseDownload
    {
        string GithubApiReleaseUrl = "https://api.github.com/repos/koozala/pacioli/releases/latest/assets";

        public ReleaseDownload()
        {
        }

        public string[] DownloadLatestVersion()
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(GithubApiReleaseUrl);
                client.DefaultRequestHeaders.Add("User-Agent", "Pacioli-koozala");
                var result = client.GetAsync(GithubApiReleaseUrl).Result;
                var str = result.Content.ReadAsStringAsync().Result;
                Console.WriteLine(str);
                var some = JsonSerializer.Deserialize<List<AssetInfo>>(str);
                return some.Select(x => x.browser_download_url).ToArray();
            }
        }
    }

    public class AssetInfo
    {
        public string browser_download_url { get; set; } = string.Empty;
    }
}
