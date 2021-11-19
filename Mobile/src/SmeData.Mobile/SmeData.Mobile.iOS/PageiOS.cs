using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using CoreGraphics;
using Foundation;
using SmeData.Mobile.iOS;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using MasterDetailPage = Xamarin.Forms.MasterDetailPage;

[assembly: ExportRenderer(typeof(ContentPage), typeof(PageiOS))]
namespace SmeData.Mobile.iOS
{
    public class PageiOS : PageRenderer
    {
        //readonly UIRectEdge swipeEdge;

        //public PageiOS(UIRectEdge edge)
        //{
        //    swipeEdge = edge;
        //}

        //public PageiOS()
        //{

        //}

        //public override void ViewDidLoad()
        //{
        //    base.ViewDidLoad();

        //    // Attach the Edge Gesture.
        //    var interactiveTransitionRecognizer = new UIScreenEdgePanGestureRecognizer();
        //    interactiveTransitionRecognizer.AddTarget(() =>
        //        InteractiveTransitionRecognizerAction(interactiveTransitionRecognizer));
        //    interactiveTransitionRecognizer.Edges = UIRectEdge.Left;
        //    View.AddGestureRecognizer(interactiveTransitionRecognizer);
        //}

        //void InteractiveTransitionRecognizerAction(UIScreenEdgePanGestureRecognizer sender)
        //{
        //    // If we get swipe from the left edge, we show the menu.
        //    if (sender.State == UIGestureRecognizerState.Began)
        //    {
        //        SetPresentedValue(true);
        //    }
        //}

        ///// <summary>
        ///// Attempt to set the internal Presented property.
        ///// </summary>
        //private void SetPresentedValue(bool newValue)
        //{
        //    foreach (PropertyInfo info in typeof(PhoneMasterDetailRenderer).GetRuntimeProperties())
        //    {
        //        if (info.Name == "Presented")
        //        {
        //            info.SetValue(this, newValue);
        //            break;
        //        }
        //    }
        //}

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            if (ViewController != null && ViewController.NavigationController != null)
            {
                ViewController.NavigationController.InteractivePopGestureRecognizer.Enabled = false;
            }
        }

        public override void ViewWillLayoutSubviews()
        {
            base.ViewWillLayoutSubviews();

            nfloat tabSize = 40.0f;

            UIInterfaceOrientation orientation = UIApplication.SharedApplication.StatusBarOrientation;

            //if (UIInterfaceOrientation.LandscapeLeft == orientation || UIInterfaceOrientation.LandscapeRight == orientation)
            //{
            //    tabSize = 32.0f;
            //}

            if (TabBarController != null)
            {
                CGRect rect = this.View.Frame;
                rect.Y = tabSize;
                this.View.Frame = rect;
                
                CGRect tabFrame = this.TabBarController.TabBar.Frame;
                //tabFrame.Inflate(0.3, 0.3);
                tabFrame.Height = tabSize;
                tabFrame.Y = 0;
                this.TabBarController.TabBar.Frame = tabFrame;
                this.TabBarController.TabBar.BarTintColor = Color.FromHex("#E8E8E8").ToUIColor();

                CGSize size = new CGSize(this.TabBarController.TabBar.Frame.Width / this.TabBarController.TabBar.Items.Length, this.TabBarController.TabBar.Frame.Height);

                //Background Color
                this.TabBarController.TabBar.SelectionIndicatorImage = ImageWithColor(size);

                if (this.TabBarController.TabBar.Items != null)
                {
                    //var fontFamily = new UITextField().Font.Name;

                    foreach (var tabItem in this.TabBarController.TabBar.Items)
                    {
                        tabItem.SetTitleTextAttributes(new UITextAttributes() { Font = UIFont.FromName("Arial", 16), TextColor = Color.FromHex("#808080").ToUIColor(), TextShadowColor = UIColor.Clear }, UIControlState.Normal);
                        tabItem.SetTitleTextAttributes(new UITextAttributes() { TextColor = Color.FromHex("#62BDD0").ToUIColor(), TextShadowColor = UIColor.Clear }, UIControlState.Selected);

                        if (UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Phone)
                        {
                            tabItem.TitlePositionAdjustment = new UIOffset(0, -10);
                        }
                        else if (UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Pad)
                        {
                            tabItem.TitlePositionAdjustment = new UIOffset(0, 5);
                        }

                        if (UIInterfaceOrientation.LandscapeLeft == orientation || UIInterfaceOrientation.LandscapeRight == orientation)
                        {
                            tabItem.TitlePositionAdjustment = new UIOffset(0, 0);
                        }
                    }
                }
            }
        }

        public UIImage ImageWithColor(CGSize size)
        {
            CGRect rect = new CGRect(0, 0, size.Width, size.Height);
            UIGraphics.BeginImageContext(size);

            using (CGContext context = UIGraphics.GetCurrentContext())
            {
                context.SetFillColor(UIColor.White.CGColor);
                context.FillRect(rect);
            }

            UIImage image = UIGraphics.GetImageFromCurrentImageContext();
            UIGraphics.EndImageContext();

            return image;
        }

        protected override void OnElementChanged(VisualElementChangedEventArgs e)
        {
            base.OnElementChanged(e);

            if (UIDevice.CurrentDevice.CheckSystemVersion(13, 0))
            {
                OverrideUserInterfaceStyle = UIUserInterfaceStyle.Light;
            }
        }
    }
}
