namespace Pacioli.Config.Persistence.Test
{
    public class Tests
    {
        ConfigDb configDb;

        [SetUp]
        public void Setup()
        {
            configDb = new ConfigDb();
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