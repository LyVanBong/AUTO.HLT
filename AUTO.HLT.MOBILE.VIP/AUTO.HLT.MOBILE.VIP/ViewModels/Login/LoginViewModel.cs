using AUTO.HLT.MOBILE.VIP.Configurations;
using AUTO.HLT.MOBILE.VIP.Helpers;
using AUTO.HLT.MOBILE.VIP.Services.Login;
using AUTO.HLT.MOBILE.VIP.Views.Login;
using Microsoft.AppCenter.Crashes;
using Prism.Navigation;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using AUTO.HLT.MOBILE.VIP.Services.Database;
using Prism.Services;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace AUTO.HLT.MOBILE.VIP.ViewModels.Login
{
    public class LoginViewModel : ViewModelBase
    {
        private View _contentLoginPage;
        private string _userName;
        private string _passwd;
        private bool _isSavePasswd;
        private string _fullName;
        private string _phoneNumber;
        private string _nguoiGioiThieu;
        private ILoginService _loginService;
        private IPageDialogService _pageDialogService;
        private IDatabaseService _databaseService;

        public View ContentLoginPage
        {
            get => _contentLoginPage;
            set => SetProperty(ref _contentLoginPage, value);
        }

        public ICommand FunctionExecuteCommand { get; private set; }
        /// <summary>
        /// ten dung de dang nhap
        /// </summary>
        public string UserName
        {
            get => _userName;
            set => SetProperty(ref _userName, value);
        }
        /// <summary>
        /// mat khau
        /// </summary>
        public string Passwd
        {
            get => _passwd;
            set => SetProperty(ref _passwd, value);
        }
        /// <summary>
        /// luu mat khau tai khoan
        /// </summary>
        public bool IsSavePasswd
        {
            get => _isSavePasswd;
            set => SetProperty(ref _isSavePasswd, value);
        }
        /// <summary>
        /// ten nguoi dung
        /// </summary>
        public string FullName
        {
            get => _fullName;
            set => SetProperty(ref _fullName, value);
        }

        public string PhoneNumber
        {
            get => _phoneNumber;
            set => _phoneNumber = value;
        }

        public string NguoiGioiThieu
        {
            get => _nguoiGioiThieu;
            set => SetProperty(ref _nguoiGioiThieu, value);
        }

        public LoginViewModel(INavigationService navigationService, ILoginService loginService, IPageDialogService pageDialogService, IDatabaseService databaseService) : base(navigationService)
        {
            _databaseService = databaseService;
            _pageDialogService = pageDialogService;
            _loginService = loginService;
            ContentLoginPage = new LoginView();
            FunctionExecuteCommand = new AsyncCommand<string>(async (key) => await FunctionExecute(key));
            IsSavePasswd = true;
        }

        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            try
            {
                if (Preferences.Get(nameof(IsSavePasswd), false))
                {
                    var dataUser = await _databaseService.GetAccountUser();
                    if (dataUser?.UserName != null)
                    {
                        UserName = dataUser.UserName;
                        await DoLogin(UserName, dataUser.Password);
                    }
                }
            }
            catch (Exception e)
            {
                Crashes.TrackError(e);
            }
        }

        private async Task FunctionExecute(string key)
        {
            switch (key)
            {
                case "0":
                    if (UserName != null && Passwd != null)
                    {
                        await DoLogin(UserName, HashFunctionHelper.GetHashCode(Passwd, 1));
                    }
                    else
                    {
                        await _pageDialogService.DisplayAlertAsync("Thông báo", "Vui lòng điền đầy đủ thông tin đăng nhập",
                            "OK");
                    }
                    break;
                case "1":
                    ContentLoginPage = new SigupView();
                    break;
                case "2":
                    await DoSigup();
                    break;
                case "3":
                    ContentLoginPage = new LoginView();
                    break;
                case "4":
                    await CheckUserName(UserName);
                    break;
                default:
                    break;
            }
        }

        private async Task CheckUserName(string userName)
        {
            try
            {

            }
            catch (Exception e)
            {
                Crashes.TrackError(e);
            }
        }

        private async Task DoSigup()
        {
            try
            {

            }
            catch (Exception e)
            {
                Crashes.TrackError(e);
                await _pageDialogService.DisplayAlertAsync("Thông báo", "Lỗi phát sinh trong quá trình xử lý vui long thử lại",
                    "OK");
            }
        }

        private async Task DoLogin(string user, string pwd)
        {
            try
            {
                var login = await _loginService.Login(user, pwd);
                if (login != null && login.Code > 0 & login.Data != null)
                {
                    App.UserLogin = login.Data;
                    if (IsSavePasswd)
                    {
                        Preferences.Set(nameof(IsSavePasswd), true);
                    }
                    else
                    {
                        Preferences.Set(nameof(IsSavePasswd), false);
                    }
                    await _databaseService.SetAccountUser(login.Data);
                    Preferences.Set(AppConstants.Authorization, login.Data.Jwt);
                    await NavigationService.NavigateAsync("/HomePage", null, false, true);
                }
                else
                {
                    await _pageDialogService.DisplayAlertAsync("Thông báo", "Tài khoản hoặc mật khẩu không đúng",
                        "OK");
                }
            }
            catch (Exception e)
            {
                Crashes.TrackError(e);
                await _pageDialogService.DisplayAlertAsync("Thông báo", "Lỗi phát sinh trong quá trình xử lý vui long thử lại",
                    "OK");
            }
        }
    }
}