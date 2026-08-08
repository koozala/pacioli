using System.Text;

namespace Pacioli.CLI.NET8.Test
{
    /*
     * Makes a quick check of a PDF file.
     *
     * The check does not read the full structure of the PDF. It finds an empty file,
     * a file that is not a PDF file, and a file that stops too early.
     */
    public static class PdfCheck
    {
        private const string Header = "%PDF-";
        private const string Trailer = "%%EOF";
        private const int TrailerSearchLength = 2048;

        /* Gives null if the file is good. If not, it gives the reason. */
        public static string? FindProblem(string path)
        {
            if (!File.Exists(path))
            {
                return "The CLI tool did not write the output file.";
            }

            var info = new FileInfo(path);
            if (info.Length == 0)
            {
                return "The output file is empty.";
            }

            using var stream = File.OpenRead(path);

            var headerBytes = new byte[Header.Length];
            if (stream.Read(headerBytes, 0, headerBytes.Length) < headerBytes.Length ||
                Encoding.ASCII.GetString(headerBytes) != Header)
            {
                return $"The output file does not start with \"{Header}\". Its size is {info.Length} bytes.";
            }

            int tailLength = (int)Math.Min(TrailerSearchLength, info.Length);
            stream.Seek(-tailLength, SeekOrigin.End);

            var tailBytes = new byte[tailLength];
            stream.ReadExactly(tailBytes, 0, tailLength);

            if (!Encoding.ASCII.GetString(tailBytes).Contains(Trailer, StringComparison.Ordinal))
            {
                return $"The output file has no \"{Trailer}\" mark at its end. The file is not complete. " +
                       $"Its size is {info.Length} bytes.";
            }

            return null;
        }
    }
}
