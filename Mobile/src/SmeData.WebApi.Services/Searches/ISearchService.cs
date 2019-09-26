using SmeData.WebApi.Models;

namespace SmeData.WebApi.Services.Searches
{
    public interface ISearchService
    {
        int[] Search(SearchApiModel model);
    }
}
