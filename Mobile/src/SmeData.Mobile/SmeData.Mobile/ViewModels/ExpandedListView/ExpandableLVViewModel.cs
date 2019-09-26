using MvvmHelpers;
using SmeData.Mobile.Models.ExpandedListView;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace SmeData.Mobile.ViewModels.ExpandedListView
{
    public class ExpandableLVViewModel : ObservableRangeCollection<ItemExpandedLVViewModel>, INotifyPropertyChanged
    {
        private ExpandableLVModel expandedLV;

        private ObservableRangeCollection<ItemExpandedLVViewModel> Items = new ObservableRangeCollection<ItemExpandedLVViewModel>();

        public ExpandableLVViewModel(ExpandableLVModel expandedLV, bool expanded = false)
        {
            this.expandedLV = expandedLV;
            this.expanded = expanded;

            foreach (var item in expandedLV.Items)
            {
                Items.Add(new ItemExpandedLVViewModel(item));
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
