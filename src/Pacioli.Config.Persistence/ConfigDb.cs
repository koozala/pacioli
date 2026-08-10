using Microsoft.Data.Sqlite;
using System;
using System.IO;

namespace Pacioli.Config.Persistence
{
    /*
     * The preferences of the user, in a SQLite file beside the documents of the user.
     *
     * READING NEVER FAILS. If the file is not there, if an other program holds it, if the
     * disk gives an error, or if a value has the wrong form, ReadPreferences gives the
     * default preferences. A user who cannot read the settings must still be able to work
     * with the program: the settings are a comfort, not a condition.
     *
     * WRITING FAILS LOUDLY. SavePreferences raises its exception, because a user who asks
     * the program to keep a setting must learn that it did not happen. A caller that only
     * keeps a folder on the side uses TrySavePreferences.
     */
    public class ConfigDb
    {
        readonly string connectionString;

        /* The name of the file, for a message and for a test. */
        public string DatabasePath { get; }

        /*
         * True if the last call of ReadPreferences did not reach the database and gave the
         * default preferences. The application can show a note.
         */
        public bool UsedDefaults { get; private set; }

        /* The reason why the last read gave the default preferences, or null. */
        public Exception? LastReadError { get; private set; }

        public ConfigDb()
            : this(Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                "pacioli.db"))
        {
        }

        /* Takes the path of the database file. A test gives a file of its own. */
        public ConfigDb(string databasePath)
        {
            DatabasePath = databasePath;
            connectionString = $"Data Source={databasePath}";

            RepairAnOldTable();
        }

        /*
         * An earlier version of Pacioli wrote the table with other columns. A read then
         * gives an error of SQLite, for example "no such column". The table holds nothing
         * that the program can use, thus it goes away and CreateTable makes it new.
         *
         * Only an error of SQLite about the form of the table leads to this step. A file
         * that an other program holds, or a disk without a right to write, must keep the
         * settings of the user: such an error goes away by itself, and the settings must
         * not go away with it.
         */
        void RepairAnOldTable()
        {
            try
            {
                ReadPreferencesOrThrow();
            }
            catch (SqliteException)
            {
                try
                {
                    DropTable();
                }
                catch (Exception)
                {
                    /* The table stays as it is. Each read then gives the defaults. */
                }
            }
            catch (Exception)
            {
                /* Not a question of the form of the table. Keep the table. */
            }
        }

        /*
         * The preferences of the user, or the default preferences if the database does not
         * answer. The method raises no exception.
         */
        public UserPreferences ReadPreferences()
        {
            try
            {
                UserPreferences preferences = ReadPreferencesOrThrow();
                UsedDefaults = false;
                LastReadError = null;
                return preferences;
            }
            catch (Exception error)
            {
                UsedDefaults = true;
                LastReadError = error;
                return new UserPreferences();
            }
        }

        UserPreferences ReadPreferencesOrThrow()
        {
            CreateTable();

            using (var connection = new SqliteConnection(connectionString))
            {
                UserPreferences preferences = new UserPreferences();
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText =
                    @"
SELECT User, Folder, AttachmentFolder, Language, OpenAfterSave, PerformUpdateCheck, SendRatingData
FROM UserPreferences
WHERE User = $user
";
                command.Parameters.AddWithValue("$user", preferences.UserName);
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        /*
                         * Each value keeps the default of UserPreferences if the column is
                         * empty or holds an other type. Thus one bad column does not take
                         * the other settings with it.
                         */
                        preferences.DefaultFolder = Text(reader, "Folder", preferences.DefaultFolder);
                        preferences.AttachmentOutputFolder =
                            Text(reader, "AttachmentFolder", preferences.AttachmentOutputFolder);
                        preferences.LanguageCode = Text(reader, "Language", preferences.LanguageCode);
                        preferences.OpenAfterSave = Bool(reader, "OpenAfterSave", preferences.OpenAfterSave);
                        preferences.PerformUpdateCheck =
                            Bool(reader, "PerformUpdateCheck", preferences.PerformUpdateCheck);
                        preferences.SendRatingData = Bool(reader, "SendRatingData", preferences.SendRatingData);
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
SET Folder = $folder, AttachmentFolder = $attachFlr, Language = $lang, OpenAfterSave = $openAfterSave, PerformUpdateCheck = $performUpdateCheck, SendRatingData = $sendRatingData
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
VALUES ($user, $folder, $attachFlr, $lang, $openAfterSave, $performUpdateCheck, $sendRatingData)
";
                    AddParameters(command2, pref);
                    r = command2.ExecuteNonQuery();
                }
            }
        }

        /*
         * Keeps the preferences, and gives false if the database does not take them.
         *
         * Use this method where the program keeps a value on the side, for example the
         * folder of the last save. There the work of the user must go on, also without the
         * database. Use SavePreferences where the user asked for the step.
         */
        public bool TrySavePreferences(UserPreferences pref)
        {
            try
            {
                SavePreferences(pref);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        void AddParameters(SqliteCommand command, UserPreferences pref)
        {
            command.Parameters.AddWithValue("$user", pref.UserName);
            command.Parameters.AddWithValue("$folder", pref.DefaultFolder);
            command.Parameters.AddWithValue("$attachFlr", pref.AttachmentOutputFolder);
            command.Parameters.AddWithValue("$lang", pref.LanguageCode);
            command.Parameters.AddWithValue("$openAfterSave", pref.OpenAfterSave);
            command.Parameters.AddWithValue("$performUpdateCheck", pref.PerformUpdateCheck);
            command.Parameters.AddWithValue("$sendRatingData", pref.SendRatingData);
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
                    command2.Parameters.AddWithValue("$name", versionName);
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

        /* The text of a column, or the given value if the column is empty or of an other type. */
        static string Text(SqliteDataReader reader, string column, string fallback)
        {
            try
            {
                int position = reader.GetOrdinal(column);
                if (reader.IsDBNull(position))
                {
                    return fallback;
                }

                string value = reader.GetString(position);
                return string.IsNullOrWhiteSpace(value) ? fallback : value;
            }
            catch (Exception)
            {
                return fallback;
            }
        }

        /*
         * The true or false value of a column.
         *
         * SQLite has no type for true and false. A version of Pacioli wrote a number, and
         * an other one can write a text. Thus the method takes both forms.
         */
        static bool Bool(SqliteDataReader reader, string column, bool fallback)
        {
            try
            {
                int position = reader.GetOrdinal(column);
                if (reader.IsDBNull(position))
                {
                    return fallback;
                }

                object value = reader.GetValue(position);

                return value switch
                {
                    long number => number != 0,
                    int number => number != 0,
                    bool flag => flag,
                    string text => bool.TryParse(text, out bool parsed)
                        ? parsed
                        : long.TryParse(text, out long number) ? number != 0 : fallback,
                    _ => fallback
                };
            }
            catch (Exception)
            {
                return fallback;
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
CREATE TABLE IF NOT EXISTS UserPreferences (User TEXT, Folder TEXT, AttachmentFolder TEXT, Language TEXT, OpenAfterSave BOOLEAN, PerformUpdateCheck BOOLEAN, SendRatingData BOOLEAN)
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
    }
}
