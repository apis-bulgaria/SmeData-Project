using Newtonsoft.Json;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using SmeData.Mobile.Data;
using SmeData.Mobile.Helpers;
using SmeData.Mobile.Models.Settings;
using SmeData.Mobile.Services;
using SmeData.Shared.Common;
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
    /// <summary>
    /// Common page for showing lists of documents
    /// </summary>
    public class CommonDocListViewPageViewModel : BaseViewModel
    {
        public InfiniteScrollCollection<DocumentResponseModel> euDocs = new InfiniteScrollCollection<DocumentResponseModel>();
        protected readonly HttpService httpService;
        protected readonly AppRepository documentsRepository;
        protected readonly IPageDialogService dialogService;

        private bool isLoading;
        private bool isBusy;

        public ICommand TabCommand { get; set; }

        public ICommand TabCommonDocCommand { get; set; }

        /// <summary>
        /// Information for font size calculation for label with doc title
        /// </summary>
        public string DocTitlesFont { get => $"t|18|{ScreenWidth}"; }

        /// <summary>
        /// Information for font size calculation for heading in tab
        /// </summary>
        public string DocInTabFont { get => $"t|16|{ScreenWidth}"; }

        public CommonDocListViewPageViewModel(HttpService httpService, INavigationService navigationService, IPageDialogService dialogService, SettingsModel settings, AppRepository documentsRepository) : base(navigationService)
        {
            this.TabCommand = new DelegateCommand<object>(this.ShowDocument);
            this.TabCommonDocCommand = new DelegateCommand<object>(this.ShowCommonDocument);
            this.httpService = httpService;
            this.settings = settings;
            this.dialogService = dialogService;
            this.documentsRepository = documentsRepository;
        }

        /// <summary>
        /// Infinite list view collection for all documents
        /// </summary>
        public InfiniteScrollCollection<DocumentResponseModel> EuDocs
        {
            get => euDocs;
            set
            {
                euDocs = value;
                this.RaisePropertyChanged(nameof(this.EuDocs));
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

        /// <summary>
        /// Property for activation/deactivation of loading indicator for part of bookmarks
        /// </summary>
        public bool IsBusy
        {
            get => isBusy;
            set
            {
                isBusy = value;
                this.RaisePropertyChanged(nameof(this.IsBusy));
            }
        }

        /// <summary>
        /// Common method for navigating to selected document detailed view
        /// </summary>
        /// <param name="doc">Selected document</param>
        private void ShowCommonDocument(object doc)
        {
            if (doc == null)
            {
                return;
            }

            string pageToNavigate = "DocCommonShowPage";

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
        /// Override OnNavigatedTo method with added logic for load documents
        /// </summary>
        /// <param name="parameters">Input parameters</param>
        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            if (parameters.ContainsKey(UrlNavHelper.CLASSIFIER))
            {
                await GetEuDocsByClassifier((string)parameters[UrlNavHelper.CLASSIFIER]);
            }
        }

        /// <summary>
        /// Method for filling EuDocs collection by classifiers
        /// </summary>
        /// <param name="classifiers">Input classifier</param>
        protected async Task GetEuDocsByClassifier(string classifiers)
        {
            IsLoading = true;
            try
            {
                SearchResultModel searchResult = null;

                try
                {
                    if (classifiers == "355BD1E1-6ADA-4689-A8BE-7BF37F75AB98")
                    {
                        //FAQs for citizens
                        switch (this.settings.Language)
                        {
                            case SharedModels.Language.SmeLanguage.Bulgarian:
                                searchResult = JsonConvert.DeserializeObject<SearchResultModel>(Compression.DecompressString((await this.documentsRepository.GetDocumentAsync("7904991f-8968-45f6-bb99-ce33ecff2eec")).JsonSmeDoc));
                                break;
                            case SharedModels.Language.SmeLanguage.English:
                                searchResult = JsonConvert.DeserializeObject<SearchResultModel>(Compression.DecompressString((await this.documentsRepository.GetDocumentAsync("ac1b54de-eb61-4bbd-a760-d061ebdece35")).JsonSmeDoc));
                                break;
                            case SharedModels.Language.SmeLanguage.Italian:
                                searchResult = JsonConvert.DeserializeObject<SearchResultModel>(Compression.DecompressString((await this.documentsRepository.GetDocumentAsync("55a2d543-d334-431b-b179-3422df93e9fd")).JsonSmeDoc));
                                break;
                            case SharedModels.Language.SmeLanguage.German:
                                searchResult = JsonConvert.DeserializeObject<SearchResultModel>(Compression.DecompressString((await this.documentsRepository.GetDocumentAsync("7b760d50-8f17-4f84-840e-b5d0d39eca04")).JsonSmeDoc));
                                break;
                            case SharedModels.Language.SmeLanguage.French:
                                searchResult = JsonConvert.DeserializeObject<SearchResultModel>(Compression.DecompressString((await this.documentsRepository.GetDocumentAsync("be95fd5d-2f56-4d12-942a-9d7438ec3819")).JsonSmeDoc));
                                break;
                            default:
                                break;
                        }
                    }
                    else if (classifiers == "0BD47035-E4CF-401D-AD46-C3D664DE0CF6")
                    {
                        //FAQs for SMEs
                        switch (this.settings.Language)
                        {
                            case SharedModels.Language.SmeLanguage.Bulgarian:
                                searchResult = JsonConvert.DeserializeObject<SearchResultModel>(Compression.DecompressString((await this.documentsRepository.GetDocumentAsync("4b360d07-2c34-4b33-82a0-8f1bf0efa9cb")).JsonSmeDoc));
                                break;
                            case SharedModels.Language.SmeLanguage.English:
                                searchResult = JsonConvert.DeserializeObject<SearchResultModel>(Compression.DecompressString((await this.documentsRepository.GetDocumentAsync("cded3048-201b-4dbb-8e25-c717126e5447")).JsonSmeDoc));
                                break;
                            case SharedModels.Language.SmeLanguage.Italian:
                                searchResult = JsonConvert.DeserializeObject<SearchResultModel>(Compression.DecompressString((await this.documentsRepository.GetDocumentAsync("98e81915-2e8d-4afc-9101-a7e7b1073095")).JsonSmeDoc));
                                break;
                            case SharedModels.Language.SmeLanguage.German:
                                searchResult = JsonConvert.DeserializeObject<SearchResultModel>(Compression.DecompressString((await this.documentsRepository.GetDocumentAsync("b580dc94-04d1-442f-b69f-fc90ff2f5fc6")).JsonSmeDoc));
                                break;
                            case SharedModels.Language.SmeLanguage.French:
                                searchResult = JsonConvert.DeserializeObject<SearchResultModel>(Compression.DecompressString((await this.documentsRepository.GetDocumentAsync("59b1b1a2-1558-4545-bf2a-5811ac997503")).JsonSmeDoc));
                                break;
                            default:
                                break;
                        }
                    }
                    else if (classifiers == "AFEE5C1D-690D-4A2E-B420-0B0FD4F081BC")
                    {
                        //GDPR Decision support tools
                        switch (this.settings.Language)
                        {
                            case SharedModels.Language.SmeLanguage.Bulgarian:
                                searchResult = JsonConvert.DeserializeObject<SearchResultModel>(Compression.DecompressString((await this.documentsRepository.GetDocumentAsync("59b5283c-a9d7-4fb7-9635-9860a9249c62")).JsonSmeDoc));
                                break;
                            case SharedModels.Language.SmeLanguage.English:
                                searchResult = JsonConvert.DeserializeObject<SearchResultModel>(Compression.DecompressString((await this.documentsRepository.GetDocumentAsync("494b90d0-3b7c-4db7-9a72-9b69a27a7df6")).JsonSmeDoc));
                                break;
                            case SharedModels.Language.SmeLanguage.Italian:
                                searchResult = JsonConvert.DeserializeObject<SearchResultModel>(Compression.DecompressString((await this.documentsRepository.GetDocumentAsync("dfd84fe9-b150-4350-b775-a2ab370a15b6")).JsonSmeDoc));
                                break;
                            case SharedModels.Language.SmeLanguage.German:
                                searchResult = JsonConvert.DeserializeObject<SearchResultModel>(Compression.DecompressString((await this.documentsRepository.GetDocumentAsync("78953b2d-59c3-46a2-a1a9-e17db9c21a10")).JsonSmeDoc));
                                break;
                            case SharedModels.Language.SmeLanguage.French:
                                searchResult = JsonConvert.DeserializeObject<SearchResultModel>(Compression.DecompressString((await this.documentsRepository.GetDocumentAsync("508b4595-2b24-4c60-90b5-35f4efc543db")).JsonSmeDoc));
                                break;
                            default:
                                break;
                        }
                    }
                }
                catch (Exception e)
                {
                }
                
                if (searchResult == null)
                {
                    if (!await ConnectivityHelper.CheckInternetConection(this.dialogService))
                    {
                        return;
                    }

                    SearchApiModel searchModel = new SearchApiModel();
                    string[] classifierArray = classifiers.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
                    searchModel.Classifiers = new List<string>();
                    searchModel.Classifiers.AddRange(classifierArray);

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

                    searchResult = await httpService.GetClassifier(searchModel);
                }

                if (!Regex.IsMatch(this.GetType().Name, @"Legislation"))
                {
                    searchResult.Data = searchResult.Data.OrderBy(x => x.PublicationDate).ToList();
                }

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

        /// <summary>
        /// Mathod for loading more elements of infinite collection
        /// </summary>
        /// <param name="allResultBookmarks">Collection with all documents</param>
        /// <param name="pageIndex">Index of loaded pages</param>
        /// <param name="pageSize">Size of one page</param>
        /// <returns></returns>
        public async Task<InfiniteScrollCollection<DocumentResponseModel>> GetItemsAsync(SearchResultModel seacrhResult, int pageIndex, int pageSize)
        {
            await Task.Delay(1000).ConfigureAwait(false);
            return new InfiniteScrollCollection<DocumentResponseModel>(seacrhResult.Data.Skip(pageIndex).Take(pageSize));
        }
    }
}

