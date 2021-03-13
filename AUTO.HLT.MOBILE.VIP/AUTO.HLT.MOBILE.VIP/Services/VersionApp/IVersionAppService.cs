using System.Threading.Tasks;
using AUTO.HLT.MOBILE.VIP.Models.RequestProviderModel;
using AUTO.HLT.MOBILE.VIP.Models.VersionApp;

namespace AUTO.HLT.MOBILE.VIP.Services.VersionApp
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