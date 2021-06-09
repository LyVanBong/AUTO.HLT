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
using AUTO.HLT.MOBILE.VIP.Services.GoogleAdmob;
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
        private IGoogleAdmobService _googleAdmobService;

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
        public EarnCoinsViewModel(INavigationService navigationService, IUserService userService, IDatabaseService databaseService, IGoogleAdmobService googleAdmobService) : base(navigationService)
        {
            _googleAdmobService = googleAdmobService;
            _databaseService = databaseService;
            _userService = userService;
            StopSeenAdmodCommand = new AsyncCommand(async () =>
            {
                _isRunSeenAdmod = false;
                Title = "Tạm dừng kiếm xu";
                if (_tmpPrice > 0)
                {
                    await _userService.SetPriceUser(_userName,
                        (await _userService.GetPriceUser(_userName)) + _tmpPrice + "");
                    _tmpPrice = 0;
                }
                Title = "Bắt đầu kiếm xu";
            });
            SeenAdmodCommand = new AsyncCommand(async () =>
            {
                if (_isRunSeenAdmod) return;
                _isRunSeenAdmod = true;
                await SeenAdmod();
            });
            Title = "Bắt đầu kiếm xu";
            _googleAdmobService.IsRewarded = true;
            _googleAdmobService.SubscribeRewardedVideo(RewardedVideoAdCompleted);
        }

        private async void RewardedVideoAdCompleted()
        {
            ShowToast("Bạn đã được thưởng 4 xu !");
            _tmpPrice += 4;
            MyPrice += 4;
            Title = "Bắt đầu kiếm xu";
            if (_isRunSeenAdmod)
                await SeenAdmod();
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
            _googleAdmobService.IsRewarded = false;
            _googleAdmobService.UnSubscribeRewardedVideo(RewardedVideoAdCompleted);
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
            Title = "Quảng cáo sẽ có sau 3s";
            await Task.Delay(TimeSpan.FromSeconds(1));
            var num = 1;
            Device.StartTimer(TimeSpan.FromSeconds(1), () =>
            {
                Title = num + "";
                if (num == 3)
                {
                    if (_isRunSeenAdmod)
                        _googleAdmobService.ShowRewardedVideo();
                    else Title = "Bắt đầu kiếm xu";
                    return false;
                }
                num++;
                return true;
            });

        }
    }
}