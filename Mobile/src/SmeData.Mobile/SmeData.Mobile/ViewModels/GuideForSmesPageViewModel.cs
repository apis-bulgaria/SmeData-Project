using Prism.Commands;
using Prism.Navigation;
using SmeData.Mobile.Helpers;
using SmeData.Mobile.Models.Settings;
using SmeData.Mobile.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace SmeData.Mobile.ViewModels
{
    /// <summary>
    /// Page with information tools for small and medium enterprises (SMEs)
    /// </summary>
    public class GuideForSmesPageViewModel : BaseViewModel
    {
        public ICommand GdprMadeSimpleCommand { get; set; }
        public ICommand DecisionSuportCommand { get; set; }
        public ICommand ContanctWithNationalDpasCommand { get; set; }
        public ICommand FagCommand { get; set; }

        public string ButtonsFont { get => $"t|18|{ScreenWidth}"; }

        public GuideForSmesPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            this.GdprMadeSimpleCommand = new DelegateCommand(this.ShowGdprMadeSimplePage);
            this.DecisionSuportCommand = new DelegateCommand(this.ShowDspPage);
            this.ContanctWithNationalDpasCommand = new DelegateCommand(this.ShowContanctWithNationalDpasPage);
            this.FagCommand = new DelegateCommand(this.ShowFagPage);
        }

        private void ShowGdprMadeSimplePage()
        {
            this.navigationService.NavigateAsync($"GdpMadeSimplePage?{UrlNavHelper.DOC_NUM}={"712BDAC3C2CB0BCA51255B24FC39AD08"}");
        }

        private void ShowDspPage()
        {
            this.navigationService.NavigateAsync($"DecisionSupportPage?{UrlNavHelper.CLASSIFIER}=AFEE5C1D-690D-4A2E-B420-0B0FD4F081BC");
        }

        private void ShowContanctWithNationalDpasPage()
        {
            this.navigationService.NavigateAsync("ContactWithNationalDPSsPage");
        }

        private void ShowFagPage()
        {
            this.navigationService.NavigateAsync($"FaqPage?{UrlNavHelper.CLASSIFIER}=0BD47035-E4CF-401D-AD46-C3D664DE0CF6");
        }
    }
}
