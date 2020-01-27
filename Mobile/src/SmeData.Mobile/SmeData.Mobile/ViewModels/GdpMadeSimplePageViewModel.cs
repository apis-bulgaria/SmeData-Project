using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using SmeData.Mobile.Data;
using SmeData.Mobile.Helpers;
using SmeData.Mobile.Models.Settings;
using SmeData.Mobile.Services;
using Xamarin.Essentials;

namespace SmeData.Mobile.ViewModels
{
    /// <summary>
    /// Page with GDPR presented in simple way 
    /// </summary>
    public class GdpMadeSimplePageViewModel : BaseShowPageViewModel
    {
        public GdpMadeSimplePageViewModel(INavigationService navigationService, IPageDialogService dialogService, DocumentService docService, SettingsModel settings, HttpService httpService, AppRepository documentsRepository) : base(navigationService, dialogService, docService, settings, httpService, documentsRepository)
        {
        }
    }
}
