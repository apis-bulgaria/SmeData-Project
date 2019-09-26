using Prism.Navigation;
using Prism.Services;
using SmeData.Mobile.Models.Settings;
using SmeData.Mobile.Services;
using System.Threading.Tasks;

namespace SmeData.Mobile.ViewModels
{
    //public class CaseLawPageViewModel : CommonDocListViewPageViewModel
    //{
    //    public CaseLawPageViewModel(HttpService httpService, INavigationService navigationService, IPageDialogService dialogService, SettingsModel settings) : base(httpService, navigationService, dialogService, settings)
    //    {
    //        GetEuDocsByClassifier("0C6CC932-1EE3-4F9B-A1FD-B35B68F61578");
    //    }
    //}

    public class CaseLawPageViewModel : CommonLegislationPageViewModel
    {
        public CaseLawPageViewModel(HttpService service, INavigationService navigationService, IPageDialogService dialogService, SettingsModel settings) : base(service, navigationService, dialogService, settings)
        {
            Task.Run(() => GetAllLegislationDocs());
        }

        protected override string UrlAction => "eucaselawcategories";
    }
}
