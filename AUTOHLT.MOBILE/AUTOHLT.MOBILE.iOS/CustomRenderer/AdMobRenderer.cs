using System.ComponentModel;
using AUTOHLT.MOBILE.CustomRenderer;
using AUTOHLT.MOBILE.iOS.CustomRenderer;
using Google.MobileAds;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(AdmobView), typeof(AdMobRenderer))]
namespace AUTOHLT.MOBILE.iOS.CustomRenderer
{
    public class AdMobRenderer : ViewRenderer<AdmobView, BannerView>
    {
        protected override void OnElementChanged(ElementChangedEventArgs<AdmobView> e)
        {
            base.OnElementChanged(e);
            if (Control != null)
            {
                SetNativeControl(CreateBannerView());
            }
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            Control.AdUnitId = Element.AdsUnitId;
        }

        private BannerView CreateBannerView()
        {
            var bannerView = new BannerView(AdSizeCons.SmartBannerPortrait)
            {
                AdUnitId = Element.AdsUnitId,
                RootViewController = GetVisibleViewController()
            };
            bannerView.LoadRequest(GetRequest());

            Request GetRequest()
            {
                var request = Request.GetDefaultRequest();
                return request;
            }

            return bannerView;
        }
        private UIViewController GetVisibleViewController()
        {
            var windows = UIApplication.SharedApplication.Windows;
            foreach (var window in windows)
            {
                if (window.RootViewController != null)
                {
                    return window.RootViewController;
                }
            }
            return null;
        }
    }
}