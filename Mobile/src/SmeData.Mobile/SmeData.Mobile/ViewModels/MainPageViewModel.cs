using Prism.Commands;
using Prism.Navigation;
using SmeData.Mobile.Helpers;
using SmeData.Mobile.Models.Settings;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace SmeData.Mobile.ViewModels
{
    /// <summary>
    /// Main page of the application contaning side menu
    /// </summary>
    public class MainPageViewModel : BaseViewModel
    {
        public DelegateCommand<string> NavigateCommand { get; set; }
        public DelegateCommand HelpPageCommand { get; set; }
        public DelegateCommand AboutPageCommand { get; set; }

        public string MenuItemFont { get => $"t|18|{ScreenWidth}"; }

        public string MenuRowHeight { get => $"t|34|{ScreenWidth}"; }

        public MainPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            this.NavigateCommand = new DelegateCommand<string>(this.ShowPage);
            this.HelpPageCommand = new DelegateCommand(this.ShowHelpPage);
            this.AboutPageCommand = new DelegateCommand(this.ShowAboutPage);
        }

        private void ShowAboutPage()
        {
            this.navigationService.NavigateAsync($"NavigationPage/WelcomePage/AboutPage?{UrlNavHelper.DOC_NUM}=about");
        }

        private void ShowHelpPage()
        {
            this.navigationService.NavigateAsync($"NavigationPage/WelcomePage/HelpPage?{UrlNavHelper.DOC_NUM}=help");
        }

        private async void ShowPage(string pageName)
        {
            await navigationService.NavigateAsync($"NavigationPage/WelcomePage/{pageName}");
        }
    }
}
