using Microsoft.AppCenter.Crashes;
using System;
using System.Collections.Generic;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using Xamarin.Forms.Xaml;

namespace AUTOHLT.MOBILE.Views.Home
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomePage : ContentPage
    {
        private bool _starTimer;

        public HomePage()
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
            _starTimer = true;
            this.CarouselView.ItemsSource = new List<AutoVip>()
            {
                new AutoVip()
                {
                    Id = 1,
                    Title = "anh 1",
                    UrlImage = @"https://firebasestorage.googleapis.com/v0/b/autohlt.appspot.com/o/ads_autovip_banner_1.jpg?alt=media&token=e3a04e1b-3468-4772-a191-40b206aa6698"
                },
                new AutoVip()
                {
                    Id = 2,
                    Title = "anh 2",
                    UrlImage = "https://firebasestorage.googleapis.com/v0/b/autohlt.appspot.com/o/ads_autovip_banner_2.jpg?alt=media&token=ed1b8bce-f047-4b94-b07d-85aa6d4135d4"
                },
            };
            Device.StartTimer(TimeSpan.FromSeconds(5), () =>
            {
                CarouselView.Position = CarouselView.Position == 0 ? 1 : 0;
                return _starTimer;
            });
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            _starTimer = false;
        }

        protected override bool OnBackButtonPressed()
        {
            return true;
        }

        private async void Button_OnClicked(object sender, EventArgs e)
        {
            try
            {
                await Browser.OpenAsync("http://autovip.haluongthien.com/", new BrowserLaunchOptions
                {
                    LaunchMode = BrowserLaunchMode.SystemPreferred,
                    TitleMode = BrowserTitleMode.Show,
                    PreferredToolbarColor = Color.WhiteSmoke,
                    PreferredControlColor = Color.MediumBlue,
                });
            }
            catch (Exception exception)
            {
                Crashes.TrackError(exception);
            }
        }
    }

    public class AutoVip
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string UrlImage { get; set; }
    }
}