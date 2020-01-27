using Prism.Commands;
using Prism.Navigation;
using SmeData.Mobile.Helpers;
using SmeData.Mobile.Models.Settings;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace SmeData.Mobile.ViewModels
{
    /// <summary>
    /// Page with buttons for navigation to European Union legal framework page, National legal framework page and International legal framework page
    /// </summary>
    public class LegalFrameworkPageViewModel : BaseViewModel
    {
        public ICommand EuLegalFrameworkCommand { get; set; }
        public ICommand NationalLegalFrameworkCommand { get; set; }
        public ICommand InternationalLegalFrameworkCommand { get; set; }

        public string ButtonsFont { get => $"t|18|{ScreenWidth}"; }

        public LegalFrameworkPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            this.EuLegalFrameworkCommand = new DelegateCommand(this.ShowEuLegalFramework);
            this.NationalLegalFrameworkCommand = new DelegateCommand(this.ShowNational);
            this.InternationalLegalFrameworkCommand = new DelegateCommand(this.ShowInternationalFramework);
        }

        private void ShowNational()
        {
            navigationService.NavigateAsync("NationalPage");
        }

        private void ShowEuLegalFramework()
        {
            navigationService.NavigateAsync("EuLegalPage");
        }

        private void ShowInternationalFramework()
        {
            navigationService.NavigateAsync("InternationalPage");
        }
    }

}
