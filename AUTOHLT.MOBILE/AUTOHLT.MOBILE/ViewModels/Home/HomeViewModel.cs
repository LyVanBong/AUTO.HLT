using Acr.UserDialogs;
using AUTOHLT.MOBILE.Controls.Dialog.BuffService;
using AUTOHLT.MOBILE.Models.Home;
using AUTOHLT.MOBILE.Models.User;
using AUTOHLT.MOBILE.Resources.Languages;
using AUTOHLT.MOBILE.Services.Database;
using AUTOHLT.MOBILE.Services.User;
using AUTOHLT.MOBILE.Views.AddFullFriend;
using AUTOHLT.MOBILE.Views.BuffLikePage;
using AUTOHLT.MOBILE.Views.BuffSub;
using AUTOHLT.MOBILE.Views.FilterFriend;
using AUTOHLT.MOBILE.Views.SecurityFb;
using AUTOHLT.MOBILE.Views.SuportCustumer;
using AUTOHLT.MOBILE.Views.UnLockFb;
using Microsoft.AppCenter.Crashes;
using Newtonsoft.Json;
using Prism.Navigation;
using Prism.Services;
using Prism.Services.Dialogs;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using AUTOHLT.MOBILE.Services.Product;
using AUTOHLT.MOBILE.Services.Telegram;
using Xamarin.Essentials;
using Xamarin.Forms;
using Color = System.Drawing.Color;
using AUTOHLT.MOBILE.Configurations;
using Syncfusion.XForms.BadgeView;

namespace AUTOHLT.MOBILE.ViewModels.Home
{
    public class HomeViewModel : ViewModelBase
    {
        private UserModel _userModel;
        private IDatabaseService _databaseService;
        private IUserService _userService;
        private string _moneyUser;
        private IPageDialogService _pageDialogService;
        private bool _isLoading;
        private int _permission;
        private IDialogService _dialogService;
        private ObservableCollection<ServiceModel> _serviceData;
        private int _heightFreeService;
        private static bool _startNotification;
        private List<NameModel> _nameModels;
        private List<ServiceModel> _dataHome = new List<ServiceModel>
                {
                    new ServiceModel
                    {
                        Icon = "icon_like.png",
                        TitleService = Resource._1000076,
                        TypeService = 1,
                        UserRole = "2",
                    },
                    new ServiceModel
                    {
                        Icon = "icon_view.png",
                        TitleService = Resource._1000077,
                        TypeService = 2,
                        UserRole = "2",
                    },
                    new ServiceModel
                    {
                        Icon = "icon_auto_boot_hear.png",
                        TitleService = Resource._1000105,
                        TypeService = 3,
                        BadgeView = "Hot",
                        UserRole = "2",
                        BadgeType = BadgeType.Error
                    },
                    new ServiceModel
                    {
                        Icon = "icon_add_friends.png",
                        TitleService = Resource._1000078,
                        TypeService = 4,
                        UserRole = "2",
                    },
                    new ServiceModel
                    {
                        Icon = "icon_follow.png",
                        TitleService = Resource._1000079,
                        TypeService = 5,
                        UserRole = "2",
                    },
                    new ServiceModel
                    {
                        Icon = "icon_like_page.png",
                        TitleService = Resource._1000080,
                        TypeService = 6,
                        UserRole = "2",
                    },
                    new ServiceModel
                    {
                        Icon = "icon_unlock.png",
                        TitleService = Resource._1000081,
                        TypeService = 7,
                        UserRole = "2",
                    },
                    new ServiceModel
                    {
                        Icon = "icon_security_fb.png",
                        TitleService = Resource._1000082,
                        TypeService = 8,
                        UserRole = "2",
                    },
                    new ServiceModel
                    {
                        Icon = "icon_filter_friends.png",
                        TitleService = Resource._1000083,
                        TypeService = -1,
                        BadgeView = "Free",
                        UserRole = "2",
                        BadgeType = BadgeType.Warning
                    },
                    new ServiceModel
                    {
                        Icon = "icon_customer_support.png",
                        TitleService = Resource._1000084,
                        TypeService = 0,
                        BadgeView = "Support",
                        UserRole = "2",
                    },
                    new ServiceModel
                    {
                        Icon = "icon_revenue.png",
                        TitleService = Resource._1000022,
                        TypeService = 9,
                        UserRole = "0",
                    },
                    new ServiceModel
                    {
                        Icon = "icon_settings.png",
                        TitleService = Resource._1000023,
                        TypeService = 10,
                        UserRole = "0",
                    },
                    new ServiceModel
                    {
                        Icon = "icon_top_up.png",
                        TitleService = Resource._1000024,
                        TypeService = 11,
                        UserRole = "0",
                    },
                    new ServiceModel
                    {
                        Icon = "icon_service.png",
                        TitleService = Resource._1000025,
                        TypeService = 12,
                        UserRole = "0",
                    },
                    new ServiceModel
                    {
                        Icon = "icon_work.png",
                        TitleService = Resource._1000027,
                        TypeService = 13,
                        UserRole = "1",
                        BadgeView = "Employees"
                    },
                    new ServiceModel
                    {
                        Icon = "icon_history.png",
                        TitleService = Resource._1000034,
                        TypeService = 14,
                        UserRole = "0",
                    },
                };

        private Dictionary<int, string> ServiceFunctions = new Dictionary<int, string>
        {
            {-1,nameof(FilterFriendPage) },
            {0,nameof(SuportCustumerPage) },
            {1,"BuffLikePage" },
            {2,"BuffEyesViewPage" },
            {3,"InteractivePage" },
            {4,nameof(AddFullFriendPage) },
            {5,nameof(BuffSubPage) },
            {6,nameof(BuffLikePagePage) },
            {7,nameof(UnLockFbPage) },
            {8,nameof(SecurityFbPage) },
            {9,"" },
            {10,"" },
            {11,"RechargeCustomersPage" },
            {12,"" },
            {13,"" },
            {14,"" },
            {15,"TransferPage" },
            {16,"ChangePasswordPage" },
            {17,"" },
            {18,"AccountInformationPage" },
        };
        private string[] _paraDialogSheet = new string[]
            {"1000 bạn bè", "2000 bạn bè", "3000 bạn bè", "4000 bạn bè", "5000 bạn bè"};
        private IProductService _productService;
        private string _idProductSecurityFb = "ae1274f0-a779-4601-b59f-8bf9b3e5cdf7";
        private ITelegramService _telegramService;
        private BadgeType _badgeType;
        private string _idProductAddFriends = "82ea2831-60b3-4cf4-9828-be58c1c51a62";

        public BadgeType BadgeType
        {
            get => _badgeType;
            set => SetProperty(ref _badgeType, value);
        }

        public BadgeType BadgeType
        {
            get => _badgeType;
            set => SetProperty(ref _badgeType, value);
        }

        public int HeightFreeService
        {
            get => _heightFreeService;
            set => SetProperty(ref _heightFreeService, value);
        }
        public ObservableCollection<ServiceModel> ServiceData
        {
            get => _serviceData;
            set => SetProperty(ref _serviceData, value);
        }

        public ICommand NavigationCommand { get; private set; }
        public ICommand BuffServiceCommand { get; private set; }
        public int Permission
        {
            get => _permission;
            set => SetProperty(ref _permission, value);
        }

        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }

        public string MoneyUser
        {
            get => _moneyUser;
            set => SetProperty(ref _moneyUser, value);
        }

        public UserModel UserModel
        {
            get => _userModel;
            set => SetProperty(ref _userModel, value);
        }

        public ICommand LogoutCommand { get; private set; }
        public HomeViewModel(INavigationService navigationService, IDatabaseService databaseService, IUserService userService, IPageDialogService pageDialogService, IDialogService dialogService, IProductService productService, ITelegramService telegramService) : base(navigationService)
        {
            _telegramService = telegramService;
            _productService = productService;
            _dialogService = dialogService;
            _pageDialogService = pageDialogService;
            _userService = userService;
            _databaseService = databaseService;
            LogoutCommand = new Command(LogoutAccount);
            BuffServiceCommand = new Command<string>(BuffService);
            NavigationCommand = new Command<Object>(NavigationPageService);
        }

        private async void NavigationPageService(Object obj)
        {
            try
            {
                if (IsLoading) return;
                IsLoading = true;
                var key = -9999;
                var data = obj is ServiceModel;
                var service = new ServiceModel();
                if (data)
                {
                    service = obj as ServiceModel;
                    key = service.TypeService;
                }
                else
                {
                    var para = obj as string;
                    if (para != null)
                        key = int.Parse(para);
                }

                if (key == 4)
                {
                    await AddFriends();
                }
                else if (key == 8)
                {
                    await ServiceSecurityFacebook();
                }
                else if (key == 5 || key == 6)
                {
                    BuffService(key + "");
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

        private async Task AddFriends()
        {
            var result =
                await _pageDialogService.DisplayActionSheetAsync("Số lượng bạn muỗn tăng !", "Cancel", null, _paraDialogSheet);
            var name = "";
            var prices = 0;
            if (result == _paraDialogSheet[0])
            {
                name = _paraDialogSheet[0];
                prices = 1;
            }
            else if (result == _paraDialogSheet[1])
            {
                name = _paraDialogSheet[1];
                prices = 2;
            }
            else if (result == _paraDialogSheet[2])
            {
                name = _paraDialogSheet[2];
                prices = 3;
            }
            else if (result == _paraDialogSheet[3])
            {
                name = _paraDialogSheet[3];
                prices = 4;
            }
            else if (result == _paraDialogSheet[4])
            {
                name = _paraDialogSheet[4];
                prices = 5;
            }
            else
            {
                name = "";
                prices = -1;
            }
            if (prices>0)
            {
                var user = await _databaseService.GetAccountUser();
                if (user != null)
                {
                    var product = await _productService.GetAllProduct();
                    if (product != null && product.Code > 0 && product.Data != null && product.Data.Any())
                    {
                        var addFriends = product.Data.FirstOrDefault(x => x.ID == _idProductAddFriends);
                        if (addFriends != null && addFriends.DateCreate != null)
                        {
                            var pr = long.Parse(addFriends.Price) * prices;
                            var regis = await _pageDialogService.DisplayAlertAsync(Resource._1000021,
                                $"Bạn đăng ký dịch vụ tăng {name} với giá {string.Format(new CultureInfo("en-US"), "{0:0,0}", pr)} VND",
                                "OK", "Cancel");
                            if (regis)
                            {
                                var myMoney = await _userService.GetMoneyUser(user.UserName);
                                if (myMoney != null && myMoney.Code > 0 && myMoney.Data != null)
                                {
                                    var money = long.Parse(myMoney.Data.Replace(".0000", ""));
                                    if (money >= pr)
                                    {
                                        var addServiceForUser =
                                            await _productService.RegisterProduct(_idProductAddFriends, user.ID);
                                        if (addServiceForUser != null && addServiceForUser.Code > 0)
                                        {
                                            await _pageDialogService.DisplayAlertAsync(Resource._1000035,
                                                "Đăng kỹ dịch vụ thành công, chúng tôi sẽ liên lạc với bạn để hỗ trợ sử dụng dịch vụ !",
                                                "OK");

                                            await _userService.SetMoneyUser(user.UserName, money - pr + "");
                                            var addHistoryUse = await _productService.AddHistoryUseService(_idProductSecurityFb,
                                                $"Tăng {name}", user.ID, "1", DateTime.Now.ToString("yyy/MM/dd hh:mm:ss"));
                                            var message = $"Tăng bạn bè\n" +
                                                          $"Nội dung: Khách hàng đăng ký gói dịch tăng {name} facebook cần liên hệ với khách để thực hiện dịch vụ cho khách\n" +
                                                          $"Id người dùng dịch vụ: {user.ID}\n" +
                                                          $"Số điện thoại: {user.NumberPhone}\n" +
                                                          $"Tên: {user.Name}\n" +
                                                          $"Thời yêu cầu dịch vụ: {DateTime.Now.ToString("F")}";
                                            var send = await _telegramService.SendMessageToTelegram(AppConstants.IdChatWork,
                                                message);
                                            var dataMoneyUser = await _userService.GetMoneyUser(UserModel.UserName);
                                            if (dataMoneyUser != null)
                                            {
                                                if (dataMoneyUser.Code > 0)
                                                {
                                                    MoneyUser = dataMoneyUser.Data.Replace(".0000", "");
                                                }
                                                else
                                                {
                                                    MoneyUser = "0";
                                                }
                                            }
                                        }
                                        else
                                        {
                                            await _pageDialogService.DisplayAlertAsync(Resource._1000035, Resource._1000041, "OK");
                                        }
                                    }
                                    else
                                    {
                                        await _pageDialogService.DisplayAlertAsync(Resource._1000021,
                                            "Số dư hiện tại của bạn không đủ, vui lòng nạp thêm tiền để sử dụng dịch vụ !",
                                            "OK");
                                    }
                                }
                                else
                                {
                                    await _pageDialogService.DisplayAlertAsync(Resource._1000021,
                                        "Số dư hiện tại của bạn không đủ, vui lòng nạp thêm tiền để sử dụng dịch vụ !",
                                        "OK");
                                }
                            }
                        }
                    }
                }
            }
        }

        private async Task ServiceSecurityFacebook()
        {
            var user = await _databaseService.GetAccountUser();
            if (user != null)
            {
                var product = await _productService.GetAllProduct();
                if (product != null && product.Code > 0 && product.Data != null && product.Data.Any())
                {
                    var securityFB = product.Data.FirstOrDefault(x => x.ID == _idProductSecurityFb);
                    if (securityFB != null)
                    {
                        var regis = await _pageDialogService.DisplayAlertAsync(Resource._1000021,
                            $"Bạn đăng ký gói bảo mật facebook 1 năm với giá {string.Format(new CultureInfo("en-US"), "{0:0,0}", long.Parse(securityFB.Price))} VND",
                            "OK", "Cancel");
                        if (regis)
                        {
                            var myMoney = await _userService.GetMoneyUser(user.UserName);
                            if (myMoney != null && myMoney.Code > 0 && myMoney.Data != null)
                            {
                                var money = long.Parse(myMoney.Data.Replace(".0000", ""));
                                var price = long.Parse(securityFB.Price);
                                if (money >= price)
                                {
                                    var addServiceForUser =
                                        await _productService.RegisterProduct(_idProductSecurityFb, user.ID);
                                    if (addServiceForUser != null && addServiceForUser.Code > 0)
                                    {
                                        await _pageDialogService.DisplayAlertAsync(Resource._1000035,
                                            "Đăng kỹ dịch vụ thành công, chúng tôi sẽ liên lạc với bạn để hỗ trợ sử dụng dịch vụ !",
                                            "OK");

                                        await _userService.SetMoneyUser(user.UserName, money - price + "");
                                        var addHistoryUse = await _productService.AddHistoryUseService(_idProductSecurityFb, "Bao mat facebook", user.ID, "1", DateTime.Now.ToString("yyy/MM/dd hh:mm:ss"));
                                        var message = $"Bảo mật facebook\n" +
                                                      $"Nội dung: Khách hàng đăng ký gói dịch bảo mật facebook cần liên hệ với khách để thực hiện dịch vụ cho khách\n" +
                                                      $"Id người dùng dịch vụ: {user.ID}\n" +
                                                      $"Số điện thoại: {user.NumberPhone}\n" +
                                                      $"Tên: {user.Name}\n" +
                                                      $"Thời yêu cầu dịch vụ: {DateTime.Now.ToString("F")}";
                                        var send = await _telegramService.SendMessageToTelegram(AppConstants.IdChatWork,
                                            message);
                                        var dataMoneyUser = await _userService.GetMoneyUser(UserModel.UserName);
                                        if (dataMoneyUser != null)
                                        {
                                            if (dataMoneyUser.Code > 0)
                                            {
                                                MoneyUser = dataMoneyUser.Data.Replace(".0000", "");
                                            }
                                            else
                                            {
                                                MoneyUser = "0";
                                            }
                                        }
                                    }
                                    else
                                    {
                                        await _pageDialogService.DisplayAlertAsync(Resource._1000035, Resource._1000041, "OK");
                                    }
                                }
                                else
                                {
                                    await _pageDialogService.DisplayAlertAsync(Resource._1000021,
                                        "Số dư hiện tại của bạn không đủ, vui lòng nạp thêm tiền để sử dụng dịch vụ !",
                                        "OK");
                                }
                            }
                            else
                            {
                                await _pageDialogService.DisplayAlertAsync(Resource._1000021,
                                    "Số dư hiện tại của bạn không đủ, vui lòng nạp thêm tiền để sử dụng dịch vụ !",
                                    "OK");
                            }
                        }
                    }
                }
            }
        }

        private async void BuffService(string key)
        {
            ;
            IsLoading = true;
            var para = new DialogParameters();
            para.Add("UserName", UserModel.UserName);
            switch (key)
            {
                case "5":
                    para.Add("ServiceName", Resource._1000079);
                    para.Add("Service", "sub");
                    para.Add("IdProduct", "1ad3c424-5333-46b0-a0e8-5c31a6dbb161");
                    break;
                case "6":
                    para.Add("ServiceName", Resource._1000080);
                    para.Add("Service", "like");
                    para.Add("IdProduct", "82ea2831-60b3-4cf4-9828-be58c1c51a62");
                    break;
                //case "8":
                //    para.Add("ServiceName", Resource._1000082);
                //    para.Add("IdProduct", "ae1274f0-a779-4601-b59f-8bf9b3e5cdf7");
                //    break;
                default:
                    break;
            }

            await _dialogService.ShowDialogAsync(nameof(BuffDialog), para);
        }

        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            await InitializeDataHome();
        }
        /// <summary>
        /// hàm khởi tạo dữ liệu ban đầu cho trang home
        /// </summary>
        /// <returns></returns>
        private async Task InitializeDataHome()
        {
            try
            {
                IsLoading = true;
                var data = await _databaseService.GetAccountUser();
                if (data != null)
                {
                    UserModel = data;
                    Permission = int.Parse(data.Role);
                }
                var money = await _userService.GetMoneyUser(UserModel.UserName);
                if (money != null)
                {
                    if (money.Code > 0)
                    {
                        MoneyUser = money.Data.Replace(".0000", "");
                    }
                    else
                    {
                        MoneyUser = "0";
                    }
                }
                if (ServiceData != null && ServiceData.Any()) return;
                ServiceData = new ObservableCollection<ServiceModel>();
                foreach (var item in _dataHome)
                {
                    await Task.Delay(TimeSpan.FromMilliseconds(200));
                    if (Permission == 2)
                    {
                        if (item.UserRole == "2")
                        {
                            ServiceData.Add(item);
                        }
                    }
                    if (Permission == 0)
                    {
                        ServiceData.Add(item);
                    }
                }
            }
            catch (Exception e)
            {
                Crashes.TrackError(e);
            }
            finally
            {
                new Thread(() =>
               {
                   NoticeServiceSubscribers();
               }).Start();
                IsLoading = false;
            }
        }
        /// <summary>
        /// gọi service lấy tên người viết nam
        /// </summary>
        /// <returns></returns>
        private async Task<List<NameModel>> GetDataname()
        {
            try
            {
                string _urlName = "https://raw.githubusercontent.com/duyet/vietnamese-namedb/master/uit_member.json";
                var client = new RestClient(new Uri(_urlName));
                client.Timeout = -1;
                var request = new RestRequest(Method.GET);
                var response = await client.ExecuteAsync(request);
                var data = JsonConvert.DeserializeObject<List<NameModel>>(response.Content);
                return data;
            }
            catch (Exception e)
            {
                Crashes.TrackError(e);
            }

            return null;
        }
        /// <summary>
        /// Gửi thông báo đang ký mỗi khách hàng
        /// </summary>
        /// <returns></returns>
        private async void NoticeServiceSubscribers()
        {
            try
            {
                if (_startNotification) return;
                _startNotification = true;
                _nameModels = _nameModels == null ? await GetDataname() : _nameModels;
                if (_nameModels != null && _nameModels.Any())
                {
                    var random = new Random();

                    Device.StartTimer(TimeSpan.FromSeconds(15), () =>
                    {
                        var name = _nameModels[random.Next(_nameModels.Count - 1)];
                        var number = string.Format(new CultureInfo("en-US"), "{0:0,0}",
                            decimal.Parse(random.Next(1000, 6000000) + ""));
                        var cmt = new string[]
                        {
                            " đăng ký gói tăng like 1 năm",
                            " đăng ký gói tăng like vính viễn",
                            " đăng ký gói tăng mắt xem livestream 1 năm",
                            " đăng ký gói tăng mắt xem livestream vính viễn",
                            " sử dụng dịch vụ thêm full 5k bạn bè",
                            $" Sử dụng dich vụ tăng { number} theo dõi trang cá nhân",
                            $" đăng ký dịch vụ tăng {number} like page",
                            " đã sử dụng dịch vụ mở khoán fb thành công",
                            " báo mật fb cá nhân",

                        };
                        var messager = $"{name.full_name} {cmt[random.Next(cmt.Length - 1)]}";
                        UserDialogs.Instance.Toast(new ToastConfig(messager)
                        {

                            BackgroundColor = Color.WhiteSmoke,
                            Message = messager,
                            Position = ToastPosition.Bottom,
                            MessageTextColor = Color.Red,
                        });
                        return _startNotification;
                    });
                }
            }
            catch (Exception e)
            {
                Crashes.TrackError(e);
            }
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