using GoogleMobileAds.Api;
using System;
using System.Collections;
using UnityEngine;

public class AdsManager : MonoBehaviour
{
    public static AdsManager Instance {  get; private set; }
    [Header("Settings")]
    [Tooltip("munculkan banner disaat script ini mulai jalan secara otomatis")]
    [SerializeField] bool automaticallyShowBanner;
    [Tooltip("munculkan app open ads disaat script ini mulai jalan secara otomatis")]
    [SerializeField] bool automaticallyShowAppOpen;
    [Tooltip("beberapa kali pemain harus main sebelum iklan ini muncul, jika automaticallyShowAppOpen inactive, varible ini tidak berguna.")]
    [SerializeField] int timesBeforeAppOpenShow = 3;
    [Tooltip("jika ada sistem no ads purchase, pastikan ini nyala")]
    public bool disableForcedAds;
    //REMIND : ganti ads unit buat masing masing build. ios dan android punya sendiri
    [Header("Intersitial Ads")] // Gunakan ShowInterstitial()
    [SerializeField] string interstitialAdsUnit = "ca-app-pub-3940256099942544/1033173712";
    private InterstitialAd interstitialAd;
    private DateTime interstitialLoadTime;
    private readonly TimeSpan adExpiryTime = TimeSpan.FromHours(1);
    [Header("Rewarded Ads")] // Gunakan ShowRewarded(string reward), effectnya harus dikasih di dalam function agar bisa memberi reward!
    [SerializeField] string rewardedAdsUnit = "ca-app-pub-3940256099942544/5224354917";
    private RewardedAd rewardedAd;
    private DateTime rewardedLoadTime;
    [Header("Banner Ads")] // Gunakan ShowBanner(int index) atau ShowAllBanner()
    [SerializeField] BannerConfig[] bannerList;
    [Header("App Open Ads")] // Gunakan ShowAppOpen()
    [SerializeField] string appOpenAdsUnit = "ca-app-pub-3940256099942544/9257395921";
    private AppOpenAd appOpenAd;
    private DateTime appOpenLoadTime;
    private readonly TimeSpan appOpenAdExpiryTime = TimeSpan.FromHours(4);
    [Header("Interface")]
    [SerializeField] GameObject interfaceBlocker;
    bool isShowingAds = false; // jika true, jangan ada yang boleh menunjukkan iklan


    // [ USABLE FUNCTION ]
    //Tools to show/hide the ads :
    public void ShowInterstitial()
    {
        if (disableForcedAds || isShowingAds)
        {
            return;
        }
        if (interstitialAd != null && interstitialAd.CanShowAd() && !IsExpired(interstitialLoadTime))
        {
            isShowingAds = true;
            PauseGame();
            interstitialAd.Show();
        } else
        {
            LoadInterstitial();
        }
    }

    // GANTI REWARD dan SCRIPT SESUAI DENGAN GAME
    public void ShowRewarded(string selectedReward)
    {
        if (isShowingAds)
            return;
        if (rewardedAd != null && rewardedAd.CanShowAd() && !IsExpired(rewardedLoadTime))
        {
            PauseGame();
            isShowingAds = true;
            rewardedAd.OnAdFullScreenContentFailed += (error) =>
            {
                if (selectedReward == "Revive")
                {
                    RevivePanel.Instance.FailedRevive();
                }
            };
            rewardedAd.Show((Reward reward) =>
            {
                // put all reward in here!!!
                switch (selectedReward)
                {
                    case "rewardTemplate":
                        RunOnMainThread(() =>
                        {
                            //jalanin semua function di dalam RunOnMainThread()!
                        });
                        break;
                    case "Revive":
                        RunOnMainThread(() =>
                        {
                            RevivePanel.Instance.Revive();
                        });
                        break;

                }
            });
        } else
        {
            LoadRewarded();
        }
    }

    public void ShowBanner(int index)
    {
        if (bannerList.Length == 0 || bannerList[index] == null || disableForcedAds)
        {
            return;
        }
        AdSize size = AdSize.GetCurrentOrientationAnchoredAdaptiveBannerAdSizeWithWidth(AdSize.FullWidth);
        if (bannerList[index].useCustomSize)
        {
            size = new AdSize((int)bannerList[index].customSize.x, (int)bannerList[index].customSize.y);
        }
        if (bannerList[index].useCustomPosition)
        {
            bannerList[index].currentBanner = new BannerView(bannerList[index].bannerAdsUnit, size, (int)bannerList[index].customPosition.x, (int)bannerList[index].customPosition.y);
        } else
        {
            bannerList[index].currentBanner = new BannerView(bannerList[index].bannerAdsUnit, size, bannerList[index].bannerPosition);
        }
        bannerList[index].currentBanner.LoadAd(new AdRequest());
    }

    public void ShowAllBanner()
    {
        if (bannerList.Length == 0 || disableForcedAds)
        {
            return;
        }
        foreach (var banner in bannerList)
        {
            AdSize size = AdSize.GetCurrentOrientationAnchoredAdaptiveBannerAdSizeWithWidth(AdSize.FullWidth);
            if (banner.useCustomSize)
            {
                size = new AdSize((int)banner.customSize.x, (int)banner.customSize.y);
            }
            if (banner.useCustomPosition)
            {
                banner.currentBanner = new BannerView(banner.bannerAdsUnit, size, (int)banner.customPosition.x, (int)banner.customPosition.y);
            }
            else
            {
                banner.currentBanner = new BannerView(banner.bannerAdsUnit, size, banner.bannerPosition);
            }
            banner.currentBanner.LoadAd(new AdRequest());
        }
    }

    public void HideBanner(int index)
    {
        if (bannerList.Length > 0 && bannerList[index].currentBanner != null)
        {
            bannerList[index].currentBanner.Destroy();
            bannerList[index].currentBanner = null;
        }
    }

    public void HideAllBanner()
    {
        if (bannerList.Length == 0)
        {
            return;
        }
        foreach (var banner in bannerList)
        {
            if (banner.currentBanner != null)
            {
                banner.currentBanner.Destroy();
                banner.currentBanner = null;
            }
        }
    }

    public void ShowAppOpen()
    {
        if (disableForcedAds || isShowingAds)
        {
            return;
        }
        if (appOpenAd != null && appOpenAd.CanShowAd() && !IsAppOpenExpired())
        {
            isShowingAds = true;
            appOpenAd.Show();
        } else
        {
            LoadAppOpen();
        }
    }

    public bool CheckAdsAvailability(string adType)
    {
        switch (adType)
        {
            case "AppOpen":
                return appOpenAd != null;
            case "Banner":
                return bannerList.Length > 0;
            case "Rewarded":
                return rewardedAd != null;
            case "Interstitial":
                return interstitialAd != null;
            default:
                print("[AdsNanager_CheckAdsAvailability] " + adType + " doesn't exist.");
                return false;
        }
    }

    // [ LOADER SEGMENT ]
    // Ad loader, use this to load those ads, NOT for showing.
    void LoadInterstitial()
    {
        interstitialAd = null;
        var adRequest = new AdRequest();
        InterstitialAd.Load(interstitialAdsUnit, adRequest, (InterstitialAd ad, LoadAdError error) =>
        {
            if (error != null)
            {
                //failure to load!, do something!
                Debug.LogWarning("[AdsManager_Interstitial] Ads failed: " + error);
                StartCoroutine(ReloadAds("Interstitial"));
                return;
            }
            //success load
            interstitialAd = ad;
            interstitialLoadTime = DateTime.Now;
            interstitialAd.OnAdFullScreenContentClosed += () =>
            {
                RunOnMainThread(() =>
                {
                    isShowingAds = false;
                    ResumeGame();
                    LoadInterstitial();
                });
            };
            interstitialAd.OnAdFullScreenContentFailed += (error) =>
            {
                isShowingAds = false;
                ResumeGame();
                Debug.Log("[AdsManager_Interstitial] Ads failed to show : " + error);
                LoadInterstitial();
            };
        });
    }

    void LoadRewarded()
    {
        rewardedAd = null;
        var adRequest = new AdRequest();
        RewardedAd.Load(rewardedAdsUnit, adRequest, (RewardedAd ad, LoadAdError error) =>
        {
            if (error != null)
            {
                //failure to load!, do something!
                Debug.LogWarning("[AdsManager_Rewarded] Ads failed: " + error);
                StartCoroutine(ReloadAds("Rewarded"));
                return;
            }
            //success load
            rewardedAd = ad;
            rewardedLoadTime = DateTime.Now;
            rewardedAd.OnAdFullScreenContentClosed += () =>
            {
                RunOnMainThread(() =>
                {
                    isShowingAds = false;
                    ResumeGame();

                    rewardedAd.Destroy();
                    rewardedAd = null;

                    LoadRewarded();
                });
            };
            rewardedAd.OnAdFullScreenContentFailed += (error) =>
            {
                isShowingAds = false;
                ResumeGame();
                Debug.Log("[AdsManager_Rewarded] Ads failed to show : " + error);
                rewardedAd.Destroy();
                LoadRewarded();
            };
        });
    }

    void LoadAppOpen()
    {
        appOpenAd = null;
        var request = new AdRequest();
        AppOpenAd.Load(appOpenAdsUnit, request, (AppOpenAd ad, LoadAdError error) =>
        {
            if (error != null || ad == null)
            {
                Debug.LogWarning("[AdsManager_AppOpen] Ads failed: " + error);
                StartCoroutine(ReloadAds("AppOpen"));
                return;
            }
            appOpenAd = ad;
            appOpenLoadTime = DateTime.Now;
            appOpenAd.OnAdFullScreenContentClosed += () =>
            {
                RunOnMainThread(() =>
                {
                    isShowingAds = false;
                    ResumeGame();
                    appOpenAd.Destroy();
                    LoadAppOpen();
                });
            };
            appOpenAd.OnAdFullScreenContentFailed += (error) =>
            {
                isShowingAds = false;
                ResumeGame();
                Debug.Log("[AdsManager_AppOpen] Ads failed to show : " + error);
                appOpenAd.Destroy();
                LoadAppOpen();
            };
        });
    }


    // [ SUPPORT SEGMENT ]
    // ads support tools, only for helping
    bool IsExpired(DateTime loadTime)
    {
        return DateTime.Now - loadTime > adExpiryTime;
    }

    bool IsAppOpenExpired()
    {
        return DateTime.Now - appOpenLoadTime > appOpenAdExpiryTime;
    }

    void PauseGame()
    {
        print("[AdsManager_General] Game paused.");
        HideAllBanner();
        interfaceBlocker.SetActive(true);
        AudioListener.pause = true;
        Time.timeScale = 0f;
    }

    void ResumeGame()
    {
        print("[AdsManager_General] Game resumed.");
        Time.timeScale = 1f;
        interfaceBlocker.SetActive(false);
        AudioListener.pause = false;
        ShowAllBanner();
    }

    IEnumerator ReloadAds(string adType)
    {
        yield return new WaitForSeconds(5);
        switch (adType)
        {
            case "AppOpen":
                LoadAppOpen();
                break;
            case "Banner":
                print("Banner is not required to 'reload'. ShowAllBanner() is used.");
                break;
            case "Rewarded":
                LoadRewarded();
                break;
            case "Interstitial":
                LoadInterstitial();
                break;
            default:
                print("[AdsNanager_ReloadAds] " + adType + " doesn't exist.");
                break;
        }
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }
    IEnumerator Start()
    {
        DontDestroyOnLoad(gameObject);

        // Initialize
        MobileAds.Initialize(_ => { });

        // Wait one frame or a small delay
        yield return new WaitForSeconds(0.5f);

        LoadInterstitial();
        LoadRewarded();
        LoadAppOpen();

        PlayerPrefs.SetInt("_TimesPlayedAppOpen", PlayerPrefs.GetInt("_TimesPlayedAppOpen", 0) + 1); // track beberapa kali pemain mulai game

        if (automaticallyShowBanner && !disableForcedAds)
            ShowAllBanner();
        if (automaticallyShowAppOpen && !disableForcedAds && PlayerPrefs.GetInt("_TimesPlayedAppOpen", 0) % timesBeforeAppOpenShow == 0)
            ShowAppOpen();
        disableForcedAds = PlayerPrefs.GetInt("disableAds", 0) == 1 ? true : false;
    }

    private void Update()
    {
        if (rewardedAd != null && IsExpired(rewardedLoadTime))
        {
            Debug.Log("[AdsManager_Rewarded] Ad expired, reloading...");
            rewardedAd.Destroy();
            LoadRewarded();
        }
        if (interstitialAd != null && IsExpired(interstitialLoadTime))
        {
            Debug.Log("[AdsManager_Interstitial] Ad expired, reloading...");
            interstitialAd.Destroy();
            LoadInterstitial();
        }
        if (appOpenAd != null && IsAppOpenExpired())
        {
            Debug.Log("[AdsManager_AppOpen] Ad expired, reloading...");
            appOpenAd.Destroy();
            LoadAppOpen();
        }
    }

    void RunOnMainThread(Action action)
    {
        StartCoroutine(RunNextFrame(action));
    }

    IEnumerator RunNextFrame(Action action)
    {
        yield return null;                 // wait 1 frame
        yield return new WaitForEndOfFrame(); // graphics restored
        action?.Invoke();
    }

}

[System.Serializable]
public class BannerConfig
{
    public string bannerAdsUnit = "ca-app-pub-3940256099942544/9214589741"; // test id
    public AdPosition bannerPosition = AdPosition.Top;
    public bool useCustomSize = false;
    public Vector2 customSize = new Vector2(0, 0);
    public bool useCustomPosition = false;
    public Vector2 customPosition = new Vector2(0, 0);
    public BannerView currentBanner;
}
