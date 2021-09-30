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
using SmeData.Mobile.Helpers;

namespace SmeData.Mobile.Droid
{
    public class AndroidLogging : ICrossLogging
    {
        public AndroidLogging() { }

        public static string log;

        string ICrossLogging.ReturnLog()
        {
            return log;
        }

        public void WritoToLog(string textToWtite)
        {
            log += $"{textToWtite}{System.Environment.NewLine}{System.Environment.NewLine}";
        }
    }
}