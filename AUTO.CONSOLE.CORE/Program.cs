using RestSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json;

namespace AUTO.CONSOLE.CORE
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var cookie = "sb=poRNYNLLZXJ3fIQPMU0WBMzN;datr=poRNYPBAtiHTxtPfI_QNCIlr;locale=vi_VN;m_pixel_ratio=1;x-referer=eyJyIjoiL2h0Y3VhZ2lvIiwiaCI6Ii9odGN1YWdpbyIsInMiOiJtIn0%3D;wd=1366x657;c_user=100027295904383;spin=r.1003759102_b.trunk_t.1620488684_s.1_v.2_;xs=46%3ANBROqwoWMVxENw%3A2%3A1620488682%3A-1%3A2769%3A%3AAcVR4-dyvlv7tZhgsasy-zDOEYZEsW42CG2ftaBfZg;fr=1RgFyqZl5tvLpUvQ9.AWV5KlHbpniVPbY1GOLxos9Hy1o.BglrNc.L2.AAA.0.0.BglrNc.AWVgDIGWV0A";
            var token = "EAAAAZAw4FxQIBAM9lelgX0RzzdrAFW7zuaMm0CgNIPf0GZB3O1UAYSDhDy3Og0lfzxA5gqL5KfjTRSJyFHzQiHIwhOrwx0RLcfox0sWF894ePcGbdv0McYby5qC5P0zajJ9aYjKq5sVvfjKZBzZAksMraFocytO2h3dpNBHrCC21MQyIkFUk";
            var time = new Stopwatch();
            time.Start();
            var idpost = GetIdPost(token, cookie);
            var lsId = new List<string>();
            if (idpost.Any())
            {
                foreach (var item in idpost)
                {
                    var data =await GetFriendComment(cookie, item.id);
                    if (data.Any())
                    {
                        lsId.AddRange(data);
                    }
                }
            }
            time.Stop();
            Console.WriteLine("Time Run : " + time.ElapsedMilliseconds);
            Console.WriteLine("Số lượng : " + lsId.Count);
            Console.ReadKey();
        }

        private static List<Datum> GetIdPost(string token, string cookie)
        {
            var lsId = new List<Datum>();
            var client = new RestClient(
                "https://graph.facebook.com/v10.0/me?fields=posts.limit(50){id}&access_token=" + token);
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

        private static List<string> FindIdFromHtml(string content)
        {
            var lsId = new List<string>();

            var regex = Regex.Matches(content, @"""><div><h3><a class=""(.*?)"" href=""(.*?)rc=p&amp;refid=[0-9]+&amp;__tn__=R"">");
            if (regex.Any())
            {
                foreach (Match o in regex)
                {
                    var id = GetIdProfileFacebook(o?.Groups[2]?.Value);
                    if (!string.IsNullOrEmpty(id))
                    {
                        lsId.Add(id);
                    }
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
            var lsId1 = FindIdFromHtml(content);
            if (lsId1.Any())
            {
                lsHtml.AddRange(lsId1);
            }
            if (content.Contains("see_next_"))
            {
                var urlSeenComment = Regex.Match(content, @"><a href=""(/story.php\?story_fbid.*?)""").Groups[1].Value;
                client.BaseUrl = new Uri(uri + HttpUtility.HtmlDecode(urlSeenComment));
                var respone2 = await client.ExecuteAsync(request);
                var content2 = respone2.Content;
                var lsId2 = FindIdFromHtml(content2);
                if (lsId2.Any())
                {
                    lsHtml.AddRange(lsId2);
                }
                if (content.Contains("see_next_" + id))
                    while (true)
                    {
                        if (!content2.Contains("see_next_" + id))
                        {
                            break;
                        }

                        var urlSeenComment3 = Regex.Matches(content2, @"><a href=""(/story.php\?story_fbid.*?)""")[1]?.Groups[1]?.Value;
                        client.BaseUrl = new Uri(uri + HttpUtility.HtmlDecode(urlSeenComment3));
                        var respone3 = await client.ExecuteAsync(request);
                        content2 = respone3.Content;
                        var lsId3 = FindIdFromHtml(content2);
                        if (lsId3.Any())
                        {
                            lsHtml.AddRange(lsId3);
                        }
                    }
            }
            return lsHtml;
        }

        private static string GetIdProfileFacebook(string urlProfile)
        {
            var urlFacebook = "https://m.facebook.com";
            var client = new RestClient("https://id.atpsoftware.vn/");
            var request = new RestRequest(Method.POST);
            request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            request.AddParameter("linkCheckUid", urlFacebook + urlProfile);
            var response = client.Execute(request);
            Regex regex = new Regex(@"<textarea.*?"">(.*?)</textarea>", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Multiline | RegexOptions.Singleline);
            return regex.Match(response.Content)?.Groups[1]?.Value;
        }
    }
}
