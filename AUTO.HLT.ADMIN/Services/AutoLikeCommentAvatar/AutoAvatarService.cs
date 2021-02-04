using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AUTO.HLT.ADMIN.Models.AutoLikeCommentAvatar;
using AUTO.HLT.ADMIN.Models.RequestProviderModel;
using AUTO.HLT.ADMIN.Services.RequestProvider;
using Microsoft.AppCenter.Crashes;

namespace AUTO.HLT.ADMIN.Services.AutoLikeCommentAvatar
{
    public class AutoAvatarService : IAutoAvatarService
    {
        private IRequestProvider _requestProvider;
        public AutoAvatarService(IRequestProvider requestProvider)
        {
            _requestProvider = requestProvider;
        }

        public async Task<ResponseModel<string>> AddUidFacebook(string id, string uid)
        {
            try
            {
                var para = new List<RequestParameter>
                {
                    new RequestParameter("Id", id),
                    new RequestParameter("UId", uid),
                };
                var data = await _requestProvider.PostAsync<string>("AutoLikeCommentAvatar/AddFUidAutoLikeComment", para);
                return data;
            }
            catch (Exception e)
            {
                Crashes.TrackError(e);
                return null;
            }
        }

        public async Task<ResponseModel<string>> DeleteUserAuto(string id)
        {
            try
            {
                var parameters = new List<RequestParameter>
                {
                    new RequestParameter("id",id),
                };
                var data = await _requestProvider.DeleteAsync<string>("AutoLikeCommentAvatar/DeleteUserAutoLikeCommetAvatar", parameters);
                return data;
            }
            catch (Exception e)
            {
                Crashes.TrackError(e);
                return null;
            }
        }

        public Task<ResponseModel<string>> AddHistoryAuto(string id, string uid, string name, string url, string noteAuto, string uidFriend,
            string nameFriend, string urlFriend)
        {
            try
            {
                var para = new List<RequestParameter>
                {
                    new RequestParameter("ID",""),
                    new RequestParameter("UId",""),
                    new RequestParameter("Name",""),
                    new RequestParameter("Avatar",""),
                    new RequestParameter("TypeAuto",""),
                    new RequestParameter("UId_Friend",""),
                    new RequestParameter("Name_Friend",""),
                    new RequestParameter("Avatar_Friend",""),
                    new RequestParameter("DateCreate",DateTime.Now.ToString()),
                };
                var data = _requestProvider.PostAsync<string>("AutoLikeCommentAvatar/AddHistory", para);
                return data;
            }
            catch (Exception e)
            {
                Crashes.TrackError(e);
                return null;
            }
        }

        public async Task<ResponseModel<List<UIdFacebookModel>>> GetUIdFacebook(string id)
        {
            try
            {
                var para = new List<RequestParameter>()
                {
                    new RequestParameter("Id",id),
                };
                var data = await _requestProvider.GetAsync<List<UIdFacebookModel>>("AutoLikeCommentAvatar/GetAllFUId", para);
                return data;
            }
            catch (Exception e)
            {
                Crashes.TrackError(e);
                return null;
            }
        }

        public async Task<ResponseModel<string>> UpdateUserFaceInfo(string id, string uid, string name, string urlAvatar, bool isRunWork)
        {
            try
            {
                var para = new List<RequestParameter>()
                {
                    new RequestParameter("Id",id),
                    new RequestParameter("UId",uid),
                    new RequestParameter("Name",name),
                    new RequestParameter("Picture",urlAvatar),
                    new RequestParameter("IsRunWork",isRunWork.ToString()),
                };
                var data = await _requestProvider.PutAsync<string>("AutoLikeCommentAvatar/UpdateUserAuto", para);
                return data;
            }
            catch (Exception e)
            {
                Crashes.TrackError(e);
                return null;
            }
        }

        public async Task<ResponseModel<List<UserAutoModel>>> GetAllUserAuto()
        {
            try
            {
                var data = await _requestProvider.GetAsync<List<UserAutoModel>>("AutoLikeCommentAvatar/GetAll");
                return data;
            }
            catch (Exception e)
            {
                Crashes.TrackError(e);
                return null;
            }
        }

        public async Task<ResponseModel<List<HistoryAutoModel>>> GetAllHistoryAuto()
        {
            try
            {
                var data = await _requestProvider.GetAsync<List<HistoryAutoModel>>("AutoLikeCommentAvatar/GetAllHistoryAutoLikeCommentAvatar");
                return data;
            }
            catch (Exception e)
            {
                Crashes.TrackError(e);
                return null;
            }
        }

        public async Task<ResponseModel<string>> AddUserAuto(string id, string regisDate, string expireTime, string cookie, string token)
        {
            try
            {
                var para = new List<RequestParameter>
                {
                    new RequestParameter("Id",id),
                    new RequestParameter("RegistrationDate",regisDate),
                    new RequestParameter("ExpiredTime",expireTime),
                    new RequestParameter("F_Cookie",cookie),
                    new RequestParameter("F_Token",token),
                };
                var data = await _requestProvider.PostAsync<string>("AutoLikeCommentAvatar/AddUserAuto", para);
                return data;
            }
            catch (Exception e)
            {
                Crashes.TrackError(e);
                return null;
            }
        }
    }
}