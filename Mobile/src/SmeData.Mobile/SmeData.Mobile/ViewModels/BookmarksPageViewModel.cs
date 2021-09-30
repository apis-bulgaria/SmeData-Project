using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;
using SmeData.Mobile.Data;
using SmeData.Mobile.Helpers;
using SmeData.Mobile.Models.Settings;
using SmeData.Mobile.Services;
using SmeData.SharedModels.Bookmark;
using SmeData.SharedModels.Document;
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
    /// Page with saved bookmarks
    /// </summary>
    public class BookmarksPageViewModel : BaseViewModel
    {
        private readonly AppRepository documentsRepository;
        private readonly HttpService httpService;
        private readonly IPageDialogService dialogService;

        public ICommand GoToElementViewCommand { get; set; }
        public ICommand ReloadBookmarksCommand { get; set; }

        /// <summary>
        /// Information for font size calculation for label with doc title
        /// </summary>
        public string DocTitleLabelFont { get => $"t|17|{ScreenWidth}"; }

        /// <summary>
        /// Information for font size calculation for bookmarks
        /// </summary>
        public string BookmarksFont { get => $"t|15|{ScreenWidth}"; }

        public BookmarksPageViewModel(INavigationService navigationService, AppRepository documentsRepository, HttpService httpService, IPageDialogService dialogService) : base(navigationService)
        {
            this.documentsRepository = documentsRepository;
            this.httpService = httpService;
            this.dialogService = dialogService;
            this.GoToElementViewCommand = new DelegateCommand<BookmarkModel>(this.GoToElementView);
            this.ReloadBookmarksCommand = new DelegateCommand(this.ReloadBookmarks);
        }

        /// <summary>
        /// Infinite list view collection for all bookmarks
        /// </summary>
        public ObservableCollection<BookmarkModelList> allBookmarks = new ObservableCollection<BookmarkModelList>();
        public ObservableCollection<BookmarkModelList> AllBookmarks
        {
            get => allBookmarks;
            set
            {
                allBookmarks = value;
                this.RaisePropertyChanged(nameof(this.AllBookmarks));
            }
        }

        /// <summary>
        /// Property for activation/deactivation of loading indicator for list of all bookmarks
        /// </summary>
        private bool isLoading;
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
        private bool isBusy;
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
        /// Navitage to selected bookmarked element of document
        /// </summary>
        /// <param name="bookmark"></param>
        private void GoToElementView(BookmarkModel bookmark)
        {
            this.navigationService.NavigateAsync($"DocMainPage?{UrlNavHelper.IDENTIFIER}={bookmark.DocIdentifier}&{UrlNavHelper.TO_PAR}={Regex.Replace(Regex.Replace(bookmark.PartId, @"^\d+\#", string.Empty), @"([^_])_([^_])", @"$1$2")}");
        }

        /// <summary>
        /// Override OnNavigatedTo method with added logic for load bookmarks
        /// </summary>
        /// <param name="parameters">Input parameters</param>
        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            await GetAllBookmarks();
        }

        /// <summary>
        /// Method for reloading bookmarks on page
        /// </summary>
        private void ReloadBookmarks()
        {
            GetAllBookmarks();
        }

        /// <summary>
        /// Method for filling AllBookmarks collection
        /// </summary>
        private async Task GetAllBookmarks()
        {
            IsLoading = true;
            try
            {
                var bookmarksFormDb = await documentsRepository.GetAllBooksmarsAsync();

                if (bookmarksFormDb != null)
                {
                    List<BookmarkModelList> tampAllBookmarks = new List<BookmarkModelList>();

                    foreach (var bookmark in bookmarksFormDb)
                    {
                        if (bookmark != null && bookmark.BookmarksParsAndText != null && bookmark.BookmarksParsAndText.Count > 0)
                        {
                            var bookmarkList = new BookmarkModelList();
                            bookmarkList.DocTitle = bookmark.DocTitle;

                            foreach (var parId in bookmark.BookmarksParsAndText.Keys)
                            {
                                bookmarkList.Add(new BookmarkModel() { DocIdentifier = bookmark.DocIdentifier, PartId = parId, PartHeading = bookmark.BookmarksParsAndText[parId] });
                            }

                            tampAllBookmarks.Add(bookmarkList);
                        }
                    }

                    int pageSize = 10;

                    //AllBookmarks = new InfiniteScrollCollection<BookmarkModelList>
                    //{
                    //    OnLoadMore = async () =>
                    //    {
                    //        IsBusy = true;

                    //        var items = await GetItemsAsync(tampAllBookmarks, AllBookmarks.Count, pageSize);

                    //        IsBusy = false;
                    //        return items;
                    //    },
                    //    OnCanLoadMore = () =>
                    //    {
                    //        return AllBookmarks.Count < tampAllBookmarks.Count;
                    //    }
                    //};

                    if (tampAllBookmarks?.Count > 0)
                    {
                        //AllBookmarks.AddRange(GetItemsAsync(tampAllBookmarks, 0, pageSize).GetAwaiter().GetResult());
                        AllBookmarks = new ObservableCollection<BookmarkModelList>();
                        tampAllBookmarks.ForEach(AllBookmarks.Add);
                    }
                }
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

        /// <summary>
        /// Mathod for loading more elements of infinite collection
        /// </summary>
        /// <param name="allResultBookmarks">Collection with all bookmarks</param>
        /// <param name="pageIndex">Index of loaded pages</param>
        /// <param name="pageSize">Size of one page</param>
        /// <returns></returns>
        public async Task<InfiniteScrollCollection<BookmarkModelList>> GetItemsAsync(List<BookmarkModelList> allResultBookmarks, int pageIndex, int pageSize)
        {
            await Task.Delay(1000).ConfigureAwait(false);

            return new InfiniteScrollCollection<BookmarkModelList>(allResultBookmarks.Skip(pageIndex).Take(pageSize));
        }
    }
}
