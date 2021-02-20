using AUTO.HLT.ADMIN.Models.AutoLikeCommentAvatar;
using AUTO.HLT.ADMIN.Models.RequestProviderModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AUTO.HLT.ADMIN.Services.AutoLikeCommentAvatar
{
    public interface IAutoAvatarService
    {
        /// <summary>
        /// lay danh sach auto
        /// </summary>
        /// <returns></returns>
        Task<ResponseModel<List<HistoryAutoModel>>> GetAllHistoryAuto();
        /// <summary>
        /// Theem user auto
        /// </summary>
        /// <param name="id"></param>
        /// <param name="regisDate"></param>
        /// <param name="expireTime"></param>
        /// <param name="cookie"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        Task<ResponseModel<string>> AddUserAuto(string id,string regisDate,string expireTime,string cookie,string token);
        /// <summary>
        /// them uid cua user vao db
        /// </summary>
        /// <param name="id"></param>
        /// <param name="uid"></param>
        /// <returns></returns>
        Task<ResponseModel<string>> AddUidFacebook(string id, string uid);
        /// <summary>
        /// xoa user auto
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ResponseModel<string>> DeleteUserAuto(string id);
        /// <summary>
        /// them lich su auto
        /// </summary>
        /// <param name="id"></param>
        /// <param name="uid"></param>
        /// <param name="name"></param>
        /// <param name="url"></param>
        /// <param name="noteAuto"></param>
        /// <param name="uidFriend"></param>
        /// <param name="nameFriend"></param>
        /// <param name="urlFriend"></param>
        /// <returns></returns>
        Task<ResponseModel<string>> AddHistoryAuto(string id, string uid, string name,
            string url, string noteAuto, string uidFriend, string nameFriend, string urlFriend);
        /// <summary>
        /// lay danh sach uid da like avatar
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ResponseModel<List<UIdFacebookModel>>> GetUIdFacebook(string id);
        /// <summary>
        /// Cập thêm thông tin vào user auto
        /// </summary>
        /// <param name="id"></param>
        /// <param name="uid"></param>
        /// <param name="urlAvatar"></param>
        /// <param name="isRunWork"></param>
        /// <returns></returns>
        Task<ResponseModel<string>> UpdateUserFaceInfo(string id, string uid, string name, string urlAvatar, bool isRunWork);
        /// <summary>
        /// lấy danh sách các user cần auto like avatar + cmt
        /// </summary>
        /// <returns></returns>
        Task<ResponseModel<List<UserAutoModel>>> GetAllUserAuto();
    }
}