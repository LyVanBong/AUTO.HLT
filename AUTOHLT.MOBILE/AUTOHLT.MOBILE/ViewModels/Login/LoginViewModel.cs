using AUTOHLT.MOBILE.Configurations;
using AUTOHLT.MOBILE.FakeModules.Views;
using AUTOHLT.MOBILE.Helpers;
using AUTOHLT.MOBILE.Resources.Languages;
using AUTOHLT.MOBILE.Services.Database;
using AUTOHLT.MOBILE.Services.Login;
using AUTOHLT.MOBILE.Views.Home;
using AUTOHLT.MOBILE.Views.Login;
using Microsoft.AppCenter.Crashes;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace AUTOHLT.MOBILE.ViewModels.Login
{
    public class LoginViewModel : ViewModelBase
    {
        private string _password;
        private string _userName;
        private bool _isLoading;
        private ILoginService _loginService;
        private IPageDialogService _pageDialogService;
        private bool _isEnabledLogin;
        private IDatabaseService _databaseService;
        private bool _isCheckSavePassword;

        public bool IsCheckSavePassword
        {
            get => _isCheckSavePassword;
            set => SetProperty(ref _isCheckSavePassword, value);
        }

        public bool IsEnabledLogin
        {
            get => _isEnabledLogin;
            set => SetProperty(ref _isEnabledLogin, value);
        }

        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }

        public string UserName
        {
            get => _userName;
            set
            {
                if (SetProperty(ref _userName, value))
                {
                    if (!string.IsNullOrWhiteSpace(UserName) && !string.IsNullOrWhiteSpace(Password))
                    {
                        IsEnabledLogin = true;
                    }
                    else IsEnabledLogin = false;
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
                    if (!string.IsNullOrWhiteSpace(UserName) && !string.IsNullOrWhiteSpace(Password))
                    {
                        IsEnabledLogin = true;
                    }
                    else IsEnabledLogin = false;
                }
            }
        }

        public ICommand SignUpCommand { get; private set; }
        public ICommand LoginCommand { get; private set; }
        public ICommand LoginWithFaceCommand { get; private set; }
        public LoginViewModel(INavigationService navigationService, ILoginService loginService, IPageDialogService pageDialogService, IDatabaseService databaseService) : base(navigationService)
        {
            _databaseService = databaseService;
            _pageDialogService = pageDialogService;
            _loginService = loginService;
            SignUpCommand = new Command(SignUpAccount);
            LoginCommand = new Command(LoginAccount);
            LoginWithFaceCommand = new Command(LoginWithFace);
            IsLoading = true;
        }

        private async void LoginWithFace()
        {
            await NavigationService.NavigateAsync("/NavigationPage/" + nameof(LoginPage) + "/" + nameof(LoginWithFacebookPage));
        }

        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            IsCheckSavePassword = true;
            if (parameters != null)
            {
                if (parameters.ContainsKey("SignUp"))
                {
                    if (parameters.ContainsKey("UserName"))
                        UserName = parameters["UserName"] as string;
                }
            }

            await InitializeDataLogin();
            IsLoading = false;
        }

        private async Task InitializeDataLogin()
        {
            try
            {
                if (Preferences.Get("IsCheckSavePassword", false))
                {
                    var data = await _databaseService.GetAccountUser();
                    if (data != null)
                    {
                        UserName = data.UserName;
                        Password = data.Password;
                        await DoLogin(Password);
                    }
                }
            }
            catch (Exception e)
            {
                Crashes.TrackError(e);
            }
        }

        private async void LoginAccount()
        {
            if (IsLoading) return;
            IsLoading = true;
            try
            {
                if (!string.IsNullOrWhiteSpace(UserName) && !string.IsNullOrWhiteSpace(Password))
                {
                    await NavigationService.NavigateAsync("/NavigationPage/" + nameof(LoginPage) + "/" + nameof(LoginWithFacebookPage));
                }
                else
                    await _pageDialogService.DisplayAlertAsync("Navigation", "Insufficient information entered", "OK");
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

        private async Task DoLogin(string pass)
        {
            var data = await _loginService.Login(UserName, pass);
            if (data != null)
            {
                if (data.Code > 0 && data.Data != null)
                {
                    if (data.Data.IsActive)
                    {
                        if (IsCheckSavePassword)
                        {
                            Preferences.Set(nameof(IsCheckSavePassword), true);
                            var user = data.Data;
                            user.Price = user.Price.Replace(".0000", "");
                            await _databaseService.UpdateAccountUser(user);
                        }
                        else
                        {
                            Preferences.Set(nameof(IsCheckSavePassword), false);
                        }

                        if (UserName == "khachhang")
                        {
                            await NavigationService.NavigateAsync(nameof(FMain2Page));
                        }
                        else
                        {
                            Preferences.Set(AppConstants.IdUser, data?.Data?.ID);
                            var para = new NavigationParameters();
                            para.Add(AppConstants.LoginApp, "LoginApp");
                            await NavigationService.NavigateAsync(nameof(HomePage), para, true, true);
                        }
                    }
                    else
                    {
                        await _pageDialogService.DisplayAlertAsync(Resource._1000035, "Tài khoản đã bị khóa vui long liên hệ với admin qua số 0824726888 để được hỗ trợ !", "OK");
                    }
                }
                else
                {
                    await _pageDialogService.DisplayAlertAsync(Resource._1000035, Resource._1000036, "OK");
                }
            }
            else
            {
                await _pageDialogService.DisplayAlertAsync(Resource._1000035, Resource._1000036, "OK");
            }

            Password = "";
        }

        private void SignUpAccount()
        {
            if (IsLoading) return;
            IsLoading = true;
            NavigationService.NavigateAsync(nameof(SignUpPage), null, true, true);
            IsLoading = false;
        }
    }
}