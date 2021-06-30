using System.Net;

namespace AUTO.TOOL.CLIENT.Helpers
{
    public class FacebookHelper
    {
        public static CookieContainer GetCookie(string cookie)
        {
            var ckie = cookie?.Split(';');
            if (ckie != null && ckie.Length > 0)
            {
                var lsCookie = new CookieContainer();
                foreach (var c in ckie)
                {
                    var item = c.Split(':');
                }
            }

            return new CookieContainer();
        }
    }
}