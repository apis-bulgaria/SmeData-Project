﻿using Prism.Navigation;
using Prism.Services;
using SmeData.Mobile.Data;
using SmeData.Mobile.Models.Settings;
using SmeData.Mobile.Services;

namespace SmeData.Mobile.ViewModels
{
    public class NationalCourtOfCassationPageViewModel : CommonDocListViewPageViewModel
    {
        public NationalCourtOfCassationPageViewModel(HttpService httpService, INavigationService navigationService, IPageDialogService dialogService, SettingsModel settings, AppRepository documentsRepository) : base(httpService, navigationService, dialogService, settings, documentsRepository)
        {
        }
    }
}
