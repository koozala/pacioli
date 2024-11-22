using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization.Metadata;
using System.Threading.Tasks;

namespace Pacioli.Updater.AutoUpdate
{
    public class PacioliRepository
    {
        private string GithubApiRelease = "https://api.github.com/repos/koozala/pacioli/releases/latest";
        private string ExecutableAsset = "paciolisetup.exe";

        public string VersionName { get; set; } = string.Empty;
        public string DownloadUrl { get; set; } = string.Empty;

        public string ShortVersionName
        {
            get
            {
                return VersionName.Substring(0, VersionName.LastIndexOf("."));
            }
        }

        public PacioliRepository()
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(GithubApiRelease);
                client.DefaultRequestHeaders.Add("User-Agent", "Pacioli-koozala");
                var result = client.GetAsync(GithubApiRelease).Result;
                var str = result.Content.ReadAsStringAsync().Result;
                var some = JsonNode.Parse(str);

                VersionName = some!["tag_name"]!.GetValue<string>();
                for (int i = 0; i < some!["assets"]!.AsArray().Count; i++)
                {
                    var assetName = some["assets"]![i]!["name"]!.ToString();
                    if (assetName == ExecutableAsset)
                    {
                        DownloadUrl = some["assets"]![i]!["browser_download_url"]!.ToString();
                    }
                }
            }
        }

        public void ExecuteSetup()
        {
            var tempPath = Path.Combine(Path.GetTempPath(), ExecutableAsset);
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("User-Agent", "Pacioli-koozala");
                var result = client.GetAsync(DownloadUrl).Result;
                using (FileStream fs = new FileStream(tempPath, FileMode.Create, FileAccess.Write))
                {
                    result.Content.ReadAsStream().CopyTo(fs);
                }
            }

            ProcessStartInfo processStartInfo = new ProcessStartInfo()
            {
                FileName = tempPath
            };
            Process process = Process.Start(processStartInfo);
        }
    }

}

