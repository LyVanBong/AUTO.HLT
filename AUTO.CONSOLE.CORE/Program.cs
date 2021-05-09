using RestSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json;
using AUTO.MOBILE.DLL.Services.Facebook;

namespace AUTO.CONSOLE.CORE
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var cookie = "sb=Ez2zX3CsnRDDIuCCSAJ8cAvo;datr=FD2zXxwc0iA3H6gp2TElljSn;_fbp=fb.1.1606969798014.1362959027;c_user=100003841239701;wd=1920x912;spin=r.1003758647_b.trunk_t.1620453701_s.1_v.2_;xs=46%3AKbrCPAGwgTJKoA%3A2%3A1618657461%3A-1%3A6373%3A%3AAcUWuj5yb2PKsA2iB75aF6WAT8kLkCfixNNYG92v25IN;fr=1LlYD8oxnLiouamCy.AWWjnDDmuio7I-MBZqB8Ziw903M.BglzeK.Ya.AAA.0.0.BglzeK.AWXAoi2D9Vc;presence=C%7B%22t3%22%3A%5B%7B%22i%22%3A%22u.100005090865078%22%7D%2C%7B%22i%22%3A%22u.100018610031687%22%7D%2C%7B%22i%22%3A%22u.100067330628348%22%7D%2C%7B%22i%22%3A%22u.100005090865078%22%7D%2C%7B%22i%22%3A%22g.2451271764968022%22%7D%2C%7B%22i%22%3A%22u.100019113150057%22%7D%2C%7B%22i%22%3A%22u.100005601702441%22%7D%2C%7B%22i%22%3A%22u.100011570709490%22%7D%2C%7B%22i%22%3A%22u.100014391513978%22%7D%2C%7B%22i%22%3A%22u.100004094209207%22%7D%2C%7B%22i%22%3A%22u.100003813486017%22%7D%2C%7B%22i%22%3A%22u.100001842012404%22%7D%5D%2C%22utc3%22%3A1620524289314%2C%22lm3%22%3A%22u.100005090865078%22%2C%22v%22%3A1%7D";
            var token = "EAAAAZAw4FxQIBAMgNYLGRwjrHnXlWnZAdZC8GZBHsTyynGt01SocgB6FrnjmJgj210CZBjGB1E4k1QmKrZCQrSzUOPDI88MFCte2vAytZCmn6REQt01FcGMXiDZCSeAAFfl5wsyCNGBTWclzubRpY0ek7xMTOlQPaSLGLRnlgnavZAXOhUdaXJLyGaaZCnzRMrxvsZD";
            var facebooke = new FacebookeService();
            var list = await facebooke.GetUIdFromPost(cookie, "15", token);
            //var time = new Stopwatch();
            //time.Start();
            //var idpost = GetIdPost(token, cookie);
            //var lsId = new List<string>();
            //Console.OutputEncoding = Encoding.UTF8;
            //var stt = 1;
            //if (idpost.Any())
            //{
            //    foreach (var item in idpost)
            //    {
            //        Console.WriteLine("#STT : " + stt++);
            //        Console.WriteLine("Id : " + item.id);
            //        var data = await GetFriendComment(cookie, item.id);
            //        Console.WriteLine("Comment");
            //        Console.WriteLine("Time Run : " + time.ElapsedMilliseconds / 1000 + " s");
            //        Console.WriteLine("Số lượng : " + data.Count);
            //        if (data.Any())
            //        {
            //            lsId.AddRange(data);
            //        }
            //        var data2 = await GetFriendLike(cookie, item.id);
            //        Console.WriteLine("Like");
            //        Console.WriteLine("Time Run : " + time.ElapsedMilliseconds / 1000 + " s");
            //        Console.WriteLine("Số lượng : " + data2.Count);
            //        if (data2.Any())
            //        {
            //            lsId.AddRange(data2);
            //        }

            //        Console.WriteLine("----------------------------------------------------");
            //    }
            //}
            //time.Stop();
            //Console.WriteLine("Ket qua");
            //Console.WriteLine("Time Run : " + time.ElapsedMilliseconds / 1000 + " s");
            //Console.WriteLine("Số lượng : " + lsId.Count);
            //Console.ReadKey();
        }

        private static async Task<List<string>> GetFriendLike(string cookie, string itemId)
        {
            var lsId = new List<string>();

            var idPost = itemId.Split('_')[1];

            var client = new RestClient("https://d.facebook.com/ufi/reaction/profile/browser/fetch/?limit=10000&total_count=10000&ft_ent_identifier=" + idPost);
            client.Timeout = -1;
            var request = new RestRequest(Method.GET);
            request.AddHeader("cookie", cookie);
            var response = client.Execute(request);
            var content = response.Content;

            var regex = Regex.Matches(content, @"<div><h3 class=""\w+""><a href=""(.*?)"">");
            if (regex.Any())
            {

                var listThread = new List<Task<string>>();
                foreach (Match o in regex)
                {
                    listThread.Add(GetIdProfileFacebook(o?.Groups[1]?.Value));
                }

                var res = await Task.WhenAll(listThread);
                if (res.Any())
                {
                    lsId = res.ToList();
                }
            }
            return lsId;
        }

        private static List<Datum> GetIdPost(string token, string cookie)
        {
            var lsId = new List<Datum>();
            var client = new RestClient(
                "https://graph.facebook.com/v10.0/me?fields=posts.limit(100){id}&access_token=" + token);
            client.Timeout = -1;
            var request = new RestRequest(Method.GET);
            request.AddHeader("Cookie", cookie);
            var response = client.Execute(request);

            var data = JsonConvert.DeserializeObject<IdPostModel>(response.Content);

            if (data != null)
            {
                lsId.AddRange(data.posts.data.ToList());
            }

            return lsId;
        }

        private static async Task<List<string>> FindIdFromHtml(string content)
        {
            var lsId = new List<string>();

            var regex = Regex.Matches(content, @"<div><h3><a class=""\w+ \w+"" href=""(.*?)refid=[0-9]+&amp;__tn__=R""");
            if (regex.Any())
            {
                var listThread = new List<Task<string>>();
                foreach (Match o in regex)
                {
                    listThread.Add(GetIdProfileFacebook(o?.Groups[1]?.Value));
                }

                var res = await Task.WhenAll(listThread);
                if (res.Any())
                {
                    lsId = res.ToList();
                }
            }
            return lsId;
        }
        private static async Task<List<string>> GetFriendComment(string cookie, string id)
        {
            var lsHtml = new List<string>();
            var uri = "https://m.facebook.com";
            var client = new RestClient(uri + "/" + id);
            client.Timeout = -1;
            var request = new RestRequest(Method.GET);
            request.AddHeader("cookie", cookie);
            var response = await client.ExecuteAsync(request);
            var content = response.Content;
            var lsId1 = await FindIdFromHtml(content);
            if (lsId1.Any())
            {
                lsHtml.AddRange(lsId1);
            }
            if (content.Contains(@"id=""see_prev_"))
            {
                var urlSeenComment = Regex.Match(content, @"><a href=""(/story.php\?story_fbid.*?)""").Groups[1].Value;
                client.BaseUrl = new Uri(uri + HttpUtility.HtmlDecode(urlSeenComment));
                var respone2 = await client.ExecuteAsync(request);
                var content2 = respone2.Content;
                var lsId2 = await FindIdFromHtml(content2);
                if (lsId2.Any())
                {
                    lsHtml.AddRange(lsId2);
                }
                if (content.Contains(@"id=""see_prev_"))
                    while (true)
                    {
                        if (!content2.Contains(@"id=""see_prev_"))
                        {
                            break;
                        }

                        var urlSeenComment3 = Regex.Matches(content2, @"><a href=""(/story.php\?story_fbid.*?)""")[0]?.Groups[1]?.Value;
                        client.BaseUrl = new Uri(uri + HttpUtility.HtmlDecode(urlSeenComment3));
                        var respone3 = await client.ExecuteAsync(request);
                        content2 = respone3.Content;
                        var lsId3 = await FindIdFromHtml(content2);
                        if (lsId3.Any())
                        {
                            lsHtml.AddRange(lsId3);
                        }
                    }
            }
            return lsHtml;
        }

        private static async Task<string> GetIdProfileFacebook(string urlProfile)
        {
            var id = "";
            var urlFacebook = "https://m.facebook.com";
            var client = new RestClient("https://id.atpsoftware.vn/");
            var request = new RestRequest(Method.POST);
            request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            request.AddParameter("linkCheckUid", urlFacebook + urlProfile);
            var response = await client.ExecuteAsync(request);
            Regex regex = new Regex(@"<textarea.*?"">(.*?)</textarea>", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Multiline | RegexOptions.Singleline);
            var strId = regex.Match(response.Content)?.Groups[1]?.Value;
            if (!string.IsNullOrEmpty(strId))
            {
                id = Regex.Match(strId, @"\d+")?.Value;
            }
            return id;
        }
    }
}
