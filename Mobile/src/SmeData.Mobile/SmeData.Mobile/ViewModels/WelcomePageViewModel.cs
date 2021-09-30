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
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace SmeData.Mobile.ViewModels
{
    /// <summary>
    /// Home page of the application with navigation to all main catagories
    /// </summary>
    public class WelcomePageViewModel : BaseViewModel
    {
        private static bool isStartUp = true;
        private readonly IPageDialogService dialogService;
        private AppRepository documentsRepository;
        private readonly HttpService httpService;
        protected readonly DocumentService docService;

        /// <summary>
        /// Command which is executed when Legal Framework button is pressed
        /// </summary>
        public ICommand LegalFrameworkCommand { get; set; }

        /// <summary>
        /// Command which is executed when Legal Framework button is pressed
        /// </summary>
        public ICommand GuideForCitizensCommand { get; set; }

        /// <summary>
        /// Command which is executed when Legal Framework button is pressed
        /// </summary>
        public ICommand GuideForSmesCommand { get; set; }

        /// <summary>
        /// Command which is executed when Legal Framework button is pressed
        /// </summary>
        public ICommand GdprDictionaryCommand { get; set; }

        /// <summary>
        /// Command which is executed when Search button in toolbar is pressed
        /// </summary>
        public ICommand SearchPageCommand { get; set; }

        /// <summary>
        /// Information for font size calculation for text in main buttons
        /// </summary>
        public string MainButtonsFont { get => $"t|18|{ScreenWidth}"; }

        /// Information for size calculation for main imag
        /// </summary>
        public string MainTitleImageSide { get => $"i|40|{ScreenWidth}"; }

        /// <summary>
        /// Information for font size calculation for intro text
        /// </summary>
        public string SmeDataIntroTextFont { get => $"t|11|{ScreenWidth}"; }

        /// <summary>
        /// Information for font size calculation for main title
        /// </summary>
        public string MainTitleFont { get => $"t|32|{ScreenWidth}"; }

        public WelcomePageViewModel(INavigationService navigationService, IPageDialogService dialogService, AppRepository documentsRepository, HttpService httpService, DocumentService docService, SettingsModel settings) : base(navigationService)
        {
            this.dialogService = dialogService;
            this.documentsRepository = documentsRepository;
            this.httpService = httpService;
            this.docService = docService;
            this.settings = settings;

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
                //globalLog = Xamarin.Forms.DependencyService.Get<ICrossLogging>().ReturnLog();

                isStartUp = false;

                if (!settings.ShowMsgUpdates)
                {
                    var allDocsSaved = await documentsRepository.GetDocumentsAsync();
                    var allSystemDocs = allDocsSaved.Where(x => x.IsMainDoc).ToList();

                    if ((Device.RuntimePlatform == Device.UWP && Connectivity.NetworkAccess == NetworkAccess.Internet) || (Device.RuntimePlatform != Device.UWP && ConnectivityHelper.IsWifiConnected()))
                    {
                        List<LastChangeOfDoc> allSystemDocIdentsWithLastChange = new List<LastChangeOfDoc>();

                        foreach (var doc in allSystemDocs)
                        {
                            allSystemDocIdentsWithLastChange.Add(new LastChangeOfDoc() { Ident = doc.Identifier, LastChangeDate = doc.LastChangeDate });
                        }

                        List<LastChangeOfDoc> updatedSystemDocuments = new List<LastChangeOfDoc>();
                        updatedSystemDocuments = await httpService.GetUpdatedDocuments(allSystemDocIdentsWithLastChange);

                        if (updatedSystemDocuments.Count > 0)
                        {
                            foreach (var systemDoc in updatedSystemDocuments)
                            {
                                var updatedSystemDoc = await httpService.GetSmeDocByIdentifier(systemDoc.Ident, null);
                                documentsRepository.UpdateDocumentAsync(
                                    new DocumentModel()
                                    {
                                        Identifier = updatedSystemDoc.Meta.Idenitifier,
                                        Title = updatedSystemDoc.Meta.Title,
                                        IsMainDoc = true,
                                        IsToHide = true,
                                        LastChangeDate = updatedSystemDoc.Meta.LastChangeDate,
                                        JsonSmeDoc = Compression.CompressString(JsonConvert.SerializeObject(updatedSystemDoc))
                                    });
                            }
                        }
                    }

                    allDocsSaved = allDocsSaved.Where(x => !x.IsMainDoc).ToList();

                    List<LastChangeOfDoc> allDocIdentsWithLastChange = new List<LastChangeOfDoc>();

                    foreach (var doc in allDocsSaved)
                    {
                        allDocIdentsWithLastChange.Add(new LastChangeOfDoc() { Ident = doc.Identifier, LastChangeDate = doc.LastChangeDate });
                    }

                    if (Connectivity.NetworkAccess == NetworkAccess.Internet)
                    {
                        List<LastChangeOfDoc> updatedDocuments = await httpService.GetUpdatedDocuments(allDocIdentsWithLastChange);

                        if (updatedDocuments?.Count > 0)
                        {
                            var isAnswerYes = await this.dialogService.DisplayAlertAsync(Translator.GetString("Message"), Translator.GetString("UpdateDocumentExist"), Translator.GetString("Yes"), Translator.GetString("No"));

                            if (isAnswerYes)
                            {
                                await navigationService.NavigateAsync(nameof(Views.OfflineDocumentsPage));
                            }
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
