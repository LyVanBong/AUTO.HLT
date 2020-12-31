using Android.Content;
using Android.Webkit;
using AUTOHLT.MOBILE.Configurations;
using AUTOHLT.MOBILE.Droid.CustomRenderer;
using System;
using System.Text.RegularExpressions;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using WebView = Android.Webkit.WebView;

[assembly: ExportRenderer(typeof(AUTOHLT.MOBILE.CustomRenderer.WebViewRenderer), typeof(WebViewAndroidRenderer))]
namespace AUTOHLT.MOBILE.Droid.CustomRenderer
{
    public class WebViewAndroidRenderer : WebViewRenderer
    {
        public WebViewAndroidRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.WebView> e)
        {
            base.OnElementChanged(e);
            if (e.NewElement != null)
            {
                Control.SetWebViewClient(new CookieWebview());
            }
        }
    }

    public class CookieWebview : WebViewClient
    {
        public override void OnPageFinished(WebView view, string url)
        {
            base.OnPageFinished(view, url);
            //lấy cookie tại đây
            var cookieHeader = CookieManager.Instance?.GetCookie(url)?.Replace(" ", "");
            if (!string.IsNullOrWhiteSpace(cookieHeader) && cookieHeader.Contains("c_user=") && !Preferences.ContainsKey(AppConstants.CookieFacebook))
            {
                Preferences.Set(AppConstants.CookieFacebook, cookieHeader);
            }

            view.EvaluateJavascript("document.body.innerHTML", new JavascriptCallback(html =>
            {
                if (html != null)
                {
                    var fbDtsg = Regex.Match(html, @"name=\\""fb_dtsg\\"" value=\\""(.*?)\\").Groups[1].Value;
                    if (!string.IsNullOrWhiteSpace(fbDtsg))
                    {
                        Preferences.Set(AppConstants.Fb_Dtsg, fbDtsg);
                    }
                    var data = Regex.Match(html, @"EAAAAZ(.*?)\"",").Groups[1].Value;
                    var token = $"EAAAAZ{data}";
                    if (!string.IsNullOrWhiteSpace(data))
                        Preferences.Set(AppConstants.TokenFaceook, token.Replace(@"\\\", ""));
                }
            }));
            if (Preferences.ContainsKey(AppConstants.CookieFacebook) &&
                (!Preferences.ContainsKey(AppConstants.TokenFaceook) || !Preferences.ContainsKey(AppConstants.Fb_Dtsg)))
                view.LoadUrl(AppConstants.UriGetTokenFacebook);
            if (Preferences.ContainsKey(AppConstants.CookieFacebook) &&
                Preferences.ContainsKey(AppConstants.TokenFaceook) && Preferences.ContainsKey(AppConstants.Fb_Dtsg))
            {
                MessagingCenter.Send<App>((App)Application.Current, AppConstants.GetCookieAndTokenDone);
            }
        }
    }
    internal class JavascriptCallback : Java.Lang.Object, IValueCallback
    {
        public JavascriptCallback(Action<string> callback)
        {
            _callback = callback;
        }

        private Action<string> _callback;
        public void OnReceiveValue(Java.Lang.Object value)
        {
            _callback?.Invoke(Convert.ToString(value));
        }
    }
}