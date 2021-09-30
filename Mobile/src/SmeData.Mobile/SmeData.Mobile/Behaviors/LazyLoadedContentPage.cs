using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace SmeData.Mobile.Behaviors
{
    public class LazyLoadedContentPage : ContentPage, IActiveAware
    {
        public event EventHandler IsActiveChanged;

        bool _isActive;
        public bool IsActive
        {
            get => _isActive;
            set
            {
                if (_isActive != value)
                {
                    _isActive = value;
                    IsActiveChanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }
    }
}
