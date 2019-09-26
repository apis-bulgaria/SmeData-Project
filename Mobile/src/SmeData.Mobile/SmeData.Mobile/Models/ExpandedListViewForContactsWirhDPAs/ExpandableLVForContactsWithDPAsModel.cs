using System;
using System.Collections.Generic;
using System.Text;

namespace SmeData.Mobile.Models.ExpandedListViewForContactsWirhDPAs
{
    public class ExpandableLVForContactsWithDPAsModel : List<ItemExpandableLVForContactsWithDPAsModel>
    {
        public string Header { get; set; }
        public List<ItemExpandableLVForContactsWithDPAsModel> Items => this;
    }
}
