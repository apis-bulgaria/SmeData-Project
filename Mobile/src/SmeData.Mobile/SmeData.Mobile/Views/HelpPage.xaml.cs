using SmeData.Mobile.Helpers;
using Xamarin.Forms;

namespace SmeData.Mobile.Views
{
    public partial class HelpPage : ContentPage
    {
        public HelpPage()
        {
            InitializeComponent();
        }
        private void WebView_Navigating(object sender, WebNavigatingEventArgs e)
        {
            if (UrlNavHelper.ShouldCancelNavigatton(e.Url))
            {
                e.Cancel = true;
            }
        }
    }
}
