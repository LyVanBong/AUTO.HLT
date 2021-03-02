using System.Threading.Tasks;
using AUTO.HLT.MOBILE.VIP.Models.Login;
using AUTO.HLT.MOBILE.VIP.Models.RequestProviderModel;

namespace AUTO.HLT.MOBILE.VIP.Services.Login
{
    public interface ILoginService
    {

        /// <summary>
        /// api dang nhap v2
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="passwd"></param>
        /// <returns></returns>
        Task<ResponseModel<LoginModel>> Login(string userName, string passwd);
    }
}