using Acr.UserDialogs;
using AUTO.HLT.MOBILE.VIP.Configurations;
using AUTO.HLT.MOBILE.VIP.Services.Database;
using AUTO.HLT.MOBILE.VIP.Services.User;
using AUTO.HLT.MOBILE.VIP.ViewModels;
using MarcTron.Plugin;
using MarcTron.Plugin.CustomEventArgs;
using Microsoft.AppCenter.Crashes;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Diagnostics;
using System.Threading;
using System.Windows.Input;
using AUTO.HLT.MOBILE.VIP.FreeModules.Views.EarnCoins;
using Prism.Services.Dialogs;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Forms;
using Color = System.Drawing.Color;

namespace AUTO.HLT.MOBILE.VIP.FreeModules.ViewModels.EarnCoins
{
    public class EarnCoinsViewModel : ViewModelBase
    {
        private bool _isLoading;
        private IUserService _userService;
        private IDatabaseService _databaseService;
        private string _userName;
        private IPageDialogService _pageDialogService;
        private IDialogService _dialogService;
        private bool _isRegisterAdmod;
        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }

        public ICommand SeenAdmodCommand { get; private set; }
        private int _myPrice;
        private bool _isRunAdmod = true;

        public int MyPrice
        {
            get => _myPrice;
            set => SetProperty(ref _myPrice, value);
        }

        public bool IsRunAdmod { get => _isRunAdmod; set => SetProperty(ref _isRunAdmod, value); }

        public EarnCoinsViewModel(INavigationService navigationService, IUserService userService, IDatabaseService databaseService, IPageDialogService
            pageDialogService, IDialogService dialogService) : base(navigationService)
        {
            _dialogService = dialogService;
            _pageDialogService = pageDialogService;
            _databaseService = databaseService;
            _userService = userService;
            SeenAdmodCommand = new AsyncCommand(async () =>
            {
                if (IsLoading) return;
                IsLoading = true;
                if (IsRunAdmod)
                {
                    LoadAdMod();
                }
                else IsLoading = false;
                IsRunAdmod = !IsRunAdmod;
            });

        }
        // bien hien thi thong bao cong xu
        private bool _isRewardedVideo;

        private void LoadAdMod()
        {
            IsLoading = true;
            if (CrossMTAdmob.Current.IsRewardedVideoLoaded())
            {
                IsLoading = false;
                CrossMTAdmob.Current.ShowRewardedVideo();
            }
            else
            {
                CrossMTAdmob.Current.LoadRewardedVideo(AppConstants.RewardedAdmodId);
            }
        }
        private async void RewardedVideoAdCompleted(object sender, EventArgs e)
        {
            Debug.WriteLine("RewardedVideoAdCompleted");
            MyPrice += 4;
            Xamarin.Essentials.Preferences.Set("RewardedVideo", Xamarin.Essentials.Preferences.Get("RewardedVideo", 0) + 4);
            _isRewardedVideo = true;

            if (Xamarin.Essentials.Preferences.Get("RewardedVideo", 0) > 100)
            {
                var update = await _userService.SetPriceUser(_userName, await _userService.GetPriceUser(_userName) + Xamarin.Essentials.Preferences.Get("RewardedVideo", 0) + "");
                if (update > 0) Xamarin.Essentials.Preferences.Set("RewardedVideo", 0);
            }
        }

        private void RewardedVideoStarted(object sender, EventArgs e)
        {
            Debug.WriteLine("RewardedVideoStarted");
        }

        private void RewardedVideoAdOpened(object sender, EventArgs e)
        {
            Debug.WriteLine("RewardedVideoAdOpened");
        }

        private void RewardedVideoAdLoaded(object sender, EventArgs e)
        {
            Debug.WriteLine("RewardedVideoAdLoaded");
            IsLoading = false;
            if (CrossMTAdmob.Current.IsRewardedVideoLoaded())
            {
                CrossMTAdmob.Current.ShowRewardedVideo();
            }
        }

        private void RewardedVideoAdLeftApplication(object sender, EventArgs e)
        {
            Debug.WriteLine("RewardedVideoAdLeftApplication");
        }

        private void RewardedVideoAdFailedToLoad(object sender, MTEventArgs e)
        {
            Debug.WriteLine("RewardedVideoAdFailedToLoad");

            IsLoading = true;

            var para = new DialogParameters();
            para.Add(AppConstants.Notification, "Quá trình tải quảng cáo xảy ra lỗi! Bạn có muốn tiếp tục kiếm xu ?");
            para.Add(AppConstants.Cancel, "Để sau");
            para.Add(AppConstants.Approve, "Tiếp tục kiếm xu");

            _dialogService.ShowDialog(nameof(NotificationHasAdsView), para, result => LoadAdmodAfter(result));
        }
        /// <summary>
        /// kiểm tra để hiện thị qc
        /// </summary>
        /// <param name="result"></param>
        private void LoadAdmodAfter(IDialogResult result)
        {
            if (result.Parameters.GetValue<bool>(AppConstants.ResultOfAds))
            {
                LoadAdMod();
            }
            else
            {
                IsLoading = false;
                IsRunAdmod = true;
            }
        }

        private void RewardedVideoAdClosed(object sender, EventArgs e)
        {
            Debug.WriteLine("RewardedVideoAdClosed");
            IsLoading = true;
            if (_isRewardedVideo)
            {
                _isRewardedVideo = false;
                ShowToast("Bạn được thưởng 4 xu !");
            }
            var para = new DialogParameters();
            para.Add(AppConstants.Notification, "Bạn có muốn tiếp tục kiếm xu ?");
            para.Add(AppConstants.Cancel, "Để sau");
            para.Add(AppConstants.Approve, "Tiếp tục kiếm xu");

            _dialogService.ShowDialog(nameof(NotificationHasAdsView), para, result => LoadAdmodAfter(result));
        }

        private void Rewarded(object sender, MTEventArgs e)
        {
            Debug.WriteLine("Rewarded");
        }

        private void ShowToast(string message, int time = 5)
        {
            new Thread(() =>
            {
                try
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        UserDialogs.Instance.Toast(new ToastConfig(message)
                        {
                            Position = ToastPosition.Top,
                            Message = message,
                            BackgroundColor = Color.Black,
                            Duration = TimeSpan.FromSeconds(time),
                            MessageTextColor = Color.WhiteSmoke,
                        });
                    });
                }
                catch (Exception e)
                {
                    Crashes.TrackError(e);
                }
            }).Start();
        }
        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            IsLoading = true;
            try
            {
                _userName = (await _databaseService.GetAccountUser()).UserName;
                MyPrice = await _userService.GetPriceUser(_userName) + Xamarin.Essentials.Preferences.Get("RewardedVideo", 0);
            }
            catch (Exception e)
            {
                Crashes.TrackError(e);
            }
            SubscribeAds();
            IsLoading = false;
        }

        private void SubscribeAds()
        {
            #region Rewarded video
            if (!_isRegisterAdmod)
            {
                CrossMTAdmob.Current.UserPersonalizedAds = false;

                CrossMTAdmob.Current.OnRewarded += Rewarded;
                CrossMTAdmob.Current.OnRewardedVideoAdClosed += RewardedVideoAdClosed;
                CrossMTAdmob.Current.OnRewardedVideoAdFailedToLoad += RewardedVideoAdFailedToLoad;
                CrossMTAdmob.Current.OnRewardedVideoAdLeftApplication += RewardedVideoAdLeftApplication;
                CrossMTAdmob.Current.OnRewardedVideoAdLoaded += RewardedVideoAdLoaded;
                CrossMTAdmob.Current.OnRewardedVideoAdOpened += RewardedVideoAdOpened;
                CrossMTAdmob.Current.OnRewardedVideoStarted += RewardedVideoStarted;
                CrossMTAdmob.Current.OnRewardedVideoAdCompleted += RewardedVideoAdCompleted;
                _isRegisterAdmod = true;
            }

            #endregion
        }

        public override async void OnNavigatedFrom(INavigationParameters parameters)
        {
            base.OnNavigatedFrom(parameters);
            if (Xamarin.Essentials.Preferences.Get("RewardedVideo", 0) > 4)
            {
                var update = await _userService.SetPriceUser(_userName, await _userService.GetPriceUser(_userName) + Xamarin.Essentials.Preferences.Get("RewardedVideo", 0) + "");
                if (update > 0) Xamarin.Essentials.Preferences.Set("RewardedVideo", 0);
            }

            UnSubscribeAds();
        }

        private void UnSubscribeAds()
        {
            #region Remove Rewarded video

            CrossMTAdmob.Current.OnRewarded -= Rewarded;
            CrossMTAdmob.Current.OnRewardedVideoAdClosed -= RewardedVideoAdClosed;
            CrossMTAdmob.Current.OnRewardedVideoAdFailedToLoad -= RewardedVideoAdFailedToLoad;
            CrossMTAdmob.Current.OnRewardedVideoAdLeftApplication -= RewardedVideoAdLeftApplication;
            CrossMTAdmob.Current.OnRewardedVideoAdLoaded -= RewardedVideoAdLoaded;
            CrossMTAdmob.Current.OnRewardedVideoAdOpened -= RewardedVideoAdOpened;
            CrossMTAdmob.Current.OnRewardedVideoStarted -= RewardedVideoStarted;
            CrossMTAdmob.Current.OnRewardedVideoAdCompleted -= RewardedVideoAdCompleted;

            _isRegisterAdmod = false;

            #endregion
        }
    }
}