using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using SmeData.Mobile.CustomControls;
using SmeData.Mobile.Droid.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using static Java.Util.ResourceBundle;

[assembly: ExportRenderer(typeof(WebView), typeof(MyWebViewRenderer))]
namespace SmeData.Mobile.Droid.Renderers
{
    
    public class MyWebViewRenderer : WebViewRenderer
    {
        public MyWebViewRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<WebView> e)
        {
            base.OnElementChanged(e);

            if (Element is CustomWebView)
            {
                Control.SetOnTouchListener(new MyOnTouchListener((CustomWebView)Element));
            }
        }
    }

    public class MyOnTouchListener : Java.Lang.Object, Android.Views.View.IOnTouchListener
    {
        float oldX;

        float newX;

        CustomWebView myWebView;
        public MyOnTouchListener(CustomWebView webView)
        {
            myWebView = webView;
        }
        public bool OnTouch(Android.Views.View v, MotionEvent e)
        {
            if (e.Action == MotionEventActions.Down)
            {
                oldX = e.GetX(0);
            }

            if (e.Action == MotionEventActions.Up)
            {
                newX = e.GetX();

                if (newX - oldX > 150)
                {
                    myWebView.PannedCommand?.Execute(PannedDirection.Right);
                }
                else if (oldX - newX > 150)
                {
                    myWebView.PannedCommand?.Execute(PannedDirection.Left);
                }
            }

            return false;
        }
    }
}