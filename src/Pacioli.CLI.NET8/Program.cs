using Pacioli.CLI.NET8.Headless;

namespace Pacioli.CLI.NET8
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var worker = new Worker(args);
            worker.Run();
        }
    }
}
