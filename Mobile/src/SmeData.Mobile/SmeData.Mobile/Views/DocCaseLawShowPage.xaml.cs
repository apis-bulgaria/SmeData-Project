using SmeData.Mobile.Helpers;
using Xamarin.Forms;

namespace SmeData.Mobile.Views
{
    public partial class DocCaseLawShowPage : ContentPage
    {
        public DocCaseLawShowPage()
        {
            InitializeComponent();
        }

        private void Description_Navigating(object sender, WebNavigatingEventArgs e)
        {
            if (UrlNavHelper.ShouldCancelNavigatton(e.Url))
            {
                e.Cancel = true;
            }
        }
    }
}
