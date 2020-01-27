using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmeData.Mobile.Data.Models
{
    
    public class DocumentModel
    {
        [PrimaryKey]
        public string Identifier { get; set; }

        public string Title { get; set; }

        public bool IsMainDoc { get; set; }

        public bool IsToHide { get; set; }

        public string JsonSmeDoc { get; set; }

        public DateTime? LastChangeDate { get; set; }

        public override string ToString()
        {
            return string.Format($"({LastChangeDate}) {Title}");
        }
    }
}
