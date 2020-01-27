using Prism.Navigation;
using Prism.Services;
using SmeData.Mobile.Data;
using SmeData.Mobile.Models.Settings;
using SmeData.Mobile.Services;

namespace SmeData.Mobile.ViewModels
{
    /// <summary>
    /// Page for showing guidelines
    /// </summary>
    public class GuidelinesPageViewModel : CommonDocListViewPageViewModel
    {
        public GuidelinesPageViewModel(HttpService service, INavigationService navigationService, IPageDialogService dialogService, SettingsModel settings, AppRepository documentsRepository) : base(service, navigationService, dialogService, settings, documentsRepository)
        {
        }
    }
}
