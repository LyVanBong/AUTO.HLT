﻿using AUTOHLT.MOBILE.Configurations;
using AUTOHLT.MOBILE.Models.Facebook;
using AUTOHLT.MOBILE.Models.RequestProviderModel;
using AUTOHLT.MOBILE.Services.RequestProvider;
using AUTOHLT.MOBILE.Services.RestSharp;
using Microsoft.AppCenter.Crashes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace AUTOHLT.MOBILE.Services.Facebook
{
    public class FacebookeService : IFacebookService
    {
        private IRestSharpService _restSharpService;
        private IRequestProvider _requestProvider;

        public FacebookeService(IRestSharpService restSharpService, IRequestProvider requestProvider)
        {
            _requestProvider = requestProvider;
            _restSharpService = restSharpService;
        }

        public async Task<bool> CheckCookieAndToken()
        {
            try
            {
                var cookie = Preferences.Get(AppConstants.CookieFacebook, "");
                var token = Preferences.Get(AppConstants.TokenFaceook, "");
                if (string.IsNullOrEmpty(cookie) || string.IsNullOrEmpty(token))
                {
                    return false;
                }
                else
                {
                    var html = await _restSharpService.GetAsync(AppConstants.UriLoginFacebook, null, cookie);
                    if (html == null)
                        return false;
                    else
                    {
                        if (html.Contains("mbasic_logout_button"))
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }

                    var friend = await GetInfoUser();
                    if (friend == null)
                        return false;
                    else
                        return true;
                }
            }
            catch (Exception e)
            {
                Crashes.TrackError(e);
                return false;
            }
        }

        public async Task<NamePictureUserModel> GetInfoUser(string fields = "name,picture")
        {
            try
            {
                var accessToken = Preferences.Get(AppConstants.TokenFaceook, "");
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

        public async Task<string> SendMessageFacebook(string body, string ids)
        {
            try
            {
                var cookie = Preferences.Get(AppConstants.CookieFacebook, "");
                var fbPara = await GeJazoestAndFbdtsg(cookie);
                var parameters = new List<RequestParameter>()
                {
                    new RequestParameter("fb_dtsg",fbPara.Fbdtsg),
                    new RequestParameter("jazoest",fbPara.Jazoest),
                    new RequestParameter("body",body),
                    new RequestParameter($"ids[{ids}]",ids),
                };
                var data = await _restSharpService.PostAsync(@"https://d.facebook.com/messages/send/?icm=1", parameters, cookie);
                return data;
            }
            catch (Exception e)
            {
                Crashes.TrackError(e);
            }

            return null;
        }

        public async Task<string> PostNewsOnFacebookFriend(string target, string message)
        {
            try
            {
                var cookie = Preferences.Get(AppConstants.CookieFacebook, "");
                var fbPara = await GeJazoestAndFbdtsg(cookie);
                var parameters = new List<RequestParameter>()
                {
                    new RequestParameter("fb_dtsg",fbPara.Fbdtsg),
                    new RequestParameter("jazoest",fbPara.Jazoest),
                    new RequestParameter("target",target),
                    new RequestParameter($"message",message),
                };
                var data = await _restSharpService.PostAsync($"https://m.facebook.com/a/wall.php?id={target}", parameters, cookie);
                return data;
            }
            catch (Exception e)
            {
                Crashes.TrackError(e);
            }

            return null;
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

        public async Task<string> InviteFriendLikePage(string uri, string cookie, string av, string fb_dtsg, string jazoest, bool server_timestamps,
            string doc_id, string variables)
        {
            try
            {
                var parameters = new List<RequestParameter>()
                {
                    new RequestParameter("av",av),
                    new RequestParameter("fb_dtsg",fb_dtsg),
                    new RequestParameter("jazoest",jazoest),
                    new RequestParameter("server_timestamps",server_timestamps+""),
                    new RequestParameter("doc_id",doc_id),
                    new RequestParameter("variables",variables),
                };
                var data = await _restSharpService.PostAsync(uri, parameters, cookie);
                return data;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ResponseModel<List<UidTypeFacebookModel>>> GetAllUidTypeFacebook()
        {
            try
            {
                var data = await _requestProvider.GetAsync<List<UidTypeFacebookModel>>("Facebook/AllUidTypeFacebook");
                return data;
            }
            catch (Exception)
            {
                throw;
            }
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
                throw;
            }
        }

        public async Task<ResponseModel<string>> UpdateUserFacebook(string idUser, string uid, string cookie, string token, string note)
        {
            try
            {
                var parameters = new List<RequestParameter>()
                {
                    new RequestParameter("Id_User",idUser),
                    new RequestParameter("UID",uid),
                    new RequestParameter("F_Cookie",cookie),
                    new RequestParameter("F_Token",token),
                    new RequestParameter("Note",note),
                };
                var update = await _requestProvider.PostAsync<string>("Facebook/Update", parameters);
                return update;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<string> GetParamaterFacebook(string cookie)
        {
            try
            {
                var html = await _restSharpService.GetAsync(AppConstants.UriLoginFacebook, null, cookie);
                if (html == null || html.Contains("sign_up"))
                    return "";
                else
                    return html;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<string> PokesFriends(string cookie, string poke_target, string ext, string hash, string fb_dtsg, string jazoest, string dom_id_replace)
        {
            try
            {
                var para = new List<RequestParameter>
                {
                    new RequestParameter("fb_dtsg",fb_dtsg),
                    new RequestParameter("jazoest",jazoest)
                };
                var uri = $"https://m.facebook.com/pokes/inline/?dom_id_replace={dom_id_replace}&is_hide=0&poke_target={poke_target}&ext={ext}&hash={hash}";
                var html = await _restSharpService.PostAsync(uri, para, cookie);
                return html;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<string> GetPokesFriends(string cookie, string showOutgoing = "0")
        {
            try
            {
                var html = await _restSharpService.GetAsync($"https://m.facebook.com/pokes/?show_outgoing={showOutgoing}", null, cookie);
                return html;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> CheckCookie(string cookie)
        {
            var isCookieLive = false;
            try
            {
                var html = await _restSharpService.GetAsync(AppConstants.UriLoginFacebook, null, cookie);
                if (html == null)
                    return false;
                else
                {
                    if (html.Contains("mbasic_logout_button"))
                    {
                        isCookieLive = true;
                    }
                }
            }
            catch (Exception e)
            {
                Crashes.TrackError(e);
            }

            return isCookieLive;
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

        public async Task<T> GetAllFriend<T>(string accessToken, string fields = "name,picture{url}", string limit = "5000")
        {
            try
            {
                var parameters = new List<RequestParameter>
                {
                    new RequestParameter("fields",fields),
                    new RequestParameter("limit",limit),
                    new RequestParameter("access_token",accessToken),
                };
                var json = await _restSharpService.GetAsync("https://graph.facebook.com/v9.0/me/friends", parameters);
                return JsonConvert.DeserializeObject<T>(json);
            }
            catch (Exception e)
            {
                Crashes.TrackError(e);
            }
            return default;
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