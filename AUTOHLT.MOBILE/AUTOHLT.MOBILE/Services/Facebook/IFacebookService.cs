using System.Threading.Tasks;
using AUTOHLT.MOBILE.Models.Facebook;

namespace AUTOHLT.MOBILE.Services.Facebook
{
    public interface IFacebookService
    {
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