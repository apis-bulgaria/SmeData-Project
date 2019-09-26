using Microsoft.EntityFrameworkCore;
using SmeData.SharedModels.Document;
using SmeData.WebApi.Data.Eucases;
using SmeData.WebApi.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace SmeData.WebApi.Services.Documents
{
    public class DocumentsServiceDbHelper
    {
        private readonly IEuCasesContextFactory factory;
        private IPathsProvider pathsProvider;
        public DocumentsServiceDbHelper(IEuCasesContextFactory factory, IPathsProvider pathsProvider)
        {
            this.factory = factory;
            this.pathsProvider = pathsProvider;
        }

        public string GetDocumentTextByDocLangId(int docLangId)
        {
            using (var db = this.factory.CreateReadOnly())
            using (var command = db.Database.GetDbConnection().CreateCommand())
            {
                command.CommandType = CommandType.Text;
                command.CommandText = "Select public.get_doc_text(@dlId, @pt)";
                command.Parameters.Add(new Npgsql.NpgsqlParameter("@dlId", NpgsqlTypes.NpgsqlDbType.Integer)
                { Value = docLangId });
                command.Parameters.Add(new Npgsql.NpgsqlParameter("@pt", NpgsqlTypes.NpgsqlDbType.Boolean)
                { Value = false });
                if (command.Connection.State == ConnectionState.Closed)
                    command.Connection.Open();
                var res = (string)command.ExecuteScalar();
                return res;
            }
        }

        public SpDocumentModel GetDocRecordByDocNumberAndLang(string docNumber, int langId)
        {
            using (var db = this.factory.CreateReadOnly())
            using (var command = db.Database.GetDbConnection().CreateCommand())
            {
                command.CommandType = CommandType.Text;
                command.CommandText = "SELECT public.get_document_by_doc_number(@_doc_number, @_lang_id, @_user_id)";
                command.Parameters.Add(new Npgsql.NpgsqlParameter("@_doc_number", NpgsqlTypes.NpgsqlDbType.Varchar) { Value = docNumber });
                command.Parameters.Add(new Npgsql.NpgsqlParameter("@_lang_id", NpgsqlTypes.NpgsqlDbType.Integer) { Value = langId });
                command.Parameters.Add(new Npgsql.NpgsqlParameter("@_user_id", NpgsqlTypes.NpgsqlDbType.Integer) { Value = 0 });
                if (command.Connection.State == ConnectionState.Closed)
                    command.Connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        dynamic record = reader.GetValue(0);
                        return new SpDocumentModel
                        {
                            DocLanguageId = record[0],
                            LangId = record[1],
                            ShortLang = record[2],
                            DocNumber = docNumber
                        };
                    }
                }
                return new SpDocumentModel { DocLanguageId = -1 };
            }
        }

        public SpDocumentModel GetDocRecordByIdentifier(string identifier)
        {
            using (var db = this.factory.CreateReadOnly())
            using (var command = db.Database.GetDbConnection().CreateCommand())
            {
                command.CommandType = CommandType.Text;
                command.CommandText = "SELECT public.get_document_by_doc_identifier(@_doc_identifier)";
                command.Parameters.Add(new Npgsql.NpgsqlParameter("@_doc_identifier", NpgsqlTypes.NpgsqlDbType.Varchar)
                { Value = identifier });
                if (command.Connection.State == ConnectionState.Closed)
                    command.Connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        dynamic record = reader.GetValue(0);
                        return new SpDocumentModel
                        {
                            DocLanguageId = record[0],
                            LangId = record[1],
                            ShortLang = record[2],
                            DocNumber = record[4]
                        };


                    }
                }
                return new SpDocumentModel { DocLanguageId = -1 };
            }
        }

        public List<GdprDictionaryResponseModel> GdprDictionary(int langId)
        {
            using (var db = this.factory.CreateReadOnly())
            {
                var res = db.Glossary.Where(x => x.LangId == langId).OrderBy(x => x.Id).Select(x => new GdprDictionaryResponseModel
                {
                    Id = x.Id,
                    Title = x.Title,
                    Description = x.Description
                }).ToList();

                return res;
            }
        }

        public Dictionary<string, DateTime> GetIdentsAndDates(IList<LastChangeOfDoc> docsForCheck)
        {
            var res = new Dictionary<string, DateTime>();
            using (var db = this.factory.CreateReadOnly())
            using (var command = db.Database.GetDbConnection().CreateCommand())
            {
                command.CommandType = CommandType.Text;
                command.CommandText = @"select dl.doc_identifier, dl.eucases_change_date
                    from public.doc_langs dl 
                    join unnest(@_doc_idents) d(ident) on d.ident = dl.doc_identifier";
                command.Parameters.Add(new Npgsql.NpgsqlParameter("@_doc_idents", NpgsqlTypes.NpgsqlDbType.Array | NpgsqlTypes.NpgsqlDbType.Varchar)
                { Value = docsForCheck.Select(x => $"{x.Ident}").ToArray() });
                if (command.Connection.State == ConnectionState.Closed)
                    command.Connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var ident = reader.GetString(0);
                        res[ident] = (reader.IsDBNull(1)) ? res[ident] = DateTime.MinValue : reader.GetDateTime(1);
                    }
                }
                return res;
            }
        }

        public void UpdateLastChangeDoc(LastChangeOfDoc doc)
        {
            using (var db = this.factory.CreateReadOnly())
            using (var command = db.Database.GetDbConnection().CreateCommand())
            {
                command.CommandType = CommandType.Text;
                command.CommandText = @"update doc_langs set eucases_change_date = @_eu_lc_date where doc_langs.doc_identifier = @_ident";
                command.Parameters.Add(new Npgsql.NpgsqlParameter("@_eu_lc_date", NpgsqlTypes.NpgsqlDbType.Date)
                { Value = doc.LastChangeDate });
                command.Parameters.Add(new Npgsql.NpgsqlParameter("@_ident", NpgsqlTypes.NpgsqlDbType.Varchar)
                { Value = doc.Ident });
                if (command.Connection.State == ConnectionState.Closed)
                    command.Connection.Open();
                command.ExecuteNonQuery();

            }
        }

        public IList<DocumentResponseModel> GetDocListData(int[] ids)
        {
            using (var db = this.factory.CreateReadOnly())
            {
                var hs = new HashSet<int>(ids);
                var res = db.VwDocLangsMats.Where(x => hs.Contains(x.DocLangId)).Select(x => new DocumentResponseModel
                {
                    Country = x.Country,
                    DocId = x.DocId,
                    DocLangId = x.DocLangId,
                    DocType = x.DocType,
                    FullTitle = x.FullTitle,
                    ShortTitle = string.IsNullOrEmpty(x.ShortTitle) ? x.FullTitle : x.ShortTitle, //must change after ShortTitle column is available
                    LangId = x.LangId,
                    PublicationDate = x.PublDate,
                    OriginalLang = x.Country,
                    DocIdentifier = x.DocIdentifier,
                    DocNumber = x.DocNumber,
                    SubTitle = x.SubTitle,
                }).ToList();

                return res;
            }
        }
    }

}
