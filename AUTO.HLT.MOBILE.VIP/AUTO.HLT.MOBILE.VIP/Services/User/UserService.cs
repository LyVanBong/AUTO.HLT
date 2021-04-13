using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AUTO.HLT.MOBILE.VIP.Models.RequestProviderModel;
using AUTO.HLT.MOBILE.VIP.Models.User;
using AUTO.HLT.MOBILE.VIP.Services.RequestProvider;
using AUTO.HLT.MOBILE.VIP.Services.RestSharp;
using Microsoft.AppCenter.Crashes;
using Newtonsoft.Json;

namespace AUTO.HLT.MOBILE.VIP.Services.User
{
    public class UserService : IUserService
    {
        private IRequestProvider _requestProvider;
        private IRestSharpService _restSharpService;
        public UserService(IRequestProvider requestProvider, IRestSharpService restSharpService)
        {
            _restSharpService = restSharpService;
            _requestProvider = requestProvider;
        }
        public async Task<ResponseModel<List<UserModel>>> GetAllUser()
        {
            try
            {
                var json = await _restSharpService.GetAsync("https://api.autohlt.vn/api/v1/user/getalluser");
                if (json != null)
                {
                    var data = JsonConvert.DeserializeObject<ResponseModel<List<UserModel>>>(json);
                    return data;
                }
            }
            catch (Exception e)
            {
                Crashes.TrackError(e);
            }
            return null;
        }

        public async Task<ResponseModel<string>> UpdateUser(UserModel user)
        {
            try
            {
                var para = new List<RequestParameter>
                {
                    new RequestParameter(nameof(user.UserName),user.UserName),
                    new RequestParameter(nameof(user.Name),user.Name),
                    new RequestParameter(nameof(user.Password),user.Password),
                    new RequestParameter(nameof(user.Email),user.Email),
                    new RequestParameter(nameof(user.NumberPhone),user.NumberPhone),
                    new RequestParameter(nameof(user.Sex),user.Sex+""),
                    new RequestParameter(nameof(user.Role),user.Role+""),
                    new RequestParameter(nameof(user.IsActive),user.IsActive+""),
                    new RequestParameter(nameof(user.Age),user.Age),
                    new RequestParameter(nameof(user.Price),user.Price+""),
                    new RequestParameter(nameof(user.IdDevice),user.IdDevice),
                };
                var json = await _restSharpService.PutAsync("http://api.autohlt.com/api/v1/user/updateuser", para);
                if (json != null)
                {
                    var data = JsonConvert.DeserializeObject<ResponseModel<string>>(json);
                    return data;
                }
            }
            catch (Exception e)
            {
                Crashes.TrackError(e);
            }
            return null;
        }
    }
}