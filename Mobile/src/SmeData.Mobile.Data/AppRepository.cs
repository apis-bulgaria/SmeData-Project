using SmeData.Mobile.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using SQLiteNetExtensions;
using SQLite;
using SQLiteNetExtensionsAsync.Extensions;
using System.Linq;
using System.IO;
using System.IO.Compression;
using Xamarin.Forms;

namespace SmeData.Mobile.Data
{
    public class AppRepository : IDataStore
    {
        private readonly SQLiteAsyncConnection db;
        public AppRepository(string dbPath)
        {
            this.db = DependencyService.Get<ISQLite>().GetConnection(dbPath);

            this.db.CreateTableAsync<DocumentModel>().Wait();
            this.db.CreateTableAsync<SettingsDbModel>().Wait();
            this.db.CreateTableAsync<BookmarksModel>().Wait();
        }

        public async Task<int> AddDocumentAsync(DocumentModel item)
        {
            if ((await this.db.Table<DocumentModel>().FirstOrDefaultAsync(x => x.Identifier == item.Identifier)) == null)
            {
                await this.db.InsertAsync(item).ConfigureAwait(false);
                return 1;
            }
            else
            {
                return 2;
            }
        }

        public async Task<int> AddRangeAsync(List<DocumentModel> items)
        {
            return await this.db.InsertAllAsync(items).ConfigureAwait(false);
        }

        public async Task<int> DeleteDocumentAsync(string identifier)
        {
            return await this.db.DeleteAsync<DocumentModel>(identifier);
        }

        public async Task<DocumentModel> GetDocumentAsync(string identifier)
        {
            return await this.db.FindAsync<DocumentModel>(identifier);
        }

        public async Task<List<DocumentModel>> GetDocumentsAsync(bool forceRefresh = false)
        {
            return await this.db.Table<DocumentModel>().ToListAsync();
        }

        public async Task<int> UpdateDocumentAsync(DocumentModel item)
        {
            var docForUpdate = await this.db.Table<DocumentModel>().FirstOrDefaultAsync(x => x.Identifier == item.Identifier);

            if (docForUpdate?.IsMainDoc != null)
            {
                item.IsMainDoc = docForUpdate.IsMainDoc;
            }

            if (docForUpdate?.IsMainDoc != null)
            {
                item.IsToHide = docForUpdate.IsToHide;
            }

            return await this.db.UpdateAsync(item);
        }

        public async Task<SettingsDbModel> GetSettingsAsync(int id)
        {
            return await this.db.Table<SettingsDbModel>().FirstOrDefaultAsync(x => x.Id == id).ConfigureAwait(false);
        }

        public async Task<int> SetSettingsAsync(SettingsDbModel item)
        {
            if ((await this.db.Table<SettingsDbModel>().FirstOrDefaultAsync(x => x.Id == item.Id)) == null)
            {
                return await this.db.InsertAsync(item).ConfigureAwait(false);
            }
            else
            {
                return await this.db.UpdateAsync(item).ConfigureAwait(false);
            }
        }

        public async Task<List<BookmarksModel>> GetAllBooksmarsAsync()
        {
            return await this.db.GetAllWithChildrenAsync<BookmarksModel>().ConfigureAwait(false);
        }

        public async Task<BookmarksModel> GetBooksmarsForDocAsync(string docIdent)
        {
            return (await this.db.GetAllWithChildrenAsync<BookmarksModel>().ConfigureAwait(false)).FirstOrDefault(x => x.DocIdentifier == docIdent);
        }

        public async Task SetBookmarksForDocAsync(BookmarksModel item)
        {
            if (item == null)
            {
                return;
            }

            if ((await this.db.Table<BookmarksModel>().FirstOrDefaultAsync(x => x.DocIdentifier == item.DocIdentifier)) == null)
            {
                await this.db.InsertWithChildrenAsync(item).ConfigureAwait(false);
            }
            else
            {
                await this.db.UpdateWithChildrenAsync(item).ConfigureAwait(false);
            }
        }
    }
}
