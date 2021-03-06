using Newtonsoft.Json;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using SmeData.Mobile.Data;
using SmeData.Mobile.Data.Models;
using SmeData.Mobile.Helpers;
using SmeData.Mobile.Models.Documents;
using SmeData.Mobile.Models.Settings;
using SmeData.Mobile.Services;
using SmeData.Shared.Common;
using SmeData.SharedModels.Document;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace SmeData.Mobile.ViewModels
{
    /// <summary>
    /// Base common page for showing documents and information
    /// </summary>
    public abstract class BaseShowPageViewModel : BaseViewModel
    {
        protected readonly IPageDialogService dialogService;
        protected readonly DocumentService docService;
        protected readonly HttpService httpService;
        protected readonly AppRepository documentsRepository;
        protected string currentToPar = string.Empty;

        public ICommand SearchInDocCommand { get; set; }
        public ICommand ToogleIsSearchBarVisibleCommand { get; set; }
        public ICommand NavigatingCommand { get; set; }
        public ICommand ExpandDocTitleLabelCommand { get; set; }
        public ICommand SaveOfflineCommand { get; set; }
        public List<ColorizeInfo> colorizeInfos = new List<ColorizeInfo>();
        public int currentColorizeIndex = -1;
        public ICommand NextMatchCommand { get; set; }
        public ICommand PrevMatchCommand { get; set; }

        /// <summary>
        /// Marker for showing/hiding search bar
        /// </summary>
        protected bool isTbSearchVisible = true;
        public bool IsTbSearchVisible
        {
            get => this.isTbSearchVisible;
            set
            {
                this.isTbSearchVisible = value;
                this.RaisePropertyChanged(nameof(IsTbSearchVisible));
            }
        }

        /// <summary>
        /// Information for font size calculation for label with doc title
        /// </summary>
        public string DocTitleLabelFont { get => $"t|18|{ScreenWidth}"; }

        /// <summary>
        /// Text in search field
        /// </summary>
        private string textInField = string.Empty;
        public string TextInField
        {
            get => this.textInField;
            set
            {
                this.textInField = value;
                this.RaisePropertyChanged(nameof(TextInField));
            }
        }

        /// <summary>
        /// Text in search field
        /// </summary>
        private string searchCounter = string.Empty;
        public string SearchCounter
        {
            get => this.searchCounter;
            set
            {
                this.searchCounter = value;
                this.RaisePropertyChanged(nameof(SearchCounter));
            }
        }

        /// <summary>
        /// Current selected doc item
        /// </summary>
        private SmeDocItem selectedItem;
        public SmeDocItem SelectedItem
        {
            get { return this.selectedItem; }
            set
            {
                this.selectedItem = value;
                this.RaisePropertyChanged(nameof(SelectedItem));
            }
        }


        public BaseShowPageViewModel(INavigationService navigationService, IPageDialogService dialogService, DocumentService docService, SettingsModel settings, HttpService httpService, AppRepository documentsRepository) : base(navigationService)
        {
            this.dialogService = dialogService;
            this.docService = docService;
            this.settings = settings;
            this.httpService = httpService;
            this.documentsRepository = documentsRepository;

            this.NavigatingCommand = new DelegateCommand<string>(this.HandleNavigation);
            this.ToogleIsSearchBarVisibleCommand = new DelegateCommand(this.ToogleIsSearchBarVisible);
            this.SearchInDocCommand = new DelegateCommand<string>(async (txt) => await this.SearchInDoc(txt));
            this.ExpandDocTitleLabelCommand = new DelegateCommand(this.ExpandDocTitleLabel);
            this.SaveOfflineCommand = new DelegateCommand(this.DoSaveOffline);
            this.IsTbSearchVisible = true;
            this.NextMatchCommand = new DelegateCommand(this.GotoNextMatch);
            this.PrevMatchCommand = new DelegateCommand(this.GotoPrevMatch);
        }

        /// <summary>
        /// Method for navigating to next match in search mode
        /// </summary>
        protected void GotoNextMatch()
        {
            if (this.colorizeInfos.Count == 0)
            {
                return;
            }

            this.currentColorizeIndex++;

            if (this.currentColorizeIndex == this.colorizeInfos.Count)
            {
                this.currentColorizeIndex = 0;
            }

            //this.currentColorizeIndex = Math.Min(this.currentColorizeIndex, this.colorizeInfos.Count - 1);

            this.SearchCounter = $"{currentColorizeIndex + 1}{Environment.NewLine}({this.colorizeInfos.Count})";

            this.DoGotoNextMatch();
        }

        /// <summary>
        /// Method for navigating to next match in search mode
        /// </summary>
        protected virtual void DoGotoNextMatch()
        {
            this.PrepareHtml(this.currentDocument?.Items[0].Text, this.currentDocument.Head);
        }

        /// <summary>
        /// Method for navigating to previous match in search mode
        /// </summary>
        protected void GotoPrevMatch()
        {
            if (this.colorizeInfos.Count == 0)
            {
                return;
            }

            this.currentColorizeIndex--;

            if (this.currentColorizeIndex == -1)
            {
                this.currentColorizeIndex = this.colorizeInfos.Count - 1;
            }

            //this.currentColorizeIndex = Math.Max(this.currentColorizeIndex, 0);
            this.SearchCounter = $"{currentColorizeIndex + 1}{Environment.NewLine}({this.colorizeInfos.Count})";

            this.DoGotoPrevMatch();
        }

        /// <summary>
        /// Method for navigating to previous match in search mode
        /// </summary>
        protected virtual void DoGotoPrevMatch()
        {
            this.PrepareHtml(this.currentDocument?.Items[0].Text, this.currentDocument.Head);
        }

        /// <summary>
        /// Navigate to inputed url
        /// </summary>
        /// <param name="url">Input url</param>
        protected virtual void HandleNavigation(string url)
        {
            UrlNavHelper.ProcessNavigation(url, this.navigationService, this.httpService);
        }

        /// <summary>
        /// Show/Hide search bar
        /// </summary>
        protected void ToogleIsSearchBarVisible()
        {
            IsSearchVisible = !IsSearchVisible;
        }

        /// <summary>
        /// Shows document with marked searched text
        /// </summary>
        /// <param name="searchText">Input searched text</param>
        /// <returns></returns>
        protected async Task SearchInDoc(string searchText)
        {
            this.IsLoading = true;
            try
            {
                if (!(await ConnectivityHelper.CheckInternetConection(this.dialogService)))
                {
                    return;
                }

                if (!string.IsNullOrWhiteSpace(CurrentDocument?.Meta?.Idenitifier))
                {
                    await this.LoadDocumentByIdentifier(CurrentDocument.Meta.Idenitifier, currentToPar != null ? Regex.Replace(currentToPar, @"([^_])_([^_])", @"$1$2") : string.Empty, searchText, null);
                }
                else if (!string.IsNullOrWhiteSpace(CurrentDocument?.Meta?.DocNumber))
                {
                    await this.LoadDocumentByDocNumber(CurrentDocument.Meta.DocNumber, currentToPar != null ? Regex.Replace(currentToPar, @"([^_])_([^_])", @"$1$2") : string.Empty, searchText, null);
                }
            }
            finally
            {
                this.IsLoading = false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private LineBreakMode docTitleLabelLineBreakMode = LineBreakMode.TailTruncation;
        public LineBreakMode DocTitleLabelLineBreakMode
        {
            get => this.docTitleLabelLineBreakMode;
            set
            {
                this.docTitleLabelLineBreakMode = value;
                this.RaisePropertyChanged(nameof(this.DocTitleLabelLineBreakMode));
            }
        }

        protected string currentDocShortTitle = string.Empty;

        private string currentDocTitle = string.Empty;
        public string CurrentDocTitle
        {
            get => this.currentDocTitle;
            set
            {
                this.currentDocTitle = value;
                this.RaisePropertyChanged(nameof(CurrentDocTitle));
            }
        }

        private string htmlText;
        public string HtmlText
        {
            get => htmlText;
            set
            {
                this.htmlText = value;
                this.RaisePropertyChanged(nameof(HtmlText));
            }
        }

        private bool isLoading;
        public bool IsLoading
        {
            get => this.isLoading;

            set
            {
                this.isLoading = value;
                this.RaisePropertyChanged(nameof(this.IsLoading));
            }
        }

        private bool isSearchVisible;
        public bool IsSearchVisible
        {
            get => this.isSearchVisible;

            set
            {
                this.isSearchVisible = value;
                this.RaisePropertyChanged(nameof(this.IsSearchVisible));
            }
        }

        private SmeDoc currentDocument;
        public SmeDoc CurrentDocument
        {
            get => currentDocument;
            set
            {
                currentDocument = value;
                this.RaisePropertyChanged(nameof(this.CurrentDocument));
            }
        }

        private int docTitleLabelMaxLines = 1;
        public int DocTitleLabelMaxLines
        {
            get => docTitleLabelMaxLines;
            set
            {
                docTitleLabelMaxLines = value;
                this.RaisePropertyChanged(nameof(this.DocTitleLabelMaxLines));
            }
        }

        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            var searchText = string.Empty;
            if (parameters.ContainsKey(UrlNavHelper.SEARCH_TEXT))
            {
                searchText = (string)parameters[UrlNavHelper.SEARCH_TEXT];
            }
            if (parameters.ContainsKey(UrlNavHelper.FULL_TITLE))
            {
                CurrentDocTitle = (string)parameters[Uri.UnescapeDataString(UrlNavHelper.FULL_TITLE)];
            }

            if (parameters.ContainsKey(UrlNavHelper.IDENTIFIER) && (string.IsNullOrEmpty(searchText)))
            {
                var ident = (string)parameters[UrlNavHelper.IDENTIFIER];

                var documentFromRepo = await documentsRepository.GetDocumentAsync(ident);

                if (documentFromRepo != null)
                {
                    var toPar = (string)parameters[UrlNavHelper.TO_PAR];
                    currentToPar = toPar;

                    await this.LoadDocumentByIdentifier(ident, toPar, searchText, JsonConvert.DeserializeObject<SmeDoc>(Compression.DecompressString(documentFromRepo.JsonSmeDoc)));
                    return;
                }
            }
            else if (parameters.ContainsKey(UrlNavHelper.DOC_NUM))
            {
                var docNum = (string)parameters[UrlNavHelper.DOC_NUM];
                DocumentModel documentFromRepo = null;

                if (docNum == "64397F33F39989D4F7BB5668A9540570")
                {
                    switch (settings.Language)
                    {
                        case SharedModels.Language.SmeLanguage.Bulgarian:
                            documentFromRepo = await documentsRepository.GetDocumentAsync("bde079be-b512-43bc-9855-ef92379cd29f");
                            break;
                        case SharedModels.Language.SmeLanguage.English:
                            documentFromRepo = await documentsRepository.GetDocumentAsync("dc185fc4-bfbb-44d7-9b28-fafd656473b2");
                            break;
                        case SharedModels.Language.SmeLanguage.Italian:
                            documentFromRepo = await documentsRepository.GetDocumentAsync("acb046a4-dd0f-4a2c-8356-474beea4083f");
                            break;
                        default:
                            break;
                    }
                }
                else if (docNum == "712BDAC3C2CB0BCA51255B24FC39AD08")
                {
                    switch (settings.Language)
                    {
                        case SharedModels.Language.SmeLanguage.Bulgarian:
                            documentFromRepo = await documentsRepository.GetDocumentAsync("1eb04526-5dd1-4710-a459-f8c9b9a82428");
                            break;
                        case SharedModels.Language.SmeLanguage.English:
                            documentFromRepo = await documentsRepository.GetDocumentAsync("c7a4e31e-fc6c-440a-837a-5466023661d4");
                            break;
                        case SharedModels.Language.SmeLanguage.Italian:
                            documentFromRepo = await documentsRepository.GetDocumentAsync("3a064539-ca4f-4a40-9fd8-841bc050fac2");
                            break;
                        default:
                            break;
                    }
                }
                else if (docNum == "about")
                {
                    switch (settings.Language)
                    {
                        case SharedModels.Language.SmeLanguage.Bulgarian:
                            documentFromRepo = await documentsRepository.GetDocumentAsync("ae90b5fd-0247-4aea-af11-d21e0f834c08");
                            break;
                        case SharedModels.Language.SmeLanguage.English:
                            documentFromRepo = await documentsRepository.GetDocumentAsync("a00d369e-fb97-4856-8b26-126e79e7e00b");
                            break;
                        case SharedModels.Language.SmeLanguage.Italian:
                            documentFromRepo = await documentsRepository.GetDocumentAsync("becf2bac-1d3b-4d5a-8644-92ec5d667c7a");
                            break;
                        default:
                            break;
                    }

                }
                else if (docNum == "help")
                {

                    switch (settings.Language)
                    {
                        case SharedModels.Language.SmeLanguage.Bulgarian:
                            documentFromRepo = await documentsRepository.GetDocumentAsync("a946f1e0-467f-4a1f-ba9b-38247b3467d3");
                            break;
                        case SharedModels.Language.SmeLanguage.English:
                            documentFromRepo = await documentsRepository.GetDocumentAsync("d4f1c296-83f4-4e1c-a88f-af3d28757cf2");
                            break;
                        case SharedModels.Language.SmeLanguage.Italian:
                            documentFromRepo = await documentsRepository.GetDocumentAsync("1b2c3b45-aacd-4c25-9355-5dad0bd51e4f");
                            break;
                        default:
                            break;
                    }
                }

                if (documentFromRepo != null)
                {
                    await this.LoadDocumentByDocNumber(docNum, null, string.Empty, JsonConvert.DeserializeObject<SmeDoc>(Compression.DecompressString(documentFromRepo.JsonSmeDoc)));
                    return;
                }
            }

            if (!parameters.ContainsKey(UrlNavHelper.IS_OFFLINE) || (parameters.ContainsKey(UrlNavHelper.IS_OFFLINE) && (string)parameters[UrlNavHelper.IS_OFFLINE] == "false"))
            {
                if (!await ConnectivityHelper.CheckInternetConection(this.dialogService))
                {
                    return;
                }
            }

            

            if (parameters.ContainsKey(UrlNavHelper.IDENTIFIER))
            {
                var identifier = (string)parameters[UrlNavHelper.IDENTIFIER];
                var toPar = (string)parameters[UrlNavHelper.TO_PAR];
                currentToPar = toPar;
                await this.LoadDocumentByIdentifier(identifier, toPar, searchText, null);
                return;
            }

            if (parameters.ContainsKey(UrlNavHelper.DOC_NUM))
            {
                var docNum = (string)parameters[UrlNavHelper.DOC_NUM];
                var toPar = (string)parameters[UrlNavHelper.TO_PAR];
                currentToPar = toPar;
                await this.LoadDocumentByDocNumber(docNum, toPar, searchText, null);
            }
        }

        private void ExpandDocTitleLabel()
        {
            if (DocTitleLabelLineBreakMode == LineBreakMode.TailTruncation)
            {
                DocTitleLabelLineBreakMode = LineBreakMode.WordWrap;
                DocTitleLabelMaxLines = 30;
            }
            else
            {
                DocTitleLabelLineBreakMode = LineBreakMode.TailTruncation;
                DocTitleLabelMaxLines = 1;
            }
        }

        protected void PrepareHtml(string res, string head)
        {
            res = res.Replace("target=\"_blank\"", " ");
            res += this.GetColorizeScript();
            this.HtmlText = $@"<html><head>{head}</head><body>{res}</body></html>";
        }

        protected string GetColorizeScript()
        {
            if (this.colorizeInfos?.Count > 0)
            {
                return $@"<script>
                    var x = document.getElementById(""{this.colorizeInfos[this.currentColorizeIndex].MatchedId}"");
                    if (x){{
                        x.scrollIntoView();
                        x.className = 'srch_f1';
                    }}
                </script>";
            }
            else
            {
                return string.Empty;
            }
        }

        protected virtual async Task LoadDocumentByDocNumber(string docNumber, string toPar, string searchText, SmeDoc smeDoc)
        {
            this.IsLoading = true;
            try
            {
                if (smeDoc == null && settings.ShowDocsOnWifiOnly && !(await ConnectivityHelper.CheckForWifiConnection(this.dialogService)))
                {
                    return;
                }

                this.currentColorizeIndex = 0;
                this.colorizeInfos = new List<ColorizeInfo>();

                SmeDoc orgDoc = null;

                if (smeDoc == null)
                {
                    orgDoc = await this.docService.GetSmeDocByDocNumber(docNumber, searchText, (int)this.settings.Language);
                }
                else
                {
                    orgDoc = smeDoc;
                }

                if (orgDoc == null)
                {
                    await this.navigationService.GoBackAsync();
                    var celex = Apis.Common.Celex.Celex.TryParse(docNumber);
                    if (celex != null)
                    {
                        await Browser.OpenAsync($"https://eur-lex.europa.eu/legal-content/{UrlNavHelper.GetEurlexCountryByLanguage(settings.Language)}/ALL/?uri=CELEX:{docNumber}");
                    }
                    return;
                }

                this.CurrentDocument = orgDoc;
                if (string.IsNullOrWhiteSpace(this.CurrentDocTitle) && this.CurrentDocument != null)
                {
                    this.CurrentDocTitle = this.CurrentDocument.Meta.Title;
                }

                var res = CurrentDocument?.Items[0]?.Text;

                this.InitColorize(searchText);
                this.PrepareHtml(res, this.currentDocument.Head);
            }
            catch (Exception ex)
            {
                await ErrorsHelper.DisplayError(this.dialogService, ex);
            }
            finally
            {
                this.IsLoading = false;
            }
        }

        private void InitColorize(string searchText)
        {
            if (!string.IsNullOrEmpty(searchText))
            {
                this.colorizeInfos = DocumentHelper.GetColorizeInfo(this.CurrentDocument);
                this.currentColorizeIndex = 0;
                this.IsSearchVisible = true;
                this.TextInField = searchText;

                if (colorizeInfos.Count > 0)
                {
                    this.SearchCounter = $"{currentColorizeIndex + 1}{Environment.NewLine}({this.colorizeInfos.Count})";
                }
                else
                {
                    this.SearchCounter = $"0{Environment.NewLine}(0)";
                }
            }
        }

        protected async Task ProcessCurrentDocument(string identifier, string searchText)
        {
            this.currentColorizeIndex = 0;
            this.colorizeInfos = new List<ColorizeInfo>();
            if (CurrentDocument?.Meta.IsBlob == true)
            {
                var fileUrl = httpService.GetStaticFileContentUrl(identifier, this.currentDocument?.Items[0]?.Text);
                this.navigationService.GoBackAsync();
                await Browser.OpenAsync(fileUrl, BrowserLaunchMode.SystemPreferred);
            }
            else
            {
                this.InitColorize(searchText);
                var res = CurrentDocument?.Items[0]?.Text;
                this.PrepareHtml(res, this.currentDocument.Head);
            }

            this.IsTbSearchVisible = !string.IsNullOrEmpty(CurrentDocument?.Head);
        }

        protected virtual async Task LoadDocumentByIdentifier(string identifier, string toPar, string searchText, SmeDoc docFromRepo)
        {
            this.IsLoading = true;

            try
            {
                if (docFromRepo != null)
                {
                    this.CurrentDocument = docFromRepo;
                }
                else
                {
                    if (settings.ShowDocsOnWifiOnly && !(await ConnectivityHelper.CheckForWifiConnection(this.dialogService)))
                    {
                        return;
                    }

                    this.CurrentDocument = await this.docService.GetSmeDocByIdentifier(identifier, searchText, (int)this.settings.Language);
                }

                await this.ProcessCurrentDocument(identifier, searchText);
            }
            catch (Exception ex)
            {
                await ErrorsHelper.DisplayError(this.dialogService, ex);
            }
            finally
            {
                this.IsLoading = false;
            }
        }

        protected async void DoSaveOffline()
        {
            if (!string.IsNullOrWhiteSpace(CurrentDocument?.Meta?.Idenitifier))
            {
                var document = new DocumentModel
                {
                    Identifier = CurrentDocument.Meta.Idenitifier,
                    Title = CurrentDocTitle,
                    IsMainDoc = false,
                    IsToHide = false,
                    LastChangeDate = CurrentDocument.Meta.LastChangeDate,
                    JsonSmeDoc = Compression.CompressString(JsonConvert.SerializeObject(this.CurrentDocument)),
                };

                int updateStatus = await documentsRepository.AddDocumentAsync(document);

                switch (updateStatus)
                {
                    case 1:
                        await this.dialogService.DisplayAlertAsync(Translator.GetString("Message"), Translator.GetString("DocumentIsSaved"), Translator.GetString("Ok"));
                        break;
                    case 2:
                        if (await this.dialogService.DisplayAlertAsync(Translator.GetString("Message"), Translator.GetString("DocumentIsAlreadySaved"), Translator.GetString("Yes"), Translator.GetString("No")))
                        {
                            await documentsRepository.UpdateDocumentAsync(document);
                            await this.dialogService.DisplayAlertAsync(Translator.GetString("Message"), Translator.GetString("DocumentIsUpdated"), Translator.GetString("Ok"));
                        }
                        break;
                    default:
                        break;
                }
            }
        }
    }
}