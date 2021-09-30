using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SmeData.WebApi.Data.Eucases
{
    [Table("glossary")]
    public class Glossary
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("group_id")]
        public string GroupId { get; set; }

        [Column("lang_id")]
        public int LangId { get; set; }

        [Column("lang")]
        public string Lang { get; set; }

        [Column("title")]
        public string Title { get; set; }

        [Column("description")]
        public string Description { get; set; }

        [Column("last_edit_date")]
        public DateTime LastEditDate { get; set; }
    }
}
