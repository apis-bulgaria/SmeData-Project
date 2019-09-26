using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;
using SmeData.Mobile.Models.Settings;
using SmeData.Mobile.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

namespace SmeData.Mobile.ViewModels
{
    public class NationalGuidelinesPageViewModel : CommonDocListViewPageViewModel
    {
        public NationalGuidelinesPageViewModel(HttpService httpService, INavigationService navigationService, IPageDialogService dialogService, SettingsModel settings) : base(httpService, navigationService, dialogService, settings)
        {
        }
    }
}
