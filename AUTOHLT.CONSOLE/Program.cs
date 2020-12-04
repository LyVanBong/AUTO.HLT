using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp;

namespace AUTOHLT.CONSOLE
{
    class Program
    {
        static void Main(string[] args)
        {
            var client = new RestClient("http://api.autohlt.com/api/v1/user/transfermoney");
            client.Timeout = -1;
            var request = new RestRequest(Method.PUT);
            request.AddParameter("IdSend", "7969f6eb-1f6b-420b-8e08-1453c19e1b68");
            request.AddParameter("IdReceive", "1851cce2-25b8-4469-9743-60ebc764de64");
            request.AddParameter("Price", "10000");
            IRestResponse response = client.Execute(request);
            Console.WriteLine(response.Content);
            Console.ReadKey();
        }
    }
}
