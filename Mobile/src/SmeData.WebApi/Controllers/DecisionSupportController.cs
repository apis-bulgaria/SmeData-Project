using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmeData.WebApi.Services.DecisionSupport;

namespace SmeData.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DecisionSupportController : ControllerBase
    {
        private IDecisionSupportService decService;
        public DecisionSupportController(IDecisionSupportService service)
        {
            this.decService = service;
        }
        [HttpGet]
        public IActionResult Index()
        {
            return Ok(this.decService.GetQuestionary(1));
        }
    }
}