using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AUTOHLT.MOBILE.Models.RequestProviderModel;
using AUTOHLT.MOBILE.Models.User;
using AUTOHLT.MOBILE.Services.RequestProvider;

namespace AUTOHLT.MOBILE.Services.Login
{
    public class LoginService : ILoginService
    {
        private IRequestProvider _requestProvider;
        public LoginService(IRequestProvider requestProvider)
        {
            _requestProvider = requestProvider;
        }
        public async Task<ResponseModel<UserModel>> Login(string userName, string pass)
        {
            try
            {
                var para = new List<RequestParameter>();
                para.Add(new RequestParameter("UserName",userName));
                para.Add(new RequestParameter("PassWord", pass));
                var data = await _requestProvider.PostAsync<UserModel>("user/login", para);
                return data;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}