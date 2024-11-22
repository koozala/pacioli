using Microsoft.Data.Sqlite;
using Pacioli.Config.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pacioli.Config.Persistence
{
    public class ConfigDb
    {
        string connectionString;

        public ConfigDb()
        {
            string mydoc = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            connectionString = $@"Data Source={mydoc}\pacioli.db";

            try
            {
                var pref = ReadPreferences();
            }
            catch
            {
                DropTable();
            }
        }

        public UserPreferences ReadPreferences()
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
                        preferences.OpenAfterSave = IntToBool((long)reader["OpenAfterSave"]);
                        var y = preferences.OpenAfterSave;
                        var x = reader["OpenAfterSave"];
                    }
                }
                return preferences;
            }
        }

        public void SavePreferences(UserPreferences pref)
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
                AddParameters(command, pref);
                var r = command.ExecuteNonQuery();
                if (r == 0)
                {
                    var command2 = connection.CreateCommand();
                    command2.CommandText =
                        @"
INSERT INTO UserPreferences
VALUES ($user, $folder, $attachFlr, $lang, $openAfterSave)
";
                    AddParameters(command2, pref);
                    r = command2.ExecuteNonQuery();
                }
            }
        }

        void AddParameters(SqliteCommand command, UserPreferences pref)
        {
            command.Parameters.AddWithValue("$user", pref.UserName);
            command.Parameters.AddWithValue("$folder", pref.DefaultFolder);
            command.Parameters.AddWithValue("$attachFlr", pref.AttachmentOutputFolder);
            command.Parameters.AddWithValue("$lang", pref.LanguageCode);
            command.Parameters.AddWithValue("$openAfterSave", pref.OpenAfterSave);
        }

        public void SaveProgramInfoVersionName(string versionName)
        {
            CreateTable_ProgramInfo();
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText =
                    @"
UPDATE ProgramInfo
SET VersionName = $name
";
                command.Parameters.AddWithValue("$name", versionName);
                var r = command.ExecuteNonQuery();
                if (r == 0)
                {
                    var command2 = connection.CreateCommand();
                    command2.CommandText =
                        @"
INSERT INTO ProgramInfo
VALUES ($name)
";
                    command.Parameters.AddWithValue("$name", versionName);
                    r = command2.ExecuteNonQuery();
                }
            }
        }

        public string ReadProgramInfoVersionName()
        {
            CreateTable_ProgramInfo();
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = @"SELECT VersionName FROM ProgramInfo";
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return reader.GetString(0);
                    }
                    else
                    {
                        return "noVersionAvailable";
                    }
                }
            }
        }

        bool IntToBool(long value)
        {
            return value != 0 ? true : false;
        }

        void CreateTable()
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText =
                    @"
CREATE TABLE IF NOT EXISTS UserPreferences (User TEXT, Folder TEXT, AttachmentFolder TEXT, Language TEXT, OpenAfterSave BOOLEAN)
";
                command.ExecuteNonQuery();
            }
        }

        void CreateTable_ProgramInfo()
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText =
                    @"
CREATE TABLE IF NOT EXISTS ProgramInfo (VersionName TEXT)
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
        void DropTable_ProgramInfo()
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText =
                    @"
DROP TABLE ProgramInfo
";
                command.ExecuteNonQuery();
            }
        }

    }
}
