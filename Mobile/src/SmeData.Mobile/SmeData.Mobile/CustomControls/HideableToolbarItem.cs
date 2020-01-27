using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SmeData.Mobile.CustomControls
{
    public class HideableToolbarItem : ToolbarItem
    {
        public HideableToolbarItem() : base()
        {
            this.InitVisibility();
        }

        private async void InitVisibility()
        {
            await Task.Delay(100);
            OnIsVisibleChanged(this, false, IsVisible);
        }
        public bool IsVisible
        {
            get { return (bool)GetValue(IsVisibleProperty); }
            set { SetValue(IsVisibleProperty, value); }
        }

        public static readonly BindableProperty IsVisibleProperty = BindableProperty.Create(nameof(IsVisible), typeof(bool), typeof(ToolbarItem), propertyChanged: OnIsVisibleChanged);

        private static void OnIsVisibleChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var item = bindable as HideableToolbarItem;

            bool newValueBool = (bool)newValue;

            if (item.Parent == null)
                return;

            var items = (item.Parent as ContentPage).ToolbarItems;
            if (items?.Count > 0)
            {
                if (newValueBool && !items.Contains(item))
                {
                    items.Add(item);
                }
                else if (!newValueBool && items.Contains(item))
                {
                    items.Remove(item);
                }
            }
        }
    }
}