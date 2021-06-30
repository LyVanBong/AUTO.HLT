using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AUTO.DLL.MOBILE.Models.Login;
using AUTO.DLL.MOBILE.Models.RequestProviderModel;
using AUTO.DLL.MOBILE.Services.RequestProvider;

namespace AUTO.DLL.MOBILE.Services.Login
{
    public class LoginService : ILoginService
    {
        private IRequestProvider _requestProvider;
        public LoginService(IRequestProvider requestProvider)
        {
            _requestProvider = requestProvider;
        }

        public async Task<ResponseModel<InfoIntroducetorModel>> Introducetor()
        {
            try
            {
                var data = await _requestProvider.GetAsync<InfoIntroducetorModel>("user/ThongTinNguoiGioiThieu");
                return data;
            }
            catch (Exception e)
            {
                
                return null;
            }
        }

        public async Task<ResponseModel<string>> AddIntroducetor(NguoiGioiThieuModel input)
        {
            try
            {
                var para = new List<RequestParameter>
                {
                    new RequestParameter("UserGioiThieu",input.UserGioiThieu),
                    new RequestParameter("UserDuocGioiThieu",input.UserDuocGioiThieu),
                    new RequestParameter("Discount",input.Discount+""),
                    new RequestParameter("Note",input.Note),
                };
                var data = await _requestProvider.PostAsync<string>("user/ThemNguoiGioiThieu", para);
                return data;
            }
            catch (Exception e)
            {
                
                return null;
            }
        }

        public async Task<ResponseModel<string>> Sigup(SigupModel sigup)
        {
            try
            {
                var para = new List<RequestParameter>
                {
                    new RequestParameter("UserName",sigup.UserName),
                    new RequestParameter("Password",sigup.Password),
                    new RequestParameter("Name",sigup.Name),
                    new RequestParameter("NumberPhone",sigup.NumberPhone),
                    new RequestParameter("Email",$"{sigup.UserName}@autohlt.com"),
                    new RequestParameter("Age",DateTime.Now.Year.ToString()),
                    new RequestParameter("Sex","0"),
                    new RequestParameter("DateCreate",DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss")),
                };
                var data = await _requestProvider.PostAsync<string>("user/Sigup", para);
                return data;
            }
            catch (Exception e)
            {
                
                return null;
            }
        }

        public async Task<ResponseModel<string>> CheckExistPhone(string phone)
        {
            try
            {
                var para = new List<RequestParameter>
                {
                    new RequestParameter("numberPhone",phone),
                };
                var data = await _requestProvider.GetAsync<string>("user/CheckExistPhone", para);
                return data;
            }
            catch (Exception e)
            {
                
                return null;
            }
        }

        public async Task<ResponseModel<string>> CheckExistUser(string userName)
        {
            try
            {
                var para = new List<RequestParameter>
                {
                    new RequestParameter("username",userName),
                };
                var data = await _requestProvider.GetAsync<string>("user/CheckExistUserName", para);
                return data;
            }
            catch (Exception e)
            {
                
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
                var data = await _requestProvider.PostAsync<LoginModel>("user/Login", para);
                return data;
            }
            catch (Exception e)
            {

                
                return null;
            }
        }
    }
}