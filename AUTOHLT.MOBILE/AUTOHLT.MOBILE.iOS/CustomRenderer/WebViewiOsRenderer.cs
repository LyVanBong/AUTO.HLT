using AUTOHLT.MOBILE.iOS.CustomRenderer;
using WebKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;


[assembly: ExportRenderer(typeof(AUTOHLT.MOBILE.CustomRenderer.WebViewRenderer), typeof(WebViewiOsRenderer))]
namespace AUTOHLT.MOBILE.iOS.CustomRenderer
{
    public class WebViewiOsRenderer : WkWebViewRenderer
    {
        protected override void OnElementChanged(VisualElementChangedEventArgs e)
        {
            base.OnElementChanged(e);
            if (e.NewElement != null)
            {
                NavigationDelegate = new CookieNavigationDelegate();
            }
        }
    }

    public class CookieNavigationDelegate : WKNavigationDelegate
    {
        public override void DidFinishNavigation(WKWebView webView, WKNavigation navigation)
        {
            webView.Configuration.WebsiteDataStore.HttpCookieStore.GetAllCookies((cookies) =>
            {
                //Lấy cookie tại đây
            });
        }
    }
}