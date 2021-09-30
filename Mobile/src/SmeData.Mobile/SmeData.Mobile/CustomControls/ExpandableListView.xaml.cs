using MvvmHelpers;
using Prism.Commands;
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
    public partial class ExpandableListView
    {
        public static readonly BindableProperty ListViewItemsProperty = BindableProperty.Create(nameof(ListViewItems),
            typeof(ObservableRangeCollection<ExpandableLVViewModel>),
            typeof(ExpandableListView));

        public static readonly BindableProperty LabelFontSizeProperty = BindableProperty.Create(nameof(LabelFontSize),
           typeof(int),
           typeof(ExpandableListView));


        public ICommand QuestionClickCommand { get; private set; }

        public ExpandableListView()
        {
            InitializeComponent();
            this.QuestionClickCommand = new DelegateCommand<ExpandableLVViewModel>(this.ExecuteQuestionClickCommand);
        }

        public int LabelFontSize
        {
            get => (int)GetValue(LabelFontSizeProperty);
            set => SetValue(LabelFontSizeProperty, value);
        }

        public ObservableRangeCollection<ExpandableLVViewModel> ListViewItems
        {
            get => (ObservableRangeCollection<ExpandableLVViewModel>)GetValue(ListViewItemsProperty);
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

        private void ExecuteQuestionClickCommand(ExpandableLVViewModel item)
        {
            item.Expanded = !item.Expanded;
        }
    }
}