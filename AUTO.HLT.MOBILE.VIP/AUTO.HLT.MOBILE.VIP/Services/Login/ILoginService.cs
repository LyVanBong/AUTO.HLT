using System.Threading.Tasks;
using AUTO.HLT.MOBILE.VIP.Models.Login;
using AUTO.HLT.MOBILE.VIP.Models.RequestProviderModel;

namespace AUTO.HLT.MOBILE.VIP.Services.Login
{
    public interface ILoginService
    {
        Task<ResponseModel<string>> Sigup(SigupModel sigup);
        /// <summary>
        /// kiểm tra tồn tại của số điện thoại
        /// </summary>
        /// <param name="phone"></param>
        /// <returns></returns>
        Task<ResponseModel<string>> CheckExistPhone(string phone);
        /// <summary>
        /// kiem tra ten dang nhap da ton tai chua
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        Task<ResponseModel<string>> CheckExistUser(string userName);
        /// <summary>
        /// api dang nhap v2
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="passwd"></param>
        /// <returns></returns>
        Task<ResponseModel<LoginModel>> Login(string userName, string passwd);
    }
}