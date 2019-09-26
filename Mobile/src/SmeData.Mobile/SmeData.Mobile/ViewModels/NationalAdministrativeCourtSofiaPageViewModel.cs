using Prism.Navigation;
using Prism.Services;
using SmeData.Mobile.Models.Settings;
using SmeData.Mobile.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmeData.Mobile.ViewModels
{
    public class NationalAdministrativeCourtSofiaPageViewModel : CommonDocListViewPageViewModel
    {
        public NationalAdministrativeCourtSofiaPageViewModel(HttpService httpService, INavigationService navigationService, IPageDialogService dialogService, SettingsModel settings) : base(httpService, navigationService, dialogService, settings)
        {
        }
    }
}
