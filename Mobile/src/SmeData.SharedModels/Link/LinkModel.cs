using System;
using System.Collections.Generic;
using System.Text;

namespace SmeData.SharedModels.Link
{
    public class LinkModel
    {

        public string LinkText { get; set; }

        public string LinkUrl { get; set; }

        public int LangId { get; set; }

        public LinkModel(string linkText, string linkUrl, int langId)
        {
            LinkText = linkText;
            LinkUrl = linkUrl;
            LangId = langId;
        }
    }
}
