using System;
using AUTOHLT.MOBILE.CustomRenderer;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using Xamarin.Forms.Xaml;

namespace AUTOHLT.MOBILE.Views.Home
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomePage : ContentPage
    {
        public HomePage()
        {
            InitializeComponent();
            var id = Device.RuntimePlatform == Device.Android ? "ca-app-pub-9881695093256851/9322509333" : "ca-app-pub-9881695093256851/1554373561";
            var adsView = new AdmobView() {AdsUnitId = id };
            ContentView.Content = adsView;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            On<iOS>().SetUseSafeArea(true);
            var safeInsets = On<iOS>().SafeAreaInsets();
            safeInsets.Bottom = -20;
            Padding = safeInsets;
        }
        protected override bool OnBackButtonPressed()
        {
            return true;
        }
    }
}