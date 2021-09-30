using Prism.Navigation;
using Prism.Services;
using SmeData.Mobile.Data;
using SmeData.Mobile.Helpers;
using SmeData.Mobile.Models.Settings;
using SmeData.Mobile.Services;
using SmeData.WebApi.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace SmeData.Mobile.ViewModels
{
    /// <summary>
    /// Page with list of italian legislation documents
    /// </summary>
    public class ItLegislationPageViewModel : CommonDocListViewPageViewModel
    {
        public ItLegislationPageViewModel(HttpService httpService, INavigationService navigationService, IPageDialogService dialogService, SettingsModel settings, AppRepository documentsRepository) : base(httpService, navigationService, dialogService, settings, documentsRepository)
        {
        }

        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            if (!(await ConnectivityHelper.CheckInternetConection(this.dialogService)))
            {
                return;
            }

            await GetEuDocsByClassifier("57B81A94-F630-4D12-A062-1882AF4DF437;938AD68B-0057-4B28-AEA1-99EC41764C13");
        }
    }
}

