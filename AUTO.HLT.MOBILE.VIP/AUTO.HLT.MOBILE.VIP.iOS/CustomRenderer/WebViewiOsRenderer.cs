using System.Linq;
using System.Text.RegularExpressions;
using AUTO.HLT.MOBILE.VIP.Configurations;
using AUTO.HLT.MOBILE.VIP.CustomRenderer;
using AUTO.HLT.MOBILE.VIP.iOS.CustomRenderer;
using Foundation;
using WebKit;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(WebViewRenderer), typeof(WebViewiOsRenderer))]
namespace AUTO.HLT.MOBILE.VIP.iOS.CustomRenderer
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
        private bool _hasToken;
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
                            var ck = data.TrimEnd(';');
                            Preferences.Set(AppConstants.CookieFacebook, ck);
                            MessagingCenter.Send<App>((App)Xamarin.Forms.Application.Current, AppConstants.GetCookieDone);
                            _hasCookie = true;
                            if (!_hasToken)
                            {
                                webView.LoadRequest(NSUrlRequest.FromUrl(new NSUrl(AppConstants.UriGetTokenFacebook)));
                            }
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
                    if (!_hasToken)
                    {
                        var data = Regex.Match(html, @"\,\\\""accessToken\\\""\:\\\""(.*?)\\\""\,\\\""useLocalFilePreview\\\""\:true\,")?.Groups[1]?.Value;
                        if (!string.IsNullOrWhiteSpace(data))
                        {
                            Preferences.Set(AppConstants.TokenFaceook, data);
                            _hasToken = true;
                        }
                    }
                }
            }

            if (_hasToken && _hasCookie)
            {
                MessagingCenter.Send<App>((App)Xamarin.Forms.Application.Current, AppConstants.GetokenDone);
                _hasCookie = false;
                _hasToken = false;
            }
        }
    }
}