using Pacioli.Language.Resources;
using Pacioli.Pdf.ERechnung;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Pacioli.WindowsApp.NET8.Util
{
    /*
     * Writes an attachment of an invoice to the download folder of the user, and opens
     * it with the program for its file type.
     *
     * An invoice can hold a document as a reference only, without the data of the file.
     * Such an attachment has no content, and this class cannot write it. The caller must
     * test HasContent before it calls Save.
     */
    internal static class AttachmentStore
    {
        /* FOLDERID_Downloads. Windows has no special folder for it in .NET. */
        private static readonly Guid DownloadsFolderId = new Guid("374DE290-123F-4565-9164-39C4925E467B");

        [DllImport("shell32.dll", CharSet = CharSet.Unicode)]
        private static extern int SHGetKnownFolderPath(ref Guid folderId, uint flags, IntPtr token, out IntPtr path);

        /* Tells if the attachment holds the data of a file. */
        public static bool HasContent(AttachmentDescriptor attachment)
        {
            return attachment.Data != null && attachment.Data.Length > 0;
        }

        /*
         * The download folder of the user.
         *
         * Windows gives the folder, because the user can move it. If Windows gives no
         * answer, the method takes the usual place below the user profile, and then the
         * folder for temporary files.
         */
        public static string GetDownloadFolder()
        {
            try
            {
                Guid id = DownloadsFolderId;
                if (SHGetKnownFolderPath(ref id, 0, IntPtr.Zero, out IntPtr result) == 0)
                {
                    try
                    {
                        string? path = Marshal.PtrToStringUni(result);
                        if (!string.IsNullOrWhiteSpace(path) && Directory.Exists(path))
                        {
                            return path!;
                        }
                    }
                    finally
                    {
                        Marshal.FreeCoTaskMem(result);
                    }
                }
            }
            catch (DllNotFoundException)
            {
                /* Not Windows. The method takes the folders below. */
            }
            catch (EntryPointNotFoundException)
            {
            }

            string profile = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads");

            return Directory.Exists(profile) ? profile : Path.GetTempPath();
        }

        /*
         * Writes the attachment to the download folder, and gives the path of the file.
         *
         * The method does not write over a file that exists. It puts a number in the
         * name, as a browser does: "Aufmass (2).pdf".
         */
        public static string Save(AttachmentDescriptor attachment)
        {
            if (!HasContent(attachment))
            {
                throw new InvalidOperationException(Resources.NurVerweis);
            }

            string folder = GetDownloadFolder();
            Directory.CreateDirectory(folder);

            string target = MakeUniquePath(folder, BuildFileName(attachment));
            File.WriteAllBytes(target, attachment.Data!);

            return target;
        }

        /* Opens a file with the program for its file type. */
        public static void Open(string path)
        {
            Process.Start(new ProcessStartInfo(path) { UseShellExecute = true });
        }

        /*
         * Builds the name of the file.
         *
         * The invoice gives the name of the file, or the name of the document, or
         * nothing. The method also gives the name an ending, because a program for the
         * file type comes from the ending.
         */
        public static string BuildFileName(AttachmentDescriptor attachment)
        {
            string name = FirstWithText(attachment.FileName, attachment.Name, Resources.Anlage);
            name = RemoveBadCharacters(name);

            if (name.Length == 0)
            {
                name = Resources.Anlage;
            }

            if (Path.GetExtension(name).Length == 0)
            {
                name += ExtensionFor(attachment.MimeType);
            }

            return name;
        }

        /* The usual ending of a file of this media type. */
        public static string ExtensionFor(string? mimeType)
        {
            switch (mimeType?.Trim().ToLowerInvariant())
            {
                case "application/pdf":
                    return ".pdf";
                case "text/xml":
                case "application/xml":
                    return ".xml";
                case "image/png":
                    return ".png";
                case "image/jpeg":
                    return ".jpg";
                case "text/csv":
                    return ".csv";
                case "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet":
                    return ".xlsx";
                case "application/vnd.oasis.opendocument.spreadsheet":
                    return ".ods";
                default:
                    return ".bin";
            }
        }

        private static string MakeUniquePath(string folder, string fileName)
        {
            string candidate = Path.Combine(folder, fileName);
            if (!File.Exists(candidate))
            {
                return candidate;
            }

            string stem = Path.GetFileNameWithoutExtension(fileName);
            string extension = Path.GetExtension(fileName);

            for (int number = 2; number < 1000; number++)
            {
                candidate = Path.Combine(folder, $"{stem} ({number}){extension}");
                if (!File.Exists(candidate))
                {
                    return candidate;
                }
            }

            return Path.Combine(folder, $"{stem} {Guid.NewGuid():N}{extension}");
        }

        private static string FirstWithText(params string?[] values)
        {
            foreach (string? value in values)
            {
                if (!string.IsNullOrWhiteSpace(value))
                {
                    return value!.Trim();
                }
            }

            return string.Empty;
        }

        /* Removes the characters that a file name must not hold. */
        private static string RemoveBadCharacters(string name)
        {
            foreach (char bad in Path.GetInvalidFileNameChars())
            {
                name = name.Replace(bad, '_');
            }

            return name.Trim();
        }
    }
}
