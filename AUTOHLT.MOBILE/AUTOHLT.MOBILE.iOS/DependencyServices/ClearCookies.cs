using AUTOHLT.MOBILE.DependencyServices;
using AUTOHLT.MOBILE.iOS.DependencyServices;
using Foundation;
using Microsoft.AppCenter.Crashes;
using System;
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
                NSHttpCookieStorage cookieStorage = NSHttpCookieStorage.SharedStorage;
                foreach (var cookie in cookieStorage.Cookies)
                    cookieStorage.DeleteCookie(cookie);
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