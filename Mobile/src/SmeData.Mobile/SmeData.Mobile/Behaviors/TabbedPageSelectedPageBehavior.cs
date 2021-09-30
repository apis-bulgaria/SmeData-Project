using Prism.Behaviors;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace SmeData.Mobile.Behaviors
{
    public class TabbedPageSelectedPageIndexBehavior : BehaviorBase<TabbedPage>
    {
        public static readonly BindableProperty AttachBehaviorProperty =
        BindableProperty.CreateAttached(
            "AttachBehavior",
            typeof(bool),
            typeof(TabbedPageSelectedPageIndexBehavior),
            false,
            propertyChanged: OnAttachBehaviorChanged);

        public static bool GetAttachBehavior(BindableObject view)
        {
            return (bool)view.GetValue(AttachBehaviorProperty);
        }

        public static void SetAttachBehavior(BindableObject view, bool value)
        {
            view.SetValue(AttachBehaviorProperty, value);
        }

        static void OnAttachBehaviorChanged(BindableObject view, object oldValue, object newValue)
        {
            if (view is TabbedPage tabPage)
            {
                tabPage.CurrentPage = tabPage.Children[1];
            }
        }
    }
}
