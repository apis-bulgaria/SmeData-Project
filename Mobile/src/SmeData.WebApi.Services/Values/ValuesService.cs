using Newtonsoft.Json;
using SmeData.SharedModels.ContactsDPAs;
using SmeData.SharedModels.Link;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmeData.WebApi.Services.Values
{
    public class ValuesService : IValuesService
    {
        private readonly string basePath;

        public ValuesService(IPathsProvider pathsProvider)
        {
            this.basePath = pathsProvider.PdfPath;
        }

        public string GetTranslations()
        {
            var fileName = @".\Data\translations.csv";
            var res = System.IO.File.ReadAllText(fileName);
            return res;
        }

        public List<ContactDpaModel> GetContactsDpa()
        {
            var fileName = @".\Data\contactsDPAs.json";
            var res = JsonConvert.DeserializeObject<List<ContactDpaModel>>(System.IO.File.ReadAllText(fileName));
            return res;
        }

        public List<LinkModel> GetUsefulLinks()
        {
            var fileName = @".\Data\usefulLinks.json";
            var res = JsonConvert.DeserializeObject<List<LinkModel>>(System.IO.File.ReadAllText(fileName));
            return res;
        }

        public byte[] GetFileBytes(string guid, string fileName)
        {
            var filePath = System.IO.Path.Combine(this.basePath, guid, fileName);
            if (System.IO.File.Exists(filePath))
            {
                return System.IO.File.ReadAllBytes(filePath);
            }else
            {
                return null;
            }
        }
    }
}
