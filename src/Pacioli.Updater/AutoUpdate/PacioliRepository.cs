using System.Diagnostics;
using System.Text.Json.Nodes;

namespace Pacioli.Updater.AutoUpdate
{
    /*
     * Tells which version the server offers, and loads the setup program.
     *
     * The server holds a small file at the side of the setup program. That file names the
     * version of the setup program:
     *
     *     { "version": "1.28.0" }
     *
     * Before, the class asked the API of GitHub for the newest release. That way had two
     * problems. GitHub counts the questions of one address and gives an error after 60
     * questions in one hour, thus the answer failed for a group of users behind one
     * address. And the answer of the API is large, because it names each file of the
     * release. The file version.json gives the answer without a limit and without an
     * account, and it comes from the same server as the setup program.
     *
     * The class gives no exception away. If the server gives no answer, the version stays
     * empty and Error names the reason. Thus a server that sleeps cannot stop the program.
     */
    public class PacioliRepository
    {
        public const string ExecutableAsset = "paciolisetup.exe";

        /* The file that names the version of the setup program on the server. */
        public const string VersionAsset = "version.json";

        /* The service gives a file of the server. The name of the file follows. */
        private const string DownloadService = "https://www.epacioli.de/FileDownload/Download?filename=";

        private static readonly TimeSpan Timeout = TimeSpan.FromSeconds(20.0);

        public string VersionName { get; private set; } = string.Empty;
        public string DownloadUrl { get; private set; } = string.Empty;

        /* Names the reason if the version of the server stays unknown. */
        public string? Error { get; private set; }

        /* True if the server named its version. */
        public bool HasVersion => VersionName.Length > 0;

        public string ShortVersionName
        {
            get
            {
                int last = VersionName.LastIndexOf('.');
                return last > 0 ? VersionName.Substring(0, last) : VersionName;
            }
        }

        private PacioliRepository() { }

        public static async Task<PacioliRepository> CreateAsync()
        {
            var repo = new PacioliRepository();
            repo.DownloadUrl = DownloadService + ExecutableAsset;

            try
            {
                using var client = new HttpClient { Timeout = Timeout };
                client.DefaultRequestHeaders.Add("User-Agent", "Pacioli-koozala");

                var response = await client.GetAsync(DownloadService + VersionAsset);
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();

                repo.VersionName = ReadVersion(json);

                if (!repo.HasVersion)
                {
                    repo.Error = $"{VersionAsset}: no version";
                }
            }
            catch (Exception ex)
            {
                repo.Error = ex.Message;
            }

            return repo;
        }

        /*
         * Takes the version out of the file.
         *
         * Gives an empty text if the file names no version. The result keeps the letter "v"
         * in front, because the program shows the version of its own assembly in that form,
         * and the two forms come together in one message.
         */
        private static string ReadVersion(string json)
        {
            var node = JsonNode.Parse(json);
            var value = node?["version"]?.ToString().Trim();

            if (string.IsNullOrEmpty(value))
            {
                return string.Empty;
            }

            return value.StartsWith("v", StringComparison.OrdinalIgnoreCase) ? value : "v" + value;
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
