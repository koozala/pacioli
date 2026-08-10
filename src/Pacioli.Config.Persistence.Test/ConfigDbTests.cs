using Microsoft.Data.Sqlite;

namespace Pacioli.Config.Persistence.Test
{
    /*
     * The preferences of the user.
     *
     * The tests use a database file of their own, thus they do not touch the file of the
     * person that runs them.
     */
    public class ConfigDbTests
    {
        string folder = string.Empty;
        string databasePath = string.Empty;

        [SetUp]
        public void Setup()
        {
            folder = Path.Combine(Path.GetTempPath(), "Pacioli.Config.Test", Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(folder);
            databasePath = Path.Combine(folder, "pacioli.db");
        }

        [TearDown]
        public void RemoveTheDatabase()
        {
            SqliteConnection.ClearAllPools();

            try
            {
                Directory.Delete(folder, recursive: true);
            }
            catch (IOException)
            {
                /* A file that stays on the disk must not make the test run fail. */
            }
        }

        [Test]
        public void ANewDatabaseGivesTheDefaultPreferences()
        {
            var db = new ConfigDb(databasePath);

            var preferences = db.ReadPreferences();

            Assert.That(db.UsedDefaults, Is.False, "A new file is not an error.");
            Assert.That(preferences.OpenAfterSave, Is.True);
            Assert.That(preferences.UserName, Is.EqualTo(Environment.UserName));
        }

        [Test]
        public void APreferenceComesBackAfterASave()
        {
            var db = new ConfigDb(databasePath);

            var preferences = db.ReadPreferences();
            preferences.OpenAfterSave = false;
            preferences.AttachmentOutputFolder = folder;
            db.SavePreferences(preferences);

            var read = new ConfigDb(databasePath).ReadPreferences();

            Assert.That(read.OpenAfterSave, Is.False);
            Assert.That(read.AttachmentOutputFolder, Is.EqualTo(folder));
        }

        /*
         * The defect that this test guards: the program could not read the preferences,
         * and the save of a PDF file then stopped with an exception. The reading now gives
         * the default preferences.
         */
        [Test]
        public void ADatabaseThatTheProgramCannotReadGivesTheDefaultPreferences()
        {
            /* A directory in the place of the file: no connection can open it. */
            string path = Path.Combine(folder, "a-directory.db");
            Directory.CreateDirectory(path);

            var db = new ConfigDb(path);

            UserPreferences preferences = db.ReadPreferences();

            Assert.That(db.UsedDefaults, Is.True);
            Assert.That(db.LastReadError, Is.Not.Null);
            Assert.That(preferences.OpenAfterSave, Is.True);
            Assert.That(preferences.AttachmentOutputFolder, Is.Not.Empty);
        }

        [Test]
        public void AFileThatIsNotADatabaseGivesTheDefaultPreferences()
        {
            File.WriteAllText(databasePath, "This is not a database.");

            var db = new ConfigDb(databasePath);

            Assert.That(db.ReadPreferences().OpenAfterSave, Is.True);
            Assert.That(db.UsedDefaults, Is.True);
        }

        /* The constructor must not raise an exception either. */
        [Test]
        public void TheConstructorTakesADatabaseThatItCannotRead()
        {
            string path = Path.Combine(folder, "an-other-directory.db");
            Directory.CreateDirectory(path);

            Assert.DoesNotThrow(new TestDelegate(() => new ConfigDb(path).ReadPreferences()));
        }

        /* TrySavePreferences gives false, and it raises no exception. */
        [Test]
        public void TrySavePreferencesGivesFalseForADatabaseThatDoesNotTakeIt()
        {
            string path = Path.Combine(folder, "one-more-directory.db");
            Directory.CreateDirectory(path);

            var db = new ConfigDb(path);

            Assert.That(db.TrySavePreferences(new UserPreferences()), Is.False);
        }

        [Test]
        public void TrySavePreferencesGivesTrueForAGoodDatabase()
        {
            var db = new ConfigDb(databasePath);

            Assert.That(db.TrySavePreferences(db.ReadPreferences()), Is.True);
        }

        /*
         * One column with an empty value must not take the other settings with it. An
         * earlier version of Pacioli wrote such a row.
         */
        [Test]
        public void AColumnWithoutAValueKeepsItsDefaultAndTheOtherColumnsStay()
        {
            var db = new ConfigDb(databasePath);
            var preferences = db.ReadPreferences();
            preferences.AttachmentOutputFolder = folder;
            db.SavePreferences(preferences);

            Execute(databasePath,
                "UPDATE UserPreferences SET Folder = NULL, OpenAfterSave = NULL");

            var second = new ConfigDb(databasePath);
            var read = second.ReadPreferences();

            Assert.That(second.UsedDefaults, Is.False);
            Assert.That(read.DefaultFolder, Is.Not.Empty, "The empty column takes its default.");
            Assert.That(read.OpenAfterSave, Is.True, "The empty column takes its default.");
            Assert.That(read.AttachmentOutputFolder, Is.EqualTo(folder), "The other column stays.");
        }

        /*
         * SQLite has no type for true and false. A version of Pacioli can write a text,
         * thus the reader must take both forms.
         */
        [TestCase("0", false)]
        [TestCase("1", true)]
        [TestCase("False", false)]
        [TestCase("True", true)]
        public void ATrueOrFalseValueReadsAsANumberAndAsAText(string written, bool expected)
        {
            var db = new ConfigDb(databasePath);
            db.SavePreferences(db.ReadPreferences());

            Execute(databasePath, $"UPDATE UserPreferences SET OpenAfterSave = '{written}'");

            Assert.That(new ConfigDb(databasePath).ReadPreferences().OpenAfterSave, Is.EqualTo(expected));
        }

        /*
         * A table of an earlier version has other columns. The constructor makes it new,
         * because it holds nothing that this version can use.
         */
        [Test]
        public void ATableOfAnEarlierVersionGoesAway()
        {
            Execute(databasePath, "CREATE TABLE UserPreferences (User TEXT, SomethingElse TEXT)");
            Execute(databasePath, "INSERT INTO UserPreferences VALUES ('someone', 'a value')");

            var db = new ConfigDb(databasePath);
            var preferences = db.ReadPreferences();

            Assert.That(db.UsedDefaults, Is.False);
            Assert.That(preferences.OpenAfterSave, Is.True);

            /* The new table takes a save. */
            Assert.That(db.TrySavePreferences(preferences), Is.True);
        }

        static void Execute(string databasePath, string sql)
        {
            using var connection = new SqliteConnection($"Data Source={databasePath}");
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText = sql;
            command.ExecuteNonQuery();
        }
    }
}
