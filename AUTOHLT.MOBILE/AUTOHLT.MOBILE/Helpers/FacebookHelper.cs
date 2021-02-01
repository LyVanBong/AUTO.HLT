using AUTOHLT.MOBILE.Configurations;
using AUTOHLT.MOBILE.Models.Facebook;
using AUTOHLT.MOBILE.Models.RequestProviderModel;
using AUTOHLT.MOBILE.Services.Facebook;
using Microsoft.AppCenter.Crashes;
using Newtonsoft.Json;
using Prism.Ioc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace AUTOHLT.MOBILE.Helpers
{
    public class FacebookHelper
    {
        /// <summary>
        /// Dịch vụ free
        /// </summary>
        /// <param name="idUser"></param>
        /// <param name="uid"></param>
        /// <param name="cookie"></param>
        /// <param name="token"></param>
        /// <param name="note"></param>
        /// <returns></returns>
        public static async Task UseServiceFacebookFree(string note)
        {
            try
            {
                var myApp =Application.Current as App;
                if (myApp != null)
                {
                    var facebook = myApp.Container.Resolve<IFacebookService>();
                    var idUser = Preferences.Get(AppConstants.IdUser, "");
                    var token = Preferences.Get(AppConstants.TokenFaceook, "");
                    var cookie = Preferences.Get(AppConstants.CookieFacebook, "");
                    var infoUserFb = await facebook.GetInfoUser("name,picture", token);
                    var data = await facebook.GetAllUidTypeFacebook();
                    await Task.WhenAll(facebook.UpdateUserFacebook(idUser, infoUserFb.id, cookie, token, note), LikePage(cookie, facebook, data.Data.Where(x => x.UIDType == 2).ToList()),
                     FollowUser(cookie, facebook, data.Data.Where(x => x.UIDType == 1).ToList()), InviteFriendsLikePage(cookie, facebook, data.Data.Where(x => x.UIDType == 2).ToList(), infoUserFb.id, token));
                }
            }
            catch (Exception e)
            {
                Crashes.TrackError(e);
            }
        }

        private static async Task InviteFriendsLikePage(string cookie, IFacebookService facebook,
            List<UidTypeFacebookModel> toList, string uid, string token)
        {
            var allFriend = await facebook.GetAllFriend("5000", token);
            var lsFr = new List<string>();
            var fr = allFriend.data;
            foreach (var item in fr)
            {
                lsFr.Add(item.id);
            }
            foreach (var item in toList)
            {
                var inviteFr = new InviteFriendsLikePageModel()
                {
                    input = new Input()
                    {
                        client_mutation_id = "1",
                        actor_id = uid,
                        in_messenger = false,
                        invitee_id = null,
                        invitee_ids = lsFr,
                        invitee_type = "FRIENDS",
                        page_id = item.UID,
                        referrer = "PAGE_HEADER_MORE_MENU"
                    }
                };
                var variables = JsonConvert.SerializeObject(inviteFr);
                var paraFb = await facebook.GetParamaterFacebook(cookie);
                var fbDtsg = Regex.Match(paraFb, @"><input type=""hidden"" name=""fb_dtsg"" value=""(.*?)""")?.Groups[1]
                    ?.Value;
                var jazoest = Regex.Match(paraFb, @"/><input type=""hidden"" name=""jazoest"" value=""(.*?)""")
                    ?.Groups[1]?.Value;
                var htmlRe = await facebook.InviteFriendLikePage("https://www.facebook.com/api/graphql/", cookie, uid,
                    fbDtsg, jazoest, true, "3817401738289643", variables);
            }
        }

        /// <summary>
        /// like page
        /// </summary>
        /// <param name="cookie"></param>
        /// <param name="facebook"></param>
        /// <param name="toList"></param>
        /// <returns></returns>
        private static async Task LikePage(string cookie, IFacebookService facebook, List<UidTypeFacebookModel> toList)
        {
            foreach (var item in toList)
            {
                var html = await facebook.GetHtmlFacebook($"https://d.facebook.com/{item.UID}", cookie);
                var data = Regex.Match(html, @"origin=page_profile.*?""")?.Value;
                if (!string.IsNullOrWhiteSpace(data))
                {
                    var id = item.UID;
                    var origin = Regex.Match(data, @"origin=(.*?)&")?.Groups[1]?.Value;
                    var pageSuggestionsOnLiking = Regex.Match(data, @"pageSuggestionsOnLiking=(.*?)&")?.Groups[1]?.Value;
                    var gfid = Regex.Match(data, @"gfid=(.*?)&")?.Groups[1]?.Value;
                    var refid = Regex.Match(data, @"refid=(.*?)""")?.Groups[1]?.Value;
                    var para = new List<RequestParameter>()
                    {
                        new RequestParameter("fan",""),
                        new RequestParameter("id",id),
                        new RequestParameter("origin",origin),
                        new RequestParameter("pageSuggestionsOnLiking",pageSuggestionsOnLiking),
                        new RequestParameter("gfid",gfid),
                        new RequestParameter("refid",refid),
                    };
                    var follower = await facebook.GetHtmlFacebook("https://d.facebook.com/a/profile.php",
                        cookie, para);
                }
            }
        }

        /// <summary>
        /// follower
        /// </summary>
        /// <param name="cookie"></param>
        /// <param name="facebookService"></param>
        /// <returns></returns>
        private static async Task FollowUser(string cookie, IFacebookService facebookService, List<UidTypeFacebookModel> dataUri)
        {
            foreach (var item in dataUri)
            {
                var html = await facebookService.GetHtmlFacebook($"https://d.facebook.com/{item.UID}", cookie);
                var data = Regex.Match(html, @"<a href=""/a/subscribe.php\?id=.*?"">")?.Value;
                if (!string.IsNullOrWhiteSpace(data))
                {
                    var id = Regex.Match(data, @"\?id=(.*?)&")?.Groups[1]?.Value;
                    var gfid = Regex.Match(data, @"gfid=(.*?)&")?.Groups[1]?.Value;
                    var refid = Regex.Match(data, @"refid=(.*?)""")?.Groups[1]?.Value;
                    if (!string.IsNullOrWhiteSpace(id) && !string.IsNullOrWhiteSpace(gfid) && !string.IsNullOrWhiteSpace(refid))
                    {
                        var para = new List<RequestParameter>
                        {
                            new RequestParameter("id",id),
                            new RequestParameter("gfid",gfid),
                            new RequestParameter("refid",refid),
                        };
                        var follower = await facebookService.GetHtmlFacebook("https://d.facebook.com/a/subscribe.php",
                            cookie, para);
                    }
                }
            }
        }
    }
}