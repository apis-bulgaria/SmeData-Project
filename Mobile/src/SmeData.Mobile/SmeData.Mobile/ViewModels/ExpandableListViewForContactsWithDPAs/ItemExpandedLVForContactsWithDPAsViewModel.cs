using SmeData.Mobile.Models.ExpandedListViewForContactsWirhDPAs;
using SmeData.SharedModels.ContactsDPAs;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmeData.Mobile.ViewModels.ExpandableListViewForContactsWithDPAs
{
    public class ItemExpandedLVForContactsWithDPAsViewModel
    {
        private ItemExpandableLVForContactsWithDPAsModel item;

        public ItemExpandedLVForContactsWithDPAsViewModel(ItemExpandableLVForContactsWithDPAsModel item)
        {
            this.item = item;
        }

        public ContactDpaModel Content { get { return item.Content; } }
    }
}
