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
    public class BgLegalPageViewModel : BaseViewModel
    {
        public ICommand GoToDecisionsCommand { get; set; }
        public ICommand GoToOpinionsCommand { get; set; }
        public ICommand GoToGuidelinesCommand { get; set; }
        public ICommand GoToSupremeAdministrativeCourtCommand { get; set; }
        public ICommand GoToAdministrativeCourtSofiaCommand { get; set; }
        public ICommand GoToOtherCourtsCommand { get; set; }

        public string ButtonsFont { get => $"t|18|{ScreenWidth}"; }


        public BgLegalPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            this.GoToDecisionsCommand = new DelegateCommand(this.ShowDecisions);
            this.GoToOpinionsCommand = new DelegateCommand(this.ShowOpinions);
            this.GoToGuidelinesCommand = new DelegateCommand(this.ShowGuidelines);
            this.GoToSupremeAdministrativeCourtCommand = new DelegateCommand(this.ShowSupremeAdministrativeCourt);
            this.GoToAdministrativeCourtSofiaCommand = new DelegateCommand(this.ShowAdministrativeCourtSofia);
            this.GoToOtherCourtsCommand = new DelegateCommand(this.ShowOtherCourts);
        }

        private void ShowDecisions()
        {
            navigationService.NavigateAsync($"NationalDecisionsPage?{UrlNavHelper.CLASSIFIER}=7E749A0F-64D8-460C-8269-9A3FBC5C2351;6136AA37-17D0-4BFD-A618-AB1D99F6C250");
        }

        private void ShowOpinions()
        {
            navigationService.NavigateAsync($"NationalOpinionsPage?{UrlNavHelper.CLASSIFIER}=7E749A0F-64D8-460C-8269-9A3FBC5C2351;8266D4E8-E768-4817-81D7-FFF925926E43");
        }

        private void ShowGuidelines()
        {
            navigationService.NavigateAsync($"NationalGuidelinesPage?{UrlNavHelper.CLASSIFIER}=7E749A0F-64D8-460C-8269-9A3FBC5C2351;9B122477-A600-488F-9A9E-3B780B6A0B50");
        }

        private void ShowSupremeAdministrativeCourt()
        {
            navigationService.NavigateAsync($"NationalSupremeAdministrativeCourtPage?{UrlNavHelper.CLASSIFIER}=7e749a0f-64d8-460c-8269-9a3fbc5c2351;fe8a24db-1e8a-49fd-9d80-bce857c6c944;E8C8803D-2C5A-434B-AC17-C07DE92B547F");
        }
        private void ShowAdministrativeCourtSofia()
        {
            navigationService.NavigateAsync($"NationalAdministrativeCourtSofiaPage?{UrlNavHelper.CLASSIFIER}=7e749a0f-64d8-460c-8269-9a3fbc5c2351;fe8a24db-1e8a-49fd-9d80-bce857c6c944;FAB8CE42-8136-415B-9880-AA783E9E135B");
        }

        private void ShowOtherCourts()
        {
            navigationService.NavigateAsync($"NationalOtherCourtsPage?{UrlNavHelper.CLASSIFIER}=7e749a0f-64d8-460c-8269-9a3fbc5c2351;fe8a24db-1e8a-49fd-9d80-bce857c6c944;EFD0367A-570B-4F8F-A5C0-2EFDC08F0BD4");
        }
    }
}
