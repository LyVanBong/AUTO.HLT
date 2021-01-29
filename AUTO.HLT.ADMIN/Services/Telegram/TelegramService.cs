using System;
using System.Net;
using System.Threading.Tasks;
using AUTOHLT.MOBILE.Models.Telegram;
using Newtonsoft.Json;
using RestSharp;

namespace AUTO.HLT.ADMIN.Services.Telegram
{
    public class TelegramService : ITelegramService
    {
        private string _tokenToAccessTelegram = "1413742738:AAGV-0OMyWCO4h_PRzq3JQfWkKj-PxYur_M";
        private string _uriApiTelegram = "https://api.telegram.org/bot";
        public async Task<TelegramModel> SendMessageToTelegram(string idChat, string message, string token = null)
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
                    ? JsonConvert.DeserializeObject<TelegramModel>(response.Content)
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