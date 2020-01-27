using Newtonsoft.Json;
using SmeData.Mobile.Data;
using SmeData.Mobile.Data.Models;
using SmeData.Shared.Common;
using SmeData.SharedModels.Document;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SmeData.Mobile.Services
{
    public class DocumentService
    {
        private readonly HttpService httpService;
        private readonly AppRepository documentsRepository;

        public DocumentService(HttpService httpService, AppRepository documentsRepository)
        {
            this.httpService = httpService;
            this.documentsRepository = documentsRepository;
        }

        public async Task<SmeDoc> GetSmeDocByDocNumber(string docNumber, string searchText, int langId)
        {
            return await this.httpService.GetSmeDocByDocNumber(docNumber, langId, searchText);
        }

        //public async Task<List<LastChangeOfDoc>> GetUpdatedDocuments(List<LastChangeOfDoc> docs)
        //{
        //    return await this.httpService.GetUpdatedDocuments(docs);
        //}

        public async Task<SmeDoc> GetSmeDocByIdentifier(string identifier, string searchText, int langId)
        {
            //return await httpService.GetSmeDocByIdentifier(identifier, searchText);
            
            var item = await this.documentsRepository.GetDocumentAsync(identifier);
            if (item != null && string.IsNullOrEmpty(searchText))
            {
                return JsonConvert.DeserializeObject<SmeDoc>(Compression.DecompressString(item.JsonSmeDoc));
            }
            else
            {
                return await httpService.GetSmeDocByIdentifier(identifier, searchText);
            }
        }
    }
}
