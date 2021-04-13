using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;
using AUTO.HLT.MOBILE.VIP.Configurations;
using AUTO.HLT.MOBILE.VIP.Helpers;
using AUTO.HLT.MOBILE.VIP.Models.Login;
using AUTO.HLT.MOBILE.VIP.Models.Telegram;
using AUTO.HLT.MOBILE.VIP.Models.User;
using AUTO.HLT.MOBILE.VIP.Services.Login;
using AUTO.HLT.MOBILE.VIP.Services.Telegram;
using AUTO.HLT.MOBILE.VIP.Services.User;
using Microsoft.AppCenter.Crashes;
using Newtonsoft.Json;
using Prism.Navigation;
using Prism.Services;
using Xamarin.CommunityToolkit.ObjectModel;

namespace AUTO.HLT.MOBILE.VIP.ViewModels.KeyGeneration
{
    public class KeyGenerationViewModel : ViewModelBase
    {
        private string _userName;
        private string _fullName;
        private string _passwd = "Autovip@12345";
        private string _phoneNumber;
        private ILoginService _loginService;
        private IPageDialogService _pageDialogService;
        private ITelegramService _telegramService;
        private IUserService _userService;
        private int _selectedIndex;
        private ObservableRangeCollection<UserModel> _userData;
        private bool _isLoading;
        private List<UserModel> _userCache;
        private int _countUser;

        public string UserName
        {
            get => _userName;
            set => SetProperty(ref _userName, value);
        }

        public string FullName
        {
            get => _fullName;
            set => SetProperty(ref _fullName, value);
        }

        public string Passwd
        {
            get => _passwd;
            set => SetProperty(ref _passwd, value);
        }

        public string PhoneNumber
        {
            get => _phoneNumber;
            set => SetProperty(ref _phoneNumber, value);
        }

        public ICommand FunctionExecuteCommand { get; private set; }
        public ICommand ResetPasswdCommand { get; private set; }

        public int SelectedIndex
        {
            get => _selectedIndex;
            set => SetProperty(ref _selectedIndex, value);
        }

        public ObservableRangeCollection<UserModel> UserData
        {
            get => _userData;
            set => SetProperty(ref _userData, value);
        }

        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }

        public int CountUser
        {
            get => _countUser;
            set => SetProperty(ref _countUser, value);
        }

        public KeyGenerationViewModel(INavigationService navigationService, ILoginService loginService, IPageDialogService pageDialogService, ITelegramService telegramService, IUserService userService) : base(navigationService)
        {
            _userService = userService;
            _telegramService = telegramService;
            _loginService = loginService;
            _pageDialogService = pageDialogService;
            FunctionExecuteCommand = new AsyncCommand<string>(async (key) => await FunctionExecute(key));
            ResetPasswdCommand = new AsyncCommand<string>(async (key) => await ResetPasswd(key));
        }

        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            IsLoading = true;
            await InitializeData();
            IsLoading = false;
        }

        private async Task InitializeData()
        {
            var data = await AllUser();
            if (data != null)
            {
                UserData = new ObservableRangeCollection<UserModel>(data);
                CountUser = data.Count;
            }
        }

        private async Task<List<UserModel>> AllUser()
        {
            try
            {
                var data = await _userService.GetAllUser();
                if (data != null && data.Code > 0 && data.Data != null && data.Data.Any())
                {
                    return data.Data;
                }
            }
            catch (Exception e)
            {
                Crashes.TrackError(e);
            }
            return null;
        }
        private async Task ResetPasswd(string key)
        {
            try
            {

            }
            catch (Exception e)
            {
                Crashes.TrackError(e);
            }
        }

        private async Task FunctionExecute(string key)
        {
            try
            {
                switch (key)
                {
                    case "0":
                        await DoSigup();
                        break;
                    case "1":
                        if (string.IsNullOrEmpty(PhoneNumber)) return;
                        await CheckPhone(PhoneNumber);
                        break;
                    case "2":
                        if (string.IsNullOrEmpty(UserName)) return;
                        await CheckUserName(UserName);
                        break;
                    default:
                        break;
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
                    var sigup = await _loginService.Sigup(new SigupModel
                    {
                        UserName = UserName,
                        Name = FullName,
                        NumberPhone = PhoneNumber.Replace(" ", ""),
                        Password = Passwd
                    });
                    if (sigup != null && sigup.Code > 0)
                    {
                        await _telegramService.SendMessageToTelegram(AppConstants.IdChatTelegramNoti, JsonConvert.SerializeObject(new ContentSendTelegramModel()
                        {
                            Ten_Thong_Bao = "Đăng ký tài khoản mới",
                            Ghi_Chu = new
                            {
                                Nguoi_Tao = "Tài khoản do admin tạo"
                            },
                            Id_Nguoi_Dung = UserName,
                            So_Luong = 1,
                            Noi_Dung_Thong_Bao = new
                            {
                                Tai_Khoan = UserName,
                                Trang_Thai = "Đăng ký thành công"
                            },
                        }, Formatting.Indented));
                        await _pageDialogService.DisplayAlertAsync("Thông báo", "Tạo tài khoản thành công",
                            "OK");
                    }
                    else
                    {
                        await _pageDialogService.DisplayAlertAsync("Thông báo", "Tạo tài khoản lỗi vui lòng thử lại",
                            "OK");
                        Passwd = "";
                    }
                }
                else
                {
                    await _pageDialogService.DisplayAlertAsync("Thông báo", "Vui lòng điền đầy đủ thông tin",
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
    }
}