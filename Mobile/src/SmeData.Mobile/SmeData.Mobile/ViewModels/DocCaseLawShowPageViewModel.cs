using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;
using SmeData.Mobile.Data.Models;
using SmeData.Mobile.Helpers;
using SmeData.Mobile.Models.Settings;
using SmeData.Mobile.Services;
using SmeData.Shared.Common;
using SmeData.SharedModels.Document;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Newtonsoft.Json;
using SmeData.Mobile.Data;

namespace SmeData.Mobile.ViewModels
{
    /// <summary>
    /// Page for showing case law documents
    /// </summary>
    public class DocCaseLawShowPageViewModel : BaseShowPageViewModel
    {
        public DocCaseLawShowPageViewModel(INavigationService navigationService, IPageDialogService dialogService, DocumentService docService, SettingsModel settings, HttpService httpService, AppRepository documentsRepository) : base(navigationService, dialogService, docService, settings, httpService, documentsRepository)
        {
        }
    }
}
