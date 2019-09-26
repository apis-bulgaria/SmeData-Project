using System;
using System.Collections.Generic;
using System.Text;

namespace SmeData.Mobile.Models.ExpandedListView
{
    public class ExpandableLVModel : List<ItemExpandableLVModel>
    {
        public string Header { get; set; }
        public List<ItemExpandableLVModel> Items => this;
    }
}
