using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using SmeData.Mobile.Helpers;
using SmeData.Mobile.Models.Grouping;
using SmeData.Mobile.Models.Settings;
using SmeData.Mobile.Services;
using SmeData.WebApi.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;

namespace SmeData.Mobile.ViewModels
{
    /// <summary>
    /// Common page for showing lists of legislation documents
    /// </summary>
    public abstract class CommonLegislationPageViewModel : BaseViewModel
    {
        protected ObservableCollection<LegislationGroupItem> legislationDocs = new ObservableCollection<LegislationGroupItem>();
        protected readonly HttpService httpService;
        protected readonly IPageDialogService dialogService;
        private readonly SynchronizationContext synchronizationContext;

        private bool isLoading;
        public ICommand TabCommand { get; set; }

        /// <summary>
        /// Information for font size calculation for heading in tab
        /// </summary>
        public string DocInTabFont { get => $"t|16|{ScreenWidth}"; }

        /// <summary>
        /// Information for font size calculation for category headings in the list
        /// </summary>
        public string CategoryHeadingFont { get => $"t|18|{ScreenWidth}"; }

        public CommonLegislationPageViewModel(HttpService service, INavigationService navigationService, IPageDialogService dialogService, SettingsModel settings) : base(navigationService)
        {
            this.TabCommand = new DelegateCommand<object>(this.ShowDocument);
            this.httpService = service;
            this.dialogService = dialogService;
            this.settings = settings;
            this.synchronizationContext = SynchronizationContext.Current;
        }

        /// <summary>
        /// Override OnNavigatedTo method with added logic for load legislation documents
        /// </summary>
        /// <param name="parameters">Input parameters</param>
        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            
            Task.Run(()=> GetAllLegislationDocs());
        }

        /// <summary>
        /// Method for navigating to selected document detailed view
        /// </summary>
        /// <param name="doc">Selected document</param>
        private void ShowDocument(object doc)
        {
            if (doc == null)
            {
                return;
            }

            string pageToNavigate = "DocMainPage";

            if ((doc as DocumentResponseModel).DocType != 2)
            {
                pageToNavigate = "DocCaseLawShowPage";
            }

            if (doc is DocumentResponseModel docResp)
            {
                if (!string.IsNullOrEmpty(docResp.DocIdentifier))
                {
                    this.navigationService.NavigateAsync($"{pageToNavigate}?{UrlNavHelper.IDENTIFIER}={docResp.DocIdentifier}&{UrlNavHelper.FULL_TITLE}={Uri.EscapeDataString(docResp.FullTitle)}");
                }
                else
                {
                    this.navigationService.NavigateAsync($"{pageToNavigate}?{UrlNavHelper.DOC_NUM}={docResp.DocNumber}&{UrlNavHelper.FULL_TITLE}={Uri.EscapeDataString(docResp.FullTitle)}");
                }
            }
            else if (doc is string)
            {
                this.navigationService.NavigateAsync($"{pageToNavigate}?{UrlNavHelper.IDENTIFIER}={doc}");
            }
        }

        /// <summary>
        /// Observable collection for all legislation documents
        /// </summary>
        public ObservableCollection<LegislationGroupItem> LegislationDocs
        {
            get => legislationDocs;
            set
            {
                legislationDocs = value;
                this.RaisePropertyChanged(nameof(this.LegislationDocs));
            }
        }

        /// <summary>
        /// Property for activation/deactivation of loading indicator for list of all bookmarks
        /// </summary>
        public bool IsLoading
        {
            get => isLoading;
            set
            {
                isLoading = value;
                this.RaisePropertyChanged(nameof(this.IsLoading));
            }
        }

        protected abstract string UrlAction { get; }

        /// <summary>
        /// Method for filling LegislationDocs collection
        /// </summary>
        public async Task GetAllLegislationDocs()
        {
            IsLoading = true;
            try
            {
                if(!(await ConnectivityHelper.CheckInternetConection(this.dialogService)))
                {
                    return;
                }

                SearchApiModel searchModel = new SearchApiModel();

                int currentLanguageId = (int)this.settings.Language;
                //searchModel.LangPreferences = new int[] { currentLanguageId, currentLanguageId != 4 ? 4 : 1, currentLanguageId == 5 ? 1 : 5 };
                switch (currentLanguageId)
                {
                    case 1:
                        searchModel.LangPreferences = new int[] { 1, 4, 5, 2, 3 };
                        break;
                    case 2:
                        searchModel.LangPreferences = new int[] { 2, 4, 1, 5, 3 };
                        break;
                    case 3:
                        searchModel.LangPreferences = new int[] { 3, 4, 1, 5, 2 };
                        break;
                    case 4:
                        searchModel.LangPreferences = new int[] { 4, 1, 5, 2, 3 };
                        break;
                    case 5:
                        searchModel.LangPreferences = new int[] { 5, 4, 1, 2, 3 };
                        break;
                    default:
                        searchModel.LangPreferences = new int[] { 4, 1, 5, 2, 3 };
                        break;
                }

                var legList = await this.httpService.GetLegislation(searchModel, this.UrlAction).ConfigureAwait(false);

                this.synchronizationContext.Post(_ => 
                {
                    LegislationDocs = new ObservableCollection<LegislationGroupItem>();
                    legList.Categories.ForEach(x => x.Heading = Translator.GetString(x.Heading));
                    legList.Categories.ForEach(x => LegislationDocs.Add(new LegislationGroupItem(x)));
                }, null);
            }
            catch(Exception ex)
            {
                await ErrorsHelper.DisplayError(this.dialogService, ex);
            }
            finally
            {
                IsLoading = false;
            }
        }
    }
}
