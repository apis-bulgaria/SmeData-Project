using MvvmHelpers;
using Prism.Commands;
using SmeData.Mobile.ViewModels.ExpandableListViewForContactsWithDPAs;
using SmeData.Mobile.ViewModels.ExpandedListView;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SmeData.Mobile.CustomControls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ExpandableLitsViewForContactWithDPAs
    {
        public static readonly BindableProperty ListViewItemsProperty = BindableProperty.Create(nameof(ListViewItems),
            typeof(ObservableRangeCollection<ExpandableLVFocContactsWithDPAsViewModel>),
            typeof(ExpandableLitsViewForContactWithDPAs));

        public static readonly BindableProperty LabelFontSizeProperty = BindableProperty.Create(nameof(LabelFontSize),
          typeof(int),
          typeof(ExpandableLitsViewForContactWithDPAs));

        public ICommand HeaderClickCommand { get; private set; }
        public ICommand CallNumberCommand { get; private set; }
        public ICommand OpenEmailClientCommand { get; private set; }
        public ICommand OpenUrlCommand { get; private set; }

        public int LabelFontSize
        {
            get => (int)GetValue(LabelFontSizeProperty);
            set => SetValue(LabelFontSizeProperty, value);
        }

        public ExpandableLitsViewForContactWithDPAs()
        {
            InitializeComponent();

            this.HeaderClickCommand = new DelegateCommand<ExpandableLVFocContactsWithDPAsViewModel>(this.DoExtendContent);
            this.CallNumberCommand = new DelegateCommand<string>(this.DoCallNumber);
            this.OpenEmailClientCommand = new DelegateCommand<string>(this.DoOpenEmailClient);
            this.OpenUrlCommand = new DelegateCommand<string>(this.DoOpenUrl);
        }

        public ObservableRangeCollection<ExpandableLVFocContactsWithDPAsViewModel> ListViewItems
        {
            get => (ObservableRangeCollection<ExpandableLVFocContactsWithDPAsViewModel>)GetValue(ListViewItemsProperty);
            set => SetValue(ListViewItemsProperty, value);
        }

        protected override void OnPropertyChanged(string propertyName)
        {
            base.OnPropertyChanged(propertyName);

            if (propertyName == ListViewItemsProperty.PropertyName)
            {
                ItemsSource = ListViewItems;
            }
        }

        private void DoExtendContent(ExpandableLVFocContactsWithDPAsViewModel item)
        {
            item.Expanded = !item.Expanded;
        }

        private void DoCallNumber(string telNumber)
        {
            Device.OpenUri(new Uri($"tel:{telNumber}"));
        }

        private void DoOpenEmailClient(string emailAddress)
        {
            Device.OpenUri(new Uri($"mailto:{emailAddress}"));
        }

        private void DoOpenUrl(string url)
        {
            Device.OpenUri(new Uri(url));
        }
    }
}