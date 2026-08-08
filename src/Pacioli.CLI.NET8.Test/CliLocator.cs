using System.Runtime.InteropServices;

namespace Pacioli.CLI.NET8.Test
{
    /*
     * Finds the CLI executable pacioli-cmd.
     *
     * The search starts at the output directory of the test. It goes up until it finds
     * the solution file Pacioli.sln. Then it looks in the build output of the CLI project.
     * If more than one executable exists, the search takes the newest file. The build
     * configuration of the test has priority.
     */
    public static class CliLocator
    {
        public const string ProjectDirectoryName = "Pacioli.CLI.NET8";
        public const string SolutionFileName = "Pacioli.sln";

        public static string ExecutableName =>
            RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? "pacioli-cmd.exe" : "pacioli-cmd";

        public static string? Find()
        {
            string? solutionDirectory = FindSolutionDirectory(AppContext.BaseDirectory);
            if (solutionDirectory == null)
            {
                return null;
            }

            string binDirectory = Path.Combine(solutionDirectory, ProjectDirectoryName, "bin");
            if (!Directory.Exists(binDirectory))
            {
                return null;
            }

            var candidates = Directory
                .EnumerateFiles(binDirectory, ExecutableName, SearchOption.AllDirectories)
                .Select(path => new FileInfo(path))
                .ToList();

            if (candidates.Count == 0)
            {
                return null;
            }

            string configuration = CurrentConfiguration();

            return candidates
                .OrderByDescending(file => ContainsDirectorySegment(file.FullName, configuration))
                .ThenByDescending(file => file.LastWriteTimeUtc)
                .First()
                .FullName;
        }

        /* Gives the directory that holds Pacioli.sln, or null if no such directory exists. */
        public static string? FindSolutionDirectory(string startDirectory)
        {
            DirectoryInfo? directory = new DirectoryInfo(startDirectory);

            while (directory != null)
            {
                if (File.Exists(Path.Combine(directory.FullName, SolutionFileName)))
                {
                    return directory.FullName;
                }
                directory = directory.Parent;
            }

            return null;
        }

        private static string CurrentConfiguration()
        {
#if DEBUG
            return "Debug";
#else
            return "Release";
#endif
        }

        private static bool ContainsDirectorySegment(string path, string segment)
        {
            return path
                .Split(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar)
                .Any(part => string.Equals(part, segment, StringComparison.OrdinalIgnoreCase));
        }
    }
}
