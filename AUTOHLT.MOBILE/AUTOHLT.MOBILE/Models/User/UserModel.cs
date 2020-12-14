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
        public string NumberPhone { get; set; }
        public string Email { get; set; }
        public string Sex { get; set; }
        public string Role { get; set; }
        public bool IsActive { get; set; }
        public string Age { get; set; }
        public string DateCreate { get; set; }
        public string Price { get; set; }
        public string IdDevice { get; set; }
    }
}