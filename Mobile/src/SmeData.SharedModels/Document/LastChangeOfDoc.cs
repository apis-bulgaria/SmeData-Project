using System;
using System.Collections.Generic;
using System.Text;

namespace SmeData.SharedModels.Document
{
    public class LastChangeOfDoc
    {
        public string Ident { get; set; }
        public string NewIdent { get; set; }
        public DateTime? LastChangeDate { get; set; }
    }
}
