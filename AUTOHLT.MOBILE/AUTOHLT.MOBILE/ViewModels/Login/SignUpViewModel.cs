using System;
using System.Windows.Input;
using AUTOHLT.MOBILE.Models.Login;
using AUTOHLT.MOBILE.Resources.Languages;
using AUTOHLT.MOBILE.Services.Login;
using AUTOHLT.MOBILE.Services.User;
using Microsoft.AppCenter.Crashes;
using Prism.Navigation;
using Prism.Services;
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
        private ILoginService _loginService;
        private IUserService _userService;
        private bool _hasErrorEmail;
        private bool _hasErrorPass;
        private bool _hasErrorConfirmPass;
        private IPageDialogService _pageDialogService;
        public bool HasErrorConfirmPass
        {
            get => _hasErrorConfirmPass;
            set => SetProperty(ref _hasErrorConfirmPass, value);
        }

        public bool HasErrorPass
        {
            get => _hasErrorPass;
            set => SetProperty(ref _hasErrorPass, value);
        }

        public bool HasErrorEmail
        {
            get => _hasErrorEmail;
            set => SetProperty(ref _hasErrorEmail, value);
        }

        public ICommand UnfocusedCommand { get; private set; }
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
                    CheckDataEnableSignUp();
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
                    CheckDataEnableSignUp();
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
                    CheckDataEnableSignUp();
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
                    CheckDataEnableSignUp();
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
                    CheckDataEnableSignUp();
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
                    CheckDataEnableSignUp();
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
                    CheckDataEnableSignUp();
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

        public SignUpViewModel(INavigationService navigationService, ILoginService loginService, IUserService userService, IPageDialogService pageDialogService) : base(navigationService)
        {
            _pageDialogService = pageDialogService;
            _userService = userService;
            _loginService = loginService;

            UnfocusedCommand = new Command<string>(Unfocused);
            SignUpCommand = new Command(SignUpAccount);
            LoginCommand = new Command(LoginAccount);
        }

        private async void Unfocused(string key)
        {
            try
            {
                var para = int.Parse(key);
                switch (para)
                {
                    case 0:
                        if (!string.IsNullOrWhiteSpace(UserName))
                        {
                            var data = await _userService.CheckExistAccount(userName: UserName);
                            if (data != null && data.Code < 0)
                            {
                                HasErrorEmail = true;
                            }
                            else
                            {
                                HasErrorEmail = false;
                            }
                        }
                        break;
                    case 1:
                        if (!string.IsNullOrWhiteSpace(Password))
                        {
                            if (Password.Length < 6)
                            {
                                HasErrorPass = true;
                            }
                            else
                            {
                                HasErrorPass = false;
                            }

                        }
                        break;
                    case 2:
                        if (!string.IsNullOrWhiteSpace(ConfirmPassword) && Password.Equals(ConfirmPassword))
                        {
                            HasErrorConfirmPass = false;
                        }
                        else
                        {
                            HasErrorConfirmPass = true;
                        }
                        break;
                }
            }
            catch (Exception e)
            {
                Crashes.TrackError(e);
            }
        }

        private void CheckDataEnableSignUp()
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
        private void LoginAccount()
        {
            if (IsLoading) return;
            IsLoading = true;
            var para = new NavigationParameters();
            para.Add("SignUp", "1");
            para.Add("UserName", UserName);
            NavigationService.NavigateAsync("/LoginPage", para, true, true);
        }

        private async void SignUpAccount()
        {
            try
            {
                if (IsLoading) return;
                IsLoading = true;
                var data = await _loginService.SignUp(UserName, Name, Password, PhoneNumber, Email, Age, IsMale);
                if (data != null && data.Code > 0)
                {
                    await _pageDialogService.DisplayAlertAsync(Resource._1000035, Resource._1000040, "OK");
                    Password = ConfirmPassword = Age = Email = PhoneNumber = "";
                }
                else
                {
                    await _pageDialogService.DisplayAlertAsync(Resource._1000035, Resource._1000040, "OK");
                    Password = ConfirmPassword = null;
                }
            }
            catch (Exception e)
            {
                Crashes.TrackError(e);
            }
            finally
            {
                IsLoading = false;
            }
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            IsLoading = false;
        }
    }
}