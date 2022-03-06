using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;
using UnityEngine.SceneManagement;


public class GoogleAds : MonoBehaviour
{
    private BannerView bannerView;
    private bool isTest = false;

    void Start()
    {
        MobileAds.Initialize(initStatus => { });
        RequestBanner();
    }
    private void RequestBanner()
    {
        string adUnitId = "ca-app-pub-3940256099942544/6300978111";
        if (!isTest)
		{
            adUnitId = "ca-app-pub-5790859037061683/1582493065";
        }
        AdSize adaptiveSize = AdSize.GetCurrentOrientationAnchoredAdaptiveBannerAdSizeWithWidth(AdSize.FullWidth);
        bannerView = new BannerView(adUnitId, adaptiveSize, AdPosition.Bottom);
        AdRequest request = new AdRequest.Builder().Build();
        bannerView.LoadAd(request);

        SceneManager.sceneUnloaded += OnSceneExit;
    }
    private void OnSceneExit(Scene current)
	{
        bannerView.Hide();
        bannerView.Destroy();
    }
}
