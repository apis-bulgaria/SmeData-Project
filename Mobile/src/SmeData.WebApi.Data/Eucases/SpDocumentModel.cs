using System;
using System.Collections.Generic;
using System.Text;

namespace SmeData.WebApi.Data.Eucases
{
    public class SpDocumentModel
    {
        public int DocLanguageId { get; set; }
        public int LangId { get; set; }
        public string ShortLang { get; set; }
        public string DocNumber { get; set; }
    }
}
