using Newtonsoft.Json;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;
using SmeData.Mobile.Data;
using SmeData.Mobile.Helpers;
using SmeData.Mobile.Models;
using SmeData.Mobile.Models.Settings;
using SmeData.Mobile.Services;
using SmeData.Shared.Common;
using SmeData.SharedModels.Language;
using SmeData.SharedModels.Link;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;

namespace SmeData.Mobile.ViewModels
{
    /// <summary>
    /// Page with useful link ralated to GDPR
    /// </summary>
    public class UsefulLinksPageViewModel : BaseViewModel
    {
        private readonly HttpService httpService;
        private readonly IPageDialogService dialogService;
        private readonly AppRepository documentsRepository;

        public ICommand GoToLinkCommand { get; set; }

        public string LinksFont { get => $"t|16|{ScreenWidth}"; }

        public string ImageFont { get => $"i|28|{ScreenWidth}"; }

        private ObservableCollection<LinkModel> links;
        public ObservableCollection<LinkModel> Links
        {
            get => links;
            set
            {
                links = value;
                this.RaisePropertyChanged(nameof(this.Links));
            }
        }

        public UsefulLinksPageViewModel(INavigationService navigationService, SettingsModel settings, HttpService httpService, IPageDialogService dialogService, AppRepository documentsRepository) : base(navigationService)
        {
            this.settings = settings;
            this.httpService = httpService;
            this.dialogService = dialogService;
            this.documentsRepository = documentsRepository;
            this.GoToLinkCommand = new DelegateCommand<string>(this.GoToLink);
        }

        private void GoToLink(string linkUrl)
        {
            Device.OpenUri(new Uri(linkUrl));
        }

        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            try
            {
                List<LinkModel> usefulLinks = null;

                try
                {
                    usefulLinks = JsonConvert.DeserializeObject<List<LinkModel>>(Compression.DecompressString((await this.documentsRepository.GetDocumentAsync("761ce7c6-0106-435d-90e9-a059519fa1cb")).JsonSmeDoc));
                }
                catch (Exception e)
                {
                }

                if (usefulLinks == null)
                {
                    if (!(await ConnectivityHelper.CheckInternetConection(this.dialogService)))
                    {
                        return;
                    }

                    usefulLinks = await httpService.GetUsefulkLinks();
                }

                switch (this.settings.Language)
                {
                    case SmeLanguage.Bulgarian:
                        Links = new ObservableCollection<LinkModel>(usefulLinks.Where(x => x.LangId == 1));
                        break;
                    case SmeLanguage.English:
                    default:
                        Links = new ObservableCollection<LinkModel>(usefulLinks.Where(x => x.LangId == 4));
                        break;
                    case SmeLanguage.Italian:
                        Links = new ObservableCollection<LinkModel>(usefulLinks.Where(x => x.LangId == 5));
                        break;
                    case SmeLanguage.German:
                        Links = new ObservableCollection<LinkModel>(usefulLinks.Where(x => x.LangId == 2));
                        break;
                    case SmeLanguage.French:
                        Links = new ObservableCollection<LinkModel>(usefulLinks.Where(x => x.LangId == 3));
                        break;
                }
            }
            catch (Exception e)
            {
            }
        }
    }
}
