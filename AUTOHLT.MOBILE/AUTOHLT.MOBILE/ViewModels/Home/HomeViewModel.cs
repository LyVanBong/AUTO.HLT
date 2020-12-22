using AUTOHLT.MOBILE.Controls.Dialog.BuffService;
using AUTOHLT.MOBILE.Models.Home;
using AUTOHLT.MOBILE.Models.User;
using AUTOHLT.MOBILE.Resources.Languages;
using AUTOHLT.MOBILE.Services.Database;
using AUTOHLT.MOBILE.Services.User;
using Microsoft.AppCenter.Crashes;
using Prism.Navigation;
using Prism.Services;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Acr.UserDialogs;
using AUTOHLT.MOBILE.Configurations;
using Newtonsoft.Json;
using RestSharp;
using Xamarin.Essentials;
using Xamarin.Forms;
using Color = System.Drawing.Color;

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
        private int _heightPaidService;
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
                        Icon = "icon_Interactive.png",
                        TitleService = Resource._1000026,
                        TypeService = 3,
                        BadgeView = "Coming",
                        UserRole = "2",
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
                        TitleService = Resource._1000026,
                        TypeService = 13,
                        UserRole = "1",
                        BadgeView = "Employees"
                    },
                    new ServiceModel
                    {
                        Icon = "icon_history.png",
                        TitleService = Resource._1000027,
                        TypeService = 14,
                        UserRole = "0",
                    },
                };

        private Dictionary<int, string> ServiceFunctions = new Dictionary<int, string>
        {
            {-1,"" },
            {0,"" },
            {1,"BuffLikePage" },
            {2,"BuffEyesViewPage" },
            {3,"InteractivePage" },
            {4,"" },
            {5,"" },
            {6,"" },
            {7,"" },
            {8,"" },
            {9,"" },
            {10,"" },
            {11,"" },
            {12,"" },
            {13,"" },
            {14,"" },
            {15,"TransferPage" },
            {16,"ChangePasswordPage" },
            {17,"" },
            {18,"AccountInformationPage" },
        };

        public int HeightFreeService
        {
            get => _heightFreeService;
            set => SetProperty(ref _heightFreeService, value);
        }

        public int HeightPaidService
        {
            get => _heightPaidService;
            set => SetProperty(ref _heightPaidService, value);
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
        public HomeViewModel(INavigationService navigationService, IDatabaseService databaseService, IUserService userService, IPageDialogService pageDialogService, IDialogService dialogService) : base(navigationService)
        {
            _dialogService = dialogService;
            _pageDialogService = pageDialogService;
            _userService = userService;
            _databaseService = databaseService;
            LogoutCommand = new Command(LogoutAccount);
            BuffServiceCommand = new Command<string>(BuffService);
            NavigationCommand = new Command<Object>(NavigationPageService);
            ServiceData = new ObservableCollection<ServiceModel>();
        }

        private async void NavigationPageService(Object obj)
        {
            if (IsLoading) return;
            IsLoading = true;
            var key = -9999;
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

            await NavigationService.NavigateAsync(ServiceFunctions[key]);

            IsLoading = false;
        }

        private async void BuffService(string key)
        {
            if (IsLoading) return;
            IsLoading = true;
            var para = new DialogParameters();
            para.Add("UserName", UserModel.UserName);
            switch (key)
            {
                case "0":
                    para.Add("ServiceName", Resource._1000031);
                    para.Add("IdProduct", "50d91145-9fb2-4be8-ac5f-1c7e5a97d34f");
                    break;
                case "1":
                    para.Add("ServiceName", Resource._1000032);
                    para.Add("IdProduct", "4f6225ee-c18c-4b14-b3cf-f7243d0f3dbf");
                    break;
                case "2":
                    para.Add("ServiceName", Resource._1000033);
                    para.Add("IdProduct", "1bddab36-9297-4698-9124-c977238a4a84");
                    break;
                default:
                    break;
            }

            await _dialogService.ShowDialogAsync(nameof(BuffDialog), para);
            IsLoading = false;
        }

        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            if (parameters != null)
            {
                if (parameters.ContainsKey(AppConstants.LoginApp))
                {
                    await InitializeDataHome();
                }
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
                ServiceData.Clear();
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

                HeightPaidService = 1000;
                var role = _userModel.Role;
                foreach (var item in _dataHome)
                {
                    await Task.Delay(TimeSpan.FromMilliseconds(50));
                    if (role == "2")
                    {
                        if (item.UserRole == "2")
                        {
                            ServiceData.Add(item);
                        }
                    }
                    if (role == "0")
                    {
                        ServiceData.Add(item);
                    }
                }
                var heightPaidService = Math.Ceiling((decimal)((double)ServiceData.Count / 3));
                HeightPaidService = (int)(heightPaidService * 140);
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
                        var cmt = new string[]
                        {
                            " đăng ký gói tăng like 1 năm",
                            " đăng ký gói tăng like vính viễn",
                            " đăng ký gói tăng mắt xem livestream 1 năm",
                            " đăng ký gói tăng mắt xem livestream vính viễn",
                            " sử dụng dịch vụ thêm full 5k bạn bè",
                            $" Sử dụng dich vụ tăng {random.Next(1000,6000000)} theo dõi trang cá nhân",
                            $" đăng ký dịch vụ tăng {random.Next(1000,6000000)} like page",
                            " đã sử dụng dịch vụ mở khoán fb thành công",
                            " báo mật fb cá nhân",
                        };
                        var xungHo = new string[] { "Anh/Chi ", "Em ", "Bạn " };
                        var messager = $"{xungHo[random.Next(xungHo.Length - 1)]}{name.full_name}{cmt[random.Next(cmt.Length - 1)]}";
                        UserDialogs.Instance.Toast(new ToastConfig(messager)
                        {

                            BackgroundColor = Color.WhiteSmoke,
                            Message = messager,
                            Position = ToastPosition.Bottom,
                            MessageTextColor = Color.Gray,
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