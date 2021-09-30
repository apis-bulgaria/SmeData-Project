using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using SmeData.Mobile.Helpers;
using SmeData.Mobile.Models.Settings;
using SmeData.Mobile.Services;
using SmeData.WebApi.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;

namespace SmeData.Mobile.ViewModels
{
    /// <summary>
    /// Page with list of legislation documents
    /// </summary>
    public class LegislationPageViewModel : CommonLegislationPageViewModel
    {
        public LegislationPageViewModel(HttpService service, INavigationService navigationService, IPageDialogService dialogService, SettingsModel settings) : base(service, navigationService, dialogService, settings)
        {
        }

        protected override string UrlAction => "euleg";
    }
}
