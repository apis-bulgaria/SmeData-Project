using System;
using System.Collections.Generic;
using System.Text;

namespace SmeData.Mobile.Behaviors
{
    public interface IActiveAware
    {
        bool IsActive { get; set; }
        event EventHandler IsActiveChanged;
    }
}
