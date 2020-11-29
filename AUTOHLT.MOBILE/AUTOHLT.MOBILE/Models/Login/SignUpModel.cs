using Prism.Mvvm;

namespace AUTOHLT.MOBILE.Models.Login
{
    public class SignUpModel : BindableBase
    {
        private bool _isMale;
        private string _age;
        private string _email;
        private string _phoneNumber;
        private string _confirmPassword;
        private string _password;
        private string _name;
        private string _userName;
        public string UserName
        {
            get => _userName;
            set => SetProperty(ref _userName, value);
        }

        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }

        public string ConfirmPassword
        {
            get => _confirmPassword;
            set => SetProperty(ref _confirmPassword, value);
        }

        public string PhoneNumber
        {
            get => _phoneNumber;
            set => SetProperty(ref _phoneNumber, value);
        }

        public string Email
        {
            get => _email;
            set => SetProperty(ref _email, value);
        }

        public string Age
        {
            get => _age;
            set => SetProperty(ref _age, value);
        }

        public bool IsMale
        {
            get => _isMale;
            set => SetProperty(ref _isMale, value);
        }
    }
}