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
using System.Threading.Tasks;
using System.Windows.Input;
using DIPS.Xamarin.UI.Controls.Toast;
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
        private List<ServiceModel> _serviceData;
        private int _heightPaidService;
        private int _heightFreeService;


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

        public List<ServiceModel> ServiceData
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
            IsLoading = true;
            BuffServiceCommand = new Command<string>(BuffService);
            NavigationCommand = new Command<ServiceModel>(NavigationPageService);
        }

        private async void NavigationPageService(ServiceModel obj)
        {
            if (IsLoading) return;
            IsLoading = true;

            var options = new ToastOptions
            {
                ToastAction = async () =>
                {
                    // something
                    await Toast.HideToast();
                },
                OnBeforeDisplayingToast = toast =>
                {
                    toast.TranslationY -= 50;
                    return toast.TranslateTo(0, toast.TranslationY + 50, 500, Easing.Linear);
                },
                OnBeforeHidingToast = toast => toast.TranslateTo(0, -(toast.TranslationY + 50), 500, Easing.Linear),
                Duration = 5000
            };

            var layout = new ToastLayout
            {
                BackgroundColor = Color.DodgerBlue,
                CornerRadius = 10,
                FontFamily = "Arial",
                FontSize = 12,
                HasShadow = true,
                HorizontalMargin = 25,
                LineBreakMode = LineBreakMode.TailTruncation,
                MaxLines = 2,
                Padding = new Thickness(20, 10),
                PositionY = 20,
                TextColor = Color.White
            };

            await Toast.DisplayToast("Hello World!", options, layout);

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
            await InitializeDataHome();
            IsLoading = false;
        }

        private async Task InitializeDataHome()
        {
            try
            {
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
                ServiceData = new List<ServiceModel>
                {
                    new ServiceModel
                    {
                        Icon="icon_like.png",
                        TitleService=Resource._1000076,
                        TypeService=1,
                    },
                    new ServiceModel
                    {
                        Icon="icon_view.png",
                        TitleService=Resource._1000077,
                        TypeService=2,
                    },
                    new ServiceModel
                    {
                        Icon="icon_Interactive.png",
                        TitleService=Resource._1000026,
                        TypeService=3,
                        BadgeView = "Coming"
                    },
                    new ServiceModel
                    {
                        Icon="icon_add_friends.png",
                        TitleService=Resource._1000078,
                        TypeService=4,
                    },
                    new ServiceModel
                    {
                        Icon="icon_follow.png",
                        TitleService=Resource._1000079,
                        TypeService=5,
                    },
                    new ServiceModel
                    {
                        Icon="icon_like_page.png",
                        TitleService=Resource._1000080,
                        TypeService=6,
                    },
                    new ServiceModel
                    {
                        Icon="icon_unlock.png",
                        TitleService=Resource._1000081,
                        TypeService=7,
                    },
                    new ServiceModel
                    {
                        Icon="icon_security_fb.png",
                        TitleService=Resource._1000082,
                        TypeService=8,
                    },
                    new ServiceModel
                    {
                        Icon="icon_filter_friends.png",
                        TitleService=Resource._1000083,
                        TypeService=-1,
                        BadgeView = "Free"
                    },
                    new ServiceModel
                    {
                        Icon="icon_customer_support.png",
                        TitleService=Resource._1000084,
                        TypeService=0,
                        BadgeView = "Support"
                    },
                };
                double count = ServiceData.Count;
                var soDu = count / 3;
                var heightPaidService = Math.Ceiling((decimal)soDu);
                HeightPaidService = (int)(heightPaidService * 140);
            }
            catch (Exception e)
            {
                Crashes.TrackError(e);
            }
        }

        private async void LogoutAccount()
        {
            IsLoading = true;
            var res = await _pageDialogService.DisplayAlertAsync(Resource._1000035, Resource._1000042, "OK", "Cancel");
            if (res)
            {
                Preferences.Clear();
                await _databaseService.DeleteAccontUser();
                await NavigationService.NavigateAsync("/LoginPage");
            }

            IsLoading = false;
        }
    }
}