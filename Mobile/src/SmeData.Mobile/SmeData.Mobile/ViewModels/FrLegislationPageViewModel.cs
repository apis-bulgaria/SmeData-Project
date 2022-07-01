using Prism.Navigation;
using Prism.Services;
using SmeData.Mobile.Data;
using SmeData.Mobile.Helpers;
using SmeData.Mobile.Models.Settings;
using SmeData.Mobile.Services;

namespace SmeData.Mobile.ViewModels
{
    public class FrLegislationPageViewModel : CommonDocListViewPageViewModel
    {
        public FrLegislationPageViewModel(HttpService httpService, INavigationService navigationService, IPageDialogService dialogService, SettingsModel settings, AppRepository documentsRepository) : base(httpService, navigationService, dialogService, settings, documentsRepository)
        {
        }

        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            if (!await ConnectivityHelper.CheckInternetConection(this.dialogService))
            {
                return;
            }

            await GetEuDocsByClassifier("F2338AFC-30BD-4270-89AA-FBC2653F1C0C");
        }
    }
}
