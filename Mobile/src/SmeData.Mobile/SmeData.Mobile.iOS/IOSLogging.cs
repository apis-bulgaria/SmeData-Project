using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using SmeData.Mobile.Helpers;
using UIKit;

namespace SmeData.Mobile.iOS
{
    public class IOSLogging : ICrossLogging
    {
        public IOSLogging() { }

        public static string log;

        string ICrossLogging.ReturnLog()
        {
            return log;
        }

        public void WritoToLog(string textToWtite)
        {
            log += $"{textToWtite}{Environment.NewLine}{Environment.NewLine}";
        }
    }
}