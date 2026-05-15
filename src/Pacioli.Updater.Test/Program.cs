using Pacioli.Updater.AutoUpdate;
using System.Reflection;

namespace Pacioli.Updater.Test
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var repo = await PacioliRepository.CreateAsync();
            await repo.ExecuteSetupAsync();

            //var version = repo.GetLatestVersion();
            //Console.WriteLine(version);

            //var assets = new ReleaseDownload().DownloadLatestVersion();

            //foreach (string d in assets)
            //{
            //    Console.WriteLine($"{d}");
            //}
        }
    }
}
