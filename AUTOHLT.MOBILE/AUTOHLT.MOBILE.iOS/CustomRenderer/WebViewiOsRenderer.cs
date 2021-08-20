using AUTOHLT.MOBILE.Configurations;
using AUTOHLT.MOBILE.iOS.CustomRenderer;
using System.Linq;
using System.Text.RegularExpressions;
using WebKit;
using Xamarin.Essentials;
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
        private bool _hasCookie;

        public override async void DidFinishNavigation(WKWebView webView, WKNavigation navigation)
        {
            if (!_hasCookie)
            {
                //Lấy cookie tại đây
                webView.Configuration.WebsiteDataStore.HttpCookieStore.GetAllCookies(async (cookies) =>
                {
                    var data = string.Empty;
                    if (cookies != null && cookies.Any())
                    {
                        foreach (var item in cookies)
                        {
                            data += $"{item.Name}={item.Value};";
                        }

                        if (data.Contains("c_user="))
                        {
                            _hasCookie = true;
                            var ck = data.TrimEnd(';');
                            Preferences.Set(AppConstants.CookieFacebook, ck);
                            MessagingCenter.Send<App>((App)Xamarin.Forms.Application.Current, AppConstants.GetCookieDone);
                        }
                    }
                });
            }

            if (_hasCookie)
            {
                var jsData = await webView.EvaluateJavaScriptAsync("document.body.innerHTML");
                var html = jsData.ToString();
                if (!string.IsNullOrWhiteSpace(html))
                {
                    var data = Regex.Match(html, @"\,\\\""accessToken\\\""\:\\\""(.*?)\\\""\,\\\""useLocalFilePreview\\\""\:true\,")?.Groups[1]?.Value;
                    if (!string.IsNullOrWhiteSpace(data))
                    {
                        _hasCookie = false;
                        Preferences.Set(AppConstants.TokenFaceook, data);
                        MessagingCenter.Send<App>((App)Xamarin.Forms.Application.Current, AppConstants.GetTokenDone);
                    }
                }
            }
        }
    }
}