using Microsoft.EntityFrameworkCore;
using SmeData.WebApi.Data.Eucases;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace SmeData.WebApi.Services.EuCaselawFilter
{
    public class EuCaselawFilterService: IEuCaselawFilterService
    {
        private Dictionary<int, int> eucasesFilter;
        private IEuCasesContextFactory dbFactory;
        public EuCaselawFilterService(IEuCasesContextFactory factory)
        {
            this.dbFactory = factory;
            this.Init();
        }

        private void Init()
        {
            this.eucasesFilter = this.GetEuCaselawFilter();
        }

        public Dictionary<int, int> GetEuCaselawFilter()
        {
            var res = new Dictionary<int, int>();
            using (var db = this.dbFactory.CreateReadOnly())
            using (var connection = db.Database.GetDbConnection())
            using (var command = connection.CreateCommand())
            {
                connection.Open();
                command.CommandType = CommandType.Text;
                command.CommandText = @"
                select distinct dj.doc_lang_id, COALESCE(dlm.doc_lang_id, dj.doc_lang_id) 
                from doc_judgments dj
                join vw_doc_langs_mat vw on vw.doc_lang_id = dj.doc_lang_id
                left join vw_doc_langs_mat dlm on dlm.doc_id = dj.lead_doc_id and dlm.lang_id = vw.lang_id";
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        res[reader.GetInt32(0)] = reader.GetInt32(1);
                    }
                }
            }

            return res;
        }

        public int[] Filter(int[] ids)
        {
            if(ids == null)
            {
                return ids;
            }
            var res = new List<int>();
            var hsAdded = new HashSet<int>();
            for (int i = 0; i < ids.Length; i++)
            {
                var id = this.eucasesFilter.ContainsKey(ids[i]) ? eucasesFilter[ids[i]] : ids[i];
                if (!hsAdded.Contains(id))
                {
                    res.Add(id);
                    hsAdded.Add(id);
                }
            }
            return res.ToArray();
        }

        public void Refresh()
        {
            this.Init();
        }
    }
}
