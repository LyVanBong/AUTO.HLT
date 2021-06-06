using AUTO.HLT.MOBILE.VIP.Configurations;
using AUTO.HLT.MOBILE.VIP.Controls.ConnectFacebook;
using AUTO.HLT.MOBILE.VIP.FreeModules.Views.BuffAPost;
using AUTO.HLT.MOBILE.VIP.Models.Home;
using AUTO.HLT.MOBILE.VIP.Models.LicenseKey;
using AUTO.HLT.MOBILE.VIP.Models.Login;
using AUTO.HLT.MOBILE.VIP.Services.Database;
using AUTO.HLT.MOBILE.VIP.Services.VersionApp;
using AUTO.HLT.MOBILE.VIP.ViewModels;
using AUTO.HLT.MOBILE.VIP.Views.ChangePassword;
using AUTO.HLT.MOBILE.VIP.Views.FilterFriend;
using AUTO.HLT.MOBILE.VIP.Views.HappyBirthday;
using AUTO.HLT.MOBILE.VIP.Views.Home;
using AUTO.HLT.MOBILE.VIP.Views.Pokes;
using AUTO.HLT.MOBILE.VIP.Views.SuportCustumer;
using Microsoft.AppCenter.Crashes;
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

namespace AUTO.HLT.MOBILE.VIP.FreeModules.ViewModels.Home
{
    public class FreeHomeViewModel : ViewModelBase
    {

        private LoginModel _infoUser;
        private IDatabaseService _databaseService;
        private IPageDialogService _pageDialogService;
        private View _licenseView = new FreeView();
        private LicenseKeyModel _licenseKey;
        private bool _isLoading;
        private IDialogService _dialogService;
        private ObservableCollection<ItemMenuModel> _listItemMenus;
        private IVersionAppService _versionAppService;
        private bool _isUpdate;


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

        public FreeHomeViewModel(INavigationService navigationService, IDatabaseService databaseService, IPageDialogService pageDialogService, IDialogService dialogService, IVersionAppService versionAppService) : base(navigationService)
        {
            _versionAppService = versionAppService;
            _dialogService = dialogService;
            _pageDialogService = pageDialogService;
            _databaseService = databaseService;
            LogoutCommant = new AsyncCommand(Logout);
            UpgradeAccountCommand = new AsyncCommand<string>(UpgradeAccount);
            ConnectFacebookCommand = new AsyncCommand(ConnectFacebook);
            UseFeatureCommand = new AsyncCommand<ItemMenuModel>(UseFeature);
        }

        public override void OnResume()
        {
            base.OnResume();
            new Thread(CheckVerionApplication).Start();
        }

        private async void CheckVerionApplication()
        {
            try
            {
                if (_isUpdate) return;
                var data = await _versionAppService.CheckVersionApp();
                if (data != null && data.Code > 0 && data.Data != null)
                {
                    var version = data.Data;
                    var sotreUri = version.Note.Split(';');
                    var build = int.Parse(VersionTracking.CurrentBuild);
                    if (version.Version > build)
                    {
                        _isUpdate = true;
                        Device.BeginInvokeOnMainThread(async () =>
                        {
                            await _pageDialogService.DisplayAlertAsync("Thông báo",
                                "Đã có phiên bản mới, vui lòng cập nhật để sử dụng tính năng ổn định nhất",
                                "Cập nhật ngay");
                            if (DeviceInfo.Platform == DevicePlatform.Android)
                            {
                                await Browser.OpenAsync(sotreUri[0], BrowserLaunchMode.SystemPreferred);
                                _isUpdate = false;
                            }
                            else if (DeviceInfo.Platform == DevicePlatform.iOS)
                            {
                                await Browser.OpenAsync(sotreUri[1], BrowserLaunchMode.SystemPreferred);
                                _isUpdate = false;
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
            var para = new NavigationParameters();
            para.Add(AppConstants.AddAdmod, nameof(AppConstants.AddAdmod));
            switch (id)
            {
                case 1:
                case 2:
                    para.Add("TypeFeature", item);
                    await NavigationService.NavigateAsync(nameof(BuffAPostPage), para);
                    break;
                case 5:
                    await NavigationService.NavigateAsync(nameof(PokesPage), para);
                    break;
                case 4:
                    await NavigationService.NavigateAsync(nameof(FilterFriendPage), para);
                    break;
                case 8:
                    await NavigationService.NavigateAsync(nameof(SuportCustumerPage), para);
                    break;
                case 6:
                    await NavigationService.NavigateAsync(nameof(HappyBirthdayPage), para);
                    break;
                case 11:
                    await NavigationService.NavigateAsync(nameof(ChangePasswordPage), para);
                    break;
                default:
                    break;
            }
            IsLoading = false;
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
            var para = new DialogParameters();
            para.Add(AppConstants.AddAdmod, nameof(AppConstants.AddAdmod));
            await _dialogService.ShowDialogAsync(nameof(ConnectFacebookDialog), para);
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
                var para = new NavigationParameters();
                para.Add("UpgradeAccount", "UpgradeAccount");
                await NavigationService.NavigateAsync("/HomePage", para);
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
            InfoUser = await _databaseService.GetAccountUser();
            ListItemMenus = new ObservableCollection<ItemMenuModel>(GetItemMenu());
            IsLoading = false;
            new Thread(CheckVerionApplication).Start();
        }

        private async Task Logout()
        {
            if (IsLoading) return;
            IsLoading = true;
            if (await _pageDialogService.DisplayAlertAsync("Thông báo", "Bạn muốn đăng xuất tài khoản",
                "Ok", "Cancel"))
            {
                await _databaseService.DeleteAccontUser();
                Preferences.Clear();
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
                    TitleItem = "Tăng người theo dõi",
                    Role = 99,
                    BgColor = Color.FromHex("e45017"),
                    Icon = "icon_follow.png"
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