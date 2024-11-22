using Pacioli.Updater.AutoUpdate;
using System.Reflection;

namespace Pacioli.Updater.Test
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var repo = new PacioliRepository();
            repo.ExecuteSetup();

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
