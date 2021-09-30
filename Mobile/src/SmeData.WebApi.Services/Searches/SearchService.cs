using Newtonsoft.Json;
using SmeData.WebApi.Models;
using System;
using System.Net.Http;

namespace SmeData.WebApi.Services.Searches
{
    public class SearchService : ISearchService
    {
        private readonly HttpClient proxyClient;
        public SearchService(HttpClient client)
        {
            this.proxyClient = client;
        }

        public int[] Search(SearchApiModel model)
        {
            var url = "searches/st";
            var temp = this.proxyClient.PostAsJsonAsync(url, model)
                .Result
                .EnsureSuccessStatusCode()
                .Content.ReadAsStringAsync().Result;
            var docIds = JsonConvert.DeserializeObject<int[]>(temp);
            docIds = docIds ?? new int[0];
            return docIds;
        }

        

    }
}
