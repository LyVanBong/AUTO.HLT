using System;

namespace AUTO.HLT.MOBILE.VIP.Services.GoogleAdmob
{
    public interface IGoogleAdmobService
    {
        bool IsRewarded { get; set; }
        void ShowRewardedVideo();
        void SubscribeInterstitial(Action subscriber);
        void UnSubscribeInterstitial(Action unSubscriber);
        void SubscribeRewardedVideo(Action subscriber);
        void UnSubscribeRewardedVideo(Action unSubscriber);
    }
}