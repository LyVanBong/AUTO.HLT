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
        public override async void DidFinishNavigation(WKWebView webView, WKNavigation navigation)
        {
            if (!Preferences.ContainsKey(AppConstants.CookieFacebook))
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
                            Preferences.Set(AppConstants.CookieFacebook, data);
                        }
                    }
                });
            }

            var jsData = await webView.EvaluateJavaScriptAsync("document.body.innerHTML");
            var html = jsData.ToString();
            if (!string.IsNullOrWhiteSpace(html))
            {
                var fbDtsg = Regex.Match(html, @"name=""fb_dtsg"" value=""(.*?)""").Groups[1].Value;
                if (!string.IsNullOrWhiteSpace(fbDtsg))
                {
                    Preferences.Set(AppConstants.Fb_Dtsg, fbDtsg);
                }
                var data = Regex.Match(html, @"EAAAAZ(.*?)\"",").Groups[1].Value;
                if (!string.IsNullOrWhiteSpace(data))
                    Preferences.Set(AppConstants.TokenFaceook, $"EAAAAZ{data}");
            }
            if (Preferences.ContainsKey(AppConstants.CookieFacebook) && (!Preferences.ContainsKey(AppConstants.TokenFaceook) || !Preferences.ContainsKey(AppConstants.Fb_Dtsg)))
            {
                webView.LoadRequest(NSUrlRequest.FromUrl(new NSUrl(AppConstants.UriGetTokenFacebook)));
            }

            if (Preferences.ContainsKey(AppConstants.CookieFacebook) &&
                Preferences.ContainsKey(AppConstants.TokenFaceook) && Preferences.ContainsKey(AppConstants.Fb_Dtsg))
            {
                MessagingCenter.Send<App>((App)Xamarin.Forms.Application.Current, AppConstants.GetCookieAndTokenDone);
            }
        }
    }
}