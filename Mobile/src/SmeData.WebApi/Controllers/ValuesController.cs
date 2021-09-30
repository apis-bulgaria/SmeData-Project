using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SmeData.SharedModels.ContactsDPAs;
using SmeData.SharedModels.Document;
using SmeData.SharedModels.Link;
using SmeData.WebApi.Services.Values;

namespace SmeData.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        public IValuesService valuesService;
        public ValuesController(IValuesService valuesService)
        {
            this.valuesService = valuesService;
        }

        // GET api/values

        [HttpGet]
        [Route("translations")]
        public ActionResult<string> GetTranslations()
        {
            return this.valuesService.GetTranslations();
        }

        [HttpGet]
        [Route("gdprRecitalsEn")]
        public ActionResult<IEnumerable<SmeDocItem>> GetGdprRecitalsEN()
        {
            return this.valuesService.GetGdprRecitalsEN();
        }

        [HttpGet]
        [Route("gdprRecitalsIt")]
        public ActionResult<IEnumerable<SmeDocItem>> GetGdprRecitalsIT()
        {
            return this.valuesService.GetGdprRecitalsIT();
        }

        [HttpGet]
        [Route("gdprRecitalsBg")]
        public ActionResult<IEnumerable<SmeDocItem>> GetGdprRecitalsBG()
        {
            return this.valuesService.GetGdprRecitalsBG();
        }

        [HttpGet]
        [Route("contactDpa")]
        public ActionResult<IEnumerable<ContactDpaModel>> GetContactDpa()
        {
            return this.valuesService.GetContactsDpa();
        }

        [HttpGet]
        [Route("usefulLinks")]
        public ActionResult<IEnumerable<LinkModel>> GetUsefulLinks()
        {
            return this.valuesService.GetUsefulLinks();
        }

        [HttpGet]
        [Route("file")]
        public IActionResult GetFile(string guid, string fileName)
        {
            var bytes = this.valuesService.GetFileBytes(guid, fileName);
            if (bytes != null)
            {
                return new FileContentResult(bytes, "application/octet-stream")
                {
                    FileDownloadName = fileName
                };
            }
            else
            {
                return NotFound();
            }
        }
    }
}
