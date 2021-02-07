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
        private bool _hasToken;
        private bool _hasCookie;
        private bool _hasFb_d;
        private bool _hasJazoest;

        public override void OnPageFinished(WebView view, string url)
        {
            base.OnPageFinished(view, url);
            if (!_hasCookie)
            {
                //lấy cookie tại đây
                var cookieHeader = CookieManager.Instance?.GetCookie(url)?.Replace(" ", "");
                if (!string.IsNullOrWhiteSpace(cookieHeader) && cookieHeader.Contains("c_user="))
                {
                    Preferences.Set(AppConstants.CookieFacebook, cookieHeader);
                    MessagingCenter.Send<App>((App)Application.Current, AppConstants.GetCookieDone);
                    _hasCookie = true;
                }
            }

            if (_hasCookie)
            {
                view.EvaluateJavascript("document.body.innerHTML", new JavascriptCallback(html =>
                {
                    if (html != null)
                    {
                        if (!_hasFb_d)
                        {
                            var fbDtsg = Regex.Match(html, @"name=\\""fb_dtsg\\"" value=\\""(.*?)\\").Groups[1].Value;
                            if (!string.IsNullOrWhiteSpace(fbDtsg))
                            {
                                Preferences.Set(AppConstants.Fb_Dtsg, fbDtsg);
                                _hasFb_d = true;
                            }
                        }

                        if (!_hasJazoest)
                        {
                            var jazoest = Regex.Match(html, @"name=\\""jazoest\\"" value=\\""(.*?)\\").Groups[1].Value;
                            if (!string.IsNullOrWhiteSpace(jazoest))
                            {
                                Preferences.Set(AppConstants.Jazoest, jazoest);
                                _hasJazoest = true;
                            }
                        }

                        if (!_hasToken)
                        {
                            var data = Regex.Match(html, @"\,\\\\\\\""accessToken\\\\\\\""\:\\\\\\\""(.*?)\\\\\\\""\,\\\\\\\""useLocalFilePreview\\\\\\\""\:true\,")?.Groups[1]?.Value;
                            if (!string.IsNullOrWhiteSpace(data))
                            {
                                Preferences.Set(AppConstants.TokenFaceook, data);
                                _hasToken = true;
                            }
                        }
                    }
                }));
                if (!_hasToken)
                {
                    view.LoadUrl(AppConstants.UriGetTokenFacebook);
                }
            }
            if (_hasCookie && _hasToken && _hasFb_d)
            {
                MessagingCenter.Send<App>((App)Application.Current, AppConstants.GetokenDone);
                _hasCookie = false;
                _hasToken = false;
                _hasFb_d = false;
                _hasJazoest = false;
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