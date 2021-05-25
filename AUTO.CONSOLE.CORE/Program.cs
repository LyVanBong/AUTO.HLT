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
using AUTO.DLL.MOBILE.Services.RestSharp;

namespace AUTO.CONSOLE.CORE
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var restSharp = new RestSharpService();
            var html =await restSharp.GetAsync("https://www.luxstay.com/vi/homestay/explore/vung-tau-biet-thu-ho-boi-618");
        }
    }
}
