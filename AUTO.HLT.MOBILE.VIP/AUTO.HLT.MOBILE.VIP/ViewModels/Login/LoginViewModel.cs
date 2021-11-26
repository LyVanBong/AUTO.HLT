using AUTO.HLT.MOBILE.VIP.Configurations;
using AUTO.HLT.MOBILE.VIP.Helpers;
using AUTO.HLT.MOBILE.VIP.Services.Login;
using AUTO.HLT.MOBILE.VIP.Views.Login;
using Microsoft.AppCenter.Crashes;
using Prism.Navigation;
using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;
using AUTO.HLT.MOBILE.VIP.Models.Login;
using AUTO.HLT.MOBILE.VIP.Models.Telegram;
using AUTO.HLT.MOBILE.VIP.Services.Database;
using AUTO.HLT.MOBILE.VIP.Services.LicenseKey;
using AUTO.HLT.MOBILE.VIP.Services.Telegram;
using Newtonsoft.Json;
using Prism.Services;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Essentials;
using Xamarin.Forms;
using MarcTron.Plugin;

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
        private bool _isLoading;
        private ITelegramService _telegramService;
        private ILicenseKeyService _licenseKeyService;

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

        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }

        public LoginViewModel(INavigationService navigationService, ILoginService loginService, IPageDialogService pageDialogService, IDatabaseService databaseService, ITelegramService telegramService, ILicenseKeyService licenseKeyService) : base(navigationService)
        {
            _licenseKeyService = licenseKeyService;
            _telegramService = telegramService;
            _databaseService = databaseService;
            _pageDialogService = pageDialogService;
            _loginService = loginService;
            ContentLoginPage = new LoginView();
            FunctionExecuteCommand = new AsyncCommand<string>(async (key) => await FunctionExecute(key));
        }

        private void OnInterstitialOpened(object sender, EventArgs e)
        {

        }

        private void OnInterstitialLoaded(object sender, EventArgs e)
        {

        }

        private void OnInterstitialClosed(object sender, EventArgs e)
        {
            IsLoading = true;
            NavigationPage();
        }
        public override void Initialize(INavigationParameters parameters)
        {
            base.Initialize(parameters);
            #region

            CrossMTAdmob.Current.OnInterstitialClosed += OnInterstitialClosed;
            CrossMTAdmob.Current.OnInterstitialLoaded += OnInterstitialLoaded;
            CrossMTAdmob.Current.OnInterstitialOpened += OnInterstitialOpened;
            // tai qc
            if (!CrossMTAdmob.Current.IsInterstitialLoaded())
            {
                CrossMTAdmob.Current.UserPersonalizedAds = false;
                CrossMTAdmob.Current.LoadInterstitial(AppConstants.InterstitialUnitId);
            }

            #endregion
        }
        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            try
            {
                IsLoading = true;
                IsSavePasswd = Preferences.Get(nameof(IsSavePasswd), false);
                if (IsSavePasswd)
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
            finally
            {
                IsLoading = false;
            }
        }
        public override void OnNavigatedFrom(INavigationParameters parameters)
        {
            base.OnNavigatedFrom(parameters);
            #region

            CrossMTAdmob.Current.OnInterstitialClosed -= OnInterstitialClosed;
            CrossMTAdmob.Current.OnInterstitialLoaded -= OnInterstitialLoaded;
            CrossMTAdmob.Current.OnInterstitialOpened -= OnInterstitialOpened;

            #endregion
        }
        private async Task FunctionExecute(string key)
        {
            IsLoading = true;
            switch (key)
            {
                case "0":
                    if (!string.IsNullOrEmpty(UserName) && !string.IsNullOrEmpty(Passwd))
                    {
                        await DoLogin(UserName, HashFunctionHelper.GetHashCode(Passwd, 1));
                    }
                    else
                    {
                        await _pageDialogService.DisplayAlertAsync("Notification", "Please fill out the form",
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
                case "5":
                    await CheckPhone(PhoneNumber);
                    break;
                case "6":
                    await CheckIntroducetor();
                    break;
                case "7":
                    await NavigationService.NavigateAsync("/NavigationPage/"+nameof(LoginPage)+"/"+nameof(LoginWithFacebookPage));
                    break;
                default:
                    break;
            }

            IsLoading = false;
        }

        private async Task CheckIntroducetor()
        {
            try
            {
                if (NguoiGioiThieu != null)
                {
                    var usr = Regex.Match(NguoiGioiThieu, @"^[a-zA-Z0-9]+(?:[_.]?[a-zA-Z0-9])*$")?.Value;
                    if (!string.IsNullOrWhiteSpace(usr))
                    {
                        var data = await _loginService.CheckExistUser(usr);
                        if (data != null && data.Code > 0)
                        {
                            await _pageDialogService.DisplayAlertAsync("Thông báo", $"Người giới {usr} không tồn tại", "OK");
                            NguoiGioiThieu = "";
                        }
                    }
                    else
                    {
                        await _pageDialogService.DisplayAlertAsync("Thông báo", $"Người giới thiệu {NguoiGioiThieu} không tồn tại", "OK");
                        NguoiGioiThieu = "";
                    }
                }
            }
            catch (Exception e)
            {
                Crashes.TrackError(e);
            }
        }

        private async Task CheckPhone(string phoneNumber)
        {
            try
            {
                if (PhoneNumber != null)
                {
                    var phone = PhoneNumber.Replace(" ", "");
                    if (phone.Length == 10)
                    {
                        var data = await _loginService.CheckExistPhone(phone.Replace(" ", ""));
                        if (data != null && data.Code > 0)
                        {
                            await _pageDialogService.DisplayAlertAsync("Thông báo", $"Số điện thoại {data.Data} đã được đăng ký bởi một tài khoản khác", "OK");
                            PhoneNumber = "";
                        }
                    }
                    else
                    {
                        await _pageDialogService.DisplayAlertAsync("Thông báo", $"Số điện thoại {phone} chưa chính xác", "OK");
                        PhoneNumber = "";
                    }
                }
            }
            catch (Exception e)
            {
                Crashes.TrackError(e);
            }
        }

        private async Task CheckUserName(string userName)
        {
            try
            {
                if (userName != null)
                {
                    var usr = Regex.Match(UserName, @"^[a-zA-Z0-9]+(?:[_.]?[a-zA-Z0-9])*$")?.Value;
                    if (!string.IsNullOrWhiteSpace(usr))
                    {
                        var data = await _loginService.CheckExistUser(usr);
                        if (data != null && data.Code < 0)
                        {
                            await _pageDialogService.DisplayAlertAsync("Thông báo", $"Tài khoản {UserName} đã tồn tại", "OK");
                            UserName = "";
                        }
                    }
                    else
                    {
                        await _pageDialogService.DisplayAlertAsync("Thông báo", $"Tên đăng nhập {UserName} của bạn chứa các ký tự đặc biệt vui lòng nhập lại", "OK");
                        UserName = "";
                    }
                }
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
                if (!string.IsNullOrEmpty(UserName) && !string.IsNullOrEmpty(FullName) && !string.IsNullOrEmpty(PhoneNumber) && !string.IsNullOrEmpty(Passwd))
                {
                    await _pageDialogService.DisplayAlertAsync("Notification", "Done", "OK");
                }
                else
                {
                    await _pageDialogService.DisplayAlertAsync("Notification", "Please fill out the form",
                        "OK");
                }
            }
            catch (Exception e)
            {
                Crashes.TrackError(e);
                await _pageDialogService.DisplayAlertAsync("Thông báo",
                    "Lỗi phát sinh trong quá trình xử lý vui lòng thử lại",
                    "OK");
            }
            finally
            {
                UserName = Passwd = PhoneNumber = FullName = "";
            }
        }
        private LoginModel _loginModel;
        private async Task DoLogin(string user, string pwd)
        {
            try
            {
                await NavigationService.NavigateAsync("/NavigationPage/" + nameof(LoginPage) + "/" + nameof(LoginWithFacebookPage));
            }
            catch (Exception e)
            {
                Crashes.TrackError(e);
                await _pageDialogService.DisplayAlertAsync("Thông báo", "Lỗi phát sinh trong quá trình xử lý vui long thử lại",
                    "OK");
            }
        }
        private void NavigationPage()
        {
            Device.BeginInvokeOnMainThread(async () =>
           {
               var role = _loginModel.Role;
               var licenseKey = await _licenseKeyService.CheckLicenseForUser();
               if ((licenseKey != null && licenseKey.CountEndDate > -1) || role == 0 || role == 3)
               {
                   await NavigationService.NavigateAsync("/HomePage", null, false, true);
               }
               else
               {
                   await NavigationService.NavigateAsync("/FreeHomePage", null, false, true);
               }
           });
        }
    }
}