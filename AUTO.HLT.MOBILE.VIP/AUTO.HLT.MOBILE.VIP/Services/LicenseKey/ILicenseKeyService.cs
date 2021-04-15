using AUTO.HLT.MOBILE.VIP.Models.LicenseKey;
using AUTO.HLT.MOBILE.VIP.Models.RequestProviderModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AUTO.HLT.MOBILE.VIP.Services.LicenseKey
{
    public interface ILicenseKeyService
    {
        /// <summary>
        /// Tạo khóa bản quyền
        /// </summary>
        /// <param name="idUser"></param>
        /// <param name="amountKey"></param>
        /// <returns></returns>
        Task<ResponseModel<string>> CreateLicense(string idUser,string amountKey);
        /// <summary>
        /// lấy dánh sách key bản quyền do đại lý đã mua
        /// </summary>
        /// <returns></returns>
        Task<ResponseModel<List<AgecyLicenseModel>>> GetLicenseForAgecy();
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