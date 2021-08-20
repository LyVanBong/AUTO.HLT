using AUTOHLT.MOBILE.Models.RequestProviderModel;
using AUTOHLT.MOBILE.Models.VersionApp;
using System.Threading.Tasks;

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