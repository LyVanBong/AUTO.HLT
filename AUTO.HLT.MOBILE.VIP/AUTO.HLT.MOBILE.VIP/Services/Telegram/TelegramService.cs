using AUTO.HLT.MOBILE.VIP.Configurations;
using AUTO.HLT.MOBILE.VIP.Models.Telegram;
using Microsoft.AppCenter.Crashes;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Net;
using System.Threading.Tasks;

namespace AUTO.HLT.MOBILE.VIP.Services.Telegram
{
    public class TelegramService : ITelegramService
    {
        public async Task<TelegramModel> SendMessageToTelegram(string idChat, string message, string token = null)
        {
            try
            {
                token = string.IsNullOrWhiteSpace(token) ? AppConstants.TokenToAccessTelegram : token;
                var uri = AppConstants.UriApiTelegram + token + "/sendMessage";

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
            catch (Exception e)
            {
                Crashes.TrackError(e);
                return null;
            }
        }
    }
}