namespace SmeData.WebApi.Services.Documents
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;
    using System.Linq;
    using HtmlAgilityPack;
    using Apis.Common.Extensions;

    internal static class DocumentLinkRewrite
    {
        private static readonly Lazy<IReadOnlyDictionary<String, String>> map = new Lazy<IReadOnlyDictionary<string, string>>(LoadMap, true);

        public static String ReplaceNationalLegislation(String html)
        {
            var result = html;

            if (String.IsNullOrEmpty(html))
            {
                return result;
            }

            if (html.IndexOf("NatLegi=", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                var htmlDocument = new HtmlDocument { OptionEmptyCollection = true };
                htmlDocument.LoadHtml(html);

                var natLegiAnchors = htmlDocument.DocumentNode.SelectNodes(".//a[contains(@href, 'NatLegi=')]");

                foreach (var anchor in natLegiAnchors)
                {
                    var href = anchor.GetAttributeValue("href", String.Empty);
                    var code = href.Split('=')[1];
                    if (map.Value.TryGetValue(code, out var url))
                    {
                        anchor.SetAttributeValue("href", url);
                    }
                }

                result = htmlDocument.DocumentNode.OuterHtml;
            }

            return result;
        }

        private static IReadOnlyDictionary<String, String> LoadMap()
        {
            var map = (from l in File.ReadAllLines("./Data/nationalLegislationMap.txt")
                       let pair = l.Split('~')
                       select (key: pair[0], url: pair[1])
                      ).ToDictionary(x => x.key, x => x.url, StringComparer.OrdinalIgnoreCase);

            return map;
        }
    }
}
