﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using AUTO.MOBILE.DLL.Configurations;
using AUTO.MOBILE.DLL.Models.Facebook;
using AUTO.MOBILE.DLL.Models.RequestProviderModel;
using AUTO.MOBILE.DLL.Services.RestSharp;
using Newtonsoft.Json;

namespace AUTO.MOBILE.DLL.Services.Facebook
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
                var urlFacebook = "https://m.facebook.com";
                var para = new List<RequestParameter>
                {
                    new RequestParameter("Content-Type", "application/x-www-form-urlencoded"),
                    new RequestParameter("linkCheckUid", urlFacebook+urlProfile),
                };
                var data = await _restSharpService.PostAsync(AppConstants.UriGetIdFacebook, para);
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

        public async Task<List<string>> FindIdFromHtmlComment(string comment)
        {
            var data = new List<string>();

            try
            {
                var regex = Regex.Matches(comment, @"<div><h3><a class=""\w+ \w+"" href=""(.*?)refid=[0-9]+&amp;__tn__=R""");
                if (regex.Any())
                {
                    var listThread = new List<Task<string>>();
                    foreach (Match o in regex)
                    {
                        listThread.Add(GetIdFromProfileFacebook(o?.Groups[1]?.Value));
                    }

                    var res = await Task.WhenAll(listThread);
                    if (res.Any())
                    {
                        data = res.ToList();
                    }
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("Lỗi : " + e.ToString());
            }

            return data;
        }

        public async Task<List<string>> GetUIdFromPost(string cookie, string limit, string token)
        {
            var data = new List<string>();

            try
            {
                var lsIdPost = await GetIdMyPost(cookie, token, limit);
                if (lsIdPost.Any())
                {
                    var stt = 1;
                    foreach (var item in lsIdPost)
                    {
                        Console.WriteLine("#STT : " + stt++);
                        Console.WriteLine("Id : " + item.id);
                        var comment = await GetUIdCommentFromAPost(cookie, item.id);
                        Console.WriteLine("Comment");
                        Console.WriteLine("Số lượng : " + comment.Count);
                        if (comment.Any())
                        {
                            data.AddRange(comment);
                        }
                        var like = await GetUIdLikeFromAPost(cookie, item.id);
                        Console.WriteLine("Like");
                        Console.WriteLine("Số lượng : " + like.Count);
                        if (like.Any())
                        {
                            data.AddRange(like);
                        }

                        Console.WriteLine("----------------------------------------------------");
                    }
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("Lỗi : " + e.ToString());
            }

            return data;
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
                        var httml2 = await _restSharpService.GetAsync(
                            AppConstants.UriLoginFacebook + HttpUtility.HtmlDecode(urlSeenComment), null, cookie);
                        var lsId2 = await FindIdFromHtmlComment(httml2);
                        if (lsId2.Any())
                        {
                            data.AddRange(lsId2);
                        }
                        if (httml2.Contains(@"id=""see_prev_"))
                            while (true)
                            {
                                if (!httml2.Contains(@"id=""see_prev_"))
                                {
                                    break;
                                }

                                var urlSeenComment3 = Regex.Matches(httml2, @"><a href=""(/story.php\?story_fbid.*?)""")[0]?.Groups[1]?.Value;
                                var html3 = await _restSharpService.GetAsync(
                                    AppConstants.UriLoginFacebook + HttpUtility.HtmlDecode(urlSeenComment3), null,
                                    cookie);
                                var lsId3 = await FindIdFromHtmlComment(html3);
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

                        var listThread = new List<Task<string>>();
                        foreach (Match o in regex)
                        {
                            listThread.Add(GetIdFromProfileFacebook(o?.Groups[1]?.Value));
                        }

                        var res = await Task.WhenAll(listThread);
                        if (res.Any())
                        {
                            data = res.ToList();
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