using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SmeData.WebApi.Data.Eucases
{
    [Table("par_texts")]
    public class ParText
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("eid")]
        public string Eid { get; set; }
        [Column("num")]
        public string Num { get; set; }
        [Column("heading")]
        public string Heading { get; set; }
        [Column("sub_heading")]
        public string SubHeading { get; set; }
        [Column("attribs")]
        public string Attribs { get; set; }
        [Column("content")]
        public string Content { get; set; }
    }
}
