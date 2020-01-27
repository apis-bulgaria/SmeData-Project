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
    /// Page with case law document, related to GDPR, form Bulgaria and Italy
    /// </summary>
    public class NationalCaseLawPageViewModel : BaseViewModel
    {
        public ICommand GoToSupremeAdministrativeCourtCommand { get; set; }
        public ICommand GoToOtherCourtsCommand { get; set; }

        public string ButtonsFont { get => $"t|18|{ScreenWidth}"; }

        public NationalCaseLawPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            this.GoToSupremeAdministrativeCourtCommand = new DelegateCommand(this.ShowSupremeAdministrativeCourt);
            this.GoToOtherCourtsCommand = new DelegateCommand(this.ShowOtherCourts);
        }

        private void ShowSupremeAdministrativeCourt()
        {
            //navigationService.NavigateAsync($"GuidelinesPage?{UrlNavHelper.CLASSIFIER}=0319DD51-EE78-4F0C-B948-E8F8BD0FE0E1 ");
        }

        private void ShowOtherCourts()
        {
            //navigationService.NavigateAsync($"GuidelinesPage?{UrlNavHelper.CLASSIFIER}=9C6843AC-B58A-4F08-89E8-C0A6C30B18C9");
        }
    }
}
