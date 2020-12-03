using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AUTOHLT.MOBILE.Models.RequestProviderModel;
using AUTOHLT.MOBILE.Services.RequestProvider;
using Microsoft.AppCenter.Crashes;

namespace AUTOHLT.MOBILE.Services.User
{
    public class UserService : IUserService
    {
        private IRequestProvider _requestProvider;
        public UserService(IRequestProvider requestProvider)
        {
            _requestProvider = requestProvider;
        }

        public async Task<ResponseModel<string>> TransferMoney(string idSend, string idReceive, string price)
        {
            try
            {
                var para = new List<RequestParameter>
                {
                    new RequestParameter("IdSend",idSend),
                    new RequestParameter("IdReceive",idReceive),
                    new RequestParameter("Price",price),
                };
                var data = await _requestProvider.GetAsync<string>("user/transfermoney", para);
                return data;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ResponseModel<string>> GetMoneyUser(string userName)
        {
            try
            {
                var para = new List<RequestParameter>
                {
                    new RequestParameter("username",userName),
                };
                var data = await _requestProvider.GetAsync<string>("user/getpriceuser", para);
                return data;
            }
            catch (Exception)
            {
                throw;
            }
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