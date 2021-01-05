using System.Threading.Tasks;
using AUTOHLT.MOBILE.Models.Facebook;

namespace AUTOHLT.MOBILE.Services.Facebook
{
    public interface IFacebookService
    {
        /// <summary>
        /// Choc ban be
        /// </summary>
        /// <param name="cookie"></param>
        /// <returns></returns>
        Task<string> PokesFriends(string cookie,string uri);
        /// <summary>
        /// Lấy dữ liệu danh sách bạn bè chọc
        /// </summary>
        /// <param name="cookie"></param>
        /// <param name="showOutgoing">0 la choc , 1 la da choc</param>
        /// <returns></returns>
        Task<string> GetPokesFriends(string cookie, string showOutgoing);
        /// <summary>
        /// Kiểm tra cookie còn sông hay đã chết
        /// </summary>
        /// <param name="cookie">cookie</param>
        /// <returns>
        /// true còn sôngs
        /// false đã tèo
        /// </returns>
        Task<bool> CheckCookie(string cookie);
        /// <summary>
        /// Huy ban be khong tuong tac
        /// </summary>
        /// <param name="fb_dtsg"></param>
        /// <param name="jazoest"></param>
        /// <param name="friend_id"></param>
        /// <param name="cookie"></param>
        /// <returns></returns>
        Task<string> UnFriend(string fb_dtsg, string jazoest, string friend_id, string cookie);
        /// <summary>
        /// Lấy 5000 bạn bè trên facebook
        /// </summary>
        /// <param name="fields"></param>
        /// <param name="accessToken"></param>
        /// <returns></returns>
        Task<FriendsModel> GetAllFriend(string fields, string accessToken);
        /// <summary>
        /// Lọc bạn bè không tương tác
        /// </summary>
        /// <param name="fbDtsg"></param>
        /// <param name="q"></param>
        /// <returns></returns>
        Task<string> GetFriendsDoNotInteract(string fbDtsg, string q);
        /// <summary>
        /// Lấy thông tin user facebook
        /// </summary>
        /// <param name="fields"></param>
        /// <param name="accessToken"></param>
        /// <returns></returns>
        Task<NamePictureUserModel> GetInfoUser(string fields, string accessToken);
    }
}