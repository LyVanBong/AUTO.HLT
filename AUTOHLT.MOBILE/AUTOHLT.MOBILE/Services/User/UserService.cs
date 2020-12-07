using AUTOHLT.MOBILE.Models.RequestProviderModel;
using AUTOHLT.MOBILE.Models.User;
using AUTOHLT.MOBILE.Services.RequestProvider;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AUTOHLT.MOBILE.Services.User
{
    public class UserService : IUserService
    {
        private IRequestProvider _requestProvider;
        public UserService(IRequestProvider requestProvider)
        {
            _requestProvider = requestProvider;
        }

        public async Task<ResponseModel<string>> HistorySetMoneyForUser(string discount, string price, string idSend, string idReceive, string transferType)
        {
            try
            {
                var para = new List<RequestParameter>
                {
                    new RequestParameter("Discount",discount),
                    new RequestParameter("Price",price),
                    new RequestParameter("IdSend",idSend),
                    new RequestParameter("IdReceive",idReceive),
                    new RequestParameter("TransferType",transferType),
                };
                var data = await _requestProvider.PostAsync<string>("MoneyTransferHistory/add", para);
                return data;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ResponseModel<string>> UpdateUser(string userName, string name, string pass, string email, string number, string sex, string role, string isActive, string age, string price, string idDevice)
        {
            try
            {
                var para = new List<RequestParameter>
                {
                    new RequestParameter("UserName",userName),
                    new RequestParameter("Name",name),
                    new RequestParameter("Password",pass),
                    new RequestParameter("Email",email),
                    new RequestParameter("NumberPhone",number),
                    new RequestParameter("Sex",sex),
                    new RequestParameter("Role",role),
                    new RequestParameter("IsActive",isActive),
                    new RequestParameter("Age",age),
                    new RequestParameter("Price",price),
                    new RequestParameter("IdDevice",idDevice),
                };
                var data = await _requestProvider.PutAsync<string>("user/updateuser", para);
                return data;
            }
            catch (Exception)
            {
                throw;
            }
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
                var data = await _requestProvider.PutAsync<string>("user/transfermoney", para);
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