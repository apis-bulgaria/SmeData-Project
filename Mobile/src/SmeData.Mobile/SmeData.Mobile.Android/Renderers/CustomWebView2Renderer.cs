using System;
using Android.Content;
using Android.Webkit;
using SmeData.Mobile.CustomControls;
using SmeData.Mobile.Droid.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(CustomWebView2), typeof(CustomWebView2Renderer))]
namespace SmeData.Mobile.Droid.Renderers
{
    public class CustomWebView2Renderer : WebViewRenderer
    {
        public CustomWebView2Renderer(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.WebView> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
            {
                Control.SetWebViewClient(new CustomWebView2Client());
                Control.Touch += (object sender, TouchEventArgs eventArgs) => 
                {
                    if (eventArgs.Event.Action == Android.Views.MotionEventActions.Down)
                    {
                        MessagingCenter.Send<Object>(this, "webviewClicked");
                    }
                };
            }
        }
    }

    internal class CustomWebView2Client : WebViewClient
    {
        public override bool ShouldOverrideUrlLoading(Android.Webkit.WebView view, string url)
        {
            if (url.StartsWith("home:"))
            {
                view.Context.StartActivity(typeof(MainActivity));
                return true;
            }
            view.LoadUrl(url);
            return true;
        }
    }
}