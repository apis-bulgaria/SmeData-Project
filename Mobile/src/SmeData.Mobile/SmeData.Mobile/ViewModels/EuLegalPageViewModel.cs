using Newtonsoft.Json;
using Prism.Commands;
using Prism.Navigation;
using SmeData.Mobile.Models.Settings;
using SmeData.Mobile.Services;
using SmeData.WebApi.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace SmeData.Mobile.ViewModels
{
    /// <summary>
    /// Tabbed page for european lagislation, case law and guidelines documents related to GDPR
    /// </summary>
    public class EuLegalPageViewModel: BaseViewModel
    {
        public EuLegalPageViewModel(INavigationService navigationService) : base(navigationService)
        {
        }
    }
}
