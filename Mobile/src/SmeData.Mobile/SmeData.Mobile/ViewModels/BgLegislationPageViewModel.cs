using Prism.Navigation;
using Prism.Services;
using SmeData.Mobile.Models.Grouping;
using SmeData.Mobile.Models.Settings;
using SmeData.Mobile.Services;
using SmeData.WebApi.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace SmeData.Mobile.ViewModels
{
    public class BgLegislationPageViewModel : CommonLegislationPageViewModel
    {
        public BgLegislationPageViewModel(HttpService service, INavigationService navigationService, IPageDialogService dialogService, SettingsModel settings) : base(service, navigationService, dialogService, settings)
        {
        }

        protected override string UrlAction => "bgleg";
    }
}
