using AUTO.HLT.MOBILE.VIP.Configurations;
using AUTO.HLT.MOBILE.VIP.Models.Home;
using AUTO.HLT.MOBILE.VIP.Models.Login;
using AUTO.HLT.MOBILE.VIP.Models.Telegram;
using AUTO.HLT.MOBILE.VIP.Services.Database;
using AUTO.HLT.MOBILE.VIP.Services.Guide;
using AUTO.HLT.MOBILE.VIP.Services.Telegram;
using AUTO.HLT.MOBILE.VIP.Services.User;
using AUTO.HLT.MOBILE.VIP.ViewModels;
using MarcTron.Plugin;
using Microsoft.AppCenter.Crashes;
using Newtonsoft.Json;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Essentials;

namespace AUTO.HLT.MOBILE.VIP.FreeModules.ViewModels.BuffAPost
{
    public class BuffAPostViewModel : ViewModelBase
    {
        private string _maxNumber;
        private ItemMenuModel _itemMenu;
        private string _number;
        private string _content;
        private IPageDialogService _pageDialogService;
        private ITelegramService _telegramService;
        private IDatabaseService _databaseService;
        private LoginModel _user;
        private bool _isLoading;
        private IUserService _userService;
        private string _userName;
        private IGuideService _guideService;

        public ICommand RunFeatureCommand { get; set; }

        public string MaxNumber
        {
            get => _maxNumber;
            set => SetProperty(ref _maxNumber, value);
        }

        public string Number
        {
            get => _number;
            set => SetProperty(ref _number, value);
        }

        public string Content
        {
            get => _content;
            set => SetProperty(ref _content, value);
        }

        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }

        public BuffAPostViewModel(INavigationService navigationService, IPageDialogService pageDialogService, ITelegramService telegramService, IDatabaseService databaseService, IUserService userService, IGuideService guideService) : base(navigationService)
        {
            _guideService = guideService;
            _userService = userService;
            _databaseService = databaseService;
            _telegramService = telegramService;
            _pageDialogService = pageDialogService;
            RunFeatureCommand = new AsyncCommand<string>(RunFeature);
            CrossMTAdmob.Current.OnRewardedVideoAdLoaded += (sender, args) =>
            {
                CrossMTAdmob.Current.ShowRewardedVideo();
            };
        }

        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            IsLoading = true;
            _user = await _databaseService.GetAccountUser();
            _userName = _user.UserName;
            if (parameters != null)
            {
                _itemMenu = parameters.GetValue<ItemMenuModel>("TypeFeature");
                if (_itemMenu != null)
                {
                    Title = _itemMenu.TitleItem;
                    await CheckAcountUseService();
                }
            }
            IsLoading = false;
            await Task.Delay(TimeSpan.FromSeconds(3));
            CrossMTAdmob.Current.LoadRewardedVideo(AppConstants.RewardedAdmod);
        }

        private async Task CheckAcountUseService()
        {
            MaxNumber = await _userService.GetPriceUser(_userName) + "";
        }

        private async Task RunFeature(string arg)
        {
            if (IsLoading) return;
            IsLoading = true;
            try
            {
                switch (arg)
                {
                    case "1":
                        await NavigationService.GoBackAsync();
                        break;
                    case "0":
                        if (string.IsNullOrWhiteSpace(Content) || string.IsNullOrWhiteSpace(Number))
                        {
                            await _pageDialogService.DisplayAlertAsync("Thông báo", "Vui lòng điền đầy đủ thông tin",
                                "OK");
                        }
                        else
                        {
                            var price = int.Parse(MaxNumber);
                            if (price > 99)
                            {
                                var num = int.Parse(Number);
                                if (num < 100)
                                {
                                    num = 100;
                                }
                                if (num > price)
                                {
                                    num = price;
                                }

                                var update = await _userService.SetPriceUser(_userName, price - num + "");
                                await UseService(num);
                            }
                            else
                            {
                                if (await _pageDialogService.DisplayAlertAsync("Thông báo", $"Số xu hiện tại của bạn nhỏ 100 nên không thể thực hiện chức năng này vui lòng kiếm thêm xu để sử dụng tính năng này !",
                                    "Kiếm thếm xu", "Để sau"))
                                {
                                    await NavigationService.NavigateAsync("EarnCoinsPage");
                                }
                                else
                                {
                                    await NavigationService.GoBackAsync();
                                }
                            }
                        }

                        break;
                    case "2":
                        await NavigationService.NavigateAsync("EarnCoinsPage");
                        var url = await _guideService.GetGuide();
                        if (!string.IsNullOrEmpty(url))
                            await Browser.OpenAsync(url, BrowserLaunchMode.SystemPreferred);
                        
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

        private async Task ThongBaoKhongHopLe()
        {
            await _pageDialogService.DisplayAlertAsync("Thông báo",
                "Số lượng nhập vào không hợp lệ", "OK");
            Number = null;
        }

        private async Task UseService(int number)
        {

            var contentSend = JsonConvert.SerializeObject(new ContentSendTelegramModel()
            {
                Ten_Thong_Bao = _itemMenu.TitleItem,
                Id_Nguoi_Dung = _user.ID,
                Noi_Dung_Thong_Bao = new
                {
                    Noi_Dung = Content
                },
                So_Luong = number,
                Ghi_Chu = new
                {
                    Ten = _user.Name,
                    Tai_Khoan = _user.UserName,
                    So_dien_thoai = _user.NumberPhone,
                    Loai_Yeu_Cau = "Khách hàng đổi xu"
                }
            }, Formatting.Indented);
            await _telegramService.SendMessageToTelegram(AppConstants.IdChatWork, contentSend);
            await _pageDialogService.DisplayAlertAsync("Thông báo", "Thành công", "OK");

            await CheckAcountUseService();
        }
    }
}