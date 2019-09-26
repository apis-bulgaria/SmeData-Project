using System;
using System.Collections.Generic;
using System.Text;

namespace SmeData.Mobile.Models.ExpandedListView
{
    public class ItemExpandableLVModel
    {
        public string Content { get; set; }

        public ItemExpandableLVModel(string content)
        {
            this.Content = content;
        }
    }
}
