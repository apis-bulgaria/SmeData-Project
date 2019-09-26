using Plugin.Multilingual;
using SmeData.Mobile.Services;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;
using System.Resources;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
namespace SmeData.Mobile.Helpers
{
    [ContentProperty("Text")]
    public class TranslateExtension : IMarkupExtension
    {
        const string ResourceId = "SmeData.Mobile.Properties.Resources";

        static readonly Lazy<ResourceManager> resmgr = new Lazy<ResourceManager>(() =>
        {
            var an = typeof(TranslateExtension).GetTypeInfo().Assembly;
            return new ResourceManager(ResourceId, typeof(TranslateExtension).GetTypeInfo().Assembly);
        });

        public string Text { get; set; }

        object IMarkupExtension.ProvideValue(IServiceProvider serviceProvider)
        {
            return ProvideValue(serviceProvider);
        }

        public BindingBase ProvideValue(IServiceProvider serviceProvider)
        {
            var binding = new Binding
            {
                Mode = BindingMode.OneWay,
                Path = $"[{Text}]",
                Source = Translator.Instance,
            };
            return binding;
        }

        //        public object ProvideValue(IServiceProvider serviceProvider)
        //        {
        //            if (Text == null)
        //                return "";


        //            //CrossMultilingual.Current.CurrentCultureInfo = new CultureInfo("bg");
        //            var ci = CrossMultilingual.Current.CurrentCultureInfo;

        //            var translation = resmgr.Value.GetString(Text, ci);

        //            if (translation == null)
        //            {

        //#if DEBUG
        //                throw new ArgumentException(
        //                    String.Format("Key '{0}' was not found in resources '{1}' for culture '{2}'.", Text, ResourceId, ci.Name),
        //                    "Text");
        //#else
        //                translation = Text; // returns the key, which GETS DISPLAYED TO THE USER
        //#endif
        //            }
        //            return translation;
        //        }
    }

    public class Translator : INotifyPropertyChanged
    {
        //const string ResourceId = "SmeData.Mobile.Properties.Resources";
        //static readonly Lazy<ResourceManager> resmgr = new Lazy<ResourceManager>(() =>
        //{
        //    var an = typeof(TranslateExtension).GetTypeInfo().Assembly;
        //    return new ResourceManager(ResourceId, typeof(TranslateExtension).GetTypeInfo().Assembly);
        //});
        private static readonly Lazy<TranslationManager> resmgr = new Lazy<TranslationManager>(() =>
        {
            return new TranslationManager();
        });
        
        public string this[string text]
        {
            get
            {
                var cultureInfo = Properties.Resources.Culture;
                var txt = resmgr.Value.GetString(text, cultureInfo);
                return txt;
            }
        }

        public static string GetString(string text)
        {
            var cultureInfo = Properties.Resources.Culture;
            var txt = resmgr.Value.GetString(text, cultureInfo);
            return txt;
        }

        public static Translator Instance { get; } = new Translator();

        public event PropertyChangedEventHandler PropertyChanged;

        public void Invalidate()
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(null));
        }
    }
}
