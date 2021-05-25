using System.Threading.Tasks;
using AUTOHLT.MOBILE.Models.RequestProviderModel;
using AUTOHLT.MOBILE.Models.VersionApp;

namespace AUTOHLT.MOBILE.Services.VersionAppService
{
    public interface IVersionAppService
    {
        /// <summary>
        /// lấy thông tin appversion
        /// </summary>
        /// <returns></returns>
        Task<ResponseModel<VersionModel>> CheckVersionApp();
    }
}