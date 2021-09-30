using Microsoft.EntityFrameworkCore;
using Npgsql;
using NpgsqlTypes;
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

        public string GetShortLangByLangId(int langId)
        {
            using (var db = this.factory.CreateReadOnly())
            using (var command = db.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = "select short_lang from langs where id = @_lang_id";
                command.Parameters.Add(new Npgsql.NpgsqlParameter("@_lang_id", NpgsqlTypes.NpgsqlDbType.Integer) { Value = langId });
                if (command.Connection.State == ConnectionState.Closed)
                    command.Connection.Open();
                var res = (string)command.ExecuteScalar();
                return res;
            }
        }

        public string GetDocNumberByDocLangId(int doclangId)
        {
            using (var db = this.factory.CreateReadOnly())
            using (var command = db.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = "select doc_number from vw_doc_langs_mat where doc_lang_id = @_doc_lang_id";
                command.Parameters.Add(new Npgsql.NpgsqlParameter("@_doc_lang_id", NpgsqlTypes.NpgsqlDbType.Integer) { Value = doclangId });
                if (command.Connection.State == ConnectionState.Closed)
                    command.Connection.Open();
                var res = (string)command.ExecuteScalar();
                return res;
            }
        }

        private int GetDocLangIdByDocNumberAndLang(string docNumber, int langId)
        {
            using (var db = this.factory.CreateReadOnly())
            using (var command = db.Database.GetDbConnection().CreateCommand())
            {
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

                        return record == null ? 0 : record[0];
                    }
                    else
                    {
                        return 0;
                    }
                }
            }
        }
        public SpDocumentModel GetDocRecordByDocNumberAndLang(string docNumber, int langId)
        {
            using (var db = this.factory.CreateReadOnly())
            using (var command = db.Database.GetDbConnection().CreateCommand())
            {
                command.CommandType = CommandType.Text;

                command.CommandText = "SELECT public.get_last_cons_doc_lang_id(@_doc_number, @_from_lang_id, @_lang_id, 0, true)";
                command.Parameters.Add(new NpgsqlParameter("@_doc_number", NpgsqlDbType.Varchar) { Value = docNumber });
                command.Parameters.Add(new NpgsqlParameter("@_from_lang_id", NpgsqlDbType.Integer) { Value = langId });
                command.Parameters.Add(new NpgsqlParameter("@_lang_id", NpgsqlDbType.Integer) { Value = langId });
                if (command.Connection.State == ConnectionState.Closed)
                    command.Connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {

                        int docLangId = 0;
                        if (reader.IsDBNull(0))
                        {
                            docLangId = this.GetDocLangIdByDocNumberAndLang(docNumber, langId);
                        }
                        else
                        {
                            docLangId = reader.GetInt32(0);
                        }
                        return new SpDocumentModel
                        {
                            DocLanguageId = docLangId,
                            LangId = langId,
                            ShortLang = this.GetShortLangByLangId(langId),
                            DocNumber = this.GetDocNumberByDocLangId(docLangId)
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

        public String GetRecitalsPreambleForConsVersion(int docLangId)
        {
            using (var dbContext = this.factory.CreateReadOnly())
            using (var connection = dbContext.Database.GetDbConnection())
            using (var command = connection.CreateCommand())
            {
                command.CommandType = CommandType.Text;
                command.CommandText = "select x.doc_lang_id, x.par_text_id, x.content from public.get_missing_recitals_info(@cons_doc_lang_id) as x";
                command.Parameters.Add(new Npgsql.NpgsqlParameter("@cons_doc_lang_id", NpgsqlDbType.Integer) { Value = docLangId });
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }


                var recitalsDocLangId = 0;
                var preambleParTextId = 0;
                var preambleContent = (String)null;
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        recitalsDocLangId = reader.GetInt32(0);
                        preambleParTextId = reader.GetInt32(1);
                        preambleContent = reader.GetString(2);

                    }
                }

                return preambleContent;
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


        public LastChangeOfDoc GetLastChangeDocForConsByIdentifier(String identifier)
        {
            using (var connection = this.factory.CreateReadOnly().Database.GetDbConnection())
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "select x.identifier, x.eucases_change_date from public.get_cons_change_info(@cons_identifier) as x";
                command.Parameters.Add(new NpgsqlParameter("@cons_identifier", identifier));
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new LastChangeOfDoc
                        {
                            Ident = identifier,
                            NewIdent = reader.GetString(0),
                            LastChangeDate = reader.GetDateTime(1)
                        };
                    }
                    else
                    {
                        return null;
                    }
                }
            }
        }

        public IList<DocumentResponseModel> GetDocListData(int[] ids)
        {
            using (var db = this.factory.CreateReadOnly())
            using (var connection = db.Database.GetDbConnection())
            using (var command = connection.CreateCommand())
            {
                command.CommandType = CommandType.Text;
                command.CommandText = @"
                    select 
                        x.country,
                        x.doc_id,
                        x.doc_lang_id,
                        x.doc_type_flag,
                        x.full_title,
                        x.short_title,
                        x.lang_id,
                        x.publication_date,
                        x.original_lang,
                        x.identifier,
                        x.doc_number,
                        x.sub_title
                    from get_vw_doc_langs_by_ids(@ids) as x";

                var param = new NpgsqlParameter("@ids", NpgsqlDbType.Array | NpgsqlDbType.Integer);
                param.Value = ids;

                command.Parameters.Add(param);

                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }

                var result = new List<DocumentResponseModel>();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        result.Add(new DocumentResponseModel
                        {
                            Country = reader.GetString(0),
                            DocId = reader.GetInt32(1),
                            DocLangId = reader.GetInt32(2),
                            DocType = reader.GetInt32(3),
                            FullTitle = reader.GetValue<String>(4),
                            ShortTitle = reader.GetValue<String>(5),
                            LangId = reader.GetInt32(6),
                            PublicationDate = reader.GetValue<DateTime?>(7),
                            OriginalLang = reader.GetString(8),
                            DocIdentifier = reader.GetValue<String>(9),
                            DocNumber = reader.GetValue<String>(10),
                            SubTitle = reader.GetValue<String>(11),
                        });
                    }
                }

                return result;
            }
        }
    }

}
