using System;
using System.Windows.Input;
using AUTOHLT.MOBILE.Models.Login;
using Prism.Navigation;
using Xamarin.Forms;

namespace AUTOHLT.MOBILE.ViewModels.Login
{
    public class SignUpViewModel : ViewModelBase
    {
        private bool _isLoading;
        private SignUpModel _signUp;
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
      

        public ICommand SignUpCommand { get; private set; }
        public ICommand LoginCommand { get; private set; }
        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }

        public SignUpViewModel(INavigationService navigationService) : base(navigationService)
        {
            SignUpCommand=new Command(SignUpAccount);
            LoginCommand = new Command(LoginAccount);
        }

        private void LoginAccount()
        {
            if (IsLoading) return;
            IsLoading = true;
            var para = new NavigationParameters();
            para.Add("SignUp","1");
            para.Add("UserName",UserName);
            NavigationService.NavigateAsync("/LoginPage",para);
        }

        private void SignUpAccount()
        {
            
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            IsLoading = false;
        }
    }
}