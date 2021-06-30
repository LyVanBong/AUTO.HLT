using System.Threading.Tasks;
using AUTO.TOOL.CLIENT.Models.Telegram;

namespace AUTO.TOOL.CLIENT.Services.Telegram
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