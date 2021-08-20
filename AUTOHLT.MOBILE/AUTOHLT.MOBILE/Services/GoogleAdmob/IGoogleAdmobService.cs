using System;

namespace AUTOHLT.MOBILE.Services.GoogleAdmob
{
    public interface IGoogleAdmobService
    {
        bool IsRewarded { get; set; }

        void ShowInterstitial();

        void ShowRewardedVideo();

        void SubscribeInterstitial(Action subscriber);

        void UnSubscribeInterstitial(Action unSubscriber);

        void SubscribeRewardedVideo(Action subscriber);

        void UnSubscribeRewardedVideo(Action unSubscriber);
    }
}