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
    /// <summary>
    /// Page with setting of the application
    /// </summary>
    public class SettingsPageViewModel : BaseViewModel
    {
        private readonly AppRepository documentsRepository;

        public string SettingItemFont { get => $"t|17|{ScreenWidth}"; }

        public SettingsPageViewModel(INavigationService navigationService, AppRepository documentsRepository, SettingsModel settings) : base(navigationService)
        {
            this.documentsRepository = documentsRepository;
            this.settings = settings;
            this.currentLanguage = settings.Language.ToString() == "Bulgarian" ? "Български" : (settings.Language.ToString() == "Italian" ? "Italiano" : (settings.Language.ToString() == "German" ? "Deutsch" : (settings.Language.ToString() == "French" ? "Français" : "English")));
        }

        public override async void OnNavigatedFrom(INavigationParameters parameters)
        {
            base.OnNavigatedFrom(parameters);

            await this.documentsRepository.SetSettingsAsync(new Data.Models.SettingsDbModel() { Id = 1, SettingsJson = JsonConvert.SerializeObject(this.settings) });
        }

        public IList<string> LanguagesList
        {
            get { return new List<string> { "Български", "English", "Italiano", "Deutsch", "Français" }; }
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
                    case "Български":
                    case "Bulgarian":
                        culture = new CultureInfo("bg");
                        this.settings.Language = SmeLanguage.Bulgarian;
                        break;
                    case "Italiano":
                    case "Italian":
                        culture = new CultureInfo("it");
                        this.settings.Language = SmeLanguage.Italian;
                        break;
                    case "English":
                        culture = new CultureInfo("en-US");
                        this.settings.Language = SmeLanguage.English;
                        break;
                    case "Deutsch":
                    case "German":
                        culture = new CultureInfo("de");
                        this.settings.Language = SmeLanguage.German;
                        break;
                    case "Français":
                    case "French":
                        culture = new CultureInfo("fr");
                        this.settings.Language = SmeLanguage.French;
                        break;
                }

                Thread.CurrentThread.CurrentUICulture = culture;
                CrossMultilingual.Current.CurrentCultureInfo = culture;
                Properties.Resources.Culture = culture;
                Translator.Instance.Invalidate();
                this.RaisePropertyChanged(nameof(this.CurrentLanguage));
            }
        }

        private bool isWifiOptionToggled;
        public bool IsWifiOptionToggled
        {
            get => isWifiOptionToggled = this.settings.ShowDocsOnWifiOnly;
            set
            {
                isWifiOptionToggled = value;
                this.settings.ShowDocsOnWifiOnly = isWifiOptionToggled;
                this.RaisePropertyChanged(nameof(this.IsWifiOptionToggled));
            }
        }

        private bool isMsgUpdatesOptionToggled;
        public bool IsMsgUpdatesOptionToggled
        {
            get => isMsgUpdatesOptionToggled = this.settings.ShowMsgUpdates;
            set
            {
                isMsgUpdatesOptionToggled = value;
                this.settings.ShowMsgUpdates = isMsgUpdatesOptionToggled;
                this.RaisePropertyChanged(nameof(this.IsMsgUpdatesOptionToggled));
            }
        }
    }
}
