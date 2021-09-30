using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SmeData.SharedModels.Document;
using SmeData.WebApi.Data.Eucases;
using SmeData.WebApi.Models;
using SmeData.Shared.Common;
using SmeData.WebApi.Services.Documents;

namespace SmeData.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DocumentsController : ControllerBase
    {
        private readonly IDocumentsService docService;
        public DocumentsController(IDocumentsService service)
        {
            this.docService = service;
        }

        [HttpGet]
        [Route("docnum")]
        public IActionResult GetSmeDocByDocNumber(string docNumber, int langId, string searchText)
        {
            return Ok(this.docService.GetSmeDocByDocNumber(docNumber, langId, searchText));
        }
        [HttpGet]
        [Route("docident")]
        public IActionResult GetSmeDocByDocIdentifier(string identifier, string searchText)
        {
            return Ok(this.docService.GetSmeDocByDocIdentifier(identifier, searchText));
        }

        [HttpPost]
        [Route("checkDocs")]
        public IActionResult GetSmeDocByDocIdentifier(List<LastChangeOfDoc> docs)
        {
            return Ok(this.docService.GetUpdatedDocuments(docs));
        }

        [HttpPost]
        [Route("checkDocsv2")]
        public IActionResult GetSmeDocByDocIdentifierV2(List<LastChangeOfDoc> docs)
        {
            return Ok(this.docService.GetUpdatedDocumentsV2(docs));
        }
    }
}