using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using SmeData.Mobile.Services;
using SmeData.Mobile.Views;
using Prism.Unity;
using Prism.Ioc;
using SmeData.Mobile.Data;
using System.IO;
using SmeData.Mobile.ViewModels;
using System.Globalization;
using System.Threading;
using SmeData.Mobile.Helpers;
using SmeData.Mobile.Models.Settings;
using System.Reflection;
using System.Windows.Input;
using Xamarin.Forms.PlatformConfiguration;
using SmeData.Mobile.Data.Models;
using SmeData.Shared.Common;
using Newtonsoft.Json;
using System.Threading.Tasks;
using SmeData.SharedModels.Document;
using System.Collections.Generic;
using Xamarin.Essentials;
using SmeData.WebApi.Models;

namespace SmeData.Mobile
{
    public partial class App : PrismApplication
    {
        //public static readonly BindableProperty TabCommandProperty = BindableProperty.Create(nameof(TabCommand),
        //    typeof(ICommand),
        //    typeof(App), ClickCommand);

        //public ICommand TabCommand
        //{
        //    get => (ICommand)GetValue(TabCommandProperty);
        //    set => SetValue(TabCommandProperty, value);
        //}

        //public static ICommand ClickCommand => new Command<string>((url) =>
        //{
        //    Xamarin.Forms.Device.OpenUri(new Uri(url));
        //});

        public static AppRepository Repository;

        public App()
        {
            //var culture = new CultureInfo("bg-BG");

            //CrossMultilingual.Current.CurrentCultureInfo = culture;
            //SmeData.Mobile.Properties.Resources.Culture = culture;
        }

        protected override void OnStart()
        {
            var assembly = typeof(Properties.Resources).GetTypeInfo().Assembly; // "EmbeddedImages" should be a class in your app
            foreach (var res in assembly.GetManifestResourceNames())
            {
                System.Diagnostics.Debug.WriteLine("found resource: " + res);
            }
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<SettingsPage, SettingsPageViewModel>();
            containerRegistry.RegisterForNavigation<EuLegalPage, EuLegalPageViewModel>();
            containerRegistry.RegisterForNavigation<LegalFrameworkPage, LegalFrameworkPageViewModel>();
            containerRegistry.RegisterForNavigation<WelcomePage, WelcomePageViewModel>();
            containerRegistry.RegisterForNavigation<GuideForSmesPage, GuideForSmesPageViewModel>();
            containerRegistry.RegisterForNavigation<DecisionSupportPage, DecisionSupportPageViewModel>();
            containerRegistry.RegisterForNavigation<DocMainPage, DocMainPageViewModel>();
            containerRegistry.RegisterForNavigation<MainPage, MainPageViewModel>();
            containerRegistry.RegisterForNavigation<FaqPage, FaqPageViewModel>();
            containerRegistry.RegisterForNavigation<GdprDictionaryPage, GdprDictionaryPageViewModel>();
            containerRegistry.RegisterForNavigation<GuideForCitizensPage, GuideForCitizensPageViewModel>();
            containerRegistry.RegisterForNavigation<TrainingMaterialsPage, TrainingMaterialsPageViewModel>();
            containerRegistry.RegisterForNavigation<UsefulLinksPage, UsefulLinksPageViewModel>();
            containerRegistry.RegisterForNavigation<BookmarksPage, BookmarksPageViewModel>();
            containerRegistry.RegisterForNavigation<HelpPage, HelpPageViewModel>();
            containerRegistry.RegisterForNavigation<GdpMadeSimplePage, GdpMadeSimplePageViewModel>();
            containerRegistry.RegisterForNavigation<ContactWithNationalDPSsPage, ContactWithNationalDPSsPageViewModel>();
            containerRegistry.RegisterForNavigation<GuidelinesPage, GuidelinesPageViewModel>();
            containerRegistry.RegisterForNavigation<GuidelinesTabPage, GuidelinesTabPageViewModel>();
            containerRegistry.RegisterForNavigation<InternationalPage, InternationalPageViewModel>();
            containerRegistry.RegisterForNavigation<InternationalTreatiesPage, InternationalTreatiesPageViewModel>();
            containerRegistry.RegisterForNavigation<InternationalCaseLawPage, InternationalCaseLawPageViewModel>();
            containerRegistry.RegisterForNavigation<SearchPage, SearchPageViewModel>();
            containerRegistry.RegisterForNavigation<DocCaseLawShowPage, DocCaseLawShowPageViewModel>();
            containerRegistry.RegisterForNavigation<NationalPage, NationalPageViewModel>();
            containerRegistry.RegisterForNavigation<BgLegislationPage, BgLegislationPageViewModel>();
            containerRegistry.RegisterForNavigation<ItLegislationPage, ItLegislationPageViewModel>();
            containerRegistry.RegisterForNavigation<BgLegalPage, BgLegalPageViewModel>();
            containerRegistry.RegisterForNavigation<ItLegalPage, ItLegalPageViewModel>();
            containerRegistry.RegisterForNavigation<DeLegislationPage, DeLegislationPageViewModel>();
            containerRegistry.RegisterForNavigation<FrLegislationPage, FrLegislationPageViewModel>();
            containerRegistry.RegisterForNavigation<DeLegalPage, DeLegalPageViewModel>();
            containerRegistry.RegisterForNavigation<FrLegalPage, FrLegalPageViewModel>();
            containerRegistry.RegisterForNavigation<NationalCaseLawPage, NationalCaseLawPageViewModel>();
            containerRegistry.RegisterForNavigation<NationalGuidelinesPage, NationalGuidelinesPageViewModel>();
            containerRegistry.RegisterForNavigation<NationalFederalConstitutionalCourtPage, NationalFederalConstitutionalCourtPageViewModel>();
            containerRegistry.RegisterForNavigation<NationalFederalCourtOfJusticePage, NationalFederalCourtOfJusticePageViewModel>();
            containerRegistry.RegisterForNavigation<NationalCourtOfCassationPage, NationalCourtOfCassationPageViewModel>();
            containerRegistry.RegisterForNavigation<NationalCNILPage, NationalCNILPageViewModel>();
            containerRegistry.RegisterForNavigation<NationalCouncilOfStatePage, NationalCouncilOfStatePageViewModel>();
            containerRegistry.RegisterForNavigation<NavigationPage>();

            var dbPath = Device.RuntimePlatform != Device.UWP ? Path.Combine(FileSystem.AppDataDirectory, "data") : FileSystem.AppDataDirectory;

            if (!Directory.Exists(dbPath))
            {
                Directory.CreateDirectory(dbPath);
            }

            dbPath = Path.Combine(dbPath, "database.sqlite");

            var repository = new AppRepository(dbPath);
            containerRegistry.RegisterInstance(repository);

            var settings = SettingsHelper.LoadFromDb(repository);
            containerRegistry.RegisterInstance(settings);

            var httpService = new HttpService(@"https://smedata.apis.bg/api/");
            DocumentHelper.HttpService = httpService;
            containerRegistry.RegisterInstance(httpService);

            DocumentService docService = new DocumentService(httpService, repository);

            containerRegistry.RegisterInstance(new DocumentService(httpService, repository));
            containerRegistry.RegisterForNavigation<OfflineDocumentsPage, OfflineDocumentsPageViewModel>();
            containerRegistry.RegisterForNavigation<ExpandableListViewCommonPage, ExpandableListViewCommonPageViewModel>();
            containerRegistry.RegisterForNavigation<NationalOpinionsPage, NationalOpinionsPageViewModel>();
            containerRegistry.RegisterForNavigation<NationalDecisionsPage, NationalDecisionsPageViewModel>();
            containerRegistry.RegisterForNavigation<NationalSupremeAdministrativeCourtPage, NationalSupremeAdministrativeCourtPageViewModel>();
            containerRegistry.RegisterForNavigation<NationalOtherCourtsPage, NationalOtherCourtsPageViewModel>();
            containerRegistry.RegisterForNavigation<AboutPage, AboutPageViewModel>();
            containerRegistry.RegisterForNavigation<NationalAdministrativeCourtSofiaPage, NationalAdministrativeCourtSofiaPageViewModel>();
            containerRegistry.RegisterForNavigation<NationalAdministrativeCourtPlovdivPage, NationalAdministrativeCourtPlovdivPageViewModel>();
            containerRegistry.RegisterForNavigation<NationalAdministrativeCourtVarnaPage, NationalAdministrativeCourtVarnaPageViewModel>();
            containerRegistry.RegisterForNavigation<NationalAdministrativeCourtBurgasPage, NationalAdministrativeCourtBurgasPageViewModel>();
            containerRegistry.RegisterForNavigation<NationalSupremeCourtOfCassationPage, NationalSupremeCourtOfCassationPageViewModel>();
            containerRegistry.RegisterForNavigation<NationalConstitutionalCourtPage, NationalConstitutionalCourtPageViewModel>();
            containerRegistry.RegisterForNavigation<EuDataProtectionBoardPage, EuDataProtectionBoardPageViewModel>();
            containerRegistry.RegisterForNavigation<DocCommonShowPage, DocCommonShowPageViewModel>();
            containerRegistry.RegisterForNavigation<EuDataProtectionSupervisorPage, EuDataProtectionSupervisorPageViewModel>();
        }

        protected override void OnInitialized()
        {
            this.InitializeComponent();
            NavigationService.NavigateAsync(nameof(Views.MainPage) + "/" + nameof(NavigationPage) + "/" + nameof(WelcomePage));
        }
    }
}
