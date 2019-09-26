using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SmeData.WebApi.Data.Eucases
{
    [Table("doc_langs")]
    public class DocLanguage
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("doc_version_id")]
        public int DocVersionId { get; set; }
        [Column("doc_dentifier")]
        public string DocIdentifier { get; set; }
        [Column("lang_id")]
        public int LangId { get; set; }
        [Column("original_lang")]
        public string OriginalLang { get; set; }
        [Column("country")]
        public string Country { get; set; }
        [Column("doc_date")]
        public DateTime? DocumentDate { get; set; }
        [Column("publ_date")]
        public DateTime? PublicationDate { get; set; }
        [Column("date_of_effect")]
        public DateTime? DateOfEffect { get; set; }
        [Column("end_date")]
        public DateTime? EndDate { get; set; }
        [Column("eucases_change_date")]
        public DateTime? EuCasesChangeDate { get; set; }
        [Column("full_title")]
        public string FullTitle{ get; set; }
        [Column("source_url")]
        public string SourceUrl { get; set; }
        [Column("publisher")]
        public string Publisher { get; set; }
    }
}
