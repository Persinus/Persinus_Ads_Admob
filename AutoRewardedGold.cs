using UnityEngine;
using UnityEngine.UI;
using GoogleMobileAds.Api;
using TMPro;
using System.Collections.Generic;

/// <summary>
/// Quản lý Rewarded Interstitial Ad (xem quảng cáo nhận vàng).
/// Sử dụng Singleton để có thể gọi ở bất kỳ đâu trong game.
/// </summary>
public class RewardedGoldAd : MonoBehaviour
{
    // ---------- Singleton ---------- //
    public static RewardedGoldAd Instance { get; private set; }

    // ---------- UI References ---------- //
    [Header("UI")]
    public TextMeshProUGUI goldText;   // Hiển thị số vàng
    public Button watchAdButton;       // Nút bấm để xem quảng cáo

    // ---------- Gameplay Data ---------- //
    private int gold = 0;              
    private RewardedInterstitialAd rewardedInterstitialAd;

    // ---------- Ad Unit ID ---------- //
#if UNITY_ANDROID
    private string _adUnitId = "ca-app-pub-3940256099942544/5354046379"; // Test ID Android
#elif UNITY_IPHONE
    private string _adUnitId = "ca-app-pub-3940256099942544/6978759866"; // Test ID iOS
#else
    private string _adUnitId = "unused";
#endif

    // ---------- Unity Lifecycle ---------- //
    private void Awake()
    {
        // Setup Singleton
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject); // Giữ lại khi đổi scene
    }

    private void Start()
    {
        gold = 0;
        UpdateGoldText();

        if (watchAdButton != null)
            watchAdButton.onClick.AddListener(ShowRewardedInterstitialAd);

        // Thêm device ID cố định vào danh sách test devices
        List<string> testDeviceIds = new List<string> { "1c97557bda7ac517" };
        RequestConfiguration requestConfiguration = new RequestConfiguration
        {
            TestDeviceIds = testDeviceIds
        };
        MobileAds.SetRequestConfiguration(requestConfiguration);

        // Khởi tạo SDK
        MobileAds.Initialize((initStatus) =>
        {
            Debug.Log("Google Mobile Ads SDK initialized.");
            LoadRewardedInterstitialAd();
        });
    }

    private void OnDestroy()
    {
        DestroyAd();
    }

    // ---------- UI Update ---------- //
    /// <summary>
    /// Cập nhật số vàng hiển thị trên UI.
    /// </summary>
    private void UpdateGoldText()
    {
        if (goldText != null)
            goldText.text = "Gold: " + gold;
    }

    // ---------- Ad Logic ---------- //
    /// <summary>
    /// Load Rewarded Interstitial Ad mới.
    /// </summary>
    public void LoadRewardedInterstitialAd()
    {
        if (rewardedInterstitialAd != null)
        {
            rewardedInterstitialAd.Destroy();
            rewardedInterstitialAd = null;
        }

        AdRequest adRequest = new AdRequest();

        RewardedInterstitialAd.Load(_adUnitId, adRequest, (RewardedInterstitialAd ad, LoadAdError error) =>
        {
            if (error != null || ad == null)
            {
                Debug.LogError("Rewarded interstitial ad failed to load: " + error);
                return;
            }

            Debug.Log("Rewarded interstitial ad loaded successfully.");
            rewardedInterstitialAd = ad;
            RegisterEventHandlers(rewardedInterstitialAd);
        });
    }

    /// <summary>
    /// Hiển thị quảng cáo và cộng vàng khi người chơi nhận thưởng.
    /// </summary>
    public void ShowRewardedInterstitialAd()
    {
        if (rewardedInterstitialAd != null && rewardedInterstitialAd.CanShowAd())
        {
            rewardedInterstitialAd.Show((Reward reward) =>
            {
                gold += 100; // Cộng vàng khi xem xong
                UpdateGoldText();
                Debug.Log("Reward granted: +100 gold");
            });
        }
        else
        {
            Debug.Log("Rewarded interstitial ad is not ready.");
        }
    }

    /// <summary>
    /// Gắn các sự kiện callback cho quảng cáo.
    /// </summary>
    private void RegisterEventHandlers(RewardedInterstitialAd ad)
    {
        ad.OnAdPaid += (AdValue adValue) =>
        {
            Debug.Log($"Ad paid {adValue.Value} {adValue.CurrencyCode}");
        };
        ad.OnAdImpressionRecorded += () => Debug.Log("Ad impression recorded.");
        ad.OnAdClicked += () => Debug.Log("Ad clicked.");
        ad.OnAdFullScreenContentOpened += () => Debug.Log("Ad full screen opened.");
        ad.OnAdFullScreenContentClosed += () =>
        {
            Debug.Log("Ad closed. Reloading...");
            LoadRewardedInterstitialAd();
        };
        ad.OnAdFullScreenContentFailed += (AdError error) =>
        {
            Debug.LogError("Ad failed to open full screen: " + error);
            LoadRewardedInterstitialAd();
        };
    }

    /// <summary>
    /// Giải phóng bộ nhớ quảng cáo khi không dùng nữa.
    /// </summary>
    private void DestroyAd()
    {
        if (rewardedInterstitialAd != null)
        {
            rewardedInterstitialAd.Destroy();
            rewardedInterstitialAd = null;
        }
    }
}
