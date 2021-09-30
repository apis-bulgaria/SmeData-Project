using Microsoft.AspNetCore.Mvc;
using SmeData.WebApi.Models;
using SmeData.WebApi.Services.Documents;
using SmeData.WebApi.Services.EuCaselawFilter;
using SmeData.WebApi.Services.Searches;
using SmeData.WebApi.Services.Sort;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SmeData.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SearchesController : ControllerBase
    {
        private readonly ISearchService searchService;
        private readonly IDocumentsService docService;
        private readonly ISortService sortService;
        private readonly IEuCaselawFilterService euCaselawFilterService;

        private readonly string keyInstrumenst = "KeyInstruments";
        private readonly string treaties = "Treaties";
        private readonly string otherInstruments = "OtherInstruments";
        private readonly string schengenAcquis = "SchengenAcquis";
        private readonly string adequacyDecisions = "AdequacyDecisions";
        private readonly string repealedInstruments = "RepealedInstruments";


        public SearchesController(ISearchService searchService, IDocumentsService docService,
            ISortService sortService, IEuCaselawFilterService euCaselawFilterService)
        {
            this.searchService = searchService;
            this.docService = docService;
            this.sortService = sortService;
            this.euCaselawFilterService = euCaselawFilterService;
        }

        [HttpPost]
        [Route("st")]
        public IActionResult SearchText(SearchApiModel model)
        {
            var ids = this.searchService.Search(model);
            var res = this.FilterSearchResult(ids, model);

            return Ok(res);
        }

        private SearchResultModel FilterSearchResult(int[] ids, SearchApiModel model)
        {
            ids = this.euCaselawFilterService.Filter(ids);
            if (model.Classifiers?.Count > 0)
            {
                ids = this.sortService.SortIds(model.Classifiers, ids);
                var sr = this.docService.GetDocList(ids, model);
                //sort
                this.sortService.SortSearchResult(model.Classifiers, sr);

                return sr;
            }
            else
            {
                var res = this.docService.GetDocList(ids, model);
                var sortDict = ids.ToList().Select((x, index) => (k: x, v: index)).ToDictionary(k => k.k, index => index.v);
                res.Data = res.Data.OrderBy(x => sortDict.ContainsKey(x.DocLangId) ? sortDict[x.DocLangId] : 100000).ToList();
                return res;
            }
        }

        [HttpPost]
        [Route("euleg")]
        public IActionResult GetLegiaslation(SearchApiModel searchModel)
        {
            var res = new LegislationResponseModel();
            res.Categories = this.GetEuCategories(searchModel);

            return Ok(res);
        }

        [HttpPost]
        [Route("eudateprotectionboardcategories")]
        public IActionResult GetEuDateProtectionBoard(SearchApiModel searchModel)
        {
            var res = new LegislationResponseModel();
            res.Categories = this.GetEuDateProtectionBoardCategories(searchModel);

            return Ok(res);
        }

        [HttpPost]
        [Route("eucaselawcategories")]
        public IActionResult GetEuCaseLaw(SearchApiModel searchModel)
        {
            var res = new LegislationResponseModel();
            res.Categories = this.GetEuCaseLawCategories(searchModel);

            return Ok(res);
        }

        [HttpPost]
        [Route("bgleg")]
        public IActionResult GetBgLegiaslation(SearchApiModel searchModel)
        {
            var res = new LegislationResponseModel();
            res.Categories = this.GetBgCategories(searchModel);

            return Ok(res);
        }

        [HttpPost]
        [Route("itleg")]
        public IActionResult GetItLegislation(SearchApiModel searchModel)
        {
            var res = new LegislationResponseModel();
            res.Categories = this.GetItCategories(searchModel);

            return Ok(res);
        }

        [Route("refresh")]
        public IActionResult Refresh()
        {
            this.sortService.Refresh();
            this.euCaselawFilterService.Refresh();
            return Ok();
        }
        private List<CategoryResponseModel> GetItCategories(SearchApiModel searchModel)
        {
            var baseClassifier = "57B81A94-F630-4D12-A062-1882AF4DF437";
            var res = new List<CategoryResponseModel>();

            res.Add(GetLegDocsByClassifier(searchModel, "938AD68B-0057-4B28-AEA1-99EC41764C13", keyInstrumenst, baseClassifier));
            res.Add(GetLegDocsByClassifier(searchModel, "A6811773-16BA-4BE9-AE74-EC947F7724C4", treaties, baseClassifier));
            res.Add(GetLegDocsByClassifier(searchModel, "9A609948-49D0-45AB-A2DC-7D98AB7B03CA", otherInstruments, baseClassifier));

            return res;
        }

        private List<CategoryResponseModel> GetBgCategories(SearchApiModel searchModel)
        {
            var baseClassifier = "E729EE04-2FED-48BC-A6C9-10EBAA85D14B";
            var res = new List<CategoryResponseModel>();


            res.Add(GetLegDocsByClassifier(searchModel, "938AD68B-0057-4B28-AEA1-99EC41764C13", keyInstrumenst, baseClassifier));
            //res.Add(GetLegDocsByClassifier(searchModel, "A6811773-16BA-4BE9-AE74-EC947F7724C4", treaties, baseClassifier));
            res.Add(GetLegDocsByClassifier(searchModel, "9A609948-49D0-45AB-A2DC-7D98AB7B03CA", otherInstruments, baseClassifier));
            res.Add(GetLegDocsByClassifier(searchModel, "06147F9E-CE58-48C7-8892-6DF7476DE3AA", repealedInstruments, baseClassifier));
            //
            return res;
        }

        private List<CategoryResponseModel> GetEuCaseLawCategories(SearchApiModel searchModel)
        {
            var baseClassifier = "0C6CC932-1EE3-4F9B-A1FD-B35B68F61578";
            var res = new List<CategoryResponseModel>();

            res.Add(GetLegDocsByClassifier(searchModel, "5BC8F8DD-2165-4582-AD4B-2BBFAE51088D", "CaseLawRelatedToTheGDPR", baseClassifier));
            res.Add(GetLegDocsByClassifier(searchModel, "6E6A168D-5DE0-46C7-99EB-C0BDF9DD6E19", "CaseLawDataProtectionDirective", baseClassifier));
            res.Add(GetLegDocsByClassifier(searchModel, "B19FED0E-F5F9-4800-94B2-3BCB990AC4A6", "CaseLawDirective2002/58/EC", baseClassifier));
            res.Add(GetLegDocsByClassifier(searchModel, "3681858C-05B7-4F4F-B311-87B2D0C6E90E", "CaseLawDirective2016/681", baseClassifier));
            res.Add(GetLegDocsByClassifier(searchModel, "BF5A9F72-21F7-465A-ACDF-6031B4BC6A7C", "CaseLawEUInstitutionsDataProtectionRegulation", baseClassifier));

            return res;
        }

        private List<CategoryResponseModel> GetEuDateProtectionBoardCategories(SearchApiModel searchModel)
        {
            var baseClassifier = "0319DD51-EE78-4F0C-B948-E8F8BD0FE0E1";
            var res = new List<CategoryResponseModel>();

            res.Add(GetLegDocsByClassifier(searchModel, "8D1F1268-97C4-4A4B-8F6B-0E7866869573", "Guidelines1", baseClassifier));
            res.Add(GetLegDocsByClassifier(searchModel, "C83789FC-77F3-4381-9880-66E98A04A47B", "OtherDocuments", baseClassifier));

            return res;
        }

        private List<CategoryResponseModel> GetEuCategories(SearchApiModel searchModel)
        {
            var baseClassifier = "BE076ED2-9F60-4B24-9560-19B5E672D947";
            var res = new List<CategoryResponseModel>();
            res.Add(GetLegDocsByClassifier(searchModel, "938AD68B-0057-4B28-AEA1-99EC41764C13", keyInstrumenst, baseClassifier));
            res.Add(GetLegDocsByClassifier(searchModel, "A6811773-16BA-4BE9-AE74-EC947F7724C4", treaties, baseClassifier));
            res.Add(GetLegDocsByClassifier(searchModel, "9A609948-49D0-45AB-A2DC-7D98AB7B03CA", otherInstruments, baseClassifier));
            res.Add(GetLegDocsByClassifier(searchModel, "0C9C6F5C-2C04-401F-988F-FAAE03B12AA7", schengenAcquis, baseClassifier));
            res.Add(GetLegDocsByClassifier(searchModel, "F7FA0E11-6FFB-4B2A-9A24-7DCB6A2506EB", adequacyDecisions, baseClassifier));
            res.Add(GetLegDocsByClassifier(searchModel, "06147F9E-CE58-48C7-8892-6DF7476DE3AA", repealedInstruments, baseClassifier));

            return res;
        }

        private CategoryResponseModel GetLegDocsByClassifier(SearchApiModel searchModel, string classifier, string headingName, string baseClassifier)
        {
            searchModel.Classifiers = new List<string>();
            searchModel.Classifiers.Add(baseClassifier);
            searchModel.Classifiers.Add(classifier);

            var res = new CategoryResponseModel();
            var ids = this.searchService.Search(searchModel);
            ids = this.euCaselawFilterService.Filter(ids);
            ids = this.sortService.SortIds(searchModel.Classifiers, ids);
            var sr = this.docService.GetDocList(ids, searchModel);
            //sort
            this.sortService.SortSearchResult(searchModel.Classifiers, sr);

            res.Items.AddRange(sr.Data);
            res.Heading = headingName;

            return res;
        }
    }
}