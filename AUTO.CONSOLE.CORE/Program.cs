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
            var cookie = "sb=poRNYNLLZXJ3fIQPMU0WBMzN;datr=poRNYPBAtiHTxtPfI_QNCIlr;locale=vi_VN;c_user=100027295904383;spin=r.1003786840_b.trunk_t.1620954360_s.1_v.2_;xs=1%3APVRPDr62Ac8jzA%3A2%3A1620954358%3A-1%3A2769%3A%3AAcVMrBuIM4KKZp93hHTeY7B4hyAAkXVvOGGv1Z7h5A;fr=1uY95uMUIHHc2vXHW.AWU3GONIu3zCG1rhCMLFCSXKwsI.BgnoWu.L2.AAA.0.0.BgnoWu.AWXYK8BjQrg;m_pixel_ratio=1;x-referer=eyJyIjoiL25ndXllbi52LnRyb25nLjU4NSIsImgiOiIvbmd1eWVuLnYudHJvbmcuNTg1IiwicyI6Im0ifQ%3D%3D;wd=1349x657";
            var token = "EAAAAZAw4FxQIBAJyXkmDucoPH4SFych788IdkbyZBeAGqsG0DDBbF1LvByj13Sr0aEWnYb9VVzBIHKaGSzZACYnxu5wQlkvOzjMJY1CqMDqucG7hqjgqkSpfZCV0RqiKlFwhMVJuZAa80YROTYMFsfW0qgkFnGybpiXFlC387gKSkUUoLCby7";
            var facebooke = new FacebookeService();
            var time = new Stopwatch();
            time.Start();
            var ls = await facebooke.GetUIdFromPost(cookie, token, "5");
            var myFriend = await facebooke.GetMyFriend(cookie);
            time.Stop();
            Console.WriteLine("Thoi gian chay : " + time.ElapsedMilliseconds / 1000 + " s");
        }
    }
}
