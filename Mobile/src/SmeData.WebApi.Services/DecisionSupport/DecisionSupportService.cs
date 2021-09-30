using System;
using System.Collections.Generic;
using System.Text;

namespace SmeData.WebApi.Services.DecisionSupport
{
    public class DecisionSupportService : IDecisionSupportService

    {
        public string GetQuestionary(int id)
        {
            return System.IO.File.ReadAllText(@".\data\dsm.html");
        }
    }
}
