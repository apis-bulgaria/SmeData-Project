using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SmeData.Mobile.CustomControls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CustomToolbarItem : ContentView
    {
        public static readonly BindableProperty MenuTextProperty = BindableProperty.Create(
                                propertyName: "MenuText",
                                returnType: typeof(string),
                                declaringType: typeof(CustomToolbarItem),
                                defaultValue: "",
                                defaultBindingMode: BindingMode.OneWay,
                                propertyChanged: TextPropertyChanged);

        public string MenuText
        {
            get { return base.GetValue(MenuTextProperty).ToString(); }
            set { base.SetValue(MenuTextProperty, value); }
        }

        private static void TextPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (CustomToolbarItem)bindable;
            control.menuText.Text = newValue.ToString();
        }

        public static readonly BindableProperty IconImageProperty = BindableProperty.Create(
                                propertyName: "IconImage",
                                returnType: typeof(string),
                                declaringType: typeof(CustomToolbarItem),
                                defaultValue: "",
                                defaultBindingMode: BindingMode.OneWay,
                                propertyChanged: ImageSourcePropertyChanged);

        public string IconImage
        {
            get { return base.GetValue(IconImageProperty).ToString(); }
            set { base.SetValue(IconImageProperty, value); }
        }

        private static void ImageSourcePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (CustomToolbarItem)bindable;
            control.iconImage.Source = ImageSource.FromFile(newValue.ToString());
        }

        public CustomToolbarItem()
        {
            InitializeComponent();
        }
    }
}