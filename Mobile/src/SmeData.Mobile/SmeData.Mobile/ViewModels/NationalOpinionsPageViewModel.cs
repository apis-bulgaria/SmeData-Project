﻿using Prism.Commands;
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
    /// Page with opinions, related to GDPR, form Bulgaria or Italy
    /// </summary>
    public class NationalOpinionsPageViewModel : CommonDocListViewPageViewModel
    {
        public NationalOpinionsPageViewModel(HttpService httpService, INavigationService navigationService, IPageDialogService dialogService, SettingsModel settings, AppRepository documentsRepository) : base(httpService, navigationService, dialogService, settings, documentsRepository)
        {
        }
    }
}
