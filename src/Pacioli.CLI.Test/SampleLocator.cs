namespace Pacioli.CLI.Test
{
    /*
     * Finds the sample documents in the samples directory.
     *
     * The locator looks in all subdirectories. It keeps the files that agree with one of the
     * include patterns. It removes the files that are in an excluded directory. The default
     * configuration excludes the directory "attachments", because these files are enclosures
     * of an invoice. They are not invoices.
     */
    public static class SampleLocator
    {
        public static IReadOnlyList<SampleFile> Find(TestSettings settings)
        {
            if (string.IsNullOrWhiteSpace(settings.SamplesDirectory) ||
                !Directory.Exists(settings.SamplesDirectory))
            {
                return Array.Empty<SampleFile>();
            }

            string root = Path.GetFullPath(settings.SamplesDirectory);

            var files = settings.IncludePatterns
                .SelectMany(pattern => Directory.EnumerateFiles(root, pattern, SearchOption.AllDirectories))
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .Where(path => !IsExcluded(root, path, settings.ExcludeDirectories))
                .OrderBy(path => path, StringComparer.OrdinalIgnoreCase)
                .ToList();

            return files
                .Select((path, position) => new SampleFile(
                    position + 1,
                    path,
                    Path.GetRelativePath(root, path)))
                .ToList();
        }

        private static bool IsExcluded(string root, string path, IReadOnlyList<string> excludedDirectories)
        {
            string relativeDirectory = Path.GetDirectoryName(Path.GetRelativePath(root, path)) ?? string.Empty;

            var segments = relativeDirectory
                .Split(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar,
                       StringSplitOptions.RemoveEmptyEntries);

            return segments.Any(segment =>
                excludedDirectories.Any(excluded =>
                    string.Equals(segment, excluded, StringComparison.OrdinalIgnoreCase)));
        }
    }
}
