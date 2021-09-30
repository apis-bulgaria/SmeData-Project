using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SmeData.WebApi.Data.Eucases
{
    [Table("doc_pars")]
    public class DocPar
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("doc_lang_id")]
        public int DocLangId { get; set; }
        [Column("ord")]
        public int Ord { get; set; }
        [Column("parent_doc_par_id")]
        public int ParentDocParId { get; set; }
        [Column("main_tag")]
        public int MainTag { get; set; }
        [Column("sub_type")]
        public int SubType { get; set; }
        [Column("par_text_id")]
        public int ParTextId { get; set; }
    }
}
