using AUTO.HLT.MOBILE.VIP.Models.LicenseKey;
using System.Threading.Tasks;
using AUTO.HLT.MOBILE.VIP.Models.RequestProviderModel;

namespace AUTO.HLT.MOBILE.VIP.Services.LicenseKey
{
    public interface ILicenseKeyService
    {
        /// <summary>
        /// cap nhat lich su dung dich vu
        /// </summary>
        /// <param name="key"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        Task<ResponseModel<string>> UpdateHistory(string key, string content);
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