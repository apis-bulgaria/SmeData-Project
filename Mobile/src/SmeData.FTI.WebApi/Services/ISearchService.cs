using SmeData.WebApi.Models;

namespace SmeData.FTI.WebApi.Services
{
    public interface ISearchService
    {
        int[] Search(SearchApiModel model);
        void Refresh();
    }
}
