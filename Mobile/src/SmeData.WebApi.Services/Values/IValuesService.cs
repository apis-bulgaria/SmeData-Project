using SmeData.SharedModels.ContactsDPAs;
using SmeData.SharedModels.Document;
using SmeData.SharedModels.Link;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SmeData.WebApi.Services.Values
{
    public interface IValuesService
    {
        string GetTranslations();
        List<SmeDocItem> GetGdprRecitalsEN();
        List<SmeDocItem> GetGdprRecitalsIT();
        List<SmeDocItem> GetGdprRecitalsBG();
        List<ContactDpaModel> GetContactsDpa();
        List<LinkModel> GetUsefulLinks();
        byte[] GetFileBytes(string guid, string fileName);

    }
}
