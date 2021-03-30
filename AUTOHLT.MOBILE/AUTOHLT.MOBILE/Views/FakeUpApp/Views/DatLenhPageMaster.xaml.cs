using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AUTOHLT.MOBILE.Views.FakeUpApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DatLenhPageMaster : Xamarin.Forms.ContentPage
    {
        public ListView ListView;

        public DatLenhPageMaster()
        {
            InitializeComponent();

            BindingContext = new DatLenhPageMasterViewModel();
            ListView = MenuItemsListView;
        }

        class DatLenhPageMasterViewModel : INotifyPropertyChanged
        {
            public ObservableCollection<DatLenhPageMasterMenuItem> MenuItems { get; set; }

            public DatLenhPageMasterViewModel()
            {
                MenuItems = new ObservableCollection<DatLenhPageMasterMenuItem>(new[]
                {
                    new DatLenhPageMasterMenuItem { Id = 0, Title = "Page 1" },
                    new DatLenhPageMasterMenuItem { Id = 1, Title = "Page 2" },
                    new DatLenhPageMasterMenuItem { Id = 2, Title = "Page 3" },
                    new DatLenhPageMasterMenuItem { Id = 3, Title = "Page 4" },
                    new DatLenhPageMasterMenuItem { Id = 4, Title = "Page 5" },
                });
            }

            #region INotifyPropertyChanged Implementation
            public event PropertyChangedEventHandler PropertyChanged;
            void OnPropertyChanged([CallerMemberName] string propertyName = "")
            {
                if (PropertyChanged == null)
                    return;

                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
            #endregion
        }
    }
}