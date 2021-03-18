using System.Collections.Generic;
using System.Threading.Tasks;
using AUTO.HLT.ADMIN.Models.Facebook;
using AUTO.HLT.ADMIN.Models.RequestProviderModel;

namespace AUTO.HLT.ADMIN.Services.Facebook
{
    public interface IFacebookService
    {
        /// <summary>
        /// giả lập request facebook bằng phương thức post
        /// </summary>
        /// <param name="url"></param>
        /// <param name="cookie"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        Task<string> PostHtmlFacebook(string url, string cookie, List<RequestParameter> parameters = null);
        /// <summary>
        /// lay id bai viet cua ban be
        /// </summary>
        /// <param name="limit"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        Task<PostIdMyFriendModel> GetIdPostFriends(string limit, string token,string id);
        /// <summary>
        /// Lấy thông tin user facebook
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="fields"></param>
        /// <returns></returns>
        Task<NamePictureUserModel> GetInfoUser(string accessToken, string fields = "name,picture");
        /// <summary>
        /// lấy html các trang web
        /// </summary>
        /// <param name="url"></param>
        /// <param name="cookie"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        Task<string> GetHtmlFacebook(string url, string cookie, List<RequestParameter> parameters = null);
        /// <summary>
        /// lấy jazoest và fbdtsg
        /// </summary>
        /// <param name="cookie"></param>
        /// <returns></returns>
        Task<(string Jazoest, string Fbdtsg)> GeJazoestAndFbdtsg(string cookie);
        /// <summary>
        /// lây danh sách bạn bè
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        Task<FriendsModel> GetIdFriends(string token, string fields = "id,name");
        /// <summary>
        /// kiểm tra cookie token
        /// </summary>
        /// <param name="token"></param>
        /// <param name="cookie"></param>
        /// <returns></returns>
        Task<bool> CheckTokenCookie(string token, string cookie);
    }
}