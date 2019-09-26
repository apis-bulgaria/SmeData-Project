using System;
using System.Collections.Generic;
using System.Text;

namespace SmeData.WebApi.Services.DecisionSupport
{
    public interface IDecisionSupportService
    {
        string GetQuestionary(int id);
    }
}
