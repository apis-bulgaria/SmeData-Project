using Plugin.Multilingual;
using SmeData.Mobile.Views.PageOrientation;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SmeData.Mobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class WelcomePage : PageOrientation.OrientationContentPage
    {
        public WelcomePage()
        {
            InitializeComponent();
            

            //if (DeviceDisplay.MainDisplayInfo.Orientation == DisplayOrientation.Landscape)
            //{
            //    SetupLandscapeLayout();
            //}

            //OnOrientationChanged += DeviceRotated;
        }

        private void DeviceRotated(object s, PageOrientationEventArgs e)
        {
            switch (e.Orientation)
            {
                case PageOrientation.PageOrientation.Horizontal:
                    SetupLandscapeLayout();
                    break;
                case PageOrientation.PageOrientation.Vertical:
                    SetupPortraitLayout();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void SetupPortraitLayout()
        {
            SecondGrid.SetValue(Grid.RowProperty, 1);
            SecondGrid.SetValue(Grid.ColumnProperty, 0);
            
            FirstGrid.SetValue(Grid.RowSpanProperty, 1);
            FirstGrid.SetValue(Grid.ColumnSpanProperty, 2);

            SecondGrid.SetValue(Grid.RowSpanProperty, 1);
            SecondGrid.SetValue(Grid.ColumnSpanProperty, 2);
        }

        private void SetupLandscapeLayout()
        {
            SecondGrid.SetValue(Grid.RowProperty, 0);
            SecondGrid.SetValue(Grid.ColumnProperty, 1);

            FirstGrid.SetValue(Grid.RowSpanProperty, 2);
            FirstGrid.SetValue(Grid.ColumnSpanProperty, 1);

            SecondGrid.SetValue(Grid.RowSpanProperty, 2);
            SecondGrid.SetValue(Grid.ColumnSpanProperty, 1);
        }
    }
}