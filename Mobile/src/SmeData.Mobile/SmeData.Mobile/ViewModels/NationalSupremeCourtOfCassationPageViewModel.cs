﻿using Prism.Navigation;
using Prism.Services;
using SmeData.Mobile.Data;
using SmeData.Mobile.Models.Settings;
using SmeData.Mobile.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmeData.Mobile.ViewModels
{
    /// <summary>
    ///  Page with document, related to GDPR, from italian Supreme Court of Cassation
    /// </summary>
    public class NationalSupremeCourtOfCassationPageViewModel : CommonDocListViewPageViewModel
    {
        public NationalSupremeCourtOfCassationPageViewModel(HttpService httpService, INavigationService navigationService, IPageDialogService dialogService, SettingsModel settings, AppRepository documentsRepository) : base(httpService, navigationService, dialogService, settings, documentsRepository)
        {
        }
    }
}
