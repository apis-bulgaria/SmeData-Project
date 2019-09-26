using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SmeData.FTI.WebApi.Services;
using SmeData.WebApi.Models;

namespace SmeData.FTI.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SearchesController : ControllerBase
    {
        private ISearchService searchService;
        private ILogger logger;
        public SearchesController(ISearchService searchService, ILogger<SearchesController> logger)
        {
            this.searchService = searchService;
            this.logger = logger;
        }

        [HttpPost]
        [Route("st")]
        public IActionResult SearchText(SearchApiModel model)
        {
            var res = this.searchService.Search(model);
            return Ok(res);
        }

        [Route("refresh")]
        public IActionResult Refresh()
        {
            this.searchService.Refresh();
            return Ok();
        }
    }
}