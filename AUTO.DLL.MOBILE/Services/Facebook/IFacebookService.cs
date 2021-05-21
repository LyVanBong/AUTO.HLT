using System.Collections.Generic;
using System.Threading.Tasks;
using AUTO.DLL.MOBILE.Models.Facebook;

namespace AUTO.DLL.MOBILE.Services.Facebook
{
    public interface IFacebookService
    {
        /// <summary>
        /// Get thong tin fake tu api face
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="token"></param>
        /// <param name="fields"></param>
        /// <returns></returns>
        Task<T> GetInfoUser<T>(string token, string fields);
        /// <summary>
        /// kiểm tra cookie và token có còn live hay không
        /// </summary>
        /// <param name="cookie"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        Task<bool> CheckCookieAndToken(string cookie, string token);
        /// <summary>
        /// xóa bạn bè
        /// </summary>
        /// <param name="cookie"></param>
        /// <param name="uid"></param>
        /// <returns></returns>
        Task<bool> UnFriend(string cookie, string uid);
        /// <summary>
        /// lấy dánh sách bạn bè
        /// </summary>
        /// <param name="cookie"></param>
        /// <returns></returns>
        Task<List<MyFriendModel>> GetMyFriend(string cookie);
        /// <summary>
        /// Tìm id từ url profile
        /// </summary>
        /// <param name="urlProfile"></param>
        /// <returns></returns>
        Task<string> GetIdFromProfileFacebook(string urlProfile);
        /// <summary>
        /// lấy danh sách id bài viết của mình
        /// </summary>
        /// <param name="cookie"></param>
        /// <param name="token"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        Task<List<Datum>> GetIdMyPost(string cookie, string token, string limit);
        /// <summary>
        /// Tim danh sách id của bạn bè từ html của page comment
        /// </summary>
        /// <param name="comment"></param>
        /// <returns></returns>
        Task<List<string>> FindIdFromHtmlComment(string comment);
        /// <summary>
        /// Lấy danh sách id bạn bè đã tương tác bài viết
        /// </summary>
        /// <param name="cookie"></param>
        /// <param name="limit"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        Task<List<string>> GetUIdFromPost(string cookie, string token, string limit);
        /// <summary>
        /// Lấy id bạn bè đã comment bài viết
        /// </summary>
        /// <param name="cookie">Cookie facebook</param>
        /// <param name="id">Id bài viết</param>
        /// <returns>Danh sách id bạn bè comment</returns>
        Task<List<string>> GetUIdCommentFromAPost(string cookie, string id);
        /// <summary>
        /// Lấy id bạn bè đã like bài viết
        /// </summary>
        /// <param name="cookie">Cookie facebook</param>
        /// <param name="id">Id bài viết</param>
        /// <returns>Danh sách id bạn bè like</returns>
        Task<List<string>> GetUIdLikeFromAPost(string cookie, string id);
    }
}