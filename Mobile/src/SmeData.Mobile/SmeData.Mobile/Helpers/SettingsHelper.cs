using Newtonsoft.Json;
using Plugin.Multilingual;
using SmeData.Mobile.Data;
using SmeData.Mobile.Data.Models;
using SmeData.Mobile.Models.Settings;
using SmeData.SharedModels.Language;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Threading.Tasks;

namespace SmeData.Mobile.Helpers
{
    public static class SettingsHelper
    {
        public static SettingsModel LoadFromDb(AppRepository repository)
        {
            SettingsDbModel settingInDb = repository.GetSettingsAsync(1).Result;

            if (settingInDb == null)
            {
                var cc = CrossMultilingual.Current.CurrentCultureInfo;
                var lang = TranslationManager.GetLangByCulture(cc);
                settingInDb = new SettingsDbModel() { Id = 1, SettingsJson = JsonConvert.SerializeObject(new SettingsModel() { Language = lang}) };
                repository.SetSettingsAsync(settingInDb);
            }

            var res = new SettingsModel
            {
                Language = JsonConvert.DeserializeObject<SettingsModel>(settingInDb.SettingsJson).Language
            };

            switch (res.Language)
            {
                case SmeLanguage.Bulgarian:
                    Properties.Resources.Culture = new CultureInfo("bg");
                    
                    break;
                case SmeLanguage.English:
                    Properties.Resources.Culture = new CultureInfo("en-US");
                    break;
                case SmeLanguage.Italian:
                    Properties.Resources.Culture = new CultureInfo("it");
                    break;
                default:
                    break;
            }
            CrossMultilingual.Current.CurrentCultureInfo = Properties.Resources.Culture;
            return res;
        }
    }
}
