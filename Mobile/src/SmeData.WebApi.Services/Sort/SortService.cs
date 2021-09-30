using Microsoft.EntityFrameworkCore;
using SmeData.WebApi.Data.Eucases;
using SmeData.WebApi.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace SmeData.WebApi.Services.Sort
{
    public class SortService : ISortService
    {
        private readonly IEuCasesContextFactory factory;
        private IPathsProvider pathsProvider;

        private Dictionary<string, int[]> sortGroups;
        public SortService(IEuCasesContextFactory factory, IPathsProvider pathsProvider)
        {
            this.factory = factory;
            this.pathsProvider = pathsProvider;
            this.FillSortGroups();
        }

        private string GenerateKey(List<string> classifiers)
        {
            classifiers = classifiers.Select(x => x.Trim().ToUpper()).ToList();
            classifiers.Sort();
            return string.Join(";", classifiers);
        }

        private static object lockObject = new object();

        private void FillSortGroups()
        {
            lock (lockObject)
            {
                this.sortGroups = new Dictionary<string, int[]>();
                List<(string key, int id, int order)> items = new List<(string key, int id, int order)>();
                using (var db = this.factory.CreateReadOnly())
                using (var command = db.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandType = CommandType.Text;
                    command.CommandText = @"select x.value, x.doc_lang_id, x.""order"" from get_order_info() as x";
                    if (command.Connection.State == ConnectionState.Closed)
                        command.Connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var classifiers = reader.GetString(0).Split(';').ToList();
                            items.Add((key: this.GenerateKey(classifiers), id: reader.GetInt32(1), order: reader.GetInt32(2)));
                        }
                    }
                }
                this.sortGroups = items.GroupBy(x => x.key)
                    .ToDictionary(g => g.Key, v => v.OrderBy(x => x.order).Select(x => x.id).ToArray());
            }
        }

        public int[] SortIds(List<string> classifiers, int[] ids)
        {
            var key = this.GenerateKey(classifiers);
            if (this.sortGroups.TryGetValue(key, out int[] sortTemplate))
            {
                var sortDict = sortTemplate.Select((x, index) => (k: x, v: index)).ToDictionary(k => k.k, index => index.v);
                ids = ids.OrderBy(x => sortDict.ContainsKey(x) ? sortDict[x] : 100000).ToArray();
            }
            return ids;
        }

        public void Refresh()
        {
            this.FillSortGroups();
        }

        public void SortSearchResult(List<string> classifiers,SearchResultModel searchResult)
        {
            var key = this.GenerateKey(classifiers);
            if (this.sortGroups.TryGetValue(key, out int[] sortTemplate))
            {
                var sortDict = sortTemplate.Select((x, index) => (k: x, v: index)).ToDictionary(k => k.k, index => index.v);
                searchResult.Data = searchResult.Data.OrderBy(x => sortDict.ContainsKey(x.DocLangId) ? sortDict[x.DocLangId] : 100000).ToList();
            }
        }
    }


}
