namespace Pacioli.Config.Persistence.Test
{
    /*
     * The database takes a preference and gives it back.
     *
     * The test uses a database file of its own. Before, it used the file of the person
     * that runs it, thus each run changed the settings of that person.
     *
     * ConfigDbTests holds the tests of the behaviour with a database that does not answer.
     */
    public class Tests
    {
        string folder = string.Empty;
        ConfigDb configDb = null!;

        [SetUp]
        public void Setup()
        {
            folder = Path.Combine(Path.GetTempPath(), "Pacioli.Config.Test", Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(folder);
            configDb = new ConfigDb(Path.Combine(folder, "pacioli.db"));
        }

        [TearDown]
        public void RemoveTheDatabase()
        {
            Microsoft.Data.Sqlite.SqliteConnection.ClearAllPools();

            try
            {
                Directory.Delete(folder, recursive: true);
            }
            catch (IOException)
            {
            }
        }

        [Test]
        public void Test1()
        {
            var pref = configDb.ReadPreferences();
            pref.OpenAfterSave = false;
            configDb.SavePreferences(pref);
            pref = configDb.ReadPreferences();
            Assert.That(pref.OpenAfterSave, Is.False);
        }
    }
}
