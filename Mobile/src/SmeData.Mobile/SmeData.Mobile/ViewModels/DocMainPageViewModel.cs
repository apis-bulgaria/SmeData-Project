using Newtonsoft.Json;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Navigation.TabbedPages;
using SmeData.Mobile.Data;
using SmeData.Mobile.Data.Models;
using SmeData.Mobile.Helpers;
using SmeData.Mobile.Services;
using SmeData.SharedModels.Document;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using SmeData.Shared.Common;
using System.Text.RegularExpressions;
using SmeData.Mobile.Models.Settings;
using Prism.Services;
using Xamarin.Essentials;
using SmeData.SharedModels.Language;
using SmeData.Mobile.CustomControls;
using SmeData.Mobile.Models.Documents;

namespace SmeData.Mobile.ViewModels
{
    /// <summary>
    /// Tabbed page for showing complex legal documents with content and hierarchical structure
    /// </summary>
    public class DocMainPageViewModel : BaseShowPageViewModel
    {
        protected int currentDocId = 1;
        protected int currentIndex = -1;

        private BookmarksModel BookMarksForDoc { get; set; }

        private SortedDictionary<string, string> SortedBookmarksAndPars { get; set; }

        public ICommand NextCommand { get; set; }
        public ICommand PPannedCommand { get; set; }
        public ICommand RightSwipeCommand { get; set; }
        public ICommand LeftSwipeCommand { get; set; }
        public ICommand PrevCommand { get; set; }
        public ICommand GoToElementViewCommand { get; set; }
        public ICommand AddRemoveBookmarkCommand { get; set; }
        public ICommand AddRemoveBookmarkToolbarCommand { get; set; }
        public ICommand PageChangeCommand { get; set; }

        public DocMainPageViewModel(INavigationService navigationService, DocumentService docService, IPageDialogService dialogService, AppRepository documentsRepository,
            SettingsModel settings, HttpService httpService) : base(navigationService, dialogService, docService, settings, httpService, documentsRepository)
        {
            this.SetDefaultValues();

            this.PPannedCommand = new Command<PannedDirection>((e) =>
            {
                if (e == PannedDirection.Left)
                {
                    this.GoNext();
                }

                if (e == PannedDirection.Right)
                {
                    this.GoPrev();
                }
            });

            this.NextCommand = new DelegateCommand(this.GoNext, () =>
            {
                return this.currentIndex < this.CurrentDocument?.Items.Count - 1;
            });

            this.PrevCommand = new DelegateCommand(this.GoPrev, () =>
            {
                return this.currentIndex > 0;
            });

            this.ToogleIsSearchBarVisibleCommand = new DelegateCommand(this.ToogleIsSearchBarVisible, () =>
             {
                 return (this?.TabIndex == 1);
             }).ObservesProperty(() => this.TabIndex);

            this.AddRemoveBookmarkCommand = new DelegateCommand<SmeDocItem>(this.AddRemoveBookmark);
            this.AddRemoveBookmarkToolbarCommand = new DelegateCommand(this.AddRemoveBookmarkToolbar, () =>
            {
                return (this.SelectedItem?.Type != SmeDocItemType.DocTitle) &&
                       (this.SelectedItem?.Type != SmeDocItemType.Recital) &&
                       (!string.IsNullOrEmpty(this.SelectedItem?.Id));
            }).ObservesProperty(() => this.SelectedItem);

            this.GoToElementViewCommand = new DelegateCommand<SmeDocItem>(this.GoToElementView);
            this.NavigatingCommand = new DelegateCommand<string>(this.HandleNavigation);
            this.PageChangeCommand = new DelegateCommand<int?>(this.PageChange);

            this.RightSwipeCommand = new DelegateCommand(this.RightSwipe);
            this.LeftSwipeCommand = new DelegateCommand(this.LeftSwipe);
            this.DecreaseFontCommand = new DelegateCommand(this.DecreaseFont);
            this.IncreaseFontCommand = new DelegateCommand(this.IncreaseFont);
        }

        private int? tabIndexProp;
        public int? TabIndex
        {
            get { return this.tabIndexProp; }
            set
            {
                this.tabIndexProp = value;
                this.RaisePropertyChanged(nameof(this.TabIndex));
            }
        }

        protected int artInListHeadingFontDefault = 16;
        protected int artInListHeadingFont = 16;
        public int ArtInListHeadingFont
        {
            get => this.artInListHeadingFont;
            set
            {
                this.artInListHeadingFont = value;
                this.RaisePropertyChanged(nameof(ArtInListHeadingFont));
                this.RaisePropertyChanged(nameof(ArtInListHeadingLabelFont));
            }
        }

        public string ArtInListHeadingLabelFont { get => $"t|{ArtInListHeadingFont + settings.FontIndex}|{ScreenWidth}"; }

        protected int artInListSubheadingFontDefault = 14;
        protected int artInListSubheadingFont = 14;
        public int ArtInListSubheadingFont
        {
            get => this.artInListSubheadingFont;
            set
            {
                this.artInListSubheadingFont = value;
                this.RaisePropertyChanged(nameof(ArtInListSubheadingFont));
                this.RaisePropertyChanged(nameof(ArtInListSubheadingLabelFont));
            }
        }

        public string ArtInListSubheadingLabelFont { get => $"t|{ArtInListSubheadingFont + settings.FontIndex}|{ScreenWidth}"; }
        private void PageChange(int? tabIndex)
        {
            this.TabIndex = tabIndex;
        }

        /// <summary>
        /// Change the status of document from boookmarked/non-bookmarked to opposite
        /// </summary>
        private void AddRemoveBookmarkToolbar()
        {
            this.DoAddRemoveBookmark(this.SelectedItem);
            this.SelectedItem = this.SelectedItem;
        }

        #region Props

        /// <summary>
        /// Marker for if doc content tab is active
        /// </summary>
        private bool isDocContentSelected;
        public bool IsDocContentSelected
        {
            get => isDocContentSelected;
            set
            {
                isDocContentSelected = value;
                this.RaisePropertyChanged(nameof(this.IsDocContentSelected));
            }
        }

        /// <summary>
        /// Observable collection with documents
        /// </summary>
        public ObservableCollection<SmeDocItem> documentItems;
        public ObservableCollection<SmeDocItem> DocumentItems
        {
            get => documentItems;
            set
            {
                documentItems = value;
                this.RaisePropertyChanged(nameof(this.DocumentItems));
            }
        }

        #endregion Props End


        public override async void OnNavigatedFrom(INavigationParameters parameters)
        {
            base.OnNavigatedFrom(parameters);
            if (this.SortedBookmarksAndPars == null)
            {
                SortedBookmarksAndPars = new SortedDictionary<string, string>();
            }
            if (this.BookMarksForDoc != null)
            {
                BookMarksForDoc.BookmarksParsAndText = new Dictionary<string, string>(SortedBookmarksAndPars);
                await this.documentsRepository.SetBookmarksForDocAsync(BookMarksForDoc);
            }
        }

        /// <summary>
        /// Load document by identifier
        /// </summary>
        /// <param name="identifier">Input identifier</param>
        /// <param name="toPar">Certain par to navigate</param>
        /// <param name="searchText">Text for search if not null</param>
        /// <returns></returns>
        protected override async Task LoadDocumentByIdentifier(string identifier, string toPar, string searchText, SmeDoc docFormRepo)
        {
            if (docFormRepo == null)
            {
                docFormRepo = await this.docService.GetSmeDocByIdentifier(identifier, searchText, (int)settings.Language);
            }

            await this.LoadDocument(identifier, toPar, searchText, docFormRepo);
        }

        /// <summary>
        /// Load the document
        /// </summary>
        /// <param name="identOrDocNum">Input identifier ot docNumber</param>
        /// <param name="toPar">Certain par to navigate</param>
        /// <param name="searchText">Text for search if not null</param>
        /// <param name="getSmeDocFunc"></param>
        /// <returns></returns>
        private async Task LoadDocument(string identOrDocNum, string toPar, string searchText, SmeDoc smeDoc)
        {
            this.IsLoading = true;
            try
            {
                this.colorizeInfos = new List<ColorizeInfo>();
                this.currentColorizeIndex = -1;

                if (smeDoc.Meta.IsBlob == true)
                {
                    var fileUrl = httpService.GetStaticFileContentUrl(identOrDocNum, smeDoc.Items[0]?.Text);
                    this.navigationService.GoBackAsync();
                    await Browser.OpenAsync(fileUrl, BrowserLaunchMode.SystemPreferred);
                }
                else
                {
                    if (settings.ShowDocsOnWifiOnly && !await ConnectivityHelper.CheckForWifiConnection(this.dialogService))
                    {
                        return;
                    }

                    var orgDoc = smeDoc;
                    if (orgDoc == null)
                    {
                        await this.navigationService.GoBackAsync();
                        var celex = Apis.Common.Celex.Celex.TryParse(identOrDocNum);
                        if (celex != null)
                        {
                            await Browser.OpenAsync($"https://eur-lex.europa.eu/legal-content/{UrlNavHelper.GetEurlexCountryByLanguage(settings.Language)}/ALL/?uri=CELEX:{identOrDocNum}");
                        }
                        return;
                    }

                    if (!orgDoc.Items.Any(x => Regex.IsMatch(x.Heading.Trim(), @"^(Recitals|Съображения|Considerando)\s{1,5}1\s{0,5}\-")))
                    {
                        this.CurrentDocument = DocumentHelper.FlatDocument(orgDoc);
                    }
                    else
                    {
                        this.CurrentDocument = orgDoc;
                    }

                    if (string.IsNullOrWhiteSpace(this.CurrentDocTitle) && !string.IsNullOrWhiteSpace(orgDoc?.Meta?.Title))
                    {
                        this.CurrentDocTitle = orgDoc?.Meta?.Title;
                    }

                    if (!string.IsNullOrWhiteSpace(orgDoc?.Meta?.ShortTitle))
                    {
                        this.currentDocShortTitle = orgDoc?.Meta?.ShortTitle;
                    }

                    this.DocumentItems = new ObservableCollection<SmeDocItem>(this.CurrentDocument.Items.Where(x => !string.IsNullOrWhiteSpace(x.Heading)));

                    if (DocumentItems.Count > 0)
                    {
                        InitCurrentDocBookmarks(this.CurrentDocument.Meta.Idenitifier, !string.IsNullOrWhiteSpace(this.currentDocShortTitle) ? this.currentDocShortTitle : this.CurrentDocTitle);
                    }
                    else
                    {
                        this.IsDocContentSelected = true;
                    }

                    if (!string.IsNullOrEmpty(searchText))
                    {
                        this.colorizeInfos = DocumentHelper.GetColorizeInfo(this.CurrentDocument);
                        this.currentColorizeIndex = -1;
                        this.GotoNextMatch();
                        this.IsDocContentSelected = true;
                        this.IsSearchVisible = true;
                        this.TextInField = searchText;
                        return;
                    }

                    if (this.ShouldGoToPar(toPar, out SmeDocItem docItem))
                    {
                        this.GoToPar(this.CurrentDocument.Items.IndexOf(docItem));
                        this.IsDocContentSelected = true;
                        return;
                    }

                    this.GoNext();
                }
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

        /// <summary>
        /// Method for navigating to next match in search mode
        /// </summary>
        protected override void DoGotoNextMatch()
        {
            this.GotoMatch();

        }

        /// <summary>
        /// Method for navigating to previous match in search mode
        /// </summary>
        protected override void DoGotoPrevMatch()
        {
            this.GotoMatch();
        }

        /// <summary>
        /// Method for navigating to first match in search mode
        /// </summary>
        private void GotoMatch()
        {
            var docItemIndex = this.CurrentDocument.Items.IndexOf(this.colorizeInfos[this.currentColorizeIndex].DocItem);
            this.currentIndex = docItemIndex;
            this.PreviewItem(true);
        }

        /// <summary>
        /// Method for cheking if input toPar is main par element 
        /// </summary>
        /// <param name="toPar">Input par element</param>
        /// <param name="docItem">Document item to navigate</param>
        /// <returns></returns>
        private bool ShouldGoToPar(string toPar, out SmeDocItem docItem)
        {
            bool isGoToPar = false;
            docItem = null;
            if (!string.IsNullOrWhiteSpace(toPar))
            {
                Match matchParts = Regex.Match(toPar, @"(art|rec|chap|sec)([^_]+)", RegexOptions.IgnoreCase);

                if (matchParts.Success)
                {
                    string newPart = $"{matchParts.Groups[1].Value.ToLower()}_{matchParts.Groups[2].Value.ToLower()}";

                    var element = this.CurrentDocument.Items.Where(x => x.Id.EndsWith(newPart))?.FirstOrDefault();

                    if (element != null)
                    {
                        docItem = element;
                        isGoToPar = true;
                    }
                }
            }

            return isGoToPar;
        }

        /// <summary>
        /// Load document by doc number
        /// </summary>
        /// <param name="docNum">Input doc number</param>
        /// <param name="toPar">Certain par to navigate</param>
        /// <param name="searchText">Text for search if not null</param>
        /// <returns></returns>
        protected override async Task LoadDocumentByDocNumber(string docNum, string toPar, string searchText, SmeDoc smeDoc)
        {
            if (smeDoc == null)
            {
                smeDoc = await this.docService.GetSmeDocByDocNumber(docNum, searchText, (int)settings.Language);
            }

            await this.LoadDocument(docNum, toPar, searchText, smeDoc);
        }

        /// <summary>
        /// Method for navigationg to certain doc item
        /// </summary>
        /// <param name="selectedItem">Input doc item</param>
        private void ShowDocItem(SmeDocItem selectedItem)
        {
            this.currentIndex = this.CurrentDocument.Items.IndexOf(selectedItem);
            this.currentToPar = this.CurrentDocument.Items[currentIndex].Id;
            this.PreviewItem();
        }

        /// <summary>
        /// Navigates to previous doc item
        /// </summary>
        private void GoPrev()
        {
            this.currentIndex--;
            this.currentIndex = Math.Max(0, this.currentIndex);
            this.PreviewItem();
        }

        /// <summary>
        /// Navigates to next doc item
        /// </summary>
        private void GoNext()
        {
            this.currentIndex++;
            this.currentIndex = Math.Min(this.CurrentDocument.Items.Count - 1, this.currentIndex);
            this.PreviewItem();
        }

        /// <summary>
        /// Go to par with certain index
        /// </summary>
        /// <param name="index">Input index of par</param>
        private void GoToPar(int index)
        {
            this.currentIndex = index;
            this.PreviewItem();
        }

        /// <summary>
        /// Return html text of the current doc item
        /// </summary>
        /// <param name="colorizeNav">Colorize parameter</param>
        /// <returns>Current doc item html</returns>
        public string GetCurrentDocItemHtml(bool colorizeNav = false)
        {
            var res = DocumentHelper.GetDisplayText(this.CurrentDocument, settings.Language, CurrentDocument.Items[this.currentIndex]);

            var cultureInfo = Properties.Resources.Culture;
            res += "<div>";
            if (this.currentIndex > 0)
            {
                res += $"&nbsp;<a  class=\"doc-go-prev\" href=\"{UrlNavHelper.GO_PREV}\">{Translator.GetString("Prev")}</a>&nbsp;";
            }

            if (this.currentIndex < this.CurrentDocument?.Items.Count - 1)
            {
                res += $"&nbsp;<a  class=\"doc-go-next\" href=\"{UrlNavHelper.GO_NEXT}\">{Translator.GetString("Next")}</a>&nbsp;";
            }

            res += "</div>";
            if (this.CurrentDocument.Meta.DocType == 2)
            {
                res = $"<div class=\"d-body\">{res}</div>";
            }

            if (colorizeNav)
            {
                if (this.SelectedItem == this.colorizeInfos[this.currentColorizeIndex].DocItem)
                {
                    res += GetColorizeScript();
                }
            }

            return res;
        }

        /// <summary>
        /// Method for prapare item html for presenting in the application 
        /// </summary>
        /// <param name="colorizeNav">Colorize parameter</param>
        private void PreviewItem(bool colorizeNav = false)
        {
            this.currentToPar = this.CurrentDocument.Items[currentIndex].Id;
            if (this.CurrentDocument.Items != null && this.CurrentDocument.Items.Count > 0)
            {
                this.SelectedItem = this.CurrentDocument.Items[this.currentIndex];
                this.HtmlText = $@"<html><head>{SetFontSizeInHead(this.CurrentDocument.Head)}</head>
                <body>{this.GetCurrentDocItemHtml(colorizeNav)}</body></html>";
            }
            else
            {
                this.HtmlText = $@"<html><body></body></html>";
            }

            (this.NextCommand as DelegateCommand).RaiseCanExecuteChanged();
            (this.PrevCommand as DelegateCommand).RaiseCanExecuteChanged();
        }

        /// <summary>
        /// Method for loading all bookmarks for the current document
        /// </summary>
        /// <param name="identifier">Identifier of the document</param>
        /// <param name="docTitle">Title of the document</param>
        private void InitCurrentDocBookmarks(string identifier, string docTitle)
        {
            BookMarksForDoc = this.documentsRepository.GetBooksmarsForDocAsync(identifier).Result;

            if (BookMarksForDoc != null && BookMarksForDoc.BookmarksParsAndText.Count() > 0 && DocumentItems?.Count > 0)
            {
                foreach (var dItem in this.DocumentItems)
                {
                    if (BookMarksForDoc.BookmarksParsAndText.Any(x => Regex.IsMatch(x.Key, $"^\\d+\\#{dItem.Id}$") || x.Key == dItem.Id))
                    {
                        dItem.IsBookmarked = true;
                    }
                }
            }

            if (BookMarksForDoc == null)
            {
                BookMarksForDoc = new BookmarksModel();
                BookMarksForDoc.DocIdentifier = identifier;
                BookMarksForDoc.DocTitle = docTitle;
                BookMarksForDoc.BookmarksParsAndText = new Dictionary<string, string>();
            }

            SortedBookmarksAndPars = new SortedDictionary<string, string>();

            int index = 1;

            foreach (var bookmarkId in BookMarksForDoc.BookmarksParsAndText.Keys)
            {
                if (Regex.IsMatch(bookmarkId, @"^\d+\#"))
                {
                    SortedBookmarksAndPars.Add(bookmarkId, BookMarksForDoc.BookmarksParsAndText[bookmarkId]);
                }
                else
                {
                    if (DocumentItems != null)
                    {
                        var curEl = DocumentItems.Where(x => x.Id == bookmarkId).FirstOrDefault();

                        if (curEl != null)
                        {
                            SortedBookmarksAndPars.Add($"{(DocumentItems.IndexOf(curEl)).ToString().PadLeft(4, '0')}#{bookmarkId}", BookMarksForDoc.BookmarksParsAndText[bookmarkId]);

                            continue;
                        }
                    }

                    SortedBookmarksAndPars.Add($"{(index++).ToString().PadLeft(4, '0')}#{bookmarkId}", BookMarksForDoc.BookmarksParsAndText[bookmarkId]);
                }
            }
        }

        /// <summary>
        /// Add or removes bookmark of the doc item
        /// </summary>
        /// <param name="docEntry">Input doc item</param>
        private void AddRemoveBookmark(SmeDocItem docEntry)
        {
            this.DoAddRemoveBookmark(docEntry);
        }

        /// <summary>
        /// Add or removes bookmark of the doc item
        /// </summary>
        /// <param name="docEntry">Input doc item</param>
        private void DoAddRemoveBookmark(SmeDocItem docEntry)
        {
            var curDocList = DocumentItems;
            if (curDocList.Any(x => x.Id == docEntry.Id))
            {
                var indexOfEl = curDocList.IndexOf(docEntry);
                string dicKey = $"{(indexOfEl).ToString().PadLeft(4, '0')}#{docEntry.Id}";

                if (curDocList.Where(x => x.Id == docEntry.Id).FirstOrDefault().IsBookmarked)
                {
                    curDocList.Where(x => x.Id == docEntry.Id).FirstOrDefault().IsBookmarked = false;
                    SortedBookmarksAndPars.Remove(dicKey);
                }
                else
                {
                    curDocList.Where(x => x.Id == docEntry.Id).FirstOrDefault().IsBookmarked = true;                    
                    SortedBookmarksAndPars.Add(dicKey, docEntry.Heading);
                }

                DocumentItems = new ObservableCollection<SmeDocItem>(curDocList);
            }
        }

        /// <summary>
        /// Go to input doc item view
        /// </summary>
        /// <param name="docEntry">Input doc item</param>
        private void GoToElementView(SmeDocItem docEntry)
        {
            this.IsDocContentSelected = !this.IsDocContentSelected;
            this.ShowDocItem(docEntry);
        }

        /// <summary>
        /// Initialise all default values when page is open
        /// </summary>
        private void SetDefaultValues()
        {
            this.HtmlText = string.Empty;
            this.documentItems = new ObservableCollection<SmeDocItem>();
            this.IsLoading = true;

        }

        private void RightSwipe()
        {
            this.GoPrev();
        }

        private void LeftSwipe()
        {
            this.GoNext();
        }

        protected override void DecreaseFont()
        {
            if (this.settings.FontIndex > -10)
            {
                this.settings.FontIndex--;
                DocTitleFont = docTitleFontDefault + this.settings.FontIndex;
                ArtInListHeadingFont = artInListHeadingFontDefault + this.settings.FontIndex;
                ArtInListSubheadingFont = artInListSubheadingFontDefault + this.settings.FontIndex;
                this.PreviewItem();
            }
        }

        protected override void IncreaseFont()
        {
            if (this.settings.FontIndex < 10)
            {
                this.settings.FontIndex++;
                DocTitleFont = docTitleFontDefault + this.settings.FontIndex;
                ArtInListHeadingFont = artInListHeadingFontDefault + this.settings.FontIndex;
                ArtInListSubheadingFont = artInListSubheadingFontDefault + this.settings.FontIndex;
                this.PreviewItem();
            }
        }

        /// <summary>
        /// Override of base HandleNavigation method with functionality for going on next and previous doc items 
        /// </summary>
        /// <param name="url">Url to navigate to</param>
        protected override void HandleNavigation(string url)
        {
            base.HandleNavigation(url);
            if (Regex.IsMatch(url, $"{UrlNavHelper.GO_NEXT}", RegexOptions.IgnoreCase))
            {
                this.GoNext();
            }
            if (Regex.IsMatch(url, $"{UrlNavHelper.GO_PREV}", RegexOptions.IgnoreCase))
            {
                this.GoPrev();
            }
        }
    }
}

