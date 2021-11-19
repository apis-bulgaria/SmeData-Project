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

        public ICommand DecreaseFontCommand { get; set; }
        public ICommand IncreaseFontCommand { get; set; }
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

        protected int docTitleFontDefault = 18;
        protected int docTitleFont = 18;
        public int DocTitleFont 
        {
            get => this.docTitleFont;
            set
            {
                this.docTitleFont = value;
                this.RaisePropertyChanged(nameof(DocTitleFont));
                this.RaisePropertyChanged(nameof(DocTitleLabelFont)); ;
            }
        }

        public Action MouseOverAction { set; get; }
        public void MouseOver(Action action)
        {
            MouseOverAction = action;
        }

        public string DocTitleLabelFont { get => $"t|{DocTitleFont + settings.FontIndex}|{ScreenWidth}"; }
        
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
            this.DecreaseFontCommand = new DelegateCommand(this.DecreaseFont);
            this.IncreaseFontCommand = new DelegateCommand(this.IncreaseFont);

            MouseOver(() =>
            {
                // do some sth
                Console.WriteLine();
            });
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
        public override async void OnNavigatedFrom(INavigationParameters parameters)
        {
            base.OnNavigatedFrom(parameters);
            await this.documentsRepository.SetSettingsAsync(new SettingsDbModel() { Id = 1, SettingsJson = JsonConvert.SerializeObject(this.settings) });
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
                        case SharedModels.Language.SmeLanguage.German:
                            documentFromRepo = await documentsRepository.GetDocumentAsync("1f3496a3-021b-4389-aa5c-a82eb578b2f1");
                            break;
                        case SharedModels.Language.SmeLanguage.French:
                            documentFromRepo = await documentsRepository.GetDocumentAsync("155322d0-05a4-422e-a3b0-6ae3fc6229f5");
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
                        case SharedModels.Language.SmeLanguage.German:
                            documentFromRepo = await documentsRepository.GetDocumentAsync("62ea5a66-c84b-4239-a9ae-c5bbbd195a68");
                            break;
                        case SharedModels.Language.SmeLanguage.French:
                            documentFromRepo = await documentsRepository.GetDocumentAsync("6cb368dc-1326-44ae-a4a0-74e3d17fb3af");
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
                        case SharedModels.Language.SmeLanguage.German:
                            documentFromRepo = await documentsRepository.GetDocumentAsync("66dbe478-c1a1-464f-8122-515ecb9fb8da");
                            break;
                        case SharedModels.Language.SmeLanguage.French:
                            documentFromRepo = await documentsRepository.GetDocumentAsync("ae6c6f54-d93b-4703-af43-3973a9e5fc61");
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
                        case SharedModels.Language.SmeLanguage.German:
                            documentFromRepo = await documentsRepository.GetDocumentAsync("26f59b3d-5b80-4cd1-9e3b-3d8919502438");
                            break;
                        case SharedModels.Language.SmeLanguage.French:
                            documentFromRepo = await documentsRepository.GetDocumentAsync("43642436-72dc-4b64-9562-ed2b96059fdb");
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
            this.HtmlText = $@"<html><head>{SetFontSizeInHead(head)}</head><body>{res}</body></html>";
        }

        protected string SetFontSizeInHead(string heading)
        {
            if (string.IsNullOrWhiteSpace(heading))
            {
                return string.Empty;
            }

            string resultHeading = heading;

            resultHeading = Regex.Replace(resultHeading, @"(?<=\s)(button\s+\{)", $@"$1{Environment.NewLine}    font-size: 14px;{Environment.NewLine}", RegexOptions.IgnoreCase);
            resultHeading = Regex.Replace(resultHeading, @"(?<=font\-size\:\s{0,3})(\d+)(?=px\;)", m => { return Math.Max(int.Parse(m.Value) + this.settings.FontIndex, 4).ToString(); }, RegexOptions.IgnoreCase);

            return resultHeading;
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
                if (smeDoc == null && settings.ShowDocsOnWifiOnly && !await ConnectivityHelper.CheckForWifiConnection(this.dialogService))
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

        protected virtual void DecreaseFont()
        {
            if (this.settings.FontIndex > -10)
            {
                this.settings.FontIndex--;
                DocTitleFont = docTitleFontDefault + this.settings.FontIndex;
                this.PrepareHtml(this.CurrentDocument?.Items[0]?.Text, this.CurrentDocument.Head);
            }
        }

        protected virtual void IncreaseFont()
        {
            if (this.settings.FontIndex < 10)
            {
                this.settings.FontIndex++;
                DocTitleFont = docTitleFontDefault + this.settings.FontIndex;
                this.PrepareHtml(this.CurrentDocument?.Items[0]?.Text, this.CurrentDocument.Head);
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
                    if (settings.ShowDocsOnWifiOnly && !await ConnectivityHelper.CheckForWifiConnection(this.dialogService))
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

                List<string> systemDocsIdents = new List<string>() {
                "a5a8acc2-9184-4946-9698-0b7a8c2120de",
                "52654854-513a-4516-8ee9-998fe73cc42e",
                "0112b2e6-5792-4cb5-af11-ff7765a5df9c",
                "7941c7fa-4459-48e0-9b4d-34328b64fcc0",
                "91d0bdc5-79c7-4180-97f4-ef7d4437ddb2",
                "2921c10b-60f2-4391-a808-fd651129d692",
                "7642ae7b-a706-4901-8399-71085ac2adab",
                "43b8c2ce-fd96-40d8-bebc-303dd9098911",
                "a1fed601-9ebe-43b3-bae7-26302c102516",
                "53e074a5-0b26-4fd3-bbab-13f7a418af4f",
                "2e2fd43c-3dec-45ec-b127-9fbe719fa4c2",
                "f6c17707-0bf6-42ae-bab5-5ab3a48771c2",
                "44f9398a-729d-4501-9845-6e56aecffb4b",
                "9df940ee-6e77-4181-8362-43788e625a2b",
                "9859dd64-59d1-4491-a89d-1dbebcd4e04b"
                };

                switch (updateStatus)
                {
                    case 1:
                        await this.dialogService.DisplayAlertAsync(Translator.GetString("Message"), Translator.GetString("DocumentIsSaved"), Translator.GetString("Ok"));
                        break;
                    case 2:
                        if (systemDocsIdents.Contains(document.Identifier))
                        {
                            if (await this.dialogService.DisplayAlertAsync(Translator.GetString("Message"), Translator.GetString("DocumentIsAlreadySavedSystemDoc"), Translator.GetString("Yes"), Translator.GetString("No")))
                            {
                                await documentsRepository.UpdateDocumentAsync(document);
                                await this.dialogService.DisplayAlertAsync(Translator.GetString("Message"), Translator.GetString("DocumentIsUpdated"), Translator.GetString("Ok"));
                            }
                        }
                        else
                        {
                            if (await this.dialogService.DisplayAlertAsync(Translator.GetString("Message"), Translator.GetString("DocumentIsAlreadySaved"), Translator.GetString("Yes"), Translator.GetString("No")))
                            {
                                await documentsRepository.UpdateDocumentAsync(document);
                                await this.dialogService.DisplayAlertAsync(Translator.GetString("Message"), Translator.GetString("DocumentIsUpdated"), Translator.GetString("Ok"));
                            }
                        }                       
                        break;
                    default:
                        break;
                }
            }
        }
    }
}