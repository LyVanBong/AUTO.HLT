using System.Collections.Generic;
using System.Threading.Tasks;
using AUTO.HLT.MOBILE.VIP.Models.RequestProviderModel;
using AUTO.HLT.MOBILE.VIP.Models.User;

namespace AUTO.HLT.MOBILE.VIP.Services.User
{
    public interface IUserService
    {
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