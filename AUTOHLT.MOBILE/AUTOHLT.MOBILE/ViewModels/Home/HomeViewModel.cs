﻿using AUTOHLT.MOBILE.Models.Home;
using AUTOHLT.MOBILE.Models.User;
using AUTOHLT.MOBILE.Resources.Languages;
using AUTOHLT.MOBILE.Services.Database;
using AUTOHLT.MOBILE.Services.User;
using AUTOHLT.MOBILE.Views.AccountInformation;
using AUTOHLT.MOBILE.Views.ChangePassword;
using AUTOHLT.MOBILE.Views.FilterFriend;
using AUTOHLT.MOBILE.Views.HappyBirthday;
using AUTOHLT.MOBILE.Views.Pokes;
using AUTOHLT.MOBILE.Views.SuportCustumer;
using Microsoft.AppCenter.Crashes;
using Prism.Navigation;
using Prism.Services;
using Prism.Services.Dialogs;
using Syncfusion.XForms.BadgeView;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using AUTOHLT.MOBILE.Controls.Dialog.ConnectFacebook;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace AUTOHLT.MOBILE.ViewModels.Home
{
    public class HomeViewModel : ViewModelBase
    {
        private UserModel _userModel;
        private IDatabaseService _databaseService;
        private IPageDialogService _pageDialogService;
        private bool _isLoading;
        private IDialogService _dialogService;
        private ObservableCollection<ServiceModel> _serviceData;
        private List<ServiceModel> _dataHome = new List<ServiceModel>
                {
                    new ServiceModel
                    {
                        Icon = "icon_pokes.png",
                        TitleService = "Chọc trên facebook có thể hiểu là cách để gây chú ý để người mình chọc để ý tới mình, ví dụ có thể bạn bè lâu ko nói chuyện chọc qua chọc lại cho biết.",
                        TypeService = 3,
                        UserRole = "2",
                        BadgeView = "Funny",
                        BadgeType = BadgeType.Warning
                    },
                    new ServiceModel
                    {
                        Icon = "icon_filter_friends.png",
                        TitleService = "Lọc bạn bè không tương tác hoặc có tương tác với nick Facebook của bạn nhằm các mục đích sau: Xoá đi những bạn bè không tương tác, dành chỗ kết bạn cho những người bạn mới. Cải thiện lượng tương tác trên tài khoản của bạn. Giúp công việc kinh doanh online, quảng bá sản phẩm được hiệu quả hơn",
                        TypeService = 4,
                        UserRole = "2",
                        BadgeView = "New",
                        BadgeType = BadgeType.Info
                    },
                    new ServiceModel
                    {
                        Icon = "icon_birthday.png",
                        TitleService = "Chúc mừng sinh nhật - Facebook là nơi mọi người kết nối với bạn bè, người thân, khách hàng cùng như các đối tác chia sẻ sở thích chung và những câu chuyện đời thường, cũng như kỷ niệm những khoảnh khắc quan trọng như ngày sinh nhật.",
                        TypeService = 5,
                        UserRole = "2",
                        BadgeView = "Free",
                        BadgeType = BadgeType.Success
                    }, 
                    //new ServiceModel
                    //{
                    //    Icon = "icon_add_friend.png",
                    //    TitleService = "Kết bạn facebook để trao đổi thông tin giao lưu và tìm kiếm các khách hàng tiềm năng trong công việc kinh doanh online trên facebook hoặc các sàn thương mại điện tử.",
                    //    TypeService = 6,
                    //    UserRole = "2",
                    //    BadgeView = "Hot",
                    //    BadgeType = BadgeType.Error
                    //},
                    new ServiceModel
                    {
                        Icon = "icon_connection.png",
                        TitleService = "Kết nỗi facebook",
                        TypeService = 7,
                        UserRole = "2",
                        BadgeView = "Connect",
                        BadgeType = BadgeType.Primary
                    },
                    new ServiceModel
                    {
                        Icon = "icon_auto_vip.png",
                        TitleService = "AUTO VIP là công cụ tăng tương tác không giới hạn\nTại sao nên chọn AUTO VIP\nĐơn giản dễ sử dụngSử dụng đơn giản, dễ dàng cài đặt và vận hành nhanh chóng chỉ với vài chạm.\nSupport 24/7Luôn luôn lắng nghe và cải thiện giải pháp không ngừng nhằm phục vụ tốt nhất",
                        TypeService = 8,
                        UserRole = "2",
                        BadgeView = "AUTOVIP",
                        BadgeType = BadgeType.Error
                    },
                };

        private Dictionary<int, string> ServiceFunctions = new Dictionary<int, string>
        {
            {0,nameof(AccountInformationPage) },
            {1,nameof(ChangePasswordPage) },
            {2,nameof(SuportCustumerPage) },
            {3,nameof(PokesPage) },
            {4,nameof(FilterFriendPage) },
            {5,nameof(HappyBirthdayPage) },
            {6,nameof(FilterFriendPage) },
        };

        public ObservableCollection<ServiceModel> ServiceData
        {
            get => _serviceData;
            set => SetProperty(ref _serviceData, value);
        }

        public ICommand NavigationCommand { get; private set; }

        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }

        public UserModel UserModel
        {
            get => _userModel;
            set => SetProperty(ref _userModel, value);
        }

        public ICommand LogoutCommand { get; private set; }
        public HomeViewModel(INavigationService navigationService, IDatabaseService databaseService, IUserService userService, IPageDialogService pageDialogService, IDialogService dialogService) : base(navigationService)
        {
            _dialogService = dialogService;
            _pageDialogService = pageDialogService;
            _databaseService = databaseService;
            LogoutCommand = new Command(LogoutAccount);
            NavigationCommand = new Command<Object>(NavigationPageService);
        }
        private async void NavigationPageService(Object obj)
        {
            try
            {
                if (IsLoading) return;
                IsLoading = true;
                var key = -1;
                var data = obj is ServiceModel;
                if (data)
                {
                    var service = obj as ServiceModel;
                    key = service.TypeService;
                }
                else
                {
                    var para = obj as string;
                    if (para != null)
                        key = int.Parse(para);
                }

                if (key == 8)
                {
                    var urlAppInStore = Device.RuntimePlatform == Device.Android ? @"https://play.google.com/store/apps/details?id=com.bsoftgroup.auto.vip" : @"https://apps.apple.com/vn/app/autovip/id1557805046";
                    await Launcher.TryOpenAsync(urlAppInStore);
                }
                else if (key == 7)
                {
                    await _dialogService.ShowDialogAsync(nameof(ConnectFacebookDialog));
                }
                else
                {
                    await NavigationService.NavigateAsync(ServiceFunctions[key]);
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
            if (ServiceData == null || !ServiceData.Any())
            {
                var data = await _databaseService.GetAccountUser();
                if (data != null)
                {
                    UserModel = data;
                }
                ServiceData = new ObservableCollection<ServiceModel>(_dataHome);
            }
            IsLoading = false;
        }

        /// <summary>
        /// đăng xuất khỏi tài khoản
        /// </summary>
        private async void LogoutAccount()
        {
            if (IsLoading) return;
            IsLoading = true;
            try
            {
                var res = await _pageDialogService.DisplayAlertAsync(Resource._1000035, Resource._1000042, "OK", "Cancel");
                if (res)
                {
                    Preferences.Clear();
                    await _databaseService.DeleteAccontUser();
                    ServiceData.Clear();
                    await NavigationService.NavigateAsync("/LoginPage");
                }
            }
            catch (Exception e)
            {
                Crashes.TrackError(e);
            }
            IsLoading = false;
        }
    }
}