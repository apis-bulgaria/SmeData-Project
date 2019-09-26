using Newtonsoft.Json;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using SmeData.Mobile.Data;
using SmeData.Mobile.Data.Models;
using SmeData.Mobile.Helpers;
using SmeData.Mobile.Models.Settings;
using SmeData.Mobile.Services;
using SmeData.Shared.Common;
using SmeData.SharedModels.Document;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace SmeData.Mobile.ViewModels
{
    public class WelcomePageViewModel : BaseViewModel
    {
        private static bool isStartUp = true;
        private readonly IPageDialogService dialogService;
        private readonly AppRepository documentsRepository;
        private readonly HttpService httpService;

        public ICommand LegalFrameworkCommand { get; set; }
        public ICommand GuideForCitizensCommand { get; set; }
        public ICommand GuideForSmesCommand { get; set; }
        public ICommand GdprDictionaryCommand { get; set; }
        public ICommand SearchPageCommand { get; set; }

        public string MainButtonsFont { get => $"t|18|{ScreenWidth}"; }
        public string MainTitleImageSide { get => $"i|40|{ScreenWidth}"; }
        public string SmeDataIntroTextFont { get => $"t|12|{ScreenWidth}"; }
        public string MainTitleFont { get => $"t|32|{ScreenWidth}"; }

        public WelcomePageViewModel(INavigationService navigationService, IPageDialogService dialogService, AppRepository documentsRepository, HttpService httpService, DocumentService docService) : base(navigationService)
        {
            this.dialogService = dialogService;
            this.documentsRepository = documentsRepository;
            this.httpService = httpService;

            this.LegalFrameworkCommand = new DelegateCommand(this.ShowLegalFramework);
            this.GuideForCitizensCommand = new DelegateCommand(this.ShowGuideForCitizens);
            this.GuideForSmesCommand = new DelegateCommand(this.ShowGuideForSmes);
            this.GdprDictionaryCommand = new DelegateCommand(this.ShowGdprDictionary);
            this.SearchPageCommand = new DelegateCommand(this.ShowSearchPage);
        }

        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            if (isStartUp)
            {
                //if (!(await ConnectivityHelper.CheckInternetConection(this.dialogService)))
                //{
                //    return;
                //}
                isStartUp = false;

                var allDocsSaved = await documentsRepository.GetDocumentsAsync();
                List<LastChangeOfDoc> allDocIdentsWithLastChange = new List<LastChangeOfDoc>();

                foreach (var doc in allDocsSaved)
                {
                    allDocIdentsWithLastChange.Add(new LastChangeOfDoc() { Ident = doc.Identifier, LastChangeDate = doc.LastChangeDate });
                }

                if (Connectivity.NetworkAccess == NetworkAccess.Internet)
                {
                    List<LastChangeOfDoc> updatedDocuments = await httpService.GetUpdatedDocuments(allDocIdentsWithLastChange);
                    //List<LastChangeOfDoc> updatedDocuments = new List<LastChangeOfDoc>() { new LastChangeOfDoc() { Ident = "51300c8d-df7b-4924-9e00-7538863d5324", LastChangeDate = null } };

                    if (updatedDocuments?.Count > 0)
                    {
                        //string message = $"There are updates on {updatedDocuments.Count} documents, you've saved on device. Do you want to navigate ot \"Saved documents\" page?";
                        //if (updatedDocuments.Count == 1)
                        //{
                        //    message = "There is update on 1 document, you've saved on device. Do you want to navigate ot \"Saved documents\" page?";
                        //}

                        var isAnswerYes = await this.dialogService.DisplayAlertAsync(Translator.GetString("Message"), Translator.GetString("UpdateDocumentExist"), 
                            Translator.GetString("Yes"), Translator.GetString("No"));

                        if (isAnswerYes)
                        {
                            await navigationService.NavigateAsync(nameof(Views.OfflineDocumentsPage));
                        }
                    }
                }
            }
        }

        private void ShowGdprDictionary()
        {
            this.navigationService.NavigateAsync("GdprDictionaryPage");
        }

        private void ShowGuideForSmes()
        {
            this.navigationService.NavigateAsync("GuideForSmesPage");
        }

        private void ShowLegalFramework()
        {
            navigationService.NavigateAsync("LegalFrameworkPage");
        }

        private void ShowGuideForCitizens()
        {
            navigationService.NavigateAsync("GuideForCitizensPage");
        }

        private void ShowSearchPage()
        {
            navigationService.NavigateAsync("SearchPage");
        }
    }
}
