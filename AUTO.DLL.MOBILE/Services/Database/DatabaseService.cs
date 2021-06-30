using System.Threading.Tasks;
using AUTO.DLL.MOBILE.Models.Login;

namespace AUTO.DLL.MOBILE.Services.Database
{
    public class DatabaseService : IDatabaseService
    {
        public void RemoveDatabaseLocal()
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> SetAccountUser(LoginModel user)
        {
            throw new System.NotImplementedException();
        }

        public Task<LoginModel> GetAccountUser()
        {
            throw new System.NotImplementedException();
        }

        public Task DeleteAccontUser()
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> UpdateAccountUser(LoginModel user)
        {
            throw new System.NotImplementedException();
        }
    }
}