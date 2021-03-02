using System.Threading.Tasks;
using AUTO.HLT.MOBILE.VIP.Models.Login;

namespace AUTO.HLT.MOBILE.VIP.Services.Database
{
    public interface IDatabaseService
    {
        /// <summary>
        /// Xoa toan bo db local
        /// </summary>
        /// <returns></returns>
        void RemoveDatabaseLocal();

        /// <summary>
        /// Lưu tài khoản người dùng vào database local
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        Task<bool> SetAccountUser(LoginModel user);

        /// <summary>
        /// lấy tài khoản người dùng từ database local
        /// </summary>
        /// <returns></returns>
        Task<LoginModel> GetAccountUser();

        /// <summary>
        /// xóa tài khoản người dùng trong database local
        /// </summary>
        /// <returns></returns>
        Task DeleteAccontUser();

        /// <summary>
        /// cập tài khoản người dùng trong database local
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        Task<bool> UpdateAccountUser(LoginModel user);
    }
}