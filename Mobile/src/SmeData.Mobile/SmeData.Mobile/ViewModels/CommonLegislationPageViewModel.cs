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
    public abstract class CommonLegislationPageViewModel : BaseViewModel
    {
        protected ObservableCollection<LegislationGroupItem> legislationDocs = new ObservableCollection<LegislationGroupItem>();
        protected readonly HttpService httpService;
        protected readonly IPageDialogService dialogService;
        private readonly SynchronizationContext synchronizationContext;

        private bool isLoading;
        public ICommand TabCommand { get; set; }

        public string DocInTabFont { get => $"t|16|{ScreenWidth}"; }

        public CommonLegislationPageViewModel(HttpService service, INavigationService navigationService, IPageDialogService dialogService, SettingsModel settings) : base(navigationService)
        {
            this.TabCommand = new DelegateCommand<object>(this.ShowDocument);
            this.httpService = service;
            this.dialogService = dialogService;
            this.settings = settings;
            this.synchronizationContext = SynchronizationContext.Current;
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            
            Task.Run(()=> GetAllLegislationDocs());
        }

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

        public ObservableCollection<LegislationGroupItem> LegislationDocs
        {
            get => legislationDocs;
            set
            {
                legislationDocs = value;
                this.RaisePropertyChanged(nameof(this.LegislationDocs));
            }
        }

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

        public async Task GetAllLegislationDocs()
        {
            IsLoading = true;
            try
            {
                if(! (await ConnectivityHelper.CheckInternetConection(this.dialogService)))
                {
                    return;
                }

                SearchApiModel searchModel = new SearchApiModel();

                int currentLanguageId = (int)this.settings.Language;
                searchModel.LangPreferences = new int[] { currentLanguageId, currentLanguageId != 4 ? 4 : 1, currentLanguageId == 5 ? 1 : 5 };


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
