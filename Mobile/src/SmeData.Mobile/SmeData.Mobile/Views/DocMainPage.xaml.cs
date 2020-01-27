using SmeData.Mobile.Helpers;
using System.Text.RegularExpressions;
using Xamarin.Forms;

namespace SmeData.Mobile.Views
{
    public partial class DocMainPage : TabbedPage
    {
        public DocMainPage()
        {
            InitializeComponent();
        }

        private void TabbedPage_Appearing(object sender, System.EventArgs e)
        {
            //this.SelectedItem = tbDocContent;
        }

        private void Description_Navigating(object sender, WebNavigatingEventArgs e)
        {
            if (UrlNavHelper.ShouldCancelNavigatton(e.Url))
            {
                e.Cancel = true;
            }
        }

        private void ListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (lvContent.SelectedItem != null)
            {
                lvContent.ScrollTo(lvContent.SelectedItem, ScrollToPosition.MakeVisible, true);
            }
        }
    }
}
