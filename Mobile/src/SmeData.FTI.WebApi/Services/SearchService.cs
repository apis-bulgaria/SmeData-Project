using ApisLucene.Classes.Common.SearchClasses;
using ApisLucene.Classes.Eucases.Search;
using DocToClassifier.LangFilter;
using SmeData.FTI.WebApi.Groupers;
using SmeData.WebApi.Models;
using System.Linq;

namespace SmeData.FTI.WebApi.Services
{
    public class SearchService : ISearchService
    {
        private EUCasesSearchWrapper searchWrapper;
        private FilterDocsStruct filterDocsStruct;
        private readonly string ftiPath;
        private readonly string docToClassifierPath;
        private readonly string resultsGrouperPath;
        private ResultsGrouper resultsGrouper;

        public SearchService(string ftiPath, string docClassifierPath, string resultsGrouperPath)
        {
            this.ftiPath = ftiPath;
            this.docToClassifierPath = docClassifierPath;
            this.resultsGrouperPath = resultsGrouperPath;
            this.Refresh();
        }

        public int[] Search(SearchApiModel model)
        {
            var sp = new MainSearch();
            if (!string.IsNullOrEmpty(model.SearchText))
            {
                sp.SearchText = model.SearchText;
            }
            if (model.Classifiers?.Count > 0)
            {
                model.Classifiers.ForEach(x => sp.ClassificatorsGroups.Add(new System.Collections.Generic.List<string> { x }));
                //sp.ClassificatorsGroups.Add(model.Classifiers);
            }
            var queries = sp.GetQueries();
            var ids = this.searchWrapper.SearchDictQueryList(queries, true);
            ids = this.resultsGrouper.FilterConsolidatedLanguageVersions(ids, this.filterDocsStruct, model.LangPreferences);

            if (model.LangPreferences?.Length > 0)
            {
                this.filterDocsStruct.FilterArray(ref ids, model.LangPreferences);
            }

            //cons versions grouper
            ids = this.resultsGrouper.FilterResults(ids);
            return ids;
        }

        public void Refresh()
        {
            if (this.searchWrapper != null)
            {
                this.searchWrapper.Dispose();
                this.searchWrapper = null;
            }

            this.searchWrapper = new EUCasesSearchWrapper(this.ftiPath);
            this.filterDocsStruct = new FilterDocsStruct();
            this.filterDocsStruct.LoadFromFile(this.docToClassifierPath);
            this.resultsGrouper = new ResultsGrouper(this.resultsGrouperPath);
        }

    }
}
