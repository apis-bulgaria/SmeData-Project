using SmeData.WebApi.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmeData.WebApi.Services.Sort
{
    public interface ISortService
    {
        int[] SortIds(List<string> classifiers, int[] ids);
        void Refresh();

        void SortSearchResult(List<string> classifier, SearchResultModel searchResult);
    }
}
