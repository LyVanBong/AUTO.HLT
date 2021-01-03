using AUTOHLT.MOBILE.Models.Facebook;
using AUTOHLT.MOBILE.Models.RequestProviderModel;
using AUTOHLT.MOBILE.Services.RestSharp;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AUTOHLT.MOBILE.Configurations;

namespace AUTOHLT.MOBILE.Services.Facebook
{
    public class FacebookeService : IFacebookService
    {
        private IRestSharpService _restSharpService;
        public FacebookeService(IRestSharpService restSharpService)
        {
            _restSharpService = restSharpService;
        }

        public async Task<bool> CheckCookie(string cookie)
        {
            try
            {
                var html = await _restSharpService.GetAsync(AppConstants.UriLoginFacebook, null, cookie);
                if (html == null || html.Contains("sign_up"))
                    return false;
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<string> UnFriend(string fb_dtsg, string jazoest, string friend_id, string cookie)
        {
            try
            {
                var para = new List<RequestParameter>
                {
                    new RequestParameter("fb_dtsg",fb_dtsg),
                    new RequestParameter("jazoest",jazoest),
                    new RequestParameter("friend_id",friend_id),
                };
                var data = await _restSharpService.PostAsync("https://d.facebook.com/a/removefriend.php", para, cookie);
                return data;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<FriendsModel> GetAllFriend(string fields, string accessToken)
        {
            try
            {
                var parameters = new List<RequestParameter>
                {
                    new RequestParameter("fields","name,picture{url}"),
                    new RequestParameter("limit",fields),
                    new RequestParameter("access_token",accessToken),
                };
                var json = await _restSharpService.GetAsync("https://graph.facebook.com/v9.0/me/friends", parameters);
                if (json != null)
                    return JsonConvert.DeserializeObject<FriendsModel>(json);
                return null;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<string> GetFriendsDoNotInteract(string fbDtsg, string q)
        {
            try
            {
                var para = new List<RequestParameter>
                {
                    new RequestParameter("fb_dtsg",fbDtsg),
                    new RequestParameter("q",q),
                };
                var data = await _restSharpService.PostAsync("https://www.facebook.com/api/graphql/", para);
                return data;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<NamePictureUserModel> GetInfoUser(string fields, string accessToken)
        {
            try
            {
                var para = new List<RequestParameter>
                {
                    new RequestParameter("fields",fields),
                    new RequestParameter("access_token",accessToken),
                };
                var data = await _restSharpService.GetAsync("https://graph.facebook.com/v9.0/me", para);
                if (data != null)
                {
                    var info = JsonConvert.DeserializeObject<NamePictureUserModel>(data);
                    if (info != null)
                        return info;
                }
                return null;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}