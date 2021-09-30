using SmeData.SharedModels.Language;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmeData.Mobile.Models.Settings
{
    public class SettingsModel
    {
        public SmeLanguage Language { get; set; }
        public bool ShowDocsOnWifiOnly { get; set; }
        public bool ShowMsgUpdates { get; set; }
        public int FontIndex { get; set; }
    }
}
