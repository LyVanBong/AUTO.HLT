using RestSharp;
using System;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;

namespace AUTOHLT.WEB.API.Controllers.Telegram
{
    [RoutePrefix("api/version3/Telegram")]
    public class TelegramController : ApiController
    {
        private string _idChatWork2 = "-588406460";
        /// <summary>
        /// Gửi tin nhắn telegram
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        [Route("Message")]
        [HttpGet]
        public async Task<IHttpActionResult> Message([FromUri] string message)
        {
            if (string.IsNullOrEmpty(message) || string.IsNullOrWhiteSpace(message))
            {
                return Ok(new
                {
                    Code = -11111,
                    Message = "Nội dung trống",
                    Data = DateTime.Now
                });
            }
            else
            {
                var client = new RestClient("https://api.telegram.org/bot1413742738:AAGV-0OMyWCO4h_PRzq3JQfWkKj-PxYur_M/sendMessage?chat_id=" + _idChatWork2 + "&text=" + message);
                var response = await client.ExecuteAsync(new RestRequest(Method.POST));
                if (response.IsSuccessful && response.StatusCode == HttpStatusCode.OK)
                    return Ok(new
                    {
                        Code = -11111,
                        Message = "Gửi tin nhắn thành công",
                        Data = message
                    });
                else
                    return Ok(new
                    {
                        Code = -11111,
                        Message = "Lỗi khi gửi tin nhắn",
                        Data = DateTime.Now
                    });
            }
        }
    }
}
