using System;
using System.Collections.Generic;
using NUnit.Framework;
using SmeData.FTI.WebApi.Services;
using SmeData.WebApi.Models;

namespace SmeData.FTI.WebApi.Tests
{
    public class SearchTests
    {
        private ISearchService searchService;
        [SetUp]
        public void Initialize()
        {
            var dcPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @".\data\DocToClassifier");
            var ftiPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @".\data\SmeDataIndex");
            var resGrouperPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @".\data\");
            this.searchService = new SearchService(ftiPath, dcPath, resGrouperPath);
        }
        [Test]
        public void TestSearchBulgariaKeyInstruments()
        {
            var searchApiModel = new SearchApiModel
            {
                Classifiers= new List<string>
                {
                    "E729EE04-2FED-48BC-A6C9-10EBAA85D14B",
                    "938AD68B-0057-4B28-AEA1-99EC41764C13"
                }
            };
            var ids = this.searchService.Search(searchApiModel);
            Assert.AreEqual(86, ids.Length);
        }
        [Test]
        public void TestSearchBulgariaTreaties()
        {
            var searchApiModel = new SearchApiModel
            {
                Classifiers = new List<string>
                {
                    "E729EE04-2FED-48BC-A6C9-10EBAA85D14B",
                    "A6811773-16BA-4BE9-AE74-EC947F7724C4"
                }
            };
            var ids = this.searchService.Search(searchApiModel);
            Assert.AreEqual(2, ids.Length);
        }
        [Test]
        public void TestSearchBulgariaOthers()
        {
            var searchApiModel = new SearchApiModel
            {
                Classifiers = new List<string>
                {
                    "E729EE04-2FED-48BC-A6C9-10EBAA85D14B",
                    "9A609948-49D0-45AB-A2DC-7D98AB7B03CA"
                }
            };
            var ids = this.searchService.Search(searchApiModel);
            Assert.AreEqual(8, ids.Length);
        }

        [Test]
        public void TestSearchInternational()
        {
            var searchApiModel = new SearchApiModel
            {
                Classifiers = new List<string>
                {
                    "A653D24E-0AA1-449A-AE3E-973D25FE6137",
                }
            };
            var ids = this.searchService.Search(searchApiModel);
            Assert.AreEqual(10, ids.Length);
        }

        [Test]
        public void TestSearchText()
        {
            var searchApiModel = new SearchApiModel
            {
                SearchText = "1"
            };
            var ids = this.searchService.Search(searchApiModel);
            Assert.AreEqual(640, ids.Length);
        }

        [Test]
        public void TestSearchCyrillic()
        {
            var searchApiModel = new SearchApiModel
            {
                SearchText = "закон"
            };
            var ids = this.searchService.Search(searchApiModel);
            Assert.AreEqual(161, ids.Length);
        }

        [Test]
        public void TestSearchEnglish()
        {
            var searchApiModel = new SearchApiModel
            {
                SearchText = "regulation"
            };
            var ids = this.searchService.Search(searchApiModel);
            Assert.AreEqual(184, ids.Length);
        }

        [Test]
        public void Tmp() 
        {
            var searchApiModel = new SearchApiModel
            {
                SearchText = "regulation",
                LangPreferences = new[] { 4 }
            };
            var ids = this.searchService.Search(searchApiModel);
        }
    }
}
