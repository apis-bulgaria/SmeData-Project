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
    public partial class SearchBarWithNavigation
    {
        public static readonly BindableProperty SearchInDocCommandProperty = BindableProperty.Create(nameof(SearchInDocCommand),
            typeof(ICommand),
            typeof(SearchBarWithNavigation),
            default(ICommand));

        public static readonly BindableProperty PrevMatchCommandProperty = BindableProperty.Create(nameof(PrevMatchCommand),
            typeof(ICommand),
            typeof(SearchBarWithNavigation),
            default(ICommand));

        public static readonly BindableProperty NextMatchCommandProperty = BindableProperty.Create(nameof(NextMatchCommand),
            typeof(ICommand),
            typeof(SearchBarWithNavigation),
            default(ICommand));

        public static readonly BindableProperty IsSearchVisibleProperty = BindableProperty.Create(nameof(IsSearchVisible),
          typeof(bool),
          typeof(SearchBarWithNavigation), true);

        public static readonly BindableProperty TextInFieldProperty = BindableProperty.Create(nameof(TextInField),
         typeof(string),
         typeof(SearchBarWithNavigation));

        public static readonly BindableProperty SearchCounterProperty = BindableProperty.Create(nameof(SearchCounter),
           typeof(string),
           typeof(SearchBarWithNavigation));

        public SearchBarWithNavigation()
        {
            InitializeComponent();
        }
        
        public ICommand SearchInDocCommand
        {
            get => (ICommand)GetValue(SearchInDocCommandProperty);
            set => SetValue(SearchInDocCommandProperty, value);
        }

        public ICommand PrevMatchCommand
        {
            get => (ICommand)GetValue(PrevMatchCommandProperty);
            set => SetValue(PrevMatchCommandProperty, value);
        }

        public ICommand NextMatchCommand
        {
            get => (ICommand)GetValue(NextMatchCommandProperty);
            set => SetValue(NextMatchCommandProperty, value);
        }

        public bool IsSearchVisible
        {
            get => (bool)GetValue(IsSearchVisibleProperty);
            set => SetValue(IsSearchVisibleProperty, value);
        }

        public string TextInField
        {
            get => (string)GetValue(TextInFieldProperty);
            set => SetValue(TextInFieldProperty, value);
        }

        public string SearchCounter
        {
            get => (string)GetValue(SearchCounterProperty);
            set => SetValue(SearchCounterProperty, value);
        }

        protected override void OnPropertyChanged(string propertyName)
        {
            base.OnPropertyChanged(propertyName);

            if (propertyName == SearchInDocCommandProperty.PropertyName)
            {
                searchBar.SearchCommand = SearchInDocCommand;
            }
            else if(propertyName == PrevMatchCommandProperty.PropertyName)
            {
                prevCommand.Command = PrevMatchCommand;
            }
            else if(propertyName == NextMatchCommandProperty.PropertyName)
            {
                nextCommand.Command = NextMatchCommand;
            }
            else if (propertyName == IsSearchVisibleProperty.PropertyName)
            {
                mainFlexLayout.IsVisible = IsSearchVisible;
            }
            else if (propertyName == TextInFieldProperty.PropertyName)
            {
                searchBar.Text = TextInField;
            }
            else if (propertyName == SearchCounterProperty.PropertyName)
            {
                searchCounterLabel.Text = SearchCounter;
            }
        }
    }
}