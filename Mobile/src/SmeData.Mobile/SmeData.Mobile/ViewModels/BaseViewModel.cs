using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Xamarin.Forms;
using SmeData.Mobile.Models;
using SmeData.Mobile.Services;
using Prism.Mvvm;
using Prism.Navigation;
using SmeData.Mobile.Data;
using SmeData.Mobile.Data.Models;
using System.Windows.Input;
using Prism.Commands;
using SmeData.Mobile.Models.Settings;
using Xamarin.Essentials;

namespace SmeData.Mobile.ViewModels
{
    /// <summary>
    /// Base viewModel with common elements for all viewModels.
    /// </summary>
    public class BaseViewModel : BindableBase, INavigationAware
    {
        public static string globalLog = string.Empty;

        public ICommand HomePageCommand { get; set; }
        protected INavigationService navigationService;
        protected SettingsModel settings;
        public readonly double screenWidth = DeviceDisplay.MainDisplayInfo.Width;

        /// <summary>
        /// Gets the width of the screen of the device
        /// </summary>
        public double ScreenWidth { get => screenWidth; }

        public virtual void OnNavigatedFrom(INavigationParameters parameters)
        { }

        public virtual void OnNavigatedTo(INavigationParameters parameters)
        { }

        public virtual void OnNavigatingTo(INavigationParameters parameters)
        { }

        public BaseViewModel()
        {
        }

        public BaseViewModel(INavigationService navigationService) : this()
        {
            this.navigationService = navigationService;
            this.HomePageCommand = new DelegateCommand(this.ShowHomePage);
        }

        /// <summary>
        /// Navigates to home page
        /// </summary>
        private void ShowHomePage()
        {
            //this.navigationService.NavigateAsync("WelcomePage");
            this.navigationService.GoBackToRootAsync();
        }
    }
}
