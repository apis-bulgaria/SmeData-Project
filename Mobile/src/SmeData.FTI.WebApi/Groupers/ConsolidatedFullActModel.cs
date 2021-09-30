namespace SmeData.FTI.WebApi.Groupers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class ConsolidatedFullActModel
    {
        public int DocLangId { get; set; }
        public int LeadDocLangId { get; set; }
        public int LangId { get; set; }
        public bool IsBase { get; set; }
    }
}
