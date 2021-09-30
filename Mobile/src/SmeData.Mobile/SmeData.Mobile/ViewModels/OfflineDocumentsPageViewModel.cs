using Newtonsoft.Json;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using SmeData.Mobile.Data;
using SmeData.Mobile.Data.Models;
using SmeData.Mobile.Helpers;
using SmeData.Mobile.Services;
using SmeData.Shared.Common;
using SmeData.SharedModels.Document;
using SmeData.WebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms.Extended;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using Prism.Services;
using SmeData.Mobile.Models.Settings;

namespace SmeData.Mobile.ViewModels
{
    /// <summary>
    /// Page for displaying documents saved by the user
    /// </summary>
    public class OfflineDocumentsPageViewModel : BaseViewModel
    {
        public ObservableCollection<OfflineDocument> allDocsToShow = new ObservableCollection<OfflineDocument>();
        public ObservableCollection<OfflineDocument> allMainDocsToShow = new ObservableCollection<OfflineDocument>();
        public ObservableCollection<OfflineDocument> allDocs = new ObservableCollection<OfflineDocument>();

        private readonly AppRepository documentsRepository;
        private readonly IPageDialogService dialogService;
        private readonly HttpService httpService;

        public ICommand TabCommand { get; set; }
        public ICommand UpdateDocCommand { get; set; }
        public ICommand EraseDocCommand { get; set; }
        public DelegateCommand<object> EraseDocObjectCommand { get; private set; }

        public ICommand UpdateAllDocsCommand { get; set; }
        public ICommand UpdateAllMainDocsCommand { get; set; }

        public string UpdateAllDocsButtonFont { get => $"t|14|{ScreenWidth}"; }

        public OfflineDocumentsPageViewModel(INavigationService navigationService, AppRepository documentsRepository, HttpService httpService, IPageDialogService dialogService, SettingsModel settings) : base(navigationService)
        {
            this.documentsRepository = documentsRepository;
            this.httpService = httpService;
            this.settings = settings;

            this.TabCommand = new DelegateCommand<SmeDoc>(this.ShowDocument);
            this.UpdateDocCommand = new DelegateCommand<OfflineDocument>(this.UpdateDoc);
            this.EraseDocCommand = new DelegateCommand<OfflineDocument>(this.EraseDoc);
            this.EraseDocObjectCommand = new DelegateCommand<object>(x => this.EraseDoc((OfflineDocument)x));
            this.UpdateAllDocsCommand = new DelegateCommand(this.UpdateAllDocs);
            this.UpdateAllMainDocsCommand = new DelegateCommand(this.UpdateAllMainDocs);
            this.dialogService = dialogService;
        }

        public ObservableCollection<OfflineDocument> AllDocsToShow
        {
            get => allDocsToShow;
            set
            {
                allDocsToShow = value;
                this.RaisePropertyChanged(nameof(this.AllDocsToShow));
            }
        }

        public ObservableCollection<OfflineDocument> AllMainDocsToShow
        {
            get => allMainDocsToShow;
            set
            {
                allMainDocsToShow = value;
                this.RaisePropertyChanged(nameof(this.AllMainDocsToShow));
            }
        }

        public ObservableCollection<OfflineDocument> AllDocs
        {
            get => allDocs;
            set
            {
                allDocs = value;
                AllDocsToShow = new ObservableCollection<OfflineDocument>(allDocs.Where(x => !x.IsMainDoc).ToList());
                AllMainDocsToShow = new ObservableCollection<OfflineDocument>(allDocs.Where(x => x.IsMainDoc && x.SmeDocument.Meta.LangId == (int)this.settings.Language).ToList());
                this.RaisePropertyChanged(nameof(this.AllDocs));
            }
        }

        private bool isAnyMainDocsForUpdate = false;
        public bool IsAnyMainDocsForUpdate
        {
            get => isAnyMainDocsForUpdate;
            set
            {
                isAnyMainDocsForUpdate = value;
                this.RaisePropertyChanged(nameof(this.IsAnyMainDocsForUpdate));
            }
        }

        private bool isAnyDocsForUpdate = false;
        public bool IsAnyDocsForUpdate
        {
            get => isAnyDocsForUpdate;
            set
            {
                isAnyDocsForUpdate = value;
                this.RaisePropertyChanged(nameof(this.IsAnyDocsForUpdate));
            }
        }

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

        private async void UpdateAllMainDocs()
        {
            foreach (var doc in AllMainDocsToShow)
            {
                if (doc.IsForUpdate)
                {
                    UpdateDoc(doc);
                }
            }

            IsAnyMainDocsForUpdate = false;
        }

        private async void UpdateAllDocs()
        {
            foreach (var doc in AllDocsToShow)
            {
                if (doc.IsForUpdate)
                {
                    UpdateDoc(doc);
                }
            }

            IsAnyDocsForUpdate = false;
        }

        private async void UpdateDoc(OfflineDocument doc)
        {
            if (doc == null)
            {
                return;
            }

            doc.IsLoading = true;
            try
            {
                if (!(await ConnectivityHelper.CheckInternetConection(this.dialogService)))
                {
                    return;
                }

                var updatedDoc = await httpService.GetSmeDocByIdentifier(doc.SmeDocument.Meta.Idenitifier, null);
                await documentsRepository.UpdateDocumentAsync(
                    new DocumentModel()
                    {
                        Identifier = updatedDoc.Meta.Idenitifier,
                        Title = updatedDoc.Meta.Title,
                        IsMainDoc = doc.IsMainDoc,
                        IsToHide = doc.IsToHide,
                        LastChangeDate = updatedDoc.Meta.LastChangeDate,
                        JsonSmeDoc = Compression.CompressString(JsonConvert.SerializeObject(updatedDoc))
                    });

                AllDocs.Remove(doc);
                var newDoc = new OfflineDocument(doc.SmeDocument, false);
                newDoc.IsToHide = doc.IsToHide;
                AllDocs.Add(newDoc);

                if (doc.IsMainDoc)
                {
                    AllMainDocsToShow.Remove(doc);
                    AllMainDocsToShow.Add(newDoc);
                    IsAnyMainDocsForUpdate = AllMainDocsToShow.Any(x => x.IsForUpdate);
                }
                else
                {
                    AllDocsToShow.Remove(doc);
                    AllDocsToShow.Add(newDoc);

                    IsAnyDocsForUpdate = AllDocsToShow.Any(x => x.IsForUpdate);
                }

                
            }
            catch (Exception ex)
            {

                await ErrorsHelper.DisplayError(this.dialogService, ex);
            }

            finally
            {
                doc.IsLoading = false;
            }
        }

        private async void EraseDoc(OfflineDocument doc)
        {
            if (doc == null)
            {
                return;
            }

            try
            {
                await documentsRepository.DeleteDocumentAsync(doc.SmeDocument.Meta.Idenitifier);

                AllDocs.Remove(doc);

                if (doc.IsMainDoc)
                {
                    AllMainDocsToShow.Remove(doc);
                    IsAnyMainDocsForUpdate = AllMainDocsToShow.Any(x => x.IsForUpdate);
                }
                else
                {
                    AllDocsToShow.Remove(doc);
                    IsAnyDocsForUpdate = AllDocsToShow.Any(x => x.IsForUpdate);
                }
            }
            catch (Exception ex)
            {
                await ErrorsHelper.DisplayError(this.dialogService, ex);
            }
        }

        private void ShowDocument(SmeDoc doc)
        {
            if (doc == null)
            {
                return;
            }

            string pageToNavigate = "DocMainPage";

            if (doc.Meta.DocType != 2)
            {
                pageToNavigate = "DocCaseLawShowPage";
            }

            if (!string.IsNullOrEmpty(doc.Meta.Idenitifier))
            {
                this.navigationService.NavigateAsync($"{pageToNavigate}?{UrlNavHelper.IDENTIFIER}={doc.Meta.Idenitifier}&{UrlNavHelper.FULL_TITLE}={Uri.EscapeDataString(doc.Meta.Title)}&{UrlNavHelper.IS_OFFLINE}=true");
            }
            else
            {
                this.navigationService.NavigateAsync($"{pageToNavigate}?{UrlNavHelper.DOC_NUM}={doc.Meta.DocNumber}&{UrlNavHelper.FULL_TITLE}={Uri.EscapeDataString(doc.Meta.Title)}&{UrlNavHelper.IS_OFFLINE}=true");
            }
        }

        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            if (this.AllDocs?.Count == 0)
            {
                IsLoading = true;

                try
                {
                    List<DocumentModel> allDocsSaved = await documentsRepository.GetDocumentsAsync();



                    allDocsSaved = allDocsSaved.Where(x => !x.IsToHide).ToList();
                    List<SmeDoc> allDocsSaveSmeDocs = ConvertDocumentModelToSmeDocList(allDocsSaved);

                    List<LastChangeOfDoc> allDocIdentsWithLastChange = new List<LastChangeOfDoc>();

                    foreach (var doc in allDocsSaved)
                    {
                        allDocIdentsWithLastChange.Add(new LastChangeOfDoc() { Ident = doc.Identifier, LastChangeDate = doc.LastChangeDate });
                    }

                    List<LastChangeOfDoc> updatedDocuments = new List<LastChangeOfDoc>();

                    if (Connectivity.NetworkAccess == NetworkAccess.Internet)
                    {
                        updatedDocuments = await httpService.GetUpdatedDocuments(allDocIdentsWithLastChange);
                        //updatedDocuments = new List<LastChangeOfDoc>() { new LastChangeOfDoc() { Ident = "a5a8acc2-9184-4946-9698-0b7a8c2120de", LastChangeDate = null } };
                    }

                    var docsForUpdate = allDocsSaveSmeDocs.Where(x => updatedDocuments.Any(y => y.Ident == x.Meta.Idenitifier)).
                        Select(x => new OfflineDocument(x, true)).ToList();

                    allDocsSaveSmeDocs.RemoveAll(x => docsForUpdate.Any(y => y.SmeDocument == x));

                    var docsUpToDate = allDocsSaveSmeDocs.Select(x => new OfflineDocument(x, false)).ToList();
                    var AllDocsFinalList = new List<OfflineDocument>();
                    AllDocsFinalList.AddRange(docsForUpdate);
                    AllDocsFinalList.AddRange(docsUpToDate);

                    AllDocsFinalList.ForEach(x =>
                    {
                        x.IsToHide = true;

                        if (allDocsSaved.Any(y => y.Identifier == x.SmeDocument.Meta.Idenitifier && y.IsMainDoc))
                        {
                            x.IsMainDoc = true;
                        }
                    });

                    AllDocs = new ObservableCollection<OfflineDocument>(AllDocsFinalList);

                    IsAnyDocsForUpdate = AllDocsToShow.Any(x => x.IsForUpdate);
                    IsAnyMainDocsForUpdate = AllMainDocsToShow.Any(x => x.IsForUpdate);
                }
                finally
                {
                    IsLoading = false;
                }
            }
        }

        public async Task<InfiniteScrollCollection<SmeDoc>> GetItemsAsync(List<SmeDoc> allDocsSaved, int pageIndex, int pageSize)
        {
            await Task.Delay(1000).ConfigureAwait(false);

            return new InfiniteScrollCollection<SmeDoc>(allDocsSaved.Skip(pageIndex).Take(pageSize));
        }

        private List<SmeDoc> ConvertDocumentModelToSmeDocList(List<DocumentModel> allDocsSaved)
        {
            var resultList = new List<SmeDoc>();

            foreach (var doc in allDocsSaved)
            {
                try
                {
                    resultList.Add(JsonConvert.DeserializeObject<SmeDoc>(Compression.DecompressString(doc.JsonSmeDoc)));
                }
                catch (Exception e)
                {
                }
            }

            return resultList;
        }
    }

    public class OfflineDocument : BindableBase
    {
        public SmeDoc SmeDocument { get; set; }

        public bool IsForUpdate { get; set; }

        public bool IsMainDoc { get; set; }

        public bool IsToHide { get; set; }

        private bool isLoading = false;
        public bool IsLoading
        {
            get
            {
                return this.isLoading;
            }
            set
            {
                this.isLoading = value;
                this.RaisePropertyChanged(nameof(this.IsLoading));
            }
        }
        public OfflineDocument(SmeDoc smeDocument, bool isForUpdate, bool isMainDoc = false)
        {
            this.SmeDocument = smeDocument;
            this.IsForUpdate = isForUpdate;
            this.IsMainDoc = isMainDoc;
        }
    }
}
