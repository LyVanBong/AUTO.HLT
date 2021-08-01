using RestSharp;
using System;
using System.Threading.Tasks;

namespace AUTO.CONSOLE
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var client = new RestClient("https://api.autohlt.vn/api/v1/user/getalluser");
            client.Timeout = -1;
            var request = new RestRequest(Method.GET);
            IRestResponse response = client.Execute(request);
            Console.WriteLine(response.Content);
        }
    }
}
