using Pacioli.Config.Persistence;
using System.Security.Cryptography;
using System.Text;

namespace Pacioli.WindowsApp.Validation
{
    /*
     * The API key of W System, in the file pacioli.db.
     *
     * The file is in the documents folder of the user, and that folder often goes to a
     * cloud store. Thus the key is not in the file as plain text: Windows encrypts it for
     * the current user (DPAPI). An other user, or the same file on an other computer,
     * cannot read the key. The user must then enter the key again.
     */
    public static class ApiKeyStore
    {
        const string Service = "wsystem";

        /* Marks a key that DPAPI encrypted. A value without it is plain text. */
        const string ProtectedPrefix = "dpapi:";

        static readonly byte[] Entropy = Encoding.UTF8.GetBytes("Pacioli.WSystem.ApiKey");

        /* The key, or null if no key is there or if the key cannot be decrypted. */
        public static string? Read(ConfigDb db)
        {
            string? stored = db.ReadApiKey(Service);
            if (stored == null)
            {
                return null;
            }

            if (!stored.StartsWith(ProtectedPrefix, StringComparison.Ordinal))
            {
                /* A person wrote the key into the file by hand. */
                return stored.Trim();
            }

            try
            {
                byte[] data = Convert.FromBase64String(stored.Substring(ProtectedPrefix.Length));
                byte[] plain = ProtectedData.Unprotect(data, Entropy, DataProtectionScope.CurrentUser);
                return Encoding.UTF8.GetString(plain);
            }
            catch (Exception ex) when (ex is CryptographicException || ex is FormatException)
            {
                return null;
            }
        }

        /* Keeps the key. An empty key removes it. The method raises the error of the database. */
        public static void Save(ConfigDb db, string? apiKey)
        {
            string? key = apiKey?.Trim();
            if (string.IsNullOrEmpty(key))
            {
                db.SaveApiKey(Service, null);
                return;
            }

            byte[] data = ProtectedData.Protect(Encoding.UTF8.GetBytes(key), Entropy, DataProtectionScope.CurrentUser);
            db.SaveApiKey(Service, ProtectedPrefix + Convert.ToBase64String(data));
        }
    }
}
