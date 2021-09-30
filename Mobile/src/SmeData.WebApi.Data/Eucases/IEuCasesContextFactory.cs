using System;
using System.Collections.Generic;
using System.Text;

namespace SmeData.WebApi.Data.Eucases
{
    public interface IEuCasesContextFactory
    {
        EuCasesContext Create();
        EuCasesContext CreateReadOnly();
    }
}
