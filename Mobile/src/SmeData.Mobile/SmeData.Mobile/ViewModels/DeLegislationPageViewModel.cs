using Prism.Navigation;
using Prism.Services;
using SmeData.Mobile.Data;
using SmeData.Mobile.Helpers;
using SmeData.Mobile.Models.Settings;
using SmeData.Mobile.Services;

namespace SmeData.Mobile.ViewModels
{
    public class DeLegislationPageViewModel : CommonDocListViewPageViewModel
    {
        public DeLegislationPageViewModel(HttpService httpService, INavigationService navigationService, IPageDialogService dialogService, SettingsModel settings, AppRepository documentsRepository) : base(httpService, navigationService, dialogService, settings, documentsRepository)
        {
        }

        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            if (!(await ConnectivityHelper.CheckInternetConection(this.dialogService)))
            {
                return;
            }

            await GetEuDocsByClassifier("AC2A647D-CDF0-4BAE-8B26-AE63B95B0A43");
        }
    }
}
