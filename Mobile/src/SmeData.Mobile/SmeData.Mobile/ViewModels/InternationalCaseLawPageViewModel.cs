using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;
using SmeData.Mobile.Data;
using SmeData.Mobile.Models.Settings;
using SmeData.Mobile.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SmeData.Mobile.ViewModels
{
    /// <summary>
    /// PAge with international case law documents
    /// </summary>
    public class InternationalCaseLawPageViewModel : CommonDocListViewPageViewModel
    {
        public InternationalCaseLawPageViewModel(HttpService httpService, INavigationService navigationService, IPageDialogService dialogService, SettingsModel settings, AppRepository documentsRepository) : base(httpService, navigationService, dialogService, settings, documentsRepository)
        {
            GetEuDocsByClassifier("6D981719-977C-4A7F-ADE1-9B6E0E8852D6");
        }
    }
}
