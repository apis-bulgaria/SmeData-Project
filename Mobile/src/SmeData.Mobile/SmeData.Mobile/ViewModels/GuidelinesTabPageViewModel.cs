using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using SmeData.Mobile.Helpers;
using SmeData.Mobile.Models.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

namespace SmeData.Mobile.ViewModels
{
    /// <summary>
    /// Page with list of guidelines 
    /// </summary>
    public class GuidelinesTabPageViewModel : BaseViewModel
    {
        public ICommand GoToProtectionBoardDocsCommand { get; set; }
        public ICommand GoToProtectionSupervisorDocsCommand { get; set; }

        public string ButtonsFont { get => $"t|18|{ScreenWidth}"; }

        public GuidelinesTabPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            this.GoToProtectionBoardDocsCommand = new DelegateCommand(this.ShowProtectionBoardDocs);
            this.GoToProtectionSupervisorDocsCommand = new DelegateCommand(this.ShowProtectionSupervisorDocs);
        }

        private void ShowProtectionBoardDocs()
        {
            navigationService.NavigateAsync($"EuDataProtectionBoardPage");
        }

        private void ShowProtectionSupervisorDocs()
        {
            navigationService.NavigateAsync($"EuDataProtectionSupervisorPage?{UrlNavHelper.CLASSIFIER}=9C6843AC-B58A-4F08-89E8-C0A6C30B18C9");
        }
    }
}
