using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmeData.WebApi.Data.Eucases
{
    public class EucasesContextFactory : IEuCasesContextFactory
    {
        private readonly EuCasesContextFactorySettings factorySettings;
        public EucasesContextFactory(EuCasesContextFactorySettings settings)
        {
            this.factorySettings = settings;
        }
        public EuCasesContext Create()
        {
            return new EuCasesContext(this.factorySettings.ConnectionString);
        }

        public EuCasesContext CreateReadOnly()
        {
            var res = new EuCasesContext(this.factorySettings.ConnectionString);
            res.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            return res;
        }
    }
}
