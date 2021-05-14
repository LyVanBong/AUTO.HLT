using AUTO.DLL.MOBILE.Services.Facebook;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace AUTO.CONSOLE.CORE
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var cookie = "sb=Ez2zX3CsnRDDIuCCSAJ8cAvo;datr=FD2zXxwc0iA3H6gp2TElljSn;_fbp=fb.1.1606969798014.1362959027;c_user=100003841239701;wd=1920x912;spin=r.1003758647_b.trunk_t.1620453701_s.1_v.2_;xs=46%3AKbrCPAGwgTJKoA%3A2%3A1618657461%3A-1%3A6373%3A%3AAcUWuj5yb2PKsA2iB75aF6WAT8kLkCfixNNYG92v25IN;fr=1LlYD8oxnLiouamCy.AWWjnDDmuio7I-MBZqB8Ziw903M.BglzeK.Ya.AAA.0.0.BglzeK.AWXAoi2D9Vc;presence=C%7B%22t3%22%3A%5B%7B%22i%22%3A%22u.100005090865078%22%7D%2C%7B%22i%22%3A%22u.100018610031687%22%7D%2C%7B%22i%22%3A%22u.100067330628348%22%7D%2C%7B%22i%22%3A%22u.100005090865078%22%7D%2C%7B%22i%22%3A%22g.2451271764968022%22%7D%2C%7B%22i%22%3A%22u.100019113150057%22%7D%2C%7B%22i%22%3A%22u.100005601702441%22%7D%2C%7B%22i%22%3A%22u.100011570709490%22%7D%2C%7B%22i%22%3A%22u.100014391513978%22%7D%2C%7B%22i%22%3A%22u.100004094209207%22%7D%2C%7B%22i%22%3A%22u.100003813486017%22%7D%2C%7B%22i%22%3A%22u.100001842012404%22%7D%5D%2C%22utc3%22%3A1620524289314%2C%22lm3%22%3A%22u.100005090865078%22%2C%22v%22%3A1%7D";
            var token = "EAAAAZAw4FxQIBAMgNYLGRwjrHnXlWnZAdZC8GZBHsTyynGt01SocgB6FrnjmJgj210CZBjGB1E4k1QmKrZCQrSzUOPDI88MFCte2vAytZCmn6REQt01FcGMXiDZCSeAAFfl5wsyCNGBTWclzubRpY0ek7xMTOlQPaSLGLRnlgnavZAXOhUdaXJLyGaaZCnzRMrxvsZD";
            var facebooke = new FacebookeService();

            var time = new Stopwatch();
            time.Start();
            var ls = await facebooke.GetUIdFromPost(cookie, "50", token);
            var html = await facebooke.GetMyFriend(cookie);
            time.Stop();
            Console.WriteLine("Thoi gian chay : " + time.ElapsedMilliseconds / 1000 + " s");
        }
    }
}
