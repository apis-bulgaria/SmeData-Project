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
                }

                this.transLanguages[SmeLanguage.English] = new TranslationLanguage(enDict);
                this.transLanguages[SmeLanguage.Bulgarian] = new TranslationLanguage(bgDict);
                this.transLanguages[SmeLanguage.Italian] = new TranslationLanguage(itDict);
            }
            catch (Exception)
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
                ["ContactNationalDPA"] = "Contact with national DPAs",
                ["Decisions"] = "Decisions",
                ["DecisionSupportTool"] = "Decision support tool",
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
                ["GuideForCitizensInfoText"] = "Provides practical guidance on personal data protection under the GDPR and how citizens can exercise their rights",
                ["GuideForSMEs"] = "Guide for SMEs",
                ["GuideForSMEsInfoText"] = "Provides practical guidance on the obligations placed on SMEs under the GDPR and how to get GDPR-compliant",
                ["Guidelines"] = "Guidelines",
                ["Help"] = "Help",
                ["International"] = "International",
                ["InternationalCaseLaw"] = "International case law",
                ["InternationalTreaties"] = "International treaties",
                ["Italy"] = "Italy",
                ["Language"] = "Language",
                ["LegalFrameworkInfoText"] = "Provides access to EU’s, national (in Bulgaria and Italy) and international legal acts in the field of personal data protection",
                ["LegalLibrary"] = "Legal library",
                ["Legislation"] = "Legislation",
                ["MainPageCaption"] = "Main page",
                ["MainPageFooter"] = "SMEDATA Project is funded by the EU's Rights, Equality and Citizenship Programme (2014-2020)",
                ["Menu"] = "Menu",
                ["National"] = "National",
                ["NationalCaseLaw"] = "National case law",
                ["NationalGuidelines"] = "National guidelines",
                ["Next"] = "Next",
                ["Opinions"] = "Opinions",
                ["OtherCourts"] = "Other courts",
                ["PagesInfoText"] = "Provide practical guidance on GDPR rules to SME.Consist of the fallowing components...",
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
                ["TrainingMaterials"] = "Training materials",
                ["Update"] = "Update",
                ["UpdateAllDocuments"] = "Update all documents",
                ["UsefulLinks"] = "Useful links",
                ["Welcome"] = "Welcome",
                ["Recitals"] = "Recitals",
                ["Articles"] = "Articles",
                ["KeyInstruments"] = "Key Instruments",
                ["Treaties"] = "Treaties",
                ["OtherInstruments"] = "Other Instruments",
                ["SchengenAcquis"] = "Schengen Acquis",
                ["AdequacyDecisions"] = "Adequacy Decisions",
                ["RepealedInstruments"] = "Repealed Instruments",
                ["Error"] = "Error",
                ["Message"]= "Message",
                ["Оk"]= "ОК",
                ["Warning"] = "Warning",
                ["The document is saved"] = "The document is saved",
                ["UpdateDocumentExist"] = "There is an update of document(s) that you have saved on your device. Do you want to open \"Saved documents\" to update the document(s)?"
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
                ["ContactNationalDPA"] = "Contatto con i DPA nazionali",
                ["Decisions"] = "Decisioni",
                ["DecisionSupportTool"] = "Strumento di supporto decisionale",
                ["DocumentContent"] = "Contenuto del documento",
                ["Erase"] = "Cancella",
                ["EUlegalframework"] = "EU quadro giuridico",
                ["EuropeanDataProtectionBoard"] = "Consiglio europeo per la protezione dei dati",
                ["EuropeanDataProtectionSupervisor"] = "Supervisore europeo per la protezione dei dati",
                ["EuropeanUnion"] = "Unione Europea",
                ["FAQ"] = "Domande frequenti",
                ["GdprDictionary"] = "Dizionario GDPR",
                ["GDPRMadeSimple"] = "GDPR reso semplice",
                ["GdprPocket"] = "GDPR in tasca",
                ["GuideForCitizens"] = "Guida per i cittadini",
                ["GuideForCitizensInfoText"] = "Provides practical guidance on personal data protection under the GDPR and how citizens can exercise their rights",
                ["GuideForSMEs"] = "Guida per le PMI",
                ["GuideForSMEsInfoText"] = "Provides practical guidance on the obligations placed on SMEs under the GDPR and how to get GDPR-compliant",
                ["Guidelines"] = "Linee guida",
                ["Help"] = "Aiuto",
                ["International"] = "Internazionale",
                ["InternationalCaseLaw"] = "Giurisprudenza internazionale",
                ["InternationalTreaties"] = "Trattati internazionali",
                ["Italy"] = "Italia",
                ["Language"] = "Lingua",
                ["LegalFrameworkInfoText"] = "Provides access to EU’s, national (in Bulgaria and Italy) and international legal acts in the field of personal data protection",
                ["LegalLibrary"] = "Biblioteca legale",
                ["Legislation"] = "Legislazioni",
                ["MainPageCaption"] = "Pagina principale",
                ["MainPageFooter"] = "Il progetto SMEDATA è finanziato dal programma UE per i diritti, l'uguaglianza e la cittadinanza (2014-2020)",
                ["Menu"] = "Menu",
                ["National"] = "Nazionale",
                ["NationalCaseLaw"] = "Giurisprudenza nazionale",
                ["NationalGuidelines"] = "Linee guida nazionali",
                ["Next"] = "Avanti",
                ["Opinions"] = "Opinioni",
                ["OtherCourts"] = "Altri tribunali",
                ["PagesInfoText"] = "Fornire indicazioni pratiche sulle regole del GDPR alle PMI. Sono costituiti dai seguenti componenti ...",
                ["Prev"] = "Indietro",
                ["ReloadBookmarks"] = "Ricarica segnalibri",
                ["SavedDocuments"] = "Documenti salvati",
                ["SaveOffline"] = "Salva offline",
                ["Search"] = "Cerca",
                ["SearchInDocuments"] = "Cerca nei documenti",
                ["SelectedEntry"] = "Voce selezionata",
                ["Settings"] = "Impostazioni",
                ["SmeDataIntroText"] = "Il progetto SMEDATA mira a garantire il massimo livello di privacy e protezione dei dati personali attraverso strumenti innovativi per le PMI e i cittadini.",
                ["SupremeAdministrativeCourt"] = "Tribunale amministrativo supremo",
                ["TrainingMaterials"] = "Materiali formativi",
                ["Update"] = "Aggiorna",
                ["UpdateAllDocuments"] = "Collegamenti utili",
                ["UsefulLinks"] = "Collegamenti utili",
                ["Welcome"] = "Benvenuti",
                ["Recitals"] = "Considerando",
                ["Articles"] = "Articoli",
                ["KeyInstruments"] = "Strumenti chiave",
                ["Treaties"] = "Trattati",
                ["OtherInstruments"] = "Altri strumenti",
                ["SchengenAcquis"] = "Аcquis di Schengen",
                ["AdequacyDecisions"] = "Decisioni di adeguatezza",
                ["RepealedInstruments"] = "Strumenti abrogati",
                ["Error"] = "Еrrore",
                ["Message"] = "Message",
                ["Оk"] = "ОК",
                ["Warning"] = "Avvertimento",
                ["The document is saved"] = "Il documento è stato salvato",
                ["UpdateDocumentExist"] = "È disponibile aggiornamento di(dei) documento(i) salvato(i) sul dispositivo. Volete aprire “I documenti salvati” per aggiornare il(i) documento(i)?"
            };
            this.transLanguages[SmeLanguage.Italian] = new TranslationLanguage(itDict);
        }

        private void AddBgTranslation()
        {
            var bgDict = new Dictionary<string, string>()
            {
                ["Bookmarks"] = "Отбелязвания",
                ["Bulgaria"] = "България",
                ["CaseLaw"] = "Съдебна практика",
                ["ContactNationalDPA"] = "Контакт с националните надзорни органи",
                ["Decisions"] = "Решения",
                ["DecisionSupportTool"] = "Инструмент за подпомагане вземането на решения",
                ["DocumentContent"] = "Съдържание на документа",
                ["Erase"] = "Изтриване",
                ["EUlegalframework"] = "ЕС правна рамка",
                ["EuropeanDataProtectionBoard"] = "Европейски комитет по защита на данните",
                ["EuropeanDataProtectionSupervisor"] = "Европейски надзорен орган по защита на данните",
                ["EuropeanUnion"] = "Европейски съюз",
                ["FAQ"] = "Често задавани въпроси",
                ["GdprDictionary"] = "GDPR речник",
                ["GDPRMadeSimple"] = "GDPR представен достъпно",
                ["GdprPocket"] = "GDPR в твоя джоб",
                ["GuideForCitizens"] = "Ръководство за граждани",
                ["GuideForCitizensInfoText"] = "Предоставя практически насоки относно защитата на личните данни съгласно GDPR и как гражданите могат да упражнят своите права",
                ["GuideForSMEs"] = "Ръководство за МСП",
                ["GuideForSMEsInfoText"] = "Предоставя практически насоки относно задълженията на МСП съгласно GDPR и как да се постигне съвместимост с изискванията на регламента",
                ["Guidelines"] = "Насоки",
                ["Help"] = "Помощ",
                ["International"] = "Международни актове",
                ["InternationalCaseLaw"] = "Международна съдебна практика",
                ["InternationalTreaties"] = "Международни договори",
                ["Italy"] = "Италия",
                ["Language"] = "Език",
                ["LegalFrameworkInfoText"] = "Предоставя достъп до актове на Правото на ЕС, национални (в България и Италия) и международни правни актове в областта на защитата на личните данни",
                ["LegalLibrary"] = "Правна библиотека",
                ["Legislation"] = "Законодателство",
                ["MainPageCaption"] = "Основна страница",
                ["MainPageFooter"] = "Проектът SMEDATA се финансира от програмата на ЕС \"Права, равенство и гражданство\" (2014-2020)",
                ["Menu"] = "Меню",
                ["National"] = "Национални актове",
                ["NationalCaseLaw"] = "Национална съдебна практика",
                ["NationalGuidelines"] = "Национални насоки",
                ["Next"] = "Следващ",
                ["Opinions"] = "Становища",
                ["OtherCourts"] = "Други съдилища",
                ["PagesInfoText"] = "Предоставя практически насоки относно правилата на GDPR за МСП. Състои се от следните компоненти...",
                ["Prev"] = "Предишен",
                ["ReloadBookmarks"] = "Презареди отбелязванията",
                ["SavedDocuments"] = "Запазени документи",
                ["SaveOffline"] = "Запази офлайн",
                ["Search"] = "Търсене",
                ["SearchInDocuments"] = "Търсене в документите",
                ["SelectedEntry"] = "Избран запис",
                ["Settings"] = "Настройки",
                ["SmeDataIntroText"] = "Това приложение е разработено по Проект SMEDATA. Проектът цели осигуряването на най-висока степен на защита на неприкосновеността на личния живот и защита на личните данни чрез иновативни инструменти за малките и средни предприятия и гражданите.",
                ["SupremeAdministrativeCourt"] = "Върховен административен съд",
                ["TrainingMaterials"] = "Материали за обучение",
                ["Update"] = "Актуализиране",
                ["UpdateAllDocuments"] = "Актуализирай всички документи",
                ["UsefulLinks"] = "Полезни връзки",
                ["Welcome"] = "Добре дошли",
                ["Recitals"] = "Съображения",
                ["Articles"] = "Членове",
                ["KeyInstruments"] = "Основни инструменти",
                ["Treaties"] = "Договори",
                ["OtherInstruments"] = "Други инструменти",
                ["SchengenAcquis"] = "Шенгенско законодателство",
                ["AdequacyDecisions"] = "Решения за адекватност",
                ["RepealedInstruments"] = "Отменени инструменти",
                ["About"] = "Относно",
                ["Error"] = "Грешка",
                ["Message"] = "Съобщение",
                ["Оk"] = "ДА",
                ["Warning"] = "Предупреждение",
                ["The document is saved"] = "Документът е запазен",
                ["UpdateDocumentExist"] = "Налична е актуализация на документ/и, който сте запазили на Вашето устройство. Искате ли да отворите \"Запазени документи\", за да актуализирате документа/ите?"
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

