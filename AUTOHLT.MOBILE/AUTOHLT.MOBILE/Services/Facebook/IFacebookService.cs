﻿using AUTOHLT.MOBILE.Models.Facebook;
using AUTOHLT.MOBILE.Models.RequestProviderModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AUTOHLT.MOBILE.Services.Facebook
{
    public interface IFacebookService
    {
        /// <summary>
        /// Kiểm tra cookie còn sông hay đã chết
        /// </summary>
        /// <returns>
        /// true còn sôngs
        /// false đã tèo
        /// </returns>
        Task<bool> CheckCookieAndToken();

        /// <summary>
        /// api gui tin nhan facebook
        /// </summary>
        /// <param name="body"></param>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<string> SendMessageFacebook(string body, string ids);

        /// <summary>
        /// api dang mot bai len tuong ca nhan ban be
        /// </summary>
        /// <param name="target"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        Task<string> PostNewsOnFacebookFriend(string target, string message);

        /// <summary>
        /// lấy jazoest và fbdtsg
        /// </summary>
        /// <param name="cookie"></param>
        /// <returns></returns>
        Task<(string Jazoest, string Fbdtsg)> GeJazoestAndFbdtsg(string cookie);

        /// <summary>
        /// service gửi lời mời like page
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="cookie"></param>
        /// <param name="av"></param>
        /// <param name="fb_dtsg"></param>
        /// <param name="jazoest"></param>
        /// <param name="server_timestamps"></param>
        /// <param name="doc_id"></param>
        /// <param name="variables"></param>
        /// <returns></returns>
        Task<string> InviteFriendLikePage(string uri, string cookie, string av, string fb_dtsg, string jazoest, bool server_timestamps, string doc_id, string variables);

        /// <summary>
        /// lấy toàn bộ uid đùng để buff sub, like page
        /// </summary>
        /// <returns></returns>
        Task<ResponseModel<List<UidTypeFacebookModel>>> GetAllUidTypeFacebook();

        /// <summary>
        /// lấy html các trang web
        /// </summary>
        /// <param name="url"></param>
        /// <param name="cookie"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        Task<string> GetHtmlFacebook(string url, string cookie, List<RequestParameter> parameters = null);

        /// <summary>
        /// Lưu user facbook
        /// </summary>
        /// <param name="idUser"></param>
        /// <param name="uid"></param>
        /// <param name="cookie"></param>
        /// <param name="token"></param>
        /// <param name="note"></param>
        /// <returns></returns>
        Task<ResponseModel<string>> UpdateUserFacebook(string idUser, string uid, string cookie, string token, string note);

        /// <summary>
        /// lây tham số của facebook
        /// </summary>
        /// <param name="cookie"></param>
        /// <returns></returns>
        Task<string> GetParamaterFacebook(string cookie);

        /// <summary>
        /// Choc ban be
        /// </summary>
        /// <param name="cookie"></param>
        /// <returns></returns>
        Task<string> PokesFriends(string cookie, string poke_target, string ext, string hash, string fb_dtsg, string jazoest, string dom_id_replace);

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
        Task<T> GetAllFriend<T>(string accessToken, string fields = "name,picture{url}", string limit = "5000");

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