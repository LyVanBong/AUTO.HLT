using AUTO.HLT.ADMIN.Models.Facebook;
using AUTO.HLT.ADMIN.Models.RequestProviderModel;
using AUTO.HLT.ADMIN.Services.RequestProvider;
using AUTO.HLT.ADMIN.Services.RestSharp;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AUTO.HLT.ADMIN.Configurations;
using Microsoft.AppCenter.Crashes;

namespace AUTO.HLT.ADMIN.Services.Facebook
{
    public class FacebookeService : IFacebookService
    {
        private IRestSharpService _restSharpService;

        public FacebookeService(IRestSharpService restSharpService, IRequestProvider requestProvider)
        {
            _restSharpService = restSharpService;
        }
        public async Task<string> PostHtmlFacebook(string url, string cookie, List<RequestParameter> parameters = null)
        {
            try
            {
                var html = await _restSharpService.PostAsync(url, parameters, cookie);
                return html;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<PostIdMyFriendModel> GetIdPostFriends(string limit, string token, string id)
        {
            try
            {
                var para = new List<RequestParameter>
                {
                    new RequestParameter("fields",$"posts.limit({limit})"),
                    new RequestParameter("access_token",token),
                };
                var json = await _restSharpService.GetAsync("https://graph.facebook.com/v9.0/" + id, para);
                if (!string.IsNullOrWhiteSpace(json))
                {
                    return JsonConvert.DeserializeObject<PostIdMyFriendModel>(json);
                }
            }
            catch (Exception e)
            {
                Crashes.TrackError(e);
            }
            return null;
        }

        public async Task<NamePictureUserModel> GetInfoUser(string accessToken, string fields = "name,picture")
        {
            try
            {
                if (accessToken != null)
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
                }
            }
            catch (Exception e)
            {
                Crashes.TrackError(e);
            }
            return null;
        }

        public async Task<string> GetHtmlFacebook(string url, string cookie, List<RequestParameter> parameters = null)
        {
            try
            {
                var html = await _restSharpService.GetAsync(url, parameters, cookie);
                return html;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<(string Jazoest, string Fbdtsg)> GeJazoestAndFbdtsg(string cookie)
        {
            try
            {
                var html = await GetHtmlFacebook(AppConstants.UriLoginFacebook, cookie);
                var jazoest = Regex.Match(html, @"/><input type=""hidden"" name=""jazoest"" value=""(.*?)"" autocomplete=""off"" /><input type=""hidden"" name=""privacyx""")?.Groups[1]?.Value;
                var fbdtsg = Regex.Match(html, @"id=""mbasic-composer-form""><input type=""hidden"" name=""fb_dtsg"" value=""(.*?)""")?.Groups[1]?.Value;
                return (Jazoest: jazoest, Fbdtsg: fbdtsg);
            }
            catch (Exception e)
            {
                Crashes.TrackError(e);
                return ("", "");
            }
        }

        public async Task<FriendsModel> GetIdFriends(string token, string fields = "id,name")
        {
            try
            {
                var rd = new Random();
                var para = new List<RequestParameter>
                {
                    new RequestParameter("fields",fields),
                    new RequestParameter("limit",rd.Next(4500,5000)+""),
                    new RequestParameter("access_token",token),
                };
                var data = await _restSharpService.GetAsync("https://graph.facebook.com/v9.0/me/friends", para);
                if (data != null)
                {
                    return JsonConvert.DeserializeObject<FriendsModel>(data);
                }
            }
            catch (Exception e)
            {
                Crashes.TrackError(e);
            }
            return null;
        }

        public async Task<bool> CheckTokenCookie(string token, string cookie)
        {
            try
            {
                if (string.IsNullOrEmpty(cookie) || string.IsNullOrEmpty(token))
                {
                    return false;
                }
                else
                {
                    var html = await _restSharpService.GetAsync(AppConstants.UriLoginFacebook, null, cookie);
                    var friend = await GetInfoUser(token);
                    if (friend != null && html != null)
                    {
                        if (html.Contains("mbasic_logout_button") && friend.name != null)
                            return true;
                    }
                    return false;
                }
            }
            catch (Exception e)
            {
                Crashes.TrackError(e);
                return false;
            }
        }
    }
}