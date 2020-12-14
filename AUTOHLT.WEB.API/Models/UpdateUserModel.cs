namespace AUTOHLT.WEB.API.Models
{
    public class UpdateUserModel
    {
        public string UserName { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string NumberPhone { get; set; }
        public int Sex { get; set; }
        public int Role { get; set; }
        public bool IsActive { get; set; }
        public string Age { get; set; }
        public int Price { get; set; }
        public string IdDevice { get; set; }
    }
}