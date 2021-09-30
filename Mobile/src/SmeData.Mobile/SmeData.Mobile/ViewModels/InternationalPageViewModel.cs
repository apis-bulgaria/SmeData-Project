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
    /// <summary>
    /// Tabbed page for international treaties and case law documents related to GDPR
    /// </summary>
    public class InternationalPageViewModel : BaseViewModel
    {
        public InternationalPageViewModel(INavigationService navigationService) : base(navigationService)
        {
        }
    }
}
