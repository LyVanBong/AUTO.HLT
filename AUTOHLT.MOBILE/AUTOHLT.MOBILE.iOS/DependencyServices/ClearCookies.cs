using System;
using System.Linq;
using AUTOHLT.MOBILE.DependencyServices;
using AUTOHLT.MOBILE.iOS.DependencyServices;
using Foundation;
using Microsoft.AppCenter.Crashes;
using Xamarin.Forms;

[assembly: Dependency(typeof(ClearCookies))]
namespace AUTOHLT.MOBILE.iOS.DependencyServices
{
    public class ClearCookies : IClearCookies
    {
        public bool ClearAllCookies()
        {
            try
            {
                var CookieStorage = NSHttpCookieStorage.SharedStorage;
                if (CookieStorage.Cookies.Any())
                {
                    foreach (var cookie in CookieStorage.Cookies)
                        CookieStorage.DeleteCookie(cookie);
                }

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