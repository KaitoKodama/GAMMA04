using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds;
using GoogleMobileAds.Api;
using UnityEngine.UI;

public class GoogleAds : MonoBehaviour
{
    private string adUnitId;
    private RewardedAd rewardedAd;


    void Start()
    {
        MobileAds.Initialize(initStatus => { });
        //adUnitId = "広告ユニットIDをコピペ（Android）";
        adUnitId = "ca-app-pub-3940256099942544/5224354917";

        this.rewardedAd = new RewardedAd(adUnitId);
        this.rewardedAd.OnAdClosed += HandleUserClosedReward;
        this.rewardedAd.OnUserEarnedReward += HandleUserEarnedReward;

        AdRequest request = new AdRequest.Builder().Build();
        this.rewardedAd.LoadAd(request);
    }


    public delegate void OnEarnedRewardNotifyer();
    public OnEarnedRewardNotifyer OnEarnedRewardNotifyerHandler;

    private void HandleUserClosedReward(object sender, EventArgs args)
    {
        AdRequest request = new AdRequest.Builder().Build();
        this.rewardedAd.LoadAd(request);
    }
    private void HandleUserEarnedReward(object sender, Reward args)
    {
        OnEarnedRewardNotifyerHandler?.Invoke();
    }

    public void ShowReawrd()
    {
        if (this.rewardedAd.IsLoaded())
        {
            this.rewardedAd.Show();
        }
    }
}
