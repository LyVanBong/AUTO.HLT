using AUTOHLT.MOBILE.Configurations;
using AUTOHLT.MOBILE.Controls.Dialog.BuffService;
using AUTOHLT.MOBILE.Models.Home;
using AUTOHLT.MOBILE.Models.Telegram;
using AUTOHLT.MOBILE.Models.User;
using AUTOHLT.MOBILE.Resources.Fonts;
using AUTOHLT.MOBILE.Resources.Languages;
using AUTOHLT.MOBILE.Services.Database;
using AUTOHLT.MOBILE.Services.Guide;
using AUTOHLT.MOBILE.Services.Product;
using AUTOHLT.MOBILE.Services.Telegram;
using AUTOHLT.MOBILE.Services.User;
using AUTOHLT.MOBILE.Views.AccountInformation;
using AUTOHLT.MOBILE.Views.ChangePassword;
using AUTOHLT.MOBILE.Views.FilterFriend;
using AUTOHLT.MOBILE.Views.HappyBirthday;
using AUTOHLT.MOBILE.Views.Pokes;
using AUTOHLT.MOBILE.Views.SuportCustumer;
using AUTOHLT.MOBILE.Views.TopUp;
using Microsoft.AppCenter.Crashes;
using Newtonsoft.Json;
using Prism.Navigation;
using Prism.Services;
using Prism.Services.Dialogs;
using RestSharp;
using Syncfusion.XForms.BadgeView;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

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
                    }, new ServiceModel
                    {
                        Icon = "icon_add_friend.png",
                        TitleService = "Kết bạn facebook để trao đổi thông tin giao lưu và tìm kiếm các khách hàng tiềm năng trong công việc kinh doanh online trên facebook hoặc các sàn thương mại điện tử.",
                        TypeService = 6,
                        UserRole = "2",
                        BadgeView = "Hot",
                        BadgeType = BadgeType.Error
                    },
                    new ServiceModel
                    {
                        Icon = "icon_connection.png",
                        TitleService = "Kết nỗi facebook",
                        TypeService = 7,
                        UserRole = "2",
                        BadgeView = "Connect",
                        BadgeType = BadgeType.Primary
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
            {7,nameof(FilterFriendPage) },
        };
        private string[] _paraDialogSheet = new string[]
            {"1000 bạn bè", "2000 bạn bè", "3000 bạn bè", "4000 bạn bè", "5000 bạn bè","Xem hướng dẫn"};
        private IProductService _productService;
        private string _idProductSecurityFb = "ae1274f0-a779-4601-b59f-8bf9b3e5cdf7";
        private ITelegramService _telegramService;
        private BadgeType _badgeType;
        private string _idProductAddFriends = "85e7192b-7a30-45ff-b327-bd9c25c8dfcb";
        private string _dataMyMoney;
        private string _iconShowMoney;
        private IGuideService _guideService;
        
        public ICommand ShowMyMoneyCommand { get; private set; }
        public string IconShowMoney
        {
            get => _iconShowMoney;
            set => SetProperty(ref _iconShowMoney, value);
        }

        public ICommand TopUpMoneyCommand { get; private set; }
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
        public HomeViewModel(INavigationService navigationService, IDatabaseService databaseService, IUserService userService, IPageDialogService pageDialogService, IDialogService dialogService, IProductService productService, ITelegramService telegramService, IGuideService guideService) : base(navigationService)
        {
            _guideService = guideService;
            _telegramService = telegramService;
            _productService = productService;
            _dialogService = dialogService;
            _pageDialogService = pageDialogService;
            _userService = userService;
            _databaseService = databaseService;
            LogoutCommand = new Command(LogoutAccount);
            BuffServiceCommand = new Command<string>(BuffService);
            NavigationCommand = new Command<Object>(NavigationPageService);
            TopUpMoneyCommand = new Command(TopUpMoney);
            ShowMyMoneyCommand = new Command(ShowMyMoney);
        }

        private void ShowMyMoney()
        {
            try
            {
                if (Preferences.Get("IsShowMoney", false))
                {
                    Preferences.Set("IsShowMoney", false);
                    IconShowMoney = FontAwesome5DuotoneSolid.EyeSlashSecondary;
                }
                else
                {
                    Preferences.Set("IsShowMoney", true);
                    IconShowMoney = FontAwesome5DuotoneSolid.EyeSecondary;
                }
                FormatMoney();
            }
            catch (Exception e)
            {
                Crashes.TrackError(e);
            }
        }

        private void TopUpMoney()
        {
            try
            {
                if (IsLoading) return;
                IsLoading = true;

                _dialogService.ShowDialog(nameof(TopUpDialog));
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
                await NavigationService.NavigateAsync(ServiceFunctions[key]);
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
            else if (result == _paraDialogSheet[5])
            {
                try
                {
                    var data = await _guideService.GetGuide(9);
                    await Browser.OpenAsync(data?.Url);
                }
                catch (Exception ex)
                {
                    Crashes.TrackError(ex);
                }
                return;
            }
            else
            {
                name = "";
                prices = -1;
            }
            if (prices > 0)
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
                                            var content = JsonConvert.SerializeObject(new MessageNotificationTelegramModel
                                            {
                                                Ten_Thong_Bao = "Tăng bạn bè",
                                                So_Luong = 1,
                                                Id_Nguoi_Dung = user?.ID,
                                                Noi_Dung_Thong_Bao = new
                                                {
                                                    Noi_Dung = $"Khách hàng đăng ký gói dịch tăng {name} facebook cần liên hệ với khách để thực hiện dịch vụ cho khách",
                                                },
                                                Ghi_Chu = new
                                                {
                                                    Ten = user?.Name,
                                                    Tai_Khoan = user?.UserName,
                                                    So_dien_thoai = user?.NumberPhone
                                                }
                                            }, Formatting.Indented);
                                            var send = await _telegramService.SendMessageToTelegram(AppConstants.IdChatWork,
                                                content);
                                            var dataMoneyUser = await _userService.GetMoneyUser(UserModel.UserName);
                                            if (dataMoneyUser != null)
                                            {
                                                if (dataMoneyUser.Code > 0)
                                                {
                                                    _dataMyMoney = dataMoneyUser.Data.Replace(".0000", "");
                                                }
                                                else
                                                {
                                                    _dataMyMoney = "0";
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
                            "Đăng ký", "Xem hướng dẫn");
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
                                        var content = JsonConvert.SerializeObject(new MessageNotificationTelegramModel
                                        {
                                            Ten_Thong_Bao = "Bảo mật facebook",
                                            So_Luong = 1,
                                            Id_Nguoi_Dung = user?.ID,
                                            Noi_Dung_Thong_Bao = new
                                            {
                                                Noi_Dung = "Khách hàng đăng ký gói dịch bảo mật facebook cần liên hệ với khách để thực hiện dịch vụ cho khách",
                                            },
                                            Ghi_Chu = new
                                            {
                                                Ten = user?.Name,
                                                Tai_Khoan = user?.UserName,
                                                So_dien_thoai = user?.NumberPhone
                                            }
                                        }, Formatting.Indented);
                                        var send = await _telegramService.SendMessageToTelegram(AppConstants.IdChatWork,
                                            content);
                                        var dataMoneyUser = await _userService.GetMoneyUser(UserModel.UserName);
                                        if (dataMoneyUser != null)
                                        {
                                            if (dataMoneyUser.Code > 0)
                                            {
                                                _dataMyMoney = dataMoneyUser.Data.Replace(".0000", "");
                                            }
                                            else
                                            {
                                                _dataMyMoney = "0";
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
                        else
                        {
                            try
                            {
                                var data = await _guideService.GetGuide(3);
                                await Browser.OpenAsync(data?.Url);
                            }
                            catch (Exception ex)
                            {
                                Crashes.TrackError(ex);
                            }
                            return;
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
            FormatMoney();
        }

        private void FormatMoney()
        {
            try
            {
                if (!Preferences.ContainsKey("IsShowMoney"))
                {
                    Preferences.Set("IsShowMoney", false);
                    IconShowMoney = FontAwesome5DuotoneSolid.EyeSlashSecondary;
                    var moneyHiden = string.Format(new CultureInfo("en-US"), "{0:0,0}", decimal.Parse(_dataMyMoney)).ToCharArray();
                    var str = "";
                    foreach (var item in moneyHiden)
                    {
                        if (item == ',')
                        {
                            str += " ";
                        }
                        else
                        {
                            str += "*";
                        }

                        MoneyUser = str;
                    }
                }
                else
                {
                    if (Preferences.Get("IsShowMoney", false))
                    {
                        IconShowMoney = FontAwesome5DuotoneSolid.EyeSecondary;
                        MoneyUser = _dataMyMoney;
                    }
                    else
                    {
                        IconShowMoney = FontAwesome5DuotoneSolid.EyeSlashSecondary;
                        var moneyHiden = string.Format(new CultureInfo("en-US"), "{0:0,0}", decimal.Parse(_dataMyMoney))
                            .ToCharArray();
                        var str = "";
                        foreach (var item in moneyHiden)
                        {
                            if (item == ',')
                            {
                                str += " ";
                            }
                            else
                            {
                                str += "*";
                            }

                            MoneyUser = str;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Crashes.TrackError(e);
            }
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
                    var money = await _userService.GetMoneyUser(UserModel.UserName);
                    if (money != null)
                    {
                        if (money.Code > 0)
                        {
                            _dataMyMoney = money.Data.Replace(".0000", "");
                        }
                        else
                        {
                            _dataMyMoney = "0";
                        }
                    }
                    if (ServiceData != null && ServiceData.Any()) return;
                    
                    ServiceData = new ObservableCollection<ServiceModel>(_dataHome);
                }
            }
            catch (Exception e)
            {
                Crashes.TrackError(e);
            }
            finally
            {
               // new Thread(() =>
               //{
               //    NoticeServiceSubscribers();
               //}).Start();
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