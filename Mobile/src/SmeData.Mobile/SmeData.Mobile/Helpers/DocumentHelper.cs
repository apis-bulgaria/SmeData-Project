using SmeData.Mobile.Models.Documents;
using SmeData.Mobile.Services;
using SmeData.SharedModels.Document;
using SmeData.SharedModels.Language;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace SmeData.Mobile.Helpers
{
    public static class DocumentHelper
    {
        private static HashSet<SmeDocItemType> endLevelItemTypes = new HashSet<SmeDocItemType> {
                SmeDocItemType.Article,
                SmeDocItemType.Recital,
                SmeDocItemType.Text,
                SmeDocItemType.DocTitle
            };
        public static SmeDoc FlatDocument(SmeDoc orgDoc)
        {
            if (orgDoc == null)
            {
                return null;
            }
            var res = new SmeDoc
            {
                Meta = orgDoc.Meta,
                Head = orgDoc.Head,
                Items = new List<SmeDocItem>()
            };

            foreach (var item in orgDoc.Items)
            {
                TravelDocument(item, res.Items);
            }
            FixRecitals(res);
            FixTitle(res);
            return res;
        }

        public static List<ColorizeInfo> GetColorizeInfo(SmeDoc smeDoc)
        {
            var res = new List<ColorizeInfo>();
            foreach (var item in smeDoc.Items)
            {
                var text = GetDisplayText(smeDoc, SmeLanguage.Bulgarian, item, false);
                MatchCollection mc = Regex.Matches(text, @"ss_(\d+)");
                if (mc.Count > 0)
                {
                    var matchedIds = new List<string>();
                    foreach (Match m in mc)
                    {
                        res.Add(new ColorizeInfo
                        {
                            DocItem = item,
                            MatchedId = m.Value
                        });
                    }

                }
            }
            return res;
        }

        private static void FixTitle(SmeDoc smeDoc)
        {
            if (smeDoc.Items.Count > 2 && (!smeDoc.Items.Any(x => x.Type == SmeDocItemType.DocTitle)))
            {
                var titleItem = smeDoc.Items[0];
                if (titleItem.Type == SmeDocItemType.Text)
                {
                    titleItem.Type = SmeDocItemType.DocTitle;
                    var cultureInfo = Properties.Resources.Culture;
                    titleItem.Heading = Translator.GetString("Title");
                }
            }
        }

        private static void FixRecitals(SmeDoc smeDoc)
        {
            var firstRecitalIndex = smeDoc.Items.IndexOf(smeDoc.Items.FirstOrDefault(x => x.Type == SmeDocItemType.Recital));
            if (firstRecitalIndex < 0)
            {
                return;
            }
            var recitalItems = smeDoc.Items.Where(x => x.Type == SmeDocItemType.Recital).ToList();
            var cultureInfo = Properties.Resources.Culture;
            smeDoc.Items = smeDoc.Items.Where(x => x.Type != SmeDocItemType.Recital).ToList();
            var partSize = 30;
            int partsCount = (int)Math.Ceiling((double)recitalItems.Count() / partSize);
            var groupedItems = new List<SmeDocItem>();
            for (int i = 0; i < partsCount; i++)
            {
                var smeDocItem = new SmeDocItem()
                {
                    Childs = new List<SmeDocItem>(recitalItems.Skip(i * partSize).Take(partSize)),
                    Type = SmeDocItemType.Recital
                };
                smeDocItem.Heading = $"{Translator.GetString("Recitals")} {i * partSize + 1}-{i * partSize + smeDocItem.Childs.Count}";
                groupedItems.Add(smeDocItem);

            }
            smeDoc.Items.InsertRange(firstRecitalIndex, groupedItems);
        }

        private static void TravelDocument(SmeDocItem orgItem, List<SmeDocItem> resItems)
        {
            var tmpItem = orgItem.Clone();
            resItems.Add(tmpItem);
            if (!endLevelItemTypes.Contains(orgItem.Type))
            {
                tmpItem.Childs = new List<SmeDocItem>();
                foreach (var child in orgItem.Childs)
                {
                    TravelDocument(child, resItems);
                }
            }
        }

        private static Dictionary<SmeLanguage, SmeDoc> oldDirective = new Dictionary<SmeLanguage, SmeDoc>();
        public static HttpService HttpService;
        private static SmeDoc OldDirective(SmeLanguage lang)
        {
            if (oldDirective.TryGetValue(lang, out SmeDoc res))
            {
                return res;
            }
            else
            {
                try
                {
                    var oldSmeDoc = HttpService.GetSmeDocByDocNumber("31995L0046", (int)lang, string.Empty).Result;
                    oldDirective[lang] = oldSmeDoc;
                    return oldSmeDoc;
                }
                catch { return null; }
            }
        }

        public static string GetDisplayText(SmeDoc document, SmeLanguage lang, SmeDocItem docItem, bool addArtRec = true)
        {
            var sb = new StringBuilder();
            GetHtml(sb, docItem, document, lang, addArtRec);
            return sb.ToString();
        }

        private static string AddButtons(List<string> items, string elId, string title, SmeDoc document, SmeLanguage lang, bool tryFindParents = false)
        {
            var cultureInfo = Properties.Resources.Culture;
            var sb = new StringBuilder();

            if (items?.Count > 0)
            {
                sb.AppendLine($@"<button onclick=""showHideDiv('{elId}')"" >{title}</button><div  class=""gdpr-recital"" style=""display:none;"" id=""{elId}"" name=""{elId}"">");

                foreach (var id in items)
                {
                    var linkedItem = document.GetItemById(id, tryFindParents);
                    if (linkedItem != null)
                    {
                        sb.AppendLine($"{GetDisplayText(document, lang, linkedItem, false)}<hr>");
                    }
                }

                sb.AppendLine("</div>");
            }

            return sb.ToString();
        }

        private static void GetHtml(StringBuilder sb, SmeDocItem item, SmeDoc document, SmeLanguage lang, bool addArtRec = true)
        {
            sb.AppendLine(item.Text);

            if (addArtRec)
            {
                sb.AppendLine(AddButtons(item.Recitals, $"{item.Id}_rec", Translator.GetString("Recitals"), document, lang));
                sb.AppendLine(AddButtons(item.Articles, $"{item.Id}_art", Translator.GetString("Articles"), document, lang));

                if (item.OldArticles?.Count > 0)
                {
                    var od = OldDirective(lang);

                    if (od != null)
                    {
                        sb.AppendLine(AddButtons(item.OldArticles, $"{item.Id}_old_art", Translator.GetString("31995L0046"), od, lang, true));
                    }
                }
            }

            foreach (var childItem in item.Childs)
            {
                GetHtml(sb, childItem, document, lang, addArtRec);
            }
        }
    }
}
