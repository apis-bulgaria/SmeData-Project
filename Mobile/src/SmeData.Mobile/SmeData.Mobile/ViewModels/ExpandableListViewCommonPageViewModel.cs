using MvvmHelpers;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;
using SmeData.Mobile.Helpers;
using SmeData.Mobile.Models;
using SmeData.Mobile.Models.ExpandedListView;
using SmeData.Mobile.Models.Settings;
using SmeData.Mobile.Services;
using SmeData.Mobile.ViewModels.ExpandedListView;
using SmeData.WebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace SmeData.Mobile.ViewModels
{
    /// <summary>
    /// Expandable list view used for some elements
    /// </summary>
    public class ExpandableListViewCommonPageViewModel : BaseViewModel
    {
        /// <summary>
        /// Expandable list view
        /// </summary>
        public ObservableRangeCollection<ExpandableLVViewModel> ExpandableDocList { get; private set; } = new ObservableRangeCollection<ExpandableLVViewModel>();

        private readonly HttpService httpService;
        protected readonly IPageDialogService dialogService;
        protected readonly DocumentService docService;

        /// <summary>
        /// 
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

        public ExpandableListViewCommonPageViewModel(INavigationService navigationService, IPageDialogService dialogService, SettingsModel settings, HttpService httpService, DocumentService docService) : base(navigationService)
        {
            this.dialogService = dialogService;
            this.settings = settings;
            this.httpService = httpService;
            this.docService = docService;
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
                await GetDocsByClassifiers((string)parameters[UrlNavHelper.CLASSIFIER]);
            }
        }

        protected async Task GetDocsByClassifiers(string classifiers)
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

                var seacrhResult = await httpService.GetClassifier(searchModel);

                foreach (var docItem in seacrhResult.Data)
                {
                    var docTitle = new ExpandableLVModel() { Header = docItem.FullTitle };
                    var document = await docService.GetSmeDocByIdentifier(docItem.DocIdentifier, string.Empty, (int)this.settings.Language);
                    var docContent = new ItemExpandableLVModel(document.Items[0]?.Text);
                    Console.WriteLine();
                    docTitle.Items.Add(docContent);

                    ExpandableDocList.Add(new ExpandableLVViewModel(docTitle));
                }

                Console.WriteLine();
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
    }
}

