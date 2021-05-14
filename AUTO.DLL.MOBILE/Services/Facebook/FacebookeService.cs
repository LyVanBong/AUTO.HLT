using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using AUTO.DLL.MOBILE.Configurations;
using AUTO.DLL.MOBILE.Models.Facebook;
using AUTO.DLL.MOBILE.Models.RequestProviderModel;
using AUTO.DLL.MOBILE.Services.RestSharp;
using Newtonsoft.Json;
using RestSharp;

namespace AUTO.DLL.MOBILE.Services.Facebook
{
    public class FacebookeService : IFacebookService
    {
        private IRestSharpService _restSharpService;

        public FacebookeService()
        {
            _restSharpService = new RestSharpService();
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
                if (regex.Any())
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

        public async Task<(List<string> LsUid, List<string> LsUsrName)> GetUIdFromPost(string cookie, string limit, string token)
        {
            var lsUid = new List<string>();
            var lsUsrName = new List<string>();
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
                            if (!string.IsNullOrEmpty(id))
                            {
                                lsUid.Add(id);
                            }
                            else
                            {
                                lsUsrName.Add(s);
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("Lỗi : " + e.ToString());
            }

            return (lsUid, lsUsrName);
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
                    if (regex.Any())
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