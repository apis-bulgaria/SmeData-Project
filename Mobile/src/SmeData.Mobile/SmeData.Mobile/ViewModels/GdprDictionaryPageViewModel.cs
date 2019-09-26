using MvvmHelpers;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;
using SmeData.Mobile.Helpers;
using SmeData.Mobile.Models.ExpandedListView;
using SmeData.Mobile.Models.Settings;
using SmeData.Mobile.Services;
using SmeData.Mobile.ViewModels.ExpandedListView;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;

namespace SmeData.Mobile.ViewModels
{
    public class GdprDictionaryPageViewModel : BaseViewModel
    {
        public ObservableRangeCollection<ExpandableLVViewModel> DictionaryList { get; private set; } = new ObservableRangeCollection<ExpandableLVViewModel>();

        private readonly HttpService httpService;
        private readonly IPageDialogService dialogService;
        private bool isLoading;

        public GdprDictionaryPageViewModel(INavigationService navigationService, HttpService httpService, IPageDialogService dialogService, SettingsModel settings) : base(navigationService)
        {
            this.httpService = httpService;
            this.dialogService = dialogService;
            this.settings = settings;
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
                if (!(await ConnectivityHelper.CheckInternetConection(this.dialogService)))
                {
                    return;
                }

                var dictionaryItems = await httpService.GetGdprDictionaryByLangId(((int)this.settings.Language).ToString());

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
