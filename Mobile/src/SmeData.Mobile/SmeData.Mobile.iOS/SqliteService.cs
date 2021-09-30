using System.IO;
using Foundation;
using SmeData.Mobile.Data;
using SmeData.Mobile.iOS;
using SQLite;
using Xamarin.Essentials;
using Xamarin.Forms;

[assembly: Dependency(typeof(SqliteService))]
namespace SmeData.Mobile.iOS
{
    public class SqliteService : ISQLite
    {
        public SqliteService() { }

        SQLiteAsyncConnection ISQLite.GetConnection(string dbPath)
        {
            CopyDatabaseIfNotExists(dbPath);
            var connection = new SQLiteAsyncConnection(dbPath);

            return connection;
        }

        private static void CopyDatabaseIfNotExists(string dbPath)
        {
            if (!File.Exists(dbPath))
            {
                var existingDb = NSBundle.MainBundle.PathForResource("database", "sqlite");
                File.Copy(existingDb, dbPath);
            }
        }
    }
}