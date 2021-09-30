using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SmeData.WebApi.Data.Eucases
{
    [Table("vw_doc_langs_mat")]
    public class VwDocLangsMat
    {
        [Column("doc_id")]
        public int DocId { get; set; }
        [Column("version_id")]
        public int VersionId { get; set; }
        [Key]
        [Column("doc_lang_id")]
        public int DocLangId { get; set; }
        //public DateTime? DocDate { get; set; }
        [Column("country")]
        public string Country { get; set; }
        [Column("doc_type")]
        public int DocType { get; set; }
        [Column("doc_number")]
        public string DocNumber { get; set; }
        [Column("lang_id")]
        public int LangId { get; set; }
        [Column("doc_identifier")]
        public string DocIdentifier { get; set; }
        [Column("publ_date")]
        public DateTime? PublDate { get; set; }
        [Column("full_title")]
        public string FullTitle { get; set; }
        [Column("short_title")]
        public string ShortTitle { get; set; }
        [Column("sub_title")]
        public string SubTitle { get; set; }
        [Column("lead_doc_lang_id")]
        public int LeadDocLangId { get; set; }
        [Column("source_url")]
        public string SourceUrl { get; set; }
        [Column("publisher")]
        public string Publisher { get; set; }
        [Column("date_of_effect")]
        public DateTime? DateOfEfect { get; set; }
        [Column("end_date")]
        public DateTime? EndDate { get; set; }
    }
}

