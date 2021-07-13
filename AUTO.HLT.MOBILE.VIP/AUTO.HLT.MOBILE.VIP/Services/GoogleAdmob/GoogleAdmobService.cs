using System;
using System.Diagnostics;
using AUTO.HLT.MOBILE.VIP.Configurations;
using MarcTron.Plugin;
using MarcTron.Plugin.CustomEventArgs;

namespace AUTO.HLT.MOBILE.VIP.Services.GoogleAdmob
{
    public class GoogleAdmobService : IGoogleAdmobService
    {
        private Action _actionInterstitial;
        private Action _actionRewarded;
        private bool _isShowRewardedVideo;
        private bool _isShowInterstitial;
        private bool _isRewardedVideoAdCompleted;
        public bool IsRewarded { get; set; }

        public GoogleAdmobService()
        {
            CrossMTAdmob.Current.UserPersonalizedAds = false;

            #region Interstitial

            CrossMTAdmob.Current.OnInterstitialClosed += InterstitialClosed;
            CrossMTAdmob.Current.OnInterstitialLoaded += InterstitialLoaded;
            CrossMTAdmob.Current.OnInterstitialOpened += InterstitialOpened;

            #endregion

            #region Rewarded video

            CrossMTAdmob.Current.OnRewarded += Rewarded;
            CrossMTAdmob.Current.OnRewardedVideoAdClosed += RewardedVideoAdClosed;
            CrossMTAdmob.Current.OnRewardedVideoAdFailedToLoad += RewardedVideoAdFailedToLoad;
            CrossMTAdmob.Current.OnRewardedVideoAdLeftApplication += RewardedVideoAdLeftApplication;
            CrossMTAdmob.Current.OnRewardedVideoAdLoaded += RewardedVideoAdLoaded;
            CrossMTAdmob.Current.OnRewardedVideoAdOpened += RewardedVideoAdOpened;
            CrossMTAdmob.Current.OnRewardedVideoStarted += RewardedVideoStarted;
            CrossMTAdmob.Current.OnRewardedVideoAdCompleted += RewardedVideoAdCompleted;

            #endregion
        }

        private void RewardedVideoAdCompleted(object sender, EventArgs e)
        {
            Debug.WriteLine("When the ads is completed : " + DateTime.Now);
            _isRewardedVideoAdCompleted = true;
        }

        private void RewardedVideoStarted(object sender, EventArgs e)
        {
            Debug.WriteLine("When the ads starts : " + DateTime.Now);
        }

        private void RewardedVideoAdOpened(object sender, EventArgs e)
        {
            Debug.WriteLine("When the ads is opened : " + DateTime.Now);
            _isRewardedVideoAdCompleted = false;
        }

        private void RewardedVideoAdLoaded(object sender, EventArgs e)
        {
            Debug.WriteLine("When the ads is loaded : " + DateTime.Now);
            if (_isShowRewardedVideo) return;
            _isShowRewardedVideo = true;
            CrossMTAdmob.Current.ShowRewardedVideo();
        }

        private void RewardedVideoAdLeftApplication(object sender, EventArgs e)
        {
            Debug.WriteLine("When the users leaves the application : " + DateTime.Now);
        }

        private void RewardedVideoAdFailedToLoad(object sender, MTEventArgs e)
        {
            Debug.WriteLine("When the ads fails to load : " + DateTime.Now);
        }

        private void RewardedVideoAdClosed(object sender, EventArgs e)
        {
            Debug.WriteLine("When the ads is closed : " + DateTime.Now);
            _isShowRewardedVideo = false;
            if (_isRewardedVideoAdCompleted)
                if (IsRewarded)
                    if (_actionRewarded != null)
                        _actionRewarded();
        }

        private void Rewarded(object sender, MTEventArgs e)
        {
            Debug.WriteLine("When the user gets a reward : " + DateTime.Now);
        }

        private void InterstitialOpened(object sender, EventArgs e)
        {
            Debug.WriteLine("When it's opened : " + DateTime.Now);
        }

        private void InterstitialLoaded(object sender, EventArgs e)
        {
            Debug.WriteLine("When it's loaded : " + DateTime.Now);
            if (_isShowInterstitial) return;
            _isShowInterstitial = true;
            CrossMTAdmob.Current.ShowInterstitial();
        }

        private void InterstitialClosed(object sender, EventArgs e)
        {
            Debug.WriteLine("When it's closed : " + DateTime.Now);
            _isShowInterstitial = true;
        }
        
        public void ShowRewardedVideo()
        {
            CrossMTAdmob.Current.LoadRewardedVideo(AppConstants.RewardedAdmodId);
        }

        public void SubscribeInterstitial(Action subscriber)
        {
            _actionInterstitial += subscriber;
        }

        public void UnSubscribeInterstitial(Action unSubscriber)
        {
            _actionInterstitial -= unSubscriber;
            if (IsRewarded) IsRewarded = false;
        }

        public void SubscribeRewardedVideo(Action subscriber)
        {
            _actionRewarded += subscriber;
        }

        public void UnSubscribeRewardedVideo(Action unSubscriber)
        {
            _actionRewarded -= unSubscriber;
            if (IsRewarded) IsRewarded = false;
        }
    }
}