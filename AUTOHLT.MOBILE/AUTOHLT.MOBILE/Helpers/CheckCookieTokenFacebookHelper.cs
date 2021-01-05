using System;
using System.Threading.Tasks;
using AUTOHLT.MOBILE.Services.Facebook;
using Microsoft.AppCenter.Crashes;

namespace AUTOHLT.MOBILE.Helpers
{
    public class CheckCookieTokenFacebookHelper
    {
        private static IFacebookService _facebookService;
        public CheckCookieTokenFacebookHelper(IFacebookService facebookService)
        {
            _facebookService = facebookService;
        }
        /// <summary>
        /// Kiêm tra token và cookie còn sống không
        /// </summary>
        /// <param name="cookie"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public static async Task<bool> CheckData(string cookie, string token)
        {

            try
            {
                var isCookie = await _facebookService.CheckCookie(cookie);
                if (!isCookie)
                {
                    return false;
                }

                var friends = await _facebookService.GetAllFriend("1", token);
                if (friends == null || friends.data == null)
                {
                    return false;
                }

                return true;
            }
            catch (Exception e)
            {
                Crashes.TrackError(e);
                return false;
            }
        }
    }
}