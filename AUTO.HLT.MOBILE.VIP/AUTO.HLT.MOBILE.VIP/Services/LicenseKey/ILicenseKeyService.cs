using System.Threading.Tasks;
using AUTO.HLT.MOBILE.VIP.Models.LicenseKey;
using AUTO.HLT.MOBILE.VIP.Models.RequestProviderModel;

namespace AUTO.HLT.MOBILE.VIP.Services.LicenseKey
{
    public interface ILicenseKeyService
    {
        /// <summary>
        /// nâng cấp tài khoản
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        Task<bool> ActiveLiceseKey(string key);
        /// <summary>
        /// kiem tra xem user da nang cap tai khoan chua
        /// </summary>
        /// <returns></returns>
        Task<LicenseKeyModel> CheckLicenseForUser();
    }
}