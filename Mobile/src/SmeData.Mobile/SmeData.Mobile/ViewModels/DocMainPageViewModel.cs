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

namespace SmeData.Mobile.ViewModels
{
    public class DocMainPageViewModel : BaseShowPageViewModel
    {
        protected int currentDocId = 1;
        protected int currentIndex = -1;

        private BookmarksModel BookMarksForDoc { get; set; }

        public ICommand NextCommand { get; set; }
        public ICommand PPannedCommand { get; set; }
        public ICommand PrevCommand { get; set; }
        public ICommand GoToElementViewCommand { get; set; }
        public ICommand AddRemoveBookmarkCommand { get; set; }
        public ICommand AddRemoveBookmarkToolbarCommand { get; set; }

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

            this.AddRemoveBookmarkCommand = new DelegateCommand<SmeDocItem>(this.AddRemoveBookmark);
            this.AddRemoveBookmarkToolbarCommand = new DelegateCommand(this.AddRemoveBookmarkToolbar, () =>
            {
                return (this.SelectedItem?.Type != SmeDocItemType.DocTitle) &&
                       (this.SelectedItem?.Type != SmeDocItemType.Recital) &&
                       (!string.IsNullOrEmpty(this.SelectedItem?.Id));
            }).ObservesProperty(() => this.SelectedItem);
            this.GoToElementViewCommand = new DelegateCommand<SmeDocItem>(this.GoToElementView);
            this.NavigatingCommand = new DelegateCommand<string>(this.HandleNavigation);
        }

        private void AddRemoveBookmarkToolbar()
        {
            this.DoAddRemoveBookmark(this.SelectedItem);
            this.SelectedItem = this.SelectedItem;
        }

        #region Props

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
            await this.documentsRepository.SetBookmarksForDocAsync(BookMarksForDoc);
        }

        protected override async Task LoadDocumentByIdentifier(string identifier, string toPar, string searchText)
        {
            this.IsLoading = true;
            try
            {
                var orgDoc = await this.docService.GetSmeDocByIdentifier(identifier, searchText);
                this.CurrentDocument = DocumentHelper.FlatDocument(orgDoc);
                this.CurrentDocTitle = orgDoc?.Meta?.Title;
                this.DocumentItems = new ObservableCollection<SmeDocItem>(this.CurrentDocument.Items.Where(x => !string.IsNullOrWhiteSpace(x.Heading)));
                
                InitCurrentDocBookmarks(identifier, CurrentDocument.Meta.ShortTitle);
                bool isGoToPar = false;

                if (!string.IsNullOrWhiteSpace(toPar))
                {
                    Match matchParts = Regex.Match(toPar, @"(art|rec|chap)([^_]+)", RegexOptions.IgnoreCase);

                    if (matchParts.Success)
                    {
                        string newPart = $"{matchParts.Groups[1].Value.ToLower()}_{matchParts.Groups[2].Value.ToLower()}";

                        var element = this.CurrentDocument.Items.Where(x => x.Id.EndsWith(newPart))?.FirstOrDefault();

                        if (element != null)
                        {
                            this.GoToPar(this.CurrentDocument.Items.IndexOf(element));
                            isGoToPar = true;
                        }
                    }
                }

                if (!isGoToPar)
                {
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

        protected override async Task LoadDocumentByDocNumber(string docNum, string toPar, string searchText)
        {
            this.IsLoading = true;
            try
            {
                if (!(await ConnectivityHelper.CheckInternetConection(this.dialogService)))
                {

                    return;
                }
                var orgDoc = await this.docService.GetSmeDocByDocNumber(docNum, (int)settings.Language, searchText);
                if (orgDoc == null)
                {
                    await this.navigationService.GoBackAsync();
                    var celex = Apis.Common.Celex.Celex.TryParse(docNum);
                    if (celex != null)
                    {
                        await Browser.OpenAsync($"https://eur-lex.europa.eu/legal-content/{UrlNavHelper.GetEurlexCountryByLanguage(settings.Language)}/ALL/?uri=CELEX:{docNum}");
                    }
                    return;
                }
                this.CurrentDocument = DocumentHelper.FlatDocument(orgDoc);

                this.DocumentItems = new ObservableCollection<SmeDocItem>(this.CurrentDocument.Items.Where(x => !string.IsNullOrWhiteSpace(x.Heading)));

                bool isGoToPar = false;

                if (!string.IsNullOrWhiteSpace(toPar))
                {
                    Match matchParts = Regex.Match(toPar, @"(art|rec|chap)([^_]+)", RegexOptions.IgnoreCase);

                    if (matchParts.Success)
                    {
                        string newPart = $"{matchParts.Groups[1].Value.ToLower()}_{matchParts.Groups[2].Value.ToLower()}";

                        var element = this.CurrentDocument.Items.Where(x => x.Id.EndsWith(newPart))?.FirstOrDefault();

                        if (element != null)
                        {
                            this.GoToPar(this.CurrentDocument.Items.IndexOf(element));
                            isGoToPar = true;
                        }
                    }
                }

                if (!isGoToPar)
                {
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

        private void ShowDocItem(SmeDocItem selectedItem)
        {
            this.currentIndex = this.CurrentDocument.Items.IndexOf(selectedItem);
            this.currentToPar = this.CurrentDocument.Items[currentIndex].Id;
            this.PreviewItem();
        }

        private void GoPrev()
        {
            this.currentIndex--;
            this.currentIndex = Math.Max(0, this.currentIndex);
            this.currentToPar = this.CurrentDocument.Items[currentIndex].Id;
            this.PreviewItem();
        }

        private void GoNext()
        {
            this.currentIndex++;
            this.currentIndex = Math.Min(this.CurrentDocument.Items.Count - 1, this.currentIndex);
            this.currentToPar = this.CurrentDocument.Items[currentIndex].Id;
            this.PreviewItem();
        }

        private void GoToPar(int index)
        {
            this.currentIndex = index;
            this.currentToPar = this.CurrentDocument.Items[currentIndex].Id;
            this.PreviewItem();
        }

        public string GetCurrentDocItemHtml()
        {
            var res = DocumentHelper.GetDisplayText(this.CurrentDocument, settings.Language, CurrentDocument.Items[this.currentIndex]);

            var cultureInfo = Properties.Resources.Culture;
            res += "<div>";
            if (this.currentIndex > 0)
            {
                res += $"&nbsp;<a  class=\"doc-go-prev\" href=\"/{UrlNavHelper.GO_PREV}\">{Translator.GetString("Prev")}</a>&nbsp;";
            }
            if (this.currentIndex < this.CurrentDocument?.Items.Count - 1)
            {
                res += $"&nbsp;<a  class=\"doc-go-next\" href=\"/{UrlNavHelper.GO_NEXT}\">{Translator.GetString("Next")}</a>&nbsp;";
            }
            res += "</div>";
            if (this.CurrentDocument.Meta.DocType == 2)
            {
                res = $"<div class=\"d-body\">{res}</div>";
            }
            return res;
        }
        private void PreviewItem()
        {
            if (this.CurrentDocument.Items != null && this.CurrentDocument.Items.Count > 0)
            {
                this.SelectedItem = this.CurrentDocument.Items[this.currentIndex];
                this.HtmlText = $@"<html><head>{this.CurrentDocument.Head}</head>
                <body>{this.GetCurrentDocItemHtml()}</body></html>";
            }
            else
            {
                this.HtmlText = $@"<html><body></body></html>";
            }

            (this.NextCommand as DelegateCommand).RaiseCanExecuteChanged();
            (this.PrevCommand as DelegateCommand).RaiseCanExecuteChanged();
        }

        private void InitCurrentDocBookmarks(string identifier, string docTitle)
        {
            BookMarksForDoc = this.documentsRepository.GetBooksmarsForDocAsync(identifier).Result;

            if (BookMarksForDoc != null && BookMarksForDoc.BookmarksParsAndText.Count() > 0 && DocumentItems?.Count > 0)
            {
                foreach (var dItem in this.DocumentItems)
                {
                    if (BookMarksForDoc.BookmarksParsAndText.ContainsKey(dItem.Id))
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
        }

        private void AddRemoveBookmark(SmeDocItem docEntry)
        {
            this.DoAddRemoveBookmark(docEntry);
        }

        private void DoAddRemoveBookmark(SmeDocItem docEntry)
        {
            var curDocList = DocumentItems;

            if (curDocList.Any(x => x.Id == docEntry.Id))
            {
                if (curDocList.Where(x => x.Id == docEntry.Id).FirstOrDefault().IsBookmarked)
                {
                    curDocList.Where(x => x.Id == docEntry.Id).FirstOrDefault().IsBookmarked = false;
                    BookMarksForDoc.BookmarksParsAndText.Remove(docEntry.Id);
                }
                else
                {
                    curDocList.Where(x => x.Id == docEntry.Id).FirstOrDefault().IsBookmarked = true;
                    BookMarksForDoc.BookmarksParsAndText.Add(docEntry.Id, docEntry.Heading);
                }

                DocumentItems = new ObservableCollection<SmeDocItem>(curDocList);
            }
        }

        private void GoToElementView(SmeDocItem docEntry)
        {
            this.IsDocContentSelected = !this.isDocContentSelected;
            this.ShowDocItem(docEntry);
        }

        private void SetDefaultValues()
        {
            this.HtmlText = string.Empty;
            this.documentItems = new ObservableCollection<SmeDocItem>();
            this.IsLoading = true;

        }

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

