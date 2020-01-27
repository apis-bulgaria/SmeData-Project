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
    /// Common page for showing document
    /// </summary>
    public class DocCommonShowPageViewModel : BaseShowPageViewModel
    {
        public DocCommonShowPageViewModel(INavigationService navigationService, IPageDialogService dialogService, DocumentService docService, SettingsModel settings, HttpService httpService, AppRepository documentsRepository) : base(navigationService, dialogService, docService, settings, httpService, documentsRepository)
        {
        }
    }
}
