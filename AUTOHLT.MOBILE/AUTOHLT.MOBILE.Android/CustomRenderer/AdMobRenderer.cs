using Android.Content;
using Android.Gms.Ads;
using AUTOHLT.MOBILE.CustomRenderer;
using AUTOHLT.MOBILE.Droid.CustomRenderer;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(AdmobView), typeof(AdMobRenderer))]
namespace AUTOHLT.MOBILE.Droid.CustomRenderer
{
    public class AdMobRenderer : ViewRenderer<AdmobView, AdView>
    {
        public AdMobRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<AdmobView> e)
        {
            base.OnElementChanged(e);
            if (Control != null)
            {
                var adview = new AdView(Context)
                {
                    AdSize = AdSize.SmartBanner,
                    AdUnitId = Element.AdsUnitId,
                };
                var requestbuilder = new AdRequest.Builder();
                adview.LoadAd(requestbuilder.Build());
                e.NewElement.HeightRequest = 50;
                SetNativeControl(adview);
            }
        }
    }
}