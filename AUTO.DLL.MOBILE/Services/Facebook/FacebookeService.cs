using AUTO.DLL.MOBILE.Configurations;
using AUTO.DLL.MOBILE.Models.Facebook;
using AUTO.DLL.MOBILE.Models.RequestProviderModel;
using AUTO.DLL.MOBILE.Services.RestSharp;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace AUTO.DLL.MOBILE.Services.Facebook
{
    public class FacebookeService : IFacebookService
    {
        private IRestSharpService _restSharpService;

        public FacebookeService()
        {
            _restSharpService = new RestSharpService();
        }

        public async Task<(string Jazoest, string Fbdtsg)> Getarameter(string cookie)
        {
            var jazoest = "";
            var fbdtsg = "";
            try
            {
                var html = await _restSharpService.GetAsync("https://d.facebook.com/", null, cookie);
                jazoest = Regex.Match(html, @"/><input type=""hidden"" name=""jazoest"" value=""(.*?)"" autocomplete=""off"" /><input type=""hidden"" name=""privacyx""")?.Groups[1]?.Value;
                fbdtsg = Regex.Match(html, @"id=""mbasic-composer-form""><input type=""hidden"" name=""fb_dtsg"" value=""(.*?)""")?.Groups[1]?.Value;
            }
            catch (Exception e)
            {
                Debug.WriteLine("Lỗi : " + e.ToString());
            }
            return (jazoest, fbdtsg);
        }

        public async Task<bool> PokeFriend(string cookie, PokesFriendsModel friend)
        {
            var isPoke = false;
            try
            {
                var getPara = await Getarameter(cookie);
                var fb_dtsg = getPara.Fbdtsg;
                var jazoest = getPara.Jazoest;
                if (!string.IsNullOrEmpty(fb_dtsg) && !string.IsNullOrEmpty(jazoest))
                {
                    var para = new List<RequestParameter>
                    {
                        new RequestParameter("fb_dtsg",fb_dtsg),
                        new RequestParameter("jazoest",jazoest)
                    };
                    var uri = $"https://m.facebook.com/pokes/inline/?dom_id_replace={friend.DomIdReplace}&is_hide=0&poke_target={friend.UId}&ext={friend.Ext}&hash={friend.Hash}";
                    var html = await _restSharpService.PostAsync(uri, para, cookie);
                    if (html.Contains("mbasic_logout_button"))
                        isPoke = true;
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("Lỗi : " + e.ToString());
            }
            return isPoke;
        }

        public async Task<List<PokesFriendsModel>> GetFriendPoke(string cookie)
        {
            List<PokesFriendsModel> data = null;
            try
            {
                var html = await _restSharpService.GetAsync(@"https://d.facebook.com/pokes/?show_outgoing=0", null, cookie);
                Regex regex = new Regex(@"<div class=""br"" id="".*?></div></div><div class=""cl""></div></div></div></div></div>", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Multiline | RegexOptions.Singleline);
                var matchCollection = regex.Matches(html);
                if (matchCollection.Count > 0)
                {
                    data = new List<PokesFriendsModel>();
                    foreach (Match match in matchCollection)
                    {
                        var machData = match.Value;
                        var poke = new PokesFriendsModel();
                        poke.FullName = Regex.Match(machData, @"<a class=""cc"" href="".*?"">(.*?)</a>")
                            ?.Groups[1]?.Value;
                        poke.IsPokes = false;
                        var uri = Regex.Match(machData, @"/pokes/inline/\?dom_id_replace(.*?)""")?.Groups[1]?.Value;
                        poke.Ext = Regex.Match(uri, @";ext=(.*?)&")?.Groups[1]?.Value;
                        poke.Hash = Regex.Match($"{uri}\"", @";hash=(.*?)""")?.Groups[1]?.Value;
                        poke.UId = Regex.Match(uri, @";poke_target=(.*?)&")?.Groups[1]?.Value;
                        poke.DomIdReplace = Regex.Match(uri, @"=(.*?)&amp;is_hide")?.Groups[1]?.Value;
                        poke.UrlAvatar =
                            HttpUtility.HtmlDecode(Regex.Match(machData, @"(https://scontent.*?)""")?.Groups[1]?.Value);
                        data.Add(poke);
                    }
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("Lỗi : " + e.ToString());
            }

            return data;
        }

        public async Task<T> GetInfoUser<T>(string token, string fields)
        {
            try
            {
                if (token != null)
                {
                    var para = new List<RequestParameter>
                    {
                        new RequestParameter("fields",fields),
                        new RequestParameter("access_token",token),
                    };
                    var data = await _restSharpService.GetAsync("https://graph.facebook.com/v9.0/me", para);
                    if (data != null)
                    {
                        var info = JsonConvert.DeserializeObject<T>(data);
                        if (info != null)
                            return info;
                    }
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("Lỗi : " + e.ToString());
            }
            return default;
        }

        public async Task<bool> CheckCookieAndToken(string cookie, string token)
        {
            var isLive = false;

            try
            {
                if (string.IsNullOrEmpty(cookie) || string.IsNullOrEmpty(token))
                {
                    isLive = false;
                }
                else
                {
                    var html = await _restSharpService.GetAsync(AppConstants.UriLoginFacebook, null, cookie);
                    if (html == null)
                        isLive = false;
                    else
                    {
                        if (html.Contains("mbasic_logout_button"))
                        {
                            isLive = true;
                        }
                        else
                        {
                            isLive = false;
                        }
                    }

                    var friend = await GetInfoUser<object>(token, "name");
                    if (friend == null)
                        isLive = false;
                    else
                        isLive = true;
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("Lỗi : " + e.ToString());
            }
            return isLive;
        }

        public async Task<bool> UnFriend(string cookie, string uid)
        {
            var isUnfriend = false;

            try
            {
                var htmlProfile = await _restSharpService.GetAsync("https://d.facebook.com/" + uid, null, cookie);
                var matchUid = Regex.Match(htmlProfile, @"<div class=""\w+ \w+""><a href=""/photo.php\?fbid=.*?id=(.*?)&");
                var matchDtsg = Regex.Match(htmlProfile, @"name=""fb_dtsg"" value=""(.*?)""");
                var matchJazoest = Regex.Match(htmlProfile, @"name=""jazoest"" value=""(.*?)""");
                var id = matchUid?.Groups[1]?.Value;
                var fb_dtsg = matchDtsg?.Groups[1]?.Value;
                var jazoest = matchJazoest?.Groups[1]?.Value;
                var name = Regex.Match(htmlProfile, @"<title>(.*?)</title>")?.Groups[1]?.Value;
                if (!string.IsNullOrEmpty(id) && !string.IsNullOrEmpty(fb_dtsg) && !string.IsNullOrEmpty(jazoest))
                {
                    var para = new List<RequestParameter>
                    {
                        new RequestParameter("fb_dtsg",fb_dtsg),
                        new RequestParameter("jazoest",jazoest),
                        new RequestParameter("friend_id",id),
                    };
                    var html = await _restSharpService.PostAsync("https://d.facebook.com/a/removefriend.php", para, cookie);
                    if (html.Contains(name))
                    {
                        isUnfriend = true;
                    }
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("Lỗi : " + e.ToString());
            }

            return isUnfriend;
        }

        public async Task<List<MyFriendModel>> GetMyFriend(string cookie)
        {
            var lsFriend = new List<MyFriendModel>();
            var stt = 1;
            try
            {
#if DEBUG
                var count = 1;
                Console.WriteLine("## " + count++);
#endif
                var html = await _restSharpService.GetAsync("https://d.facebook.com/profile.php?v=friends", null, cookie);
                var regex = Regex.Matches(html, @"style=""vertical-align: middle""><img src=""(.*?)"".href=""/(.*?)"">(.*?)</a>");
                if (regex.Count>0)
                {
                    foreach (Match o in regex)
                    {
                        var picture = HttpUtility.HtmlDecode(Regex.Match(o.Groups[1].Value, @"(https.*?)""").Groups[1].Value);
                        var regexUid = Regex.Match(o.Groups[2]?.Value, @"profile.php\?id=(\d+)");
                        var uid = regexUid.Groups[1].Value;
                        if (string.IsNullOrEmpty(uid))
                        {
                            var regexUsr = Regex.Match(o.Groups[2]?.Value, @"(.*?)\?");
                            uid = regexUsr.Groups[1].Value;
                        }
                        lsFriend.Add(new MyFriendModel(stt++, uid, o?.Groups[3]?.Value, 0, "", false, picture));
                    }
                }

                if (html.Contains("m_more_friends"))
                {
                    while (true)
                    {
#if DEBUG
                        Console.WriteLine("## " + count++);
#endif
                        var urlMoreFriend = Regex.Match(html, @"id=""m_more_friends""><a href=""(.*?)""");
                        html = await _restSharpService.GetAsync("https://d.facebook.com" + HttpUtility.HtmlDecode(urlMoreFriend?.Groups[1]?.Value), null, cookie);
                        var regex2 = Regex.Matches(html, @"style=""vertical-align: middle""><img src=""(.*?)"".href=""/(.*?)"">(.*?)</a>");
                        if (regex2.Count > 0)
                        {
                            foreach (Match o in regex2)
                            {
                                var picture = HttpUtility.HtmlDecode(Regex.Match(o.Groups[1].Value, @"(https.*?)""").Groups[1].Value);
                                var regexUid = Regex.Match(o.Groups[2]?.Value, @"profile.php\?id=(\d+)");
                                var uid = regexUid.Groups[1].Value;
                                if (string.IsNullOrEmpty(uid))
                                {
                                    var regexUsr = Regex.Match(o.Groups[2]?.Value, @"(.*?)\?");
                                    uid = regexUsr.Groups[1].Value;
                                }
                                lsFriend.Add(new MyFriendModel(stt++, uid, o?.Groups[3]?.Value, 0, "", false, picture));
                            }
                        }
                        if (!html.Contains("m_more_friends"))
                            break;
                    }
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("Lỗi : " + e.ToString());
            }

            return lsFriend;
        }

        public async Task<string> GetIdFromProfileFacebook(string urlProfile)
        {
            var id = "";
            try
            {
                var client = new RestClient("https://id.atpsoftware.vn/");
                client.Timeout = -1;
                var request = new RestRequest(Method.POST);
                request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
                request.AddParameter("linkCheckUid", "https://m.facebook.com" + urlProfile);
                var response = await client.ExecuteAsync(request);

                var data = response.Content;
                var regex = new Regex(@"<textarea.*?"">(.*?)</textarea>", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Multiline | RegexOptions.Singleline);
                var strId = regex.Match(data)?.Groups[1]?.Value;
                if (!string.IsNullOrEmpty(strId))
                {
                    id = Regex.Match(strId, @"\d+")?.Value;
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("Lỗi : " + e.ToString());
            }
            return id;
        }

        public async Task<List<Datum>> GetIdMyPost(string cookie, string token, string limit)
        {
            var data = new List<Datum>();

            try
            {
                var para = new List<RequestParameter>
                {
                    new RequestParameter("fields","posts.limit("+limit+"){id}"),
                    new RequestParameter("access_token",token),
                };
                var json = await _restSharpService.GetAsync(AppConstants.UriGetApiFacebook, para, cookie);
                if (!string.IsNullOrEmpty(json))
                {
                    var obj = JsonConvert.DeserializeObject<IdMyPostModel>(json);
                    if (obj != null)
                    {
                        data = obj.posts?.data.ToList();
                    }
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("Lỗi : " + e.ToString());
            }

            return data;
        }

        public Task<List<string>> FindIdFromHtmlComment(string comment)
        {
            var data = new List<string>();

            try
            {
                var regex = Regex.Matches(comment, @"<div><h3><a class=""\w+ \w+"" href=""(.*?)refid=[0-9]+&amp;__tn__=R""");
                if (regex.Count > 0)
                {
                    foreach (Match o in regex)
                    {
                        data.Add(o?.Groups[1]?.Value);
                    }
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("Lỗi : " + e.ToString());
            }

            return Task.FromResult(data);
        }

        public async Task<List<string>> GetUIdFromPost(string cookie, string token, string limit)
        {
            var lsUid = new List<string>();
            try
            {
                var lsIdPost = await GetIdMyPost(cookie, token, limit);
                if (lsIdPost.Any())
                {
#if DEBUG
                    var stt = 1;
#endif
                    var lsTask = new List<Task<List<string>>>();
                    foreach (var item in lsIdPost)
                    {
#if DEBUG
                        Console.WriteLine("#" + stt++ + " : " + item.id);
#endif
                        lsTask.Add(GetUIdCommentFromAPost(cookie, item.id));
                        lsTask.Add(GetUIdLikeFromAPost(cookie, item.id));
                    }

                    var resTask = await Task.WhenAll(lsTask);
                    foreach (var list in resTask)
                    {
                        foreach (var s in list)
                        {
                            var regex = Regex.Match(s, @"/profile.php\?id=(\d+)");
                            var id = regex?.Groups[1]?.Value;
                            if (string.IsNullOrEmpty(id))
                            {
                                id = Regex.Match(s, @"/(.*?)\?")?.Groups[1].Value;
                                if (string.IsNullOrEmpty(id) && !s.Contains("?"))
                                {
                                    id = s.Remove(0, 1);
                                }
                            }
                            lsUid.Add(id);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("Lỗi : " + e.ToString());
            }

            return lsUid;
        }

        public async Task<List<string>> GetUIdCommentFromAPost(string cookie, string id)
        {
            var data = new List<string>();

            try
            {
                var html1 = await _restSharpService.GetAsync(AppConstants.UriLoginFacebook + "/" + id, null, cookie);
                if (!string.IsNullOrEmpty(html1))
                {
                    var lsId1 = await FindIdFromHtmlComment(html1);
                    if (lsId1.Any())
                    {
                        data.AddRange(lsId1);
                    }
                    if (html1.Contains(@"id=""see_prev_"))
                    {
                        var urlSeenComment = Regex.Match(html1, @"><a href=""(/story.php\?story_fbid.*?)""").Groups[1].Value;
                        var html2 = await _restSharpService.GetAsync(
                            AppConstants.UriLoginFacebook + HttpUtility.HtmlDecode(urlSeenComment), null, cookie);
                        var lsId2 = await FindIdFromHtmlComment(html2);
                        if (lsId2.Any())
                        {
                            data.AddRange(lsId2);
                        }
                        if (html2.Contains(@"id=""see_prev_"))
                            while (true)
                            {
                                if (!html2.Contains(@"id=""see_prev_"))
                                {
                                    break;
                                }

                                var urlSeenComment3 = Regex.Matches(html2, @"><a href=""(/story.php\?story_fbid.*?)""")[0]?.Groups[1]?.Value;
                                html2 = await _restSharpService.GetAsync(
                                    AppConstants.UriLoginFacebook + HttpUtility.HtmlDecode(urlSeenComment3), null,
                                    cookie);
                                var lsId3 = await FindIdFromHtmlComment(html2);
                                if (lsId3.Any())
                                {
                                    data.AddRange(lsId3);
                                }
                            }
                    }
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("Lỗi : " + e.ToString());
            }

            return data;
        }

        public async Task<List<string>> GetUIdLikeFromAPost(string cookie, string id)
        {
            var data = new List<string>();

            try
            {
                var idPost = id.Split('_')[1];
                var random = new Random();
                var para = new List<RequestParameter>()
                {
                    new RequestParameter("limit",random.Next(8000,9000)+""),
                    new RequestParameter("total_count",random.Next(9000,10000)+""),
                    new RequestParameter("ft_ent_identifier",idPost),
                };
                var html = await _restSharpService.GetAsync(AppConstants.UriGetHtmlLikePostFacebook, para, cookie);
                if (!string.IsNullOrEmpty(html))
                {
                    var regex = Regex.Matches(html, @"<div><h3 class=""\w+""><a href=""(.*?)"">");
                    if (regex.Count > 0)
                    {
                        foreach (Match o in regex)
                        {
                            data.Add(o?.Groups[1]?.Value);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("Lỗi : " + e.ToString());
            }

            return data;
        }
    }
}