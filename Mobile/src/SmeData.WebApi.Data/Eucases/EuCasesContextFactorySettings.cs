using System;
using System.Collections.Generic;
using System.Text;

namespace SmeData.WebApi.Data.Eucases
{
    public class EuCasesContextFactorySettings
    {
        public string ConnectionString { get; }

        public EuCasesContextFactorySettings(string connectionString)
        {
            this.ConnectionString = connectionString;
        }
    }
}
