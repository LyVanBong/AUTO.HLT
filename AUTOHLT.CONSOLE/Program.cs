﻿using RestSharp;
using System;

namespace AUTOHLT.CONSOLE
{
    class Program
    {
        static void Main(string[] args)
        {
            var client = new RestClient("http://api.autohlt.com/api/v1/user/updateuser");
            client.Timeout = -1;
            var request = new RestRequest(Method.PUT);
            request.AddParameter("UserName", "admin");
            request.AddParameter("Name", "Ly gia");
            request.AddParameter("Password", "D033E22AE348AEB5660FC2140AEC35850C4DA997");
            request.AddParameter("Email", "admin@hope.com");
            request.AddParameter("NumberPhone", "987654321");
            request.AddParameter("Sex", "1");
            request.AddParameter("Role", "1");
            request.AddParameter("IsActive", "true");
            request.AddParameter("Age", "1995");
            request.AddParameter("Price", "999999999");
            request.AddParameter("IdDevice", "null");
            IRestResponse response = client.Execute(request);
            Console.WriteLine(response.Content);
            Console.ReadKey();
        }
    }
}