namespace Pacioli.CLI.Test
{
    /*
     * Finds the sample documents in the samples directories.
     *
     * The locator looks in all subdirectories of each directory. It keeps the files that
     * agree with one of the include patterns. It removes the files that are in an excluded
     * directory. The default configuration excludes the directory "attachments", because
     * these files are enclosures of an invoice. They are not invoices.
     *
     * A test run can hold more than one directory. Then the name of a sample starts with
     * the name of its directory, thus two samples with the same relative path stay apart.
     */
    public static class SampleLocator
    {
        public static IReadOnlyList<SampleFile> Find(TestSettings settings)
        {
            var roots = settings.SamplesDirectories
                .Where(entry => !string.IsNullOrWhiteSpace(entry) && Directory.Exists(entry))
                .Select(Path.GetFullPath)
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToList();

            if (roots.Count == 0)
            {
                return Array.Empty<SampleFile>();
            }

            var found = new List<(string Path, string Name)>();

            foreach (string root in roots)
            {
                string prefix = roots.Count > 1 ? Path.GetFileName(root.TrimEnd(Path.DirectorySeparatorChar)) : string.Empty;

                var files = settings.IncludePatterns
                    .SelectMany(pattern => Directory.EnumerateFiles(root, pattern, SearchOption.AllDirectories))
                    .Distinct(StringComparer.OrdinalIgnoreCase)
                    .Where(path => !IsExcluded(root, path, settings.ExcludeDirectories))
                    .Select(path => (
                        Path: path,
                        Name: prefix.Length == 0
                            ? Path.GetRelativePath(root, path)
                            : Path.Combine(prefix, Path.GetRelativePath(root, path))));

                found.AddRange(files);
            }

            return found
                .OrderBy(entry => entry.Name, StringComparer.OrdinalIgnoreCase)
                .Select((entry, position) => new SampleFile(position + 1, entry.Path, entry.Name))
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
