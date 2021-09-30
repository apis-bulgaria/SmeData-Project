using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using SmeData.SharedModels.Document;
using SmeData.WebApi.Data.Eucases;
using SmeData.WebApi.Models;
using System.Linq;
using AkomaNtosoXml.Xslt.Core.Classes.Resolver;
using Microsoft.EntityFrameworkCore;
using System.Data;
using FtiSearchColorizer;
using System.Xml.Linq;
using System.Text.RegularExpressions;
using HtmlAgilityPack;

namespace SmeData.WebApi.Services.Documents
{
    public class DocumentsService : IDocumentsService
    {
        private IPathsProvider pathsProvider;
        private DocumentsServiceDbHelper dbHelper;
        public DocumentsService(IEuCasesContextFactory factory, IPathsProvider pathsProvider)
        {
            this.dbHelper = new DocumentsServiceDbHelper(factory, pathsProvider);
            AkomaNtosoPreProcessor.ReThrowExceptions = true;
            AkomaNtosoPreProcessor.RemoveHiddenElements = true;
            AkomaNtosoPreProcessor.ImagePath = "static_file";
            AkomaNtosoPreProcessor.IsSmeData = true;
            this.pathsProvider = pathsProvider;
        }

        public SearchResultModel GetDocList(int[] ids, SearchApiModel model)
        {
            ids = ids ?? new int[0];
            var res = new SearchResultModel();
            res.TotalCount = ids.Length;
            res.PageSize = (model.PageSize == 0) ? Math.Max(1, res.TotalCount) : model.PageSize;
            res.PageNum = model.PageNum == 0 ? 1 : model.PageNum;
            var pagesCount = Math.Ceiling((double)res.TotalCount / (double)res.PageSize);
            if (res.PageNum <= pagesCount)
            {
                ids = ids.Skip((res.PageNum - 1) * res.PageSize).Take(res.PageSize).ToArray();
            }

            res.Data = this.dbHelper.GetDocListData(ids);
            return res;
        }



        private SmeDoc CreateSmeDoc(SpDocumentModel docInfo, string searchText)
        {
            if (docInfo.DocLanguageId != -1)
            {
                var docText = this.dbHelper.GetDocumentTextByDocLangId(docInfo.DocLanguageId);
                if (this.IsBlob(docText, out string fileName))
                {
                    var smeDoc = new SmeDoc();
                    smeDoc.Meta = DocConverter.GetSmeDocMeta(docText);
                    smeDoc.Meta.ShortTitle = smeDoc.Meta.ShortTitle; //must change when ShortTitle column is available in database
                    this.FillMetaFromDocRecord(smeDoc, docInfo);
                    smeDoc.Meta.IsBlob = true;

                    var filePath = $@"{this.pathsProvider.PdfPath}\{smeDoc.Meta.Idenitifier}\{fileName}";
                    if (System.IO.File.Exists(filePath))
                    {

                        smeDoc.Items = new List<SmeDocItem>();
                        var fileExt = System.IO.Path.GetExtension(filePath).ToLower();
                        if (fileExt == ".html")
                        {
                            var docContent = System.IO.File.ReadAllText(filePath);
                            smeDoc.Meta.IsBlob = false;
                            smeDoc.Items.Add(new SmeDocItem
                            {
                                Text = docContent,
                                Type = SmeDocItemType.Text
                            });
                        }
                        else
                        {
                            smeDoc.Items.Add(new SmeDocItem
                            {
                                Text = fileName,
                                Type = SmeDocItemType.Text
                            });
                        }


                    }
                    return smeDoc;
                }
                else
                {
                    var html = AkomaNtosoPreProcessor.ConvertToHtml(docText, new AkomaNtosoPreProcessorConfig());
                    html = this.ReplaceImgUrls(html);

                    if (!string.IsNullOrEmpty(searchText))
                    {
                        html = this.ColorizeSearch(searchText, html, docInfo.LangId);
                    }

                    html = DocumentLinkRewrite.ReplaceNationalLegislation(html);

                    var res = this.CreateSmeDoc(html, docText, docInfo);
                    return res;
                }
            }
            else
            {
                return null;
            }
        }

        private string ReplaceImgUrls(string html)
        {
            return Regex.Replace(html, @"static_file[^""]+", (m) =>
            {
                var v = m.Value;
                foreach (var ext in this.imagesExtensions)
                {
                    if (v.ToLower().EndsWith(ext))
                    {
                        v = v.Replace("static_file", "https://smedata.apis.bg/images");
                        return v;
                    }
                }

                return v;

            }, RegexOptions.IgnoreCase);
        }

        private readonly HashSet<string> imagesExtensions = new HashSet<string>
        {
            ".jpg", ".png", ".gif", ".tiff"
        };

        private bool IsBlob(string docText, out string fileName)
        {
            fileName = string.Empty;
            XNamespace eucases = "http://eucases/proprietary";
            XNamespace ns = "http://docs.oasis-open.org/legaldocml/ns/akn/3.0/CSD11";
            var el = XElement.Parse(docText);
            var blobNode = el.Descendants(ns + "proprietary").Descendants(eucases + "p").Where(x => x.Attribute("class")?.Value == "#Blob").FirstOrDefault();
            if (blobNode != null)
            {
                fileName = blobNode.Elements().Where(x => x.Attribute("class")?.Value == "#Name").FirstOrDefault()?.Value;

                return true;
            }
            return false;
        }


        public List<GdprDictionaryResponseModel> GdprDictionary(int langId)
        {
            return this.dbHelper.GdprDictionary(langId);
        }

        private ColorizeSearch GetColorizerByLangId(int langId)
        {
            switch (langId)
            {
                case 1: return ColorizeSearchFactory.Create(ColorizeLanguage.Bulgarian);
                case 5: return ColorizeSearchFactory.Create(ColorizeLanguage.Italian);
                case 4: return ColorizeSearchFactory.Create(ColorizeLanguage.English);
                default: throw new NotImplementedException($"Non supported language with id:{langId}");
            }
        }

        public SmeDoc GetSmeDocByDocNumber(string docNumber, int langId, string searchText)
        {
            var docInfo = this.dbHelper.GetDocRecordByDocNumberAndLang(docNumber, langId);
            return this.CreateSmeDoc(docInfo, searchText);
        }

        private SmeDoc CreateSmeDoc(string html, string pt, SpDocumentModel docInfo)
        {
            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(html);

            var res = DocConverter.DoDocConvert(htmlDoc, pt);
            if (res.Meta.IsConsolidatedEuLegislation && res.HasRecitals() == false)
            {
                var preambleWithRecitals = this.dbHelper.GetRecitalsPreambleForConsVersion(docInfo.DocLanguageId);
                if (String.IsNullOrEmpty(preambleWithRecitals) == false)
                {
                    var preambleWithRecitalsHtml = AkomaNtosoPreProcessor.ConvertToHtml(preambleWithRecitals, new AkomaNtosoPreProcessorConfig());
                    InsertRecitalsAtPreambleEnd(htmlDoc, preambleWithRecitalsHtml);
                    res = DocConverter.DoDocConvert(htmlDoc, pt);
                }
            }


            this.FillMetaFromDocRecord(res, docInfo);
            var filePath = @".\data\smedata.css";
            if (!string.IsNullOrEmpty(this.pathsProvider.BasePath))
            {
                filePath = System.IO.Path.Combine(this.pathsProvider.BasePath, filePath);
            }
            res.Head += System.IO.File.ReadAllText(filePath);
            return res;
        }

        private void FillMetaFromDocRecord(SmeDoc res, SpDocumentModel docInfo)
        {
            res.Meta.LangId = docInfo.LangId;
            res.Meta.ShortLang = docInfo.ShortLang;
            res.Meta.DocLangId = docInfo.DocLanguageId;
            res.Meta.DocNumber = docInfo.DocNumber;
        }

        private string ColorizeSearch(string searchText, string html, int langId)
        {
            return this.GetColorizerByLangId(langId).Colorize(searchText, html, true);
        }

        public SmeDoc GetSmeDocByDocIdentifier(string identifier, string searchText)
        {
            var docInfo = this.dbHelper.GetDocRecordByIdentifier(identifier);
            return this.CreateSmeDoc(docInfo, searchText);
        }

        public IList<LastChangeOfDoc> GetUpdatedDocuments(IList<LastChangeOfDoc> docsForCheck)
        {
            var dict = this.GetIdentsAndDates(docsForCheck);
            var res = new List<LastChangeOfDoc>();
            foreach (var item in docsForCheck)
            {
                if (dict.TryGetValue(item.Ident, out DateTime date))
                {
                    if (item.LastChangeDate < date)
                    {
                        res.Add(new LastChangeOfDoc { Ident = item.Ident, LastChangeDate = date });
                    }
                }
            }

            return res;
        }

        public IList<LastChangeOfDoc> GetUpdatedDocumentsV2(IList<LastChangeOfDoc> docsForCheck)
        {
            var dict = this.GetIdentsAndDates(docsForCheck);
            var res = new List<LastChangeOfDoc>();
            foreach (var item in docsForCheck)
            {
                if (dict.TryGetValue(item.Ident, out DateTime date))
                {
                    if (item.LastChangeDate < date || (item.LastChangeDate == date && item.LastChangeDate.Value.TimeOfDay < date.TimeOfDay) || (item.LastChangeDate == null && date != null))
                    {
                        res.Add(new LastChangeOfDoc { Ident = item.Ident, NewIdent = item.Ident, LastChangeDate = date });
                    }
                    else
                    {
                        var lastChange = this.dbHelper.GetLastChangeDocForConsByIdentifier(item.Ident);
                        if (lastChange != null)
                        {
                            if (lastChange.Ident != item.Ident)
                            {
                                res.Add(lastChange);
                            }
                        }
                    }
                }
            }

            return res;
        }

        private Dictionary<string, DateTime> GetIdentsAndDates(IList<LastChangeOfDoc> docsForCheck)
        {
            return this.dbHelper.GetIdentsAndDates(docsForCheck);
        }

        public void UpdateLastChangeDoc(LastChangeOfDoc doc)
        {
            this.dbHelper.UpdateLastChangeDoc(doc);
        }

        private static void InsertRecitalsAtPreambleEnd(HtmlDocument akomaNtosoHtml, String preableWithRecitalsHtml)
        {
            var insertAfter = akomaNtosoHtml.DocumentNode
                .SelectSingleNode(".//*[contains(concat(' ', @class, ' '), ' d-preamble ')]")
                ?.ChildNodes
                ?.LastOrDefault();

            var preambleNode = HtmlNode.CreateNode(preableWithRecitalsHtml);

            if (insertAfter != null)
            {
                var recitalsRoot = preambleNode.SelectSingleNode(".//*[contains(concat(' ', @class, ' '), ' d-recitals ')]");
                insertAfter.ParentNode.InsertAfter(newChild: recitalsRoot, refChild: insertAfter);
            }
            else
            {
                // when there is no preamble
                // we can insert the new preamble before the body
                var insertBefore = akomaNtosoHtml.DocumentNode.SelectSingleNode(".//*[contains(concat(' ', @class, ' '), ' d-body ')]");
                insertBefore.ParentNode.InsertBefore(newChild: preambleNode, refChild: insertBefore);

            }
        }
    }
}
