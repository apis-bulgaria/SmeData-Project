using Prism.Navigation;
using SmeData.Mobile.Services;
using SmeData.SharedModels.Language;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using Xamarin.Essentials;

namespace SmeData.Mobile.Helpers
{
    public static class UrlNavHelper
    {
        public static readonly string DOC_NUM = "docNum";
        public static readonly string IDENTIFIER = "ident";
        public static readonly string SEARCH_TEXT = "searchText";
        public static readonly string TO_PAR = "ToPar";
        public static readonly string CLASSIFIER = "classifier";
        public static readonly string FULL_TITLE = "fullTitle";
        public static readonly string IS_OFFLINE = "isOffline";
        public static readonly string GO_NEXT = "docgonext";
        public static readonly string GO_PREV = "docgoprev";


        public static string GetEurlexCountryByLanguage(SmeLanguage lang)
        {
            switch (lang)
            {
                case SmeLanguage.Bulgarian: return "BG";
                case SmeLanguage.English: return "EN";
                case SmeLanguage.Italian: return "IT";
                case SmeLanguage.German: return "DE";
                case SmeLanguage.French: return "FR";
                default: return "EN";
            }
        }

        public static bool ShouldCancelNavigatton(string url)
        {
            return (Regex.IsMatch(url, @"Guid=(.*)", RegexOptions.IgnoreCase) || Regex.IsMatch(url, @"Celex=(.*)", RegexOptions.IgnoreCase)
                || Regex.IsMatch(url, @"static_file(.*)", RegexOptions.IgnoreCase)
                || Regex.IsMatch(url, $"{GO_PREV}", RegexOptions.IgnoreCase)
                || Regex.IsMatch(url, $"{GO_NEXT}", RegexOptions.IgnoreCase)
                || Regex.IsMatch(url, @"\/Base=.*", RegexOptions.IgnoreCase)
                || url.ToLower().StartsWith("http://")
                || url.ToLower().StartsWith("https://")
                );
        }

        public static void ProcessNavigation(string url, INavigationService navigationService, HttpService httpService)
        {
            Match m = Regex.Match(url, @"Guid=(.*)", RegexOptions.IgnoreCase);
            if (m.Success)
            {
                navigationService.NavigateAsync($"GdpMadeSimplePage?{UrlNavHelper.IDENTIFIER}={m.Groups[1].Value}");
                return;
            }

            m = Regex.Match(url, @"\/(Base=.*)", RegexOptions.IgnoreCase);
            if (m.Success)
            {
                var navUrl = $"https://web.apis.bg/p.php?redir=0&{m.Groups[1].Value}";
                Browser.OpenAsync(navUrl);
                return;
            }

            m = Regex.Match(url, @"Celex=(.*)", RegexOptions.IgnoreCase);
            if (m.Success)
            {
                string celex = m.Groups[1].Value;
                if (celex?.StartsWith("6") == true) {
                    navigationService.NavigateAsync($"DocCaseLawShowPage?{UrlNavHelper.DOC_NUM}={celex}");
                }
                else
                {
                    navigationService.NavigateAsync($"DocMainPage?{UrlNavHelper.DOC_NUM}={celex}");
                } 

                return;
            }

            m = Regex.Match(url, @"static_file(.*)", RegexOptions.IgnoreCase);
            if (m.Success)
            {
                var urlParams = m.Value.Split('/');
                if (urlParams.Length > 1)
                {
                    var fileUrl = httpService.GetStaticFileContentUrl(urlParams[1], urlParams[2]);
                    Browser.OpenAsync(fileUrl, BrowserLaunchMode.SystemPreferred);
                }

                return;
            }

            //regex for proper http or https format
            if (!url.ToLower().StartsWith(@"file:///") && !url.ToLower().StartsWith(@"ms-appx-web:///"))
            {
                Browser.OpenAsync(url, BrowserLaunchMode.SystemPreferred);
            }
        }
    }
}
