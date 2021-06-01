
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using Xamarin.Forms.Xaml;

namespace AUTOHLT.MOBILE.FakeModules.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FDetailProductPage : ContentPage
    {
        public FDetailProductPage()
        {
            InitializeComponent();
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            On<iOS>().SetUseSafeArea(true);
            var safeInsets = On<iOS>().SafeAreaInsets();
            safeInsets.Bottom = 0;
            Padding = safeInsets;
        }
    }
}