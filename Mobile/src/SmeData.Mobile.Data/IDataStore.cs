using SmeData.Mobile.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SmeData.Mobile.Data
{
    public interface IDataStore
    {
        Task<int> AddDocumentAsync(DocumentModel item);
        Task<int> UpdateDocumentAsync(DocumentModel item);
        Task<int> DeleteDocumentAsync(string ident);
        Task<DocumentModel> GetDocumentAsync(string ident);
        Task<List<DocumentModel>> GetDocumentsAsync(bool forceRefresh = false);
        Task<SettingsDbModel> GetSettingsAsync(int id);
        Task<int> SetSettingsAsync(SettingsDbModel item);
    }
}
