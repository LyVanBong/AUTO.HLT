using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using AUTO.DLL.Models;
using Newtonsoft.Json;
using RestSharp;

namespace AUTO.DLL.Services
{
    public static class TelegramService
    {
        private static string _tokenToAccessTelegram = "1413742738:AAGV-0OMyWCO4h_PRzq3JQfWkKj-PxYur_M";
        private static string _uriApiTelegram = "https://api.telegram.org/bot";

        public static async Task<TelegramModel> SendMessageToTelegram(string idChat, string message, string token = null)
        {
            try
            {
                token = string.IsNullOrWhiteSpace(token) ? _tokenToAccessTelegram : token;
                var uri = _uriApiTelegram + token + "/sendMessage";

                var client = new RestClient(uri);

                client.Timeout = -1;

                var request = new RestRequest(Method.POST);
                request.AddParameter("chat_id", idChat);
                request.AddParameter("text", message);
                var response = await client.ExecuteAsync(request);
                var data = response.StatusCode == HttpStatusCode.OK
                    ? JsonSerializer.Deserialize<TelegramModel>(response.Content)
                    : null;
                return data;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}