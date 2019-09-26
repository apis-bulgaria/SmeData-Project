using Prism.Commands;
using Prism.Mvvm;
using SmeData.Mobile.ViewModels;
using SmeData.SharedModels.Document;
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
    public partial class LoadIndicatorButton
    {
        public static readonly BindableProperty BtnCommandProperty = BindableProperty.Create(nameof(BtnCommand),
            typeof(ICommand),
            typeof(LoadIndicatorButton),
            default(ICommand));

        public static readonly BindableProperty CommandParProperty = BindableProperty.Create(nameof(CommandPar),
             typeof(OfflineDocument),
             typeof(LoadIndicatorButton));

        public static readonly BindableProperty LabelTextProperty = BindableProperty.Create(nameof(LabelText),
            typeof(string),
            typeof(LoadIndicatorButton));
        public static readonly BindableProperty IsButtonBusyProperty = BindableProperty.Create(nameof(IsButtonBusy),
            typeof(bool), typeof(LoadIndicatorButton), null);

        public LoadIndicatorButton()
        {
            InitializeComponent();
        }

        public string LabelText
        {
            get => (string)GetValue(LabelTextProperty);
            set => SetValue(LabelTextProperty, value);
        }

        public ICommand BtnCommand
        {
            get => (ICommand)GetValue(BtnCommandProperty);
            set => SetValue(BtnCommandProperty, value);
        }

        public OfflineDocument CommandPar
        {
            get => (OfflineDocument)GetValue(CommandParProperty);
            set => SetValue(CommandParProperty, value);
        }

        public bool IsButtonBusy
        {
            get { return (bool)GetValue(IsButtonBusyProperty); }
            set
            {
                SetValue(IsButtonBusyProperty, value);
            }
        }
        protected override void OnPropertyChanged(string propertyName)
        {
            base.OnPropertyChanged(propertyName);

            if (propertyName == BtnCommandProperty.PropertyName)
            {
                btnMain.Command = BtnCommand;
            }
            else if (propertyName == CommandParProperty.PropertyName)
            {
                btnMain.CommandParameter = CommandPar;
            }
            else if (propertyName == IsButtonBusyProperty.PropertyName)
            {
                actInd.IsVisible = IsButtonBusy;
                actInd.IsRunning = IsButtonBusy;
                btnMain.IsVisible = !IsButtonBusy;
            }
            else if (propertyName == LabelTextProperty.PropertyName)
            {
                btnMain.Text = LabelText;
            }
        }
    }
}