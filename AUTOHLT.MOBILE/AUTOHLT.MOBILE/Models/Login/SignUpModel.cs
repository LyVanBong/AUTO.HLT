using Prism.Mvvm;

namespace AUTOHLT.MOBILE.Models.Login
{
    public class SignUpModel : BindableBase
    {
        public string UserName { get; set; }

        public string Name { get; set; }

        public string Password { get; set; }

        public string ConfirmPassword { get; set; }
        public string PhoneNumber { get; set; }

        public string Email { get; set; }

        public string Age { get; set; }

        public bool IsMale { get; set; }
    }
}