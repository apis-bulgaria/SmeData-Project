using SmeData.Mobile.Services;
using SmeData.SharedModels.Language;
// using SmeData.SharedModels.Translation;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace SmeData.Mobile.Helpers
{
    public class TranslationManager
    {
        private readonly HttpService httpService = new HttpService(@"https://smedata.apis.bg/api/");
        private Dictionary<SmeLanguage, TranslationLanguage> transLanguages;

        public TranslationManager()
        {
            this.Init();
        }

        public void Init()
        {
            this.transLanguages = new Dictionary<SmeLanguage, TranslationLanguage>();

            try
            {
                var enDict = new Dictionary<string, string>();
                var bgDict = new Dictionary<string, string>();
                var itDict = new Dictionary<string, string>();
                var deDict = new Dictionary<string, string>();
                var frDict = new Dictionary<string, string>();

                var allTransaltionsLines = httpService.GetTranslations().Result;
                foreach (var line in allTransaltionsLines)
                {
                    if (string.IsNullOrWhiteSpace(line))
                    {
                        continue;
                    }

                    var entries = line.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);

                    enDict.Add(entries[0], entries[1]);
                    bgDict.Add(entries[0], entries[2]);
                    itDict.Add(entries[0], entries[3]);
                    deDict.Add(entries[0], entries[4]);
                    frDict.Add(entries[0], entries[5]);
                }

                this.transLanguages[SmeLanguage.English] = new TranslationLanguage(enDict);
                this.transLanguages[SmeLanguage.Bulgarian] = new TranslationLanguage(bgDict);
                this.transLanguages[SmeLanguage.Italian] = new TranslationLanguage(itDict);
                this.transLanguages[SmeLanguage.German] = new TranslationLanguage(deDict);
                this.transLanguages[SmeLanguage.French] = new TranslationLanguage(frDict);
            }
            catch (Exception e)
            {
                this.AddEnTranslation();
                this.AddBgTranslation();
                this.AddItTranslation();
            }
        }

        private void AddEnTranslation()
        {
            var enDict = new Dictionary<string, string>()
            {
                ["Bookmarks"] = "Bookmarks",
                ["Bulgaria"] = "Bulgaria",
                ["CaseLaw"] = "Case law",
                ["ContactNationalDPA"] = "Contact with National Data Protection Authorities",
                ["Decisions"] = "Decisions",
                ["DecisionSupportTool"] = "Decision support tools",
                ["DocumentContent"] = "Document content",
                ["Erase"] = "Erase",
                ["EUlegalframework"] = "EU legal framework",
                ["EuropeanDataProtectionBoard"] = "European Data Protection Board",
                ["EuropeanDataProtectionSupervisor"] = "European Data Protection Supervisor",
                ["EuropeanUnion"] = "European Union",
                ["FAQ"] = "Frequently asked questions",
                ["GdprDictionary"] = "GDPR dictionary",
                ["GDPRMadeSimple"] = "GDPR made simple",
                ["GdprPocket"] = "GDPR in Your Pocket",
                ["GuideForCitizens"] = "Guide for citizens",
                ["GuideForSMEs"] = "Guide for SMEs",
                ["Guidelines1"] = "Guidelines",
                ["Guidelines2"] = "Guidelines",
                ["Help"] = "Help",
                ["International"] = "International acts",
                ["InternationalCaseLaw"] = "International case law",
                ["InternationalTreaties"] = "International treaties",
                ["Italy"] = "Italy",
                ["Language"] = "Language",
                ["LegalLibrary"] = "Legal library",
                ["Legislation"] = "Legislation",
                ["MainPageCaption"] = "Main page",
                ["MainPageFooter"] = "SMEDATA Project is funded by the EU's Rights, Equality and Citizenship Programme (2014-2020)",
                ["Menu"] = "Menu",
                ["National"] = "National acts",
                ["NationalCaseLaw"] = "National case law",
                ["NationalGuidelines"] = "National guidelines",
                ["Next"] = "Next",
                ["Opinions"] = "Opinions",
                ["OtherCourts"] = "Other courts",
                ["GuideForSMEsInfoText"] = "Provides practical guidance on the obligations placed on SMEs under the GDPR and how to get GDPR-compliant",
                ["Prev"] = "Previous",
                ["ReloadBookmarks"] = "Reload bookmarks",
                ["SavedDocuments"] = "Saved documents",
                ["SaveOffline"] = "Save offline",
                ["Search"] = "Search",
                ["SearchInDocuments"] = "Search in documents",
                ["SelectedEntry"] = "Selected entry",
                ["Settings"] = "Settings",
                ["SmeDataIntroText"] = "This app was developed by SMEDATA Project. The project aims at ensuring the highest degree of privacy and personal data protection through innovative tools for SMEs and citizens.",
                ["SupremeAdministrativeCourt"] = "Supreme Administrative Court",
                ["AdministrativeCourtSofia"] = "Administrative Court Sofia-city",
                ["TrainingMaterials"] = "Training materials",
                ["Update"] = "Update",
                ["UpdateAllDocuments"] = "Update all documents",
                ["UsefulLinks"] = "Useful links",
                ["Welcome"] = "Welcome",
                ["GuideForCitizensInfoText"] = "Provides practical guidance on personal data protection under the GDPR and how citizens can exercise their rights",
                ["LegalFrameworkInfoText"] = "Provides access to EU’s, national (Bulgaria and Italy) and international legal acts in the field of personal data protection",
                ["CaseLawDataProtectionDirective"] = "Case law related to the Data Protection Directive",
                ["CaseLawDirective2016/681"] = "Case law related to Directive 2016/681",
                ["CaseLawEUInstitutionsDataProtectionRegulation"] = "Case law related to the EU Institutions Data Protection Regulation",
                ["CaseLawDirective2002/58/EC"] = "Case law related to Directive 2002/58/EC",
                ["ConstitutionalCourt"] = "Constitutional Court",
                ["SupremeCourtOfCassation"] = "Supreme Court of Cassation",
                ["Recitals"] = "Recitals",
                ["Citations"] = "Citations ",
                ["Preamble"] = "Preamble ",
                ["Articles"] = "Articles ",
                ["Title"] = "Title ",
                ["About"] = "About",
                ["KeyInstruments"] = "Key Instruments",
                ["OtherInstruments"] = "Other Instruments",
                ["AdequacyDecisions"] = "Adequacy decisions",
                ["SchengenAcquis"] = "Schengen Acquis",
                ["RepealedInstruments"] = "Repealed Instruments",
                ["Treaties"] = "Treaties",
                ["31995L0046"] = "95/46/EC",
                ["Error"] = "Error",
                ["Message"] = "Message",
                ["Оk"] = "ОК",
                ["WifiSettingsOption"] = "Download documents only on Wifi connectivity",
                ["No Internet Access"] = "No Internet Access",
                ["No Wifi Access"] = "You are not connected to Wifi network. If you want to open documents not only when connected to a Wifi network, disable this option from \"Settings\".",
                ["Yes"] = "Yes",
                ["No"] = "No",
                ["UpdateDocumentExist"] = "There is an update of document(s) that you have saved on your device. Do you want to open \"Saved documents\" to update the document(s)?",
                ["Warning"] = "Warning",
                ["CurrentAppVersion"] = "Version: ",
                ["OtherDocuments"] = "Other Documents",
                ["DocumentIsSaved"] = "The document is saved",
                ["DocumentIsUpdated"] = "The document is updated",
                ["DocumentIsAlreadySaved"] = "The document is already saved. Do you want to update it?",
                ["SelectLanguage"] = "Select language",
                ["UserDocuments"] = "User documents",
                ["SystemDocuments"] = "System documents",

            };

            this.transLanguages[SmeLanguage.English] = new TranslationLanguage(enDict);
        }

        private void AddItTranslation()
        {
            var itDict = new Dictionary<string, string>()
            {
                ["Bookmarks"] = "Segnalibri",
                ["Bulgaria"] = "Bulgaria",
                ["CaseLaw"] = "Giurisprudenza",
                ["ContactNationalDPA"] = "Contatto con i autorita nazionali per la protezione dei dati",
                ["Decisions"] = "Decisioni",
                ["DecisionSupportTool"] = "Strumenti di supporto decisionale",
                ["DocumentContent"] = "Contenuto del documento",
                ["Erase"] = "Cancellare",
                ["EUlegalframework"] = "Quadro giuridico dell'UE",
                ["EuropeanDataProtectionBoard"] = "Comitato europeo per la protezione dei dati",
                ["EuropeanDataProtectionSupervisor"] = "Garante europeo della protezione dei dati",
                ["EuropeanUnion"] = "Unione europea",
                ["FAQ"] = "Domande frequenti",
                ["GdprDictionary"] = "Dizionario GDPR",
                ["GDPRMadeSimple"] = "GDPR reso semplice",
                ["GdprPocket"] = "GDPR in tasca",
                ["GuideForCitizens"] = "Guida per i cittadini",
                ["GuideForSMEs"] = "Guida per le PMI",
                ["Guidelines1"] = "Linee guida",
                ["Guidelines2"] = "Provvedimenti generali",
                ["Help"] = "Aiuto",
                ["International"] = "Atti internazionali",
                ["InternationalCaseLaw"] = "Giurisprudenza internazionale",
                ["InternationalTreaties"] = "Trattati internazionali",
                ["Italy"] = "Italia",
                ["Language"] = "Lingua",
                ["LegalLibrary"] = "Biblioteca legale",
                ["Legislation"] = "Legislazioni",
                ["MainPageCaption"] = "Pagina principale",
                ["MainPageFooter"] = "Il progetto SMEDATA e finanziato dal programma UE per i diritti, l'uguaglianza e la cittadinanza (2014-2020)",
                ["Menu"] = "Menu",
                ["National"] = "Atti nazionali",
                ["NationalCaseLaw"] = "Giurisprudenza nazionale",
                ["NationalGuidelines"] = "Linee guida nazionali",
                ["Next"] = "Avanti",
                ["Opinions"] = "Pareri",
                ["OtherCourts"] = "Altri tribunali",
                ["GuideForSMEsInfoText"] = "Fornisce una guida pratica sugli obblighi imposti alle PMI dal GDPR e come i cittadini possono esercitare i propri diritti",
                ["Prev"] = "Indietro",
                ["ReloadBookmarks"] = "Ricarica segnalibri",
                ["SavedDocuments"] = "Documenti salvati",
                ["SaveOffline"] = "Salva offline",
                ["Search"] = "Cerca",
                ["SearchInDocuments"] = "Cerca nei documenti",
                ["SelectedEntry"] = "Voce selezionata",
                ["Settings"] = "Impostazioni",
                ["SmeDataIntroText"] = "Questa app e stata sviluppata dal Progetto SMEDATA. Il progetto mira a garantire il massimo livello di privacy e protezione dei dati personali attraverso strumenti innovativi per le PMI e i cittadini.",
                ["SupremeAdministrativeCourt"] = "Tribunale amministrativo supremo",
                ["AdministrativeCourtSofia"] = "Tribunale amministrativo della città di Sofia",
                ["TrainingMaterials"] = "Materiali formativi",
                ["Update"] = "Aggiorna",
                ["UpdateAllDocuments"] = "Aggiorna tutti i documenti",
                ["UsefulLinks"] = "Collegamenti utili",
                ["Welcome"] = "Benvenuti",
                ["GuideForCitizensInfoText"] = "Fornisce una guida pratica sulla protezione dei dati personali ai sensi del GDPR ed in che modo i cittadini possono esercitare i propri diritti",
                ["LegalFrameworkInfoText"] = "Fornisce accesso alla normativa dell’UE, agli atti normativi nazionali (Bulgaria ed Italia) ed al diritto internazionale nel settore della protezione dei dati personali",
                ["CaseLawDataProtectionDirective"] = "Giurisprudenza relativa alla Direttiva sulla protezione dei dati personali",
                ["CaseLawDirective2016/681"] = "Giurisprudenza relativa alla Direttiva 2006/43/CE",
                ["CaseLawEUInstitutionsDataProtectionRegulation"] = "Giurisprudenza sul Regolamento relativo alla protezione delle persone fisiche con riguardo al trattamento dei dati personali da parte delle istituzioni dell'UE",
                ["CaseLawDirective2002/58/EC"] = "Giurisprudenza relativa alla Direttiva 2002/58/CE",
                ["ConstitutionalCourt"] = "Corte Costituzionale",
                ["SupremeCourtOfCassation"] = "Corte Suprema di Cassazione",
                ["Recitals"] = "Considerando",
                ["Citations"] = "Visti",
                ["Preamble"] = "Preambolo",
                ["Articles"] = "Articoli",
                ["Title"] = "Titolo",
                ["About"] = "Sull’applicazione",
                ["KeyInstruments"] = "Principali atti",
                ["OtherInstruments"] = "Altri atti",
                ["AdequacyDecisions"] = "Decisioni di adeguatezza",
                ["SchengenAcquis"] = "Acquis di Schengen",
                ["RepealedInstruments"] = "Atti abrogati",
                ["Treaties"] = "Trattati",
                ["31995L0046"] = "95/46/CE",
                ["Error"] = "Errore",
                ["Message"] = "Message",
                ["Оk"] = "OK",
                ["WifiSettingsOption"] = "Caricare documenti solo con connettività Wifi",
                ["No Internet Access"] = "No Internet Access",
                ["No Wifi Access"] = "Non sei collegato alla rete Wifi. Se vuoi aprire documenti non solo quando sei connesso a una rete Wifi, disabilita questa opzione da Impostazioni.",
                ["Yes"] = "Si",
                ["No"] = "No",
                ["UpdateDocumentExist"] = "È disponibile aggiornamento di(dei) documento(i) salvato(i) sul dispositivo. Volete aprire “I documenti salvati” per aggiornare il(i) documento(i)?",
                ["Warning"] = "Avvertimento",
                ["CurrentAppVersion"] = "Versione: ",
                ["OtherDocuments"] = "Altri documenti",
                ["DocumentIsSaved"] = "Il documento è stato salvato",
                ["DocumentIsUpdated"] = "Il documento è stato aggiornato",
                ["DocumentIsAlreadySaved"] = "Il documento è già stato salvato. Vuoi aggiornarlo?",
                ["SelectLanguage"] = "Seleziona la lingua",
                ["UserDocuments"] = "Documenti dell'utente",
                ["SystemDocuments"] = "Documenti di sistema",
            };
            this.transLanguages[SmeLanguage.Italian] = new TranslationLanguage(itDict);
        }

        private void AddBgTranslation()
        {
            var bgDict = new Dictionary<string, string>()
            {
                ["Bookmarks"] = "Отметки",
                ["Bulgaria"] = "България",
                ["CaseLaw"] = "Съдебна практика",
                ["ContactNationalDPA"] = "Контакт с националните надзорни органи",
                ["Decisions"] = "Решения",
                ["DecisionSupportTool"] = "Инструменти за подпомагане вземането на решения",
                ["DocumentContent"] = "Съдържание на документа",
                ["Erase"] = "Изтрий",
                ["EUlegalframework"] = "Правна рамка на ЕС",
                ["EuropeanDataProtectionBoard"] = "Европейски комитет по защита на данните",
                ["EuropeanDataProtectionSupervisor"] = "Европейски надзорен орган по защита на данните",
                ["EuropeanUnion"] = "Европейски съюз",
                ["FAQ"] = "Често задавани въпроси",
                ["GdprDictionary"] = "GDPR речник",
                ["GDPRMadeSimple"] = "GDPR представен достъпно",
                ["GdprPocket"] = "GDPR в твоя джоб",
                ["GuideForCitizens"] = "Ръководство за граждани",
                ["GuideForSMEs"] = "Ръководство за МСП",
                ["Guidelines1"] = "Насоки",
                ["Guidelines2"] = "Насоки",
                ["Help"] = "Помощ",
                ["International"] = "Международни актове",
                ["InternationalCaseLaw"] = "Международна съдебна практика",
                ["InternationalTreaties"] = "Международни договори",
                ["Italy"] = "Италия",
                ["Language"] = "Език",
                ["LegalLibrary"] = "Правна библиотека",
                ["Legislation"] = "Законодателство",
                ["MainPageCaption"] = "Основна страница",
                ["MainPageFooter"] = "Проектът SMEDATA се финансира от Програма \"Права, равенство и гражданство\" на ЕС (2014-2020)",
                ["Menu"] = "Меню",
                ["National"] = "Национални актове",
                ["NationalCaseLaw"] = "Национална съдебна практика",
                ["NationalGuidelines"] = "Национални насоки",
                ["Next"] = "Следващ",
                ["Opinions"] = "Становища",
                ["OtherCourts"] = "Други съдилища",
                ["GuideForSMEsInfoText"] = "Предоставя практически насоки относно задълженията на МСП съгласно GDPR и как да се постигне съвместимост с изискванията на регламента",
                ["Prev"] = "Предишен",
                ["ReloadBookmarks"] = "Презареди отметките",
                ["SavedDocuments"] = "Запазени документи",
                ["SaveOffline"] = "Запази офлайн",
                ["Search"] = "Търсене",
                ["SearchInDocuments"] = "Търсене в документите",
                ["SelectedEntry"] = "Избран запис",
                ["Settings"] = "Настройки",
                ["SmeDataIntroText"] = "Това приложение е разработено по Проект SMEDATA. Проектът цели осигуряването на най-висока степен на защита на неприкосновеността на личния живот и защита на личните данни чрез иновативни инструменти за малките и средни предприятия и гражданите.",
                ["SupremeAdministrativeCourt"] = "Върховен административен съд",
                ["AdministrativeCourtSofia"] = "Административен съд София-град",
                ["TrainingMaterials"] = "Материали за обучение",
                ["Update"] = "Актуализиране",
                ["UpdateAllDocuments"] = "Актуализирай всички документи",
                ["UsefulLinks"] = "Полезни връзки",
                ["Welcome"] = "Добре дошли",
                ["GuideForCitizensInfoText"] = "Предоставя практически насоки относно защитата на личните данни съгласно GDPR и как гражданите могат да упражнят своите права",
                ["LegalFrameworkInfoText"] = "Предоставя достъп до актове на Правото на ЕС, национални (България и Италия) и международни правни актове в областта на защитата на личните данни",
                ["CaseLawDataProtectionDirective"] = "Съдебна практика по Директивата за защита на личните данни",
                ["CaseLawDirective2016/681"] = "Съдебна практика по Директива 2016/681",
                ["CaseLawEUInstitutionsDataProtectionRegulation"] = "Съдебна практика по Регламента относно защитата на физическите лица във връзка с обработването на лични данни от институциите на ЕС",
                ["CaseLawDirective2002/58/EC"] = "Съдебна практика по Директива 2002/58/ЕО",
                ["ConstitutionalCourt"] = "Конституционен съд",
                ["SupremeCourtOfCassation"] = "Върховен касационен съд",
                ["Recitals"] = "Съображения",
                ["Citations"] = "Позовавания",
                ["Preamble"] = "Преамбюл",
                ["Articles"] = "Членове ",
                ["Title"] = "Заглавие ",
                ["About"] = "За приложението",
                ["KeyInstruments"] = "Основни актове",
                ["OtherInstruments"] = "Други актове",
                ["AdequacyDecisions"] = "Решения относно адекватното ниво на защита",
                ["SchengenAcquis"] = "Шенгенско законодателство",
                ["RepealedInstruments"] = "Отменени актове",
                ["Treaties"] = "Договори",
                ["31995L0046"] = "95/46/ЕО",
                ["Error"] = "Грешка",
                ["Message"] = "Съобщение",
                ["Оk"] = "Да",
                ["WifiSettingsOption"] = "Свалай документи само при Wifi свързаност",
                ["No Internet Access"] = "Липсва достъп до интернет",
                ["No Wifi Access"] = "Не сте свързани към Wifi мрежа. Ако желаете да отваряте документи не само, когато се свързан към Wifi мрежа, изключете тази настройка от \"Настройки\".",
                ["Yes"] = "Да",
                ["No"] = "Не",
                ["UpdateDocumentExist"] = "Налична е актуализация на документ/и, който сте запазили на Вашето устройство. Искате ли да отворите \"Запазени документи\", за да актуализирате документа/ите?",
                ["Warning"] = "Предупреждение",
                ["CurrentAppVersion"] = "Версия: ",
                ["OtherDocuments"] = "Други документи",
                ["DocumentIsSaved"] = "Документтът е запазен",
                ["DocumentIsUpdated"] = "Документът е актуализиран",
                ["DocumentIsAlreadySaved"] = "Документът вече е запазен. Искате ли да го актуализирате?",
                ["SelectLanguage"] = "Изберете език",
                ["UserDocuments"] = "Потребителски документи",
                ["SystemDocuments"] = "Системни документи",
            };
            this.transLanguages[SmeLanguage.Bulgarian] = new TranslationLanguage(bgDict);
        }

        public string GetString(string text, CultureInfo culture)
        {
            var lang = GetLangByCulture(culture);
            if (this.transLanguages.ContainsKey(lang))
            {
                return this.transLanguages[lang].GetString(text);
            }

            return text;
        }

        public static SmeLanguage GetLangByCulture(CultureInfo cultureInfo)
        {
            switch (cultureInfo.TwoLetterISOLanguageName)
            {
                case "it": return SmeLanguage.Italian;
                case "bg": return SmeLanguage.Bulgarian;
                case "de": return SmeLanguage.German;
                case "fr": return SmeLanguage.French;
                default: return SmeLanguage.English;
            }
        }
    }

    class TranslationLanguage
    {
        public SmeLanguage Language { get; set; }
        private Dictionary<string, string> transDict;
        public TranslationLanguage(Dictionary<string, string> translations)
        {
            this.transDict = translations;
        }

        public string GetString(string key)
        {
            if (this.transDict.TryGetValue(key, out string res))
            {
                return res;
            }
            else
            {
                return key;
            }
        }
    }
}

