using SmeData.SharedModels.Document;
using SmeData.WebApi.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmeData.WebApi.Services.Documents
{
    public interface IDocumentsService
    {
        SmeDoc GetSmeDocByDocNumber(string docNumber, int langId, string searchText);
        SmeDoc GetSmeDocByDocIdentifier(string identifier, string searchText);
        List<GdprDictionaryResponseModel> GdprDictionary(int langId);
        SearchResultModel GetDocList(int[] ids, SearchApiModel model);
        IList<LastChangeOfDoc> GetUpdatedDocuments(IList<LastChangeOfDoc> docsForCheck);
        IList<LastChangeOfDoc> GetUpdatedDocumentsV2(IList<LastChangeOfDoc> docsForCheck);
        void UpdateLastChangeDoc(LastChangeOfDoc doc);
    }
}
