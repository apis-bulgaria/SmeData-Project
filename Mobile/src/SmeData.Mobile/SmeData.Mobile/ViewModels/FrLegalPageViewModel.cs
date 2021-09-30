using Prism.Commands;
using Prism.Navigation;
using SmeData.Mobile.Helpers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace SmeData.Mobile.ViewModels
{
    public class FrLegalPageViewModel : BaseViewModel
    {
        public ICommand GoToCNILCommand { get; set; }
        public ICommand GoToCouncilOfStateCommand { get; set; }
        public ICommand GoToCourtOfCassationCommand { get; set; }
        public ICommand GoToOpinionsCommand { get; set; }
        public ICommand GoToGuidelinesCommand { get; set; }

        public string ButtonsFont { get => $"t|18|{ScreenWidth}"; }


        public FrLegalPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            this.GoToCNILCommand = new DelegateCommand(this.ShowCNIL);
            this.GoToCouncilOfStateCommand = new DelegateCommand(this.ShowCouncilOfState);
            this.GoToCourtOfCassationCommand = new DelegateCommand(this.ShowCourtOfCassation);
            this.GoToOpinionsCommand = new DelegateCommand(this.ShowOpinions);
            this.GoToGuidelinesCommand = new DelegateCommand(this.ShowGuidelines);
        }

        private void ShowCNIL()
        {
            navigationService.NavigateAsync($"NationalCNILPage?{UrlNavHelper.CLASSIFIER}=3E5F20FB-17FE-4234-B5B3-4D5E9F9F7966, FE8A24DB-1E8A-49FD-9D80-BCE857C6C944;9B59E6E3-2666-48DD-9686-386895F0B3D1");
        }

        private void ShowCouncilOfState()
        {
            navigationService.NavigateAsync($"NationalCouncilOfStatePage?{UrlNavHelper.CLASSIFIER}=3E5F20FB-17FE-4234-B5B3-4D5E9F9F7966, FE8A24DB-1E8A-49FD-9D80-BCE857C6C944;C2284EC3-E2AB-4BC4-8BCB-901114E231B4");
        }
        private void ShowCourtOfCassation()
        {
            navigationService.NavigateAsync($"NationalCourtOfCassationPage?{UrlNavHelper.CLASSIFIER}=3E5F20FB-17FE-4234-B5B3-4D5E9F9F7966, FE8A24DB-1E8A-49FD-9D80-BCE857C6C944;01818A2E-AB43-4152-9346-3B91CF0CF701");
        }
        private void ShowOpinions()
        {
            navigationService.NavigateAsync($"NationalOpinionsPage?{UrlNavHelper.CLASSIFIER}=3E5F20FB-17FE-4234-B5B3-4D5E9F9F7966;8266D4E8-E768-4817-81D7-FFF925926E43");
        }

        private void ShowGuidelines()
        {
            navigationService.NavigateAsync($"NationalGuidelinesPage?{UrlNavHelper.CLASSIFIER}=3E5F20FB-17FE-4234-B5B3-4D5E9F9F7966;9B122477-A600-488F-9A9E-3B780B6A0B50");
        }
    }
}
