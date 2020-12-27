using System.Net;
using Android.Content;
using Android.Webkit;
using AUTOHLT.MOBILE.Droid.CustomRenderer;
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
        public override void OnPageFinished(WebView? view, string? url)
        {
            base.OnPageFinished(view, url);
            //lấy cookie tại đây
            var cookieHeader = CookieManager.Instance.GetCookie(url);
            var cookies = new CookieCollection();
            var cookiePairs = cookieHeader.Split('&');
            foreach (var cookiePair in cookiePairs)
            {
                var cookiePieces = cookiePair.Split('=');
                if (cookiePieces[0].Contains(":"))
                    cookiePieces[0] = cookiePieces[0].Substring(0, cookiePieces[0].IndexOf(":"));
                cookies.Add(new Cookie
                {
                    Name = cookiePieces[0],
                    Value = cookiePieces[1]
                });
            }
        }
    }
}