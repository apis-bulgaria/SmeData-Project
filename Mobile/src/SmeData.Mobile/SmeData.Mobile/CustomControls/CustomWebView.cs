using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace SmeData.Mobile.CustomControls
{
    /// <summary>
    /// 
    /// </summary>
    public class CustomWebView : WebView
    {
        public static readonly BindableProperty PannedCommandProperty = BindableProperty.Create(
            nameof(PannedCommand), 
            typeof(ICommand), 
            typeof(CustomWebView));


        public ICommand PannedCommand
        {
            set { SetValue(PannedCommandProperty, value); }
            get { return (ICommand)GetValue(PannedCommandProperty); }
        }
    }

    public enum PannedDirection
    {
        Left,
        Right
    }
}