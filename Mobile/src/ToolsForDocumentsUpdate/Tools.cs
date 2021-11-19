using SmeData.SharedModels.Language;
using SmeData.WebApi.Data.Eucases;
using SmeData.WebApi.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmeData.ToolsForDocumentsUpdate
{
    public class Tools
    {
        private readonly IEuCasesContextFactory factory;
        private IPathsProvider pathsProvider;

        public Tools()
        {
            var cs = @"Server=********;Database=*********;User Id=*********;Password=**********; Timeout=300";
            this.factory = new EucasesContextFactory(new EuCasesContextFactorySettings(cs));
            this.pathsProvider = new PathsProvider()
            {
                BasePath = AppDomain.CurrentDomain.BaseDirectory
            };
        }

        public void AddGdprDictionaryTerm(int langId, string langAbr, string groupIdent, string term, string termDescription)
        {
            using (var db = this.factory.Create())
            {
                Glossary forAdd = new Glossary();
                forAdd.GroupId = groupIdent;
                forAdd.LangId = langId;
                forAdd.Lang = langAbr;
                forAdd.Title = term;
                forAdd.Description = termDescription;

                db.Glossary.Add(forAdd);
                db.SaveChanges();
            }
        }
    }
}
