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
                throw;
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

        public Task<object> GetIdFriends(string token)
        {
            throw new NotImplementedException();
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

                    var friend = await GetInfoUser(token);
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
    }
}