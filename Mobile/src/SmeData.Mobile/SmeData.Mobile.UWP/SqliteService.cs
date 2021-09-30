using SmeData.Mobile.Data;
using SmeData.Mobile.UWP;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Xamarin.Forms;

[assembly: Dependency(typeof(SqliteService))]
namespace SmeData.Mobile.UWP
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

        private async Task CopyDatabaseIfNotExists(string dbPath)
        {
            var storageFile = IsolatedStorageFile.GetUserStoreForApplication();

            if (!storageFile.FileExists(dbPath))
            {
                var fileToRead = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///Assets/db.sqlite", UriKind.Absolute));
                StorageFolder storageFolder = ApplicationData.Current.LocalFolder;
                await fileToRead.CopyAsync(storageFolder, "database.sqlite", NameCollisionOption.ReplaceExisting);
            }
        }
    }
}
