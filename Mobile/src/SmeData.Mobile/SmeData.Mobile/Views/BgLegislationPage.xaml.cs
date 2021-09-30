using SmeData.Mobile.Helpers;
using System;
using Xamarin.Forms;

namespace SmeData.Mobile.Views
{
    public partial class BgLegislationPage : ContentPage
    {
        public BgLegislationPage()
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
