using System;
using System.Collections.Generic;
using System.Text;

namespace SmeData.Mobile.Models
{
    public enum MenuItemType
    {
        WelcomePage,
        About,
        Settings
    }
    public class HomeMenuItem
    {
        public MenuItemType Id { get; set; }

        public string Title { get; set; }
    }
}
