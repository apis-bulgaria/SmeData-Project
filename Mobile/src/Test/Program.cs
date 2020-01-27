using System;
using SmeData.WebApi.Services.Documents;
using SmeData.WebApi.Data.Eucases;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            EuCasesContextFactorySettings connectionSettings = new EuCasesContextFactorySettings("Server=web.eucases.eu;Port=6432;Database=eurocases_live_4;User Id=postgres;Password=L1m0n@d@; Timeout=300");

            EucasesContextFactory aaa = new EucasesContextFactory(connectionSettings);

            DocumentsService service = new DocumentsService(aaa);

            //законодателство: 6101605, 6108904
            //практика: 4529366, 3790604, 4335906
            //open doc: 4912185

            service.GetDocument(1);
        }
    }
}
