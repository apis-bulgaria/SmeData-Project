using SmeData.Mobile.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SmeData.Mobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CaseLawPage : ContentPage
    {
        public CaseLawPage()
        {
            InitializeComponent();
        }

        private void OnGroupHeaderCellBindingContextChanged(object sender, EventArgs _)
        {
            var cell = (ViewCell)sender;
            var label = cell?.View.FindByName<Label>("HeadingLabel");

            if (Device.RuntimePlatform == Device.iOS && label?.Text != null)
            {
                //cell.Height = TextMeter.MeasureTextSizeIOS(label.Text, Width, label.FontSize) + 10;
            }
        }
    }
}