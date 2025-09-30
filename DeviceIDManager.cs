using UnityEngine;

/// <summary>
/// Quản lý và cung cấp Device ID của thiết bị.
/// Device ID sẽ được lấy khi Awake() và lưu trữ để dùng trong toàn bộ ứng dụng.
/// </summary>
public class DeviceIDManager : MonoBehaviour
{
    // ---- Public Properties ---- //
    [Tooltip("Device ID sẽ được gán tự động khi Awake()")]
    public string deviceID;

    // ---- Unity Events ---- //
    /// <summary>
    /// Awake chạy trước Start.
    /// Dùng để khởi tạo và lấy Device ID ngay khi script được load.
    /// </summary>
    private void Awake()
    {
        deviceID = GetDeviceID();
        Debug.Log("DEVICE_ID: " + deviceID);
    }

    // ---- Helper Methods ---- //
    /// <summary>
    /// Lấy Device ID phù hợp theo nền tảng (Android, Editor, Others).
    /// </summary>
    /// <returns>Chuỗi Device ID duy nhất</returns>
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
        catch (System.Exception e)
        {
            Debug.LogError("Error getting Android ID: " + e);
            return "UNKNOWN_ANDROID_ID";
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
