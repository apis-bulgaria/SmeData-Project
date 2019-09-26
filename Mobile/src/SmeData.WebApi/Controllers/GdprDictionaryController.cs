using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmeData.WebApi.Services.Documents;

namespace SmeData.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GdprDictionaryController : ControllerBase
    {
        private readonly IDocumentsService docService;
        public GdprDictionaryController(IDocumentsService service)
        {
            this.docService = service;
        }

        [HttpGet]
        [Route("terms/{langId}")]
        public IActionResult GetTerms(int langId)
        {
            return Ok(this.docService.GdprDictionary(langId));
        }
    }
}