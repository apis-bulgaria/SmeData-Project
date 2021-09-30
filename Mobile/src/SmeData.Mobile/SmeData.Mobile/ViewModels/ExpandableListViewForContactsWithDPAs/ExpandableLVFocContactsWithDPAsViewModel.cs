using MvvmHelpers;
using SmeData.Mobile.Models.ExpandedListViewForContactsWirhDPAs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace SmeData.Mobile.ViewModels.ExpandableListViewForContactsWithDPAs
{
    public class ExpandableLVFocContactsWithDPAsViewModel : ObservableRangeCollection<ItemExpandedLVForContactsWithDPAsViewModel>, INotifyPropertyChanged
    {
        private ExpandableLVForContactsWithDPAsModel expandedLV;

        private ObservableRangeCollection<ItemExpandedLVForContactsWithDPAsViewModel> Items = new ObservableRangeCollection<ItemExpandedLVForContactsWithDPAsViewModel>();

        public ExpandableLVFocContactsWithDPAsViewModel(ExpandableLVForContactsWithDPAsModel expandedLV, bool expanded = false)
        {
            this.expandedLV = expandedLV;
            this.expanded = expanded;

            foreach (var item in expandedLV.Items)
            {
                Items.Add(new ItemExpandedLVForContactsWithDPAsViewModel(item));
            }

            if (expanded)
            {
                this.AddRange(Items);
            }
        }

        public string Header { get { return expandedLV.Header; } }

        private bool expanded;
        public bool Expanded
        {
            get => expanded;
            set
            {
                if (expanded != value)
                {
                    expanded = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("Expanded"));
                    OnPropertyChanged(new PropertyChangedEventArgs("IsExpanded"));
                    if (expanded)
                    {
                        this.AddRange(Items);
                    }
                    else
                    {
                        this.Clear();
                    }
                }
            }
        }

        public bool IsExpanded
        {
            get { return Expanded; }
        }
    }
}
