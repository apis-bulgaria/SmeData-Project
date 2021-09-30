using HtmlAgilityPack;
using SmeData.SharedModels.Document;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace SmeData.DocConverter
{
    public static class DocConverter
    {
        public static SmeDoc DoDocConvert(List<string> htmlStringDocs)
        {
            SmeDoc devidedDoc = new SmeDoc();

            HtmlDocument htmlDoc = new HtmlDocument();

            htmlDoc.LoadHtml(htmlStringDocs[0]);
            devidedDoc.Items = GetRecitalsFromPrefaceHtml(htmlDoc);

            htmlDoc.LoadHtml(htmlStringDocs[1]);
            devidedDoc.Items.AddRange(GetDevidedLegalPartFromBodyHtml(htmlDoc));

            //var returnedDocs = JsonConvert.SerializeObject(devidedDoc);

            return devidedDoc;
        }

        public static List<SmeDocItem> GetRecitalsFromPrefaceHtml(HtmlDocument htmlDoc)
        {
            List<SmeDocItem> resultItems = new List<SmeDocItem>();

            var allContentDivs = htmlDoc.DocumentNode.SelectNodes(@"/div/div/div");

            SmeDocItem notRecItem = new SmeDocItem();
            notRecItem.Type = SmeDocItemType.Text;

            foreach (HtmlNode divNode in allContentDivs)
            {
                Match matchRec = Regex.Match(divNode.InnerText, @"^\s*\((\d+)\)\s+");

                if (matchRec.Success)
                {
                    if (notRecItem != null)
                    {
                        resultItems.Add(notRecItem);
                        notRecItem = null;
                    }

                    SmeDocItem recItem = new SmeDocItem();
                    recItem.Type = SmeDocItemType.Recital;
                    recItem.Id = $"rec_{matchRec.Groups[1].Value}";
                    recItem.Text = divNode.OuterHtml;

                    resultItems.Add(recItem);
                }
                else
                {
                    if (notRecItem == null)
                    {
                        notRecItem = new SmeDocItem();
                        notRecItem.Type = SmeDocItemType.Text;
                    }

                    notRecItem.Text += divNode.OuterHtml;
                }
            }

            if (notRecItem != null)
            {
                resultItems.Add(notRecItem);
            }

            return resultItems;
        }

        public static List<SmeDocItem> GetDevidedLegalPartFromBodyHtml(HtmlDocument htmlDoc)
        {
            var mainAnchors = htmlDoc.DocumentNode.FirstChild.SelectNodes("./a[@class='doc-anchor']");

            List<SmeDocItem> resultItems = new List<SmeDocItem>();

            foreach (HtmlNode currAnchor in mainAnchors)
            {
                string currBaseEId = currAnchor.Attributes[@"eId"]?.Value;
                if (!string.IsNullOrWhiteSpace(currBaseEId))
                {
                    SmeDocItem currentBaseEl = new SmeDocItem();
                    currentBaseEl.Text += currAnchor.OuterHtml;
                    currentBaseEl.TreeLevel = 0;
                    SetElementIdAndType(currBaseEId, currentBaseEl);

                    var currBaseNode = currAnchor.NextSibling;
                    GetAndSetChildsNodes(currentBaseEl, currBaseNode);

                    resultItems.Add(currentBaseEl);
                }
            }

            return resultItems;
        }

        private static void GetAndSetChildsNodes(SmeDocItem currentBaseEl, HtmlNode currBaseNode)
        {
            {
                if (currBaseNode != null)
                {
                    currentBaseEl.Text += Regex.Match(currBaseNode.OuterHtml, @"^\s*\<[^\<\>]+\>").Value + Regex.Match(currBaseNode.OuterHtml, @"\<\/[^\<\>]+\>\s*$").Value;

                    bool isChildNodeNext = false;

                    foreach (HtmlNode childNode in currBaseNode.ChildNodes)
                    {
                        if (isChildNodeNext)
                        {
                            isChildNodeNext = false;
                            continue;
                        }

                        if (childNode.Attributes[@"class"] != null)
                        {
                            if (childNode.Attributes[@"class"].Value.Contains("d-num"))
                            {
                                currentBaseEl.Heading = childNode.InnerText.Trim();
                            }
                            else if (childNode.Attributes[@"class"].Value.Contains("d-heading"))
                            {
                                currentBaseEl.SubHeading = childNode.InnerText.Trim();
                            }
                        }

                        if (childNode.Name.ToLower() != "a")
                        {
                            currentBaseEl.Text += childNode.OuterHtml;
                        }
                        else
                        {
                            string childEId = childNode.Attributes[@"eId"]?.Value;
                            if (!string.IsNullOrWhiteSpace(childEId))
                            {
                                SmeDocItem childBaseEl = new SmeDocItem();
                                childBaseEl.TreeLevel = currentBaseEl.TreeLevel + 1;
                                childBaseEl.Text += childNode.OuterHtml;
                                SetElementIdAndType(childEId, childBaseEl);
                                var childBaseNode = childNode.NextSibling;
                                isChildNodeNext = true;

                                if (Regex.IsMatch(childBaseEl.Id, @"^art_", RegexOptions.IgnoreCase))
                                {
                                    childBaseEl.Text += Regex.Match(childBaseNode.OuterHtml, @"^\s*\<[^\<\>]+\>").Value + Regex.Match(childBaseNode.OuterHtml, @"\<\/[^\<\>]+\>\s*$").Value;

                                    foreach (var artChild in childBaseNode.ChildNodes)
                                    {
                                        if (artChild.Attributes[@"class"] != null)
                                        {
                                            if (artChild.Attributes[@"class"].Value.Contains("d-num"))
                                            {
                                                childBaseEl.Heading = artChild.InnerText.Trim();
                                            }
                                            else
                                            {
                                                Match matchSubHeading = Regex.Match(artChild.InnerHtml, @"\<[^\<\>]+class\s?=\s?['""][^'""]*?d-c-sti-art[^'""]*?['""][^\<\>]*\>([^\<\>]+)\<");

                                                if (matchSubHeading.Success)
                                                {
                                                    childBaseEl.SubHeading = matchSubHeading.Groups[1].Value;
                                                }
                                            }
                                        }

                                        if (artChild.Name.ToLower() != "div")
                                        {
                                            childBaseEl.Text += artChild.OuterHtml;
                                        }
                                        else
                                        {
                                            childBaseEl.Text += Regex.Match(artChild.OuterHtml, @"^\s*\<[^\<\>]+\>").Value + Regex.Match(artChild.OuterHtml, @"\<\/[^\<\>]+\>\s*$").Value;

                                            foreach (var divChild in artChild.ChildNodes)
                                            {
                                                if (divChild.Name.ToLower() == "p")
                                                {
                                                    Match matchNumber = Regex.Match(divChild.InnerText.Trim(), @"^(\d+)\.");

                                                    if (matchNumber.Success)
                                                    {
                                                        SmeDocItem parEl = new SmeDocItem();
                                                        parEl.TreeLevel = childBaseEl.TreeLevel + 1;
                                                        parEl.Text = divChild.OuterHtml;
                                                        var parEId = $"{childBaseEl.Id}__par_{matchNumber.Groups[1].Value}";
                                                        SetElementIdAndType(parEId, parEl);

                                                        childBaseEl.Childs.Add(parEl);
                                                    }
                                                    else
                                                    {
                                                        childBaseEl.Text += divChild.OuterHtml;
                                                    }
                                                }
                                                else if (divChild.Name.ToLower() == "table")
                                                {
                                                    SmeDocItem currEl = new SmeDocItem();
                                                    currEl.TreeLevel = childBaseEl.TreeLevel + 1;
                                                    currEl.Text = divChild.OuterHtml;
                                                    var currEId = string.Empty;

                                                    Match matchBegin = Regex.Match(divChild.InnerText.Trim(), @"^(\p{L})\)");

                                                    if (matchBegin.Success)
                                                    {
                                                        currEId = $"{(childBaseEl.Childs.Count > 0 ? childBaseEl.Childs.Last().Id : childBaseEl.Id)}__let_{matchBegin.Groups[1].Value}";
                                                    }
                                                    else
                                                    {
                                                        matchBegin = Regex.Match(divChild.InnerText.Trim(), @"^(\d+)\)");

                                                        if (matchBegin.Success)
                                                        {
                                                            currEId = $"{(childBaseEl.Childs.Count > 0 ? childBaseEl.Childs.Last().Id : childBaseEl.Id)}__pt_{matchBegin.Groups[1].Value}";
                                                        }
                                                        else
                                                        {
                                                            childBaseEl.Text += divChild.OuterHtml;
                                                        }
                                                    }

                                                    if (!string.IsNullOrWhiteSpace(currEId))
                                                    {
                                                        SetElementIdAndType(currEId, currEl);

                                                        if (childBaseEl.Childs.Count > 0)
                                                        {
                                                            currEl.TreeLevel = childBaseEl.Childs.Last().TreeLevel + 1;
                                                            childBaseEl.Childs.Last().Childs.Add(currEl);
                                                        }
                                                        else
                                                        {
                                                            childBaseEl.Childs.Add(currEl);
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    childBaseEl.Text += divChild.OuterHtml;
                                                }
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    GetAndSetChildsNodes(childBaseEl, childBaseNode);
                                }

                                currentBaseEl.Childs.Add(childBaseEl);
                            }
                        }
                    }
                }
            }
        }

        private static void SetElementIdAndType(string currBaseEId, SmeDocItem currentBaseEl)
        {
            Match matchType = Regex.Match(currBaseEId, @"([^_]+)_([^_]+)$");

            if (matchType.Success)
            {
                if (currBaseEId.StartsWith("art_"))
                {
                    currentBaseEl.Id = currBaseEId;
                }
                else
                {
                    currentBaseEl.Id = matchType.Value;
                }

                switch (matchType.Groups[1].Value)
                {
                    case "tit":
                        currentBaseEl.Type = SmeDocItemType.Title;
                        break;
                    case "rec":
                        currentBaseEl.Type = SmeDocItemType.Recital;
                        break;
                    case "sect":
                        currentBaseEl.Type = SmeDocItemType.Section;
                        break;
                    case "chap":
                        currentBaseEl.Type = SmeDocItemType.Chapter;
                        break;
                    case "part":
                        currentBaseEl.Type = SmeDocItemType.Part;
                        break;
                    case "art":
                        currentBaseEl.Type = SmeDocItemType.Article;
                        break;
                    case "par":
                        currentBaseEl.Type = SmeDocItemType.Paragraph;
                        break;
                    case "pt":
                        currentBaseEl.Type = SmeDocItemType.Point;
                        break;
                    case "sent":
                        currentBaseEl.Type = SmeDocItemType.Sentence;
                        break;
                    case "let":
                        currentBaseEl.Type = SmeDocItemType.Letter;
                        break;
                    case "num":
                        currentBaseEl.Type = SmeDocItemType.Number;
                        break;
                    default:
                        currentBaseEl.Type = SmeDocItemType.Text;
                        break;
                }
            }
        }
    }
}
