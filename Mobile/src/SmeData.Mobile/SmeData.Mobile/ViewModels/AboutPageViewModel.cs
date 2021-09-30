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
using System.Windows.Input;
using Xamarin.Essentials;

namespace SmeData.Mobile.ViewModels
{
    /// <summary>
    /// Page with information about the application
    /// </summary>
    public class AboutPageViewModel : BaseShowPageViewModel
    {
        public ICommand VersionTapCommand { get; set; }

        /// <summary>
        /// Gets the curent version of the application
        /// </summary>
        public string AppVersion { get => VersionTracking.CurrentVersion; }

        public AboutPageViewModel(INavigationService navigationService, IPageDialogService dialogService, DocumentService docService, SettingsModel settings, HttpService httpService, AppRepository documentsRepository) : base(navigationService, dialogService, docService, settings, httpService, documentsRepository)
        {
            VersionTracking.Track();
            this.VersionTapCommand = new DelegateCommand(this.ShowGlobalLog);
        }

        private void ShowGlobalLog()
        {
            dialogService.DisplayAlertAsync("Global log", globalLog, "Ok");
        }
    }
}
