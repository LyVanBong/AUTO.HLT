using AUTO.HLT.MOBILE.VIP.Models.Telegram;
using System.Threading.Tasks;

namespace AUTO.HLT.MOBILE.VIP.Services.Telegram
{
    public interface ITelegramService
    {
        /// <summary>
        /// service gui tin nhan den group chat telegram
        /// </summary>
        /// <param name="idChat"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        Task<TelegramModel> SendMessageToTelegram(string idChat, string message, string token = null);
    }
}