using Newtonsoft.Json;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using SmeData.Mobile.Models;
using SmeData.Mobile.Models.Settings;
using SmeData.Mobile.Services;
using SmeData.SharedModels.Language;
using SmeData.SharedModels.Link;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;

namespace SmeData.Mobile.ViewModels
{
    public class UsefulLinksPageViewModel : BaseViewModel
    {
        private readonly HttpService httpService;
        public ICommand GoToLinkCommand { get; set; }

        public string LinksFont { get => $"t|16|{ScreenWidth}"; }

        public string ImageFont { get => $"i|28|{ScreenWidth}"; }

        private ObservableCollection<LinkModel> links;
        public ObservableCollection<LinkModel> Links
        {
            get => links;
            set
            {
                links = value;
                this.RaisePropertyChanged(nameof(this.Links));
            }
        }

        public UsefulLinksPageViewModel(INavigationService navigationService, SettingsModel settings, HttpService httpService) : base(navigationService)
        {
            this.settings = settings;
            this.httpService = httpService;
            this.GoToLinkCommand = new DelegateCommand<string>(this.GoToLink);
        }

        private void GoToLink(string linkUrl)
        {
            Device.OpenUri(new Uri(linkUrl));
        }

        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            //switch (this.settings.Language)
            //{
            //    case SmeLanguage.Bulgarian:
            //        Links = new ObservableCollection<LinkModel>() {
            //                new LinkModel("Европейска комисия – Защита на данните",
            //                              "https://ec.europa.eu/info/law/law-topic/data-protection_bg", 1),
            //                new LinkModel("Европейски комитет за защита на данните",
            //                              "https://edpb.europa.eu/edpb_bg", 1),
            //                new LinkModel("Европейски надзорен орган по защита на данните",
            //                              "https://edps.europa.eu/edps-homepage_en", 1),
            //                new LinkModel("Агенция на Европейския съюз за основните права – Защита на данните",
            //                              "https://fra.europa.eu/en/theme/information-society-privacy-and-data-protection/practical-guidance", 1),
            //                new LinkModel("Съд на ЕС – Съдебна практика",
            //                              "https://eur-lex.europa.eu/search.html?lang=en&SUBDOM_INIT=EU_CASE_LAW&DTS_DOM=EU_LAW&CASE_LAW_SUMMARY=false&type=advanced&DTS_SUBDOM=EU_CASE_LAW&qid=1568112726237&orDC_DOM_CODED=DC_CODED%3D2828%2CDC_CODED%3D5595&locale=bg", 1),
            //                new LinkModel("Комисия за защита на личните данни (България) – Практика",
            //                              "https://www.cpdp.bg/?p=rubric&aid=3", 1),
            //                new LinkModel("Надзорен орган за защита на личните данни (Италия) – Практика",
            //                              "https://www.garanteprivacy.it/web/guest/home/provvedimenti-normativa/provvedimenti", 1),
            //                new LinkModel("Организация за икономическо сътрудничество и развитие – Защита на данните",
            //                              "https://www.oecd.org/internet/ieconomy/oecdguidelinesontheprotectionofprivacyandtransborderflowsofpersonaldata.htm", 1),
            //                new LinkModel("Съвет на Европа – Защита на данните",
            //                              "https://www.coe.int/en/web/data-protection", 1),
            //                new LinkModel("Европейски съд по правата на човека – Защита на данните",
            //                              "https://www.coe.int/en/web/data-protection/echr-case-law", 1),

            //            };
            //        break;
            //    case SmeLanguage.English:
            //    default:
            //        Links = new ObservableCollection<LinkModel>() {
            //                new LinkModel("European Commission – Data Protection",
            //                              "https://ec.europa.eu/info/law/law-topic/data-protection_en", 4),
            //                new LinkModel("European Data protection Board",
            //                              "https://edpb.europa.eu/", 4),
            //                new LinkModel("European Data Protection Supervisor",
            //                              "https://edps.europa.eu/data-protection/our-work/subjects/privacy-eu-institutions_en", 4),
            //                new LinkModel("European Union Agency for Fundamental Rights – Data Protection",
            //                              "https://fra.europa.eu/en/theme/information-society-privacy-and-data-protection/practical-guidance", 4),
            //                new LinkModel("Court of Justice of the EU – Case law",
            //                              "https://eur-lex.europa.eu/search.html?qid=1568112726237&CASE_LAW_SUMMARY=false&DTS_DOM=EU_LAW&type=advanced&lang=en&SUBDOM_INIT=EU_CASE_LAW&DTS_SUBDOM=EU_CASE_LAW&orDC_DOM_CODED=DC_CODED%3D2828,DC_CODED%3D5595", 4),
            //                new LinkModel("Commission for Personal Data Protection (Bulgaria) – Practice",
            //                              "https://www.cpdp.bg/en/index.php?p=rubric&aid=3", 4),
            //                new LinkModel("Italian Data Protection Authority – Practice",
            //                              "https://www.garanteprivacy.it/web/guest/home/provvedimenti-normativa/provvedimenti", 4),
            //                new LinkModel("Organisation for Economic Co-operation and Development – Data Protection",
            //                              "https://www.oecd.org/internet/ieconomy/oecdguidelinesontheprotectionofprivacyandtransborderflowsofpersonaldata.htm", 4),
            //                new LinkModel("Council of Europe – Data Protection",
            //                              "https://www.coe.int/en/web/data-protection", 4),
            //                new LinkModel("European Court of Human Rights – Data Protection",
            //                              "https://www.coe.int/en/web/data-protection/echr-case-law", 4),
            //            };
            //        break;
            //    case SmeLanguage.Italian:
            //        Links = new ObservableCollection<LinkModel>() {
            //                new LinkModel("Commissione Europea - Protezione dei dati",
            //                              "https://ec.europa.eu/info/law/law-topic/data-protection_it", 5),
            //                new LinkModel("Comitato europeo per la protezione dei dati",
            //                              "https://edpb.europa.eu/edpb_it", 5),
            //                new LinkModel("Garante europeo della protezione dei dati",
            //                              "https://edps.europa.eu/edps-homepage_en", 5),
            //                new LinkModel("Agenzia europea dei diritti fondamentali – Protezione dei dati",
            //                              "https://fra.europa.eu/en/theme/information-society-privacy-and-data-protection/practical-guidance", 5),
            //                new LinkModel("Corte di giustizia dell’UE – Giurisprudenza",
            //                              "https://eur-lex.europa.eu/search.html?qid=1568112726237&CASE_LAW_SUMMARY=false&DTS_DOM=EU_LAW&type=advanced&lang=it&SUBDOM_INIT=EU_CASE_LAW&DTS_SUBDOM=EU_CASE_LAW&orDC_DOM_CODED=DC_CODED%3D2828,DC_CODED%3D5595", 5),
            //                new LinkModel("Commissione per la protezione dei dati personali (Bulgaria) – Provvedimenti",
            //                              "https://www.cpdp.bg/en/index.php?p=rubric&aid=3", 5),
            //                new LinkModel("Garante per la protezione dei dati personali – Provvedimenti",
            //                              "https://www.garanteprivacy.it/web/guest/home/provvedimenti-normativa/provvedimenti", 5),
            //                new LinkModel("Organizzazione per la cooperazione e lo sviluppo economico – Protezione dei dati",
            //                              "https://www.oecd.org/internet/ieconomy/oecdguidelinesontheprotectionofprivacyandtransborderflowsofpersonaldata.htm", 5),
            //                new LinkModel("Consiglio d'Europa - Protezione dei dati",
            //                              "https://www.coe.int/en/web/data-protection", 5),
            //                new LinkModel("Corte europea dei diritti dell'uomo – Protezione dei dati",
            //                              "https://www.coe.int/en/web/data-protection/echr-case-law", 5),
            //            };
            //        break;
            //}

            var usefulLinks = await httpService.GetUsefulkLinks();

            switch (this.settings.Language)
            {
                case SmeLanguage.Bulgarian:
                    Links = new ObservableCollection<LinkModel>(usefulLinks.Where(x => x.LangId == 1));
                    break;
                case SmeLanguage.English:
                default:
                    Links = new ObservableCollection<LinkModel>(usefulLinks.Where(x => x.LangId == 4));
                    break;
                case SmeLanguage.Italian:
                    Links = new ObservableCollection<LinkModel>(usefulLinks.Where(x => x.LangId == 5));
                    break;
            }
        }
    }
}
