using AUTO.TOOL.CLIENT.Models.RequestProviderModel;
using AUTO.TOOL.CLIENT.Models.User;
using AUTO.TOOL.CLIENT.Services.RequestProvider;
using AUTO.TOOL.CLIENT.Services.RestSharp;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace AUTO.TOOL.CLIENT.Services.User
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

        public async Task<int> SetPriceUser(string userName, string price)
        {
            var data = 0;
            try
            {
                var parameters = new List<RequestParameter>()
                {
                    new RequestParameter("UserName",userName),
                    new RequestParameter("Price",price),
                };
                var res = await _restSharpService.PutAsync("https://api.autohlt.vn/api/v1/user/SetMoney", parameters);
                var obj = JsonConvert.DeserializeObject<ResponseModel<int>>(res);
                if (obj != null)
                {
                    data = obj.Data;
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("Error: " + e);
            }

            return data;
        }

        public async Task<int> GetPriceUser(string userName)
        {
            var data = 0;
            try
            {
                var parameters = new List<RequestParameter>()
                {
                    new RequestParameter("UserName",userName),
                };
                var res = await _restSharpService.GetAsync("http://api.autohlt.vn/api/v1/user/getpriceuser", parameters);
                var obj = JsonConvert.DeserializeObject<ResponseModel<int>>(res);
                if (obj != null)
                    data = obj.Data;
            }
            catch (Exception e)
            {
                Debug.WriteLine("Error: " + e);
            }

            return data;
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
                Debug.WriteLine("Error: " + e);
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
                var json = await _restSharpService.PutAsync("https://api.autohlt.vn/api/v1/user/updateuser", para);
                if (json != null)
                {
                    var data = JsonConvert.DeserializeObject<ResponseModel<string>>(json);
                    return data;
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("Error: " + e);
            }
            return null;
        }
    }
}