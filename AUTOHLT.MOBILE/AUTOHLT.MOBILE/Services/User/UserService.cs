using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AUTOHLT.MOBILE.Models.RequestProviderModel;
using AUTOHLT.MOBILE.Services.RequestProvider;

namespace AUTOHLT.MOBILE.Services.User
{
    public class UserService : IUserService
    {
        private IRequestProvider _requestProvider;
        public UserService(IRequestProvider requestProvider)
        {
            _requestProvider = requestProvider;
        }
        public async Task<ResponseModel<string>> CheckExistAccount(string userName)
        {
            try
            {
                var para = new List<RequestParameter>
                {
                    new RequestParameter("username",userName),
                };
                var data = await _requestProvider.GetAsync<string>("user/checkusername", para);
                return data;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}