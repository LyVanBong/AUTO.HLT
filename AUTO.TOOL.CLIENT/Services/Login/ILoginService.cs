using System.Threading.Tasks;
using AUTO.TOOL.CLIENT.Models.Login;
using AUTO.TOOL.CLIENT.Models.RequestProviderModel;

namespace AUTO.TOOL.CLIENT.Services.Login
{
    public interface ILoginService
    {
        /// <summary>
        /// lay thong tin nguoi gioi thieu
        /// </summary>
        /// <returns></returns>
        Task<ResponseModel<InfoIntroducetorModel>> Introducetor();
        /// <summary>
        /// Thêm người giới thiệu
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<ResponseModel<string>> AddIntroducetor(NguoiGioiThieuModel input);
        /// <summary>
        /// đăng ký tài khoản
        /// </summary>
        /// <param name="sigup"></param>
        /// <returns></returns>
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