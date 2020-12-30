using System;
using System.Threading.Tasks;
using Android.Webkit;
using AUTOHLT.MOBILE.DependencyServices;
using AUTOHLT.MOBILE.Droid.DependencyServices;
using Microsoft.AppCenter.Crashes;
using Xamarin.Forms;

[assembly: Dependency(typeof(ClearCookies))]
namespace AUTOHLT.MOBILE.Droid.DependencyServices
{
    public class ClearCookies : IClearCookies
    {
        public bool ClearAllCookies()
        {
            try
            {
                var cookieManager = CookieManager.Instance;
                if (cookieManager != null)
                    cookieManager.RemoveAllCookie();
                return true;
            }
            catch (Exception e)
            {
                Crashes.TrackError(e);
            }

            return false;
        }
    }
}