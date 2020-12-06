using System.Threading.Tasks;
using AUTOHLT.MOBILE.Models.RequestProviderModel;
using AUTOHLT.MOBILE.Models.User;

namespace AUTOHLT.MOBILE.Services.User
{
    public interface IUserService
    {
        /// <summary>
        /// Cập lại user
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        Task<ResponseModel<string>> UpdateUser(string userName, string name, string pass, string email, string number, string sex, string role, string isActive, string age, string price, string idDevice);
        /// <summary>
        /// Chuyển tiền từ user sang user
        /// </summary>
        /// <param name="idSend"></param>
        /// <param name="idReceive"></param>
        /// <param name="price"></param>
        /// <returns></returns>
        Task<ResponseModel<string>> TransferMoney(string idSend, string idReceive, string price);
        /// <summary>
        /// Lấy số dư hiện tại của user
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        Task<ResponseModel<string>> GetMoneyUser(string userName);
        /// <summary>
        /// Kiểm tra sự tồn tại của username
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        Task<ResponseModel<string>> CheckExistAccount(string userName);
    }
}