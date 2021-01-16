using AUTOHLT.MOBILE.Models.RequestProviderModel;
using AUTOHLT.MOBILE.Models.User;
using AUTOHLT.MOBILE.Services.RequestProvider;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AUTOHLT.MOBILE.Services.Login
{
    public class LoginService : ILoginService
    {
        private IRequestProvider _requestProvider;

        public LoginService(IRequestProvider requestProvider)
        {
            _requestProvider = requestProvider;
        }

        public async Task<ResponseModel<int>> SignUp(string userName, string name, string pass, string numberPhone, string email, string age,
            bool isMale)
        {
            try
            {
                var para = new List<RequestParameter>
                {
                    new RequestParameter("UserName",userName),
                    new RequestParameter("Name",name),
                    new RequestParameter("Password",pass),
                    new RequestParameter("NumberPhone",numberPhone),
                    new RequestParameter("Email",email),
                    new RequestParameter("Age",age),
                    new RequestParameter("Sex",isMale+""),
                    new RequestParameter("DateCreate",DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss")),
                };
                var data = await _requestProvider.PostAsync<int>("user/registrationcccount", para);
                return data;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ResponseModel<UserModel>> Login(string userName, string pass)
        {
            try
            {
                var para = new List<RequestParameter>();
                para.Add(new RequestParameter("UserName", userName));
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