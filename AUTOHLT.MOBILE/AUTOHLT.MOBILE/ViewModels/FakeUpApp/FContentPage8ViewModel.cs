using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Prism.Navigation;
using Syncfusion.XForms.Buttons;
using Xamarin.Forms;

namespace AUTOHLT.MOBILE.ViewModels.FakeUpApp
{
    public class FContentPage8ViewModel : ViewModelBase
    {
        private string fromText = "";
        public string FromText
        {
            get { return fromText; }
            set { fromText = value; NotifyPropertyChanged("FromText"); }
        }
        private string toText = "";
        public string ToText
        {
            get { return toText; }
            set { toText = value; NotifyPropertyChanged("ToText"); }
        }
        public FContentPage8ViewModel(INavigationService navigationService) : base(navigationService)
        {
            itemCollection = new ObservableCollection<SfSegmentItem>
                    {
                        new SfSegmentItem() {  Text = "Seater"},
                        new SfSegmentItem() {  Text = "Sleeper"},
                    };
            ViewCollection = new ObservableCollection<View>
                    {
                       ResetViewButton,
                       GoViewButton
                    };
        }
        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private ObservableCollection<SfSegmentItem> itemCollection = new ObservableCollection<SfSegmentItem>();
        private ObservableCollection<View> viewCollection = new ObservableCollection<View>();
        public ObservableCollection<SfSegmentItem> ItemCollection
        {
            get { return itemCollection; }
            set { itemCollection = value; }
        }
        public ObservableCollection<View> ViewCollection
        {
            get { return viewCollection; }
            set { viewCollection = value; }
        }
        private Button ResetViewButton = new Button
        {
            Text = "Reset",
            TextColor = Color.FromHex("#979797"),
            BackgroundColor = Color.White,
            BorderColor = Color.FromHex("#979797"),
            BorderWidth = 1,
            CornerRadius = 4,
            HeightRequest = 50,
            VerticalOptions = LayoutOptions.Center
        };
        private Button GoViewButton = new Button
        {
            Text = "Go",
            TextColor = Color.FromHex("#979797"),
            BackgroundColor = Color.White,
            BorderColor = Color.FromHex("#979797"),
            BorderWidth = 1,
            CornerRadius = 4,
            HeightRequest = 50,
            VerticalOptions = LayoutOptions.Center
        };
    }
}