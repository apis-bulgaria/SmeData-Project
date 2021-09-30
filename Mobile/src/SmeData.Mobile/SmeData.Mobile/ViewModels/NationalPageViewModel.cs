using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using SmeData.Mobile.Models.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

namespace SmeData.Mobile.ViewModels
{
    /// <summary>
    /// Page with buttons for selection of bulgarian ot italian national legal documents ralated to GDPR
    /// </summary>
    public class NationalPageViewModel : BaseViewModel
    {
        public ICommand GoToBulgarianNationalDocsCommand { get; set; }
        public ICommand GoToItalianNationalDocsCommand { get; set; }
        public ICommand GoToGermanNationalDocsCommand { get; set; }
        public ICommand GoToFranceNationalDocsCommand { get; set; }

        public string ButtonsFont { get => $"t|18|{ScreenWidth}"; }

        public NationalPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            this.GoToBulgarianNationalDocsCommand = new DelegateCommand(this.ShowBulgarianNationalDocs);
            this.GoToItalianNationalDocsCommand = new DelegateCommand(this.ShowItalianNationalDocs);
            this.GoToGermanNationalDocsCommand = new DelegateCommand(this.ShowGermanNationalDocs);
            this.GoToFranceNationalDocsCommand = new DelegateCommand(this.ShowFranceNationalDocs);
        }

        private void ShowBulgarianNationalDocs()
        {
            navigationService.NavigateAsync("BgLegalPage");
        }

        private void ShowItalianNationalDocs()
        {
            navigationService.NavigateAsync("ItLegalPage");
        }

        private void ShowGermanNationalDocs()
        {
            navigationService.NavigateAsync("DeLegalPage");
        }

        private void ShowFranceNationalDocs()
        {
            navigationService.NavigateAsync("FrLegalPage");
        }
    }
}
