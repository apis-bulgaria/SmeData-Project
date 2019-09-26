using System;
using System.Collections.Generic;
using System.Text;

namespace SmeData.WebApi.Models
{
    public class DocumentResponseModel
    {
        public int DocId { get; set; }
        public int DocLangId { get; set; }
        public int DocType { get; set; }
        public string FullTitle { get; set; }
        public string ShortTitle { get; set; }
        public string SubTitle { get; set; }
        public string Country { get; set; }
        public int LangId { get; set; }
        public string OriginalLang { get; set; }
        public DateTime? PublicationDate { get; set; }
        public string DocIdentifier { get; set; }
        public string DocNumber { get; set; }
    }
}
