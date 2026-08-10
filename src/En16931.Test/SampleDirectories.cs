using System.Text.Json;
using System.Text.Json.Serialization;

namespace En16931.Test;

/*
 * The directories that hold the sample documents.
 *
 * The values come from three sources. The order of precedence is:
 *   1. The NUnit test parameter SamplesDirectories
 *   2. The environment variable PACIOLI_SAMPLES_DIRS
 *   3. The file testsettings.json in the output directory
 * A semicolon separates the entries in a parameter and in an environment variable.
 *
 * The corpus is big, and it does not belong to the repository. Thus a directory that is
 * not on this computer is not an error: the test that needs it gives the result
 * "ignored". The unit tests with the fixtures always run.
 */
internal static class SampleDirectories
{
    public const string SettingsFileName = "testsettings.json";

    /* The directories that exist on this computer. */
    public static IReadOnlyList<string> Existing { get; } = Configured()
        .Where(Directory.Exists)
        .ToList();

    /* The directories of the settings, also the ones that are not on this computer. */
    public static IReadOnlyList<string> Configured()
    {
        string? fromParameter = TestContext.Parameters.Get("SamplesDirectories");
        if (!string.IsNullOrWhiteSpace(fromParameter))
        {
            return Split(fromParameter);
        }

        string? fromEnvironment = Environment.GetEnvironmentVariable("PACIOLI_SAMPLES_DIRS");
        if (!string.IsNullOrWhiteSpace(fromEnvironment))
        {
            return Split(fromEnvironment);
        }

        string path = Path.Combine(AppContext.BaseDirectory, SettingsFileName);
        if (!File.Exists(path))
        {
            return [];
        }

        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        var file = JsonSerializer.Deserialize<SettingsFile>(File.ReadAllText(path), options);

        return file?.SamplesDirectories?
            .Where(entry => !string.IsNullOrWhiteSpace(entry))
            .Select(entry => entry.Trim())
            .ToList() ?? [];
    }

    /* Each XML file of each directory that exists, without the excluded directories. */
    public static IReadOnlyList<string> XmlFiles(params string[] excludeDirectories)
    {
        var files = new List<string>();

        foreach (string directory in Existing)
        {
            files.AddRange(Directory
                .EnumerateFiles(directory, "*.xml", SearchOption.AllDirectories)
                .Where(file => !IsExcluded(file, directory, excludeDirectories)));
        }

        files.Sort(StringComparer.OrdinalIgnoreCase);
        return files;
    }

    /* The path of a file, relative to its samples directory. The test report shows it. */
    public static string ShortName(string file)
    {
        foreach (string directory in Existing)
        {
            if (file.StartsWith(directory, StringComparison.OrdinalIgnoreCase))
            {
                return Path.GetRelativePath(directory, file);
            }
        }

        return file;
    }

    private static bool IsExcluded(string file, string root, string[] excludeDirectories)
    {
        if (excludeDirectories.Length == 0)
        {
            return false;
        }

        string relative = Path.GetRelativePath(root, file);
        string[] parts = relative.Split(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);

        /* The last part is the file name. Look at the directory names only. */
        return parts.Take(parts.Length - 1).Any(part =>
            excludeDirectories.Contains(part, StringComparer.OrdinalIgnoreCase));
    }

    private static string[] Split(string value) => value
        .Split(';', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

    private sealed class SettingsFile
    {
        [JsonPropertyName("samplesDirectories")]
        public string[]? SamplesDirectories { get; set; }
    }
}
