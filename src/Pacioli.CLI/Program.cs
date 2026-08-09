using Pacioli.CLI.Headless;

namespace Pacioli.CLI
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
