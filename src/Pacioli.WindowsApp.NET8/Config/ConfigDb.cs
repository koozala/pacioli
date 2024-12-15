using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pacioli.WindowsApp.NET8.Config
{
    internal class ConfigDb
    {
        string connectionString = "Data Source=pacioli.db";

        public ConfigDb()
        {
            try
            {
                var pref = ReadPreferences();
            }
            catch
            {
                DropTable();
            }
        }

        internal UserPreferences ReadPreferences()
        {
            CreateTable();
            using (var connection = new SqliteConnection(connectionString))
            {
                UserPreferences preferences = new UserPreferences();
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText =
                    @"
SELECT User, Folder, AttachmentFolder, Language, OpenAfterSave
FROM UserPreferences
WHERE User = $user
";
                command.Parameters.AddWithValue("$user", preferences.UserName);
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        preferences.DefaultFolder = (string)reader["Folder"];
                        preferences.AttachmentOutputFolder = (string)reader["AttachmentFolder"];
                        preferences.LanguageCode = (string)reader["Language"];
                        preferences.OpenAfterSave = (bool)reader["OpenAfterSave"];
                        var y = preferences.OpenAfterSave;
                        var x = reader["OpenAfterSave"];
                    }
                }
                return preferences;
            }
        }

        internal void SavePreferences(UserPreferences pref)
        {
            CreateTable();
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText =
                    @"
UPDATE UserPreferences
SET Folder = $folder, AttachmentFolder = $attachFlr, Language = $lang, OpenAfterSave = $openAfterSave
WHERE User = $user
";
                command.Parameters.AddWithValue("$user", pref.UserName);
                command.Parameters.AddWithValue("$folder", pref.DefaultFolder);
                command.Parameters.AddWithValue("$attachFlr", pref.AttachmentOutputFolder);
                command.Parameters.AddWithValue("$lang", pref.LanguageCode);
                command.Parameters.AddWithValue("$openAfterSave", pref.OpenAfterSave);
                var r = command.ExecuteNonQuery();
                if (r == 0)
                {
                    var command2 = connection.CreateCommand();
                    command2.CommandText =
                        @"
INSERT INTO UserPreferences
VALUES ($user, $folder, $attachFlr, $lang, $openAfterSave)
";
                    command2.Parameters.AddWithValue("$user", pref.UserName);
                    command2.Parameters.AddWithValue("$folder", pref.DefaultFolder);
                    command2.Parameters.AddWithValue("$attachFlr", pref.AttachmentOutputFolder);
                    command2.Parameters.AddWithValue("$lang", pref.LanguageCode);
                    command2.Parameters.AddWithValue("$openAfterSave", pref.OpenAfterSave);
                    command2.ExecuteNonQuery();
                }
            }
        }

        void CreateTable()
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText =
                    @"
CREATE TABLE IF NOT EXISTS UserPreferences (User varchar(100), Folder varchar(500), AttachmentFolder varchar(500), Language varchar(16), OpenAfterSave BOOLEAN)
";
                command.ExecuteNonQuery();
            }
        }

        void DropTable()
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText =
                    @"
DROP TABLE UserPreferences 
";
                command.ExecuteNonQuery();
            }
        }
    }
}
