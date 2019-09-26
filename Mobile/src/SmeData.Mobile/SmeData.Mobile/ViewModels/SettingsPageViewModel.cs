using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Newtonsoft.Json;
using Plugin.Multilingual;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using SmeData.Mobile.Data;
using SmeData.Mobile.Helpers;
using SmeData.Mobile.Models;
using SmeData.Mobile.Models.Settings;
using SmeData.SharedModels.Language;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Windows.Input;

namespace SmeData.Mobile.ViewModels
{
    public class SettingsPageViewModel : BaseViewModel
    {
        private readonly AppRepository documentsRepository;

        public string SettingItemFont { get => $"t|20|{ScreenWidth}"; }

        public SettingsPageViewModel(INavigationService navigationService, AppRepository documentsRepository, SettingsModel settings) : base(navigationService)
        {
            this.documentsRepository = documentsRepository;
            this.settings = settings;
            this.currentLanguage = settings.Language.ToString();
        }

        public IList<string> LanguagesList
        {
            get { return Enum.GetNames(typeof(SmeLanguage)).ToList(); }
        }

        private string currentLanguage = SmeLanguage.English.ToString();

        public string CurrentLanguage
        {
            get => this.currentLanguage == null ? SmeLanguage.English.ToString() : this.currentLanguage.ToString();
            set
            {
                if (value == null)
                {
                    return;
                }
                this.currentLanguage = value;

                var culture = new CultureInfo("bg");
                switch (this.currentLanguage)
                {
                    case "Bulgarian":
                        culture = new CultureInfo("bg");
                        this.settings.Language = SmeLanguage.Bulgarian;
                        break;
                    case "Italian":
                        culture = new CultureInfo("it");
                        this.settings.Language = SmeLanguage.Italian;
                        break;
                    case "English":
                        culture = new CultureInfo("en-US");
                        this.settings.Language = SmeLanguage.English;
                        break;
                }

                this.documentsRepository.SetSettingsAsync(new Data.Models.SettingsDbModel() { Id = 1, SettingsJson = JsonConvert.SerializeObject(this.settings) });

                Thread.CurrentThread.CurrentUICulture = culture;
                CrossMultilingual.Current.CurrentCultureInfo = culture;
                Properties.Resources.Culture = culture;
                Translator.Instance.Invalidate();
                this.RaisePropertyChanged(nameof(this.CurrentLanguage));
            }
        }

        private bool isSwitchedToggled = true;
        public bool IsSwitchedToggled
        {
            get => isSwitchedToggled;
            set
            {
                isSwitchedToggled = value;
                this.RaisePropertyChanged(nameof(this.IsSwitchedToggled));
            }
        }
    }
}
