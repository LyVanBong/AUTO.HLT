using System.Threading.Tasks;
using AUTOHLT.MOBILE.Models.RequestProviderModel;
using AUTOHLT.MOBILE.Models.User;

namespace AUTOHLT.MOBILE.Services.Login
{
    public interface ILoginService
    {
        Task<ResponseModel<UserModel>> Login(string userName, string pass);
    }
}