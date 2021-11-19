using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SmeData.Mobile.Effects
{
    public static class MouseOverEffect
    {
        public static readonly BindableProperty MouseOverProperty =
            BindableProperty.CreateAttached("MouseOver", typeof(Action), typeof(MouseOverEffect), default(Action), propertyChanged: OnhandlerChanged);


        public static Action GetMouseOver(BindableObject view)
        {
            return (Action)view.GetValue(MouseOverProperty);
        }


        public static void SetMouseOver(BindableObject view, Action value)
        {
            view.SetValue(MouseOverProperty, value);
        }

        static void OnhandlerChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var view = bindable as View;
            if (view == null)
            {
                return;
            }

            Action action = (Action)newValue;
            if (action != null)
            {
                view.Effects.Add(new ControlTooltipEffect());
            }
            else
            {
                var toRemove = view.Effects.FirstOrDefault(e => e is ControlTooltipEffect);
                if (toRemove != null)
                {
                    view.Effects.Remove(toRemove);
                }
            }
        }

        class ControlTooltipEffect : RoutingEffect
        {
            public ControlTooltipEffect() : base($"Microsoft.{nameof(MouseOverEffect)}")
            {

            }
        }
    }
}
