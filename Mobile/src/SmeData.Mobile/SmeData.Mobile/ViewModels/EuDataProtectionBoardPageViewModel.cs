using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;
using SmeData.Mobile.Models.Settings;
using SmeData.Mobile.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmeData.Mobile.ViewModels
{
    /// <summary>
    /// Page for showing list of legal documents from EU Data Protecion Board
    /// </summary>
    public class EuDataProtectionBoardPageViewModel : CommonLegislationPageViewModel
    {
        public EuDataProtectionBoardPageViewModel(HttpService service, INavigationService navigationService, IPageDialogService dialogService, SettingsModel settings) : base(service, navigationService, dialogService, settings)
        {
        }

        protected override string UrlAction => "eudateprotectionboardcategories";
    }
}
