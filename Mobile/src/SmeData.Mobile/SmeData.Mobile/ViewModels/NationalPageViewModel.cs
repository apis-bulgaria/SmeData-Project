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
    public class NationalPageViewModel : BaseViewModel
    {
        public ICommand GoToBulgarianNationalDocsCommand { get; set; }
        public ICommand GoToItalianNationalDocsCommand { get; set; }

        public string ButtonsFont { get => $"t|18|{ScreenWidth}"; }

        public NationalPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            this.GoToBulgarianNationalDocsCommand = new DelegateCommand(this.ShowBulgarianNationalDocs);
            this.GoToItalianNationalDocsCommand = new DelegateCommand(this.ShowItalianNationalDocs);
        }

        private void ShowBulgarianNationalDocs()
        {
            navigationService.NavigateAsync("BgLegalPage");
        }

        private void ShowItalianNationalDocs()
        {
            navigationService.NavigateAsync("ItLegalPage");
        }
    }
}
