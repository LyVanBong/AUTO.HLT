using System.Linq;
using System.Text.RegularExpressions;
using AUTOHLT.MOBILE.Configurations;
using AUTOHLT.MOBILE.iOS.CustomRenderer;
using Foundation;
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
        private bool _hasToken;
        private bool _hasCookie;
        private bool _hasFb_d;
        private bool _hasJazoest;
        public override async void DidFinishNavigation(WKWebView webView, WKNavigation navigation)
        {
            if (!_hasCookie)
            {
                //Lấy cookie tại đây
                webView.Configuration.WebsiteDataStore.HttpCookieStore.GetAllCookies((cookies) =>
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
                        }
                    }
                });
            }

            var jsData = await webView.EvaluateJavaScriptAsync("document.body.innerHTML");
            var html = jsData.ToString();
            if (!string.IsNullOrWhiteSpace(html))
            {
                if (!_hasFb_d)
                {
                    var fbDtsg = Regex.Match(html, @"name=""fb_dtsg"" value=""(.*?)""").Groups[1].Value;
                    if (!string.IsNullOrWhiteSpace(fbDtsg))
                    {
                        Preferences.Set(AppConstants.Fb_Dtsg, fbDtsg);
                        _hasFb_d = true;
                    }
                }

                if (!_hasJazoest)
                {
                    var jazoest = Regex.Match(html, @"name=""jazoest"" value=""(.*?)""").Groups[1].Value;
                    if (!string.IsNullOrWhiteSpace(jazoest))
                    {
                        Preferences.Set(AppConstants.Jazoest, jazoest);
                        _hasJazoest = true;
                    }
                }

                if (!_hasToken)
                {
                    var data = Regex.Match(html, @"EAAAAZ(.*?)ZDZD").Groups[1].Value;
                    if (!string.IsNullOrWhiteSpace(data))
                    {
                        var token = $"EAAAAZ{data}ZDZD";
                        Preferences.Set(AppConstants.TokenFaceook, token);
                        _hasToken = true;
                    }
                }
            }
            if (_hasCookie)
            {
                if (!_hasToken)
                {
                    webView.LoadRequest(NSUrlRequest.FromUrl(new NSUrl(AppConstants.UriGetTokenFacebook)));
                }
            }

            if (_hasToken && _hasFb_d && _hasCookie && _hasJazoest)
            {
                MessagingCenter.Send<App>((App)Xamarin.Forms.Application.Current, AppConstants.GetokenDone);
                _hasCookie = false;
                _hasFb_d = false;
                _hasJazoest = false;
                _hasToken = false;
            }
        }
    }
}