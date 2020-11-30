using System;
using System.Windows.Input;
using AUTOHLT.MOBILE.Helpers;
using AUTOHLT.MOBILE.Resources.Languages;
using AUTOHLT.MOBILE.Services.Database;
using AUTOHLT.MOBILE.Services.Login;
using AUTOHLT.MOBILE.Views.Home;
using AUTOHLT.MOBILE.Views.Login;
using Microsoft.AppCenter.Crashes;
using Prism.Navigation;
using Prism.Services;
using Prism.Services.Dialogs;
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

        public bool IsCheckSavePassword
        {
            get => Preferences.Get(nameof(IsCheckSavePassword), true);
            set => Preferences.Set(nameof(IsCheckSavePassword), IsCheckSavePassword);
        }

        public ICommand SignUpCommand { get; private set; }
        public ICommand LoginCommand { get; private set; }
        public LoginViewModel(INavigationService navigationService, ILoginService loginService, IPageDialogService pageDialogService, IDatabaseService databaseService) : base(navigationService)
        {
            _databaseService = databaseService;
            _pageDialogService = pageDialogService;
            _loginService = loginService;
            SignUpCommand = new Command(SignUpAccount);
            LoginCommand = new Command(LoginAccount);
            IsLoading = true;
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            if (parameters != null)
            {
                if (parameters.ContainsKey("SignUp"))
                {
                    if (parameters.ContainsKey("UserName"))
                        UserName = parameters["UserName"] as string;
                }
            }
            IsLoading = false;
        }

        private async void LoginAccount()
        {
            if (IsLoading) return;
            IsLoading = true;
            try
            {
                if (!string.IsNullOrWhiteSpace(UserName) && !string.IsNullOrWhiteSpace(Password))
                {
                    var data = await _loginService.Login(UserName, HashFunctionHelper.GetHashCode(Password, 1));
                    if (data != null)
                    {
                        if (data.Code > 0 && data.Data != null)
                        {
                            if (IsCheckSavePassword)
                            {
                                Preferences.Set(nameof(IsCheckSavePassword), true);
                                await _databaseService.UpdateAccountUser(data.Data);
                            }
                            else
                            {
                                Preferences.Set(nameof(IsCheckSavePassword), false);
                            }
                            await NavigationService.NavigateAsync(nameof(HomePage), null, true, true);
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

        private void SignUpAccount()
        {
            if (IsLoading) return;
            IsLoading = true;
            NavigationService.NavigateAsync(nameof(SignUpPage), null, true, true);
        }
    }
}