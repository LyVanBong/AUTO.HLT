using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AUTO.HLT.MOBILE.VIP.Models.Login;
using AUTO.HLT.MOBILE.VIP.Models.RequestProviderModel;
using AUTO.HLT.MOBILE.VIP.Services.RequestProvider;
using Microsoft.AppCenter.Crashes;

namespace AUTO.HLT.MOBILE.VIP.Services.Login
{
    public class LoginService : ILoginService
    {
        private IRequestProvider _requestProvider;
        public LoginService(IRequestProvider requestProvider)
        {
            _requestProvider = requestProvider;
        }

        public async Task<ResponseModel<string>> CheckExistUser(string userName)
        {
            try
            {

            }
            catch (Exception e)
            {
                Crashes.TrackError(e);
                return null;
            }
        }

        public async Task<ResponseModel<LoginModel>> Login(string userName, string passwd)
        {
            try
            {
                var para = new List<RequestParameter>
                {
                    new RequestParameter("UserName",userName),
                    new RequestParameter("PassWord",passwd),
                };
                var data =await _requestProvider.PostAsync<LoginModel>("user/Login",para);
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