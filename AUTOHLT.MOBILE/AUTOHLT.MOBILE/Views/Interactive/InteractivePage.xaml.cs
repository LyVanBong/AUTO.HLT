using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using Xamarin.Forms.Xaml;

namespace AUTOHLT.MOBILE.Views.Interactive
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class InteractivePage : ContentPage
    {
        public InteractivePage()
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

        private async void WebView_OnNavigated(object sender, WebNavigatedEventArgs e)
        {
            var webView = sender as WebView;
            if (webView != null)
            {
                var cookie = await webView.EvaluateJavaScriptAsync("document.cookie");
            }
        }
    }
}