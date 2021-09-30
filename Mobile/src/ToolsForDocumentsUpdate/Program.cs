using SmeData.SharedModels.Language;
using SmeData.WebApi.Data.Eucases;
using SmeData.WebApi.Services;
using System;
using System.IO;
using System.Linq;

namespace SmeData.ToolsForDocumentsUpdate
{
    public class Program
    {
        public static void Main()
        {
            Tools tools = new Tools();
            var allLines = File.ReadAllLines(@"C:\Users\Lazarov\Desktop\Smedata - translation\New entries\DicFR.csv");

            foreach (var line in allLines)
            {
                var parts = line.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

                if (parts.Count() > 3)
                {
                    Console.WriteLine();
                }

                tools.AddGdprDictionaryTerm(3, "FR", parts[0], parts[1], parts[2]);
            }

            Console.WriteLine();
        }
    }
}
