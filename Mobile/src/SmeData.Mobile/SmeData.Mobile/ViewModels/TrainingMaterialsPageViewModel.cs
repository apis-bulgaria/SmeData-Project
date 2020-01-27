using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using SmeData.Mobile.Models.Settings;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SmeData.Mobile.ViewModels
{
    /// <summary>
    /// Page with training materials
    /// </summary>
    public class TrainingMaterialsPageViewModel : BaseViewModel
    {
        public string HtmlString { get; set; }
        public TrainingMaterialsPageViewModel(INavigationService navigationService) : base(navigationService)
        {

        }
    }
}
