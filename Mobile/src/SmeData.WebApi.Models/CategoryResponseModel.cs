using System;
using System.Collections.Generic;
using System.Text;

namespace SmeData.WebApi.Models
{
    public class CategoryResponseModel
    {
        public string Heading { get; set; }
        public List<DocumentResponseModel> Items = new List<DocumentResponseModel>();
    }
}
