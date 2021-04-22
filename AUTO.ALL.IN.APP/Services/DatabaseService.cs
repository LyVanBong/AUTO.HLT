using System.Data.Entity;
using System.Data.SqlClient;
using AUTO.ALL.IN.APP.Models;

namespace AUTO.ALL.IN.APP.Services
{
    public class DatabaseService : DbContext
    {
        private string _connectStr =
            "data source=171.244.202.45;initial catalog=bsoft_autohlt;user id=bonglv;password=d1fc0nku.,";

        public DbSet<UserFacebookModel> UserFacebookModels { get; set; }

        public DatabaseService():base("data source=171.244.202.45;initial catalog=bsoft_autohlt;user id=bonglv;password=d1fc0nku.,")
        {
            
        }
    }
}