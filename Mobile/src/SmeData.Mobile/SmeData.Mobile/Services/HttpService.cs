using Newtonsoft.Json;
using SmeData.SharedModels.ContactsDPAs;
using SmeData.SharedModels.Document;
using SmeData.SharedModels.Link;
using SmeData.WebApi.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace SmeData.Mobile.Services
{
    public class HttpService
    {
        private string baseUrl { get; set; }
        public HttpService(string baseUrl)
        {
            this.baseUrl = baseUrl;
        }

        public async Task<SearchResultModel> GetClassifier(SearchApiModel model)
        {
            ObservableCollection<DocumentResponseModel> euDocs = new ObservableCollection<DocumentResponseModel>();
            euDocs.Clear();

            using (HttpClient client = new HttpClient())
            {
                var url = Path.Combine(this.baseUrl, $"searches/st");

                var stringContent = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");

                var res = await client.PostAsync(url, stringContent)
                    .Result
                    .EnsureSuccessStatusCode()
                    .Content.ReadAsStringAsync()
                    .ConfigureAwait(false);

                return JsonConvert.DeserializeObject<SearchResultModel>(res);
            }
        }

        
        public async Task<LegislationResponseModel> GetLegislation(SearchApiModel model, string action)
        {
            using (HttpClient client = new HttpClient())
            {
                var url = Path.Combine(this.baseUrl, $"searches/{action}");

                var stringContent = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");

                var res = await client.PostAsync(url, stringContent)
                    .Result
                    .EnsureSuccessStatusCode()
                    .Content.ReadAsStringAsync()
                    .ConfigureAwait(false);

                return JsonConvert.DeserializeObject<LegislationResponseModel>(res);
            }
        }

        private string FixQuotes(string searchText)
        {
            if (string.IsNullOrEmpty(searchText))
            {
                return searchText;
            }else
            {
                return searchText.Replace("“", "\"").Replace("”", "\"");
            }
        }
        public async Task<SmeDoc> GetSmeDocByIdentifier(string identifier, string searchText)
        {
            searchText = this.FixQuotes(searchText);
            using (HttpClient client = new HttpClient())
            {
                var url = Path.Combine(this.baseUrl, $"documents/docident?identifier={identifier}&searchText={searchText}");
                var response = await client.GetAsync(Uri.EscapeUriString(url));
                var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                return JsonConvert.DeserializeObject<SmeDoc>(json);
            }
        }

        public async Task<SmeDoc> GetSmeDocByDocNumber(string docNumber, int langId, string searchText)
        {
            searchText = this.FixQuotes(searchText);
            using (HttpClient client = new HttpClient())
            {
                var url = Path.Combine(this.baseUrl, $"documents/docnum?docNumber={docNumber}&langId={langId}&searchText={searchText}");
                var response = await client.GetAsync(url).ConfigureAwait(false);
                var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                return JsonConvert.DeserializeObject<SmeDoc>(json);
            }
        }

        public async Task<List<LastChangeOfDoc>> GetUpdatedDocuments(List<LastChangeOfDoc> docs)
        {
            using (HttpClient client = new HttpClient())
            {
                var jsonDocs = JsonConvert.SerializeObject(docs);

                var buffer = Encoding.UTF8.GetBytes(jsonDocs);
                var byteContent = new ByteArrayContent(buffer);

                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                var url = Path.Combine(this.baseUrl, $"documents/checkDocsv2");
                var response = await client.PostAsync(url, byteContent);
                var json = await response.Content.ReadAsStringAsync();

                return JsonConvert.DeserializeObject<List<LastChangeOfDoc>>(json);
            }
        }

        public async Task<List<GdprDictionaryResponseModel>> GetGdprDictionaryByLangId(string id)
        {
            using (HttpClient client = new HttpClient())
            {
                var url = Path.Combine(this.baseUrl, $"GdprDictionary/terms/{id}");
                var response = await client.GetAsync(url);
                var json = await response.Content.ReadAsStringAsync();

                return JsonConvert.DeserializeObject<List<GdprDictionaryResponseModel>>(json);
            }
        }

        public async Task<List<string>> GetTranslations()
        {
            using (HttpClient client = new HttpClient())
            {
                var url = Path.Combine(this.baseUrl, $"values/translations");
                var response = await client.GetAsync(url).ConfigureAwait(false);
                var translations = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                return new List<string>(translations.Split(new[] { Environment.NewLine }, StringSplitOptions.None));
            }
        }

        public async Task<List<ContactDpaModel>> GetContactsDPAs()
        {
            using (HttpClient client = new HttpClient())
            {
                var url = Path.Combine(this.baseUrl, $"values/contactDpa");
                var response = await client.GetAsync(url);
                var json = await response.Content.ReadAsStringAsync();

                return JsonConvert.DeserializeObject<List<ContactDpaModel>>(json);
            }
        }

        public async Task<List<LinkModel>> GetUsefulkLinks()
        {
            using (HttpClient client = new HttpClient())
            {
                var url = Path.Combine(this.baseUrl, $"values/usefulLinks");
                var response = await client.GetAsync(url);
                var json = await response.Content.ReadAsStringAsync();

                return JsonConvert.DeserializeObject<List<LinkModel>>(json);
            }
        }

        public async Task<string> GetTestDsp()
        {
            using (HttpClient client = new HttpClient())
            {
                var url = Path.Combine(this.baseUrl, $"decisionsupport");
                var response = await client.GetAsync(url);
                var htmlString = await response.Content.ReadAsStringAsync();

                return htmlString;
            }
        }

        public string GetStaticFileContentUrl(string guid, string fileName)
        {
            return $@"{this.baseUrl}values/file?guid={guid}&filename={fileName}";
        }
    }
}
