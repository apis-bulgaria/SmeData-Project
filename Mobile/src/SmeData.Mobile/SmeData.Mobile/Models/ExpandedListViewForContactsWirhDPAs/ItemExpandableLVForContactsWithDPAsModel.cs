using SmeData.SharedModels.ContactsDPAs;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmeData.Mobile.Models.ExpandedListViewForContactsWirhDPAs
{

    public class ItemExpandableLVForContactsWithDPAsModel
    {
        public ContactDpaModel Content { get; set; }

        public ItemExpandableLVForContactsWithDPAsModel(ContactDpaModel content)
        {
            this.Content = content;
        }
    }
}
