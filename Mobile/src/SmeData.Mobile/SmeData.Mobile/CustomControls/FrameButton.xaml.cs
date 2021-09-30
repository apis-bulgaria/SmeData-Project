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
    public partial class FrameButton
    {
        public static readonly BindableProperty ImageSourceProperty = BindableProperty.Create(nameof(ImageSource),
             typeof(string),
             typeof(FrameButton));

        public static readonly BindableProperty LabelTextProperty = BindableProperty.Create(nameof(LabelText),
            typeof(string),
            typeof(FrameButton));

        public static readonly BindableProperty TabCommandProperty = BindableProperty.Create(nameof(TabCommand),
            typeof(ICommand),
            typeof(FrameButton),
            default(ICommand));

        public static readonly BindableProperty SideOfImageProperty = BindableProperty.Create(nameof(SideOfImage),
            typeof(int),
            typeof(FrameButton));

        public static readonly BindableProperty DefaultFontSizeProperty = BindableProperty.Create(nameof(DefaultFontSize),
            typeof(int),
            typeof(FrameButton));

        public FrameButton()
        {
            InitializeComponent();
            Content.BindingContext = this;

            var touchEffect = new TouchEffect
            {
                Capture = true
            };
            touchEffect.TouchAction += TouchEffect_TouchAction;
            slContent.Effects.Add(touchEffect);
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

        public int SideOfImage
        {
            get => (int)GetValue(SideOfImageProperty);
            set => SetValue(SideOfImageProperty, value);
        }

        public int DefaultFontSize
        {
            get => (int)GetValue(DefaultFontSizeProperty);
            set => SetValue(DefaultFontSizeProperty, value);
        }
    }
}