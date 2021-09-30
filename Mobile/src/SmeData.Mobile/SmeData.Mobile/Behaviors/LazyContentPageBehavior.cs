using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace SmeData.Mobile.Behaviors
{
    public class LazyContentPageBehavior : LoadContentOnActivateBehavior<ContentView>
    {
        protected override void SetContent(ContentView element, View contentView)
        {
            element.Content = contentView;
        }
    }
}
