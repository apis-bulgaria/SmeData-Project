using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmeData.Mobile.Data.Models
{
    public class SettingsDbModel
    {
        [PrimaryKey]
        public int Id { get; set; }
        public string SettingsJson { get; set; }
    }
}
