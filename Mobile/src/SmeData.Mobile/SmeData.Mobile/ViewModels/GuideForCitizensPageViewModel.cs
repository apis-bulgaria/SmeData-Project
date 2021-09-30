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
    /// Page with information tools for citizens 
    /// </summary>
    public class GuideForCitizensPageViewModel : BaseViewModel
    {
        public ICommand GdprMadeSimpleCommand { get; set; }
        public ICommand ContanctWithNationalDpasCommand { get; set; }
        public ICommand FagCommand { get; set; }

        public string ButtonsFont { get => $"t|18|{ScreenWidth}"; }

        public GuideForCitizensPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            this.GdprMadeSimpleCommand = new DelegateCommand(this.ShowGdprMadeSimplePage);
            this.ContanctWithNationalDpasCommand = new DelegateCommand(this.ShowContanctWithNationalDpasPage);
            this.FagCommand = new DelegateCommand(this.ShowFagPage);
        }

        private void ShowGdprMadeSimplePage()
        {
            this.navigationService.NavigateAsync($"GdpMadeSimplePage?{UrlNavHelper.DOC_NUM}={"64397F33F39989D4F7BB5668A9540570"}");
        }

        private void ShowContanctWithNationalDpasPage()
        {
            this.navigationService.NavigateAsync("ContactWithNationalDPSsPage");
        }

        private void ShowFagPage()
        {
            this.navigationService.NavigateAsync($"FaqPage?{UrlNavHelper.CLASSIFIER}=355BD1E1-6ADA-4689-A8BE-7BF37F75AB98");
        }
    }
}
