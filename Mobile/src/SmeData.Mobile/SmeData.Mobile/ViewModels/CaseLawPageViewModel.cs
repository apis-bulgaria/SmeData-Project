using Prism.Navigation;
using Prism.Services;
using SmeData.Mobile.Models.Settings;
using SmeData.Mobile.Services;
using System.Threading.Tasks;

namespace SmeData.Mobile.ViewModels
{
    /// <summary>
    /// Page with list case law documents
    /// </summary>
    public class CaseLawPageViewModel : CommonLegislationPageViewModel
    {
        public CaseLawPageViewModel(HttpService service, INavigationService navigationService, IPageDialogService dialogService, SettingsModel settings) : base(service, navigationService, dialogService, settings)
        {
            Task.Run(() => GetAllLegislationDocs());
        }

        protected override string UrlAction => "eucaselawcategories";
    }
}
