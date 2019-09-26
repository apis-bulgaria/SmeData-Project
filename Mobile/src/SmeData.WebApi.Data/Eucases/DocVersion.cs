using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SmeData.WebApi.Data.Eucases
{
    [Table("doc_versions")]
    public class DocVersion
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("doc_id")]
        public int DocId { get; set; }
        [Column("doc_date")]
        public DateTime DocDate { get; set; }
    }
}
