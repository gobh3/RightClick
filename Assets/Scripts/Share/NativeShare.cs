using UnityEngine;
using System.Runtime.InteropServices;

public class NativeShare : MonoBehaviour
{
    // Call this to open share menu with simple text
    public void Share(string message)
    {
#if UNITY_ANDROID
        ShareAndroid(message);
#elif UNITY_IOS
        ShareIOS(message);
#else
        Debug.Log("Sharing not supported on this platform.");
#endif
    }

#if UNITY_ANDROID
    private void ShareAndroid(string message)
    {
        try
        {
            AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject activity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");

            AndroidJavaObject intent = new AndroidJavaObject("android.content.Intent");
            intent.Call<AndroidJavaObject>("setAction", "android.intent.action.SEND");
            intent.Call<AndroidJavaObject>("setType", "text/plain");
            intent.Call<AndroidJavaObject>("putExtra", "android.intent.extra.TEXT", message);

            AndroidJavaObject chooser = intent.CallStatic<AndroidJavaObject>("createChooser", intent, "Share via");
            activity.Call("startActivity", chooser);
        }
        catch (System.Exception e)
        {
            Debug.LogError("Android share failed: " + e.Message);
        }
    }
#endif

#if UNITY_IOS
    [DllImport("__Internal")]
    private static extern void ShowShareSheet(string message);

    private void ShareIOS(string message)
    {
        ShowShareSheet(message);
    }
#endif
}
