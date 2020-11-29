using System;
using System.Windows.Input;
using AUTOHLT.MOBILE.Models.Login;
using Microsoft.AppCenter.Crashes;
using Prism.Navigation;
using Xamarin.Forms;

namespace AUTOHLT.MOBILE.ViewModels.Login
{
    public class SignUpViewModel : ViewModelBase
    {
        private bool _isLoading;
        private bool _isMale;
        private string _age;
        private string _email;
        private string _phoneNumber;
        private string _confirmPassword;
        private string _password;
        private string _name;
        private string _userName;
        private bool _isEnableSignUp;

        public bool IsEnableSignUp
        {
            get => _isEnableSignUp;
            set => SetProperty(ref _isEnableSignUp, value);
        }

        public string UserName
        {
            get => _userName;
            set
            {
                if (SetProperty(ref _userName, value))
                {
                    if (!string.IsNullOrWhiteSpace(Age) &&
                        !string.IsNullOrWhiteSpace(Email) && !string.IsNullOrWhiteSpace(PhoneNumber) &&
                        !string.IsNullOrWhiteSpace(ConfirmPassword) && !string.IsNullOrWhiteSpace(Password) &&
                        !string.IsNullOrWhiteSpace(Name) && !string.IsNullOrWhiteSpace(UserName))
                    {
                        IsEnableSignUp = true;
                    }
                    else
                    {
                        IsEnableSignUp = false;
                    }
                }
            }
        }

        public string Name
        {
            get => _name;
            set
            {
                if (SetProperty(ref _name, value))
                {
                    if (!string.IsNullOrWhiteSpace(Age) &&
                        !string.IsNullOrWhiteSpace(Email) && !string.IsNullOrWhiteSpace(PhoneNumber) &&
                        !string.IsNullOrWhiteSpace(ConfirmPassword) && !string.IsNullOrWhiteSpace(Password) &&
                        !string.IsNullOrWhiteSpace(Name) && !string.IsNullOrWhiteSpace(UserName))
                    {
                        IsEnableSignUp = true;
                    }
                    else
                    {
                        IsEnableSignUp = false;
                    }
                }
            }
        }

        public string Password
        {
            get => _password;
            set
            {
                if (SetProperty(ref _password, value))
                {
                    if (!string.IsNullOrWhiteSpace(Age) &&
                        !string.IsNullOrWhiteSpace(Email) && !string.IsNullOrWhiteSpace(PhoneNumber) &&
                        !string.IsNullOrWhiteSpace(ConfirmPassword) && !string.IsNullOrWhiteSpace(Password) &&
                        !string.IsNullOrWhiteSpace(Name) && !string.IsNullOrWhiteSpace(UserName))
                    {
                        IsEnableSignUp = true;
                    }
                    else
                    {
                        IsEnableSignUp = false;
                    }
                }
            }
        }

        public string ConfirmPassword
        {
            get => _confirmPassword;
            set
            {
                if (SetProperty(ref _confirmPassword, value))
                {
                    if (!string.IsNullOrWhiteSpace(Age) &&
                        !string.IsNullOrWhiteSpace(Email) && !string.IsNullOrWhiteSpace(PhoneNumber) &&
                        !string.IsNullOrWhiteSpace(ConfirmPassword) && !string.IsNullOrWhiteSpace(Password) &&
                        !string.IsNullOrWhiteSpace(Name) && !string.IsNullOrWhiteSpace(UserName))
                    {
                        IsEnableSignUp = true;
                    }
                    else
                    {
                        IsEnableSignUp = false;
                    }
                }
            }
        }

        public string PhoneNumber
        {
            get => _phoneNumber;
            set
            {
                if (SetProperty(ref _phoneNumber, value))
                {
                    if (!string.IsNullOrWhiteSpace(Age) &&
                        !string.IsNullOrWhiteSpace(Email) && !string.IsNullOrWhiteSpace(PhoneNumber) &&
                        !string.IsNullOrWhiteSpace(ConfirmPassword) && !string.IsNullOrWhiteSpace(Password) &&
                        !string.IsNullOrWhiteSpace(Name) && !string.IsNullOrWhiteSpace(UserName))
                    {
                        IsEnableSignUp = true;
                    }
                    else
                    {
                        IsEnableSignUp = false;
                    }
                }
            }
        }

        public string Email
        {
            get => _email;
            set
            {
                if (SetProperty(ref _email, value))
                {
                    if (!string.IsNullOrWhiteSpace(Age) &&
                        !string.IsNullOrWhiteSpace(Email) && !string.IsNullOrWhiteSpace(PhoneNumber) &&
                        !string.IsNullOrWhiteSpace(ConfirmPassword) && !string.IsNullOrWhiteSpace(Password) &&
                        !string.IsNullOrWhiteSpace(Name) && !string.IsNullOrWhiteSpace(UserName))
                    {
                        IsEnableSignUp = true;
                    }
                    else
                    {
                        IsEnableSignUp = false;
                    }
                }
            }
        }

        public string Age
        {
            get => _age;
            set
            {
                if (SetProperty(ref _age, value))
                {
                    if (!string.IsNullOrWhiteSpace(Age) &&
                        !string.IsNullOrWhiteSpace(Email) && !string.IsNullOrWhiteSpace(PhoneNumber) &&
                        !string.IsNullOrWhiteSpace(ConfirmPassword) && !string.IsNullOrWhiteSpace(Password) &&
                        !string.IsNullOrWhiteSpace(Name) && !string.IsNullOrWhiteSpace(UserName))
                    {
                        IsEnableSignUp = true;
                    }
                    else
                    {
                        IsEnableSignUp = false;
                    }
                }
            }
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
            SignUpCommand = new Command(SignUpAccount);
            LoginCommand = new Command(LoginAccount);
        }

        private void LoginAccount()
        {
            if (IsLoading) return;
            IsLoading = true;
            var para = new NavigationParameters();
            para.Add("SignUp", "1");
            para.Add("UserName", UserName);
            NavigationService.NavigateAsync("/LoginPage", para);
        }

        private void SignUpAccount()
        {
            try
            {

            }
            catch (Exception e)
            {
                Crashes.TrackError(e);
            }
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            IsLoading = false;
        }
    }
}