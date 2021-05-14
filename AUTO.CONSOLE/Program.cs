using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using AUTO.DLL.Services;

namespace AUTO.CONSOLE
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var cookie = "sb=poRNYNLLZXJ3fIQPMU0WBMzN;datr=poRNYPBAtiHTxtPfI_QNCIlr;c_user=100027295904383;spin=r.1003745490_b.trunk_t.1620307551_s.1_v.2_;xs=40%3Ao3mINWne0t9iWQ%3A2%3A1619358736%3A-1%3A2769%3A%3AAcXs5v14yAMUq2CR7wYlt94BDQeAlmU4CsCP-d4PGQ;fr=1qk5YzuzzRktaPx9j.AWWDanIoh1XwMCdpnDQu_l-uq0s.Bgk-5h.L2.AAA.0.0.Bgk-5h.AWUFxjLlt4s;presence=C%7B%22t3%22%3A%5B%5D%2C%22utc3%22%3A1620308243782%2C%22v%22%3A1%7D;m_pixel_ratio=1;x-referer=eyJyIjoiL2ZyaWVuZHMvY2VudGVyL3N1Z2dlc3Rpb25zLz9tZmZfbmF2PTEiLCJoIjoiL2ZyaWVuZHMvY2VudGVyL3N1Z2dlc3Rpb25zLz9tZmZfbmF2PTEiLCJzIjoibSJ9;wd=1349x657";
            var res = await FacebookService.AddFriend(cookie);
        }
    }
}
