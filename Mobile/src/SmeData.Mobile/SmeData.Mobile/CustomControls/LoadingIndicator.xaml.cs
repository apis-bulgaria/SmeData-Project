using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SmeData.Mobile.CustomControls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoadingIndicator
    {
        public static readonly BindableProperty IsLoadingProperty = BindableProperty.Create(nameof(IsLoading),
           typeof(bool),
           typeof(LoadingIndicator));

        public LoadingIndicator()
        {
            InitializeComponent();
        }

        public bool IsLoading
        {
            get => (bool)GetValue(IsLoadingProperty);
            set => SetValue(IsLoadingProperty, value);
        }

        protected override void OnPropertyChanged(string propertyName)
        {
            base.OnPropertyChanged(propertyName);

            if (propertyName == IsLoadingProperty.PropertyName)
            {
                IsVisible = IsLoading;
                IsRunning = IsLoading;
            }
        }
    }
}