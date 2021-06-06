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

        public EarnCoinsViewModel(INavigationService navigationService, IUserService userService, IDatabaseService databaseService) : base(navigationService)
        {
            _databaseService = databaseService;
            _userService = userService;
            SeenAdmodCommand = new AsyncCommand(async () =>
            {
                _isRunSeenAdmod = !_isRunSeenAdmod;
                await SeenAdmod();
            });
            Title = "Bắt đầu kiếm xu";
            CrossMTAdmob.Current.OnRewardedVideoAdLoaded += (sender, args) =>
            {
                CrossMTAdmob.Current.ShowRewardedVideo();
            };
            CrossMTAdmob.Current.OnRewardedVideoAdFailedToLoad += (sender, args) =>
            {
                UserDialogs.Instance.Toast("Xem quảng cáo lỗi", TimeSpan.FromSeconds(2));
                _isRunSeenAdmod = false;
            };
            CrossMTAdmob.Current.OnRewardedVideoAdLeftApplication += (sender, args) =>
            {
                UserDialogs.Instance.Toast("Bạn đang rời khỏi ứng dụng có thể không được tính xu", TimeSpan.FromSeconds(2));
            };
            CrossMTAdmob.Current.OnRewardedVideoAdCompleted += (sender, args) =>
            {
                UserDialogs.Instance.Toast("Bạn đã được thưởng 2 xu !", TimeSpan.FromSeconds(3));
                _tmpPrice += 2;
                MyPrice += _tmpPrice;
            };
            CrossMTAdmob.Current.OnRewardedVideoAdClosed += (sender, args) =>
            {
                Task.Delay(TimeSpan.FromSeconds(1)).Await();
                SeenAdmod().Await();
            };
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
                    Title = "Bắt đầu kiếm xu";
                    CrossMTAdmob.Current.LoadRewardedVideo(AppConstants.RewardedAdmod);
                    return false;
                }
                num++;
                return true;
            });

        }
    }
}