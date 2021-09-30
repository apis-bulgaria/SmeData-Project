using System;
using System.Collections.Generic;
using System.Text;

namespace SmeData.WebApi.Models
{
    public class SearchResultModel
    {
        public int PageNum { get; set; }
        public int PageSize { get; set; }

        public int TotalCount { get; set; }
        public IList<DocumentResponseModel> Data { get; set; } = new List<DocumentResponseModel>();

    }
}
