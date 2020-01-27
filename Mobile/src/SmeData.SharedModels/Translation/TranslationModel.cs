using System;
using System.Collections.Generic;
using System.Text;

namespace SmeData.SharedModels.Translation
{
    public class TranslationModel
    {
        public string KeyName { get; set; }
        public string English { get; set; }
        public string Bulgarian { get; set; }
        public string Italian { get; set; }

        public TranslationModel(string keyName, string english, string bulgarian, string italian)
        {
            this.KeyName = keyName;
            this.English = english;
            this.Bulgarian = bulgarian;
            this.Italian = italian;
        }
    }
}
