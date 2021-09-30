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
    public partial class DocInTabListView
    {
        public static readonly BindableProperty ImageSourceProperty = BindableProperty.Create(nameof(ImageSource),
             typeof(string),
             typeof(DocInTabListView));

        public static readonly BindableProperty LabelTitleTextProperty = BindableProperty.Create(nameof(LabelTitleText),
            typeof(string),
            typeof(DocInTabListView));

        public static readonly BindableProperty LabelSubTitleTextProperty = BindableProperty.Create(nameof(LabelSubTitleText),
            typeof(string),
            typeof(DocInTabListView));

        public static readonly BindableProperty TabCommandProperty = BindableProperty.Create(nameof(TabCommand),
            typeof(ICommand),
            typeof(DocInTabListView),
            default(ICommand));

        public static readonly BindableProperty CommandParProperty = BindableProperty.Create(nameof(CommandPar),
             typeof(object),
             typeof(DocInTabListView));

        public static readonly BindableProperty DefaultFontSizeProperty = BindableProperty.Create(nameof(DefaultFontSize),
           typeof(int),
           typeof(DocInTabListView));

        public static readonly BindableProperty IsSubTitleNotNullProperty = BindableProperty.Create(nameof(IsSubTitleNotNull),
          typeof(bool),
          typeof(DocInTabListView), true);

        public DocInTabListView()
        {
            InitializeComponent();
        }

        public string ImageSource
        {
            get => (string)GetValue(ImageSourceProperty);
            set => SetValue(ImageSourceProperty, value);
        }

        public string LabelTitleText
        {
            get => (string)GetValue(LabelTitleTextProperty);
            set => SetValue(LabelTitleTextProperty, value);
        }

        public string LabelSubTitleText
        {
            get => (string)GetValue(LabelSubTitleTextProperty);
            set => SetValue(LabelSubTitleTextProperty, value);
        }

        public ICommand TabCommand
        {
            get => (ICommand)GetValue(TabCommandProperty);
            set => SetValue(TabCommandProperty, value);
        }

        public object CommandPar
        {
            get => (object)GetValue(CommandParProperty);
            set => SetValue(CommandParProperty, value);
        }

        public bool IsSubTitleNotNull
        {
            get => (bool)GetValue(IsSubTitleNotNullProperty);
            set => SetValue(IsSubTitleNotNullProperty, value);
        }

        public int DefaultFontSize
        {
            get => (int)GetValue(DefaultFontSizeProperty);
            set => SetValue(DefaultFontSizeProperty, value);
        }

        protected override void OnPropertyChanged(string propertyName)
        {
            base.OnPropertyChanged(propertyName);

            if (propertyName == ImageSourceProperty.PropertyName)
            {
                ThisImage.Source = ImageSource;
            }
            else if (propertyName == LabelTitleTextProperty.PropertyName)
            {
                TitleLabel.Text = LabelTitleText;
            }
            else if (propertyName == LabelSubTitleTextProperty.PropertyName)
            {
                SubTitleLabel.Text = LabelSubTitleText;
            }
            else if (propertyName == TabCommandProperty.PropertyName)
            {
                ThisCommand.Command = TabCommand;
            }
            else if (propertyName == CommandParProperty.PropertyName)
            {
                ThisCommand.CommandParameter = CommandPar;
            }
            else if (propertyName == DefaultFontSizeProperty.PropertyName)
            {
                TitleLabel.FontSize = DefaultFontSize;
                SubTitleLabel.FontSize = DefaultFontSize - 2;
            }
            else if (propertyName == IsSubTitleNotNullProperty.PropertyName)
            {
                SubTitleLabel.IsVisible = IsSubTitleNotNull;
            }
        }
    }
}