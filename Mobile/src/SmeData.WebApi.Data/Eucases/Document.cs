using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SmeData.WebApi.Data.Eucases
{
    [Table("documents")]
    public class Document
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("doc_number")]
        public string DocNumber { get; set; }

        [Column("doc_type")]
        public int DocType { get; set; }
    }
}
