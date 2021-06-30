using System.Collections.Generic;
using System.Threading.Tasks;
using AUTO.DLL.MOBILE.Models.RequestProviderModel;
using AUTO.DLL.MOBILE.Models.User;

namespace AUTO.DLL.MOBILE.Services.User
{
    public interface IUserService
    {
        /// <summary>
        /// Cộng xu cho người dùng
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="price"></param>
        /// <returns></returns>
        Task<int> SetPriceUser(string userName, string price);
        /// <summary>
        /// lấy số tiền hiên tại của user
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        Task<int> GetPriceUser(string userName);
        /// <summary>
        /// Lấy danh sách tất cả user
        /// </summary>
        /// <returns></returns>
        Task<ResponseModel<List<UserModel>>> GetAllUser();
        /// <summary>
        /// Cập nhật lại user
        /// </summary>
        /// <param name="user"></param>
        /// <param name="passwd"></param>
        /// <returns></returns>
        Task<ResponseModel<string>> UpdateUser(UserModel user);
    }
}