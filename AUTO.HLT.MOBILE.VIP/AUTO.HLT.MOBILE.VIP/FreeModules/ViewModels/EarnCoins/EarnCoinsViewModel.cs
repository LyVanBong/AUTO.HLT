using Acr.UserDialogs;
using AUTO.HLT.MOBILE.VIP.Configurations;
using AUTO.HLT.MOBILE.VIP.Services.Database;
using AUTO.HLT.MOBILE.VIP.Services.User;
using AUTO.HLT.MOBILE.VIP.ViewModels;
using MarcTron.Plugin;
using Microsoft.AppCenter.Crashes;
using Prism.Navigation;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Forms;
using Color = System.Drawing.Color;

namespace AUTO.HLT.MOBILE.VIP.FreeModules.ViewModels.EarnCoins
{
    public class EarnCoinsViewModel : ViewModelBase
    {
        private bool _isLoading;
        private bool _isRunSeenAdmod;
        private IUserService _userService;
        private IDatabaseService _databaseService;
        private string _userName;
        private int _tmpPrice;

        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }

        public ICommand SeenAdmodCommand { get; private set; }
        private int _myPrice;
        public int MyPrice
        {
            get => _myPrice;
            set => SetProperty(ref _myPrice, value);
        }

        public ICommand StopSeenAdmodCommand { get; private set; }
        public EarnCoinsViewModel(INavigationService navigationService, IUserService userService, IDatabaseService databaseService) : base(navigationService)
        {
            _databaseService = databaseService;
            _userService = userService;
            StopSeenAdmodCommand = new AsyncCommand(async () =>
            {
                _isRunSeenAdmod = false;
                Title = "Bắt đầu kiếm xu";
                if (_tmpPrice > 0)
                {
                    await _userService.SetPriceUser(_userName,
                        (await _userService.GetPriceUser(_userName)) + _tmpPrice + "");
                    _tmpPrice = 0;
                }
            });
            SeenAdmodCommand = new AsyncCommand(async () =>
            {
                if (_isRunSeenAdmod) return;
                _isRunSeenAdmod = true;
                await SeenAdmod();
            });
            Title = "Bắt đầu kiếm xu";
            CrossMTAdmob.Current.OnRewardedVideoAdLoaded += (sender, args) =>
            {
                if (_isRunSeenAdmod)
                    CrossMTAdmob.Current.ShowRewardedVideo();
            };
            CrossMTAdmob.Current.OnRewardedVideoAdFailedToLoad += (sender, args) =>
            {
                ShowToast("Xem quảng cáo lỗi");
                Title = "Bắt đầu kiếm xu";
            };
            CrossMTAdmob.Current.OnRewardedVideoAdLeftApplication += (sender, args) =>
            {
                ShowToast("Bạn đang rời khỏi ứng dụng có thể không được tính xu");
            };
            CrossMTAdmob.Current.OnRewardedVideoAdCompleted += (sender, args) =>
            {
                ShowToast("Bạn đã được thưởng 4 xu !");
                _tmpPrice += 4;
                MyPrice += 4;
            };
            CrossMTAdmob.Current.OnRewardedVideoAdClosed += (sender, args) =>
            {
                Title = "Bắt đầu kiếm xu";
                if (_isRunSeenAdmod)
                    SeenAdmod().Await();
            };
        }

        private void ShowToast(string message, int time = 5)
        {
            try
            {
                UserDialogs.Instance.Toast(new ToastConfig(message)
                {
                    Position = ToastPosition.Top,
                    Message = message,
                    BackgroundColor = Color.Black,
                    Duration = TimeSpan.FromSeconds(time),
                    MessageTextColor = Color.WhiteSmoke,
                });
            }
            catch (Exception e)
            {
                Crashes.TrackError(e);
            }
        }
        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            try
            {
                _userName = (await _databaseService.GetAccountUser()).UserName;
                MyPrice = await _userService.GetPriceUser(_userName);
            }
            catch (Exception e)
            {
                Crashes.TrackError(e);
            }
        }

        public override void OnResume()
        {
            base.OnResume();
            _isRunSeenAdmod = true;
        }

        public override async void OnSleep()
        {
            base.OnSleep();
            _isRunSeenAdmod = false;
            await _userService.SetPriceUser(_userName, (await _userService.GetPriceUser(_userName)) + _tmpPrice + "");
            _tmpPrice = 0;
        }

        public override async void OnNavigatedFrom(INavigationParameters parameters)
        {
            base.OnNavigatedFrom(parameters);
            _isRunSeenAdmod = false;
            await _userService.SetPriceUser(_userName, (await _userService.GetPriceUser(_userName)) + _tmpPrice + "");
            _tmpPrice = 0;
        }

        private async Task SeenAdmod()
        {
            if (!_isRunSeenAdmod)
            {
                Title = "Bắt đầu kiếm xu";
                return;
            }
            Title = "Quảng cáo sẽ hiển thị sau 3s";
            await Task.Delay(TimeSpan.FromSeconds(2));
            var num = 1;
            Device.StartTimer(TimeSpan.FromSeconds(1), () =>
            {
                Title = num + "";
                if (num == 3)
                {
                    if (_isRunSeenAdmod)
                        CrossMTAdmob.Current.LoadRewardedVideo(AppConstants.RewardedAdmod);
                    else Title = "Bắt đầu kiếm xu";
                    return false;
                }
                num++;
                return true;
            });

        }
    }
}