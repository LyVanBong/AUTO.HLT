using Realms;

namespace AUTOHLT.MOBILE.Models.User
{
    public class UserModel : RealmObject
    {
        [PrimaryKey]
        public string ID { get; set; }

        public string UserName { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public int NumberPhone { get; set; }
        public string Email { get; set; }
        public int Sex { get; set; }
        public int Role { get; set; }
        public bool IsActive { get; set; }
        public int Age { get; set; }
        public string DateCreate { get; set; }
        public int Price { get; set; }
        public string IdDevice { get; set; }
    }
}