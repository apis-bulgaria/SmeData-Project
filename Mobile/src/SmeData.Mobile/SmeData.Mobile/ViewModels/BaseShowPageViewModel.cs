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
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace SmeData.Mobile.ViewModels
{
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
        

        public string DocTitleLabelFont { get => $"t|18|{ScreenWidth}"; }

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
        }

        protected virtual void HandleNavigation(string url)
        {
            UrlNavHelper.ProcessNavigation(url, this.navigationService, this.httpService);
        }

        protected void ToogleIsSearchBarVisible()
        {
            IsSearchVisible = !IsSearchVisible;
        }

        protected async Task SearchInDoc(string searchText)
        {
            if (!(await ConnectivityHelper.CheckInternetConection(this.dialogService)))
            {
                return;
            }


            this.IsLoading = true;
            try
            {
                if (!string.IsNullOrWhiteSpace(CurrentDocument?.Meta?.Idenitifier))
                {
                    this.LoadDocumentByIdentifier(CurrentDocument.Meta.Idenitifier, currentToPar != null ? Regex.Replace(currentToPar, @"([^_])_([^_])", @"$1$2") : string.Empty, searchText);
                }
                else if (!string.IsNullOrWhiteSpace(CurrentDocument?.Meta?.DocNumber))
                {
                    this.LoadDocumentByDocNumber(CurrentDocument.Meta.DocNumber, currentToPar != null ? Regex.Replace(currentToPar, @"([^_])_([^_])", @"$1$2") : string.Empty, searchText);
                }
            }
            finally
            {
                this.IsLoading = false;
            }
        }

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

        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);


            if (!parameters.ContainsKey(UrlNavHelper.IS_OFFLINE) || (parameters.ContainsKey(UrlNavHelper.IS_OFFLINE) && (string)parameters[UrlNavHelper.IS_OFFLINE] == "false"))
            {
                if (!await ConnectivityHelper.CheckInternetConection(this.dialogService))
                {
                    return;
                }
            }

            if (parameters.ContainsKey(UrlNavHelper.FULL_TITLE))
            {
                CurrentDocTitle = (string)parameters[Uri.UnescapeDataString(UrlNavHelper.FULL_TITLE)];
            }

            var searchText = string.Empty;
            if (parameters.ContainsKey(UrlNavHelper.SEARCH_TEXT))
            {
                searchText = (string)parameters[UrlNavHelper.SEARCH_TEXT];
            }

            if (parameters.ContainsKey(UrlNavHelper.IDENTIFIER))
            {
                var identifier = (string)parameters[UrlNavHelper.IDENTIFIER];
                var toPar = (string)parameters[UrlNavHelper.TO_PAR];
                currentToPar = toPar;
                await this.LoadDocumentByIdentifier(identifier, toPar, searchText);
                return;
            }

            if (parameters.ContainsKey(UrlNavHelper.DOC_NUM))
            {
                var docNum = (string)parameters[UrlNavHelper.DOC_NUM];
                var toPar = (string)parameters[UrlNavHelper.TO_PAR];
                currentToPar = toPar;
                await this.LoadDocumentByDocNumber(docNum, toPar, searchText);
            }
        }

        private void ExpandDocTitleLabel()
        {
            if (DocTitleLabelLineBreakMode == LineBreakMode.TailTruncation)
            {
                DocTitleLabelLineBreakMode = LineBreakMode.WordWrap;
            }
            else
            {
                DocTitleLabelLineBreakMode = LineBreakMode.TailTruncation;
            }
        }

        protected void PrepareHtml(string res, string head)
        {
            res = res.Replace("target=\"_blank\"", " ");
            this.HtmlText = $@"<html><head>{head}</head><body>{res}</body></html>";
        }

        protected virtual async Task LoadDocumentByDocNumber(string docNumber, string toPar, string searchText)
        {
            this.IsLoading = true;
            try
            {
                CurrentDocument = await this.docService.GetSmeDocByDocNumber(docNumber, (int)this.settings.Language, searchText);
                var res = CurrentDocument?.Items[0]?.Text;
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
        protected async Task ProcessCurrentDocument(string identifier)
        {
            if (CurrentDocument.Meta.IsBlob)
            {
                var fileUrl = httpService.GetStaticFileContentUrl(identifier, this.currentDocument?.Items[0]?.Text);
                this.navigationService.GoBackAsync();
                await Browser.OpenAsync(fileUrl, BrowserLaunchMode.SystemPreferred);
            }
            else
            {
                var res = CurrentDocument?.Items[0]?.Text;
                this.PrepareHtml(res, this.currentDocument.Head);
            }
        }
        protected virtual async Task LoadDocumentByIdentifier(string identifier, string toPar, string searchText)
        {
            this.IsLoading = true;

            try
            {
                CurrentDocument = await this.docService.GetSmeDocByIdentifier(identifier, searchText);
                await this.ProcessCurrentDocument(identifier);
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

        private async void DoSaveOffline()
        {
            if (!string.IsNullOrWhiteSpace(CurrentDocument?.Meta?.Idenitifier))
            {
                var document = new DocumentModel
                {
                    Identifier = CurrentDocument.Meta.Idenitifier,
                    Title = CurrentDocument.Meta.Title,
                    LastChangeDate = CurrentDocument.Meta.LastChangeDate,
                    JsonSmeDoc = Compression.CompressString(JsonConvert.SerializeObject(this.CurrentDocument)),
                };

                await documentsRepository.AddDocumentAsync(document);
                await this.dialogService.DisplayAlertAsync(Translator.GetString("Error"), Translator.GetString("The document is saved"), Translator.GetString("Ok"));
            }
        }
    }
}