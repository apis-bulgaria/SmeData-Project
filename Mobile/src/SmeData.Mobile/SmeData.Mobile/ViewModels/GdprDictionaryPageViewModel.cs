using MvvmHelpers;
using Newtonsoft.Json;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;
using SmeData.Mobile.Data;
using SmeData.Mobile.Helpers;
using SmeData.Mobile.Models.ExpandedListView;
using SmeData.Mobile.Models.Settings;
using SmeData.Mobile.Services;
using SmeData.Mobile.ViewModels.ExpandedListView;
using SmeData.Shared.Common;
using SmeData.WebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;

namespace SmeData.Mobile.ViewModels
{
    /// <summary>
    /// Page with terms and explanations ralated to GDPR
    /// </summary>
    public class GdprDictionaryPageViewModel : BaseViewModel
    {
        public ObservableRangeCollection<ExpandableLVViewModel> DictionaryList { get; private set; } = new ObservableRangeCollection<ExpandableLVViewModel>();

        private readonly HttpService httpService;
        private readonly IPageDialogService dialogService;
        private readonly AppRepository documentsRepository;
        private bool isLoading;

        public GdprDictionaryPageViewModel(INavigationService navigationService, HttpService httpService, IPageDialogService dialogService, SettingsModel settings, AppRepository documentsRepository) : base(navigationService)
        {
            this.httpService = httpService;
            this.dialogService = dialogService;
            this.settings = settings;
            this.documentsRepository = documentsRepository;
            LoadData();
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

        private async Task LoadData()
        {
            IsLoading = true;
            try
            {
                List<GdprDictionaryResponseModel> dictionaryItems = null;

                try
                {
                    switch (this.settings.Language)
                    {
                        case SharedModels.Language.SmeLanguage.Bulgarian:
                            dictionaryItems = JsonConvert.DeserializeObject<List<GdprDictionaryResponseModel>>(Compression.DecompressString((await this.documentsRepository.GetDocumentAsync("b492d8b4-1ea0-46f6-9451-41bddfe76462")).JsonSmeDoc));
                            break;
                        case SharedModels.Language.SmeLanguage.English:
                            dictionaryItems = JsonConvert.DeserializeObject<List<GdprDictionaryResponseModel>>(Compression.DecompressString((await this.documentsRepository.GetDocumentAsync("62e915c1-5bd8-464d-8a57-c2a3a212305b")).JsonSmeDoc));
                            break;
                        case SharedModels.Language.SmeLanguage.Italian:
                            dictionaryItems = JsonConvert.DeserializeObject<List<GdprDictionaryResponseModel>>(Compression.DecompressString((await this.documentsRepository.GetDocumentAsync("e40ed442-64cf-4947-a220-6108d9fdffb4")).JsonSmeDoc));
                            break;
                        case SharedModels.Language.SmeLanguage.German:
                            dictionaryItems = JsonConvert.DeserializeObject<List<GdprDictionaryResponseModel>>(Compression.DecompressString((await this.documentsRepository.GetDocumentAsync("c4be0d79-4079-48d1-aee3-6a30c7b8a91a")).JsonSmeDoc));
                            break;
                        case SharedModels.Language.SmeLanguage.French:
                            dictionaryItems = JsonConvert.DeserializeObject<List<GdprDictionaryResponseModel>>(Compression.DecompressString((await this.documentsRepository.GetDocumentAsync("ff8482c2-4a9d-4a8c-b4bb-c062e0c4d441")).JsonSmeDoc));
                            break;
                        default:
                            break;
                    }
                }
                catch (Exception e) { }

                if (dictionaryItems == null)
                {
                    if (!(await ConnectivityHelper.CheckInternetConection(this.dialogService)))
                    {
                        return;
                    }

                    dictionaryItems = await httpService.GetGdprDictionaryByLangId(((int)this.settings.Language).ToString());
                }

                dictionaryItems = dictionaryItems.OrderBy(x => x.Title).ToList();

                foreach (var item in dictionaryItems)
                {
                    var expandableItem = new ExpandableLVModel() { Header = item.Title };
                    var expandableContent = new ItemExpandableLVModel(item.Description);
                    expandableItem.Items.Add(expandableContent);

                    DictionaryList.Add(new ExpandableLVViewModel(expandableItem));
                }
            }
            finally
            {
                IsLoading = false;
            }
        }
    }
}
