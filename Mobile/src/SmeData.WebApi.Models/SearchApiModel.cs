using System;
using System.Collections.Generic;
using System.Text;

namespace SmeData.WebApi.Models
{
    public class SearchApiModel
    {
        public int PageNum { get; set; }
        public int PageSize { get; set; }

        public string SearchText { get; set; }
        public List<string> Classifiers { get; set; }

        public int[] LangPreferences { get; set; }
    }
}
