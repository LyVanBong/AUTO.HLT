using System;

namespace AUTOHLT.WEB.API.Models
{
    public class RegistrationAccountModel
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string NumberPhone { get; set; }
        public string Email { get; set; }
        public string Age { get; set; }
        public int Sex { get; set; }
        public DateTime DateCreate { get; set; }
    }
}