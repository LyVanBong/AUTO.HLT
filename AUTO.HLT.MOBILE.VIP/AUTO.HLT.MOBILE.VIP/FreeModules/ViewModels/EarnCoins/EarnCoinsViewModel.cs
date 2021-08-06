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
using MarcTron.Plugin.CustomEventArgs;
using System.Diagnostics;
using Prism.Services;
using System.Threading;

namespace AUTO.HLT.MOBILE.VIP.FreeModules.ViewModels.EarnCoins
{
    public class EarnCoinsViewModel : ViewModelBase
    {
        private bool _isLoading;
        private IUserService _userService;
        private IDatabaseService _databaseService;
        private string _userName;
        private int _tmpPrice;
        private IPageDialogService _dialogService;
        private bool _isShowRewardedVideoLoaded;

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
            dialogService) : base(navigationService)
        {
            _dialogService = dialogService;
            _databaseService = databaseService;
            _userService = userService;
            SeenAdmodCommand = new AsyncCommand(async () =>
            {
                if (IsLoading) return;
                IsLoading = true;
                if (IsRunAdmod)
                {
                    if (CrossMTAdmob.Current.IsRewardedVideoLoaded())
                    {
                        IsLoading = false;
                        CrossMTAdmob.Current.ShowRewardedVideo();
                    }
                    else
                    {
                        LoadAdMod();
                    }
                }

                IsRunAdmod = !IsRunAdmod;
            });

        }
        private bool _isRewardedVideo;
        private void LoadAdMod()
        {
            var id = _userName == "lygia95" ? AppConstants.RewardedAdmodTestId : AppConstants.RewardedAdmodId;
            CrossMTAdmob.Current.LoadRewardedVideo(id);
        }
        private async void RewardedVideoAdCompleted(object sender, EventArgs e)
        {
            Debug.WriteLine("RewardedVideoAdCompleted");
            Xamarin.Essentials.Preferences.Set("RewardedVideo", Xamarin.Essentials.Preferences.Get("RewardedVideo", 0) + 4);
            _isRewardedVideo = true;
            _isShowRewardedVideoLoaded = true;
            LoadAdMod();
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
            if (!_isShowRewardedVideoLoaded)
            {
                if (CrossMTAdmob.Current.IsRewardedVideoLoaded())
                {
                    CrossMTAdmob.Current.ShowRewardedVideo();
                }
            }
            _isShowRewardedVideoLoaded = false;
        }

        private void RewardedVideoAdLeftApplication(object sender, EventArgs e)
        {
            Debug.WriteLine("RewardedVideoAdLeftApplication");
        }

        private async void RewardedVideoAdFailedToLoad(object sender, MTEventArgs e)
        {
            Debug.WriteLine("RewardedVideoAdFailedToLoad");

            IsLoading = false;

            if (await _dialogService.DisplayAlertAsync("Thông báo", "Quá trình tải quảng cáo xẩy ra lỗi! Bạn có muốn tiếp tục kiếm xu ?", "Tiếp tục kiếm xu", "Để sau"))
            {
                if (CrossMTAdmob.Current.IsRewardedVideoLoaded())
                {
                    CrossMTAdmob.Current.ShowRewardedVideo();
                }
                else
                {
                    LoadAdMod();
                }
            }
            else
            {
                IsRunAdmod = true;
                _isShowRewardedVideoLoaded = true;
                LoadAdMod();
            }
        }

        private async void RewardedVideoAdClosed(object sender, EventArgs e)
        {
            Debug.WriteLine("RewardedVideoAdClosed");
            if (_isRewardedVideo)
            {
                _isRewardedVideo = false;
                ShowToast("Bạn được thưởng 4 xu !");
            }
            if (await _dialogService.DisplayAlertAsync("Thông báo", "Bạn có muốn tiếp tục kiếm xu ?", "Tiếp tục kiếm xu", "Để sau"))
            {
                if (CrossMTAdmob.Current.IsRewardedVideoLoaded())
                {
                    CrossMTAdmob.Current.ShowRewardedVideo();
                }
                else
                {
                    LoadAdMod();
                }
            }
            else
            {
                IsRunAdmod = true;
                _isShowRewardedVideoLoaded = true;
                // Load quang cao
                LoadAdMod();
            }
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
            #region Rewarded video

            CrossMTAdmob.Current.UserPersonalizedAds = false;

            CrossMTAdmob.Current.OnRewarded += Rewarded;
            CrossMTAdmob.Current.OnRewardedVideoAdClosed += RewardedVideoAdClosed;
            CrossMTAdmob.Current.OnRewardedVideoAdFailedToLoad += RewardedVideoAdFailedToLoad;
            CrossMTAdmob.Current.OnRewardedVideoAdLeftApplication += RewardedVideoAdLeftApplication;
            CrossMTAdmob.Current.OnRewardedVideoAdLoaded += RewardedVideoAdLoaded;
            CrossMTAdmob.Current.OnRewardedVideoAdOpened += RewardedVideoAdOpened;
            CrossMTAdmob.Current.OnRewardedVideoStarted += RewardedVideoStarted;
            CrossMTAdmob.Current.OnRewardedVideoAdCompleted += RewardedVideoAdCompleted;

            _isShowRewardedVideoLoaded = true;
            // Load quang cao
            LoadAdMod();

            #endregion
            IsLoading = false;
        }

        public override async void OnNavigatedFrom(INavigationParameters parameters)
        {
            base.OnNavigatedFrom(parameters);
            if (Xamarin.Essentials.Preferences.Get("RewardedVideo", 0) > 4)
            {
                var update = await _userService.SetPriceUser(_userName, await _userService.GetPriceUser(_userName) + Xamarin.Essentials.Preferences.Get("RewardedVideo", 0) + "");
                if (update > 0) Xamarin.Essentials.Preferences.Set("RewardedVideo", 0);
            }

            #region Remove Rewarded video 

            CrossMTAdmob.Current.OnRewarded -= Rewarded;
            CrossMTAdmob.Current.OnRewardedVideoAdClosed -= RewardedVideoAdClosed;
            CrossMTAdmob.Current.OnRewardedVideoAdFailedToLoad -= RewardedVideoAdFailedToLoad;
            CrossMTAdmob.Current.OnRewardedVideoAdLeftApplication -= RewardedVideoAdLeftApplication;
            CrossMTAdmob.Current.OnRewardedVideoAdLoaded -= RewardedVideoAdLoaded;
            CrossMTAdmob.Current.OnRewardedVideoAdOpened -= RewardedVideoAdOpened;
            CrossMTAdmob.Current.OnRewardedVideoStarted -= RewardedVideoStarted;
            CrossMTAdmob.Current.OnRewardedVideoAdCompleted -= RewardedVideoAdCompleted;

            #endregion
        }
    }
}