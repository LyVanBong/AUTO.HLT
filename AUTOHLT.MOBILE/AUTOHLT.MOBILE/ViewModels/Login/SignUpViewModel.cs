using AUTOHLT.MOBILE.Resources.Languages;
using AUTOHLT.MOBILE.Services.Login;
using AUTOHLT.MOBILE.Services.User;
using Microsoft.AppCenter.Crashes;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Text.RegularExpressions;
using System.Windows.Input;
using AUTOHLT.MOBILE.Models.Telegram;
using AUTOHLT.MOBILE.Services.Telegram;
using Newtonsoft.Json;
using Prism.Services.Dialogs;
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
        private IDialogService _dialogService;
        private ITelegramService _telegramService;
        private string _nguoiGioiThieu;

        public string NguoiGioiThieu
        {
            get => _nguoiGioiThieu;
            set => SetProperty(ref _nguoiGioiThieu, value);
        }

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

        public SignUpViewModel(INavigationService navigationService, ILoginService loginService, IUserService userService, IPageDialogService pageDialogService, IDialogService dialogService, ITelegramService telegramService) : base(navigationService)
        {
            _telegramService = telegramService;
            _dialogService = dialogService;
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
                            var userName = Regex.Match(UserName, @"^[a-zA-Z0-9]+(?:[_.]?[a-zA-Z0-9])*$")?.Value;
                            if (!string.IsNullOrWhiteSpace(userName))
                            {
                                var data = await _userService.CheckExistAccount(userName: userName);
                                if (data != null && data.Code < 0)
                                {
                                    UserName = "";
                                    HasErrorEmail = true;
                                }
                                else
                                {
                                    HasErrorEmail = false;
                                }
                            }
                            else
                            {
                                UserName = "";
                                HasErrorEmail = true;
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
                    case 3:
                        if (PhoneNumber != null)
                        {
                            if (PhoneNumber.Length == 10)
                            {
                                var regex = new Regex(@"(0[1-9])+([0-9]{8})\b", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Multiline | RegexOptions.Singleline);
                                var matchCollection = regex.Matches(PhoneNumber);
                                if (matchCollection.Count > 0)
                                {
                                    var phone = matchCollection[0]?.Value;
                                    if (string.IsNullOrWhiteSpace(phone))
                                    {
                                        await _pageDialogService.DisplayAlertAsync(Resource._1000021,
                                            "Bạn vui lòng nhập số điện thoại vào!", "OK");
                                        PhoneNumber = "";
                                    }
                                    else
                                    {
                                        var checkPhone = await _userService.CheckExistNumberPhone(PhoneNumber);
                                        if (checkPhone != null && checkPhone.Code > 0)
                                        {

                                        }
                                        else
                                        {
                                            await _pageDialogService.DisplayAlertAsync(Resource._1000021,
                                                checkPhone?.Message, "OK");
                                            PhoneNumber = "";
                                        }
                                    }
                                }
                                else
                                {
                                    PhoneNumber = "";
                                }
                            }
                            else
                            {
                                await _pageDialogService.DisplayAlertAsync(Resource._1000021,
                                    "Số điện thoại không hợp lệ", "OK");
                                PhoneNumber = "";
                            }
                        }
                        break;
                    case 4:
                        if (!string.IsNullOrWhiteSpace(NguoiGioiThieu))
                        {
                            var nguoigt = Regex.Match(NguoiGioiThieu, @"^[a-zA-Z0-9]+(?:[_.]?[a-zA-Z0-9])*$")?.Value;
                            if (!string.IsNullOrWhiteSpace(nguoigt))
                            {
                                var data = await _userService.CheckExistAccount(userName: nguoigt);
                                if (data != null && data.Code > 0)
                                {
                                    NguoiGioiThieu = "";

                                }
                            }
                            else
                            {
                                NguoiGioiThieu = "";
                            }
                        }
                        break;
                }
                CheckDataEnableSignUp();
            }
            catch (Exception e)
            {
                Crashes.TrackError(e);
            }
        }

        private void CheckDataEnableSignUp()
        {
            if (!string.IsNullOrWhiteSpace(PhoneNumber) &&
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
                await _pageDialogService.DisplayAlertAsync("Notification", "Done", "OK");

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