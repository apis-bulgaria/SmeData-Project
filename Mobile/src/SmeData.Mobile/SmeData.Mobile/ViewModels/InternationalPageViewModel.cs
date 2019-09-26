using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;
using SmeData.Mobile.Models.Settings;
using SmeData.Mobile.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SmeData.Mobile.ViewModels
{
    public class InternationalPageViewModel : BaseViewModel
    {
        public InternationalPageViewModel(INavigationService navigationService) : base(navigationService)
        {
        }
    }
}
