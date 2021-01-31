using System;
using AUTOHLT.MOBILE.Configurations;
using Microsoft.AppCenter.Crashes;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using Xamarin.Forms.Xaml;

namespace AUTOHLT.MOBILE.Views.VipInteraction
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class VipInteractionPage : ContentPage
    {
        public VipInteractionPage()
        {
            InitializeComponent();
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            On<iOS>().SetUseSafeArea(true);
            var safeInsets = On<iOS>().SafeAreaInsets();
            safeInsets.Bottom = -20;
            Padding = safeInsets;
        }
        private async void TapGestureRecognizerHDSD_OnTapped(object sender, EventArgs e)
        {
            try
            {
                await Browser.OpenAsync(AppConstants.HdTangLike);
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }
    }
}