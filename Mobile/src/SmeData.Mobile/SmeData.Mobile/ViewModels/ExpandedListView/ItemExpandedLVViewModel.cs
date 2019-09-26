using SmeData.Mobile.Models.ExpandedListView;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmeData.Mobile.ViewModels.ExpandedListView
{
    public class ItemExpandedLVViewModel
    {
        private ItemExpandableLVModel item;

        public ItemExpandedLVViewModel(ItemExpandableLVModel item)
        {
            this.item = item;
        }

        public string Content { get { return item.Content; } }
    }
}
