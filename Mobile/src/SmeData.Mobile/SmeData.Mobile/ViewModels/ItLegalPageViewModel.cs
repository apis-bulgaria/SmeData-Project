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
    /// Tabbed page for italian lagislation, case law and guidelines documents related to GDPR
    /// </summary>
    public class ItLegalPageViewModel : BaseViewModel
    {
        public ICommand GoToDecisionsCommand { get; set; }
        public ICommand GoToOpinionsCommand { get; set; }
        public ICommand GoToGuidelinesCommand { get; set; }
        public ICommand GoToSupremeCourtOfCassationCommand { get; set; }
        public ICommand GoToConstitutionalCourtCommand { get; set; }
        public ICommand GoToOtherCourtsCommand { get; set; }

        public string ButtonsFont { get => $"t|18|{ScreenWidth}"; }


        public ItLegalPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            this.GoToDecisionsCommand = new DelegateCommand(this.ShowDecisions);
            this.GoToOpinionsCommand = new DelegateCommand(this.ShowOpinions);
            this.GoToGuidelinesCommand = new DelegateCommand(this.ShowGuidelines);
            this.GoToSupremeCourtOfCassationCommand = new DelegateCommand(this.ShowSupremeCourtOfCassation);
            this.GoToConstitutionalCourtCommand = new DelegateCommand(this.ShowConstitutionalCourt);
            this.GoToOtherCourtsCommand = new DelegateCommand(this.ShowOtherCourts);
        }

        private void ShowDecisions()
        {
            navigationService.NavigateAsync($"NationalDecisionsPage?{UrlNavHelper.CLASSIFIER}=44F51AE2-0A61-486A-B821-6E4502F81F6C;6136AA37-17D0-4BFD-A618-AB1D99F6C250");
        }

        private void ShowOpinions()
        {
            navigationService.NavigateAsync($"NationalOpinionsPage?{UrlNavHelper.CLASSIFIER}=44F51AE2-0A61-486A-B821-6E4502F81F6C;8266D4E8-E768-4817-81D7-FFF925926E43");
        }

        private void ShowGuidelines()
        {
            navigationService.NavigateAsync($"NationalGuidelinesPage?{UrlNavHelper.CLASSIFIER}=44F51AE2-0A61-486A-B821-6E4502F81F6C;9B122477-A600-488F-9A9E-3B780B6A0B50");
        }

        private void ShowSupremeCourtOfCassation()
        {
            navigationService.NavigateAsync($"NationalSupremeCourtOfCassationPage?{UrlNavHelper.CLASSIFIER}=44F51AE2-0A61-486A-B821-6E4502F81F6C;fe8a24db-1e8a-49fd-9d80-bce857c6c944;5E52844E-496C-4FA6-AB5D-9E69A2193A6E");
        }

        private void ShowConstitutionalCourt()
        {
            navigationService.NavigateAsync($"NationalConstitutionalCourtPage?{UrlNavHelper.CLASSIFIER}=44F51AE2-0A61-486A-B821-6E4502F81F6C;fe8a24db-1e8a-49fd-9d80-bce857c6c944;C96D1805-7D30-4542-99CF-570F6BD77475");
        }

        private void ShowOtherCourts()
        {
            navigationService.NavigateAsync($"NationalOtherCourtsPage?{UrlNavHelper.CLASSIFIER}=44F51AE2-0A61-486A-B821-6E4502F81F6C;fe8a24db-1e8a-49fd-9d80-bce857c6c944;88C45BE3-8A6C-4596-9393-48B4ADEB2BD9");
        }
    }
}
