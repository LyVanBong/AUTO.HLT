using System.Threading.Tasks;
using AUTOHLT.MOBILE.Models.RequestProviderModel;

namespace AUTOHLT.MOBILE.Services.User
{
    public interface IUserService
    {
        /// <summary>
        /// Kiểm tra sự tồn tại của username
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        Task<ResponseModel<string>> CheckExistAccount(string userName);
    }
}