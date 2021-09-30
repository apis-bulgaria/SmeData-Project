using Prism.Commands;
using Prism.Navigation;
using SmeData.Mobile.Helpers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace SmeData.Mobile.ViewModels
{
    public class DeLegalPageViewModel : BaseViewModel
    {
        public ICommand GoToFederalConstitutionalCourtCommand { get; set; }
        public ICommand GoToFederalCourtOfJusticeCommand { get; set; }
        public ICommand GoToOpinionsCommand { get; set; }
        public ICommand GoToGuidelinesCommand { get; set; }

        public string ButtonsFont { get => $"t|18|{ScreenWidth}"; }


        public DeLegalPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            this.GoToFederalConstitutionalCourtCommand = new DelegateCommand(this.ShowFederalConstitutionalCourt);
            this.GoToFederalCourtOfJusticeCommand = new DelegateCommand(this.ShowFederalCourtOfJustice);
            this.GoToOpinionsCommand = new DelegateCommand(this.ShowOpinions);
            this.GoToGuidelinesCommand = new DelegateCommand(this.ShowGuidelines);
        }

        private void ShowFederalConstitutionalCourt()
        {
            navigationService.NavigateAsync($"NationalFederalConstitutionalCourtPage?{UrlNavHelper.CLASSIFIER}=C5B19F72-CAAB-4A10-AD8F-76E65D09D002, FE8A24DB-1E8A-49FD-9D80-BCE857C6C944;8F3BD2D7-3B85-4BCF-A484-54F83B718A23");
        }

        private void ShowFederalCourtOfJustice()
        {
            navigationService.NavigateAsync($"NationalFederalCourtOfJusticePage?{UrlNavHelper.CLASSIFIER}=C5B19F72-CAAB-4A10-AD8F-76E65D09D002, FE8A24DB-1E8A-49FD-9D80-BCE857C6C944;2AEDDE71-DA45-470C-B832-FFC9C587CAE4");
        }

        private void ShowOpinions()
        {
            navigationService.NavigateAsync($"NationalOpinionsPage?{UrlNavHelper.CLASSIFIER}=C5B19F72-CAAB-4A10-AD8F-76E65D09D002;8266D4E8-E768-4817-81D7-FFF925926E43");
        }

        private void ShowGuidelines()
        {
            navigationService.NavigateAsync($"NationalGuidelinesPage?{UrlNavHelper.CLASSIFIER}=C5B19F72-CAAB-4A10-AD8F-76E65D09D002;9B122477-A600-488F-9A9E-3B780B6A0B50");
        }
    }
}
