using Prism.Commands;
using Prism.Mvvm;
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
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms.Extended;

namespace SmeData.Mobile.ViewModels
{
    /// <summary>
    /// Page for searching in all documents and navigating to them
    /// </summary>
    public class SearchPageViewModel : BaseViewModel
    {
        private readonly HttpService httpService;
        private readonly IPageDialogService dialogService;
        private bool isLoading;
        private bool isBusy;
        public InfiniteScrollCollection<DocumentResponseModel> euDocs = new InfiniteScrollCollection<DocumentResponseModel>();

        private string SearchedText { get; set; }
        public ICommand TabCommand { get; set; }
        public ICommand SearchCommand { get; set; }

        public string SubTitleFont { get => $"t|18|{ScreenWidth}"; }

        public string DocInTabFont { get => $"t|16|{ScreenWidth}"; }

        public SearchPageViewModel(HttpService service, INavigationService navigationService, IPageDialogService dialogService, SettingsModel settings) : base(navigationService)
        {
            this.SearchCommand = new DelegateCommand<string>(async (text) => await this.ShowSearchedDocuments(text));
            this.TabCommand = new DelegateCommand<object>(this.ShowDocument);
            this.httpService = service;
            this.dialogService = dialogService;
            this.settings = settings;
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
                    this.navigationService.NavigateAsync($"{pageToNavigate}?{UrlNavHelper.IDENTIFIER}={docResp.DocIdentifier}&{UrlNavHelper.FULL_TITLE}={Uri.EscapeDataString(docResp.FullTitle)}&{UrlNavHelper.SEARCH_TEXT}={SearchedText}");
                }
                else
                {
                    this.navigationService.NavigateAsync($"{pageToNavigate}?{UrlNavHelper.DOC_NUM}={docResp.DocNumber}&{UrlNavHelper.FULL_TITLE}={Uri.EscapeDataString(docResp.FullTitle)}&{UrlNavHelper.SEARCH_TEXT}={SearchedText}");
                }
            }
            else if (doc is string)
            {
                this.navigationService.NavigateAsync($"{pageToNavigate}?{UrlNavHelper.IDENTIFIER}={doc}&{UrlNavHelper.SEARCH_TEXT}={SearchedText}");
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

        public bool IsLoading
        {
            get => isLoading;
            set
            {
                isLoading = value;
                this.RaisePropertyChanged(nameof(this.IsLoading));
            }
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

        private async Task ShowSearchedDocuments(string searchedText)
        {
            if (string.IsNullOrWhiteSpace(searchedText))
            {
                return;
            }

            this.SearchedText = WebUtility.UrlEncode(searchedText.Trim());

            IsLoading = true;
            try
            {
                if (!(await ConnectivityHelper.CheckInternetConection(this.dialogService)))
                {
                    return;
                }
                SearchApiModel searchModel = new SearchApiModel()
                {
                    SearchText = searchedText
                };

                int pageSize = 10;

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

                var seacrhResult = httpService.GetClassifier(searchModel).Result;

                if (seacrhResult.Data.Count > 0)
                {
                    EuDocs = new InfiniteScrollCollection<DocumentResponseModel>
                    {
                        OnLoadMore = async () =>
                        {
                            IsBusy = true;

                            var items = await GetItemsAsync(seacrhResult, EuDocs.Count, pageSize);

                            IsBusy = false;
                            return items;
                        },
                        OnCanLoadMore = () =>
                        {
                            return EuDocs.Count < seacrhResult.TotalCount;
                        }
                    };

                    EuDocs.AddRange(GetItemsAsync(seacrhResult, 0, pageSize).Result);
                }
                else
                {
                    await this.dialogService.DisplayAlertAsync(Translator.GetString("Message"), $@"{Translator.GetString("NoResult")} ""{searchedText}""", Translator.GetString("Ok"));
                }
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
