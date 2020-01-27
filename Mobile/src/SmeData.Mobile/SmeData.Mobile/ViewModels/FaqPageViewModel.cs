using MvvmHelpers;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;
using SmeData.Mobile.Data;
using SmeData.Mobile.Models;
using SmeData.Mobile.Models.ExpandedListView;
using SmeData.Mobile.Models.Settings;
using SmeData.Mobile.Services;
using SmeData.Mobile.ViewModels.ExpandedListView;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace SmeData.Mobile.ViewModels
{
    /// <summary>
    /// Page with frequently asked questions ralated to GDPR 
    /// </summary>
    public class FaqPageViewModel : CommonDocListViewPageViewModel
    {
        public FaqPageViewModel(HttpService httpService, INavigationService navigationService, IPageDialogService dialogService, SettingsModel settings, AppRepository documentsRepository) : base(httpService, navigationService, dialogService, settings, documentsRepository)
        {
        }
    }
}
