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
            var cookie = "sb=poRNYNLLZXJ3fIQPMU0WBMzN;datr=poRNYPBAtiHTxtPfI_QNCIlr;locale=vi_VN;c_user=100004359125508;wd=1366x657;spin=r.1003840495_b.trunk_t.1621673379_s.1_v.2_;xs=32%3AkZHY1dieXJetCw%3A2%3A1621163542%3A-1%3A6383%3A%3AAcVsI920Fykb8T6snmVrL0DfL3YSBG_Ghk23oE5oz14;fr=1qOiu7qeh8RK4VPAU.AWXKfr2216eerI1KFpKSovq1ojM.BgqMW1.L2.AAA.0.0.BgqMW1.AWWs8rDd-0o";
            var token = "EAAAAZAw4FxQIBACSQgcyZCXNGNaU5zyiPoNHMkPI0aBahFx6BpSZCJ4giWKewwyYr1ZBtDzqLcH8CyRrr9dJ0wZBtrD3BI3vZC5sl3pYwzZCPsZBh8kboXnQIlmWBmEuh6Ogx2Y7JSpKPZAC8RIZC3vEEqgcDaBULZA3ysVbJ5pnkbrNcr1Fl5ZB2dZB5Odtk0S4njA8ZD";
            var facebooke = new FacebookeService();
            var time = new Stopwatch();
            time.Start();
            var check = await facebooke.GetFriendPoke(cookie);
            var poke = await facebooke.PokeFriend(cookie, check[0]);
            time.Stop();
            Console.WriteLine("Thoi gian chay : " + time.ElapsedMilliseconds / 1000 + " s");
        }
    }
}
