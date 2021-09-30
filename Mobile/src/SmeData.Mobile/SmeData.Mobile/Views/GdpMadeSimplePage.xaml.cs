using SmeData.Mobile.Helpers;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SmeData.Mobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class GdpMadeSimplePage : ContentPage
    {
        public GdpMadeSimplePage()
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