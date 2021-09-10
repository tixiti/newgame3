using System;
using GoogleMobileAds.Api;
using UnityEngine;
public class AdsManager : MonoBehaviour
{
    public static AdsManager instance;

    private BannerView _bannerView;

    private InterstitialAd _interstitial;

    private RewardedAd _rewardedAds;

    public bool publishingApp;
    public static bool isHintReady;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else Destroy(this);
    }
    // Start is called before the first frame update
    public void Start()
    {
        // Initialize the Google Mobile Ads SDK.
        MobileAds.Initialize((initStatus)=>{InitAdsDone();});
        this.RequestBanner();
        this.RequestRewardedVideo();
        this.RequestInterstitial();
    }

    private void Update()
    {
        if (isHintReady)
        {
            HintController.instance.Next();
            isHintReady = false;
        }
    }

    private void InitAdsDone()
    {
        Debug.Log("OK");
    }
    private void RequestBanner()
    {
#if UNITY_ANDROID
        string adUnitId = "ca-app-pub-3940256099942544/6300978111";
#elif UNITY_IPHONE
            string adUnitId = "ca-app-pub-3940256099942544/2934735716";
#else
            string adUnitId = "unexpected_platform";
#endif

        // Create a 320x50 banner at the top of the screen.
        this._bannerView = new BannerView(adUnitId, AdSize.Banner, AdPosition.Bottom);
        // Called when an ad request has successfully loaded.
        this._bannerView.OnAdLoaded += this.HandleOnBannerLoaded;
        // Called when an ad request failed to load.
        this._bannerView.OnAdFailedToLoad += this.HandleOnBannerFailedToLoad;
        // Called when an ad is clicked.
        this._bannerView.OnAdOpening += this.HandleOnBannerOpened;
        // Called when the user returned from the app after an ad click.
        this._bannerView.OnAdClosed += this.HandleOnBannerClosed;
        // Called when the ad click caused the user to leave the application.

    }
    #region Handle Banner Callback
    public void HandleOnBannerLoaded(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleAdLoaded event received");
    }

    public void HandleOnBannerFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        MonoBehaviour.print("HandleFailedToReceiveAd event received with message: "
                            + args.LoadAdError);
        RequestBanner();

    }

    public void HandleOnBannerOpened(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleAdOpened event received");
    }

    public void HandleOnBannerClosed(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleAdClosed event received");
        RequestBanner();

    }

    public void HandleOnBannerLeavingApplication(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleAdLeavingApplication event received");
    }
    #endregion
    private void RequestInterstitial()
    {
#if UNITY_ANDROID
        string adUnitId = "ca-app-pub-9165042803570217/1899931803";
#elif UNITY_IPHONE
        string adUnitId = "ca-app-pub-9165042803570217/1357840476";
#else
        string adUnitId = "unexpected_platform";
#endif

        // Initialize an InterstitialAd.
        this._interstitial = new InterstitialAd(adUnitId);
        // Called when an ad request has successfully loaded.
        this._interstitial.OnAdLoaded += HandleOnAdLoaded;
        // Called when an ad request failed to load.
        this._interstitial.OnAdFailedToLoad += HandleOnAdFailedToLoad;
        // Called when an ad is shown.
        this._interstitial.OnAdOpening += HandleOnAdOpened;
        // Called when the ad is closed.
        this._interstitial.OnAdClosed += HandleOnAdClosed;
        // Called when the ad click caused the user to leave the application.
        if (publishingApp)
        {
            // Create an empty ad request.
            AdRequest request = new AdRequest.Builder().Build();
            // Load the interstitial with the request.
            this._interstitial.LoadAd(request);

        }
        else
        {
            // Create an empty ad request.
            AdRequest request = new AdRequest.Builder().Build();
            // Load the interstitial with the request.
            this._interstitial.LoadAd(request);
        }


    }
    #region Handle Interstitial
    public void HandleOnAdLoaded(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleAdLoaded event received");
    }

    public void HandleOnAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        MonoBehaviour.print("HandleFailedToReceiveAd event received with message: "
                            + args.LoadAdError);
        RequestInterstitial();
    }

    public void HandleOnAdOpened(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleAdOpened event received");
    }

    public void HandleOnAdClosed(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleAdClosed event received");
        RequestInterstitial();
    }
    #endregion
    private void RequestRewardedVideo()
    {
        string adUnitId;
#if UNITY_ANDROID
        adUnitId = "ca-app-pub-9165042803570217/1586910924";
#elif UNITY_IPHONE
            adUnitId = "ca-app-pub-9165042803570217/7413952677";
#else
            adUnitId = "unexpected_platform";
#endif
        this._rewardedAds = new RewardedAd(adUnitId);
        // Called when an ad request has successfully loaded.
        this._rewardedAds.OnAdLoaded += HandleRewardedAdLoaded;
        // Called when an ad request failed to load.
        this._rewardedAds.OnAdFailedToLoad += HandleRewardedAdFailedToLoad;
        // Called when an ad is shown.
        this._rewardedAds.OnAdOpening += HandleRewardedAdOpening;
        // Called when an ad request failed to show.
        this._rewardedAds.OnAdFailedToShow += HandleRewardedAdFailedToShow;
        // Called when the user should be rewarded for interacting with the ad.
        this._rewardedAds.OnUserEarnedReward += HandleUserGetHint;
        // Called when the ad is closed.
        this._rewardedAds.OnAdClosed += HandleRewardedAdClosed;
        if (publishingApp)
        {
            // Create an empty ad request.
            AdRequest request = new AdRequest.Builder().Build();
            // Load the rewarded video ad with the request.
            this._rewardedAds.LoadAd(request);
        }
        else
        {
            // Create an empty ad request.
            AdRequest request = new AdRequest.Builder().Build();
            // Load the rewarded video ad with the request.
            this._rewardedAds.LoadAd(request);
        }
    }
    #region HandleRewardedVideo
    public void HandleRewardedAdLoaded(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleRewardedAdLoaded event received");
    }

    public void HandleRewardedAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        MonoBehaviour.print(
            "HandleRewardedAdFailedToLoad event received with message: "
                             + args.LoadAdError);
        RequestRewardedVideo();
    }

    public void HandleRewardedAdOpening(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleRewardedAdOpening event received");
    }

    public void HandleRewardedAdFailedToShow(object sender, AdErrorEventArgs args)
    {
        MonoBehaviour.print(
            "HandleRewardedAdFailedToShow event received with message: "
                             + args.AdError);
        UIController.instance.PopUpNotificationPanel("Check your internet connection and try again!");
        RequestRewardedVideo();
    }

    public void HandleRewardedAdClosed(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleRewardedAdClosed event received");
        RequestRewardedVideo();
    }

    public void HandleUserGetHint(object sender, Reward args)
    {
        isHintReady = true;
    }
    #endregion
    public void ShowIntestellarAds()
    {
        if (this._interstitial.IsLoaded())
        {
            this._interstitial.Show();
        }
    }
    public void ShowBannerAds()
    {
        AdRequest request = new AdRequest.Builder().Build();
        _bannerView.LoadAd(request);
    }
    public void HideBannerAds()
    {
        _bannerView.Destroy();
        RequestBanner();
    }
    public void DisplayHintAds()
    {
        if (_rewardedAds.IsLoaded())
        {
            _rewardedAds.Show();
        }
    }
}
