using System;
using System.Threading.Tasks;
using AUTO.TOOL.CLIENT.Models.Login;

namespace AUTO.TOOL.CLIENT.Services.Database
{
    public class DatabaseService : IDatabaseService
    {
        public void RemoveDatabaseLocal()
        {
            throw new NotImplementedException();
        }

        public Task<bool> SetAccountUser(LoginModel user)
        {
            throw new NotImplementedException();
        }

        public Task<LoginModel> GetAccountUser()
        {
            throw new NotImplementedException();
        }

        public Task DeleteAccontUser()
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateAccountUser(LoginModel user)
        {
            throw new NotImplementedException();
        }
    }
}