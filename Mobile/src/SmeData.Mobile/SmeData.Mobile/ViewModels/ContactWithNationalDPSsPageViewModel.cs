using MvvmHelpers;
using Newtonsoft.Json;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;
using SmeData.Mobile.Data;
using SmeData.Mobile.Helpers;
using SmeData.Mobile.Models.ExpandedListView;
using SmeData.Mobile.Models.ExpandedListViewForContactsWirhDPAs;
using SmeData.Mobile.Models.Settings;
using SmeData.Mobile.Services;
using SmeData.Mobile.ViewModels.ExpandableListViewForContactsWithDPAs;
using SmeData.Shared.Common;
using SmeData.SharedModels.ContactsDPAs;
using SmeData.SharedModels.Language;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace SmeData.Mobile.ViewModels
{
    /// <summary>
    /// Page for showing information abouth contacts with national DPSs
    /// </summary>
    public class ContactWithNationalDPSsPageViewModel : BaseViewModel
    {
        private readonly IPageDialogService dialogService;
        protected readonly AppRepository documentsRepository;

        /// <summary>
        /// Observable expandable collection cointaining DPA contact informations for european countries 
        /// </summary>
        public ObservableRangeCollection<ExpandableLVFocContactsWithDPAsViewModel> ExpandedList { get; private set; } = new ObservableRangeCollection<ExpandableLVFocContactsWithDPAsViewModel>();

        public ICommand HeaderClickCommand { get; private set; }
        public ICommand CallNumberCommand { get; private set; }
        public ICommand OpenEmailClientCommand { get; private set; }
        public ICommand OpenUrlCommand { get; private set; }

        /// <summary>
        /// Information for font size calculation for contacts information
        /// </summary>
        public string ContactInfoFont { get => $"t|15|{ScreenWidth}"; }

        /// <summary>
        /// Information for font size calculation for contacts heading
        /// </summary>
        public string ContactHeaderFont { get => $"t|18|{ScreenWidth}"; }

        private readonly HttpService httpService;

        public ContactWithNationalDPSsPageViewModel(HttpService httpService, INavigationService navigationService, SettingsModel settings, IPageDialogService dialogService, AppRepository documentsRepository) : base(navigationService)
        {
            this.settings = settings;
            this.httpService = httpService;
            this.dialogService = dialogService;
            this.documentsRepository = documentsRepository;
            this.HeaderClickCommand = new DelegateCommand<ExpandableLVFocContactsWithDPAsViewModel>(this.DoExtendContent);
            this.CallNumberCommand = new DelegateCommand<string>(this.DoCallNumber);
            this.OpenEmailClientCommand = new DelegateCommand<string>(this.DoOpenEmailClient);
            this.OpenUrlCommand = new DelegateCommand<string>(this.DoOpenUrl);
            LoadDpasData();
        }

        /// <summary>
        /// Method for filling LegislationDocs collection
        /// </summary>
        private async Task LoadDpasData()
        {
            List<ContactDpaModel> contactDpaItems = null;

            try
            {
                contactDpaItems = JsonConvert.DeserializeObject<List<ContactDpaModel>>(Compression.DecompressString((await this.documentsRepository.GetDocumentAsync("3bd96c0d-816f-4502-ab9a-799c4f518564")).JsonSmeDoc));
            }
            catch (Exception e)
            {
            }

            if (contactDpaItems == null)
            {
                if (!(await ConnectivityHelper.CheckInternetConection(this.dialogService)))
                {
                    return;
                }

                contactDpaItems = await httpService.GetContactsDPAs();
            }

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

        /// <summary>
        /// Method for extending/shrink content of clicked heading
        /// </summary>
        /// <param name="item"></param>
        private void DoExtendContent(ExpandableLVFocContactsWithDPAsViewModel item)
        {
            item.Expanded = !item.Expanded;
        }

        /// <summary>
        /// Open dialar of the device for phone call
        /// </summary>
        /// <param name="telNumber">Clicked phone number</param>
        private void DoCallNumber(string telNumber)
        {
            try
            {
                PhoneDialer.Open(telNumber);
            }
            catch (Exception e)
            {
            }

            //Device.OpenUri(new Uri($"tel:{telNumber}"));
        }

        /// <summary>
        /// Open default email client on device
        /// </summary>
        /// <param name="emailAddress">Clicked email address</param>
        private void DoOpenEmailClient(string emailAddress)
        {
            try
            {
                var message = new EmailMessage
                {
                    Subject = string.Empty,
                    Body = string.Empty,
                    To = new List<string> { emailAddress }
                };

                Email.ComposeAsync(message);
            }
            catch (Exception e)
            {
            }

            //Device.OpenUri(new Uri($"mailto:{emailAddress}"));
        }

        /// <summary>
        /// Open default browser with clicked url
        /// </summary>
        /// <param name="url">Clicked url</param>
        private void DoOpenUrl(string url)
        {
            Device.OpenUri(new Uri(url));
        }
    }
}
