# 🎮 Unity Ads & Device ID Manager

This repository contains scripts for handling **Rewarded Interstitial Ads** and managing **Device IDs** in Unity projects. It includes a full ad workflow and device ID utilities compatible with Android, iOS, PC, and Unity Editor.

---

## 📂 Files in this Repository

1. **RewardedGoldAd.cs**
   - Handles Rewarded Interstitial Ads (users watch ads to receive gold).
   - Implements a **Singleton** pattern for global access.
   - Features ad loading, showing, callbacks, and automatic reload.

2. **DeviceIDDisplay.cs**
   - Displays the device ID on a TextMeshPro UI element.
   - Logs the device ID to the console.
   - Automatically adapts to Android, iOS, PC, and Unity Editor.

3. **DeviceIDManager.cs**
   - Provides a persistent, globally accessible **Device ID**.
   - Handles platform-specific ID retrieval and error handling.
   - Useful for AdMob test devices, analytics, and other services.

---

## 📦 Required Packages

To run the ads functionality, import these Unity packages:

1. **Google Mobile Ads SDK (v10.4.2)**  
   🔗 [Download](https://www.mediafire.com/file/qgrck8dyaq3qdsr/GoogleMobileAds-v10.4.2.unitypackage/file)

2. **External Dependency Manager (EDM4U - latest)**  
   🔗 [Download](https://www.mediafire.com/file/h8mv9o6ot65mfy5/external-dependency-manager-latest.unitypackage/file)

> After importing, go to `Assets → External Dependency Manager → Android Resolver → Force Resolve` to ensure all dependencies are set.

---

## 🛠️ Setup & Usage

### 1. RewardedGoldAd

- Attach `RewardedGoldAd.cs` to a GameObject in your scene (e.g., `AdManager`).
- Assign **UI References**: `goldText` (TextMeshPro) and `watchAdButton` (Button).
- The script automatically initializes Google Mobile Ads SDK and loads rewarded interstitial ads.
- Watching an ad will reward the player with **100 gold**.

**Example Usage:**
```csharp
void Start()
{
    // Show ad when button clicked
    watchAdButton.onClick.AddListener(RewardedGoldAd.Instance.ShowRewardedInterstitialAd);
}
```

### 2. DeviceIDDisplay

- Attach `DeviceIDDisplay.cs` to a UI GameObject.
- Assign a **TextMeshProUGUI** reference to `deviceIDText`.
- Displays the device's unique ID on screen and logs it.

### 3. DeviceIDManager

- Attach `DeviceIDManager.cs` to a persistent GameObject (like a manager in your scene).
- Access the device ID anywhere in your project:
```csharp
string deviceID = FindObjectOfType<DeviceIDManager>().deviceID;
Debug.Log("Device ID: " + deviceID);
```

- Works on **Android, iOS, PC, and Editor**.
- On Android, it retrieves `android_id`. On Editor, it returns `UNITY_EDITOR_TEST_ID`.

---

## 📋 Notes

- Add your **device ID** to test devices in AdMob:
```csharp
List<string> testDevices = new List<string> { myDeviceID };
RequestConfiguration config = new RequestConfiguration { TestDeviceIds = testDevices };
MobileAds.SetRequestConfiguration(config);
```
- All ad events are logged, including impressions, clicks, and rewards.
- Singleton patterns ensure only one instance exists for ad management.

---

## ✅ License / Disclaimer
- These scripts are free to use and modify for Unity projects.
- Use responsibly and respect user privacy when handling device identifiers.
- Not intended for production without replacing **test ad unit IDs** with your own.

---

*Created for Unity projects integrating Google Mobile Ads and device ID management.*

