using System;
using System.IO;
using System.Reflection;
using System.Text.Json.Nodes;

namespace Pacioli.WindowsApp.Util
{
    /*
     * Names the version of this program.
     *
     * The version sits in the file version.json at the side of the program:
     *
     *     { "version": "1.28.0" }
     *
     * The build puts that file into the output folder, and the installation puts it into
     * the folder of the program. Thus one file names the version, and the deployment
     * writes the same number into the version.json of the server. The two files then
     * carry the same form of the number, and the check for a new version compares like
     * with like.
     *
     * If the file is absent or holds no version, the class takes the version of the
     * assembly. Thus the program names a version at all times.
     */
    public class VersionInformation
    {
        public const string VersionFile = "version.json";

        /* The file does not change while the program runs, thus one reading is enough. */
        private static string? version;

        public static string GetVersion()
        {
            return version ??= ReadVersion() ?? AssemblyVersion();
        }

        /*
         * Reads the version out of the file at the side of the program. Gives null if the
         * file is absent, if it cannot be read, or if it names no version.
         */
        private static string? ReadVersion()
        {
            try
            {
                string path = Path.Combine(AppContext.BaseDirectory, VersionFile);

                if (!File.Exists(path))
                {
                    return null;
                }

                var node = JsonNode.Parse(File.ReadAllText(path));
                string? value = node?["version"]?.ToString().Trim();

                if (string.IsNullOrEmpty(value))
                {
                    return null;
                }

                return value.StartsWith("v", StringComparison.OrdinalIgnoreCase) ? value : "v" + value;
            }
            catch (Exception)
            {
                return null;
            }
        }

        private static string AssemblyVersion()
        {
            Version? assembly = Assembly.GetExecutingAssembly().GetName().Version;
            return $"v{assembly!.Major}.{assembly.Minor}.{assembly.Build}";
        }
    }
}
