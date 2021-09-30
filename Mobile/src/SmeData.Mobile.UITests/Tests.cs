using System;
using System.IO;
using System.Linq;
using NUnit.Framework;
using Xamarin.UITest;
using Xamarin.UITest.Queries;

namespace SmeData.Mobile.UITests
{
    [TestFixture(Platform.Android)]
    [TestFixture(Platform.iOS)]
    public class Tests
    {
        IApp app;
        Platform platform;

        public Tests(Platform platform)
        {
            this.platform = platform;
        }

        [SetUp]
        public void BeforeEachTest()
        {
            app = AppInitializer.StartApp(platform);


            app.SwipeLeftToRight(0.99, 5000, true);
            app.Tap(x => x.Marked("albMenuSettings"));
            app.WaitForElement(x => x.Marked("attlSettings"));

            app.WaitForElement(x => x.Marked("apckrLanguage"));
            app.Tap(x => x.Marked("apckrLanguage"));
            app.Tap(x => x.Property("text").Contains("Bulgarian"));
            app.TapCoordinates(500, 700);

            app.Back();
        }

        //[Test]
        //public void TestLanguageChange()
        //{
        //    //app.Repl();

        //    app.SwipeLeftToRight(0.99, 5000, true);
        //    app.Tap(x => x.Marked("albMenuSettings"));
        //    app.WaitForElement(x => x.Marked("attlSettings"));
        //    app.Screenshot("SettingsPage");

        //    app.WaitForElement(x => x.Marked("apckrLanguage"));
        //    app.Tap(x => x.Marked("apckrLanguage"));
        //    app.Screenshot("LanguagePickerPage");
        //    app.Tap(x => x.Property("text").Contains("Bulgarian"));

        //    app.TapCoordinates(500, 700);
        //    app.Back();

        //    app.Screenshot("WelcomeScreenWithBulgarianLanguage");
        //}

        [Test]
        public void TestSideMenu()
        {
            //app.Repl();

            app.SwipeLeftToRight(0.99, 5000, true);
            app.Screenshot("SideMenuPic");
            app.Tap(x => x.Marked("albMenuLegalLibrary"));
            app.WaitForElement(x => x.Marked("attlLegalLibrary"));
            app.Screenshot("LegalLibraryPage");
            app.Back();

            app.SwipeLeftToRight(0.99, 5000, true);
            app.Tap(x => x.Marked("albMenuGdprDictionary"));
            app.WaitForElement(x => x.Marked("attlGdprDictionary"));
            app.Screenshot("GdprDictionaryPage");
            app.Back();

            app.SwipeLeftToRight(0.99, 5000, true);
            app.Tap(x => x.Marked("albMenuGuideForCitizens"));
            app.WaitForElement(x => x.Marked("attlGuideForCitizens"));
            app.Screenshot("GuideForCitizensPage");
            app.Back();

            app.SwipeLeftToRight(0.99, 5000, true);
            app.Tap(x => x.Marked("albMenuGuideForSMEs"));
            app.WaitForElement(x => x.Marked("attlGuideForSme"));
            app.Screenshot("GuideForSmePage");
            app.Back();

            app.SwipeLeftToRight(0.99, 5000, true);
            app.Tap(x => x.Marked("albMenuTrainingMaterials"));
            app.WaitForElement(x => x.Marked("attlTrainingMaterials"));
            app.Screenshot("TrainingMaterialsPage");
            app.Back();

            app.SwipeLeftToRight(0.99, 5000, true);
            app.Tap(x => x.Marked("albMenuUsefulLinks"));
            app.WaitForElement(x => x.Marked("attlUsefulLinks"));
            app.Screenshot("UsefulLinksPage");
            app.Back();

            app.SwipeLeftToRight(0.99, 5000, true);
            app.Tap(x => x.Marked("albMenuBookmarks"));
            app.WaitForElement(x => x.Marked("attlBookmarks"));
            app.Screenshot("BookmarksPage");
            app.Back();

            app.SwipeLeftToRight(0.99, 5000, true);
            app.Tap(x => x.Marked("albMenuSavedDocuments"));
            app.WaitForElement(x => x.Marked("attlOfflineDocuments"));
            app.Screenshot("OfflineDocumentsPage");
            app.Back();

            app.SwipeLeftToRight(0.99, 5000, true);
            app.Tap(x => x.Marked("albMenuSearch"));
            app.WaitForElement(x => x.Marked("attlSearch"));
            app.Screenshot("SearchPage");
            app.Back();

            app.SwipeLeftToRight(0.99, 5000, true);
            app.Tap(x => x.Marked("albMenuSettings"));
            app.WaitForElement(x => x.Marked("attlSettings"));
            app.Screenshot("SettingsPage");
            app.Back();

            app.SwipeLeftToRight(0.99, 5000, true);
            app.Tap(x => x.Marked("albMenuHelp"));
            app.WaitForElement(x => x.Marked("attlHelp"));
            app.Screenshot("HelpPage");
            app.Back();
        }

        [Test]
        public void TestWelcomePageButtonsMenu()
        {
            app.Repl();

            app.Screenshot("WelcomePagePic");
            app.Tap(x => x.Marked("abtnLegalFramework"));
            app.WaitForElement(x => x.Marked("attlLegalLibrary"));
            app.Screenshot("LegalFrameworkPage");
            app.Back();

            app.Tap(x => x.Marked("abtnGdprDictionary"));
            app.WaitForElement(x => x.Marked("attlGdprDictionary"));
            app.Screenshot("GdprDictionaryPage");
            app.Back();

            app.Tap(x => x.Marked("abtnGuideForCitizens"));
            app.WaitForElement(x => x.Marked("attlGuideForCitizens"));
            app.Screenshot("GuideForCitizensPage");
            app.Back();

            app.Tap(x => x.Marked("abtnGuideForSmes"));
            app.WaitForElement(x => x.Marked("attlGuideForSme"));
            app.Screenshot("GuideForSmesPage");
            app.Back();
        }

        [Test]
        public void TestLegalFrameworkPage()
        {
            //app.Repl();

            app.Tap(x => x.Marked("abtnLegalFramework"));
            app.WaitForElement(x => x.Marked("attlLegalLibrary"));
            app.Screenshot("LegalFrameworkPage");

            app.Tap(x => x.Marked("abtnEuropeanUnion"));
            app.WaitForElement(x => x.Marked("attlEuLegal"));
            app.Screenshot("EuropeanUnionPage");
            app.Back();

            app.Tap(x => x.Marked("abtnNational"));
            app.WaitForElement(x => x.Marked("attlNational"));
            app.Screenshot("NationalPage");
            app.Back();

            app.Tap(x => x.Marked("abtnInternational"));
            app.WaitForElement(x => x.Marked("attlInternational"));
            app.Screenshot("InternationalPage");
            app.Back();
        }

        [Test]
        public void TestEuLegalPage()
        {
            //app.Repl();

            app.Tap(x => x.Marked("abtnLegalFramework"));
            app.WaitForElement(x => x.Marked("attlLegalLibrary"));
            app.Screenshot("LegalFrameworkPage");

            app.Tap(x => x.Marked("abtnEuropeanUnion"));
            app.WaitForElement(x => x.Marked("attlEuLegal"));
            app.WaitForElement(x => x.Marked("atbLegislation"));
            app.Screenshot("EuropeanUnionPage");

            app.Tap(x => x.Marked("Guidelines"));
            app.WaitForElement(x => x.Marked("atbGuidelines"));
            app.WaitForElement(x => x.Marked("abtnEuropeanDataProtectionBoard"));
            app.WaitForElement(x => x.Marked("abtnEuropeanDataProtectionSupervisor"));
            app.Screenshot("GuidelinesTabActive");

            app.Tap(x => x.Marked("Case law"));
            app.WaitForElement(x => x.Marked("atbCaseLaw"));
            app.WaitForElement(x => x.Marked("alDocInCaseLaw"));
            app.Screenshot("CaseLawTabActive");

            app.Tap(x => x.Marked("Legislation"));
            app.WaitForElement(x => x.Marked("atbLegislation"));
            app.WaitForElement(x => x.Marked("alDocInLegislation"));
            app.Screenshot("LegislationTabActive");
        }

        [Test]
        public void TesNationalPage()
        {
            //app.Repl();

            app.Tap(x => x.Marked("abtnLegalFramework"));
            app.WaitForElement(x => x.Marked("attlLegalLibrary"));
            app.Screenshot("LegalFrameworkPage");

            app.Tap(x => x.Marked("abtnNational"));
            app.WaitForElement(x => x.Marked("attlNational"));
            app.WaitForElement(x => x.Marked("abtnBulgaria"));
            app.WaitForElement(x => x.Marked("abtnItaly"));
            app.Screenshot("NationalPage");

            app.Tap(x => x.Marked("abtnBulgaria"));
            app.WaitForElement(x => x.Marked("attlBulgaria"));
            app.Screenshot("BulgariaPage");

            app.Tap(x => x.Marked("Guidelines"));
            app.WaitForElement(x => x.Marked("atbBgGuidelines"));
            app.WaitForElement(x => x.Marked("abtnDecisions"));
            app.WaitForElement(x => x.Marked("abtnOpinions"));
            app.WaitForElement(x => x.Marked("abtnFAQ"));
            app.Screenshot("GuidelinesBgTabActive");

            app.Tap(x => x.Marked("Case law"));
            app.WaitForElement(x => x.Marked("atbBgCaseLaw"));
            app.WaitForElement(x => x.Marked("abtnSupremeAdministrativeCourt"));
            app.WaitForElement(x => x.Marked("abtnOtherCourts"));
            app.Screenshot("CaseLawBgTabActive");

            app.Tap(x => x.Marked("Legislation"));
            app.WaitForElement(x => x.Marked("atbBgLegislation"));
            app.WaitForElement(x => x.Marked("alDocInBgLegislation"));
            app.Screenshot("LegislationBgTabActive");

            app.Back();

            app.Tap(x => x.Marked("abtnItaly"));
            app.WaitForElement(x => x.Marked("attlItaly")); 
            app.Screenshot("ItalyPage");

            app.Tap(x => x.Marked("Guidelines"));
            app.WaitForElement(x => x.Marked("atbItGuidelines"));
            app.WaitForElement(x => x.Marked("abtnDecisions"));
            app.WaitForElement(x => x.Marked("abtnOpinions"));
            app.WaitForElement(x => x.Marked("abtnFAQ"));
            app.Screenshot("GuidelinesItTabActive");

            app.Tap(x => x.Marked("Case law"));
            app.WaitForElement(x => x.Marked("atbItCaseLaw"));
            app.WaitForElement(x => x.Marked("abtnSupremeAdministrativeCourt"));
            app.WaitForElement(x => x.Marked("abtnOtherCourts"));
            app.Screenshot("CaseLawItTabActive");

            app.Tap(x => x.Marked("Legislation"));
            app.WaitForElement(x => x.Marked("atbItLegislation"));
            // Still no documents in Italian Legislation. To uncomments after documents are added.
            //app.WaitForElement(x => x.Marked("alDocInBgLegislation"));
            app.Screenshot("LegislationItTabActive");
        }

        [Test]
        public void TestInternationalPage()
        {
            //app.Repl();

            app.Tap(x => x.Marked("abtnLegalFramework"));
            app.WaitForElement(x => x.Marked("attlLegalLibrary"));
            app.Screenshot("LegalFrameworkPage");

            app.Tap(x => x.Marked("abtnInternational"));
            app.WaitForElement(x => x.Marked("attlInternational"));
            app.Screenshot("InternationalPage");

            app.Tap(x => x.Marked("Case Law"));
            app.WaitForElement(x => x.Marked("alDocInIntCaseLaw"));
            app.Screenshot("CaseLawTabActive");

            app.Tap(x => x.Marked("Treaties"));
            app.WaitForElement(x => x.Marked("alDocInIntTreaties"));
            app.Screenshot("TreatiesTabActive");

            app.Back();
        }

        [Test]
        public void TestGdprDictionatyPage()
        {
            //app.Repl();

            app.Tap(x => x.Marked("abtnGdprDictionary"));
            app.WaitForElement(x => x.Marked("attlGdprDictionary"));
            app.WaitForElement(x => x.Marked("aimgExtendedListArrow"));
            app.Screenshot("GdprDictionaryPage");

            app.Tap(x => x.Marked("aimgExtendedListArrow"));
            app.WaitForElement(x => x.Marked("alblExtendedListContent"));
            app.Screenshot("ExtendedEntry");

            app.Tap(x => x.Marked("aimgExtendedListArrow"));
            app.Screenshot("UnextendedEntry");

            app.Back();
        }

        [Test]
        public void TestGuideForCitizensPage()
        {
            //app.Repl();

            app.Tap(x => x.Marked("abtnGuideForCitizens"));
            app.WaitForElement(x => x.Marked("attlGuideForCitizens"));
            app.Screenshot("GuideForCitizensPage");

            app.WaitForElement(x => x.Marked("abtnGDPRMadeSimple"));
            app.Tap(x => x.Marked("abtnGDPRMadeSimple"));
            app.WaitForElement(x => x.Marked("attlGdpMadeSimple")); 
            app.WaitForElement(x => x.Marked("awvGDPRMadeSimple"));
            app.Screenshot("GDPRMadeSimplePage");
            app.Back();

            app.WaitForElement(x => x.Marked("abtnContactNationalDPA"));
            app.Tap(x => x.Marked("abtnContactNationalDPA"));
            app.WaitForElement(x => x.Marked("attlContactNationalDPA"));
            app.WaitForElement(x => x.Marked("alblExtendedListContent"));
            app.Screenshot("ContactNationalDPAPage");
            app.Tap(x => x.Marked("aimgExtendedListArrow"));
            app.Screenshot("DpaUnextendedEntry");
            app.Tap(x => x.Marked("aimgExtendedListArrow"));
            app.WaitForElement(x => x.Marked("alblExtendedListContent"));
            app.Screenshot("DpaExtendedEntry");
            app.Back();

            app.WaitForElement(x => x.Marked("abtnFAQ"));
            app.Tap(x => x.Marked("abtnFAQ"));
            app.WaitForElement(x => x.Marked("attlFAQ"));
            app.WaitForElement(x => x.Marked("aimgExtendedListArrow"));
            app.Screenshot("FAQPage");
            app.Tap(x => x.Marked("aimgExtendedListArrow"));
            app.WaitForElement(x => x.Marked("alblExtendedListContent"));
            app.Screenshot("FaqExtendedEntry");
            app.Tap(x => x.Marked("aimgExtendedListArrow"));
            app.Screenshot("FaqUnextendedEntry");
            
            app.Back();
        }

        [Test]
        public void TestGuideForSmesPage()
        {
            //app.Repl();

            app.Tap(x => x.Marked("abtnGuideForSmes"));
            app.WaitForElement(x => x.Marked("attlGuideForSme"));
            app.Screenshot("GuideForSmesPage");

            app.WaitForElement(x => x.Marked("abtnGDPRMadeSimple"));
            app.Tap(x => x.Marked("abtnGDPRMadeSimple"));
            app.WaitForElement(x => x.Marked("attlGdpMadeSimple"));
            app.WaitForElement(x => x.Marked("awvGDPRMadeSimple"));
            app.Screenshot("GDPRMadeSimplePage");
            app.Back();

            app.WaitForElement(x => x.Marked("abtnDecisionSupportTool"));
            app.Tap(x => x.Marked("abtnDecisionSupportTool"));
            app.WaitForElement(x => x.Marked("attlDecisionSupportTool"));
            app.WaitForElement(x => x.Marked("awvDecisionSupportTool"));
            app.Screenshot("DecisionSupportToolPage");
            app.Back();

            app.WaitForElement(x => x.Marked("abtnContactNationalDPA"));
            app.Tap(x => x.Marked("abtnContactNationalDPA"));
            app.WaitForElement(x => x.Marked("attlContactNationalDPA"));
            app.WaitForElement(x => x.Marked("alblExtendedListContent"));
            app.Screenshot("ContactNationalDPAPage");
            app.Tap(x => x.Marked("aimgExtendedListArrow"));
            app.Screenshot("DpaUnextendedEntry");
            app.Tap(x => x.Marked("aimgExtendedListArrow"));
            app.WaitForElement(x => x.Marked("alblExtendedListContent"));
            app.Screenshot("DpaExtendedEntry");
            app.Back();

            app.WaitForElement(x => x.Marked("abtnFAQ"));
            app.Tap(x => x.Marked("abtnFAQ"));
            app.WaitForElement(x => x.Marked("attlFAQ"));
            app.WaitForElement(x => x.Marked("aimgExtendedListArrow"));
            app.Screenshot("FAQPage");
            app.Tap(x => x.Marked("aimgExtendedListArrow"));
            app.WaitForElement(x => x.Marked("alblExtendedListContent"));
            app.Screenshot("FaqExtendedEntry");
            app.Tap(x => x.Marked("aimgExtendedListArrow"));
            app.Screenshot("FaqUnextendedEntry");
            app.Back();
        }

        [Test]
        public void TestSearch()
        {
            //app.Repl();

            app.SwipeLeftToRight(0.99, 5000, true);
            app.Tap(x => x.Marked("albMenuSearch"));
            app.WaitForElement(x => x.Marked("attlSearch"));
            app.Screenshot("SearchPage");

            app.Tap(x => x.Marked("asbSearch"));
            app.EnterText("case");
            app.PressEnter();
            app.WaitForElement(x => x.Marked("alSearchResultEntry"));
            app.Screenshot("SearchResults");

            app.Tap(x => x.Marked("alSearchResultEntry"));
            app.WaitForElement(x => x.Marked("attlDocCaseLawShow"));
            app.Screenshot("SearchResultsFirstDocumentsOpen");
        }
    }
}
