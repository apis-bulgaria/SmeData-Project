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
    /// Page with international treties
    /// </summary>
    public class InternationalTreatiesPageViewModel : CommonDocListViewPageViewModel
    {
        public InternationalTreatiesPageViewModel(HttpService httpService, INavigationService navigationService, IPageDialogService dialogService, SettingsModel settings, AppRepository documentsRepository) : base(httpService, navigationService, dialogService, settings, documentsRepository)
        {
            GetEuDocsByClassifier("A653D24E-0AA1-449A-AE3E-973D25FE6137");
        }
    }
}
