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
        private bool _hasCookie;
        public override void OnPageFinished(WebView view, string url)
        {
            base.OnPageFinished(view, url);

            if (!_hasCookie)
            {
                //lấy cookie tại đây
                var cookieHeader = CookieManager.Instance?.GetCookie(url)?.Replace(" ", "");
                if (!string.IsNullOrWhiteSpace(cookieHeader) && cookieHeader.Contains("c_user="))
                {
                    _hasCookie = true;
                    Preferences.Set(AppConstants.CookieFacebook, cookieHeader);
                    MessagingCenter.Send<App>((App)Application.Current, AppConstants.GetCookieDone);
                }
            }

            if (_hasCookie)
            {
                view.EvaluateJavascript("document.body.innerHTML", new JavascriptCallback(html =>
                {
                    if (html != null)
                    {
                        var data = Regex.Match(html, @"\,\\\\\\\""accessToken\\\\\\\""\:\\\\\\\""(.*?)\\\\\\\""\,\\\\\\\""useLocalFilePreview\\\\\\\""\:true\,")?.Groups[1]?.Value;
                        if (!string.IsNullOrWhiteSpace(data))
                        {
                            _hasCookie = false;
                            Preferences.Set(AppConstants.TokenFaceook, data);
                            MessagingCenter.Send<App>((App)Application.Current, AppConstants.GetTokenDone);
                        }
                    }
                }));
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