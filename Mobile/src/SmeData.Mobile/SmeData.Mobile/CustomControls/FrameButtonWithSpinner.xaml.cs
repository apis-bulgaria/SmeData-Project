using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TouchTracking;
using TouchTracking.Forms;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SmeData.Mobile.CustomControls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FrameButtonWithSpinner
    {
        public static readonly BindableProperty LabelTextProperty = BindableProperty.Create(nameof(LabelText),
            typeof(string),
            typeof(FrameButtonWithSpinner));

        public static readonly BindableProperty IsButtonBusyProperty = BindableProperty.Create(nameof(IsButtonBusy),
            typeof(bool),
            typeof(FrameButtonWithSpinner));

        public static readonly BindableProperty TabCommandProperty = BindableProperty.Create(nameof(TabCommand),
            typeof(ICommand),
            typeof(FrameButtonWithSpinner),
            default(ICommand));

        public static readonly BindableProperty CommandParProperty = BindableProperty.Create(nameof(CommandPar),
             typeof(string),
             typeof(FrameButtonWithSpinner));

        public FrameButtonWithSpinner()
        {
            InitializeComponent();
            Content.BindingContext = this;
            var touchEffect = new TouchEffect
            {
                Capture = true
            };

            touchEffect.TouchAction += TouchEffect_TouchAction;
            gridContent.Effects.Add(touchEffect);
        }

        private object touchEffect_lock = new object();
        private void TouchEffect_TouchAction(object sender, TouchActionEventArgs e)
        {
            lock (touchEffect_lock)
            {
                switch (e.Type)
                {
                    case TouchActionType.Pressed:
                        this.Scale = 0.98;
                        this.HasShadow = false;
                        break;
                    case TouchActionType.Exited:
                    case TouchActionType.Cancelled:
                        this.Scale = 1;
                        this.HasShadow = true;
                        break;
                    case TouchActionType.Released:
                        if (this.Scale != 1)
                        {
                            this.TabCommand?.Execute(null);
                            this.Scale = 1;
                            this.HasShadow = true;
                        }
                        break;
                }
            }
        }

        public string LabelText
        {
            get => (string)GetValue(LabelTextProperty);
            set => SetValue(LabelTextProperty, value);
        }

        public bool IsButtonBusy
        {
            get => (bool)GetValue(IsButtonBusyProperty);
            set => SetValue(IsButtonBusyProperty, value);
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
    }
}