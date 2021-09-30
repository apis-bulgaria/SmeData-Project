using Microsoft.EntityFrameworkCore;
using System;

namespace SmeData.WebApi.Data.Eucases
{
    public partial class EuCasesContext : DbContext
    {
        private readonly string connectionString;

        public EuCasesContext(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public EuCasesContext(DbContextOptions<EuCasesContext> options)
            : base(options)
        {
            this.Database.SetCommandTimeout(100000);
        }
        public virtual DbSet<Document> Docs { get; set; }
        public virtual DbSet<DocLanguage> DocLanguages { get; set; }
        public virtual DbSet<DocVersion> DocVersions { get; set; }
        public virtual DbSet<DocPar> DocPars { get; set; }
        public virtual DbSet<ParText> ParTexts { get; set; }
        public virtual DbSet<VwDocLangsMat> VwDocLangsMats { get; set; }
        public virtual DbSet<Glossary> Glossary { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(this.connectionString);
        }

        [DbFunction("get_doc_text", "public")]
        public static int GetDocText(int _doc_lnag_id, bool _plain_xml)
        {
            throw new NotImplementedException();
        }
    }
}
