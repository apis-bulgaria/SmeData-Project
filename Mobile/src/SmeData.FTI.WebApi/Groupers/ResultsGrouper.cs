using DocToClassifier.LangFilter;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SmeData.FTI.WebApi.Groupers
{
    public class ResultsGrouper
    {
        private readonly Dictionary<int, List<ConsolidatedActModel>> BaseActsDict;
        private readonly Dictionary<int, int> ConsActsDict;

        private readonly List<ConsolidatedActModel>[] BaseActsArray;
        private readonly int[] ConsActsArray;
        private readonly ConsolidatedActModel[] ActsArray;
        public const string JsonBaseFilename = @"baseActs.json";

        public ResultsGrouper(string filepath)
        {
            var path = filepath.TrimEnd('/', '\\');
            ActsArray = this.GetActsInfo(filepath);
        }

        public int[] FilterConsolidatedLanguageVersions(int[] docLangIds, FilterDocsStruct filterDocsStruct, int[] langPreferences)
        {
            if (langPreferences?.Any() != true || filterDocsStruct?.FilterDocInfos?.Any() != true)
                return docLangIds;

                var nonConsolidatedDocLangIds = new List<int>();
            var consolidatedInfo = new List<ConsolidatedFullActModel>();

            foreach (var docLangId in docLangIds)
            {
                var langInfo = filterDocsStruct.FilterDocInfos[docLangId];
                if (docLangId < ActsArray.Length && ActsArray[docLangId] != null)
                {
                    var consolidated = ActsArray[docLangId];
                    consolidatedInfo.Add(new ConsolidatedFullActModel
                    {
                        DocLangId = docLangId,
                        LangId = langInfo.LangId,
                        LeadDocLangId = consolidated.LeadDocId,
                        IsBase = consolidated.IsBase
                    });
                }
                else
                {
                    nonConsolidatedDocLangIds.Add(docLangId);
                }
            }

            var preferedLang = langPreferences[0];
            var consolidatedForLanguage = consolidatedInfo.Where(x => x.LangId == preferedLang).ToList();

            return nonConsolidatedDocLangIds.Concat(consolidatedForLanguage.Select(x => x.DocLangId)).ToArray();
        }

        public int[] FilterResults(int[] ids)
        {
            var result = new List<DocumentModel>(400000);
            var leadDocsGroups = new Dictionary<int, DocumentModel>();

            for (int i = 0; i < ids.Length; i++)
            {
                var id = ids[i];

                if (id < ActsArray.Length && ActsArray[id] != null)
                {
                    var act = ActsArray[id];
                    if (leadDocsGroups.ContainsKey(act.LeadDocId))
                    {
                        var leadDocModel = leadDocsGroups[act.LeadDocId];
                        leadDocModel.ConsolidatedActIds.Add(id);
                    }
                    else
                    {
                        var docModel = new DocumentModel(DocumentTypeEnum.BaseAct, id);
                        docModel.ConsolidatedActIds.Add(id);
                        result.Add(docModel);
                        leadDocsGroups.Add(act.LeadDocId, docModel);
                    }
                }
                else
                {
                    var docModel = new DocumentModel(DocumentTypeEnum.Other, id);
                    result.Add(docModel);
                }
            }

            // BaseActs ordering, determine if it should go here or it should happen above.
            foreach (var kvp in leadDocsGroups)
            {
                var docModel = kvp.Value;
                docModel.ConsolidatedActIds = docModel.ConsolidatedActIds.OrderBy(c => ActsArray[c].Order).ToList();
                var firstId = docModel.ConsolidatedActIds.First();
                docModel.ConsolidatedActIds.RemoveAt(0);
                docModel.DocId = firstId;
            }

            return result.Select(x => x.ConsolidatedActIds.Count > 0 ? x.ConsolidatedActIds.First() : x.DocId).ToArray();
        }

        private ConsolidatedActModel[] GetActsInfo(string filepath)
        {
            var path = Path.Combine(filepath, JsonBaseFilename);
            if (!File.Exists(path))
            {
                throw new FileNotFoundException($"No file named {JsonBaseFilename} found at path {filepath}");
                //this.PopulateDocIds(filepath);
            }

            var jsonStr = File.ReadAllText(path);
            var result = JsonConvert.DeserializeObject<Dictionary<int, ConsolidatedActModel>>(jsonStr);
            var maxIndex = result.Keys.Max();
            var resultArr = new ConsolidatedActModel[maxIndex + 1];
            foreach (var kvp in result)
            {
                resultArr[kvp.Key] = kvp.Value;
            }

            return resultArr;
        }
    }
}
