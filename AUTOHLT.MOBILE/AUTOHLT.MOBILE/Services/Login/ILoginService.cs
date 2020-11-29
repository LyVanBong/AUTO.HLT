using System.Threading.Tasks;
using AUTOHLT.MOBILE.Models.RequestProviderModel;
using AUTOHLT.MOBILE.Models.User;

namespace AUTOHLT.MOBILE.Services.Login
{
    public interface ILoginService
    {
       /// <summary>
       /// Service dang ky tai khoan
       /// </summary>
       /// <param name="userName"></param>
       /// <param name="name"></param>
       /// <param name="pass"></param>
       /// <param name="numberPhone"></param>
       /// <param name="email"></param>
       /// <param name="age"></param>
       /// <param name="isMale"></param>
       /// <returns></returns>
        Task<ResponseModel<int>> SignUp(string userName, string name, string pass,string numberPhone, string email, string age, bool isMale);
        /// <summary>
        /// Service đăng nhập vào ứng dụng
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="pass"></param>
        /// <returns></returns>
        Task<ResponseModel<UserModel>> Login(string userName, string pass);
    }
}