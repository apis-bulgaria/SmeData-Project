using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SmeData.Mobile.CustomControls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MenuItem
    {
        public static readonly BindableProperty ImageSourceProperty = BindableProperty.Create(nameof(ImageSource),
             typeof(string),
             typeof(MenuItem));

        public static readonly BindableProperty LabelTextProperty = BindableProperty.Create(nameof(LabelText),
            typeof(string),
            typeof(MenuItem));

        public static readonly BindableProperty TabCommandProperty = BindableProperty.Create(nameof(TabCommand),
            typeof(ICommand),
            typeof(MenuItem),
            default(ICommand));

        public static readonly BindableProperty CommandParProperty = BindableProperty.Create(nameof(CommandPar),
             typeof(string),
             typeof(MenuItem));

        public static readonly BindableProperty DefaultFontSizeProperty = BindableProperty.Create(nameof(DefaultFontSize),
           typeof(int),
           typeof(FrameButtonHorizontal));

        public static readonly BindableProperty DefaultHeightSizeProperty = BindableProperty.Create(nameof(DefaultHeightSize),
          typeof(int),
          typeof(FrameButtonHorizontal));

        public MenuItem()
        {
            InitializeComponent();
            Content.BindingContext = this;
        }

        public string ImageSource
        {
            get => (string)GetValue(ImageSourceProperty);
            set => SetValue(ImageSourceProperty, value);
        }

        public string LabelText
        {
            get => (string)GetValue(LabelTextProperty);
            set => SetValue(LabelTextProperty, value);
        }

        public ICommand TabCommand
        {
            get => (ICommand)GetValue(TabCommandProperty);
            set => SetValue(TabCommandProperty, value);
        }

        public string CommandPar
        {
            get => (string)GetValue(CommandParProperty);
            set => SetValue(CommandParProperty, value);
        }

        public int DefaultFontSize
        {
            get => (int)GetValue(DefaultFontSizeProperty);
            set => SetValue(DefaultFontSizeProperty, value);
        }

        public int DefaultHeightSize
        {
            get => (int)GetValue(DefaultHeightSizeProperty);
            set => SetValue(DefaultHeightSizeProperty, value);
        }
    }
}