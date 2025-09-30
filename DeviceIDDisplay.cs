using UnityEngine;
using TMPro; // TextMeshPro namespace

/// <summary>
/// Hiển thị Device ID của thiết bị lên UI TextMeshPro và log ra console.
/// </summary>
public class DeviceIDDisplay : MonoBehaviour
{
    // ---- Inspector References ---- //
    [Tooltip("Text UI để hiển thị Device ID")]
    public TextMeshProUGUI deviceIDText; 

    // ---- Unity Events ---- //
    /// <summary>
    /// Khi script bắt đầu chạy, lấy Device ID và gán lên UI.
    /// </summary>
    private void Start()
    {
        string deviceID = GetDeviceID();
        
        if (deviceIDText != null)
            deviceIDText.text = "Device ID: " + deviceID;

        Debug.Log("Device ID: " + deviceID);
    }

    // ---- Helper Methods ---- //
    /// <summary>
    /// Lấy Device ID phù hợp theo nền tảng (Android, Editor, Others).
    /// </summary>
    /// <returns>Chuỗi Device ID</returns>
    private string GetDeviceID()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        try
        {
            // ---- Lấy Android ID qua AndroidJavaObject ---- //
            using (var unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
            using (var currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity"))
            using (var contentResolver = currentActivity.Call<AndroidJavaObject>("getContentResolver"))
            using (var secure = new AndroidJavaClass("android.provider.Settings$Secure"))
            {
                string androidId = secure.CallStatic<string>("getString", contentResolver, "android_id");
                return androidId;
            }
        }
        catch
        {
            return "ERROR_ANDROID_ID";
        }
#elif UNITY_EDITOR
        // ---- Trường hợp test trong Unity Editor ---- //
        return "UNITY_EDITOR_TEST_ID";
#else
        // ---- Các nền tảng khác (iOS, PC, ...) ---- //
        return SystemInfo.deviceUniqueIdentifier;
#endif
    }
}