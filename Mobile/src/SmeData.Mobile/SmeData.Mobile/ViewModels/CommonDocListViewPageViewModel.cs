using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using SmeData.Mobile.Helpers;
using SmeData.Mobile.Models.Settings;
using SmeData.Mobile.Services;
using SmeData.WebApi.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms.Extended;

namespace SmeData.Mobile.ViewModels
{
    public class CommonDocListViewPageViewModel : BaseViewModel
    {
        public InfiniteScrollCollection<DocumentResponseModel> euDocs = new InfiniteScrollCollection<DocumentResponseModel>();
        protected readonly HttpService httpService;
        protected readonly IPageDialogService dialogService;

        private bool isLoading;
        private bool isBusy;

        public ICommand TabCommand { get; set; }

        public string DocTitlesFont { get => $"t|18|{ScreenWidth}"; }

        public string DocInTabFont { get => $"t|16|{ScreenWidth}"; }

        public CommonDocListViewPageViewModel(HttpService httpService, INavigationService navigationService, IPageDialogService dialogService, SettingsModel settings) : base(navigationService)
        {
            this.TabCommand = new DelegateCommand<object>(this.ShowDocument);
            this.httpService = httpService;
            this.settings = settings;
            this.dialogService = dialogService;
        }

        public InfiniteScrollCollection<DocumentResponseModel> EuDocs
        {
            get => euDocs;
            set
            {
                euDocs = value;
                this.RaisePropertyChanged(nameof(this.EuDocs));
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

        public bool IsBusy
        {
            get => isBusy;
            set
            {
                isBusy = value;
                this.RaisePropertyChanged(nameof(this.IsBusy));
            }
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

        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            if (!(await ConnectivityHelper.CheckInternetConection(this.dialogService)))
            {
                return;
            }

            if (parameters.ContainsKey(UrlNavHelper.CLASSIFIER))
            {
                await GetEuDocsByClassifier((string)parameters[UrlNavHelper.CLASSIFIER]);
            }
        }

        protected async Task GetEuDocsByClassifier(string classifiers)
        {
            IsLoading = true;
            try
            {
                if (!(await ConnectivityHelper.CheckInternetConection(this.dialogService)))
                {
                    return;
                }

                SearchApiModel searchModel = new SearchApiModel();
                string[] classifierArray = classifiers.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
                searchModel.Classifiers = new List<string>();
                searchModel.Classifiers.AddRange(classifierArray);

                int currentLanguageId = (int)this.settings.Language;
                searchModel.LangPreferences = new int[] { currentLanguageId, currentLanguageId != 4 ? 4 : 1, currentLanguageId == 5 ? 1 : 5 };
                var searchResult = await httpService.GetClassifier(searchModel);

                foreach (var searchItem in searchResult.Data)
                {
                    searchItem.FullTitle = Regex.Replace(searchItem.FullTitle.Replace("&nbsp;", " "), @"([\r\n]+|\s{2,})", " ").Trim();
                }

                int pageSize = 10;

                EuDocs = new InfiniteScrollCollection<DocumentResponseModel>
                {
                    OnLoadMore = async () =>
                    {
                        IsBusy = true;
                        try
                        {
                            var items = await GetItemsAsync(searchResult, EuDocs.Count, pageSize);
                            return items;
                        }
                        finally
                        {
                            IsBusy = false;
                        }
                    },
                    OnCanLoadMore = () =>
                    {
                        return EuDocs.Count < searchResult.TotalCount;
                    }
                };

                EuDocs.AddRange(await GetItemsAsync(searchResult, 0, pageSize));
            }
            catch (Exception ex)
            {
                await ErrorsHelper.DisplayError(this.dialogService, ex);
            }
            finally
            {
                IsLoading = false;
            }
        }

        public async Task<InfiniteScrollCollection<DocumentResponseModel>> GetItemsAsync(SearchResultModel seacrhResult, int pageIndex, int pageSize)
        {
            await Task.Delay(1000).ConfigureAwait(false);

            return new InfiniteScrollCollection<DocumentResponseModel>(seacrhResult.Data.Skip(pageIndex).Take(pageSize));
        }
    }
}

