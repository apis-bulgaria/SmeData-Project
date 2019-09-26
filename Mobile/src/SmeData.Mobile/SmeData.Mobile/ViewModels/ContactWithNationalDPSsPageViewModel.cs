using MvvmHelpers;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using SmeData.Mobile.Models.ExpandedListView;
using SmeData.Mobile.Models.ExpandedListViewForContactsWirhDPAs;
using SmeData.Mobile.Models.Settings;
using SmeData.Mobile.Services;
using SmeData.Mobile.ViewModels.ExpandableListViewForContactsWithDPAs;
using SmeData.SharedModels.ContactsDPAs;
using SmeData.SharedModels.Language;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace SmeData.Mobile.ViewModels
{
    public class ContactWithNationalDPSsPageViewModel : BaseViewModel
    {
        public ObservableRangeCollection<ExpandableLVFocContactsWithDPAsViewModel> ExpandedList { get; private set; } = new ObservableRangeCollection<ExpandableLVFocContactsWithDPAsViewModel>();

        public ICommand HeaderClickCommand { get; private set; }
        public ICommand CallNumberCommand { get; private set; }
        public ICommand OpenEmailClientCommand { get; private set; }
        public ICommand OpenUrlCommand { get; private set; }

        public string ContactInfoFont { get => $"t|15|{ScreenWidth}"; }

        public string ContactHeaderFont { get => $"t|18|{ScreenWidth}"; }

        private readonly HttpService httpService;

        public ContactWithNationalDPSsPageViewModel(HttpService httpService, INavigationService navigationService, SettingsModel settings) : base(navigationService)
        {
            this.settings = settings;
            this.httpService = httpService;
            this.HeaderClickCommand = new DelegateCommand<ExpandableLVFocContactsWithDPAsViewModel>(this.DoExtendContent);
            this.CallNumberCommand = new DelegateCommand<string>(this.DoCallNumber);
            this.OpenEmailClientCommand = new DelegateCommand<string>(this.DoOpenEmailClient);
            this.OpenUrlCommand = new DelegateCommand<string>(this.DoOpenUrl);
            LoadDpasData();
        }

        private async Task LoadDpasData()
        {
            var contactDpaItems = await httpService.GetContactsDPAs();

            bool isFirst = true;

            foreach (var item in contactDpaItems)
            {
                ExpandableLVForContactsWithDPAsModel expandableItem = null;

                switch (this.settings.Language)
                {
                    case SmeLanguage.Bulgarian:
                        expandableItem = new ExpandableLVForContactsWithDPAsModel() { Header = item.CountryBgLang };
                        break;
                    case SmeLanguage.Italian:
                        expandableItem = new ExpandableLVForContactsWithDPAsModel() { Header = item.CountryItLang };
                        break;
                    case SmeLanguage.English:
                    default:
                        expandableItem = new ExpandableLVForContactsWithDPAsModel() { Header = item.CountryEnLang };
                        break;
                }

                var expandableContent = new ItemExpandableLVForContactsWithDPAsModel(item);

                expandableItem.Items.Add(expandableContent);

                ExpandedList.Add(new ExpandableLVFocContactsWithDPAsViewModel(expandableItem, isFirst));

                if (isFirst)
                {
                    isFirst = false;
                }
            }
        }

        private void DoExtendContent(ExpandableLVFocContactsWithDPAsViewModel item)
        {
            item.Expanded = !item.Expanded;
        }

        private void DoCallNumber(string telNumber)
        {
            Device.OpenUri(new Uri($"tel:{telNumber}"));
        }

        private void DoOpenEmailClient(string emailAddress)
        {
            Device.OpenUri(new Uri($"mailto:{emailAddress}"));
        }

        private void DoOpenUrl(string url)
        {
            Device.OpenUri(new Uri(url));
        }
    }
}
