using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using Xamarin.Forms.Xaml;

namespace AUTOHLT.MOBILE.FakeModules.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FProfilePage : ContentPage
    {
        public FProfilePage()
        {
            InitializeComponent();
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            var safeInsets = On<iOS>().SafeAreaInsets();
            safeInsets.Left = 20;
            Padding = safeInsets;
        }
    }
}